using System.Collections.Generic;

namespace Volo.Abp.Logging;

public interface IInitLoggerFactory
{
    IInitLogger<T> Create<T>();

    List<AbpInitLogEntry> GetAllEntries();

    void ClearAllEntries();
}
