using ProtoBuf;

namespace ChatApp.Common.Net.Packet;

public class InPacket : AbstractPacket
{
    public uint PacketLen { get; }
    public uint Header { get; }

    public InPacket(byte[] buffer, int size, bool readSize = true, bool readHeader = true)
    {
        stream = new MemoryStream(buffer, 0, size, false);

        if (readSize)
        {
            PacketLen = BitConverter.ToUInt32(buffer, Position);
            stream.Seek(4, SeekOrigin.Current);
        }

        if (readHeader)
        {
            Header = BitConverter.ToUInt32(buffer, Position);
            stream.Seek(4, SeekOrigin.Current);
        }
    }

    public void Skip(int count) => stream.Seek(count, SeekOrigin.Current);

    public T Decode<T>() => Serializer.Deserialize<T>(stream);
}