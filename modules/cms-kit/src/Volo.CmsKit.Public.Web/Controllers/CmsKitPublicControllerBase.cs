using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Public.Web.Controllers;

public abstract class CmsKitPublicControllerBase : AbpController
{
	public CmsKitPublicControllerBase()
	{
		LocalizationResource = typeof(CmsKitResource);
	}
}
