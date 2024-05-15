import { CoreModule, provideAbpCore, withOptions } from '@abp/ng.core';
import { registerLocale } from '@abp/ng.core/locale';
import {
  InternetConnectionStatusComponent,
  ThemeSharedModule,
  provideAbpThemeShared,
} from '@abp/ng.theme.shared';
import { provideFeatureManagementConfig } from '@abp/ng.feature-management';
import { provideAbpOAuth } from '@abp/ng.oauth';
import { provideIdentityConfig } from '@abp/ng.identity/config';
import { provideSettingManagementConfig } from '@abp/ng.setting-management/config';
import { provideTenantManagementConfig } from '@abp/ng.tenant-management/config';
import { provideAccountConfig } from '@abp/ng.account/config';
import { ThemeLeptonXModule } from '@abp/ng.theme.lepton-x';
import { SideMenuLayoutModule } from '@abp/ng.theme.lepton-x/layouts';
import { AccountLayoutModule } from '@abp/ng.theme.lepton-x/account';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { APP_ROUTE_PROVIDER } from './route.provider';

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule,
    ThemeSharedModule,
    ThemeLeptonXModule.forRoot(),
    SideMenuLayoutModule.forRoot(),
    AccountLayoutModule.forRoot(),
    InternetConnectionStatusComponent,
  ],
  declarations: [AppComponent],
  providers: [
    APP_ROUTE_PROVIDER,
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocale(),
      })
    ),
    provideAbpOAuth(),
    provideAbpThemeShared(),
    provideSettingManagementConfig(),
    provideAccountConfig(),
    provideIdentityConfig(),
    provideTenantManagementConfig(),
    provideFeatureManagementConfig(),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
