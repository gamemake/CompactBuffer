
using System;
using System.IO;
using System.Reflection;

namespace CompactBuffer.CodeGen
{
    public class CodeGen
    {
        public static void Main(string[] arg)
        {
            if (arg.Length < 2)
            {
                Console.WriteLine("invalid parameter");
                Environment.Exit(-1);
            }

            var serializerGenerator = new SerializerGenerator();
            var protocolGenerator = new ProtocolGenerator(serializerGenerator);
            CompactBuffer.Reset();

            for (var i = 1; i < arg.Length; i++)
            {
                var assembly = Assembly.LoadFrom(arg[i]);
                serializerGenerator.AddAssembly(assembly);
                protocolGenerator.AddAssembly(assembly);
            }

            var resultProtocol = protocolGenerator.GenCode();
            File.WriteAllText(Path.Join(arg[0], "CodeGen.Protocol.cs"), resultProtocol);

            var resultSerializer = serializerGenerator.GenCode();
            File.WriteAllText(Path.Join(arg[0], "CodeGen.CompactBuffer.cs"), resultSerializer);
        }
    }
}
