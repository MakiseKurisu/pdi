using System.Threading.Tasks;

namespace pdi.Assets
{
    public interface IHost
    {
        public void Connect();
        public void Disconnect();
        public Task<(int ExitStatus, string Result, string Error)> Execute(string commandText);
    }
}
