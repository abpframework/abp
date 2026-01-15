using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;

public static class MudBlazorPageToolbarExtensions
{
    public static PageToolbar AddMudButton(
        this PageToolbar toolbar,
        string text,
        Func<Task> clicked,
        object? icon = null,
        int order = 0,
        string? requiredPolicyName = null)
    {
        toolbar.AddComponent(
            typeof(MudPageToolbarButton),
            new Dictionary<string, object?>
            {
                { nameof(MudPageToolbarButton.Text), text },
                { nameof(MudPageToolbarButton.Disabled), false },
                { nameof(MudPageToolbarButton.Icon), icon },
                { nameof(MudPageToolbarButton.Clicked), clicked }
            },
            order,
            requiredPolicyName
        );

        return toolbar;
    }
}

