using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.ObjectMapping;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Commenting;
using Volo.CmsKit.Public.Web.Security.Captcha;

namespace Volo.CmsKit.Public.Web.Controllers;

//[Route("cms-kit/public-comments")]
public class CmsKitPublicCommentsController : AbpController
{
    public ICommentPublicAppService CommentPublicAppService { get; }
    protected CmsKitCommentOptions CmsKitCommentOptions { get; }
    public SimpleMathsCaptchaGenerator SimpleMathsCaptchaGenerator { get; }

    public CmsKitPublicCommentsController(
        ICommentPublicAppService commentPublicAppService,
        IOptions<CmsKitCommentOptions> cmsKitCommentOptions,
        SimpleMathsCaptchaGenerator simpleMathsCaptchaGenerator)
    {
        CommentPublicAppService = commentPublicAppService;
        CmsKitCommentOptions = cmsKitCommentOptions.Value;
        SimpleMathsCaptchaGenerator = simpleMathsCaptchaGenerator;
    }

    [HttpPost]
    public async Task ValidateAsync([FromBody] CreateCommentWithParameteresInput input)
    {
        if (CmsKitCommentOptions.IsRecaptchaEnabled && input.CaptchaToken.HasValue)
        {
            SimpleMathsCaptchaGenerator.Validate(input.CaptchaToken.Value, input.CaptchaAnswer);
        }

        var dto = ObjectMapper.Map<CreateCommentWithParameteresInput, CreateCommentInput> (input);
        await CommentPublicAppService.CreateAsync(input.EntityType, input.EntityId, dto);
    }
}
