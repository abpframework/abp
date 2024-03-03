using Volo.Abp;

namespace Volo.CmsKit.Domain.Volo.CmsKit.Pages;

[Serializable]
public class MultipleHomePageException : BusinessException
{
	public MultipleHomePageException()
	{
		Code = CmsKitErrorCodes.Pages.MultipleHomePage;
	}
}
