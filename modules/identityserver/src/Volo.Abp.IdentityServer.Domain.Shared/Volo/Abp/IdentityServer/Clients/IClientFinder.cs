using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.IdentityServer.Clients;

public interface IClientFinder
{
    Task<List<ClientFinderResult>> SearchAsync(string filter, int page = 1);
}
