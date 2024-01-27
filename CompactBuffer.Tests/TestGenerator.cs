
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace CompactBuffer.Tests;

public class TestGenerator
{
    [Fact]
    public void Generator()
    {
        var generator = new SerializerGenerator();
        generator.AddAssembly(typeof(Test.AAA).Assembly);
        CompactBuffer.Reset();
        var result = generator.GenCode();

#pragma warning disable IL3000
        var fileName = Assembly.GetEntryAssembly().Location;
#pragma warning restore IL3000
        if (string.IsNullOrWhiteSpace(fileName))
            fileName = Process.GetCurrentProcess().MainModule.FileName;
        for (; ; )
        {
            if (Path.GetFileName(fileName) == "CompactBuffer") break;
            fileName = Path.GetDirectoryName(fileName);
        }
        fileName = Path.Join(fileName, "CompactBuffer.Tests", "CompactBuffer.CodeGen.cs");
        File.WriteAllText(fileName, result);
    }
}
