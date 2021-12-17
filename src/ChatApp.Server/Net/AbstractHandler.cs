using ChatApp.Common.Net.Packet;

namespace ChatApp.Server.Net;

public abstract class AbstractHandler
{
    internal abstract void Handle(ChatSession session, InPacket packet);
}