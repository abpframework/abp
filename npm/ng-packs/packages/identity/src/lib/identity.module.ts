import { LazyModuleFactory } from '@abp/ng.core';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { IdentityExtensionsGuard } from './guards/extensions.guard';
import { IdentityConfigOptions } from './models/config-options';
import {
  IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
  IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
  IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
  IDENTITY_ENTITY_PROP_CONTRIBUTORS,
  IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';
import { IdentityRoutingModule } from './identity-routing.module';
import { RolesComponent, UsersComponent } from './components';

@NgModule({
  declarations: [],
  exports: [],
  imports: [IdentityRoutingModule, RolesComponent, UsersComponent],
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
  /**
   * @deprecated `IdentityModule.forLazy()` is deprecated. You can use `createRoutes` **function** instead.
   */
  static forLazy(options: IdentityConfigOptions = {}): NgModuleFactory<IdentityModule> {
    return new LazyModuleFactory(IdentityModule.forChild(options));
  }
}
