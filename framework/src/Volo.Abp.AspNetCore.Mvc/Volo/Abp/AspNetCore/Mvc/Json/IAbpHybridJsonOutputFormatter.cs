using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public interface IAbpHybridJsonOutputFormatter
{
    Task<bool> CanHandleAsync(Type type);

    Task<TextOutputFormatter> GetTextOutputFormatterAsync();
}
