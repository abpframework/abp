using System;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Theming;

public interface ITheme
{
    Type GetLayout(string name, bool fallbackToDefault = true);
}
