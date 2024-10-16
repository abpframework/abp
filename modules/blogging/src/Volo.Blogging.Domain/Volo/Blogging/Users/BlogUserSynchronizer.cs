using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace Volo.Blogging.Users
{
    public class BlogUserSynchronizer : EntitySynchronizer<BlogUser, Guid, UserEto>
    {
        public BlogUserSynchronizer([NotNull] IObjectMapper objectMapper,
            [NotNull] IRepository<BlogUser, Guid> repository) : base(objectMapper, repository)
        {
        }

        protected override Task<BlogUser> MapToEntityAsync(UserEto eto)
        {
            return Task.FromResult(new BlogUser(eto));
        }

        protected override Task MapToEntityAsync(UserEto eto, BlogUser localEntity)
        {
            localEntity.Update(eto);

            return Task.CompletedTask;
        }
    }
}