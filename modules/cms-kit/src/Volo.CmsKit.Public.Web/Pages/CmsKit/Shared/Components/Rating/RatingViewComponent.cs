using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Users;
using Volo.CmsKit.Public.Ratings;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Rating
{
    [ViewComponent(Name = "CmsRating")]
    [Widget(
        StyleTypes = new[] {typeof(RatingStyleBundleContributor)},
        ScriptTypes = new[] {typeof(RatingScriptBundleContributor)},
        RefreshUrl = "/CmsKitPublicWidgets/Rating",
        AutoInitialize = true
    )]
    public class RatingViewComponent : AbpViewComponent
    {
        public IRatingPublicAppService RatingPublicAppService { get; }
        public AbpMvcUiOptions AbpMvcUiOptions { get; }
        public ICurrentUser CurrentUser { get; }

        public RatingViewComponent(IRatingPublicAppService ratingPublicAppService, IOptions<AbpMvcUiOptions> options, ICurrentUser currentUser)
        {
            RatingPublicAppService = ratingPublicAppService;
            AbpMvcUiOptions = options.Value;
            CurrentUser = currentUser;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(string entityType, string entityId)
        {
            var ratings = await RatingPublicAppService.GetGroupedStarCountsAsync(entityType, entityId);
            var totalRating = ratings.Sum(x => x.Count);

            short? currentUserRating = null;
            if (CurrentUser.IsAuthenticated)
            {
                currentUserRating = ratings.Find(x => x.IsSelectedByCurrentUser)?.StarCount;
            }

            var loginUrl =
                $"{AbpMvcUiOptions.LoginUrl}?returnUrl={HttpContext.Request.Path.ToString()}&returnUrlHash=#cms-rating_{entityType}_{entityId}";

            var viewModel = new RatingViewModel
            {
                EntityId = entityId,
                EntityType = entityType,
                LoginUrl = loginUrl,
                Ratings = ratings,
                CurrentRating = currentUserRating,
                TotalRating = totalRating
            };

            return View("~/Pages/CmsKit/Shared/Components/Rating/Default.cshtml", viewModel);
        }
    }

    public class RatingViewModel
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string LoginUrl { get; set; }

        public List<RatingWithStarCountDto> Ratings { get; set; }

        public short? CurrentRating { get; set; }

        public int TotalRating { get; set; }
    }
}
