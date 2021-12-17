using ChatApp.Client.Data.WaitableJobs;
using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;

namespace ChatApp.Client.Net.Handlers;

[PacketHandler(ServerHeader.ServerLogin)]
internal class LoginHandler : AbstractHandler
{
    internal override void Handle(ChatClient session, InPacket packet)
    {
        var response = packet.Decode<ServerLogin>();

        Constants.LoginResult.Set(new LoginResult() { Result = response.Result });
    }
}