using Microsoft.Extensions.Configuration;

using System;

using Xunit;

namespace pdi.Assets.Tests
{
    public class HostTests
    {
        [Fact]
        public void SshHostRecordTest()
        {
            var i = new SshHostRecord();
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
            var i = new SshHostRecord();
            using var s = new SshHost(i);
            Assert.Throws<InvalidOperationException>(() => s.Connect());
        }

        [Fact]
        public void ConnectPasswordTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            var i = new SshHostRecord()
            {
                Address = secrets["SSH_ADDRESS"],
                Port = Convert.ToInt32(secrets["SSH_PORT"]),
                UserName = secrets["SSH_USERNAME"],
                Password = secrets["SSH_PASSWORD"]
            };

            using var s = new SshHost(i);
            s.Connect();
            s.Disconnect();
        }

        [Fact(Skip = "Current private key is not supported in SSH.NET 2020.0.1. See https://github.com/sshnet/SSH.NET/pull/614")]
        public void ConnectPrivateKeyTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            var i = new SshHostRecord()
            {
                Address = secrets["SSH_ADDRESS"],
                Port = Convert.ToInt32(secrets["SSH_PORT"]),
                UserName = secrets["SSH_USERNAME"],
                PrivateKey = secrets["SSH_PRIVATE_KEY"],
                PrivateKeyPassword = secrets["SSH_PRIVATE_KEY_PASSWORD"]
            };

            using var s = new SshHost(i);
            s.Connect();
            s.Disconnect();
        }

        [Theory]
        [InlineData("echo 0", 0, "0\n", "")]
        [InlineData("echo 0 > /test", 1, "", "bash: /test: Permission denied\n")]
        public async void ExecuteTest(string commandText, int? expectedExitStatus, string? expectedResult, string? expectedError)
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            var i = new SshHostRecord()
            {
                Address = secrets["SSH_ADDRESS"],
                Port = Convert.ToInt32(secrets["SSH_PORT"]),
                UserName = secrets["SSH_USERNAME"],
                Password = secrets["SSH_PASSWORD"]
            };

            using var s = new SshHost(i);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await s.Execute(commandText));

            s.Connect();

            (var ExitStatus, var Result, var Error) = await s.Execute(commandText);
            Assert.Equal(expectedExitStatus ?? ExitStatus, ExitStatus);
            Assert.Equal(expectedResult ?? Result, Result);
            Assert.Equal(expectedError ?? Error, Error);

            s.Disconnect();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await s.Execute(commandText));
        }
    }
}