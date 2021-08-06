using Microsoft.Extensions.Configuration;

using System;

using Xunit;

namespace pdi.Assets.Tests
{
    public class HostTests
    {
        [Fact]
        public void HostTest()
        {
            using var i = new Host();
            Assert.Equal(string.Empty, i.Name);
            Assert.Equal(string.Empty, i.Address);
            Assert.Equal(22, i.Port);
            Assert.Equal(string.Empty, i.UserName);
            Assert.Equal(string.Empty, i.Password);
            Assert.Equal(string.Empty, i.PrivateKey);
            Assert.Equal(string.Empty, i.PrivateKeyPassword);
        }

        [Fact]
        public void ConnectExceptionTest()
        {
            using var i = new Host();
            Assert.Throws<InvalidOperationException>(() => i.Connect());
        }

        [Fact]
        public void ConnectPasswordTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            using var i = new Host()
            {
                Address = secrets["SSH_ADDRESS"],
                Port = Convert.ToInt32(secrets["SSH_PORT"]),
                UserName = secrets["SSH_USERNAME"],
                Password = secrets["SSH_PASSWORD"]
            };

            i.Connect();
            i.Disconnect();
        }

        [Fact(Skip = "Current private key is not supported in SSH.NET 2020.0.1. See https://github.com/sshnet/SSH.NET/pull/614")]
        public void ConnectPrivateKeyTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            using var i = new Host()
            {
                Address = secrets["SSH_ADDRESS"],
                Port = Convert.ToInt32(secrets["SSH_PORT"]),
                UserName = secrets["SSH_USERNAME"],
                PrivateKey = secrets["SSH_PRIVATE_KEY"],
                PrivateKeyPassword = secrets["SSH_PRIVATE_KEY_PASSWORD"]
            };

            i.Connect();
            i.Disconnect();
        }
    }
}