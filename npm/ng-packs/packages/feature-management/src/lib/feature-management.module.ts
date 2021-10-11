import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { FeatureManagementComponent } from './components/feature-management/feature-management.component';
import { FreeTextInputDirective } from './directives/free-text-input.directive';

const exported = [FeatureManagementComponent, FreeTextInputDirective];

@NgModule({
  declarations: [...exported],
  imports: [CoreModule, ThemeSharedModule, NgbNavModule],
  exports: [...exported],
})
export class FeatureManagementModule {}
