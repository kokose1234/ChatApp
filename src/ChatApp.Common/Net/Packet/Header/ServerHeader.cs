namespace ChatApp.Common.Net.Packet.Header;

public enum ServerHeader : uint
{
    NullPacket = 0,
    ServerAccountRegister = 3515379851,
    ServerChat = 4182789604,
    ServerHandshake = 23746173,
    ServerLogin = 1005611576,
    ServerMessage = 1244765267,
}