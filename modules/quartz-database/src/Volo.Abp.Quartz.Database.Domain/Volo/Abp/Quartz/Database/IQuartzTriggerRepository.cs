using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Quartz.Database
{
    public interface IQuartzTriggerRepository : IReadOnlyRepository<QuartzTrigger>
    {
    }
}
