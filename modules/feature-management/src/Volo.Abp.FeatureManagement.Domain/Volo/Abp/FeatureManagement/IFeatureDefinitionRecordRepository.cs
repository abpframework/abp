using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.FeatureManagement;

public interface IFeatureDefinitionRecordRepository : IBasicRepository<FeatureDefinitionRecord, Guid>
{

}
