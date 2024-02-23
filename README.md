# CompactBuffer

Implementing Binary Serialization Using C#.

# Requirement

* .NET Core 2.1 and later versions.
* Unity 2021 and later versions.

# Why use it？

* Use Code Generation to Avoid Reflection.
* Serialization with zero GC. (Depends on the data type being a value type and not containing any reference types)
* Simple and easy to use.

# How to Install?

On Windows
```dos
cd Unity
Build.bat
./CopyDllTo.bat ${UNITY_PROJECT_DIR}\Assets\CompactBuffer
```

On MacOS and Linux
```bash
cd Unity
./Build.sh
./CopyDllTo.sh ${UNITY_PROJECT_DIR}/Assets/CompactBuffer
```

# How to use it?
* Serializers
* Protocols

## Serializers
```cs
[CompactBufferGenCode]
public class LiveData
{
    public Guid guid;
    public string UserName;
}

var serailizer = CompactBuffer.Serializers.GetSerializer<LiveData>();
var bytes = new byte[1000];
var writer = new BufferWriter(bytes);
var liveData = new LiveData();
serializer.Write(writer, ref liveData);
var reader = new BufferReader(bytes, 0, writer.Length);
liveData = null;
serializer.Read(reader, ref liveData);
```

## Protocols

```cs
[ProtocolId(0)]
public interface ISyncUserData : IProtocol
{
    void Sync(string type, string data);
}
```

Protocol Proxy

```cs

public class ProtocolSender : IProtocolSender
{
    public BufferWriter GetStreamWriter()
    {
        var bytes = new byte[10000];
        return BufferWrite(bytes);
    }
    
    public void Send(BufferWriter writer, byte channel)
    {
        // Send writer.GetWriteBytes()
    }
}

var proxy = Protocol.GetProxy<ISyncUserData>(sender)
proxy.Sync("type", "data");
```

Protocol Stub

```cs
public class SyncUserDataImpl : ISyncUserData
{
    public void Sync(string type, string data)
    {
        // do something
    }
}

var target = new SyncDataImpl();
var stub = Protocol.GetStub<ISyncUserData>(target);
stub.Dispach(bufferReader);
```

# Attributes for Serialize

* Float16
* Variant
* Channel

## Float16

```cs
[Float16(1000)]
public struct Position
{
    public float x, y, z;
}

public struct Movement
{
    [Float16(1)]
    float NormalX;
    [Float16(1)]
    float NormalY;
    [Float16(1)]
    float NormalZ;
}

[ProtocolId(0)]
public interface ISyncPosition : IProtocol
{
    void Sync(
        [Float16(1)] float normalX,
        [Float16(1)] float normalY,
        [Float16(1)] float normalZ,
    );
}
```

The original float takes up 4 bytes, but after adding float16attribute, it only takes up 2 bytes.

## Variant

```cs
public struct Data
{
    [Variant]
    public int Count;
}

[ProtocolId(0)]
public interface ISyncPosition : IProtocol
{
    void Sync(
        [Variant] int Count
    );
}
```

## Channel

```cs
public enum ChannelType : byte
{
    OrderAndReliable,
    OrderAndUnreliable,
    DisorderAndUnreliable,
}

[ProtocolId(0)]
public interface ISyncPosition : IProtocol
{
    [Channel(ChannelType.OrderAndReliable)]
    void Sync(Vector3 position);

    [Channel(ChannelType.OrderAndUnreliable)]
    void LastPosition(Vector3 position)

    [Channel(ChannelType.DisorderAndUnreliable)]
    void Chat(string message);
}
```

# Generate code

1. Add CompactBufferGroup
2. Add asmdef
3. Click the menu [Tools/CompactBuffer/Generate] in the Unity editor.
4. CodeGen.CompactBuffer.cs and CodeGen.Protocol.cs have been generated in the directory of the asmdef file.

If code generation results in a compilation error, please click on Unity Editor menu's [Tools/CompactBuffer/Clean] then regenerate the code.

# License

CompactBuffer is licensed under the [MIT](LICENSE) license.