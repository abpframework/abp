using Volo.Abp.Collections;

namespace Volo.Abp.Json.SystemTextJson
{
    public class AbpSystemTextJsonSupportTypeMatcherOptions
    {
        public ITypeList UnsupportedTypes { get; }

        public AbpSystemTextJsonSupportTypeMatcherOptions()
        {
            UnsupportedTypes = new TypeList();
        }
    }
}
