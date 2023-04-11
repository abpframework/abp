using System.Threading.Tasks;

namespace Volo.Abp.Http.Client.IdentityModel.MauiBlazor;

public interface IAbpMauiAccessTokenProvider
{
    Task<string> GetAccessTokenAsync();
}