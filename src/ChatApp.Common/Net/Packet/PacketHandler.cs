using ChatApp.Common.Net.Packet.Header;

namespace ChatApp.Common.Net.Packet;

[AttributeUsage(AttributeTargets.Class)]
public class PacketHandler : Attribute
{
    public uint Header { get; }

    public PacketHandler(ServerHeader header) => Header = (uint)header;
    public PacketHandler(ClientHeader header) => Header = (uint)header;
}