namespace pdi.Assets
{
    /// <summary>
    /// Define a physical host on the network
    /// </summary>
    public record HostRecord
    {
        /// <summary>
        /// Name of the host
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// IP / DNS name of the host
        /// </summary>
        public string Address { get; init; }

        /// <summary>
        /// Management user name
        /// </summary>
        public string UserName { get; init; }

        /// <summary>
        /// Management user password
        /// </summary>
        public string Password { get; init; }

        public HostRecord()
        {
            Name = string.Empty;
            Address = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
        }

        public HostRecord(HostRecord i)
        {
            Name = i.Name;
            Address = i.Address;
            UserName = i.UserName;
            Password = i.Password;
        }
    }

    /// <summary>
    /// Define a physical host on the network that can be accessed via SSH
    /// </summary>
    public record SshHostRecord : HostRecord
    {
        /// <summary>
        /// Port for SSH
        /// </summary>
        public int Port { get; init; }

        /// <summary>
        /// Management user SSH private key
        /// </summary>
        public string PrivateKey { get; init; }

        /// <summary>
        /// Management user SSH private key's password
        /// </summary>
        public string PrivateKeyPassword { get; init; }

        public SshHostRecord() : base()
        {
            Port = 22;
            PrivateKey = string.Empty;
            PrivateKeyPassword = string.Empty;
        }

        public SshHostRecord(HostRecord i) : base(i)
        {
            if (i is SshHostRecord j)
            {
                Port = j.Port;
                PrivateKey = j.PrivateKey;
                PrivateKeyPassword = j.PrivateKeyPassword;
            }
            else
            {
                Port = 22;
                PrivateKey = string.Empty;
                PrivateKeyPassword = string.Empty;
            }
        }
    }
}
