using Volo.Abp.Collections;

namespace Volo.Abp.Json
{
    public class AbpJsonOptions
    {
        /// <summary>
        /// Used to set default value for the DateTimeFormat.
        /// </summary>
        public string DefaultDateTimeFormat { get; set; }

        public ITypeList<IJsonSerializerProvider> Providers { get; }

        public AbpJsonOptions()
        {
            Providers = new TypeList<IJsonSerializerProvider>();
        }
    }
}
