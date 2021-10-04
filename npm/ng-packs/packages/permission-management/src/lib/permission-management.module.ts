import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { PermissionManagementComponent } from './components/permission-management.component';

@NgModule({
  declarations: [PermissionManagementComponent],
  imports: [CoreModule, ThemeSharedModule],
  exports: [PermissionManagementComponent],
})
export class PermissionManagementModule {}
