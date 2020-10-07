namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public static class AbpAntiForgeryManagerAspNetCoreExtensions
    {
        public static void SetCookie(this IAbpAntiForgeryManager manager)
        {
            manager.HttpContext.Response.Cookies.Append(
                manager.Options.TokenCookie.Name,
                manager.GenerateToken(),
                manager.Options.TokenCookie.Build(manager.HttpContext)
            );
        }
    }
}
