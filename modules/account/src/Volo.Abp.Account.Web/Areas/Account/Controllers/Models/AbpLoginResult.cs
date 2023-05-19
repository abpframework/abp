namespace Volo.Abp.Account.Web.Areas.Account.Controllers.Models;

public class AbpLoginResult
{
    public AbpLoginResult(LoginResultType result)
    {
        Result = result;
    }

    public LoginResultType Result { get; }

    public string Description => Result.ToString();
}
