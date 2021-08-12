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

        private SshClient Ssh { get; set; }

        public SshHost(SshHostRecord Record)
        {
            Host = Record;

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

        ~SshHost()
        {
            Ssh.Disconnect();
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
                    Ssh.Dispose();
                }

                Disposed = true;
            }
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
                throw new TimeoutException($"Execution of `{commandText}` has been timed out.");
            }
        }
    }
}
