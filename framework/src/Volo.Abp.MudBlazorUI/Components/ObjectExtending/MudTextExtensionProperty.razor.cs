using System.ComponentModel.DataAnnotations;
using System.Linq;
using MudBlazor;
using Volo.Abp.Data;
using Volo.Abp.Reflection;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public partial class MudTextExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected string? Value
    {
        get => PropertyInfo.GetTextInputValueOrNull(Entity.GetProperty(PropertyInfo.Name));
        set => Entity.SetProperty(PropertyInfo.Name, value, validate: false);
    }

    protected InputType GetInputType()
    {
        foreach (var attribute in PropertyInfo.Attributes)
        {
            if (attribute is DataTypeAttribute dataTypeAttribute)
            {
                return dataTypeAttribute.DataType switch
                {
                    DataType.Password => InputType.Password,
                    DataType.EmailAddress => InputType.Email,
                    DataType.Url => InputType.Url,
                    DataType.PhoneNumber => InputType.Telephone,
                    _ => InputType.Text
                };
            }

            if (attribute is EmailAddressAttribute)
            {
                return InputType.Email;
            }

            if (attribute is UrlAttribute)
            {
                return InputType.Url;
            }

            if (attribute is PhoneAttribute)
            {
                return InputType.Telephone;
            }
        }

        return InputType.Text;
    }

    protected InputMode GetInputMode()
    {
        foreach (var attribute in PropertyInfo.Attributes)
        {
            if (attribute is DataTypeAttribute dataTypeAttribute)
            {
                return dataTypeAttribute.DataType switch
                {
                    DataType.EmailAddress => InputMode.email,
                    DataType.Url => InputMode.url,
                    DataType.PhoneNumber => InputMode.tel,
                    _ => GetInputModeFromType()
                };
            }

            if (attribute is EmailAddressAttribute)
            {
                return InputMode.email;
            }

            if (attribute is UrlAttribute)
            {
                return InputMode.url;
            }

            if (attribute is PhoneAttribute)
            {
                return InputMode.tel;
            }
        }

        return GetInputModeFromType();
    }

    private InputMode GetInputModeFromType()
    {
        if (TypeHelper.IsFloatingType(PropertyInfo.Type))
        {
            return InputMode.@decimal;
        }

        if (MudBlazorUiObjectExtensionPropertyInfoExtensions.NumberTypes.Contains(PropertyInfo.Type))
        {
            return InputMode.numeric;
        }

        return InputMode.text;
    }
}
