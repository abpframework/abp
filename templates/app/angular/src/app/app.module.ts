import { CoreModule } from '@abp/ng.core';
import { LAYOUTS } from '@abp/ng.theme.basic';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { NgxsModule } from '@ngxs/store';
import { OAuthModule } from 'angular-oauth2-oidc';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { AccountConfigModule } from '@abp/ng.account.config';
import { IdentityConfigModule } from '@abp/ng.identity.config';
import { TenantManagementConfigModule } from '@abp/ng.tenant-management.config';
import { SettingManagementConfigModule } from '@abp/ng.setting-management.config';

const LOGGERS = [NgxsLoggerPluginModule.forRoot({ disabled: false })];

@NgModule({
  declarations: [AppComponent],
  imports: [
    CoreModule.forRoot({
      environment,
      requirements: {
        layouts: LAYOUTS,
      },
    }),
    ThemeSharedModule.forRoot(),
    OAuthModule.forRoot(),
    NgxsModule.forRoot([]),
    AccountConfigModule.forRoot({ redirectUrl: '/' }),
    IdentityConfigModule,
    TenantManagementConfigModule,
    SettingManagementConfigModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedModule,

    ...(environment.production ? [] : LOGGERS),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
