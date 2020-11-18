import { AuthConfig } from 'angular-oauth2-oidc';
import { ABP } from './common';

export interface Environment {
  apis: Apis;
  application: ApplicationInfo;
  hmr?: boolean;
  test?: boolean;
  localization?: { defaultResourceName?: string };
  oAuthConfig: AuthConfig;
  production: boolean;
  remoteEnv?: RemoteEnv;
}

export interface ApplicationInfo {
  name: string;
  baseUrl?: string;
  logoUrl?: string;
}

export type ApiConfig = {
  [key: string]: string;
  url: string;
} & Partial<{
  rootNamespace: string;
}>;

export interface Apis {
  [key: string]: ApiConfig;
  default: ApiConfig;
}

export type customMergeFn = (localEnv: Partial<Environment>, remoteEnv: any) => Environment;

export interface RemoteEnv {
  url: string;
  mergeStrategy: 'deepmerge' | 'overwrite' | customMergeFn;
  method?: string;
  headers?: ABP.Dictionary<string>;
}
