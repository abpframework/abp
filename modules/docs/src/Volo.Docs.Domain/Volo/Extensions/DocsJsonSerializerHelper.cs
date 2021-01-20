using System.Text.Json;

namespace Volo.Extensions
{
    public static class DocsJsonSerializerHelper
    {
        public static bool TryDeserialize<T>(string jsonContent, out T result)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };
                result = JsonSerializer.Deserialize<T>(jsonContent, options);
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
