using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CompactBuffer.UnityEditor
{
    public static class MenuItems
    {
        private static string ReadAssemblyDefineName(string file)
        {
            var lines = File.ReadAllLines(file);
            foreach (var _line in lines)
            {
                var line = _line.Trim();
                if (!line.StartsWith("\"name\"")) continue;

                var pos = line.IndexOf(":");
                if (pos <= 0) break;
                line = line.Substring(pos + 1);
                pos = line.IndexOf("\"");
                if (pos <= 0) break;
                line = line.Substring(pos + 1);
                pos = line.IndexOf("\"");
                if (pos <= 0) break;
                line = line.Substring(0, pos);
                return line;
            }
            Debug.LogWarning($"Parse {file} failed");
            return "";
        }

        private static void FindAssemblyDefines(string path, Dictionary<string, string> result)
        {
            var dirInfo = new DirectoryInfo(path);

            foreach (var file in dirInfo.GetFiles())
            {
                if (file.Extension == ".asmdef")
                {
                    var name = ReadAssemblyDefineName(file.FullName);
                    if (!string.IsNullOrEmpty(name))
                    {
                        Debug.Log($"{name} : {file.DirectoryName}");
                        result.Add(name, file.Directory.FullName);
                    }
                }
            }

            foreach (var dir in dirInfo.GetDirectories())
            {
                FindAssemblyDefines(dir.FullName, result);
            }
        }

        private static Dictionary<string, string> GetAssemblyDefines()
        {
            var retval = new Dictionary<string, string>();
            FindAssemblyDefines(Application.dataPath, retval);
            return retval;
        }

        private static Dictionary<CompactBufferGroup, string> GetCompactBufferGroups()
        {
            var retval = new Dictionary<CompactBufferGroup, string>();
            var assemblyDefines = GetAssemblyDefines();

            foreach (var type in CompactBufferUtils.EnumAllTypes(typeof(CompactBufferGroup)))
            {
                if (type.IsAbstract) continue;


                var group = (CompactBufferGroup)null;
                try
                {
                    group = (CompactBufferGroup)Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    continue;
                }

                var name = group.GetAssemblyName();
                if (!assemblyDefines.ContainsKey(name)) continue;
                retval.Add(group, assemblyDefines[name]);
            }

            return retval;
        }

        private static readonly string CompactBufferFileName = "GenCode.CompactBuffer.cs";
        private static readonly string ProtocolFileName = "GenCode.Protocol.cs";

        [MenuItem("Tools/CompactBuffer/Clean", false, 1)]
        public static void Clean()
        {
            foreach (var path in GetCompactBufferGroups().Values)
            {
                File.Delete(Path.Join(path, CompactBufferFileName));
                File.Delete(Path.Join(path, ProtocolFileName));
            }
        }

        [MenuItem("Tools/CompactBuffer/Generate", false, 2)]
        public static void Generate()
        {
            foreach (var (group, path) in GetCompactBufferGroups())
            {
                var serializerGenerator = new SerializerGenerator();
                var protocolGenerator = new ProtocolGenerator(serializerGenerator);
                foreach (var assembly in group.GetAssemblies())
                {
                    serializerGenerator.AddAssembly(assembly);
                    protocolGenerator.AddAssembly(assembly);
                }
                CompactBuffer.Reset();

                var resultProtocol = protocolGenerator.GenCode();
                var resultSerializer = serializerGenerator.GenCode();
                File.WriteAllText(Path.Join(path, ProtocolFileName), resultProtocol);
                File.WriteAllText(Path.Join(path, CompactBufferFileName), resultSerializer);
            }
        }
    }
}
