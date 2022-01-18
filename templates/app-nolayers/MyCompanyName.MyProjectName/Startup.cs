namespace MyCompanyName.MyProjectName;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<MyProjectNameModule>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.InitializeApplication();
    }
}