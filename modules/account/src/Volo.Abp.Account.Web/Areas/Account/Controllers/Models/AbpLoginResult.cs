namespace Volo.Abp.Account.Web.Areas.Account.Controllers.Models;

public class AbpLoginResult(LoginResultType result)
{
    public LoginResultType Result { get; } = result;

    public string Description => Result.ToString();
}
