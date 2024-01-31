
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Tests;

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
        generator.AddAssembly(typeof(AAA).Assembly);

        generator.GenCode();
    }

    [Fact]
    public void GenProtocol()
    {
        var serializerGenerator = new SerializerGenerator();
        var protocolGenerator = new ProtocolGenerator(serializerGenerator);
        serializerGenerator.AddAssembly(typeof(AAA).Assembly);
        protocolGenerator.AddAssembly(typeof(AAA).Assembly);

        var resultProtocol = protocolGenerator.GenCode();
        File.WriteAllText(Path.Join(GetDirName(), "CompactBuffer.Tests", "CodeGen.Protocol.cs"), resultProtocol);

        var resultSerializer = serializerGenerator.GenCode();
        File.WriteAllText(Path.Join(GetDirName(), "CompactBuffer.Tests", "CodeGen.CompactBuffer.cs"), resultSerializer);
    }
}
