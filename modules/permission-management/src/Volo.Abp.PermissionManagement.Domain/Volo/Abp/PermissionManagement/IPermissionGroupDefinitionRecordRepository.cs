using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.PermissionManagement;

public interface IPermissionGroupDefinitionRecordRepository : IBasicRepository<PermissionGroupDefinitionRecord, Guid>
{
    
}