using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic
{
    public partial class NavToolbar
    {
        [Inject]
        private IToolbarManager ToolbarManager { get; set; }

        private List<RenderFragment> ToolbarItemRenders { get; set; } = new List<RenderFragment>();

        protected override async Task OnInitializedAsync()
        {
            var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);

            ToolbarItemRenders.Clear();

            foreach (var item in toolbar.Items)
            {
                ToolbarItemRenders.Add(builder =>
                {
                    builder.OpenComponent(0, item.ComponentType);
                    builder.CloseComponent();
                });
            }
        }

    }
}
