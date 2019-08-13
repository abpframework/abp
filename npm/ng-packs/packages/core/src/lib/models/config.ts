import { AuthConfig } from 'angular-oauth2-oidc';
import { Type } from '@angular/core';

export namespace Config {
  export interface State {
    [key: string]: any;
  }

  export interface Environment {
    application: Application;
    production: boolean;
    oAuthConfig: AuthConfig;
    apis: Apis;
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
}
