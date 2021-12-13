using ChatApp.Client.Net;

namespace ChatApp.Client;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        PacketHandlers.RegisterPackets();
        _ = ChatClient.Instance;

        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}