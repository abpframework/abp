using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web;

public interface ILocalStorageService
{
    public ValueTask SetItemAsync(string key, string value);
    public ValueTask<string> GetItemAsync(string key);
    public ValueTask RemoveItemAsync(string key);
}