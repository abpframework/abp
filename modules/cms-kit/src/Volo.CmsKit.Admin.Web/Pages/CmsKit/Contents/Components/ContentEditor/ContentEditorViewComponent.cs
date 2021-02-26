using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.TuiEditor;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Uppy;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Contents.Components.ContentEditor
{
    [Widget(
        StyleTypes = new[]
        {
            typeof(TuiEditorStyleContributor)
        },
        ScriptTypes = new[]
        {
            typeof(TuiEditorScriptContributor),
            typeof(UppyScriptContributor)
        },
        ScriptFiles = new[]
        {
            "/Pages/CmsKit/Contents/Components/ContentEditor/default.js"
        })]
    public class ContentEditorViewComponent : AbpViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string inputId)
        {
            return View(
                "~/Pages/CmsKit/Contents/Components/ContentEditor/Default.cshtml",
                new ContentEditorViewModel(inputId));
        }

        public class ContentEditorViewModel
        {
            public ContentEditorViewModel(string inputId)
            {
                InputId = inputId;
            }

            public string InputId { get; set; }
        }
    }
}
