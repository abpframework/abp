using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.Clients
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> FindByCliendIdIncludingAllAsync([NotNull] string clientId);
    }
}