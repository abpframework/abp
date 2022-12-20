import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OAuthModule as OAuthModule2, OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { TimeoutLimitedOAuthService } from '@abp/ng.core';
import { storageFactory } from './utils/storage.factory';

@NgModule({
  imports: [CommonModule, OAuthModule2],
})
export class OAuthModule {
  static forRoot(): ModuleWithProviders<OAuthModule> {
    return {
      ngModule: OAuthModule,
      providers: [
        OAuthModule2.forRoot().providers,
        { provide: OAuthStorage, useFactory: storageFactory },
        { provide: OAuthService, useClass: TimeoutLimitedOAuthService },
      ],
    };
  }
}
