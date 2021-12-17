using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;

namespace ChatApp.Client.Net.Handlers;

[PacketHandler(ServerHeader.ServerMessage)]
internal class ServerMessageHandler : AbstractHandler
{
    internal override void Handle(ChatClient session, InPacket packet)
    {
        var response = packet.Decode<ServerMessage>();

        if (Chat.Instance != null)
        {
            Chat.Instance.AddSystemMessage(response.Message);
        }
    }
}