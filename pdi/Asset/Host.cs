namespace pdi.Asset
{
    public partial class Host : IHost
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string PrivateKey { get; set; }
        public string PrivateKeyPassword { get; set; }
    }
}
