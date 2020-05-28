using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.BlobStoring.Database
{
    public interface IContainerRepository : IBasicRepository<Container, Guid>
    {
        Task<Container> GetContainerAsync([NotNull] string name);

        Task<Container> FindContainerAsync([NotNull] string name);
    }
}