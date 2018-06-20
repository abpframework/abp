using System;
using System.Collections.Generic;

namespace Volo.Abp.Localization.ExceptionHandling
{
    public class ExceptionLocalizationOptions
    {
        public Dictionary<string, Type> ErrorCodeNamespaceMappings { get; }

        public ExceptionLocalizationOptions()
        {
            ErrorCodeNamespaceMappings = new Dictionary<string, Type>();
        }

        public void MapCodeNamespace(string errorCodeNamespace, Type type)
        {
            ErrorCodeNamespaceMappings[errorCodeNamespace] = type;
        }
    }
}
