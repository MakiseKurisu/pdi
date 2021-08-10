using pdi.Extensions;

using Renci.SshNet;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace pdi.Assets
{
    public class SshHost : IDisposable, IHost
    {
        public SshHostRecord Host { get; init; }
        private bool Disposed { get; set; }

        private SshClient? Ssh { get; set; }

        public SshHost(SshHostRecord Record)
        {
            Host = Record;
        }

        ~SshHost()
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
            if (Host.PrivateKey != string.Empty)
            {
                using var p = new MemoryStream(Encoding.UTF8.GetBytes(Host.PrivateKey));
                connInfo = new PrivateKeyConnectionInfo(Host.Address, Host.Port, Host.UserName, new PrivateKeyFile(p, Host.PrivateKeyPassword));
            }
            else if (Host.Password != string.Empty)
            {
                connInfo = new PasswordConnectionInfo(Host.Address, Host.Port, Host.UserName, Host.Password);
            }
            else
            {
                throw new InvalidOperationException($"Need to define either {nameof(Host.PrivateKey)} or {nameof(Host.Password)} for SSH authentication.");
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
                throw new InvalidOperationException($"Host {Host.Name} is not connected.");
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
