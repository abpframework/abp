## AbpLanguages

AbpLanguages is a table to store languages.

### Description

This table stores information about the languages supported in the application. Each record in the table represents a language and allows to manage and track the languages effectively. This table is important for supporting multiple languages in the application and for providing a better user experience by allowing users to switch between different languages.

### Module
    
[`Volo.Abp.Localization`](../../Localization.md)

---

## AbpLanguageTexts

AbpLanguageTexts is a table that stores the localization texts.

### Description

This table stores the localized text for different languages in the application. Each record in the table represents a localized text and allows to manage and track the localized texts effectively. This table is important for providing a better user experience by allowing the application to display text in the user's preferred language.

### Module

[`Volo.Abp.Localization`](../../Localization.md)

---

## AbpLocalizationResources

AbpLocalizationResources is a table that stores the localization resources.

### Description

This table stores the localization resources for the application. Each record in the table represents a localization resource and allows to manage and track the resources effectively. This table is important for providing a better user experience by allowing the application to support multiple resources and providing localized text and other localization-specific features.

### Module

[`Volo.Abp.Localization`](../../Localization.md)

---

## AbpLocalizationTexts

AbpLocalizationTexts is a table that stores the localization texts.

### Description

This table stores the localization texts in the application. Each record in the table represents a localization text for a specific resource, culture and language. The table contains the resource name, culture name, and a json encoded value which holds the key-value pair of localization text. It allows for efficient storage and management of localization texts and allows for easy update or addition of new translations for specific resources and cultures.

### Module

[`Volo.Abp.Localization`](../../Localization.md)