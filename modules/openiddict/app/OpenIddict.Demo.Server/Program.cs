using OpenIddict.Demo.Server;
using Volo.Abp.OpenIddict.Jwt;
using Volo.Abp.Users;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "https://localhost:44304")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();;
        });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Host.UseAutofac();

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:44301";
        options.Audience = "AbpAPIResource";

        options.MapInboundClaims = false;
    });

await builder.AddApplicationAsync<OpenIddictServerModule>();

var app = builder.Build();
await app.InitializeApplicationAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.UseJwtTokenMiddleware();
app.UseAuthentication();
app.UseMultiTenancy();
app.UseAuthorization();

app.UseConfiguredEndpoints();

app.Run();
