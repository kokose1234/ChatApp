using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Header;

namespace ChatApp.Server.Net.Handlers;

[PacketHandler(ClientHeader.ClientAccountRegister)]
internal class AccountRegisterHandler : AbstractHandler
{
    internal override void Handle(ChatSession session, InPacket inPacket) //TODO: 맥주소 밴
    {
        var packet = new OutPacket(ServerHeader.ServerAccountRegister);
    }
}