namespace Volo.Abp.Account.Web
{
    public class AbpAccountOptions
    {
        /// <summary>
        /// Default value: "Windows".
        /// </summary>
        public string WindowsAuthenticationSchemeName { get; set; }

        public AbpAccountOptions()
        {
            //TODO: This makes us depend on the Microsoft.AspNetCore.Server.IISIntegration package.
            WindowsAuthenticationSchemeName = "Windows"; //Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
        }
    }
}
