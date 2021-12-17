using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Client;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;
using ChatApp.Server.Database;
using SqlKata.Execution;
using LogManager = ChatApp.Server.Managers.LogManager;

namespace ChatApp.Server.Net.Handlers;

[PacketHandler(ClientHeader.ClientLogin)]
public class LoginHandler : AbstractHandler
{
    internal override void Handle(ChatSession session, InPacket inPacket)
    {
        var packet = new OutPacket(ServerHeader.ServerLogin);

        try
        {
            var request = inPacket.Decode<ClientLogin>();
            var account = DatabaseManager.Factory.Query("accounts").Get().ToArray();
            var result = account.FirstOrDefault(x => x.username == request.UserName && x.password == request.Password);
            if (result == null)
            {
                packet.Encode(new ServerLogin { Result = ServerLogin.LoginResult.FailedWrongInfo });
                session.Send(packet);
                return;
            }

            session.Name = request.UserName;
            session.IsAdmin = (byte)result.admin == 1;
            packet.Encode(new ServerLogin { Result = ServerLogin.LoginResult.Success });
            session.Send(packet);

            packet = new OutPacket(ServerHeader.ServerMessage);
            var serverMessage = new ServerMessage { Message = $"{session.Name}이(가) 채팅방에 입장하였습니다." };

            packet.Encode(serverMessage);
            packet.WriteLength();

            foreach (var client in ChatServer.Sessions.Values.Where(x => x.Name != ""))
            {
                client.Send(packet, false);
            }

            packet.Dispose();
        }
        catch (Exception ex)
        {
            packet.Encode(new ServerLogin { Result = ServerLogin.LoginResult.FailedUnkown });
            session.Send(packet);
            LogManager.GetExceptionLogger().Error(ex);
        }
    }
}