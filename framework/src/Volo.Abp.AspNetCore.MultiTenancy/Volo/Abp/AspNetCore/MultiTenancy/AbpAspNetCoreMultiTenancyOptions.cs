using System;
using System.Globalization;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Volo.Abp.AspNetCore.MultiTenancy.Views;
using Volo.Abp.AspNetCore.RazorViews;
using Volo.Abp.Http;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.MultiTenancy;

public class AbpAspNetCoreMultiTenancyOptions
{
    /// <summary>
    /// Default: <see cref="TenantResolverConsts.DefaultTenantKey"/>.
    /// </summary>
    public string TenantKey { get; set; }

    /// <summary>
    /// Return true to stop the pipeline, false to continue.
    /// </summary>
    public Func<HttpContext, Exception, Task<bool>> MultiTenancyMiddlewareErrorPageBuilder { get; set; }

    public AbpAspNetCoreMultiTenancyOptions()
    {
        TenantKey = TenantResolverConsts.DefaultTenantKey;
        MultiTenancyMiddlewareErrorPageBuilder = async (context, exception) =>
        {
            var isCookieAuthentication = false;
            var tenantResolveResult = context.RequestServices.GetRequiredService<ITenantResolveResultAccessor>().Result;
            if (tenantResolveResult != null)
            {
                if (tenantResolveResult.AppliedResolvers.Count == 1 && tenantResolveResult.AppliedResolvers.Contains(CurrentUserTenantResolveContributor.ContributorName))
                {
                    var authenticationType = context.User.Identity?.AuthenticationType;
                    if (authenticationType != null)
                    {
                        var scheme = await context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>().GetHandlerAsync(context, authenticationType);
                        if (scheme is CookieAuthenticationHandler cookieAuthenticationHandler)
                        {
                            // Try to delete the authentication's cookie if it does not exist or is inactive.
                            await cookieAuthenticationHandler.SignOutAsync(null);
                            isCookieAuthentication = true;
                        }
                    }
                }

                var options = context.RequestServices.GetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>().Value;
                if (tenantResolveResult.AppliedResolvers.Contains(CookieTenantResolveContributor.ContributorName) ||
                    context.Request.Cookies.ContainsKey(options.TenantKey))
                {
                    // Try to delete the tenant's cookie if it does not exist or is inactive.
                    AbpMultiTenancyCookieHelper.SetTenantCookie(context, null, options.TenantKey);
                }
            }

            context.Response.Headers.Append("Abp-Tenant-Resolve-Error", HtmlEncoder.Default.Encode(exception.Message));
            if (isCookieAuthentication && context.Request.Method.Equals("Get", StringComparison.OrdinalIgnoreCase) && !context.Request.IsAjax())
            {
                context.Response.Redirect(context.Request.GetEncodedUrl());
            }
            else if (context.Request.IsAjax())
            {
                var error = new RemoteServiceErrorResponse(new RemoteServiceErrorInfo(exception.Message, exception is BusinessException businessException ? businessException.Details : string.Empty));

                var jsonSerializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;

                ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                    null,
                    context.Response.ContentType,
                    (new MediaTypeHeaderValue("application/json")
                    {
                        Encoding = Encoding.UTF8
                    }.ToString(), Encoding.UTF8),
                    MediaType.GetEncoding,
                    out var resolvedContentType,
                    out var resolvedContentTypeEncoding);

                context.Response.ContentType = resolvedContentType;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var cancellationTokenProvider = context.RequestServices.GetRequiredService<ICancellationTokenProvider>();
                var responseStream = context.Response.Body;
                if (resolvedContentTypeEncoding.CodePage == Encoding.UTF8.CodePage)
                {
                    try
                    {
                        await JsonSerializer.SerializeAsync(responseStream, error, error.GetType(), jsonSerializerOptions, cancellationTokenProvider.Token);
                        await responseStream.FlushAsync(cancellationTokenProvider.Token);
                    }
                    catch (OperationCanceledException) when (cancellationTokenProvider.Token.IsCancellationRequested) { }
                }
                else
                {
                    var transcodingStream = Encoding.CreateTranscodingStream(context.Response.Body, resolvedContentTypeEncoding, Encoding.UTF8, leaveOpen: true);
                    ExceptionDispatchInfo? exceptionDispatchInfo = null;
                    try
                    {
                        await JsonSerializer.SerializeAsync(transcodingStream, error, error.GetType(), jsonSerializerOptions, cancellationTokenProvider.Token);
                        await transcodingStream.FlushAsync(cancellationTokenProvider.Token);
                    }
                    catch (OperationCanceledException) when (cancellationTokenProvider.Token.IsCancellationRequested) { }
                    catch (Exception ex)
                    {
                        exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
                    }
                    finally
                    {
                        try
                        {
                            await transcodingStream.DisposeAsync();
                        }
                        catch when (exceptionDispatchInfo != null)
                        {
                        }
                        exceptionDispatchInfo?.Throw();
                    }
                }
            }
            else
            {
                var message = exception.Message;
                var details = exception is BusinessException businessException ? businessException.Details : string.Empty;

                var errorPage = new MultiTenancyMiddlewareErrorPage(new MultiTenancyMiddlewareErrorPageModel(message, details!));
                await errorPage.ExecuteAsync(context);
            }

            return true;
        };
    }
}
