using ChatApp.Common.Net.Packet;
using ChatApp.Common.Net.Packet.Data.Client;
using ChatApp.Common.Net.Packet.Header;
using ChatApp.Common.Tools;
using Xunit;

namespace ChatApp.Test
{
    public class ObjectPoolTest
    {
        private static readonly ObjectPool<ClientAccountRegister> Pool = new(() => new());

        [Fact]
        public void TestMessageObjectPooling()
        {
            var exception = Record.Exception(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    using var packet = new OutPacket(ClientHeader.ClientAccountRegister);
                    var message = Pool.Get();
                    message.UserName = $"test-username-{i}";
                    message.Password = $"test-password-{i}";
                    message.MacAddress = $"test-mac-{i}";

                    packet.Encode(message);
                    Pool.Return(message);
                    _ = packet.Buffer;
                }
            });

            Assert.Null(exception);
        }
    }
}