import { ApplicationConfiguration } from '../models/application-configuration';

export function localize(resourceName: string, key: string, defaultValue: string) {
  /* tslint:disable-next-line:only-arrow-functions */
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
  /* tslint:disable-next-line:only-arrow-functions */
  return function(localization: ApplicationConfiguration.Localization) {
    resourceNames = resourceNames.concat(localization.defaultResourceName).filter(Boolean);

    const resourceCount = resourceNames.length;
    const keyCount = keys.length;

    for (let i = 0; i < resourceCount; i++) {
      const resourceName = resourceNames[i];

      for (let j = 0; j < keyCount; j++) {
        const localized = localize(resourceName, keys[j], null)(localization);
        if (localized) return localized;
      }
    }

    return defaultValue;
  };
}
