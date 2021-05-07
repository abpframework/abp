import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideMenuApplicationLayoutComponent } from './side-menu-application-layout/side-menu-application-layout.component';
import { SideMenuLayoutModule as LpxSideMenuLayoutModule } from '@lepton-x/lite/layouts';
import { LpxToolbarModule } from '@lepton-x/lite';
import { NavbarModule, ToolbarModule } from '@lepton-x/common';
import { LPX_LAYOUT_PROVIDER } from './providers/layout.provider';
import { RouterModule } from '@angular/router';
import { LPX_NAVBAR_ITEMS_PROVIDER } from './providers/navbar-items.provider';
import { CoreModule } from '@abp/ng.core';

@NgModule({
  declarations: [SideMenuApplicationLayoutComponent],
  imports: [
    CommonModule,
    LpxSideMenuLayoutModule,
    RouterModule,
    NavbarModule,
    ToolbarModule,
    LpxToolbarModule,
    CoreModule,
  ],
})
export class SideMenuLayoutModule {
  static forRoot(): ModuleWithProviders<SideMenuLayoutModule> {
    return {
      ngModule: SideMenuLayoutModule,
      providers: [LPX_LAYOUT_PROVIDER, LPX_NAVBAR_ITEMS_PROVIDER],
    };
  }
}
