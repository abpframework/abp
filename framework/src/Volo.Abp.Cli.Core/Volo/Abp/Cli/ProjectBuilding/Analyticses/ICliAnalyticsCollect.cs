using System.Threading.Tasks;

namespace Volo.Abp.Cli.ProjectBuilding.Analyticses
{
    public interface ICliAnalyticsCollect
    {
        Task CollectAsync(CliAnalyticsCollectInputDto input);
    }
}
