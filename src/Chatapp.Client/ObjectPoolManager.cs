using ChatApp.Common.Net.Packet.Data.Client;
using ChatApp.Common.Net.Packet.Data.Server;
using ChatApp.Common.Tools;

namespace ChatApp.Client;

internal static class ObjectPoolManager
{
    internal static readonly ObjectPool<ClientChat> ChatPool = new(() => new ());
}