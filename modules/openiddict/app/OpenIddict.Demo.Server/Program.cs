using System.Text;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Demo.Server;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Localization;
using Volo.Abp.OpenIddict.Jwt;

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

builder.Services.Configure<AbpLocalizationOptions>(options =>
{
    options.Languages.Add(new LanguageInfo("en", "en", "English"));
    options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
    options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
    options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
});

// Use Microsoft.AspNetCore.Authentication.JwtBearer instead of OpenIddict.Validation.AspNetCore
// builder.Services.AddAuthentication()
//     .AddJwtBearer(options =>
//     {
//         options.Authority = "https://localhost:44301";
//         options.Audience = "AbpAPIResource";
//
//         options.MapInboundClaims = false;
//
//         // See OpenIddictServerModule`s PreConfigureServices method.
//         options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Abp_OpenIddict_Demo_C40DBB176E78"));
//         options.TokenValidationParameters.TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Abp_OpenIddict_Demo_87E33FC57D80"));
//     });

await builder.AddApplicationAsync<OpenIddictServerModule>();

var app = builder.Build();
await app.InitializeApplicationAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAbpRequestLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseErrorPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

// Use Microsoft.AspNetCore.Authentication.JwtBearer instead of OpenIddict.Validation.AspNetCore
//app.UseJwtTokenMiddleware();

app.UseAuthentication();
app.UseMultiTenancy();
app.UseAuthorization();

app.UseConfiguredEndpoints();

app.Run();
