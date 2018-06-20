namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public static class AbpAntiForgeryManagerAspNetCoreExtensions
    {
        public static void SetCookie(this IAbpAntiForgeryManager manager)
        {
            manager.HttpContext.Response.Cookies.Append(manager.Options.TokenCookieName, manager.GenerateToken());
        }
    }
}