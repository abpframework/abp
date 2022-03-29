using System;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;

namespace Volo.Abp.OpenIddict;

public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
{
    private readonly string _name;

    public FormValueRequiredAttribute(string name)
    {
        _name = name;
    }

    public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
    {
        if (string.Equals(routeContext.HttpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(routeContext.HttpContext.Request.Method, "HEAD", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(routeContext.HttpContext.Request.Method, "DELETE", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(routeContext.HttpContext.Request.Method, "TRACE", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (string.IsNullOrEmpty(routeContext.HttpContext.Request.ContentType))
        {
            return false;
        }

        if (!routeContext.HttpContext.Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return !string.IsNullOrEmpty(routeContext.HttpContext.Request.Form[_name]);
    }
}
