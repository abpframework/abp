using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

/// <summary>
/// Wraps an <see cref="IUrlHelper"/> to automatically inject the culture route value
/// into all URL generation calls.
/// </summary>
public class AbpCultureAwareUrlHelper : IUrlHelper
{
    protected IUrlHelper Inner { get; }
    protected string Culture { get; }

    public AbpCultureAwareUrlHelper(IUrlHelper inner, string culture)
    {
        Inner = inner;
        Culture = culture;
    }

    public ActionContext ActionContext => Inner.ActionContext;

    public virtual string? Action(UrlActionContext actionContext)
    {
        var values = new RouteValueDictionary(actionContext.Values);
        values.TryAdd("culture", Culture);

        return Inner.Action(new UrlActionContext
        {
            Action = actionContext.Action,
            Controller = actionContext.Controller,
            Values = values,
            Protocol = actionContext.Protocol,
            Host = actionContext.Host,
            Fragment = actionContext.Fragment,
        });
    }

    public virtual string? Content(string? contentPath)
    {
        return Inner.Content(contentPath);
    }

    public virtual bool IsLocalUrl(string? url)
    {
        return Inner.IsLocalUrl(url);
    }

    public virtual string? Link(string? routeName, object? values)
    {
        var rvd = new RouteValueDictionary(values);
        rvd.TryAdd("culture", Culture);
        return Inner.Link(routeName, rvd);
    }

    public virtual string? RouteUrl(UrlRouteContext routeContext)
    {
        var values = new RouteValueDictionary(routeContext.Values);
        values.TryAdd("culture", Culture);

        return Inner.RouteUrl(new UrlRouteContext
        {
            RouteName = routeContext.RouteName,
            Values = values,
            Protocol = routeContext.Protocol,
            Host = routeContext.Host,
            Fragment = routeContext.Fragment,
        });
    }
}
