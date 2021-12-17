namespace ChatApp.Common.Net.Packet.Header;

public enum ClientHeader : uint
{
    NullPacket = 0,
    ClientAccountRegister = 4183544625,
    ClientChat = 3762148378,
    ClientLogin = 1642911930,
}