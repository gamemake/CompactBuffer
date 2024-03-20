
using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public interface IAddAddtionType
    {
        bool AddAdditionType(Type type);
    }

    public class ProtocolGenerator : Generator
    {
        private readonly IAddAddtionType m_AddAdditionType;

        public ProtocolGenerator(IAddAddtionType addAdditionType)
        {
            m_AddAdditionType = addAdditionType;
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

                var customSerializer = type.GetCustomAttribute<ProtocolIdAttribute>();
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
                    throw new CompactBufferException($"{type.FullName}.{method.Name} cannot Generic Method");
                }
                if (method.ReturnType != typeof(void))
                {
                    throw new CompactBufferException($"{type.FullName}.{method.Name} must return void");
                }

                foreach (var p in method.GetParameters())
                {
                    if (!m_Assemblies.Contains(p.ParameterType.Assembly)) continue;

                    var customSerializer = p.GetCustomAttribute<OverwriteAttribute>();
                    if (customSerializer != null) continue;


                    if (!m_AddAdditionType.AddAdditionType(p.ParameterType))
                    {
                        throw new CompactBufferException($"{type.FullName}.{method.Name} parameter {p.Name} unsupport type {GetTypeName(p.ParameterType)}");
                    }
                }

                methods.Add(method);
            }

            GenProxy(builder, type, methods);
            GenStub(builder, type, methods);
            if (type.GetCustomAttribute<ProtocolIdAttribute>().Dispatch)
            {
                GenDispach(builder, type, methods);
            }
        }

        private void GenProxy(StringBuilder builder, Type type, List<MethodInfo> methods)
        {
            var protocol = type.GetCustomAttribute<ProtocolIdAttribute>();
            if (protocol.ProtocolType == ProtocolType.Client)
            {
                builder.AppendLine($"#if PROTOCOL_SERVER");
            }
            if (protocol.ProtocolType == ProtocolType.Server)
            {
                builder.AppendLine($"#if PROTOCOL_CLIENT");
            }

            byte defaultChannel = byte.MaxValue;
            var interfaceChannel = type.GetCustomAttribute<ChannelAttribute>();
            if (interfaceChannel != null)
            {
                defaultChannel = interfaceChannel.Channel;
            }

            builder.AppendLine($"namespace ProtocolAutoGen");
            builder.AppendLine($"{{");
            builder.AppendLine($"    [CompactBuffer.Protocol(typeof({type.FullName}))]");
            builder.AppendLine($"    public class {type.FullName.Replace(".", "_")}_Proxy : CompactBuffer.ProtocolProxy, {type.FullName}");
            builder.AppendLine($"    {{");
            builder.AppendLine($"        public {type.FullName.Replace(".", "_")}_Proxy(CompactBuffer.IProtocolSender sender) : base(sender)");
            builder.AppendLine($"        {{");
            builder.AppendLine($"        }}");
            for (var i = 0; i < methods.Count; i++)
            {
                builder.AppendLine($"");

                var method = methods[i];
                var channel = defaultChannel;
                var methodChannel = method.GetCustomAttribute<ChannelAttribute>();
                if(methodChannel!=null)
                {
                    channel = methodChannel.Channel;
                }

                var paramsText = string.Join(", ", Array.ConvertAll(method.GetParameters(), (x) =>
                {
                    return $"{GetDeclarePrefix(x)}{GetTypeName(x.ParameterType)} ___{x.Name}";
                }));
                builder.AppendLine($"        void {type.FullName}.{method.Name}({paramsText})");
                builder.AppendLine($"        {{");
                builder.AppendLine($"            var writer = m_Sender.GetStreamWriter({type.GetCustomAttribute<ProtocolIdAttribute>().ProtocolId});");
                builder.AppendLine($"            writer.Write7BitEncodedInt32({i});");
                foreach (var param in method.GetParameters())
                {
                    GenProxyParameter(builder, type, method, param);
                }
                builder.AppendLine($"            m_Sender.Send(writer, {channel});");
                builder.AppendLine($"        }}");
            }
            builder.AppendLine($"    }}");
            builder.AppendLine($"}}");

            if (protocol.ProtocolType == ProtocolType.Client || protocol.ProtocolType == ProtocolType.Server)
            {
                builder.AppendLine($"#endif");
            }
        }

        private void GenProxyParameter(StringBuilder builder, Type type, MethodInfo method, ParameterInfo param)
        {
            var customSerializer = param.GetCustomAttribute<OverwriteAttribute>();
            var float16 = param.GetCustomAttribute<Float16Attribute>();
            if (float16 == null)
            {
                float16 = type.GetCustomAttribute<Float16Attribute>();
            }
            var sevenBitEncoded = param.GetCustomAttribute<SevenBitEncodedIntAttribute>();
            var originType = param.ParameterType;

            if (customSerializer != null)
            {
                builder.AppendLine($"            {GetTypeName(customSerializer.SerializerType)}.Write(write, in ___{param.Name});");
            }
            else if (originType.IsEnum)
            {
                builder.AppendLine($"            writer.Write7BitEncodedInt32((int)___{param.Name});");
            }
            else if (originType == typeof(float) && float16 != null)
            {
                builder.AppendLine($"            writer.WriteFloat16(___{param.Name}, {float16.IntegerMax});");
            }
            else if (Is7BitEncoded(originType) && sevenBitEncoded != null)
            {
                builder.AppendLine($"            writer.Write7BitEncoded{originType.Name}(___{param.Name});");
            }
            else if (IsBaseType(originType))
            {
                builder.AppendLine($"            writer.Write(___{param.Name});");
            }
            else
            {
                builder.AppendLine($"            {GetSerializerName(originType)}.Write(writer, in ___{param.Name});");
            }
        }

        private void GenStub(StringBuilder builder, Type type, List<MethodInfo> methods)
        {
            var protocol = type.GetCustomAttribute<ProtocolIdAttribute>();
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
            builder.AppendLine($"    [CompactBuffer.Protocol(typeof({type.FullName}))]");
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
            builder.AppendLine($"            var index = reader.Read7BitEncodedInt32();");
            for (var i = 0; i < methods.Count; i++)
            {
                var method = methods[i];
                builder.AppendLine($"            if (index == {i})");
                builder.AppendLine($"            {{");
                foreach (var param in method.GetParameters())
                {
                    GenStubParameter(builder, type, method, param);
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

        private void GenStubParameter(StringBuilder builder, Type type, MethodInfo method, ParameterInfo param)
        {
            var customSerializer = param.GetCustomAttribute<OverwriteAttribute>();
            var float16 = param.GetCustomAttribute<Float16Attribute>();
            if (float16 == null)
            {
                float16 = type.GetCustomAttribute<Float16Attribute>();
            }
            var sevenBitEncoded = param.GetCustomAttribute<SevenBitEncodedIntAttribute>();
            var originType = param.ParameterType;
            if (originType.IsByRef) originType = originType.GetElementType();

            if (customSerializer != null)
            {
                builder.AppendLine($"                {GetTypeName(originType)} ___{param.Name} = default;");
                builder.AppendLine($"            {GetTypeName(customSerializer.SerializerType)}.Read(reader, ref ___{param.Name});");
            }
            else if (originType.IsEnum)
            {
                builder.AppendLine($"                var ___{param.Name} = ({originType.FullName})reader.Read7BitEncodedInt32();");
            }
            else if (originType == typeof(float) && float16 != null)
            {
                builder.AppendLine($"                var ___{param.Name} = reader.ReadFloat16({float16.IntegerMax});");
            }
            else if (Is7BitEncoded(originType) && sevenBitEncoded != null)
            {
                builder.AppendLine($"                var ___{param.Name} = reader.Read7BitEncoded{originType.Name}();");
            }
            else if (IsBaseType(originType))
            {
                builder.AppendLine($"                var ___{param.Name} = reader.Read{originType.Name}();");
            }
            else
            {
                builder.AppendLine($"                {GetTypeName(originType)} ___{param.Name} = default;");
                builder.AppendLine($"                {GetSerializerName(originType)}.Read(reader, ref ___{param.Name});");
            }
        }

        private void GenDispach(StringBuilder builder, Type type, List<MethodInfo> methods)
        {
        }

        private string GetDeclarePrefix(ParameterInfo param)
        {
            var isRef = param.ParameterType.IsByRef;
            var isIn = param.IsIn;
            if (isRef && isIn) return "in ";
            if (isRef) return "ref ";
            if (isIn) return "in ";
            return "";
        }
    }
}
