import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { FeatureManagementComponent } from './components/feature-management/feature-management.component';
import { NgxsModule } from '@ngxs/store';
import { FeatureManagementState } from './states/feature-management.state';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [FeatureManagementComponent],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbNavModule,
    NgxsModule.forFeature([FeatureManagementState]),
  ],
  exports: [FeatureManagementComponent],
})
export class FeatureManagementModule {}
