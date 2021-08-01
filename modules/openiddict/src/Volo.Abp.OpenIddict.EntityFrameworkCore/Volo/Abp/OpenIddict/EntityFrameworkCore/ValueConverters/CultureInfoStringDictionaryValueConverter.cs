using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore.ValueConverters
{
    public class CultureInfoStringDictionaryValueConverter : ValueConverter<Dictionary<CultureInfo, string>, string>
    {
        public CultureInfoStringDictionaryValueConverter()
            : base(
                d => SerializeObject(d),
                s => DeserializeObject(s))
        {

        }

        private static string SerializeObject(Dictionary<CultureInfo, string> cultureInfoStringDictionary)
        {
            var stringMapDictionary = new Dictionary<string, string>();
            foreach (var item in cultureInfoStringDictionary)
            {
                stringMapDictionary.Add(item.Key.Name, item.Value);
            }
            return JsonSerializer.Serialize(stringMapDictionary);
        }

        private static Dictionary<CultureInfo, string> DeserializeObject(string s)
        {
            var deserializeOptions = new JsonSerializerOptions();
            deserializeOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            deserializeOptions.WriteIndented = false;
            var stringMapDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(s, deserializeOptions);
            var cultureInfoStringDictionary = new Dictionary<CultureInfo, string>();
            foreach (var item in stringMapDictionary)
            {
                cultureInfoStringDictionary.Add(CultureInfo.GetCultureInfo(item.Key), item.Value);
            }
            return cultureInfoStringDictionary;
        }
    }
}
