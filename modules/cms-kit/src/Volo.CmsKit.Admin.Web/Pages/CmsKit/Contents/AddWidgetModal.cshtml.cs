using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Contents;

public class AddWidgetModal : AbpPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Text { get; set; }


    public AddWidgetModal()
    {
    }

    public async Task OnGetAsync()
    {

    }

    public async Task OnPostAsync()
    {

    }
}
