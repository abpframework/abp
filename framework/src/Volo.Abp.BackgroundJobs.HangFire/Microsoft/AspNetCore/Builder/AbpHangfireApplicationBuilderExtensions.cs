using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs.Hangfire;

namespace Microsoft.AspNetCore.Builder;

public static class AbpHangfireApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAbpHangfireDashboard(
        this IApplicationBuilder app,
        string pathMatch = "/hangfire",
        Action<DashboardOptions>? configure = null,
        JobStorage? storage = null)
    {
        var options = app.ApplicationServices.GetRequiredService<AbpDashboardOptionsProvider>().Get();
        configure?.Invoke(options);
        return app.UseHangfireDashboard(pathMatch, options, storage);
    }
}