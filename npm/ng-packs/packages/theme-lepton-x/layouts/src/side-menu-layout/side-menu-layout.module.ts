import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CoreModule } from '@abp/ng.core';
import { SideMenuLayoutModule as LpxSideMenuLayoutModule } from '@volo/ngx-lepton-x.lite/layouts';
import { NavbarModule } from '@volo/ngx-lepton-x.core';
import { SideMenuApplicationLayoutComponent } from './side-menu-application-layout/side-menu-application-layout.component';
import { LPX_LAYOUT_PROVIDER } from './providers/layout.provider';
import { LPX_NAVBAR_ITEMS_PROVIDER } from './providers/navbar-items.provider';

@NgModule({
  declarations: [SideMenuApplicationLayoutComponent],
  imports: [CommonModule, LpxSideMenuLayoutModule, RouterModule, NavbarModule, CoreModule],
})
export class SideMenuLayoutModule {
  static forRoot(): ModuleWithProviders<SideMenuLayoutModule> {
    return {
      ngModule: SideMenuLayoutModule,
      providers: [LPX_LAYOUT_PROVIDER, LPX_NAVBAR_ITEMS_PROVIDER],
    };
  }
}
