# New Minimal APIs features in ASP.NET Core 8.0

In this article, we will see the new features of Minimal APIs in ASP.NET Core 8.0.

## Binding to forms

We can bind to forms using the [FromForm] attribute. Let's see an example:

```csharp
app.MapPost("/books", async ([FromForm] string name,
    [FromForm] BookType bookType, IFormFile? cover, BookDb db) =>
{
    var book = new Book
    {
        Name = name,
        BookType = bookType
    };

    if (cover is not null)
    {
        var coverName = Path.GetRandomFileName();

        using var stream = File.Create(Path.Combine("wwwroot", coverName));
        await cover.CopyToAsync(stream);
        book.Cover = coverName;
    }

    db.Books.Add(book);
    await db.SaveChangesAsync();

    return Results.Ok();
});
```

Another way is using the [AsParameters] attribute, the following code binds from form values to properties of the `NewBookRequest` record struct:

```csharp
public record NewBookRequest([FromForm] string Name, [FromForm] BookType BookType, IFormFile? Cover);

app.MapPost("/books", async ([AsParameters] NewBookRequest request, BookDb db) =>
{
    var book = new Book
    {
        Name = request.Name,
        BookType = request.BookType
    };

    if (request.Cover is not null)
    {
        var coverName = Path.GetRandomFileName();

        using var stream = File.Create(Path.Combine("wwwroot", coverName));
        await request.Cover.CopyToAsync(stream);
        book.Cover = coverName;
    }

    db.Books.Add(book);
    await db.SaveChangesAsync();

    return Results.Ok();
});
```

## Antiforgery

ASP.NET Core 8.0 adds support for antiforgery tokens. We can call the `AddAntiforgery` method to register the antiforgery services and `WebApplicationBuilder` will automatically add the antiforgery middleware to the pipeline:

```csharp
var builder = WebApplication.CreateBuilder();

builder.Services.AddAntiforgery();

var app = builder.Build();

// Implicitly added by WebApplicationBuilder if AddAntiforgery is called.
// app.UseAntiforgery();

app.MapGet("/", () => "Hello World!");

app.Run();
```

Example of using antiforgery tokens:

```csharp

// Use the antiforgery service to generate tokens.
app.MapGet("/", (HttpContext context, IAntiforgery antiforgery) =>
{
    var token = antiforgery.GetAndStoreTokens(context);
    return Results.Content(...., "text/html");
});

// It will automatically validate the token.
app.MapPost("/todo", ([FromForm] Todo todo) => Results.Ok(todo));

// Disable antiforgery validation for this endpoint.
app.MapPost("/todo2", ([FromForm] Todo todo) => Results.Ok(todo))
                                                .DisableAntiforgery();
```
 

## Native AOT

ASP.NET Core 8.0 adds support for Native AOT.

You can add `PublishAot` to the project file to enable Native AOT:

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
</PropertyGroup> 
```

### Web API (native AOT) template

You can use the `dotnet new webapiaot` command to create a new Minimal APIs project with Native AOT enabled.

```diff
+using System.Text.Json.Serialization;

-var builder = WebApplication.CreateBuilder();
+var builder = WebApplication.CreateSlimBuilder(args);

+builder.Services.ConfigureHttpJsonOptions(options =>
+{
+  options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
+});

var app = builder.Build();

var sampleTodos = TodoGenerator.GenerateTodos().ToArray();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();

+[JsonSerializable(typeof(Todo[]))]
+internal partial class AppJsonSerializerContext : JsonSerializerContext
+{
+
+}
```

* Reflection isn't supported in native AOT, you must use the `JsonSerializable` attribute to specify the types that you want to serialize/deserialize.

## References

* https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-8.0?view=aspnetcore-8.0#minimal-apis
* https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-8.0#explicit-binding-from-form-values
* https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot?view=aspnetcore-8.0
