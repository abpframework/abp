using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters
{
    public class AbpJsonValueConverter<TPropertyType> : ValueConverter<TPropertyType, string>
    {
        public AbpJsonValueConverter()
            : base(
                d => SerializeObject(d),
                s => DeserializeObject(s))
        {

        }

        private static string SerializeObject(TPropertyType d)
        {
            return JsonConvert.SerializeObject(d, Formatting.None);
        }

        private static TPropertyType DeserializeObject(string s)
        {
            return JsonConvert.DeserializeObject<TPropertyType>(s);
        }
    }
}
