using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.Views.Shared.Components.AbpScriptBundle
{
    public class AbpScriptBundleViewComponent : AbpViewComponent
    {
        private readonly IBundleManager _bundleManager;

        public AbpScriptBundleViewComponent(IBundleManager bundleManager)
        {
            _bundleManager = bundleManager;
        }

        public IViewComponentResult Invoke(string name)
        {
            var files = _bundleManager.GetScriptBundleFiles(name);
            return View(files);
        }
    }
}
