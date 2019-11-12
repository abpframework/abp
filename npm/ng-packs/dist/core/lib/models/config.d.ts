import { AuthConfig } from 'angular-oauth2-oidc';
import { Type } from '@angular/core';
import { ApplicationConfiguration } from './application-configuration';
import { ABP } from './common';
export declare namespace Config {
    type State = ApplicationConfiguration.Response & ABP.Root & {
        environment: Environment;
    } & {
        routes: ABP.FullRoute[];
        flattedRoutes: ABP.FullRoute[];
    };
    interface Environment {
        application: Application;
        production: boolean;
        oAuthConfig: AuthConfig;
        apis: Apis;
        localization: {
            defaultResourceName: string;
        };
    }
    interface Application {
        name: string;
        logoUrl?: string;
    }
    interface Apis {
        [key: string]: {
            [key: string]: string;
        };
    }
    interface Requirements {
        layouts: Type<any>[];
    }
    interface LocalizationWithDefault {
        key: string;
        defaultValue: string;
    }
    type LocalizationParam = string | LocalizationWithDefault;
}
