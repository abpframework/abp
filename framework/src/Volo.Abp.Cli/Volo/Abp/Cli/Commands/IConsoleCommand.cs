using System.Threading.Tasks;

namespace Volo.Abp.Cli
{
    public interface IConsoleCommand
    {
        Task ExecuteAsync();
    }
}