using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public interface IAbpAntiForgeryManager
    {
        AbpAntiForgeryOptions Options { get; }

        HttpContext HttpContext { get; }

        void SetCookie();

        string GenerateToken();
    }
}
