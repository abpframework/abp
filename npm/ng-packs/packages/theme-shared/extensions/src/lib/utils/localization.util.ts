import { ApplicationConfiguration, createLocalizationPipeKeyGenerator } from '@abp/ng.core';
import { ObjectExtensions } from '../models/object-extensions';

export function createDisplayNameLocalizationPipeKeyGenerator(
  localization: ApplicationConfiguration.Localization,
) {
  const generateLocalizationPipeKey = createLocalizationPipeKeyGenerator(localization);

  return (displayName: ObjectExtensions.DisplayName, fallback: ObjectExtensions.DisplayName) => {
    if (displayName && displayName.name)
      return generateLocalizationPipeKey(
        [displayName.resource],
        [displayName.name],
        displayName.name,
      );

    const key = generateLocalizationPipeKey(
      [fallback.resource],
      ['DisplayName:' + fallback.name],
      undefined,
    );

    if (key) return key;

    return generateLocalizationPipeKey([fallback.resource], [fallback.name], fallback.name);
  };
}
