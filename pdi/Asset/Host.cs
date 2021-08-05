namespace pdi.Asset
{
    public partial class Host : IHost
    {
        public string Name { get; init; }
        public string Address { get; init; }
        public int Port { get; init; }

        public string UserName { get; init; }
        public string Password { get; init; }
        public string PrivateKey { get; init; }
        public string PrivateKeyPassword { get; init; }
    }
}
