using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public class AbpAntiForgeryAuthCookieNameProvider : ITransientDependency
    {
        private readonly IOptionsSnapshot<CookieAuthenticationOptions> _namedOptionsAccessor;
        private readonly AbpAntiForgeryOptions _abpAntiForgeryOptions;

        public AbpAntiForgeryAuthCookieNameProvider(
            IOptionsSnapshot<CookieAuthenticationOptions> namedOptionsAccessor,
            IOptions<AbpAntiForgeryOptions> abpAntiForgeryOptions)
        {
            _namedOptionsAccessor = namedOptionsAccessor;
            _abpAntiForgeryOptions = abpAntiForgeryOptions.Value;
        }

        public virtual string GetNameOrNull()
        {
            return _namedOptionsAccessor.Get(_abpAntiForgeryOptions.AuthCookieSchemaName)?.Cookie?.Name;
        }
    }
}
