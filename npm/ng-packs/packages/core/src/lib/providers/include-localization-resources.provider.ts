import { Provider } from '@angular/core';
import { INCUDE_LOCALIZATION_RESOURCES_TOKEN } from '../tokens/include-localization-resources.token';

export const IncludeLocalizationResourcesProvider: Provider = {
  provide: INCUDE_LOCALIZATION_RESOURCES_TOKEN,
  useValue: false,
};
