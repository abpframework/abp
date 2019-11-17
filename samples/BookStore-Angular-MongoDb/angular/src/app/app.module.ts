import { CoreModule } from '@abp/ng.core';
import { LAYOUTS } from '@abp/ng.theme.basic';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { NgxsModule } from '@ngxs/store';
import { OAuthModule } from 'angular-oauth2-oidc';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { AccountProviders } from '@abp/ng.account';
import { IdentityProviders } from '@abp/ng.identity';
import { TenantManagementProviders } from '@abp/ng.tenant-management';
import { BooksState } from './store/states/books.state';

@NgModule({
  declarations: [AppComponent],
  imports: [
    ThemeSharedModule.forRoot(),
    CoreModule.forRoot({
      environment,
      requirements: {
        layouts: LAYOUTS,
      },
    }),
    OAuthModule.forRoot(),
    NgxsModule.forRoot([]),
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedModule,
    NgxsModule.forRoot([BooksState, ]),
    NgxsReduxDevtoolsPluginModule.forRoot({ disabled: environment.production }),
  ],
  providers: [...AccountProviders({ redirectUrl: '/' }), ...IdentityProviders(), ...TenantManagementProviders()],
  bootstrap: [AppComponent],
})
export class AppModule {}
