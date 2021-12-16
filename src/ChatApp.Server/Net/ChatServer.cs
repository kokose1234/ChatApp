using System.Collections.Concurrent;
using System.Net;
using ChatApp.Server.Managers;
using NetCoreServer;
using System.Net.Sockets;
using LogLevel = NLog.LogLevel;

namespace ChatApp.Server.Net;

internal class ChatServer : TcpServer
{
    internal static ChatServer Instance { get; private set; } = null!;
    internal new static ConcurrentDictionary<string, ChatSession> Sessions { get; } = new();

    public ChatServer() : base(IPAddress.Any, 12000) => Instance = this;

    protected override TcpSession CreateSession() => new ChatSession(this);

    protected override void OnError(SocketError error)
    {
        LogManager.GetLogger().Log(LogLevel.Error, $"ChatServer 오류 발생 {error}");
    }
}