
using System;
using System.IO;
using System.Text;
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

            var generator = new SerializerGenerator();

            for (var i = 1; i < arg.Length; i++)
            {
                var assembly = Assembly.LoadFrom(arg[i]);
                generator.AddAssembly(assembly);
            }

            var newText = generator.GenCode();
            var orgText = string.Empty;
            if (File.Exists(arg[0]))
            {
                File.ReadAllText(arg[0], UTF8Encoding.UTF8);
            }
            if (newText != orgText)
            {
                File.WriteAllText(arg[0], newText);
            }
        }
    }
}
