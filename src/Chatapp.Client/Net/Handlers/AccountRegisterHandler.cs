using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;

namespace ChatApp.Client.Net.Handlers;

[PacketHandler(ServerHeader.ServerAccountRegister)]
internal class AccountRegisterHandler : AbstractHandler
{
    internal override void Handle(ChatClient session, InPacket packet)
    {
        var response = packet.Decode<ServerAccountRegister>();
    }
}