using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenIddict.Demo.API;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:44304")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:44301/connect/authorize"),
                TokenUrl = new Uri("https://localhost:44301/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "AbpAPI", "AbpAPI"}
                }
            }
        }
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement()
    {
        [new OpenApiSecuritySchemeReference("oauth2", document)] = []
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddAbpJwtBearer(options =>
    {
        options.Authority = "https://localhost:44301";
        options.Audience = "AbpAPIResource";
    });

await builder.AddApplicationAsync<OpenIddictApiModule>();
var app = builder.Build();
await app.InitializeApplicationAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenIddict.Demo.API");
        options.OAuthClientId("Swagger");
        options.OAuthScopes("AbpAPI");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseConfiguredEndpoints(options =>
{
    options.MapFallback("{**slug}", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
});
app.Run();
