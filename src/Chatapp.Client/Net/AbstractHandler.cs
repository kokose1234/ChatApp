using ChatApp.Common.Net.Packet;

namespace ChatApp.Client.Net;

internal abstract class AbstractHandler
{
    internal abstract void Handle(ChatClient session, InPacket packet);
}