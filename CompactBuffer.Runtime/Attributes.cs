
using System;

namespace CompactBuffer
{
    public class VariantAttribute : Attribute
    {
    }

    public class Float16Attribute : Attribute
    {
        public readonly int IntegerMax;

        public Float16Attribute(int integerMax)
        {
            IntegerMax = integerMax;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CompactBufferAttribute : Attribute
    {
        public readonly Type SerializerType;
        public readonly bool IsAutoGen;

        public CompactBufferAttribute(Type serializerType, bool isAutoGen = false)
        {
            SerializerType = serializerType;
            IsAutoGen = isAutoGen;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CompactBufferGenCodeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class CustomSerializerAttribute : Attribute
    {
        public readonly Type SerializerType;

        public CustomSerializerAttribute(Type serializerType)
        {
            SerializerType = serializerType;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class DontReturnAttribute : Attribute
    {
    }

    public enum ProtocolType
    {
        None,
        Client,
        Server,
    }

    [AttributeUsage(AttributeTargets.Interface)]
    public class ProtocolIdAttribute : Attribute
    {
        public readonly int ProtocolId;
        public readonly ProtocolType ProtocolType;
        public readonly bool Dispatch;

        public ProtocolIdAttribute(int protocolId, ProtocolType protocolType = ProtocolType.None, bool dispatch = false)
        {
            ProtocolId = protocolId;
            ProtocolType = protocolType;
            Dispatch = dispatch;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ProtocolAttribute : Attribute
    {
        public readonly Type ProtocolType;

        public ProtocolAttribute(Type protocolType)
        {
            ProtocolType = protocolType;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class ChannelAttribute : Attribute
    {
        public readonly byte Channel;

        public ChannelAttribute(byte channel)
        {
            Channel = channel;
        }
    }
}