using ChatApp.Client.Data;
using ChatApp.Client.Data.WaitableJobs;
using ChatApp.Common.Net.Packet.Data.Server;

namespace ChatApp.Client;

internal static class Constants
{
    internal static WaitableResult<AccountRegisterResult> RegisterResult { get; set; }
    internal static WaitableResult<LoginResult> LoginResult { get; set; }
}