using System;
using System.Collections.Generic;

namespace Volo.Abp.Localization.ExceptionHandling
{
    public class ExceptionLocalizationOptions
    {
        public Dictionary<string, Type> CodeNamespaceMappings { get; }

        public ExceptionLocalizationOptions()
        {
            CodeNamespaceMappings = new Dictionary<string, Type>();
        }

        public void MapCodeNamespace(string codeNamespace, Type type)
        {
            CodeNamespaceMappings[codeNamespace] = type;
        }
    }
}
