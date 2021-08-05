namespace pdi.Asset
{
    /// <summary>
    /// Define a physical host on the network that can be accessed via SSH
    /// </summary>
    interface IHost
    {
        /// <summary>
        /// Name of the host
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IP / DNS name of the host
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Port for SSH
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Management user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Management user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Management user SSH private key
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Management user SSH private key's password
        /// </summary>
        public string PrivateKeyPassword { get; set; }
    }
}
