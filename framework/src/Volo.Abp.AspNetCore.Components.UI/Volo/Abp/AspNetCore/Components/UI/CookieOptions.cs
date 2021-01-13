using System;

namespace Volo.Abp.AspNetCore.Components.UI
{
    public class CookieOptions
    {
        public DateTimeOffset? ExpireDate { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
    }
}