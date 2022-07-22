using Microsoft.AspNetCore.Mvc.Formatters;
using Volo.Abp.Collections;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public class AbpHybridJsonFormatterOptions
{
    public ITypeList<IAbpHybridJsonInputFormatter> TextInputFormatters { get; }

    public ITypeList<IAbpHybridJsonInputFormatter> TextOutputFormatters { get; }

    public AbpHybridJsonFormatterOptions()
    {
        TextInputFormatters = new TypeList<IAbpHybridJsonInputFormatter>();
        TextOutputFormatters = new TypeList<IAbpHybridJsonInputFormatter>();
    }
}
