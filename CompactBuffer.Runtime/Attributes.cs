
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
}
