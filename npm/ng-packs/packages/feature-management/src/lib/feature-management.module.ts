import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { FreeTextInputDirective } from './directives/free-text-input.directive';
import { FeatureManagementComponent } from './components/feature-management/feature-management.component';
import { FeatureManagementState } from './states/feature-management.state';

const exported = [FeatureManagementComponent, FreeTextInputDirective];

@NgModule({
  declarations: [...exported],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbNavModule,
    NgxsModule.forFeature([FeatureManagementState]),
  ],
  exports: [...exported],
})
export class FeatureManagementModule {}
