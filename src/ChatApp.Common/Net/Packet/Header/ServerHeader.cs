namespace ChatApp.Common.Net.Packet.Header;

public enum ServerHeader : uint
{
    NullPacket = 0,
    ServerAccountRegister = 3515379851,
    ServerHandshake = 23746173,
}