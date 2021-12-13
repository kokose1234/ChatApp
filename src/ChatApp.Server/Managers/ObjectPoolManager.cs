using ChatApp.Common.Net.Packet.Data.Client;
using ChatApp.Common.Tools;

namespace ChatApp.Server.Managers;

internal static class ObjectPoolManager
{
    internal static readonly ObjectPool<ClientAccountRegister> Test = new(() => new ());
}