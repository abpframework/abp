using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Middleware;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Uow;

public class AbpUnitOfWorkMiddleware : AbpMiddlewareBase, ITransientDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly AbpAspNetCoreUnitOfWorkOptions _options;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;

    public AbpUnitOfWorkMiddleware(
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AbpAspNetCoreUnitOfWorkOptions> options,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _options = options.Value;
        _cancellationTokenProvider = cancellationTokenProvider;
    }

    public async override Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (await ShouldSkipAsync(context, next) || IsIgnoredUrl(context))
        {
            await next(context);
            return;
        }

        using (var uow = _unitOfWorkManager.Reserve(UnitOfWork.UnitOfWorkReservationName))
        {
            var completionStarted = false;

            if (!context.Response.HasStarted && ShouldCompleteOnResponseStarting(context))
            {
                context.Response.OnStarting(async () =>
                {
                    // Skip if the completion has already been started at the end of the pipeline;
                    // the response is then being started from inside that completion (e.g. by an
                    // event handler writing to the response), so completing again would fail.
                    // A nested (requiresNew) unit of work that is current and an active child
                    // unit of work scope are left to their owners; the request unit of work then
                    // completes at the end of the pipeline as usual.
                    if (!completionStarted &&
                        _unitOfWorkManager.Current == uow &&
                        !uow.HasActiveChildUnitOfWorks())
                    {
                        // Set before completing so a post-commit failure isn't masked by the completion below.
                        completionStarted = true;
                        await uow.CompleteAsync(_cancellationTokenProvider.Token);
                    }
                });
            }

            await next(context);

            if (!completionStarted)
            {
                completionStarted = true;
                await uow.CompleteAsync(_cancellationTokenProvider.Token);
            }
        }
    }

    private bool IsIgnoredUrl(HttpContext context)
    {
        return context.Request.Path.Value != null &&
               _options.IgnoredUrls.Any(x => context.Request.Path.Value.StartsWith(x, StringComparison.OrdinalIgnoreCase));
    }

    private bool ShouldCompleteOnResponseStarting(HttpContext context)
    {
        return _options.CompleteUnitOfWorkOnResponseStarting ||
               (context.Request.Path.Value != null &&
                _options.CompleteUnitOfWorkOnResponseStartingUrls.Any(x => context.Request.Path.Value.StartsWith(x, StringComparison.OrdinalIgnoreCase)));
    }

    protected async override Task<bool> ShouldSkipAsync(HttpContext context, RequestDelegate next)
    {
        // Blazor components will render concurrently, so we need to skip the middleware for them.
        // Otherwise, We will get the following exception:
        // A second operation started on this context before a previous operation completed.
        // This is usually caused by different threads using the same instance of DbContext.
        if (context.GetEndpoint()?.Metadata?.GetMetadata<RootComponentMetadata>() != null)
        {
            return true;
        }

        return await base.ShouldSkipAsync(context, next);
    }
}
