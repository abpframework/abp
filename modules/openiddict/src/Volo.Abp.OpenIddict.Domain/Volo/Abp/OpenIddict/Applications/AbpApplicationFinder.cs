using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpApplicationFinder : IApplicationFinder, ITransientDependency
{
    protected IOpenIddictApplicationRepository ApplicationRepository { get; }

    public AbpApplicationFinder(IOpenIddictApplicationRepository applicationRepository)
    {
        ApplicationRepository = applicationRepository;
    }

    public virtual async Task<List<ApplicationFinderResult>> SearchAsync(string filter, int page = 1)
    {
        using (ApplicationRepository.DisableTracking())
        {
            page = page < 1 ? 1 : page;
            var applications = await ApplicationRepository.GetListAsync(nameof(OpenIddictApplication.CreationTime), filter: filter, skipCount: (page - 1) * 10, maxResultCount: 10);
            return applications.Select(x => new ApplicationFinderResult
            {
                Id = x.Id,
                ClientId = x.ClientId
            }).ToList();
        }
    }
}
