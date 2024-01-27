
using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public class Generator
    {
        private HashSet<Assembly> m_Assemblies = new HashSet<Assembly>();
        private HashSet<Type> m_Types = new HashSet<Type>();

        public Generator()
        {
        }

        public void AddAssembly(Assembly assembly)
        {
            m_Assemblies.Add(assembly);
        }

        public void GenCode(string filename)
        {
        }

        public string GenCode()
        {
            var builder = new StringBuilder();
            GenCode(builder);
            return builder.ToString();
        }

        public void GenCode(StringBuilder builder)
        {
            builder.AppendLine($"// Generate by CompactBuffer.Generator");
            builder.AppendLine();
            builder.AppendLine($"namespace CompactBufferAutoGen");
            builder.AppendLine($"{{");

            m_Types.Clear();
            foreach (var type in CompactBufferUtils.EnumAllClass(typeof(object)))
            {
                var attribute = type.GetCustomAttribute<CompactBufferGenCodeAttribute>();
                if (attribute == null) continue;

                GenCode(builder, type);
            }

            builder.AppendLine($"}}");
            builder.AppendLine();
        }

        private void GenCode(StringBuilder builder, Type type)
        {
            if (!m_Assemblies.Contains(type.Assembly)) return;
            if (m_Types.Contains(type)) return;
            if (m_Types.Count > 0)
            {
                builder.AppendLine();
            }
            m_Types.Add(type);

            var fields = new List<FieldInfo>();
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                GenCode(builder, field.FieldType);
                fields.Add(field);
            }

            builder.AppendLine($"    [CompactBuffer.CompactBuffer(typeof({type.FullName}))]");
            builder.AppendLine($"    public class {type.FullName.Replace(".", "_")}_Serializer : CompactBuffer.ICompactBufferSerializer<{type.FullName}>");
            builder.AppendLine($"    {{");
            builder.AppendLine($"        public void Read(System.IO.BinaryReader reader, ref {type.FullName} target)");
            builder.AppendLine($"        {{");
            if (!type.IsValueType)
            {
                builder.AppendLine($"            var length = CompactBuffer.CompactBufferUtils.ReadLength(reader);");
                builder.AppendLine($"            if (length == 0) {{ target = null; return; }}");
                builder.AppendLine($"            if (length != {fields.Count + 1}) {{ throw new System.Exception(\"aaaa\"); }}");
                builder.AppendLine($"            if (target == null) {{ target = new {type.FullName}(); }}");
            }
            foreach (var field in fields)
            {
                GenReadField(builder, field);
            }
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        public void Write(System.IO.BinaryWriter writer, ref {type.FullName} target)");
            builder.AppendLine($"        {{");
            if (!type.IsValueType)
            {
                builder.AppendLine($"            if (target == null)");
                builder.AppendLine($"            {{");
                builder.AppendLine($"                CompactBuffer.CompactBufferUtils.WriteLength(writer, 0);");
                builder.AppendLine($"                return;");
                builder.AppendLine($"            }}");
            }
            builder.AppendLine($"            CompactBuffer.CompactBufferUtils.WriteLength(writer, {fields.Count + 1});");
            foreach (var field in fields)
            {
                GenWriteField(builder, field);
            }
            builder.AppendLine($"        }}");
            builder.AppendLine($"");
            builder.AppendLine($"        public void Copy(ref {type.FullName} src, ref {type.FullName} dst)");
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
            builder.AppendLine($"    }}");
        }

        private void GenReadField(StringBuilder builder, FieldInfo field)
        {
            if (field.GetCustomAttribute<CustomSerializerAttribute>() == null && CompactBuffer.IsBaseType(field.FieldType))
            {
                builder.AppendLine($"            target.{field.Name} = reader.Read{field.FieldType.Name}();");
                return;
            }
            builder.AppendLine($"            {GetSerializerName(field)}.Read(reader, ref target.{field.Name});");
        }

        private void GenWriteField(StringBuilder builder, FieldInfo field)
        {
            if (field.GetCustomAttribute<CustomSerializerAttribute>() == null && CompactBuffer.IsBaseType(field.FieldType))
            {
                builder.AppendLine($"            writer.Write(target.{field.Name});");
                return;
            }
            builder.AppendLine($"            {GetSerializerName(field)}.Write(writer, ref target.{field.Name});");
        }

        private void GenCopyField(StringBuilder builder, FieldInfo field)
        {
            if (field.GetCustomAttribute<CustomSerializerAttribute>() == null && CompactBuffer.IsBaseType(field.FieldType))
            {
                builder.AppendLine($"            dst.{field.Name} = src.{field.Name};");
                return;
            }
            builder.AppendLine($"            {GetSerializerName(field)}.Copy(ref src.{field.Name}, ref dst.{field.Name});");
        }

        private string GetSerializerName(FieldInfo field)
        {
            var attribute = field.GetCustomAttribute<CustomSerializerAttribute>();
            if (attribute != null)
            {
                return $"CompactBuffer.CompactBuffer.GetCustomSerializer<{attribute.SerializerType.FullName}, {field.FieldType.FullName}>()";
            }

            if (field.FieldType.IsArray)
            {
                return $"CompactBuffer.CompactBuffer.GetArraySerializer<{field.FieldType.GetElementType()}>()";
            }
            if (field.FieldType.IsGenericType)
            {
                if (field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return $"CompactBuffer.CompactBuffer.GetListSerializer<{field.FieldType.GetGenericArguments()[0].FullName}>()";
                }
                if (field.FieldType.GetGenericTypeDefinition() == typeof(HashSet<>))
                {
                    return $"CompactBuffer.CompactBuffer.GetHashSetSerializer<{field.FieldType.GetGenericArguments()[0].FullName}>()";
                }
                if (field.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return $"CompactBuffer.CompactBuffer.GetDictionarySerializer<{field.FieldType.GetGenericArguments()[0].FullName}, {field.FieldType.GetGenericArguments()[1].FullName}>()";
                }
            }
            return $"CompactBuffer.CompactBuffer.GetSerializer<{field.FieldType.FullName}>()";
        }
    }
}
