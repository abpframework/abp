using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Admin.Contents;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Contents.Components.EntityContentEditor
{
    [Widget(
        ScriptFiles = new[]
        {
            "/Pages/CmsKit/Contents/Components/EntityContentEditor/default.js"
        })]
    public class EntityContentEditorViewComponent : AbpViewComponent
    {
        private readonly IContentAdminAppService contentAdminAppService;

        public EntityContentEditorViewComponent(IContentAdminAppService contentAdminAppService)
        {
            this.contentAdminAppService = contentAdminAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string entityType, string entityId, bool displaySubmitButton = true)
        {
            var viewModel = new ContentViewModel(entityType, entityId);

            try
            {
                if (entityId != null)
                {
                    var content = await contentAdminAppService.GetAsync(entityType, entityId);
                    viewModel.Value = content.Value;
                    viewModel.Id = content.Id;
                }
            }
            catch (EntityNotFoundException)
            {
                // Initialize editor even content doesn't exist.
            }

            return View(
                "~/Pages/CmsKit/Contents/Components/EntityContentEditor/Default.cshtml",
                new EntityContentEditorViewModel(viewModel, displaySubmitButton));
        }

        public class EntityContentEditorViewModel
        {
            public ContentViewModel ViewModel { get; set; }

            public bool DisplaySubmitButton { get; set; }

            public EntityContentEditorViewModel(ContentViewModel viewModel, bool displaySubmitButton)
            {
                ViewModel = viewModel;
                DisplaySubmitButton = displaySubmitButton;
            }
        }
    }
}
