using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    public static class ValidationAttributeHelper
    {
        private static readonly PropertyInfo ValidationAttributeErrorMessageStringProperty = typeof(ValidationAttribute)
            .GetProperty("ErrorMessageString", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly PropertyInfo ValidationAttributeCustomErrorMessageSetProperty = typeof(ValidationAttribute)
            .GetProperty("CustomErrorMessageSet", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void SetDefaultErrorMessage(ValidationAttribute validationAttribute)
        {
            if (validationAttribute is StringLengthAttribute stringLength && stringLength.MinimumLength != 0)
            {
                var customErrorMessageSet = ValidationAttributeCustomErrorMessageSetProperty.GetValue(validationAttribute) as bool?;
                if (customErrorMessageSet != true)
                {
                    validationAttribute.ErrorMessage =
                        "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.";
                    return;
                }
            }

            validationAttribute.ErrorMessage =
                ValidationAttributeErrorMessageStringProperty.GetValue(validationAttribute) as string;
        }
    }
}
