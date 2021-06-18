import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxsModule } from '@ngxs/store';
import { FeatureManagementComponent } from './components/feature-management/feature-management.component';
import { FreeTextInputDirective } from './directives/free-text-input.directive';

const exported = [FeatureManagementComponent, FreeTextInputDirective];

@NgModule({
  declarations: [...exported],
  imports: [CoreModule, ThemeSharedModule, NgbNavModule, NgxsModule.forFeature([])],
  exports: [...exported],
})
export class FeatureManagementModule {}
