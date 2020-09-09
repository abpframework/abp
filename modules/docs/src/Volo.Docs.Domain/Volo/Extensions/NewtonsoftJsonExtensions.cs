using System;
using Newtonsoft.Json;

namespace Volo.Extensions
{
    public static class JsonConvertExtensions
    {
        public static bool TryDeserializeObject<T>(string jsonContent, out T result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(jsonContent);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}