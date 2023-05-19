using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

public interface IBackgroundJobExecuter
{
    Task ExecuteAsync(JobExecutionContext context);
}
