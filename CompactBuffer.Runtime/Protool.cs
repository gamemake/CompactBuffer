
using System;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public interface IProtocol
    {
    }

    public interface IProtocolSender
    {
        BufferWriter GetStreamWriter(int protocolId);
        void Send(BufferWriter writer);
    }

    public interface IProtocolStub
    {
        void Dispatch(BufferReader reader);
    }

    public abstract class ProtocolProxy
    {
        protected readonly IProtocolSender m_Sender;

        public ProtocolProxy(IProtocolSender sender)
        {
            m_Sender = sender;
        }
    }

    public static class Protocol
    {
        private static Dictionary<Type, Type> m_ProxyTypes = GetAllProxyTypes();
        private static Dictionary<Type, Type> m_StubTypes = GetAllStubTypes();

        private static Dictionary<Type, Type> GetAllProxyTypes()
        {
            var retval = new Dictionary<Type, Type>();
            foreach (var type in CompactBufferUtils.EnumAllTypes(typeof(ProtocolProxy)))
            {
                if (type.IsAbstract) continue;
                var proxyAttribute = type.GetCustomAttribute<ProtocolAttribute>();
                if (proxyAttribute == null) continue;
                var protocolType = proxyAttribute.ProtocolType;
                if (protocolType == null || !protocolType.IsInterface || !typeof(IProtocol).IsAssignableFrom(protocolType))
                {
                    throw new ArgumentException("invalid");
                }
                retval.Add(protocolType, type);
            }
            return retval;
        }

        private static Dictionary<Type, Type> GetAllStubTypes()
        {
            var retval = new Dictionary<Type, Type>();
            foreach (var type in CompactBufferUtils.EnumAllTypes(typeof(IProtocolStub)))
            {
                if (type.IsAbstract) continue;
                var protocolAttribute = type.GetCustomAttribute<ProtocolAttribute>();
                if (protocolAttribute == null) continue;
                var protocolType = protocolAttribute.ProtocolType;
                if (protocolType == null || !protocolType.IsInterface || !typeof(IProtocol).IsAssignableFrom(protocolType))
                {
                    throw new ArgumentException("invalid");
                }
                retval.Add(protocolType, type);
            }
            return retval;
        }

        public static T GetProxy<T>(IProtocolSender sender)
            where T : IProtocol
        {
            if (!m_ProxyTypes.TryGetValue(typeof(T), out var type))
            {
                throw new CompactBufferExeption("proxy not found");
            }
            else
            {
                var proxy = (T)Activator.CreateInstance(type, sender);
                return proxy;
            }
        }

        public static IProtocolStub GetStub<T>(T target)
            where T : IProtocol
        {
            if (!m_StubTypes.TryGetValue(typeof(T), out var type))
            {
                throw new CompactBufferExeption("stub not found");
            }
            else
            {
                var stub = (IProtocolStub)Activator.CreateInstance(type, target);
                return stub;
            }
        }

        public static IProtocolStub GetDispacher<T>(T impl)
            where T : IProtocol
        {
            return default;
        }
    }
}
