using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.Clients;

public class ClientFinder : IClientFinder, ITransientDependency
{
    protected IClientRepository ClientRepository { get; }

    public ClientFinder(IClientRepository clientRepository)
    {
        ClientRepository = clientRepository;
    }

    public virtual async Task<List<ClientFinderResult>> SearchAsync(string filter, int page = 1)
    {
        using (ClientRepository.DisableTracking())
        {
            page = page < 1 ? 1 : page;
            var clients = await ClientRepository.GetListAsync(nameof(Client.ClientName), filter: filter, skipCount: (page - 1) * 10, maxResultCount: 10);
            return clients.Select(x => new ClientFinderResult
            {
                Id = x.Id,
                ClientId = x.ClientId
            }).ToList();
        }
    }
}
