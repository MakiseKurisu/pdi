using Microsoft.Extensions.Configuration;

using System;

using Xunit;

namespace pdi.Assets.Tests
{
    public class HostFactoryTests
    {
        [Fact]
        public async void ShellDetectionTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<SshHostTests>().Build();

            var i = new SshHostRecord()
            {
                Address = secrets["SSH_ADDRESS"],
                Port = Convert.ToInt32(secrets["SSH_PORT"]),
                UserName = secrets["SSH_USERNAME"],
                Password = secrets["SSH_PASSWORD"]
            };

            using var s = new SshHost(i);
            var f = new HostFactory();
            Assert.Equal(HOST_SHELL_TYPE.SH, await f.ShellDetection(s));
        }
    }
}