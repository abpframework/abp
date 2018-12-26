using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Testing.Utils;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class DistributedUserUpdateHandler : IDistributedEventHandler<EntityUpdatedEto<UserEto>>, ITransientDependency
    {
        private readonly ITestCounter _testCounter;

        public DistributedUserUpdateHandler(ITestCounter testCounter)
        {
            _testCounter = testCounter;
        }

        public Task HandleEventAsync(EntityUpdatedEto<UserEto> eventData)
        {
            if (eventData.Entity.UserName == "john.nash")
            {
                _testCounter.Increment("EntityUpdatedEto<UserEto>");
            }

            return Task.CompletedTask;
        }
    }
}