using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities
{
    //TODO: Re-consider this interface

    public interface IGeneratesDomainEvents
    {
        IEnumerable<object> GetDomainEvents();

        void ClearDomainEvents();
    }
}
