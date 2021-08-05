using Microsoft.Extensions.Configuration;

using Xunit;

namespace pdi.Asset.Tests
{
    public class HostTests
    {
        [Fact]
        public void HostTest()
        {
            var i = new Host();
            Assert.Null(i.Name);
            Assert.Null(i.Address);
            Assert.Equal(22, i.Port);
            Assert.Null(i.UserName);
            Assert.Null(i.Password);
            Assert.Null(i.PrivateKey);
            Assert.Null(i.PrivateKeyPassword);
        }

        [Fact]
        public void ConnectPasswordTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            var i = new Host()
            {
                Address = secrets["SSH_ADDRESS"],
                UserName = secrets["SSH_USERNAME"],
                Password = secrets["SSH_PASSWORD"]
            };

            i.Connect();
        }

        [Fact(Skip = "Current private key is not supported in SSH.NET 2020.0.1. See https://github.com/sshnet/SSH.NET/pull/614")]
        public void ConnectPrivateKeyTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            var i = new Host()
            {
                Address = secrets["SSH_ADDRESS"],
                UserName = secrets["SSH_USERNAME"],
                PrivateKey = secrets["SSH_PRIVATE_KEY"],
                PrivateKeyPassword = secrets["SSH_PRIVATE_KEY_PASSWORD"]
            };

            i.Connect();
        }
    }
}