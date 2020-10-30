using System;
using Volo.Abp.Collections;

namespace Volo.Abp.Json.SystemTextJson
{
    public class SystemTextJsonSupportTypesOptions
    {
        public ITypeList<Attribute> IgnoreAttributes;

        public SystemTextJsonSupportTypesOptions()
        {
            IgnoreAttributes = new TypeList<Attribute>();
        }
    }
}
