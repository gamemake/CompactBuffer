# CompactBuffer

Binary serialization implemented in pure C#.

# Install to unity project

On Windows
```dos
cd Unity
Build.bat
./CopyDllTo.bat ${UNITY_PROJECT_DIR}\Assets\CompactBuffer
```

On MacOS
```bash
cd Unity
./Build.sh
./CopyDllTo.sh ${UNITY_PROJECT_DIR}/Assets/CompactBuffer
```

# How to use it?
Pure C# binary data serialze

Compact Buffer
```cs
[CompactBufferGenCode]
public class LiveData
{
    public Guid guid;
    public string UserName;
}

var serailizer = CompactBuffer.GetSerializer<LiveData>();
var bytes = new byte[1000];
var writer = new BufferWriter(bytes);
var liveData = new LiveData();
serializer.Write(writer, ref liveData);
var reader = new BufferReader(bytes, 0, writer.Length);
liveData = null;
serializer.Read(reader, ref liveData);
```

Protocol

```cs
```

Protocol Proxy

```cs
```

Protocol Stub

```cs
```

# Generate code

1. Add asmdef
2. Add CompactBufferGroup
3. [Tools/CompactBuffer/Generate]

When compilation errors occur in the generated code, please try [Tools/CompactBuffer/Clean] and then regenerate the code.

# License

CompactBuffer is licensed under the [MIT](LICENSE) license.