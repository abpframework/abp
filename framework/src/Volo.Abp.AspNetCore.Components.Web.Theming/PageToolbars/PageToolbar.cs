namespace Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

public class PageToolbar
{
    public PageToolbarContributorList Contributors { get; set; }

    public PageToolbar()
    {
        Contributors = new PageToolbarContributorList();
    }
}
