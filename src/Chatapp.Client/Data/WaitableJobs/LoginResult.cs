using ChatApp.Common.Net.Packet.Data.Server;

namespace ChatApp.Client.Data.WaitableJobs;

internal class LoginResult
{
    internal ServerLogin.LoginResult Result { get; init; }
}