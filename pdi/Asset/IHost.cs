namespace pdi.Asset
{
    // Define a physical host on the network that can be accessed via SSH
    interface IHost
    {
        // Name of the host
        public string Name { get; set; }
        
        // IP / DNS name of the host
        public string Address { get; set; }

        // Port for SSH
        public int Port { get; set; }

        // Management user name
        public string UserName { get; set; }

        // Management user password
        public string Password { get; set; }

        // Management user SSH private key
        public string PrivateKey { get; set; }
    }
}
