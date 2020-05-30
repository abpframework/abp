using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpPageSearchBox
{
    public class AbpPageSearchBoxViewComponent : AbpViewComponent
    {
        public string PlaceHolder { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(
            string placeHolder,
            string formClassName,
            string textBoxClassName)
        {
            return await Task.FromResult(View("~/Pages/Shared/Components/AbpPageSearchBox/Default.cshtml",
                new SearchBoxViewModel
                {
                    PlaceHolder = placeHolder,
                    FormClassName = formClassName,
                    TextBoxClassName = textBoxClassName
                }));
            ;
        }
    }

    public class SearchBoxViewModel
    {
        public string PlaceHolder { get; set; }

        public string FormClassName { get; set; }

        public string TextBoxClassName { get; set; }
    }
}