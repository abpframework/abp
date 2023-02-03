import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
import {
  ApiInterceptor,
  AuthGuard,
  AuthService,
  CHECK_AUTHENTICATION_STATE_FN_KEY,
  noop,
  PIPE_TO_LOGIN_FN_KEY,
  SET_TOKEN_RESPONSE_TO_STORAGE_FN_KEY,
} from '@abp/ng.core';
import { storageFactory } from './utils/storage.factory';
import { AbpOAuthService } from './services';
import { OAuthConfigurationHandler } from './handlers/oauth-configuration.handler';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { OAuthApiInterceptor } from './interceptors/api.interceptor';
import { AbpOAuthGuard } from './guards/oauth.guard';
import { NavigateToManageProfileProvider } from './providers';
import { checkAccessToken, pipeToLogin, setTokenResponseToStorage } from './utils';

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
          provide: SET_TOKEN_RESPONSE_TO_STORAGE_FN_KEY,
          useValue: setTokenResponseToStorage,
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
        OAuthModule.forRoot().providers,
        { provide: OAuthStorage, useFactory: storageFactory },
      ],
    };
  }
}
