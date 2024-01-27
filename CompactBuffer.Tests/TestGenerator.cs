
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace CompactBuffer.Tests;

public class TestGenerator
{
    public static string GetDirName()
    {
#pragma warning disable IL3000
        var dirName = Assembly.GetEntryAssembly().Location;
#pragma warning restore IL3000
        if (string.IsNullOrWhiteSpace(dirName))
            dirName = Process.GetCurrentProcess().MainModule.FileName;
        for (; ; )
        {
            if (Path.GetFileName(dirName) == "CompactBuffer") break;
            dirName = Path.GetDirectoryName(dirName);
        }
        return dirName;
    }

    [Fact]
    public void GenCompactBuffer()
    {
        var generator = new SerializerGenerator();
        generator.AddAssembly(typeof(Test.AAA).Assembly);
        CompactBuffer.Reset();

        generator.GenCode();
    }

    [Fact]
    public void GenProtocol()
    {
        var serializerGenerator = new SerializerGenerator();
        var protocolGenerator = new ProtocolGenerator(serializerGenerator);
        serializerGenerator.AddAssembly(typeof(Test.AAA).Assembly);
        protocolGenerator.AddAssembly(typeof(Test.AAA).Assembly);
        CompactBuffer.Reset();

        var resultProtocol = protocolGenerator.GenCode();
        File.WriteAllText(Path.Join(GetDirName(), "CompactBuffer.Tests", "Protocol.CodeGen.cs"), resultProtocol);

        var resultSerializer = serializerGenerator.GenCode();
        File.WriteAllText(Path.Join(GetDirName(), "CompactBuffer.Tests", "CompactBuffer.CodeGen.cs"), resultSerializer);
    }
}
