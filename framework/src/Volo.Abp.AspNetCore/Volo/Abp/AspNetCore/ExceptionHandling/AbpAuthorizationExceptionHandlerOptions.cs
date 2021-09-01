namespace Volo.Abp.AspNetCore.ExceptionHandling
{
    public class AbpAuthorizationExceptionHandlerOptions
    {
        public bool UseAuthenticationScheme { get; set; }

        /// <summary>
        /// Use default forbid/challenge scheme if this is not specified.
        /// </summary>
        public string AuthenticationScheme { get; set; }

        public AbpAuthorizationExceptionHandlerOptions()
        {
            UseAuthenticationScheme = true;
        }
    }
}
