import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { PermissionManagementComponent } from './components/permission-management.component';
import { PermissionManagementState } from './states/permission-management.state';

@NgModule({
  declarations: [PermissionManagementComponent],
  imports: [CoreModule, ThemeSharedModule, NgxsModule.forFeature([PermissionManagementState])],
  exports: [PermissionManagementComponent],
})
export class PermissionManagementModule {}
