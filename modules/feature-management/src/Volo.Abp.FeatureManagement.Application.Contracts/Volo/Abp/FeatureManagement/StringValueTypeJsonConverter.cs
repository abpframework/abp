using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement
{
    public class StringValueTypeJsonConverter : JsonConverter, ITransientDependency
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IStringValueType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("This method should not be called to write (since CanWrite is false).");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //TODO: Deserialize!
            return null;
        }
    }
}
