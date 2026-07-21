using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudBlazor;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;

public static class PageToolbarExtensions
{
    public static PageToolbar AddComponent<TComponent>(
        this PageToolbar toolbar,
        Dictionary<string, object?>? arguments = null,
        int order = 0,
        string? requiredPolicyName = null)
    {
        return toolbar.AddComponent(
            typeof(TComponent),
            arguments,
            order,
            requiredPolicyName
        );
    }

    public static PageToolbar AddComponent(
        this PageToolbar toolbar,
        Type componentType,
        Dictionary<string, object?>? arguments = null,
        int order = 0,
        string? requiredPolicyName = null)
    {
        toolbar.Contributors.Add(
            new SimplePageToolbarContributor(
                componentType,
                arguments,
                order,
                requiredPolicyName
            )
        );

        return toolbar;
    }

    public static PageToolbar AddButton(
        this PageToolbar toolbar,
        string text,
        Func<Task> clicked,
        object? icon = null,
        Color? color = null,
        bool disabled = false,
        int order = 0,
        string? requiredPolicyName = null)
    {
        toolbar.AddComponent<MudPageToolbarButton>(
            new Dictionary<string, object?>
            {
                { nameof(MudPageToolbarButton.Color), color ?? Color.Primary },
                { nameof(MudPageToolbarButton.Text), text },
                { nameof(MudPageToolbarButton.Disabled), disabled },
                { nameof(MudPageToolbarButton.Icon), icon },
                { nameof(MudPageToolbarButton.OnClickAsync), clicked },
            },
            order,
            requiredPolicyName
        );

        return toolbar;
    }

    public static PageToolbar AddMudButton(
        this PageToolbar toolbar,
        string text,
        Func<Task> clicked,
        string? icon = null,
        Color? color = null,
        bool disabled = false,
        int order = 0,
        string? requiredPolicyName = null)
    {
        return toolbar.AddButton(text, clicked, icon, color, disabled, order, requiredPolicyName);
    }
}
