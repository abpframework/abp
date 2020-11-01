using System;
using Volo.Abp.Collections;

namespace Volo.Abp.Json.SystemTextJson
{
    public class SystemTextJsonSupportTypeMatcherOptions
    {
        public ITypeList<Attribute> UnsupportedAttributes { get; }

        public ITypeList UnsupportedTypes { get; }

        public SystemTextJsonSupportTypeMatcherOptions()
        {
            UnsupportedAttributes = new TypeList<Attribute>();
            UnsupportedTypes = new TypeList();
        }
    }
}
