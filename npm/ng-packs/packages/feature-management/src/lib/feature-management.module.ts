import { ModuleWithProviders, NgModule } from '@angular/core';
import { FreeTextInputDirective } from './directives';
import { provideFeatureManagementConfig } from './providers';
import { FeatureManagementTabComponent, FeatureManagementComponent } from './components';

export const FEATURE_MANAGEMENT_EXPORTS = [
  FeatureManagementComponent,
  FreeTextInputDirective,
  FeatureManagementTabComponent,
];

/**
 * @deprecated FeatureManagementModule is deprecated .
 * @description use `provideFeatureManagementConfig` *function* for config settings.
 * You can import directives and pipes directly whichs were belongs to FeatureManagementModule are switched to standalone.
 */
@NgModule({
  imports: [...FEATURE_MANAGEMENT_EXPORTS],
  exports: [...FEATURE_MANAGEMENT_EXPORTS],
})
export class FeatureManagementModule {
  static forRoot(): ModuleWithProviders<FeatureManagementModule> {
    return {
      ngModule: FeatureManagementModule,
      providers: [provideFeatureManagementConfig()],
    };
  }
}
