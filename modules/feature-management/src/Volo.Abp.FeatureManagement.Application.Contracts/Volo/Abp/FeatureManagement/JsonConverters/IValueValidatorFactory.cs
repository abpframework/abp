using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters;

public interface IValueValidatorFactory
{
    bool CanCreate(string name);
    
    IValueValidator Create();
}