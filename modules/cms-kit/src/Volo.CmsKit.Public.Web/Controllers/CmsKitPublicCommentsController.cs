using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.ObjectMapping;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Commenting;
using Volo.CmsKit.Public.Web.Security.Captcha;

namespace Volo.CmsKit.Public.Web.Controllers;

//[Route("cms-kit/public-comments")]
public class CmsKitPublicCommentsController : CmsKitPublicControllerBase
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
    public virtual async Task ValidateAsync([FromBody] CreateCommentWithParametersInput input)
    {
        if (CmsKitCommentOptions.IsRecaptchaEnabled)
        {
            CheckCaptchaTokenNullity(input.CaptchaToken);

            await SimpleMathsCaptchaGenerator.ValidateAsync(input.CaptchaToken.Value, input.CaptchaAnswer);
        }

        var dto = ObjectMapper.Map<CreateCommentWithParametersInput, CreateCommentInput> (input);
        await CommentPublicAppService.CreateAsync(input.EntityType, input.EntityId, dto);
    }
    
    [HttpPost]
    public virtual async Task UpdateAsync(Guid id, [FromBody] UpdateCommentInput input)
    {
        if (CmsKitCommentOptions.IsRecaptchaEnabled)
        {
            CheckCaptchaTokenNullity(input.CaptchaToken);

            await SimpleMathsCaptchaGenerator.ValidateAsync(input.CaptchaToken.Value, input.CaptchaAnswer);
        }
        
        await CommentPublicAppService.UpdateAsync(id, input);
    }

    private void CheckCaptchaTokenNullity(Guid? captchaToken)
    {
        if (!captchaToken.HasValue)
        {
            throw new UserFriendlyException(L["CaptchaCodeMissingMessage"]);
        }
    }
}
