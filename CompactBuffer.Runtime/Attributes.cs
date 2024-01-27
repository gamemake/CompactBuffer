
using System;

namespace CompactBuffer
{
    public class CompactBufferAttribute : Attribute
    {
        public readonly Type SerializerType;

        public CompactBufferAttribute(Type serializerType)
        {
            SerializerType = serializerType;
        }
    }

    public class CompactBufferGenCodeAttribute : Attribute
    {
        public CompactBufferGenCodeAttribute()
        {
        }
    }

    public class CustomSerializerAttribute : Attribute
    {
        public readonly Type SerializerType;

        public CustomSerializerAttribute(Type serializerType)
        {
            SerializerType = serializerType;
        }
    }

    public class ProtocolAttribute : Attribute
    {
        public readonly int ProtocolId;
        public readonly bool Dispatch;

        public ProtocolAttribute(int protocolId, bool dispatch = false)
        {
            ProtocolId = protocolId;
            Dispatch = dispatch;
        }
    }

    public class ProtocolProxyAttribute : Attribute
    {
        public readonly Type ProtocolType;

        public ProtocolProxyAttribute(Type protocolType)
        {
            ProtocolType = protocolType;
        }
    }

    public class ProtocolStubAttribute : Attribute
    {
        public readonly Type ProtocolType;

        public ProtocolStubAttribute(Type protocolType)
        {
            ProtocolType = protocolType;
        }
    }

    public class ProtocolDispacherAttribute : Attribute
    {
        public readonly Type ProtocolType;

        public ProtocolDispacherAttribute(Type protocolType)
        {
            ProtocolType = protocolType;
        }
    }
}
