using NLog;

namespace ChatApp.Server.Managers;

internal static class LogManager
{
    private static readonly Logger Logger = NLog.LogManager.GetLogger("general_logger");
    private static readonly Logger PacketLogger = NLog.LogManager.GetLogger("packet_logger");
    private static readonly Logger ErrorLogger = NLog.LogManager.GetLogger("exceiption_logger");


    static LogManager() => NLog.LogManager.LoadConfiguration("NLog.config");

    internal static Logger GetLogger() => Logger;

    internal static Logger GetPacketLogger() => PacketLogger;

    internal static Logger GetExceptionLogger() => ErrorLogger;
}