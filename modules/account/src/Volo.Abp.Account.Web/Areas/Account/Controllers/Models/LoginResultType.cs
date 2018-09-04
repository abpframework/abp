namespace Volo.Abp.Account.Web.Areas.Account.Controllers.Models
{
    public enum LoginResultType : byte
    {
        Success = 1,

        InvalidUserNameOrPassword = 2,

        NotAllowed = 3,

        LockedOut = 4,

        RequiresTwoFactor = 5
    }
}