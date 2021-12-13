using ChatApp.Server.Database;
using ChatApp.Server.Net;

namespace ChatApp.Server;

internal static class Program
{
    private static void Main(string[] args)
    {
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