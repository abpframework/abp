namespace Volo.Abp.AspNetCore.ExceptionHandling
{
    public class AbpAuthorizationExceptionHandlerOptions
    {
        public bool UseAuthenticationScheme { get; set; }

        public string AuthenticationScheme { get; set; }

        public AbpAuthorizationExceptionHandlerOptions()
        {
            UseAuthenticationScheme = true;
        }
    }
}
