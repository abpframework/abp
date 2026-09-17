using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class AbpCultureAwareUrlHelper_Tests
{
    [Fact]
    public void Action_Should_Inject_Culture()
    {
        var inner = Substitute.For<IUrlHelper>();
        inner.Action(Arg.Any<UrlActionContext>()).Returns(callInfo =>
        {
            var ctx = callInfo.Arg<UrlActionContext>();
            var values = new RouteValueDictionary(ctx.Values);
            return values.ContainsKey("culture") ? $"/{values["culture"]}/{ctx.Controller}/{ctx.Action}" : $"/{ctx.Controller}/{ctx.Action}";
        });

        var helper = new AbpCultureAwareUrlHelper(inner, "zh-Hans");
        var result = helper.Action(new UrlActionContext { Controller = "Home", Action = "Index" });

        result.ShouldContain("zh-Hans");
    }

    [Fact]
    public void Action_Should_Not_Override_Explicit_Culture()
    {
        var inner = Substitute.For<IUrlHelper>();
        inner.Action(Arg.Any<UrlActionContext>()).Returns(callInfo =>
        {
            var ctx = callInfo.Arg<UrlActionContext>();
            var values = new RouteValueDictionary(ctx.Values);
            return $"/{values["culture"]}/Home/Index";
        });

        var helper = new AbpCultureAwareUrlHelper(inner, "zh-Hans");
        var result = helper.Action(new UrlActionContext
        {
            Controller = "Home",
            Action = "Index",
            Values = new { culture = "en" }
        });

        // Explicit "en" should not be overridden by "zh-Hans"
        result.ShouldBe("/en/Home/Index");
    }

    [Fact]
    public void RouteUrl_Should_Inject_Culture()
    {
        var inner = Substitute.For<IUrlHelper>();
        inner.RouteUrl(Arg.Any<UrlRouteContext>()).Returns(callInfo =>
        {
            var ctx = callInfo.Arg<UrlRouteContext>();
            var values = new RouteValueDictionary(ctx.Values);
            return values.ContainsKey("culture") ? $"/{values["culture"]}/page" : "/page";
        });

        var helper = new AbpCultureAwareUrlHelper(inner, "tr");
        var result = helper.RouteUrl(new UrlRouteContext());

        result.ShouldBe("/tr/page");
    }

    [Fact]
    public void Content_Should_Pass_Through()
    {
        var inner = Substitute.For<IUrlHelper>();
        inner.Content("~/test").Returns("/test");

        var helper = new AbpCultureAwareUrlHelper(inner, "en");
        helper.Content("~/test").ShouldBe("/test");
    }

    [Fact]
    public void IsLocalUrl_Should_Pass_Through()
    {
        var inner = Substitute.For<IUrlHelper>();
        inner.IsLocalUrl("/test").Returns(true);

        var helper = new AbpCultureAwareUrlHelper(inner, "en");
        helper.IsLocalUrl("/test").ShouldBeTrue();
    }

    [Fact]
    public void Factory_Should_Return_CultureAwareHelper_When_Culture_In_Route()
    {
        var factory = CreateFactory(useRouteBasedCulture: true);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.RouteValues["culture"] = "tr";
        var actionContext = new ActionContext(httpContext, new RouteData(httpContext.Request.RouteValues), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        var urlHelper = factory.GetUrlHelper(actionContext);

        urlHelper.ShouldBeOfType<AbpCultureAwareUrlHelper>();
    }

    [Fact]
    public void Factory_Should_Return_Default_Helper_When_No_Culture()
    {
        var factory = CreateFactory(useRouteBasedCulture: true);
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        var urlHelper = factory.GetUrlHelper(actionContext);

        urlHelper.ShouldNotBeOfType<AbpCultureAwareUrlHelper>();
    }

    [Fact]
    public void Factory_Should_Return_Default_Helper_When_RouteBasedCulture_Disabled()
    {
        var factory = CreateFactory(useRouteBasedCulture: false);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.RouteValues["culture"] = "tr";
        var actionContext = new ActionContext(httpContext, new RouteData(httpContext.Request.RouteValues), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        // Even with culture in route, should not wrap when the feature is disabled
        var urlHelper = factory.GetUrlHelper(actionContext);

        urlHelper.ShouldNotBeOfType<AbpCultureAwareUrlHelper>();
    }

    private static AbpCultureRouteUrlHelperFactory CreateFactory(bool useRouteBasedCulture)
    {
        return new AbpCultureRouteUrlHelperFactory(
            new UrlHelperFactory(),
            Microsoft.Extensions.Options.Options.Create(new AbpRequestLocalizationOptions { UseRouteBasedCulture = useRouteBasedCulture }));
    }
}
