using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Logging
{
    public interface IInitLogger<out T> : ILogger<T>
    {
        public List<AbpInitLogEntry> Entries { get; }
    }
}
