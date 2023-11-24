using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity;

public interface IIdentitySessionRepository : IBasicRepository<IdentitySession, Guid>
{

}
