using System;
using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.Validation
{
    public class AbpValidationOptions
    {
        public List<Type> IgnoredTypes { get; }

        public ITypeList<IMethodInvocationValidator> ValidationContributor { get; set; }

        public AbpValidationOptions()
        {
            IgnoredTypes = new List<Type>();
            ValidationContributor = new TypeList<IMethodInvocationValidator>();
        }
    }
}