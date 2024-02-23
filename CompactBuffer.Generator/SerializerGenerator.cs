
using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public class SerializerGenerator : Generator, IAddAddtionType
    {
        private HashSet<Type> m_Types = new HashSet<Type>();
        private List<Type> m_AdditionTypes = new List<Type>();
        private List<Type> m_SupportGenericTypes = new List<Type> { typeof(List<>), typeof(HashSet<>), typeof(Dictionary<,>), typeof(Span<>), typeof(ReadOnlySpan<>) };

        public SerializerGenerator()
        {
        }

        public bool AddAdditionType(Type type)
        {
            if (type.IsByRef && type.GetElementType() != null) type = type.GetElementType();
            
            if (!m_Assemblies.Contains(type.Assembly)) return true;
            if (m_CustomSerializerTypes.ContainsKey(type)) return true;
            if (type.GetCustomAttribute<GenCodeAttribute>() != null) return true;
            if (type.IsArray) return AddAdditionType(type.GetElementType());
            if (type.IsEnum) return true;
            if (m_AdditionTypes.Contains(type)) return true;
            if (type.IsInterface) return false;
            if (type.IsAbstract) return false;
            if (type.IsGenericType)
            {
                if (!m_SupportGenericTypes.Contains(type.GetGenericTypeDefinition())) return false;
                foreach (var gType in type.GetGenericArguments())
                {
                    if (!AddAdditionType(gType)) return false;
                }
                return true;
            }
            m_AdditionTypes.Add(type);
            return true;
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

                if (m_CustomSerializerTypes.ContainsKey(type)) continue;
                var customSerializer = type.GetCustomAttribute<GenCodeAttribute>();
                if (customSerializer == null) continue;

                GenCode(builder, type);
            }
        }

        private void GenCode(StringBuilder builder, Type type)
        {
            if (m_Types.Contains(type)) return;
            if (m_Types.Count > 0) builder.AppendLine();
            m_Types.Add(type);

            var fields = new List<FieldInfo>();
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (field.IsInitOnly) continue;

                if (!AddAdditionType(field.FieldType))
                {
                    throw new CompactBufferExeption($"{type.FullName}.{field.Name} unsupport type {GetTypeName(field.FieldType)}");

                }
                fields.Add(field);
            }

            builder.AppendLine($"    [CompactBuffer.AutoGen(typeof({type.FullName}))]");
            builder.AppendLine($"    public class {type.FullName.Replace(".", "_")}_Serializer : CompactBuffer.ICompactBufferSerializer<{type.FullName}>");
            builder.AppendLine($"    {{");
            builder.AppendLine($"        public static void Read(CompactBuffer.BufferReader reader, ref {type.FullName} target)");
            builder.AppendLine($"        {{");
            if (!type.IsValueType)
            {
                builder.AppendLine($"            var length = reader.Read7BitEncodedInt32();");
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
                builder.AppendLine($"                writer.Write7BitEncodedInt32(0);");
                builder.AppendLine($"                return;");
                builder.AppendLine($"            }}");
                builder.AppendLine($"            writer.Write7BitEncodedInt32({fields.Count + 1});");
            }
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
            var customSerializer = field.GetCustomAttribute<OverwriteAttribute>();
            var float16 = field.GetCustomAttribute<Float16Attribute>();
            if (float16 == null)
            {
                float16 = type.GetCustomAttribute<Float16Attribute>();
            }
            var sevenBitEncoded = field.GetCustomAttribute<SevenBitEncodedIntAttribute>();
            var originType = field.FieldType;

            if (customSerializer != null)
            {
                builder.AppendLine($"            {GetTypeName(customSerializer.SerializerType)}.Read(reader, ref target.{field.Name});");
            }
            else if (originType.IsEnum)
            {
                builder.AppendLine($"            target.{field.Name} = ({originType.FullName})reader.Read7BitEncodedInt32();");
            }
            else if (originType == typeof(float) && float16 != null)
            {
                builder.AppendLine($"            target.{field.Name} = reader.ReadFloat16({float16.IntegerMax});");
            }
            else if (Is7BitEncoded(originType) && sevenBitEncoded != null)
            {
                builder.AppendLine($"            target.{field.Name} = reader.Read7BitEncoded{originType.Name}();");
            }
            else if (IsBaseType(originType))
            {
                builder.AppendLine($"            target.{field.Name} = reader.Read{originType.Name}();");
            }
            else
            {
                builder.AppendLine($"            {GetSerializerName(originType)}.Read(reader, ref target.{field.Name});");
            }
        }

        private void GenWriteField(StringBuilder builder, Type type, FieldInfo field)
        {
            var customSerializer = field.GetCustomAttribute<OverwriteAttribute>();
            var float16 = field.GetCustomAttribute<Float16Attribute>();
            if (float16 == null)
            {
                float16 = type.GetCustomAttribute<Float16Attribute>();
            }
            var sevenBitEncoded = field.GetCustomAttribute<SevenBitEncodedIntAttribute>();
            var originType = field.FieldType;

            if (customSerializer != null)
            {
                builder.AppendLine($"            {GetTypeName(customSerializer.SerializerType)}.Write(writer, in target.{field.Name});");
            }
            else if (originType.IsEnum)
            {
                builder.AppendLine($"            writer.Write7BitEncodedInt32((int)target.{field.Name});");
            }
            else if (originType == typeof(float) && float16 != null)
            {
                builder.AppendLine($"            writer.WriteFloat16(target.{field.Name}, {float16.IntegerMax});");
            }
            else if (Is7BitEncoded(originType) && sevenBitEncoded != null)
            {
                builder.AppendLine($"            writer.Write7BitEncoded{originType.Name}(target.{field.Name});");
            }
            else if (IsBaseType(originType))
            {
                builder.AppendLine($"            writer.Write(target.{field.Name});");
            }
            else
            {
                builder.AppendLine($"            {GetSerializerName(originType)}.Write(writer, in target.{field.Name});");
            }
        }

        private void GenCopyField(StringBuilder builder, FieldInfo field)
        {
            var originType = field.FieldType;

            if (originType.IsValueType || IsBaseType(originType))
            {
                builder.AppendLine($"            dst.{field.Name} = src.{field.Name};");
            }
            else
            {
                builder.AppendLine($"            {GetSerializerName(originType)}.Copy(in src.{field.Name}, ref dst.{field.Name});");
            }
        }
    }
}
