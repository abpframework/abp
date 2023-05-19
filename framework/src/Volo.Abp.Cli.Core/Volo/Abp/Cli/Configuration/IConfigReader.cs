using System.Threading.Tasks;

namespace Volo.Abp.Cli.Configuration;

public interface IConfigReader
{
    AbpCliConfig Read(string directory);
}
