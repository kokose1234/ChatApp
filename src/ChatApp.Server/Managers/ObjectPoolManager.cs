using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Tools;

namespace ChatApp.Server.Managers;

internal static class ObjectPoolManager
{
    internal static readonly ObjectPool<ServerChat> ChatPool = new(() => new ());
}