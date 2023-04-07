using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.ResponseCache;

/// <summary>
/// Specifies the parameters necessary for setting appropriate headers in response caching.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AbpResponseCacheAttribute : Attribute, IFilterFactory, IOrderedFilter
{
    // A nullable-int cannot be used as an Attribute parameter.
    // Hence this nullable-int is present to back the Duration property.
    // The same goes for nullable-ResponseCacheLocation and nullable-bool.
    private int? _duration;
    private ResponseCacheLocation? _location;
    private bool? _noStore;

    /// <summary>
    /// Gets or sets the duration in seconds for which the response is cached.
    /// This sets "max-age" in "Cache-control" header.
    /// </summary>
    public int Duration
    {
        get => _duration ?? 0;
        set => _duration = value;
    }

    /// <summary>
    /// Gets or sets the location where the data from a particular URL must be cached.
    /// </summary>
    public ResponseCacheLocation Location
    {
        get => _location ?? ResponseCacheLocation.Any;
        set => _location = value;
    }

    /// <summary>
    /// Gets or sets the value which determines whether the data should be stored or not.
    /// When set to <see langword="true"/>, it sets "Cache-control" header to "no-store".
    /// Ignores the "Location" parameter for values other than "None".
    /// Ignores the "duration" parameter.
    /// </summary>
    public bool NoStore
    {
        get => _noStore ?? false;
        set => _noStore = value;
    }

    /// <summary>
    /// Gets or sets the value for the Vary response header.
    /// </summary>
    public string VaryByHeader { get; set; }

    /// <summary>
    /// Gets or sets the query keys to vary by.
    /// </summary>
    /// <remarks>
    /// <see cref="VaryByQueryKeys"/> requires the response cache middleware.
    /// </remarks>
    public string[] VaryByQueryKeys { get; set; }

    /// <summary>
    /// Gets or sets the value of the cache profile name.
    /// </summary>
    public string CacheProfileName { get; set; }

    public int Order { get; set; }

    public bool IsReusable => true;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext != null && httpContext.Features.Get<IResponseCachingFeature>() != null)
        {
            var responseCacheAttribute = new ResponseCacheAttribute()
            {
                VaryByHeader = VaryByHeader,
                VaryByQueryKeys = VaryByQueryKeys,
                CacheProfileName = CacheProfileName,
                Order = Order
            };
            if (_duration.HasValue)
            {
                responseCacheAttribute.Duration = _duration.Value;
            }
            if (_location.HasValue)
            {
                responseCacheAttribute.Location = _location.Value;
            }
            if (_noStore.HasValue)
            {
                responseCacheAttribute.NoStore = _noStore.Value;
            }
            return responseCacheAttribute.CreateInstance(serviceProvider);
        }

        return new AbpNullResponseCacheFilter();
    }
}
