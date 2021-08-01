using System.Collections.Generic;
using System.Security.Claims;

// Move other packages?
namespace Volo.Abp.Account.Web.AbpGrantTypes
{
    public class GrantTypeResult
    {
        public ClaimsPrincipal Principal { get; protected set; }

        public bool Success { get; protected set; }

        public string Error { get; protected set; }

        public string ErrorDescription { get; protected set; }

        public Dictionary<string, string> Properties { get; protected set; }

        public static GrantTypeResult SuccessResult(ClaimsPrincipal principal)
        {
            return new GrantTypeResult(principal);
        }

        public static GrantTypeResult FailedResult(string error, string errorDescription)
        {
            return new GrantTypeResult(error, errorDescription);
        }

        protected GrantTypeResult()
        {
            Properties = new Dictionary<string, string>();
        }

        public GrantTypeResult(ClaimsPrincipal principal) : this()
        {
            Success = true;
            Principal = principal;
        }

        public GrantTypeResult(string error, string errorDescription) : this()
        {
            Success = false;
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
