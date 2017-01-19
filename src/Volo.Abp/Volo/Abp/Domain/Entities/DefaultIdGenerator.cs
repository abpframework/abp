using System;
using Volo.Abp.Guids;
using Volo.DependencyInjection;

namespace Volo.Abp.Domain.Entities
{
    //TODO: No need! Directly use Guid Generator!
    public class DefaultIdGenerator : IIdGenerator, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;

        public DefaultIdGenerator(IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }

        public string GenerateStringId()
        {
            return GenerateGuid().ToString();
        }

        public Guid GenerateGuid()
        {
            return _guidGenerator.Create();
        }
    }
}