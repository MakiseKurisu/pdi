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

        private SshClient? Ssh { get; set; }

        public Host()
        {
            Name = string.Empty;
            Address = string.Empty;
            Port = 22;
            UserName = string.Empty;
            Password = string.Empty;
            PrivateKey = string.Empty;
            PrivateKeyPassword = string.Empty;
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
            if (Ssh?.IsConnected ?? false)
            {
                return;
            }

            ConnectionInfo connInfo;
            if (PrivateKey != string.Empty)
            {
                using var p = new MemoryStream(Encoding.UTF8.GetBytes(PrivateKey));
                connInfo = new PrivateKeyConnectionInfo(Address, Port, UserName, new PrivateKeyFile(p, PrivateKeyPassword));
            }
            else if (Password != string.Empty)
            {
                connInfo = new PasswordConnectionInfo(Address, Port, UserName, Password);
            }
            else
            {
                throw new InvalidOperationException($"Need to define either {nameof(PrivateKey)} or {nameof(Password)} for SSH authentication.");
            }

            Ssh = new SshClient(connInfo);
            Ssh.Connect();
        }

        public void Disconnect()
        {
            if (Ssh is null || !Ssh.IsConnected)
            {
                return;
            }

            Ssh.Disconnect();
        }

        public async Task<(int ExitStatus, string Result, string Error)> Execute(string commandText)
        {
            if (Ssh is null || !Ssh.IsConnected)
            {
                throw new InvalidOperationException($"Host {Name} is not connected.");
            }

            using var c = Ssh.CreateCommand(commandText);
            if (await c.BeginExecute())
            {
                return (c.ExitStatus, c.Result, c.Error);
            }
            else
            {
                throw new TimeoutException($"Execution of `{commandText}` has been timed out.");
            }
        }
    }
}
