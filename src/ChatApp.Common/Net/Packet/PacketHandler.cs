using ChatApp.Common.Net.Packet.Header;

namespace ChatApp.Common.Net.Packet;

public class PacketHandler : System.Attribute
{
    public uint Header { get; } = 0;
        
    public PacketHandler(ServerHeader header) => Header = (uint)header;
    public PacketHandler(ClientHeader header) => Header = (uint)header;
}