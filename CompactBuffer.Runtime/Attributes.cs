
using System;

namespace CompactBuffer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
    public class SevenBitEncodedIntAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class Float16Attribute : Attribute
    {
        public readonly int IntegerMax;

        public Float16Attribute(int integerMax)
        {
            IntegerMax = integerMax;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Class)]
    public class OverwriteAttribute : Attribute
    {
        public readonly Type SerializerType;
        public readonly bool IsAutoGen;

        public OverwriteAttribute(Type serializerType)
        {
            SerializerType = serializerType;
            IsAutoGen = false;
        }

        protected OverwriteAttribute(Type serializerType, bool isAutoGen)
        {
            SerializerType = serializerType;
            IsAutoGen = isAutoGen;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class GenCodeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class AutoGenAttribute : OverwriteAttribute
    {
        public AutoGenAttribute(Type serializerType) : base(serializerType, true)
        {
        }
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
