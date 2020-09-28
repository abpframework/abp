import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { PermissionManagementModule } from '@abp/ng.permission-management';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { NgbDropdownModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { RolesComponent } from './components/roles/roles.component';
import { UsersComponent } from './components/users/users.component';
import { IdentityRoutingModule } from './identity-routing.module';
import { IdentityState } from './states/identity.state';

@NgModule({
  declarations: [RolesComponent, UsersComponent],
  exports: [RolesComponent, UsersComponent],
  imports: [
    NgxsModule.forFeature([IdentityState]),
    CoreModule,
    IdentityRoutingModule,
    NgbNavModule,
    ThemeSharedModule,
    NgbDropdownModule,
    PermissionManagementModule,
    NgxValidateCoreModule,
  ],
})
export class IdentityModule {
  static forChild(): ModuleWithProviders<IdentityModule> {
    return {
      ngModule: IdentityModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<IdentityModule> {
    return new LazyModuleFactory(IdentityModule.forChild());
  }
}
