using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.Web.Pages;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs;

public class FeaturesModalModel : CmsKitAdminPageModel
{
    [BindProperty(SupportsGet = true)]
    [HiddenInput]
    public Guid BlogId { get; set; }

    [BindProperty]
    public List<BlogFeatureViewModel> Items { get; set; }

    protected IBlogFeatureAdminAppService BlogFeatureAdminAppService { get; }

    public FeaturesModalModel(IBlogFeatureAdminAppService blogFeatureAdminAppService)
    {
        BlogFeatureAdminAppService = blogFeatureAdminAppService;
    }

    public async Task OnGetAsync()
    {
        var blogFeatureDtos = await BlogFeatureAdminAppService.GetListAsync(BlogId);

        //Sort by localized feature name
        blogFeatureDtos.Sort((x, y) => string.Compare(L[x.FeatureName].Value, L[y.FeatureName].Value, StringComparison.CurrentCultureIgnoreCase));

        Items = ObjectMapper.Map<List<BlogFeatureDto>, List<BlogFeatureViewModel>>(blogFeatureDtos);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var dtos = ObjectMapper.Map<List<BlogFeatureViewModel>, List<BlogFeatureInputDto>>(Items);

        foreach (var item in dtos)
        {
            await BlogFeatureAdminAppService.SetAsync(BlogId, item);
        }

        return NoContent();
    }

    public class BlogFeatureViewModel
    {
        private string featureName;
        [HiddenInput]
        public string FeatureName { get => featureName; set => SetFeatureName(value); }

        public bool IsEnabled { get; set; }

        public bool IsAvailable { get; private set; }

        private void SetFeatureName(string value)
        {
            featureName = value;

            IsAvailable = GlobalFeatureManager.Instance.Modules.CmsKit().GetFeatures().Any(a => a.FeatureName == FeatureName) ?
                                GlobalFeatureManager.Instance.IsEnabled(FeatureName) :
                                true;

            if (!IsAvailable)
            {
                IsEnabled = false;
            }
        }
    }
}
