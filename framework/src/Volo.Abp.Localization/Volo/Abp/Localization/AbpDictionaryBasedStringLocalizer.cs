using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Resources;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization
{
    public class AbpDictionaryBasedStringLocalizer : IStringLocalizer, IStringLocalizerSupportsInheritance
    {
        public LocalizationResource Resource { get; }

        public List<IStringLocalizer> BaseLocalizers { get; }

        public virtual LocalizedString this[string name] => GetLocalizedString(name);

        public virtual LocalizedString this[string name, params object[] arguments] => GetLocalizedStringFormatted(name, arguments);

        public AbpDictionaryBasedStringLocalizer(LocalizationResource resource, List<IStringLocalizer> baseLocalizers)
        {
            Resource = resource;
            BaseLocalizers = baseLocalizers;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return GetAllStrings(
                CultureInfo.CurrentUICulture.Name,
                includeParentCultures
            );
        }
        
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, bool includeBaseLocalizers)
        {
            return GetAllStrings(
                CultureInfo.CurrentUICulture.Name,
                includeParentCultures,
                includeBaseLocalizers
            );
        }

        [Obsolete("This method is obsolete. Use `CurrentCulture` and `CurrentUICulture` instead.")]
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new CultureWrapperStringLocalizer(culture.Name, this);
        }

        protected virtual LocalizedString GetLocalizedStringFormatted(string name, params object[] arguments)
        {
            return GetLocalizedStringFormatted(name, CultureInfo.CurrentUICulture.Name, arguments);
        }

        protected virtual LocalizedString GetLocalizedStringFormatted(string name, string cultureName, params object[] arguments)
        {
            var localizedString = GetLocalizedString(name, cultureName);
            return new LocalizedString(name, string.Format(localizedString.Value, arguments), localizedString.ResourceNotFound, localizedString.SearchedLocation);
        }

        protected virtual LocalizedString GetLocalizedString(string name)
        {
            return GetLocalizedString(name, CultureInfo.CurrentUICulture.Name);
        }

        protected virtual LocalizedString GetLocalizedString(string name, string cultureName)
        {
            var value = GetLocalizedStringOrNull(name, cultureName);

            if (value == null)
            {
                foreach (var baseLocalizer in BaseLocalizers)
                {
                    using (CultureHelper.Use(CultureInfo.GetCultureInfo(cultureName)))
                    {
                        var baseLocalizedString = baseLocalizer[name];
                        if (baseLocalizedString != null && !baseLocalizedString.ResourceNotFound)
                        {
                            return baseLocalizedString;
                        }
                    }
                }

                return new LocalizedString(name, name, resourceNotFound: true);
            }

            return value;
        }

        protected virtual LocalizedString GetLocalizedStringOrNull(string name, string cultureName, bool tryDefaults = true)
        {
            //Try to get from original dictionary (with country code)
            var strOriginal = Resource.Contributors.GetOrNull(cultureName, name);
            if (strOriginal != null)
            {
                return strOriginal;
            }

            if (!tryDefaults)
            {
                return null;
            }

            //Try to get from same language dictionary (without country code)
            if (cultureName.Contains("-")) //Example: "tr-TR" (length=5)
            {
                var strLang = Resource.Contributors.GetOrNull(GetBaseCultureName(cultureName), name);
                if (strLang != null)
                {
                    return strLang;
                }
            }

            //Try to get from default language
            if (!Resource.DefaultCultureName.IsNullOrEmpty())
            {
                var strDefault = Resource.Contributors.GetOrNull(Resource.DefaultCultureName, name);
                if (strDefault != null)
                {
                    return strDefault;
                }
            }

            //Not found
            return null;
        }

        protected virtual IReadOnlyList<LocalizedString> GetAllStrings(
            string cultureName, 
            bool includeParentCultures = true,
            bool includeBaseLocalizers = true)
        {
            //TODO: Can be optimized (example: if it's already default dictionary, skip overriding)

            var allStrings = new Dictionary<string, LocalizedString>();

            if (includeBaseLocalizers)
            {
                foreach (var baseLocalizer in BaseLocalizers.Select(l => l))
                {
                    using (CultureHelper.Use(CultureInfo.GetCultureInfo(cultureName)))
                    {
                        //TODO: Try/catch is a workaround here!
                        try
                        {
                            var baseLocalizedString = baseLocalizer.GetAllStrings(includeParentCultures);
                            foreach (var localizedString in baseLocalizedString)
                            {
                                allStrings[localizedString.Name] = localizedString;
                            }
                        }
                        catch (MissingManifestResourceException)
                        {

                        }
                    }
                }
            }

            if (includeParentCultures)
            {
                //Fill all strings from default culture
                if (!Resource.DefaultCultureName.IsNullOrEmpty())
                {
                    Resource.Contributors.Fill(Resource.DefaultCultureName, allStrings);
                }

                //Overwrite all strings from the language based on country culture
                if (cultureName.Contains("-"))
                {
                    Resource.Contributors.Fill(GetBaseCultureName(cultureName), allStrings);
                }
            }

            //Overwrite all strings from the original culture
            Resource.Contributors.Fill(cultureName, allStrings);

            return allStrings.Values.ToImmutableList();
        }

        protected virtual string GetBaseCultureName(string cultureName)
        {
            return cultureName.Contains("-")
                ? cultureName.Left(cultureName.IndexOf("-", StringComparison.Ordinal))
                : cultureName;
        }

        public class CultureWrapperStringLocalizer : IStringLocalizer, IStringLocalizerSupportsInheritance
        {
            private readonly string _cultureName;
            private readonly AbpDictionaryBasedStringLocalizer _innerLocalizer;

            LocalizedString IStringLocalizer.this[string name] => _innerLocalizer.GetLocalizedString(name, _cultureName);

            LocalizedString IStringLocalizer.this[string name, params object[] arguments] => _innerLocalizer.GetLocalizedStringFormatted(name, _cultureName, arguments);

            public CultureWrapperStringLocalizer(string cultureName, AbpDictionaryBasedStringLocalizer innerLocalizer)
            {
                _cultureName = cultureName;
                _innerLocalizer = innerLocalizer;
            }

            public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            {
                return _innerLocalizer.GetAllStrings(_cultureName, includeParentCultures);
            }

            [Obsolete("This method is obsolete. Use `CurrentCulture` and `CurrentUICulture` instead.")]
            public IStringLocalizer WithCulture(CultureInfo culture)
            {
                return new CultureWrapperStringLocalizer(culture.Name, _innerLocalizer);
            }

            public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, bool includeBaseLocalizers)
            {
                return _innerLocalizer.GetAllStrings(_cultureName, includeParentCultures, includeBaseLocalizers);
            }
        }
    }
}