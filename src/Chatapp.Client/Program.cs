using ChatApp.Client.Data;
using ChatApp.Client.Data.WaitableJobs;
using ChatApp.Client.Net;
using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Client;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Net.Packet.Header;
using Spectre.Console;

namespace ChatApp.Client;

internal static class Program
{
    internal static Chat Chat
    {
        get => default;
        set
        {
        }
    }

    internal static LoginResult LoginResult
    {
        get => default;
        set
        {
        }
    }

    internal static WaitableResult<object> WaitableResult
    {
        get => default;
        set
        {
        }
    }

    internal static ChatClient ChatClient
    {
        get => default;
        set
        {
        }
    }

    internal static Net.Handlers.LoginHandler LoginHandler
    {
        get => default;
        set
        {
        }
    }

    internal static Net.Handlers.AccountRegisterHandler AccountRegisterHandler
    {
        get => default;
        set
        {
        }
    }

    private static void Main(string[] args)
    {
        Console.Title = "Login Window";
        PacketHandlers.RegisterPackets();
        AnsiConsole.Status()
                   .Start("[green]채팅 서버에 연결중...[/]", _ =>
                   {
                       while (!ChatClient.Instance.IsConnected) Thread.Sleep(1000);
                   });
        
        ShowMainMenu();
    }

    private static void ShowMainMenu(string print = "")
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("ChatApp").LeftAligned().Color(Color.Blue));
        if (print != "") AnsiConsole.Write(new Markup($"{print}\r\n\r\n"));

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("메뉴를 선택해 주세요")
                .PageSize(3)
                .AddChoices("로그인", "회원가입", "종료")
        );

        switch (selection)
        {
            case "로그인":
                TryLogin().Wait();
                break;
            case "회원가입":
                TryRegister().Wait();
                break;
            case "종료":
                return;
        }
    }

    private static async Task TryLogin()
    {
        var id = AnsiConsole.Ask<string>("[green]Id[/]:");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("[red]비밀번호[/]:")
                .PromptStyle("red")
                .Secret());

        using var packet = new OutPacket(ClientHeader.ClientLogin);
        var request = new ClientLogin { UserName = id, Password = password };

        packet.Encode(request);
        packet.WriteLength();

        var result = await GetLoginResult(packet);
        switch (result.Result)
        {
            case ServerLogin.LoginResult.Success:
                var chat = new Chat(id);
                chat.Start();
                return;
            case ServerLogin.LoginResult.FailedWrongInfo:
                ShowMainMenu("[red]아이디 또는 비밀번호가 올바르지 않습니다.[/]");
                break;
            case ServerLogin.LoginResult.FailedUnkown:
                ShowMainMenu("[red]알수없는 오류로 로그인에 실패하였습니다.[/]");
                break;
        }
    }

    private static async Task TryRegister()
    {
        var id = AnsiConsole.Ask<string>("[green]Id[/]를 입력해 주세요:");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("[red]비밀번호[/]를 입력해 주세요:")
                .PromptStyle("red")
                .Secret());
        using var packet = new OutPacket(ClientHeader.ClientAccountRegister);
        var request = new ClientAccountRegister { UserName = id, Password = password };

        packet.Encode(request);
        packet.WriteLength();

        var result = await GetRegisterResult(packet);
        switch (result.Result)
        {
            case ServerAccountRegister.RegisterResult.Success:
                ShowMainMenu("[green]회원가입에 성공하였습니다.[/]");
                break;
            case ServerAccountRegister.RegisterResult.FailDuplicateUsesrname:
                ShowMainMenu("[red]이미 사용중인 Id입니다.[/]");
                break;
            case ServerAccountRegister.RegisterResult.FailUnkown:
                ShowMainMenu("[red]알수없는 오류로 회원가입에 실패하였습니다.[/]");
                break;
        }
    }

    private static async Task<AccountRegisterResult> GetRegisterResult(OutPacket packet)
    {
        return await Task.Run(() =>
        {
            var result = new WaitableResult<AccountRegisterResult>();

            Constants.RegisterResult = result;
            ChatClient.Instance.Send(packet);
            result.Wait();

            return result.Value;
        });
    }

    private static async Task<LoginResult> GetLoginResult(OutPacket packet)
    {
        return await Task.Run(() =>
        {
            var result = new WaitableResult<LoginResult>();

            Constants.LoginResult = result;
            ChatClient.Instance.Send(packet);
            result.Wait();

            return result.Value;
        });
    }
}