using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Volo.Abp.TextTemplating.Razor;

public class AbpRazorTemplateCSharpCompilerOptions
{
    public List<PortableExecutableReference> References { get; }

    public AbpRazorTemplateCSharpCompilerOptions()
    {
        References = new List<PortableExecutableReference>();
    }
}
