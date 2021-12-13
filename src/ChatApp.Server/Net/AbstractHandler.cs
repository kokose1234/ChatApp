using ChatApp.Common.Net.Packet;

namespace ChatApp.Server.Net;

internal abstract class AbstractHandler
{
    internal abstract void Handle(ChatSession session, InPacket packet);
}