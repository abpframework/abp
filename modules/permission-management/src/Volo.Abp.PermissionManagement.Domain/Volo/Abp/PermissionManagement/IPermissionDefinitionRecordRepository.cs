using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.PermissionManagement;

public interface IPermissionDefinitionRecordRepository : IBasicRepository<PermissionDefinitionRecord, Guid>
{
    
}