import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CoreModule } from '@abp/ng.core';
import { SideMenuLayoutModule as LpxSideMenuLayoutModule } from '@volo/ngx-lepton-x.lite/layouts';
import { NavbarModule } from '@volo/ngx-lepton-x.core';
import { SideMenuApplicationLayoutComponent } from './side-menu-application-layout/side-menu-application-layout.component';
import { LPX_LAYOUT_PROVIDER } from './providers/layout.provider';
import { LPX_NAVBAR_ITEMS_PROVIDER } from './providers/navbar-items.provider';
import { NAV_ITEM_PROVIDER } from './providers/nav-item.provider';
import { NavItemsComponent } from './components/nav-items/nav-items.component';
import { LanguageSelectionComponent } from './components/language-selection/language-selection.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { LanguageSelectionModule, UserProfileModule } from '@volo/ngx-lepton-x.lite';

@NgModule({
  declarations: [
    SideMenuApplicationLayoutComponent,
    NavItemsComponent,
    LanguageSelectionComponent,
    UserProfileComponent,
  ],
  imports: [
    CommonModule,
    LpxSideMenuLayoutModule,
    RouterModule,
    NavbarModule,
    CoreModule,
    UserProfileModule,
    LanguageSelectionModule,
  ],
})
export class SideMenuLayoutModule {
  static forRoot(): ModuleWithProviders<SideMenuLayoutModule> {
    return {
      ngModule: SideMenuLayoutModule,
      providers: [LPX_LAYOUT_PROVIDER, LPX_NAVBAR_ITEMS_PROVIDER, NAV_ITEM_PROVIDER],
    };
  }
}
