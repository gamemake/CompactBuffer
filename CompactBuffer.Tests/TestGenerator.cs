
using System.IO;

namespace CompactBuffer.Tests;

public class TestGenerator
{
    [Fact]
    public void Generator()
    {
        var generator = new Generator();
        generator.AddAssembly(typeof(Test.AAA).Assembly);
        var result = generator.GenCode();
        File.WriteAllText("/Users/deepblue/aaaa.cs", result);
    }
}
