import snq from 'snq';
import { ApplicationLocalizationConfigurationDto } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';

// This will not be necessary when only Angukar 9.1+ is supported
export function getLocaleDirection(locale: string): 'ltr' | 'rtl' {
  return /^(ar(-[A-Z]{2})?|ckb(-IR)?|fa(-AF)?|he|ks|lrc(-IQ)?|mzn|pa-Arab|ps(-PK)?|sd|ug|ur(-IN)?|uz-Arab|yi)$/.test(
    locale,
  )
    ? 'rtl'
    : 'ltr';
}

export function createLocalizer(localization: ApplicationLocalizationConfigurationDto) {
  return (resourceName: string, key: string, defaultValue: string) => {
    if (resourceName === '_') return key;

    const resource = snq(() => localization.values[resourceName]);

    if (!resource) return defaultValue;

    return resource[key] || defaultValue;
  };
}

export function createLocalizerWithFallback(localization: ApplicationLocalizationConfigurationDto) {
  const findLocalization = createLocalizationFinder(localization);

  return (resourceNames: string[], keys: string[], defaultValue: string) => {
    const { localized } = findLocalization(resourceNames, keys);
    return localized || defaultValue;
  };
}

export function createLocalizationPipeKeyGenerator(
  localization: ApplicationLocalizationConfigurationDto,
) {
  const findLocalization = createLocalizationFinder(localization);

  return (resourceNames: string[], keys: string[], defaultKey: string) => {
    const { resourceName, key } = findLocalization(resourceNames, keys);
    return !resourceName ? defaultKey : resourceName === '_' ? key : `${resourceName}::${key}`;
  };
}

function createLocalizationFinder(localization: ApplicationLocalizationConfigurationDto) {
  const localize = createLocalizer(localization);

  return (resourceNames: string[], keys: string[]) => {
    resourceNames = resourceNames.concat(localization.defaultResourceName).filter(Boolean);

    const resourceCount = resourceNames.length;
    const keyCount = keys.length;

    for (let i = 0; i < resourceCount; i++) {
      const resourceName = resourceNames[i];

      for (let j = 0; j < keyCount; j++) {
        const key = keys[j];
        const localized = localize(resourceName, key, null);
        if (localized) return { resourceName, key, localized };
      }
    }

    return { resourceName: undefined, key: undefined, localized: undefined };
  };
}
