using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Users;

public class CmsUserSynchronizer : EntitySynchronizer<CmsUser, Guid, UserEto>
{
    public CmsUserSynchronizer([NotNull] IObjectMapper objectMapper, [NotNull] IRepository<CmsUser, Guid> repository) :
        base(objectMapper, repository)
    {
        if (!GlobalFeatureManager.Instance.IsEnabled<CmsUserFeature>())
        {
            IgnoreEntityCreatedEvent = true;
            IgnoreEntityUpdatedEvent = true;
            IgnoreEntityDeletedEvent = true;
        }
    }

    protected override Task<CmsUser> MapToEntityAsync(UserEto eto)
    {
        return Task.FromResult(new CmsUser(eto));
    }

    protected override Task MapToEntityAsync(UserEto eto, CmsUser localEntity)
    {
        localEntity.Update(eto);

        return Task.CompletedTask;
    }
}