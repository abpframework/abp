using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities
{
    public interface IGeneratesDomainEvents
    {
        ICollection<object> DomainEvents { get; }
    }
}
