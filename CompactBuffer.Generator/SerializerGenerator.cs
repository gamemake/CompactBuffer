
using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public class SerializerGenerator : Generator, IAddAddtionType
    {
        private HashSet<Assembly> m_Assemblies = new HashSet<Assembly>();
        private HashSet<Type> m_Types = new HashSet<Type>();
        private HashSet<Type> m_AdditionTypes = new HashSet<Type>();
        private HashSet<Type> m_CustomSerializerTypes = new HashSet<Type>();

        public SerializerGenerator()
        {
        }

        public void AddAssembly(Assembly assembly)
        {
            m_Assemblies.Add(assembly);

            foreach (var type in assembly.GetTypes())
            {
                if (!typeof(ICompactBufferSerializer).IsAssignableFrom(type)) continue;
                var attribute = type.GetCustomAttribute<CompactBufferAttribute>();
                if (attribute == null) continue;
                if (attribute.IsAutoGen) continue;
                m_CustomSerializerTypes.Add(attribute.SerializerType);
            }
        }

        public void AddAdditionType(Type type)
        {
            if (type.IsEnum) return;
            if (!m_Assemblies.Contains(type.Assembly)) return;
            if (m_AdditionTypes.Contains(type)) return;
            m_AdditionTypes.Add(type);
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
            builder.AppendLine($"namespace CompactBufferAutoGen");
            builder.AppendLine($"{{");

            foreach (var assembly in m_Assemblies)
            {
                GenCode(builder, assembly);
            }

            foreach (var type in m_AdditionTypes)
            {
                if (m_Assemblies.Contains(type.Assembly))
                {
                    GenCode(builder, type);
                }
            }

            builder.AppendLine($"}}");
            builder.AppendLine();
        }

        private void GenCode(StringBuilder builder, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsInterface) continue;
                if (type.IsAbstract) continue;
                if (type.IsEnum) continue;

                var customSerializer = type.GetCustomAttribute<CompactBufferGenCodeAttribute>();
                if (customSerializer == null) continue;

                GenCode(builder, type);
            }
        }

        private void GenCode(StringBuilder builder, Type type)
        {
            if (type.IsEnum) return;
            if (m_CustomSerializerTypes.Contains(type)) return;
            if (m_Types.Contains(type)) return;

            if (m_Types.Count > 0) builder.AppendLine();
            m_Types.Add(type);

            var fields = new List<FieldInfo>();
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                fields.Add(field);

                if (!m_Assemblies.Contains(field.FieldType.Assembly)) continue;

                if (field.FieldType.IsArray)
                {
                    var elementType = field.FieldType.GetElementType();
                    if (m_Assemblies.Contains(elementType.Assembly)) GenCode(builder, elementType);
                    continue;
                }

                if (field.FieldType.IsGenericType)
                {
                    if (field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var args = field.FieldType.GetGenericArguments();
                        if (m_Assemblies.Contains(args[0].Assembly)) GenCode(builder, args[0]);
                        continue;
                    }
                    if (field.FieldType.GetGenericTypeDefinition() == typeof(HashSet<>))
                    {
                        var args = field.FieldType.GetGenericArguments();
                        if (m_Assemblies.Contains(args[0].Assembly)) GenCode(builder, args[0]);
                        continue;
                    }
                    if (field.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        var args = field.FieldType.GetGenericArguments();
                        if (m_Assemblies.Contains(args[0].Assembly)) GenCode(builder, args[0]);
                        if (m_Assemblies.Contains(args[1].Assembly)) GenCode(builder, args[1]);
                        continue;
                    }
                }

                GenCode(builder, field.FieldType);
            }

            builder.AppendLine($"    [CompactBuffer.CompactBuffer(typeof({type.FullName}), true)]");
            builder.AppendLine($"    public class {type.FullName.Replace(".", "_")}_Serializer : CompactBuffer.ICompactBufferSerializer<{type.FullName}>");
            builder.AppendLine($"    {{");
            builder.AppendLine($"        public static void Read(CompactBuffer.BufferReader reader, ref {type.FullName} target)");
            builder.AppendLine($"        {{");
            if (!type.IsValueType)
            {
                builder.AppendLine($"            var length = reader.ReadVariantInt32();");
                builder.AppendLine($"            if (length == 0) {{ target = null; return; }}");
                builder.AppendLine($"            if (length != {fields.Count + 1}) {{ throw new CompactBuffer.CompactBufferExeption(\"data version not match\"); }}");
                builder.AppendLine($"            if (target == null) {{ target = new {type.FullName}(); }}");
            }
            foreach (var field in fields)
            {
                GenReadField(builder, type, field);
            }
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        public static void Write(CompactBuffer.BufferWriter writer, in {type.FullName} target)");
            builder.AppendLine($"        {{");
            if (!type.IsValueType)
            {
                builder.AppendLine($"            if (target == null)");
                builder.AppendLine($"            {{");
                builder.AppendLine($"                writer.WriteVariantInt32(0);");
                builder.AppendLine($"                return;");
                builder.AppendLine($"            }}");
            }
            builder.AppendLine($"            writer.WriteVariantInt32({fields.Count + 1});");
            foreach (var field in fields)
            {
                GenWriteField(builder, type, field);
            }
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        public static void Copy(in {type.FullName} src, ref {type.FullName} dst)");
            builder.AppendLine($"        {{");
            if (!type.IsValueType)
            {
                builder.AppendLine($"            if (src == null) {{ dst = null; return; }}");
                builder.AppendLine($"            if (dst == null) dst = new {type.FullName}();");
            }
            foreach (var field in fields)
            {
                GenCopyField(builder, field);
            }
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        void CompactBuffer.ICompactBufferSerializer<{type.FullName}>.Read(CompactBuffer.BufferReader reader, ref {type.FullName} target)");
            builder.AppendLine($"        {{");
            builder.AppendLine($"            Read(reader, ref target);");
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        void CompactBuffer.ICompactBufferSerializer<{type.FullName}>.Write(CompactBuffer.BufferWriter writer, in {type.FullName} target)");
            builder.AppendLine($"        {{");
            builder.AppendLine($"            Write(writer, in target);");
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        void CompactBuffer.ICompactBufferSerializer<{type.FullName}>.Copy(in {type.FullName} src, ref {type.FullName} dst)");
            builder.AppendLine($"        {{");
            builder.AppendLine($"            Copy(in src, ref dst);");
            builder.AppendLine($"        }}");
            builder.AppendLine($"    }}");
        }

        private void GenReadField(StringBuilder builder, Type type, FieldInfo field)
        {
            var customSerializer = field.GetCustomAttribute<CustomSerializerAttribute>();
            if (customSerializer == null && field.FieldType.IsEnum)
            {
                builder.AppendLine($"            target.{field.Name} = ({field.FieldType.FullName})reader.ReadVariantInt32();");
                return;
            }
            else if (customSerializer == null && IsBaseType(field.FieldType))
            {
                var float16 = field.GetCustomAttribute<Float16Attribute>();
                if (float16 == null)
                {
                    float16 = type.GetCustomAttribute<Float16Attribute>();
                }

                if (field.GetCustomAttribute<VariantIntAttribute>() != null && IsVariantable(field.FieldType))
                {
                    builder.AppendLine($"            target.{field.Name} = reader.ReadVariant{field.FieldType.Name}();");
                }
                else if (float16 != null && field.FieldType == typeof(float))
                {
                    builder.AppendLine($"            target.{field.Name} = reader.ReadFloat16({float16.IntegerMax});");
                }
                else
                {
                    builder.AppendLine($"            target.{field.Name} = reader.Read{field.FieldType.Name}();");
                }
                return;
            }

            builder.AppendLine($"            {GetSerializerName(field)}.Read(reader, ref target.{field.Name});");
        }

        private void GenWriteField(StringBuilder builder, Type type, FieldInfo field)
        {
            var customSerializer = field.GetCustomAttribute<CustomSerializerAttribute>();
            if (customSerializer == null && field.FieldType.IsEnum)
            {
                builder.AppendLine($"            writer.WriteVariantInt32((int)target.{field.Name});");
                return;
            }
            else if (customSerializer == null && IsBaseType(field.FieldType))
            {
                var float16 = field.GetCustomAttribute<Float16Attribute>();
                if (float16 == null)
                {
                    float16 = type.GetCustomAttribute<Float16Attribute>();
                }

                if (field.GetCustomAttribute<VariantIntAttribute>() != null && IsVariantable(field.FieldType))
                {
                    builder.AppendLine($"            writer.WriteVariant{field.FieldType.Name}(target.{field.Name});");
                }
                else if (float16 != null && field.FieldType == typeof(float))
                {
                    builder.AppendLine($"            writer.WriteFloat16(target.{field.Name}, {float16.IntegerMax});");
                }
                else
                {
                    builder.AppendLine($"            writer.Write(target.{field.Name});");
                }
                return;
            }
            builder.AppendLine($"            {GetSerializerName(field)}.Write(writer, in target.{field.Name});");
        }

        private void GenCopyField(StringBuilder builder, FieldInfo field)
        {
            if (IsBaseType(field.FieldType) || field.FieldType.IsEnum)
            {
                builder.AppendLine($"            dst.{field.Name} = src.{field.Name};");
                return;
            }
            builder.AppendLine($"            {GetSerializerName(field)}.Copy(in src.{field.Name}, ref dst.{field.Name});");
        }

        private string GetSerializerName(FieldInfo field)
        {
            var customSerializer = field.GetCustomAttribute<CustomSerializerAttribute>();
            if (customSerializer != null)
            {
                var type = customSerializer.SerializerType;
                if (type.IsGenericType)
                {
                    var className = "";
                    foreach (var ttype in type.GetGenericArguments())
                    {
                        if (!string.IsNullOrEmpty(className)) className += ", ";
                        className += ttype.FullName;
                    }
                    className = $"{type.GetGenericTypeDefinition().FullName}<{className}>";
                    return className;
                }
                else
                {
                    return customSerializer.SerializerType.FullName;
                }
            }
            else
            {
                var serializer = CompactBuffer.GetSerializer(field.FieldType);
                if (serializer != null)
                {
                    return GetTypeName(serializer.GetType());
                }
            }

            if (customSerializer != null)
            {
                return $"CompactBuffer.CompactBuffer.GetCustomSerializer<{customSerializer.SerializerType.FullName}, {field.FieldType.FullName}>()";
            }

            if (field.FieldType.IsArray)
            {
                return $"CompactBuffer.CompactBuffer.GetArraySerializer<{GetTypeName(field.FieldType.GetElementType())}>()";
            }

            if (field.FieldType.IsGenericType)
            {
                var args = field.FieldType.GetGenericArguments();
                if (field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return $"CompactBuffer.CompactBuffer.GetListSerializer<{GetTypeName(args[0])}>()";
                }
                if (field.FieldType.GetGenericTypeDefinition() == typeof(HashSet<>))
                {
                    return $"CompactBuffer.CompactBuffer.GetHashSetSerializer<{GetTypeName(args[0])}>()";
                }
                if (field.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return $"CompactBuffer.CompactBuffer.GetDictionarySerializer<{GetTypeName(args[0])}, {GetTypeName(args[1])}>()";
                }
            }

            return $"CompactBufferAutoGen.{field.FieldType.FullName.Replace(".", "_")}_Serializer";
        }
    }
}
