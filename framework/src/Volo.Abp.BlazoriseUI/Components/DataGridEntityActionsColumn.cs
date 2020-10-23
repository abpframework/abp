using System.Linq;
using Blazorise.DataGrid;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.BlazoriseUI.Components
{
    public class DataGridEntityActionsColumn<TItem> : DataGridColumn<TItem>
    {
        [Inject]
        public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Caption = UiLocalizer["Actions"];
            Width = "150px";
            Sortable = false;
            Field = typeof(TItem).GetProperties().First().Name;
        }
    }
}
