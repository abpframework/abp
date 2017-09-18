using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.Views.Shared.Components.AbpStyleBundle
{
    public class AbpStyleBundleViewComponent : AbpViewComponent
    {
        private readonly IBundleManager _bundleManager;

        public AbpStyleBundleViewComponent(IBundleManager bundleManager)
        {
            _bundleManager = bundleManager;
        }

        public IViewComponentResult Invoke(string name)
        {
            var files = _bundleManager.GetStyleBundleFiles(name);
            return View(files);
        }
    }
}
