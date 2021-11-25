using System.Collections.Generic;
using Volo.Abp.FeatureManagement.JsonConverters;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement;

public class AbpFeatureManagementApplicationContractsOptions
{
    public HashSet<IValueValidatorFactory> ValueValidatorFactory { get; }
    
    public AbpFeatureManagementApplicationContractsOptions()
    {
        ValueValidatorFactory = new HashSet<IValueValidatorFactory> 
        {
            new ValueValidatorFactory<AlwaysValidValueValidator>("NULL"),
            new ValueValidatorFactory<BooleanValueValidator>("BOOLEAN"),
            new ValueValidatorFactory<NumericValueValidator>("NUMERIC"),
            new ValueValidatorFactory<StringValueValidator>("STRING")
        };
    }
}