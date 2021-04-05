using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Volo.Abp.TextTemplating.Razor
{
    public class CSharpCompilerOptions
    {
        public List<PortableExecutableReference> References { get; }

        public CSharpCompilerOptions()
        {
            References = new List<PortableExecutableReference>();
        }
    }
}
