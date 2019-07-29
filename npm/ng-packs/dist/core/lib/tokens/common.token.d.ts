import { InjectionToken } from '@angular/core';
import { Config } from '../models';
export declare function environmentFactory(environment: Config.Environment): {
    production: boolean;
    oAuthConfig: import("angular-oauth2-oidc").AuthConfig;
    apis: Config.Apis;
};
export declare function configFactory(config: Config.Requirements): {
    layouts: import("@angular/core").Type<any>[];
};
export declare const ENVIRONMENT: InjectionToken<{}>;
export declare const CONFIG: InjectionToken<{}>;
