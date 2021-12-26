using Microsoft.Net.Http.Headers;

namespace Volo.Abp.AspNetCore.Mvc.Json;

/// <summary>
/// https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.NewtonsoftJson/src/MediaTypeHeaderValues.cs
/// </summary>
internal static class MediaTypeHeaderValues
{
    public static readonly MediaTypeHeaderValue ApplicationJson = MediaTypeHeaderValue.Parse("application/json").CopyAsReadOnly();

    public static readonly MediaTypeHeaderValue TextJson = MediaTypeHeaderValue.Parse("text/json").CopyAsReadOnly();

    public static readonly MediaTypeHeaderValue ApplicationAnyJsonSyntax = MediaTypeHeaderValue.Parse("application/*+json").CopyAsReadOnly();

    public static readonly MediaTypeHeaderValue ApplicationXml = MediaTypeHeaderValue.Parse("application/xml").CopyAsReadOnly();

    public static readonly MediaTypeHeaderValue TextXml = MediaTypeHeaderValue.Parse("text/xml").CopyAsReadOnly();

    public static readonly MediaTypeHeaderValue ApplicationAnyXmlSyntax = MediaTypeHeaderValue.Parse("application/*+xml").CopyAsReadOnly();
}
