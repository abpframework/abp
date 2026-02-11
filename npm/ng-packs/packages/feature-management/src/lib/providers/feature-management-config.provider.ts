import { makeEnvironmentProviders } from '@angular/core';
import { FEATURE_MANAGEMENT_SETTINGS_PROVIDERS } from './';

export function provideFeatureManagementConfig() {
  return makeEnvironmentProviders([FEATURE_MANAGEMENT_SETTINGS_PROVIDERS]);
}
