using System;

namespace Volo.Abp.Localization
{
    public class ShortLocalizationResourceNameAttribute : Attribute
    {
        public string Name { get; }

        public ShortLocalizationResourceNameAttribute(string name)
        {
            Name = name;
        }
    }
}