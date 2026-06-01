using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;

namespace Volo.Abp.MudBlazorUI;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpGlobalFeaturesModule),
    typeof(AbpFeaturesModule)
)]
public class AbpMudBlazorUIModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureMudBlazor(context);
    }

    private void ConfigureMudBlazor(ServiceConfigurationContext context)
    {
        context.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomEnd;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        context.Services.AddSingleton(typeof(AbpMudBlazorMessageLocalizerHelper<>));
    }
}
