using ChatApp.Client.Net;
using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Header;
using Spectre.Console;

namespace ChatApp.Client;

internal class Chat
{
    internal static Chat? Instance { get; private set; } = null;

    internal ChatClient ChatClient
    {
        get => default;
        set
        {
        }
    }

    private readonly string _name;

    internal Chat(string name)
    {
        Instance = this;
        _name = name;
    }

    internal void Start()
    {
        Console.Title = $"Chat Window ({_name})";
        AnsiConsole.Clear();

        while (true)
        {
            var message = GetMessage();

            if (!string.IsNullOrWhiteSpace(message) && !string.IsNullOrEmpty(message))
            {
                using var packet = new OutPacket(ClientHeader.ClientChat);
                var request = ObjectPoolManager.ChatPool.Get();
                request.Author = _name;
                request.Message = message;

                packet.Encode(request);
                packet.WriteLength();

                ChatClient.Instance.Send(packet);

                ObjectPoolManager.ChatPool.Return(request);
            }
        }
    }

    internal void AddChat(string name, string message) => Console.WriteLine($"{name} : {message}");

    internal void AddSystemMessage(string message) => AnsiConsole.Write(new Rule($"[red]{message}[/]\r\n"));

    private string GetMessage()
    {
        var message = Console.ReadLine();
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        var currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        for (var i = 0; i < Console.WindowWidth; i++)
            Console.Write(" ");
        Console.SetCursorPosition(0, currentLineCursor);

        return message;
    }
}