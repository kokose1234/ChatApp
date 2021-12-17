using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;

namespace ChatApp.Client.Net.Handlers;

[PacketHandler(ServerHeader.ServerChat)]
internal class ChatHandler : AbstractHandler
{
    internal override void Handle(ChatClient session, InPacket packet)
    {
        var response = packet.Decode<ServerChat>();

        if (Chat.Instance != null)
        {
            Chat.Instance.AddChat(response.Author, response.Message);
        }
    }
}