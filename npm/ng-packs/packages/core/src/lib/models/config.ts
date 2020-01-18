import { AuthConfig } from 'angular-oauth2-oidc';
import { Type } from '@angular/core';
import { ApplicationConfiguration } from './application-configuration';
import { ABP } from './common';

export namespace Config {
  export type State = ApplicationConfiguration.Response &
    ABP.Root & { environment: Environment } & {
      routes: ABP.FullRoute[];
      flattedRoutes: ABP.FullRoute[];
    };

  export interface Environment {
    application: Application;
    production: boolean;
    oAuthConfig: AuthConfig;
    apis: Apis;
    localization: { defaultResourceName: string };
  }

  export interface Application {
    name: string;
    logoUrl?: string;
  }

  export interface Apis {
    [key: string]: { [key: string]: string };
  }

  export interface Requirements {
    layouts: Type<any>[];
  }

  export interface LocalizationWithDefault {
    key: string;
    defaultValue: string;
  }

  export type LocalizationParam = string | LocalizationWithDefault;
}
