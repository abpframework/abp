import { InjectionToken } from '@angular/core';
import { Config } from '../models';
export declare function environmentFactory(environment: Config.Environment): {
    application: Config.Application;
    production: boolean;
    oAuthConfig: import("angular-oauth2-oidc").AuthConfig;
    apis: Config.Apis;
    localization: {
        defaultResourceName: string;
    };
};
export declare function configFactory(config: Config.Requirements): {
    layouts: import("@angular/core").Type<any>[];
};
export declare const ENVIRONMENT: InjectionToken<unknown>;
export declare const CONFIG: InjectionToken<unknown>;
