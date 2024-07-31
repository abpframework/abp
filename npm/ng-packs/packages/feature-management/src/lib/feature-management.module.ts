import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { FeatureManagementComponent } from './components/feature-management/feature-management.component';
import { FreeTextInputDirective } from './directives/free-text-input.directive';
import { provideFeatureManagementConfig } from './providers';
import { FeatureManagementTabComponent } from './components';

const exported = [
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
  imports: [CoreModule, ThemeSharedModule, NgbNavModule, ...exported],
  exports: [...exported],
})
export class FeatureManagementModule {
  static forRoot(): ModuleWithProviders<FeatureManagementModule> {
    return {
      ngModule: FeatureManagementModule,
      providers: [provideFeatureManagementConfig()],
    };
  }
}
