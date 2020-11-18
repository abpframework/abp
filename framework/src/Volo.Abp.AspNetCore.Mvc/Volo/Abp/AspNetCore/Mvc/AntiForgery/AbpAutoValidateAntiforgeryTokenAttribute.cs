using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AbpAutoValidateAntiforgeryTokenAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        /// <summary>
        /// Gets the order value for determining the order of execution of filters. Filters execute in
        /// ascending numeric value of the <see cref="Order"/> property.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Filters are executed in a sequence determined by an ascending sort of the <see cref="Order"/> property.
        /// </para>
        /// <para>
        /// The default Order for this attribute is 1000 because it must run after any filter which does authentication
        /// or login in order to allow them to behave as expected (ie Unauthenticated or Redirect instead of 400).
        /// </para>
        /// <para>
        /// Look at <see cref="IOrderedFilter.Order"/> for more detailed info.
        /// </para>
        /// </remarks>
        public int Order { get; set; } = 1000;

        /// <inheritdoc />
        public bool IsReusable => true;

        /// <inheritdoc />
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<AbpAutoValidateAntiforgeryTokenAuthorizationFilter>();
        }
    }
}
