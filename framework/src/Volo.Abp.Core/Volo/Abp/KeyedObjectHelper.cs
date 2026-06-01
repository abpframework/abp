using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp;

public static class KeyedObjectHelper
{
    public static string EncodeCompositeKey(params object?[] keys)
    {
        var raw = keys.JoinAsString("||");
        var bytes = Encoding.UTF8.GetBytes(raw);
        var base64 = Convert.ToBase64String(bytes);
        var base64Url = base64
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
        
        return base64Url;
    }
    
    public static string DecodeCompositeKey(string encoded)
    {
        var base64 = encoded
            .Replace("-", "+")
            .Replace("_", "/");

        switch (encoded.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        var bytes = Convert.FromBase64String(base64);
        var raw = Encoding.UTF8.GetString(bytes);

        return raw;
    }
}