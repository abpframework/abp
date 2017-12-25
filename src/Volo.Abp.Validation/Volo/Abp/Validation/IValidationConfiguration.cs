using System;
using System.Collections.Generic;

namespace Volo.Abp.Validation
{
    public class AbpValidationOptions
    {
        public List<Type> IgnoredTypes { get; }

        public AbpValidationOptions()
        {
            IgnoredTypes = new List<Type>();
        }
    }
}