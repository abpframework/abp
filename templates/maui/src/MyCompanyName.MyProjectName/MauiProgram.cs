using System.Reflection;
using Microsoft.Extensions.Configuration;
using Volo.Abp;

namespace MyCompanyName.MyProjectName;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddApplication<MyProjectNameModule>(options =>
        {
            options.UseAutofac();
            options.Services.ReplaceConfiguration(builder.Configuration);
        });

        var app = builder.Build();

        app.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().Initialize(app.Services);

        return app;
    }
}