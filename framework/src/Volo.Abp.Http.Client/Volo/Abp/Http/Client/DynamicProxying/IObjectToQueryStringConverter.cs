namespace Volo.Abp.Http.Client.DynamicProxying
{
    public interface IObjectToQueryStringConverter
    {
        string ConvertObjectToQueryString(string name, object obj);
    }
}