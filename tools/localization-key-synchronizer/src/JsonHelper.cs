using System;
using Newtonsoft.Json;

namespace LocalizationKeySynchronizer;

public static class JsonHelper
{
    public static bool TryDeserialize<T>(string json, out T? result)
    {
        try
        {
            result = JsonConvert.DeserializeObject<T>(json);
            return true;
        }
        catch (Exception)
        {
            result = default;
            return false;
        }
    }
    
    public static string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value, Formatting.Indented);
    }
}