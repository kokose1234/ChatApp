using ChatApp.Common.Net.Packet.Data.Server;

namespace ChatApp.Client.Data.WaitableJobs;

internal sealed record AccountRegisterResult
{
    internal ServerAccountRegister.RegisterResult Result { get; init; }
}