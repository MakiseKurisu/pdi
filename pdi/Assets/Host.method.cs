using pdi.Extensions;

using Renci.SshNet;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace pdi.Assets
{
    public partial class Host : IDisposable
    {
        private bool Disposed { get; set; }

        private SshClient Ssh { get; set; }

        public Host()
        {
            Port = 22;
        }

        ~Host()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    Ssh?.Dispose();
                }

                Disposed = true;
            }
        }

        public void Connect()
        {
            _ = Address ?? throw new ArgumentNullException(nameof(Address));
            _ = UserName ?? throw new ArgumentNullException(nameof(UserName));

            if (Ssh?.IsConnected ?? false)
            {
                return;
            }

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

            Ssh = new SshClient(connInfo);
            Ssh.Connect();
        }

        public void Disconnect()
        {
            if (Ssh?.IsConnected ?? false)
            {
                return;
            }

            Ssh.Disconnect();
        }

        public async Task<(int ExitStatus, string Result, string Error)> Execute(string commandText)
        {
            using var c = Ssh.CreateCommand(commandText);
            if (await c.BeginExecute())
            {
                return (c.ExitStatus, c.Result, c.Error);
            }
            else
            {
                throw new TimeoutException($"Execution of `commandText` has been timed out.");
            }
        }
    }
}
