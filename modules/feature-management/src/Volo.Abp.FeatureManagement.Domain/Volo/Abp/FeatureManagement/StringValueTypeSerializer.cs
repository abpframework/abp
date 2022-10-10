using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement;

public class StringValueTypeSerializer : ITransientDependency
{
    protected IJsonSerializer JsonSerializer { get; }

    public StringValueTypeSerializer(IJsonSerializer jsonSerializer)
    {
        JsonSerializer = jsonSerializer;
    }

    public virtual string Serialize(IStringValueType stringValueType)
    {
        return JsonSerializer.Serialize(stringValueType);
    }

    public virtual IStringValueType Deserialize(string value)
    {
        return JsonSerializer.Deserialize<IStringValueType>(value);
    }
}
