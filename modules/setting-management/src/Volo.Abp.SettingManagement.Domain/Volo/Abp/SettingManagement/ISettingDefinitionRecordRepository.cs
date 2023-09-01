using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.SettingManagement;

public interface ISettingDefinitionRecordRepository : IBasicRepository<SettingDefinitionRecord, Guid>
{
    Task<SettingDefinitionRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
