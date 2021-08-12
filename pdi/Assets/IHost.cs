using System.Threading.Tasks;

namespace pdi.Assets
{
    public interface IHost
    {
        public Task<(int ExitStatus, string Result, string Error)> Execute(string commandText);
    }
}
