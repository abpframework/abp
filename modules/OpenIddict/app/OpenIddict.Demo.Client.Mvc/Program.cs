using IdentityModel;
using Microsoft.AspNetCore.Identity;

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
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:44301/";

        options.ClientId = "MyClient";
        options.ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654";

        options.RequireHttpsMetadata = false;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        options.ResponseType = OidcConstants.ResponseTypes.Code;

        options.SignOutScheme = "Cookies";

        options.Scope.Add("email");
        options.Scope.Add("roles");
    });

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

