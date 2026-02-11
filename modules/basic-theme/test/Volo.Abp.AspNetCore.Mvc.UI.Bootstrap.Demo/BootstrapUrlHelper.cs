using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo;

public static class BootstrapUrlHelper
{
    public const string BootstrapVersion = "5.3";
    public static string GetDocUrl(string path)
    {
        return $"https://getbootstrap.com/docs/{BootstrapVersion}{path.EnsureStartsWith('/')}";
    }
}