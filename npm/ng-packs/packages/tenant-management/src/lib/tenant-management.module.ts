import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { TenantsComponent } from './components/tenants/tenants.component';
import { TenantManagementState } from './states/tenant-management.state';
import { TenantManagementRoutingModule } from './tenant-management-routing.module';

@NgModule({
  declarations: [TenantsComponent],
  exports: [TenantsComponent],
  imports: [
    TenantManagementRoutingModule,
    NgxsModule.forFeature([TenantManagementState]),
    NgxValidateCoreModule,
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    FeatureManagementModule,
  ],
})
export class TenantManagementModule {
  static forChild(): ModuleWithProviders<TenantManagementModule> {
    return {
      ngModule: TenantManagementModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<TenantManagementModule> {
    return new LazyModuleFactory(TenantManagementModule.forChild());
  }
}
