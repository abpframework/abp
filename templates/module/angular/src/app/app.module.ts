import { AccountConfigModule } from '@abp/ng.account.config';
import { CoreModule } from '@abp/ng.core';
import { IdentityConfigModule } from '@abp/ng.identity.config';
import { SettingManagementConfigModule } from '@abp/ng.setting-management.config';
import { TenantManagementConfigModule } from '@abp/ng.tenant-management.config';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { NgxsModule } from '@ngxs/store';
import { OAuthModule } from 'angular-oauth2-oidc';
import { MyProjectNameConfigModule } from '../../projects/my-project-name-config/src/public-api';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';

const LOGGERS = [NgxsLoggerPluginModule.forRoot({ disabled: false })];

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    ThemeSharedModule.forRoot(),
    CoreModule.forRoot({
      environment,
    }),
    OAuthModule.forRoot(),
    NgxsModule.forRoot([]),
    AccountConfigModule.forRoot({ redirectUrl: '/' }),
    IdentityConfigModule,
    TenantManagementConfigModule,
    SettingManagementConfigModule,
    MyProjectNameConfigModule,
    SharedModule,

    ...(environment.production ? [] : LOGGERS),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
