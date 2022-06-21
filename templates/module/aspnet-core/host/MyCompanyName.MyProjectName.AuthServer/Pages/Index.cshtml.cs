using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using IdentityServer4.Stores;
using Volo.Abp.IdentityServer.Clients;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyCompanyName.MyProjectName.Pages;

public class IndexModel : AbpPageModel
{
    public List<Client> Clients { get; protected set; }
    
    protected IClientRepository ClientRepository { get; }
    
    public IndexModel(IClientRepository clientRepository)
    {
        this.ClientRepository = clientRepository;
    }

    public async Task OnGetAsync()
    {
        Clients = await ClientRepository.GetListAsync(includeDetails: true);
    }
}