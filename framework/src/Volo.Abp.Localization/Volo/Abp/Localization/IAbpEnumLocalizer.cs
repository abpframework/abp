using System;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public interface IAbpEnumLocalizer
{
    string GetString(Type enumType, object enumValue);

    string GetString(Type enumType, object enumValue, IStringLocalizer?[] specifyLocalizers);
}
