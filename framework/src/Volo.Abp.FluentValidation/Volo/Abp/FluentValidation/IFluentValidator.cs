using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Validation;

namespace Volo.Abp.FluentValidation
{
    public interface IFluentValidator
    {
        void Validate(MethodInvocationValidationContext context);
    }
}
