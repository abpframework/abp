using System;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpAspNetCoreMvcApplicationBuilderExtensions
    {
        ///  <summary>
        ///  Adds MVC to the <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> request execution pipeline
        ///  with the following default routes:
        /// 
        ///  - a default route named 'defaultWithArea' and the following template: '{area}/{controller=Home}/{action=Index}/{id?}'.
        ///  - a default route named 'default' and the following template: '{controller=Home}/{action=Index}/{id?}'.
        ///  </summary>
        ///  <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <param name="additionalConfigurationAction">Additional action to configure routes</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        [Obsolete("Use app.UseConfiguredEndpoints(...) extension method instead!")]
        public static IApplicationBuilder UseMvcWithDefaultRouteAndArea(
            this IApplicationBuilder app, 
            Action<IEndpointRouteBuilder> additionalConfigurationAction = null)
        {
            return app.UseConfiguredEndpoints(additionalConfigurationAction);
        }
    }
}
