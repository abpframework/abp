using System;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public class AbpAntiForgeryOptions
    {
        /// <summary>
        /// Use to set the cookie options to transfer Anti Forgery token between server and client.
        /// Default name of the cookie: "XSRF-TOKEN".
        /// </summary>
        public CookieBuilder TokenCookie { get; }

        /// <summary>
        /// Used to find auth cookie when validating Anti Forgery token.
        /// Default value: "Identity.Application".
        /// </summary>
        public string AuthCookieSchemaName { get; set; }

        public AbpAntiForgeryOptions()
        {
            TokenCookie = new CookieBuilder
            {
                Name = "XSRF-TOKEN",
                HttpOnly = false,
                IsEssential = true,
                Expiration = TimeSpan.FromDays(3650) //10 years!
            };

            AuthCookieSchemaName = "Identity.Application";
        }
    }
}
