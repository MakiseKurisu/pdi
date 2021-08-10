using Xunit;

namespace pdi.Assets.Tests
{
    public class HostRecordTests
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
    }
}
