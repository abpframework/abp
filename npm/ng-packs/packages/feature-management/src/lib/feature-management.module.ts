import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule, makeEnvironmentProviders } from '@angular/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { FeatureManagementComponent } from './components/feature-management/feature-management.component';
import { FreeTextInputDirective } from './directives/free-text-input.directive';
import { FEATURE_MANAGEMENT_SETTINGS_PROVIDERS } from './providers';
import { FeatureManagementTabComponent } from './components';

const exported = [
  FeatureManagementComponent,
  FreeTextInputDirective,
  FeatureManagementTabComponent,
];

@NgModule({
  imports: [CoreModule, ThemeSharedModule, NgbNavModule, ...exported],
  exports: [...exported],
})
export class FeatureManagementModule {
  static forRoot(): ModuleWithProviders<FeatureManagementModule> {
    return {
      ngModule: FeatureManagementModule,
      providers: [FEATURE_MANAGEMENT_SETTINGS_PROVIDERS],
    };
  }
}

export function provideFeatureManagement() {
  return makeEnvironmentProviders([FEATURE_MANAGEMENT_SETTINGS_PROVIDERS]);
}
