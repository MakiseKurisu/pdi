using pdi.Asset;

using System;

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
        }
    }
}