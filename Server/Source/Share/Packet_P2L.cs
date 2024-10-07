using System;
using Global;
using SNetwork;

namespace Packet_C2G
{
    enum Protocol
    {
        C2G_RequestAuth = 10000,
        G2C_ResultAuth,
        Max,
    }

    public class C2G_RequestAuth : INetPacket
    {
        public int ServerKey = 0;
        public string DeviceID = string.Empty;
        public CDefine.AuthType m_AuthType = CDefine.AuthType.Max;

        public CryptType GetCrypt() { return CryptType.None; }


        public ushort GetProtocol()
        {
            return (ushort)Protocol.C2G_RequestAuth;
        }

        public void Read(SNetReader reader)
        {
            reader.Read(ref ServerKey); 
            reader.Read(ref DeviceID);
            ushort type = 0;
            reader.Read(ref type);
            m_AuthType = (CDefine.AuthType)type;
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(ServerKey);
            writer.Write(DeviceID);
            ushort type = (ushort)m_AuthType;
            writer.Write(type);
        }
    }

    public class G2C_ResultAuth : INetPacket
    {
        public ushort m_Result = 0;
        public long m_UID = 0;
        public string m_DeviceID = string.Empty;
        public string m_ServerHost = string.Empty;
        public ushort m_ServerPort = 0;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol()
        {
            return (ushort)Protocol.G2C_ResultAuth;
        }

        public void Read(SNetReader reader)
        {
            if (!reader.Read(ref m_Result)) return;
            if (!reader.Read(ref m_UID)) return;
            if (!reader.Read(ref m_DeviceID)) return;
            if (!reader.Read(ref m_ServerHost)) return;
            if (!reader.Read(ref m_ServerPort)) return;
        }

        public void Write(SNetWriter writer)
        {
            if (!writer.Write(m_Result)) return;
            if (!writer.Write(m_UID)) return;
            if (!writer.Write(m_DeviceID)) return;
            if (!writer.Write(m_ServerHost)) return;
            if (!writer.Write(m_ServerPort)) return;
        }
    }

}
