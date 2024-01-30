using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CompactBuffer.UnityEditor
{
    public class AssemblyDefine
    {
        public string name;
        public string[] references;
        public string[] includePlatforms;
        public string[] excludePlatforms;
        public bool allowUnsafeCode;
        public bool overrideReferences;
        public string[] precompiledReferences;
        public bool autoReferenced;
        public string[] defineConstraints;
        public string[] versionDefines;
        public bool noEngineReferences;
    }

    public static class MenuItems
    {
        private static string ReadAssemblyDefineName(string file)
        {
            var json = File.ReadAllText(file);
            var asmdef = JsonUtility.FromJson<AssemblyDefine>(json);
            return asmdef.name;
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
                Debug.Log($"CompactBuffer.CodeGen : Clean {path}");
                File.Delete(Path.Join(path, CompactBufferFileName));
                File.Delete(Path.Join(path, ProtocolFileName));
            }
        }

        [MenuItem("Tools/CompactBuffer/Generate", false, 2)]
        public static void Generate()
        {
            foreach (var (group, path) in GetCompactBufferGroups())
            {
                Debug.Log($"CompactBuffer.CodeGen : Generate {path}");

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
