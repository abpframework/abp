using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Features;

namespace Volo.Abp.AspNetCore.Mvc.Features
{
    public class FeatureTestPage : AbpPageModel
    {
        [RequiresFeature("AllowedFeature")]
        public Task OnGetAllowedFeatureAsync()
        {
            return Task.CompletedTask;
        }

        [RequiresFeature("NotAllowedFeature")]
        public void OnGetNotAllowedFeature()
        {
            
        }

        public ObjectResult OnGetNoFeature()
        {
            return new ObjectResult(42);
        }
    }
}