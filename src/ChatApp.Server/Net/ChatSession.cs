using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;
using ChatApp.Common.Tools;
using NetCoreServer;
using NLog;
using System.Net;
using System.Net.Sockets;
using LogManager = ChatApp.Server.Managers.LogManager;

namespace ChatApp.Server.Net;

internal class ChatSession : TcpSession
{
    private string _remoteEndpoint;
    private readonly byte[] _sendKey;
    private readonly byte[] _recvKey;

    public ChatSession(TcpServer server) : base(server)
    {
        _sendKey = Randomizer.NextBytes(8);
        _recvKey = Randomizer.NextBytes(8);
        _remoteEndpoint = Id.ToString();
    }

    protected override void OnConnected()
    {
        _remoteEndpoint = (Socket.RemoteEndPoint as IPEndPoint)?.Address.ToString()!;
        using var packet = new OutPacket((uint)ServerHeader.ServerHandshake);
        packet.Encode(GetHandshake());
        packet.WriteLength();

        base.SendAsync(packet.Buffer, 0, packet.Length);
        ChatServer.Sessions.TryAdd(Id.ToString(), this);
        LogManager.GetLogger().Log(LogLevel.Info, $"{_remoteEndpoint}가 연결됨");
    }

    protected override void OnDisconnected()
    {
        LogManager.GetLogger().Log(LogLevel.Info, $"{_remoteEndpoint}가 연결 해제됨");
        ChatServer.Sessions.TryRemove(Id.ToString(), out _);
    }

    protected override void OnError(SocketError error)
    {
        LogManager.GetLogger().Log(LogLevel.Error, $"ChatSession({_remoteEndpoint}) 오류 발생 {error}");
        Disconnect();
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        if (size >= 8)
        {
            using var packet = new InPacket(Decrypt(buffer, (int)size), (int)size);
            var headerName = Enum.GetName(typeof(ServerHeader), packet.Header) ?? string.Format($"0x{packet.Header:X4}");
            LogManager.GetPacketLogger().Info($"[C->S] [{headerName}]\r\n{packet}");

            if (PacketHandlers.GetHandler(packet.Header, out var handler))
            {
                if (handler != null)
                {
                    handler.Handle(this, packet);
                }
                else
                {
                    LogManager.GetLogger().Log(LogLevel.Error, $"패킷 Id {packet.Header}의 패킷 핸들러가 존재하지 않음.");
                }
            }
            else
            {
                LogManager.GetLogger().Log(LogLevel.Error, "알 수 없는 패킷을 수신함.");
                Disconnect();
            }
        }
        else
        {
            LogManager.GetLogger().Log(LogLevel.Error, $"패킷 사이즈가 너무 작음. 크기: {size}");
            Disconnect();
        }
    }

    internal void Send(OutPacket buffer)
    {
        var headerName = Enum.GetName(typeof(ServerHeader), buffer.Header) ?? string.Format($"0x{buffer.Header:X4}");

        buffer.WriteLength();
        base.SendAsync(Encrypt(buffer.Buffer), 0, buffer.Length);
        LogManager.GetPacketLogger().Info($"[S->C] [{headerName}]\r\n{buffer}");
        buffer.Dispose();
    }

    private byte[] Encrypt(IReadOnlyList<byte> buffer)
    {
        var result = new byte[buffer.Count];

        for (var i = 0; i < buffer.Count; i++)
        {
            result[i] = (byte)(buffer[i] ^ _recvKey[i % 8]);
        }

        return result;
    }

    private byte[] Decrypt(IReadOnlyList<byte> buffer, int size)
    {
        var result = new byte[size];

        for (var i = 0; i < size; i++)
        {
            result[i] = (byte)(buffer[i] ^ _sendKey[i % 8]);
        }

        return result;
    }

    private ServerHandshake GetHandshake()
    {
        return new()
        {
            SendIv = BitConverter.ToUInt64(_sendKey),
            RecieveIv = BitConverter.ToUInt64(_recvKey)
        };
    }
}