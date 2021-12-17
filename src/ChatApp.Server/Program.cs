using ChatApp.Server.Database;
using ChatApp.Server.Net;

namespace ChatApp.Server;

internal static class Program
{
    internal static ChatServer ChatServer
    {
        get => default;
        set
        {
        }
    }

    private static void Main(string[] args)
    {
        Console.Title = "Chat Server";
        var server = new ChatServer();
        PacketHandlers.RegisterPackets();
        DatabaseManager.Setup();
        server.Start();

        while (true)
        {
            //TODO: 서버 명령어 추가하기
            switch (Console.ReadLine() ?? "") { }
        }
    }
}