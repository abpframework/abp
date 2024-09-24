using IdentityModel;
using OpenIddict.Demo.Client.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies", options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(365);
    })
    .AddAbpOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:44301/";
        options.RequireHttpsMetadata = true;
        options.ResponseType = OidcConstants.ResponseTypes.Code;

        options.ClientId = "AbpApp";
        options.ClientSecret = "1q2w3e*";

        options.UsePkce = true;
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.Scope.Add("email");
        options.Scope.Add("roles");
        options.Scope.Add("phone");
        options.Scope.Add("AbpAPI");
    });

await builder.AddApplicationAsync<OpenIddictMvcModule>();
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
app.MapAbpStaticAssets();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseConfiguredEndpoints();
app.Run();
