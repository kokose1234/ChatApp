using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Client;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;
using ChatApp.Server.Managers;
using LogManager = ChatApp.Server.Managers.LogManager;

namespace ChatApp.Server.Net.Handlers;

[PacketHandler(ClientHeader.ClientChat)]
public class ChatHandler : AbstractHandler
{
    internal override void Handle(ChatSession session, InPacket inPacket)
    {
        try
        {
            var request = inPacket.Decode<ClientChat>();

            if (request.Message.StartsWith('/') && session.IsAdmin)
            {
                var split = request.Message.Split(' ');

                switch (split[0])
                {
                    case "/kick":
                    {
                        var packet = new OutPacket(ServerHeader.ServerMessage);
                        var serverMessage = new ServerMessage();

                        if (split.Length != 2)
                        {
                            serverMessage.Message = "올바르지 않은 사용법. 사용예: /kick name";

                            packet.Encode(serverMessage);
                            packet.WriteLength();
                            session.Send(packet);
                            return;
                        }

                        if (ChatServer.Sessions.Values.All(x => x.Name != split[1]))
                        {
                            serverMessage.Message = $"{split[1]}은(는) 현재 접속중이지 않습니다.";

                            packet.Encode(serverMessage);
                            packet.WriteLength();
                            session.Send(packet);
                            return;
                        }

                        ChatServer.Sessions.Values.FirstOrDefault(x => x.Name == split[1])?.Disconnect();
                        serverMessage.Message = $"{split[1]}이(가) 강제퇴장 당했습니다.";

                        packet.Encode(serverMessage);
                        packet.WriteLength();

                        foreach (var client in ChatServer.Sessions.Values.Where(x => x.Name != ""))
                        {
                            client.Send(packet, false);
                        }

                        packet.Dispose();
                        break;
                    }
                }
            }
            else
            {
                var packet = new OutPacket(ServerHeader.ServerChat);
                var response = ObjectPoolManager.ChatPool.Get();
                response.Author = request.Author;
                response.Message = request.Message;

                packet.Encode(response);
                packet.WriteLength();

                foreach (var client in ChatServer.Sessions.Values.Where(x => x.Name != ""))
                {
                    client.Send(packet, false);
                }

                packet.Dispose();
                ObjectPoolManager.ChatPool.Return(response);
            }
        }
        catch (Exception ex)
        {
            LogManager.GetExceptionLogger().Error(ex);
        }
    }
}