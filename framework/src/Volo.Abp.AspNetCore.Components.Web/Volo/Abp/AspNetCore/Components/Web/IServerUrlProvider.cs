namespace Volo.Abp.AspNetCore.Components.Web
{
    public interface IServerUrlProvider
    {
        string GetBaseUrl(string remoteServiceName = null);
    }
}