namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public class AbpAntiForgeryOptions
    {
        /// <summary>
        /// Get/sets cookie name to transfer Anti Forgery token between server and client.
        /// Default value: "XSRF-TOKEN".
        /// </summary>
        public string TokenCookieName { get; set; }

        /// <summary>
        /// Used to find auth cookie when validating Anti Forgery token.
        /// Default value: ".AspNet.ApplicationCookie".
        /// </summary>
        public string AuthorizationCookieName { get; set; }

        public AbpAntiForgeryOptions()
        {
            TokenCookieName = "XSRF-TOKEN";
            AuthorizationCookieName = ".AspNet.ApplicationCookie";
        }
    }
}
