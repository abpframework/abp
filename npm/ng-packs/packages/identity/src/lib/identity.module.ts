import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { PermissionManagementModule } from '@abp/ng.permission-management';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { NgbDropdownModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { RolesComponent } from './components/roles/roles.component';
import { UsersComponent } from './components/users/users.component';
import { IdentityExtensionsGuard } from './guards/extensions.guard';
import { IdentityRoutingModule } from './identity-routing.module';
import { IdentityConfigOptions } from './models/config-options';
import {
  IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
  IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
  IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
  IDENTITY_ENTITY_PROP_CONTRIBUTORS,
  IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';
import { PageModule } from '@abp/ng.components/page';

@NgModule({
  declarations: [RolesComponent, UsersComponent],
  exports: [RolesComponent, UsersComponent],
  imports: [
    CoreModule,
    IdentityRoutingModule,
    NgbNavModule,
    ThemeSharedModule,
    ExtensibleModule,
    NgbDropdownModule,
    PermissionManagementModule,
    NgxValidateCoreModule,
    PageModule,
  ],
})
export class IdentityModule {
  static forChild(options: IdentityConfigOptions = {}): ModuleWithProviders<IdentityModule> {
    return {
      ngModule: IdentityModule,
      providers: [
        {
          provide: IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: IDENTITY_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
        IdentityExtensionsGuard,
      ],
    };
  }

  static forLazy(options: IdentityConfigOptions = {}): NgModuleFactory<IdentityModule> {
    return new LazyModuleFactory(IdentityModule.forChild(options));
  }
}
