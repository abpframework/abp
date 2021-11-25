using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Volo.Abp.TextTemplating.Razor;

public class AbpCompiledViewProviderOptions
{
    public Dictionary<string, List<PortableExecutableReference>> TemplateReferences { get; }

    public AbpCompiledViewProviderOptions()
    {
        TemplateReferences = new Dictionary<string, List<PortableExecutableReference>>();
    }
}
