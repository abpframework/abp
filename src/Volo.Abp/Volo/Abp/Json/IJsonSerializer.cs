namespace Volo.Abp.Json
{
    public interface IJsonSerializer
    {
        string Serialize(object obj, bool camelCase = true, bool indented = false);

        T Deserialize<T>(string jsonString);
    }
}