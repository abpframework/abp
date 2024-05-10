import { makeEnvironmentProviders } from '@angular/core';
import { FEATURE_MANAGEMENT_SETTINGS_PROVIDERS } from './feature-management-settings.provider';

export function provideFeatureManagementConfig() {
  return makeEnvironmentProviders([FEATURE_MANAGEMENT_SETTINGS_PROVIDERS]);
}
