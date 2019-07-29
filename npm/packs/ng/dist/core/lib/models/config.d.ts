import { AuthConfig } from 'angular-oauth2-oidc';
import { Type } from '@angular/core';
export declare namespace Config {
    interface State {
        [key: string]: any;
    }
    interface Environment {
        production: boolean;
        oAuthConfig: AuthConfig;
        apis: Apis;
    }
    interface Apis {
        [key: string]: {
            [key: string]: string;
        };
    }
    interface Requirements {
        layouts: Type<any>[];
    }
}
