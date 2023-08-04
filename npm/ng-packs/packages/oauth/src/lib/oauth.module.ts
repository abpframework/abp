import { APP_INITIALIZER, ModuleWithProviders, NgModule, Provider } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
import {
  AbpLocalStorageService,
  ApiInterceptor,
  AuthGuard,
  AuthService,
  CHECK_AUTHENTICATION_STATE_FN_KEY,
  noop,
  PIPE_TO_LOGIN_FN_KEY,
} from '@abp/ng.core';
import { AbpOAuthService } from './services';
import { OAuthConfigurationHandler } from './handlers/oauth-configuration.handler';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { OAuthApiInterceptor } from './interceptors/api.interceptor';
import { AbpOAuthGuard } from './guards/oauth.guard';
import { NavigateToManageProfileProvider } from './providers';
import { checkAccessToken, pipeToLogin } from './utils';

@NgModule({
  imports: [CommonModule, OAuthModule],
})
export class AbpOAuthModule {
  static forRoot(): ModuleWithProviders<AbpOAuthModule> {
    return {
      ngModule: AbpOAuthModule,
      providers: [
        {
          provide: AuthService,
          useClass: AbpOAuthService,
        },
        {
          provide: AuthGuard,
          useClass: AbpOAuthGuard,
        },
        {
          provide: ApiInterceptor,
          useClass: OAuthApiInterceptor,
        },
        {
          provide: PIPE_TO_LOGIN_FN_KEY,
          useValue: pipeToLogin,
        },
        {
          provide: CHECK_AUTHENTICATION_STATE_FN_KEY,
          useValue: checkAccessToken,
        },
        {
          provide: HTTP_INTERCEPTORS,
          useExisting: ApiInterceptor,
          multi: true,
        },
        NavigateToManageProfileProvider,
        {
          provide: APP_INITIALIZER,
          multi: true,
          deps: [OAuthConfigurationHandler],
          useFactory: noop,
        },
        OAuthModule.forRoot().providers as Provider[],
        { provide: OAuthStorage, useClass: AbpLocalStorageService },
      ],
    };
  }
}
