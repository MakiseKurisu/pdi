using Renci.SshNet;

using System;
using System.IO;
using System.Text;

namespace pdi.Asset
{
    public partial class Host
    {
        public Host()
        {
            Port = 22;
        }

        public void Connect()
        {
            _ = Address ?? throw new ArgumentNullException(nameof(Address));
            _ = UserName ?? throw new ArgumentNullException(nameof(UserName));

            ConnectionInfo connInfo;
            if (PrivateKey is not null)
            {
                using var p = new MemoryStream(Encoding.UTF8.GetBytes(PrivateKey));
                connInfo = new PrivateKeyConnectionInfo(Address, Port, UserName, new PrivateKeyFile(p, PrivateKeyPassword));
            }
            else if (Password is not null)
            {
                connInfo = new PasswordConnectionInfo(Address, Port, UserName, Password);
            }
            else
            {
                throw new ArgumentNullException(nameof(PrivateKey));
            }

            using var client = new SshClient(connInfo);
            client.Connect();
            client.Disconnect();
        }
    }
}
