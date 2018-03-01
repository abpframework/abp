namespace Volo.Abp.Account.Web
{
    public class AbpAccountOptions
    {
        public string WindowsAuthenticationSchemeName { get; set; }

        public AbpAccountOptions()
        {
            //TODO: This makes us depend on the Microsoft.AspNetCore.Server.IISIntegration package.
            WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
        }
    }
}
