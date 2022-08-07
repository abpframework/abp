using Microsoft.Net.Http.Headers;

namespace Volo.Abp.AspNetCore.Mvc.Json;

/// <summary>
/// https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.NewtonsoftJson/src/MediaTypeHeaderValues.cs
/// </summary>
internal static class MediaTypeHeaderValues
{
    public readonly static MediaTypeHeaderValue ApplicationJson = MediaTypeHeaderValue.Parse("application/json").CopyAsReadOnly();

    public readonly static MediaTypeHeaderValue TextJson = MediaTypeHeaderValue.Parse("text/json").CopyAsReadOnly();

    public readonly static MediaTypeHeaderValue ApplicationAnyJsonSyntax = MediaTypeHeaderValue.Parse("application/*+json").CopyAsReadOnly();

    public readonly static MediaTypeHeaderValue ApplicationXml = MediaTypeHeaderValue.Parse("application/xml").CopyAsReadOnly();

    public readonly static MediaTypeHeaderValue TextXml = MediaTypeHeaderValue.Parse("text/xml").CopyAsReadOnly();

    public readonly static MediaTypeHeaderValue ApplicationAnyXmlSyntax = MediaTypeHeaderValue.Parse("application/*+xml").CopyAsReadOnly();
}
