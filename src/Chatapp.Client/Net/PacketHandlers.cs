using ChatApp.Common.Net.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChatApp.Client.Net;

internal static class PacketHandlers
{
    private static readonly Dictionary<uint, AbstractHandler?> Packets = new();

    internal static void RegisterPackets()
    {
        var handlers = Assembly.GetExecutingAssembly().GetTypes()
                               .Where(type => Attribute.IsDefined(type, typeof(PacketHandler)))
                               .ToDictionary(type => ((PacketHandler)type.GetCustomAttributes(typeof(PacketHandler), true).FirstOrDefault()!).Header,
                                   type => (AbstractHandler)Activator.CreateInstance(type)!);

        foreach (var (packetId, handler) in handlers.Where(kvp => !Packets.ContainsKey(kvp.Key)))
        {
            Packets.Add(packetId, handler);
        }
    }

    internal static bool GetHandler(uint header, out AbstractHandler? handler) => Packets.TryGetValue(header, out handler);
}