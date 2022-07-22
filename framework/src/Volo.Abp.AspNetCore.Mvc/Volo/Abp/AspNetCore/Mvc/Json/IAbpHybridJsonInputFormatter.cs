using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public interface IAbpHybridJsonInputFormatter
{
    Task<bool> CanHandleAsync(Type type);

    Task<TextInputFormatter> GetTextInputFormatterAsync();
}
