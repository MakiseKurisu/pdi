namespace pdi.Assets
{
    /// <summary>
    /// Define a physical host on the network that can be accessed via SSH
    /// </summary>
    interface IHost
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
        /// Port for SSH
        /// </summary>
        public int Port { get; init; }

        /// <summary>
        /// Management user name
        /// </summary>
        public string UserName { get; init; }

        /// <summary>
        /// Management user password
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// Management user SSH private key
        /// </summary>
        public string PrivateKey { get; init; }

        /// <summary>
        /// Management user SSH private key's password
        /// </summary>
        public string PrivateKeyPassword { get; init; }
    }
}
