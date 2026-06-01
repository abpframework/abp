using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Volo.Abp.MudBlazorUI.Validation;

public static class MudValidationAttributeHelper
{
    internal static readonly PropertyInfo ValidationAttributeErrorMessageStringProperty = typeof(ValidationAttribute)
        .GetProperty("ErrorMessageString", BindingFlags.Instance | BindingFlags.NonPublic)!;

    internal static readonly PropertyInfo ValidationAttributeCustomErrorMessageSetProperty = typeof(ValidationAttribute)
        .GetProperty("CustomErrorMessageSet", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private const string PlaceholderPrefix = "__Abp_";

    public static string SetErrorMessagePlaceholders(string? errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            return string.Empty;
        }

        return Regex.Replace(errorMessage, @"\{(\d+)\}", m => PlaceholderPrefix + m.Groups[1].Value);
    }

    public static string RevertErrorMessagePlaceholders(string? errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            return string.Empty;
        }

        return Regex.Replace(errorMessage, PlaceholderPrefix + @"(\d+)", m => "{" + m.Groups[1].Value + "}");
    }

    public static string GetDefaultErrorMessage(ValidationAttribute validationAttribute)
    {
        if (validationAttribute is StringLengthAttribute stringLengthAttribute && stringLengthAttribute.MinimumLength != 0)
        {
            var customErrorMessageSet = ValidationAttributeCustomErrorMessageSetProperty.GetValue(validationAttribute) as bool?;
            if (customErrorMessageSet != true)
            {
                return SetErrorMessagePlaceholders("The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.");
            }
        }

        var errorMessageString = ValidationAttributeErrorMessageStringProperty.GetValue(validationAttribute) as string;
        return SetErrorMessagePlaceholders(errorMessageString);
    }
}
