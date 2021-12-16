﻿using ChatApp.Client.Data;
using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;
using System.Net.Sockets;
using TcpClient = NetCoreServer.TcpClient;

namespace ChatApp.Client.Net;

internal class ChatClient : TcpClient
{
    private static readonly Lazy<ChatClient> Lazy = new(() => new());

    private byte[] _sendKey = null!;
    private byte[] _recvKey = null!;
    private bool _initialized;

    internal static ChatClient Instance => Lazy.Value;

    internal ChatClient() : base(Setting.Value.ServerAddress, Setting.Value.ServerPort) { }

    internal void DisconnectAndStop()
    {
        DisconnectAsync();
        while (IsConnected) Thread.Yield();
    }

    protected override void OnConnected()
    {
        Console.WriteLine($"ChatSession이 Id {Id}로 생성됨.");
    }

    protected override void OnDisconnected()
    {
        Console.WriteLine($"ChatSession {Id}와 연결이 끊어짐.");
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"ChatSession({Id}) 오류 발생 {error}");
        DisconnectAndStop();
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        if (size >= 8)
        {
            if (_initialized)
            {
                using var packet = new InPacket(Decrypt(buffer, (int)size), (int)size);
                var headerName = Enum.GetName(typeof(ServerHeader), packet.Header) ?? string.Format($"0x{packet.Header:X4}");
                Console.WriteLine($"[S->C] [{headerName}]\r\n{packet}");
            }
            else
            {
                using var packet = new InPacket(buffer, (int)size);
                if (packet.Header == (uint)ServerHeader.ServerHandshake)
                {
                    var handshake = packet.Decode<ServerHandshake>();

                    _sendKey = BitConverter.GetBytes(handshake.SendIv);
                    _recvKey = BitConverter.GetBytes(handshake.RecieveIv);
                    _initialized = true;
                }
                else
                {
                    Console.WriteLine("알 수 없는 핸드셰이크 패킷을 수신함.");
                    DisconnectAndStop();
                }
            }
        }
        else
        {
            Console.WriteLine("패킷 사이즈가 너무 작음.");
            DisconnectAndStop();
        }
    }

    internal void Send(OutPacket buffer)
    {
        var headerName = Enum.GetName(typeof(ServerHeader), buffer.Header) ?? string.Format($"0x{buffer.Header:X4}");

        buffer.WriteLength();
        base.SendAsync(Encrypt(buffer.Buffer));
        Console.WriteLine($"[C->S] [{headerName}]\r\n{buffer}");
        buffer.Dispose();
    }

    private byte[] Encrypt(IReadOnlyList<byte> buffer)
    {
        var result = new byte[buffer.Count];

        for (var i = 0; i < buffer.Count; i++)
        {
            result[i] = (byte)(buffer[i] ^ _sendKey[i % 8]);
        }

        return result;
    }

    private byte[] Decrypt(IReadOnlyList<byte> buffer, int size)
    {
        var result = new byte[size];

        for (var i = 0; i < size; i++)
        {
            result[i] = (byte)(buffer[i] ^ _recvKey[i % 8]);
        }

        return result;
    }
}