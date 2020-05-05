using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    public static class ValidationAttributeHelper
    {
        private static readonly PropertyInfo ValidationAttributeErrorMessageStringProperty = typeof(ValidationAttribute)
            .GetProperty("ErrorMessageString", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void SetDefaultErrorMessage(ValidationAttribute validationAttribute)
        {
            validationAttribute.ErrorMessage =
                ValidationAttributeErrorMessageStringProperty.GetValue(validationAttribute) as string;
        }
    }
}
