using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

public class AbpAntiForgeryOptions
{
    /// <summary>
    /// Use to set the cookie options to transfer Anti Forgery token between server and client.
    /// Default name of the cookie: "XSRF-TOKEN".
    /// </summary>
    public CookieBuilder TokenCookie { get; }

    /// <summary>
    /// Used to find auth cookie when validating Anti Forgery token.
    /// Default value: "Identity.Application".
    /// </summary>
    public string AuthCookieSchemaName { get; set; }

    /// <summary>
    /// Default value: true.
    /// </summary>
    public bool AutoValidate { get; set; } = true;

    /// <summary>
    /// A predicate to filter types to auto-validate.
    /// Return true to select the type to validate.
    /// Default: returns true for all given types.
    /// </summary>
    [NotNull]
    public Predicate<Type> AutoValidateFilter
    {
        get => _autoValidateFilter;
        set => _autoValidateFilter = Check.NotNull(value, nameof(value));
    }
    private Predicate<Type> _autoValidateFilter;

    /// <summary>
    /// Default methods: "GET", "HEAD", "TRACE", "OPTIONS".
    /// </summary>
    [NotNull]
    public HashSet<string> AutoValidateIgnoredHttpMethods
    {
        get => _autoValidateIgnoredHttpMethods;
        set => _autoValidateIgnoredHttpMethods = Check.NotNull(value, nameof(value));
    }
    private HashSet<string> _autoValidateIgnoredHttpMethods;

    public AbpAntiForgeryOptions()
    {
        AutoValidateFilter = type => true;

        TokenCookie = new CookieBuilder
        {
            Name = "XSRF-TOKEN",
            HttpOnly = false,
            IsEssential = true,
            SameSite = SameSiteMode.None,
            Expiration = TimeSpan.FromDays(3650) //10 years!
        };

        AuthCookieSchemaName = "Identity.Application";

        AutoValidateIgnoredHttpMethods = new HashSet<string> { "GET", "HEAD", "TRACE", "OPTIONS" };
    }
}
