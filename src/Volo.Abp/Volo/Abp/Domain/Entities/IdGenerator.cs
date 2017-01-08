using System;

namespace Volo.Abp.Domain.Entities
{
    public interface IIdGenerator
    {
        string GenerateStringId();

        Guid GenerateGuid();
    }
}
