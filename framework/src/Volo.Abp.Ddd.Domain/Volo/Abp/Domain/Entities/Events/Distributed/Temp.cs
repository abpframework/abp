using System;
using System.Threading.Tasks;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.Domain.Entities.Events.Distributed
{
    /*
    [EventName("Volo.Abp.Identity.Users.IdentityUser")]
    public class UserEto : EtoBase
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
    }

    //Event name (default): [Volo.Abp.Identity.Users.IdentityUser].Created/Updated/Deleted

    public class MyEventHandler : IDistributedEventHandler<EntityCreatedEventInfo<Temp>>
    {
        //Event name: Volo.Abp.Identity.Users.IdentityUser.Created
        public Task HandleEventAsync(EntityCreatedEventInfo<Temp> eventData)
        {
            throw new NotImplementedException();
        }
    }

    */
}