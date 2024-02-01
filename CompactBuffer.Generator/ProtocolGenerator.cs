
using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public interface IAddAddtionType
    {
        void AddAdditionType(Type type);
    }

    public class ProtocolGenerator : Generator
    {
        private readonly IAddAddtionType m_AddAdditionType;
        private readonly HashSet<Assembly> m_Assemblies = new HashSet<Assembly>();

        public ProtocolGenerator(IAddAddtionType addAdditionType)
        {
            m_AddAdditionType = addAdditionType;
        }

        public void AddAssembly(Assembly assembly)
        {
            m_Assemblies.Add(assembly);
        }

        public string GenCode()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"// Generate by CompactBuffer.CodeGen");
            builder.AppendLine();
            GenCode(builder);
            return builder.ToString();
        }

        public void GenCode(StringBuilder builder)
        {
            builder.AppendLine($"#if !PROTOCOL_CLIENT && !PROTOCOL_SERVER");
            builder.AppendLine($"#define PROTOCOL_CLIENT");
            builder.AppendLine($"#define PROTOCOL_SERVER");
            builder.AppendLine($"#endif");
            builder.AppendLine();

            foreach (var assembly in m_Assemblies)
            {
                GenCode(builder, assembly);
            }
        }

        private void GenCode(StringBuilder builder, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsInterface) continue;
                if (!typeof(IProtocol).IsAssignableFrom(type)) continue;

                var customSerializer = type.GetCustomAttribute<ProtocolAttribute>();
                if (customSerializer == null) continue;

                GenCode(builder, type);
            }
        }

        private void GenCode(StringBuilder builder, Type type)
        {
            var methods = new List<MethodInfo>();
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.IsGenericMethod)
                {
                    throw new CompactBufferExeption($"{type.FullName}.{method.Name} cannot Generic Method");
                }
                if (method.ReturnType != typeof(void))
                {
                    throw new CompactBufferExeption($"{type.FullName}.{method.Name} must return void");
                }

                foreach (var p in method.GetParameters())
                {
                    if (!m_Assemblies.Contains(p.ParameterType.Assembly)) continue;

                    var customSerializer = p.GetCustomAttribute<CustomSerializerAttribute>();
                    if (customSerializer != null) continue;

                    var originType = p.ParameterType;
                    if (originType.IsByRef) originType = originType.GetElementType();

                    if (originType.IsArray)
                    {
                        var elementType = originType.GetElementType();
                        if (m_Assemblies.Contains(elementType.Assembly)) m_AddAdditionType.AddAdditionType(elementType);
                        continue;
                    }

                    if (originType.IsGenericType)
                    {
                        if (originType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            var args = originType.GetGenericArguments();
                            if (m_Assemblies.Contains(args[0].Assembly)) m_AddAdditionType?.AddAdditionType(args[0]);
                            continue;
                        }
                        if (p.ParameterType.GetGenericTypeDefinition() == typeof(HashSet<>))
                        {
                            var args = originType.GetGenericArguments();
                            if (m_Assemblies.Contains(args[0].Assembly)) m_AddAdditionType?.AddAdditionType(args[0]);
                            continue;
                        }
                        if (originType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                        {
                            var args = originType.GetGenericArguments();
                            if (m_Assemblies.Contains(args[0].Assembly)) m_AddAdditionType?.AddAdditionType(args[0]);
                            if (m_Assemblies.Contains(args[1].Assembly)) m_AddAdditionType?.AddAdditionType(args[1]);
                            continue;
                        }
                    }

                    m_AddAdditionType?.AddAdditionType(originType);
                }

                methods.Add(method);
            }

            GenProxy(builder, type, methods);
            GenStub(builder, type, methods);
            if (type.GetCustomAttribute<ProtocolAttribute>().Dispatch)
            {
                GenDispach(builder, type, methods);
            }
        }

        private void GenProxy(StringBuilder builder, Type type, List<MethodInfo> methods)
        {
            var protocol = type.GetCustomAttribute<ProtocolAttribute>();
            if (protocol.ProtocolType == ProtocolType.Client)
            {
                builder.AppendLine($"#if PROTOCOL_SERVER");
            }
            if (protocol.ProtocolType == ProtocolType.Server)
            {
                builder.AppendLine($"#if PROTOCOL_CLIENT");
            }

            builder.AppendLine($"namespace ProtocolAutoGen");
            builder.AppendLine($"{{");
            builder.AppendLine($"    [CompactBuffer.ProtocolProxy(typeof({type.FullName}))]");
            builder.AppendLine($"    public class {type.FullName.Replace(".", "_")}_Proxy : CompactBuffer.ProtocolProxy, {type.FullName}");
            builder.AppendLine($"    {{");
            builder.AppendLine($"        public {type.FullName.Replace(".", "_")}_Proxy(CompactBuffer.IProtocolSender sender) : base(sender)");
            builder.AppendLine($"        {{");
            builder.AppendLine($"        }}");
            for (var i = 0; i < methods.Count; i++)
            {
                builder.AppendLine($"");
                var method = methods[i];
                var paramsText = string.Join(", ", Array.ConvertAll(method.GetParameters(), (x) =>
                {
                    return $"{GetDeclarePrefix(x)}{GetTypeName(x.ParameterType)} ___{x.Name}";
                }));
                builder.AppendLine($"        void {type.FullName}.{method.Name}({paramsText})");
                builder.AppendLine($"        {{");
                builder.AppendLine($"            var writer = m_Sender.GetStreamWriter();");
                builder.AppendLine($"            writer.WriteVariantInt32({i});");
                foreach (var param in method.GetParameters())
                {
                    GenProxyField(builder, type, method, param);
                }
                builder.AppendLine($"            m_Sender.Send(writer);");
                builder.AppendLine($"        }}");
            }
            builder.AppendLine($"    }}");
            builder.AppendLine($"}}");

            if (protocol.ProtocolType == ProtocolType.Client || protocol.ProtocolType == ProtocolType.Server)
            {
                builder.AppendLine($"#endif");
            }
        }

        private void GenProxyField(StringBuilder builder, Type type, MethodInfo method, ParameterInfo param)
        {
            var customSerializer = param.ParameterType.GetCustomAttribute<CustomSerializerAttribute>();
            if (customSerializer == null && param.ParameterType.IsEnum)
            {
                builder.AppendLine($"            writer.WriteVariantInt32((int)___{param.Name});");
            }
            else if (customSerializer == null && IsBaseType(param.ParameterType))
            {
                var float16 = param.GetCustomAttribute<Float16Attribute>();
                if (float16 == null)
                {
                    float16 = method.GetCustomAttribute<Float16Attribute>();
                    if (float16 == null)
                    {
                        float16 = type.GetCustomAttribute<Float16Attribute>();
                    }
                }

                if (param.GetCustomAttribute<VariantIntAttribute>() != null && IsVariantable(param.ParameterType))
                {
                    builder.AppendLine($"            writer.WriteVariant{param.ParameterType.Name}(___{param.Name});");
                }
                else if (float16 != null && param.ParameterType == typeof(float))
                {
                    builder.AppendLine($"            writer.WriteFloat16(___{param.Name}, {float16.IntegerMax});");
                }
                else
                {
                    builder.AppendLine($"            writer.Write(___{param.Name});");
                }
            }
            else
            {
                builder.AppendLine($"            {GetSerializerName(param)}.Write(writer, in ___{param.Name});");
            }
        }

        private void GenStub(StringBuilder builder, Type type, List<MethodInfo> methods)
        {
            var protocol = type.GetCustomAttribute<ProtocolAttribute>();
            if (protocol.ProtocolType == ProtocolType.Client)
            {
                builder.AppendLine($"#if PROTOCOL_CLIENT");
            }
            if (protocol.ProtocolType == ProtocolType.Server)
            {
                builder.AppendLine($"#if PROTOCOL_SERVER");
            }

            builder.AppendLine($"namespace ProtocolAutoGen");
            builder.AppendLine($"{{");
            builder.AppendLine($"    [CompactBuffer.ProtocolStub(typeof({type.FullName}))]");
            builder.AppendLine($"    public class {type.FullName.Replace(".", "_")}_Stub : CompactBuffer.IProtocolStub");
            builder.AppendLine($"    {{");
            builder.AppendLine($"        protected readonly {type.FullName} m_Target;");
            builder.AppendLine($"");
            builder.AppendLine($"        public {type.FullName.Replace(".", "_")}_Stub({type.FullName} target = null)");
            builder.AppendLine($"        {{");
            builder.AppendLine($"            m_Target = target;");
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        void Dispatch(CompactBuffer.BufferReader reader)");
            builder.AppendLine($"        {{");
            builder.AppendLine($"            var index = reader.ReadVariantInt32();");
            for (var i = 0; i < methods.Count; i++)
            {
                var method = methods[i];
                builder.AppendLine($"            if (index == {i})");
                builder.AppendLine($"            {{");
                foreach (var param in method.GetParameters())
                {
                    GenStubField(builder, type, method, param);
                }
                var paramsText = string.Join(", ", Array.ConvertAll(method.GetParameters(), (x) =>
                {
                    if (x.IsIn)
                    {
                        return $"in ___{x.Name}";
                    }
                    else if (x.ParameterType.IsByRef)
                    {
                        return $"ref ___{x.Name}";
                    }
                    else
                    {
                        return $"___{x.Name}";
                    }
                }));
                builder.AppendLine($"                m_Target?.{method.Name}({paramsText});");
                builder.AppendLine($"                return;");
                builder.AppendLine($"            }}");
            }
            builder.AppendLine($"            throw new CompactBuffer.CompactBufferExeption(\"{type.FullName} invalid method index\" + index);");
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        void CompactBuffer.IProtocolStub.Dispatch(CompactBuffer.BufferReader reader)");
            builder.AppendLine($"        {{");
            builder.AppendLine($"            var top = CompactBuffer.Internal.SpanAllocator.Begin();");
            builder.AppendLine($"            try");
            builder.AppendLine($"            {{");
            builder.AppendLine($"                Dispatch(reader);");
            builder.AppendLine($"            }}");
            builder.AppendLine($"            finally");
            builder.AppendLine($"            {{");
            builder.AppendLine($"                CompactBuffer.Internal.SpanAllocator.End(top);");
            builder.AppendLine($"            }}");
            builder.AppendLine($"        }}");
            builder.AppendLine($"    }}");
            builder.AppendLine($"}}");

            if (protocol.ProtocolType == ProtocolType.Client || protocol.ProtocolType == ProtocolType.Server)
            {
                builder.AppendLine($"#endif");
            }
        }

        private void GenStubField(StringBuilder builder, Type type, MethodInfo method, ParameterInfo param)
        {
            var customSerializer = param.ParameterType.GetCustomAttribute<CustomSerializerAttribute>();
            if (customSerializer == null && param.ParameterType.IsEnum)
            {
                builder.AppendLine($"                var ___{param.Name} = ({GetTypeName(param.ParameterType)})reader.ReadVariantInt32();");
            }
            else if (customSerializer == null && IsBaseType(param.ParameterType))
            {
                var float16 = param.GetCustomAttribute<Float16Attribute>();
                if (float16 == null)
                {
                    float16 = method.GetCustomAttribute<Float16Attribute>();
                    if (float16 == null)
                    {
                        float16 = type.GetCustomAttribute<Float16Attribute>();
                    }
                }

                if (param.GetCustomAttribute<VariantIntAttribute>() != null && IsVariantable(param.ParameterType))
                {
                    builder.AppendLine($"                var ___{param.Name} = reader.ReadVariant{param.ParameterType.Name}();");
                }
                else if (float16 != null && param.ParameterType == typeof(float))
                {
                    builder.AppendLine($"                var ___{param.Name} = reader.ReadFloat16({float16.IntegerMax});");
                }
                else
                {
                    builder.AppendLine($"                var ___{param.Name} = reader.Read{param.ParameterType.Name}();");
                }
            }
            else
            {
                builder.AppendLine($"                {GetTypeName(param.ParameterType)} ___{param.Name} = default;");
                builder.AppendLine($"                {GetSerializerName(param)}.Read(reader, ref ___{param.Name});");
            }
        }

        private void GenDispach(StringBuilder builder, Type type, List<MethodInfo> methods)
        {
        }

        private string GetDeclarePrefix(ParameterInfo param)
        {
            var isRef = param.ParameterType.IsByRef;
            var isIn = param.IsIn;
            if (isRef && isIn) return "ref readonly ";
            if (isRef) return "ref ";
            if (isIn) return "in ";
            return "";
        }


        private string GetSerializerName(ParameterInfo field)
        {
            var originType = field.ParameterType;
            if (originType.IsByRef) originType = originType.GetElementType();

            var customSerializer = field.GetCustomAttribute<CustomSerializerAttribute>();
            if (customSerializer != null)
            {
                var type = customSerializer.SerializerType;
                return GetTypeName(type);
            }
            else
            {
                var serializer = CompactBuffer.GetSerializer(originType);
                if (serializer != null)
                {
                    return serializer.GetType().FullName;
                }
            }

            if (customSerializer != null)
            {
                return $"CompactBuffer.CompactBuffer.GetCustomSerializer<{customSerializer.SerializerType.FullName}, {originType.FullName}>()";
            }

            if (originType.IsArray)
            {
                return $"CompactBuffer.Internal.ArraySerializer<{GetTypeName(originType.GetElementType())}>";
            }

            if (originType.IsGenericType)
            {
                if(originType.GetGenericTypeDefinition()==typeof(Span<>))
                {
                    return $"CompactBuffer.Internal.SpanSerializer<{GetTypeName(originType.GetGenericArguments()[0])}>";
                }
                if(originType.GetGenericTypeDefinition()==typeof(ReadOnlySpan<>))
                {
                    return $"CompactBuffer.Internal.ReadOnlySpanSerializer<{GetTypeName(originType.GetGenericArguments()[0])}>";
                }
                if (originType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return $"CompactBuffer.Internal.ListSerializer<{GetTypeName(originType.GetGenericArguments()[0])}>";
                }
                if (originType.GetGenericTypeDefinition() == typeof(HashSet<>))
                {
                    return $"CompactBuffer.Internal.HashSetSerializer<{GetTypeName(originType.GetGenericArguments()[0])}>";
                }
                if (originType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return $"CompactBuffer.Internal.DictionarySerializer<{GetTypeName(originType.GetGenericArguments()[0])}, {GetTypeName(originType.GetGenericArguments()[1])}>";
                }
                throw new CompactBufferExeption($"{GetTypeName(originType)} unsupport generic type");
            }

            return $"CompactBufferAutoGen.{originType.FullName.Replace(".", "_")}_Serializer";
        }
    }
}
