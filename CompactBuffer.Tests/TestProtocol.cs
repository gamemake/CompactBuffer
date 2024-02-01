
using System;
using System.Reflection;
using System.Text.Json;
using System.Collections.Generic;
using Tests;

namespace CompactBuffer.Tests;

public class TestProtocol : IProtocolSender, IServerApi
{
    private byte[] m_Bytes = null;
    private int m_Length = 0;
    private object[] m_Input = null;
    private object[] m_Output = null;
    private JsonSerializerOptions m_JsonOptions = new JsonSerializerOptions { IncludeFields = true };

    private object[] ToObjs(params object[] args)
    {
        return args;
    }

    BufferWriter IProtocolSender.GetStreamWriter(int protocolId)
    {
        m_Bytes = new byte[100000];
        return new BufferWriter(m_Bytes);
    }

    void IProtocolSender.Send(BufferWriter writer)
    {
        m_Length = writer.Length;
    }

    void GoTest(string methodName, params object[] args)
    {
        var proxy = Protocol.GetProxy<IServerApi>(this);
        var stub = Protocol.GetStub<IServerApi>(this);
        var method = typeof(IServerApi).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
        method.Invoke(proxy, args);
        m_Input = args;
        var reader = new BufferReader(m_Bytes, 0, m_Length);
        stub.Dispatch(reader);
        Assert.Equal(reader.Position, m_Length);
        Assert.Equal(m_Input.Length, m_Output.Length);
        for (var i = 0; i < m_Input.Length; i++)
        {
            var iJson = JsonSerializer.Serialize(m_Input[i], m_JsonOptions);
            var oJson = JsonSerializer.Serialize(m_Output[i], m_JsonOptions);
            Assert.Equal(iJson, oJson);
        }
    }

    [Fact]
    public void TestCall()
    {
        GoTest("Call");
    }

    [Fact]
    public void TestCallString()
    {
        GoTest("CallString", "a");
    }

    [Fact]
    public void TestCallInt()
    {
        GoTest("CallInt", 111);
    }

    [Fact]
    public void CallTypeClass()
    {
        GoTest("CallTypeClass", new TypeClass(){
            kkk = 11,
            list = new List<int>(){345, 35,234},
            hashset = new HashSet<int>() {6, 7, 8},
            dict = new Dictionary<string, int>() {{"a", 1}},
        });
    }

    [Fact]
    public void CallVariant()
    {
        GoTest("CallVariant", (int)1, (long)444, (uint)4535, (int)23423, (int)2344);
    }

    [Fact]
    public void CallGuid()
    {
        GoTest("CallGuid", Guid.NewGuid());
    }

    [Fact]
    public void CallEnum()
    {
        GoTest("CallEnum", EnumTypes.Int);
    }

    [Fact]
    public void CallFloat16()
    {
        GoTest("CallFloat16", 0);
    }

    void IServerApi.Call()
    {
        m_Output = ToObjs();
    }

    void IServerApi.CallString(string a)
    {
        m_Output = ToObjs(a);
    }

    void IServerApi.CallInt(int a)
    {
        m_Output = ToObjs(a);
    }

    void IServerApi.CallTypeClass(TypeClass pa)
    {
        m_Output = ToObjs(pa);
    }

    void IServerApi.CallVariant(int v1, long v2, uint v3, int v4, int v5)
    {
        m_Output = ToObjs(v1, v2, v3, v4, v5);
    }

    void IServerApi.CallGuid(Guid _guid)
    {
        m_Output = ToObjs(_guid);
    }

    void IServerApi.CallEnum(EnumTypes _enum)
    {
        m_Output = ToObjs(_enum);
    }

    void IServerApi.CallFloat16(float v)
    {
        m_Output = ToObjs(v);
    }

    void IServerApi.CallReadOnlySpan(ReadOnlySpan<byte> aaaaaaaa)
    {
        throw new NotImplementedException();
    }

    void IServerApi.CallVariantType(VaiantType vv)
    {
        m_Output = ToObjs(vv);
    }

    void IServerApi.CallTypeClassRefReadonly(in TypeClass a)
    {
        m_Output = ToObjs(a);
    }

    void IServerApi.CallTypeClassRef(ref TypeClass a)
    {
        m_Output = ToObjs(a);
    }

    void IServerApi.CallTypeStructRefReadonly(in RefStruct a)
    {
        m_Output = ToObjs(a);
    }

    void IServerApi.CallTypeStructRef(ref RefStruct a)
    {
        m_Output = ToObjs(a);
    }

    void IServerApi.CallIntArray(int[] array)
    {
        m_Output = ToObjs(array);
    }

    void IServerApi.CallIntSpan(Span<int> span)
    {
    }

    void IServerApi.CallIntReadOnlySpan(ReadOnlySpan<int> span)
    {
    }
}
