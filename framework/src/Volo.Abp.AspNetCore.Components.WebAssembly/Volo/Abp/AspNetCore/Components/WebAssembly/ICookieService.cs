﻿using System;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public interface ICookieService
    {
        public ValueTask SetAsync(string key, string value, CookieOptions options = null);
        public ValueTask<string> GetAsync(string key);
        public ValueTask DeleteAsync(string key, string path = null);
    }
}