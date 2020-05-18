import { ApplicationConfiguration } from '../models/application-configuration';

export function localize(resourceName: string, key: string, defaultValue: string) {
  return function(localization: ApplicationConfiguration.Localization) {
    if (resourceName === '_') return key;

    const resource = localization.values[resourceName];

    if (!resource) return defaultValue;

    return resource[key] || defaultValue;
  };
}

export function localizeWithFallback(
  resourceNames: string[],
  keys: string[],
  defaultValue: string,
) {
  return function(localization: ApplicationConfiguration.Localization) {
    resourceNames = resourceNames.concat(localization.defaultResourceName).filter(Boolean);

    for (let i = 0; i < resourceNames.length; i++) {
      const resourceName = resourceNames[i];

      for (let j = 0; j < keys.length; j++) {
        const localized = localize(resourceName, keys[j], null)(localization);
        if (localized) return localized;
      }
    }

    return defaultValue;
  };
}
