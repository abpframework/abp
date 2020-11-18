namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public interface IAbpAntiForgeryManager
    {
        void SetCookie();

        string GenerateToken();
    }
}
