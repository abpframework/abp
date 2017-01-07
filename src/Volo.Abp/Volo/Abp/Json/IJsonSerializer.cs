namespace Volo.Abp.Json
{
    public interface IJsonSerializer
    {
        string Serialize(object obj, bool camelCase = false, bool indented = false);

        T Deserialize<T>(string jsonString);
    }
}