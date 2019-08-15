import { AuthConfig } from 'angular-oauth2-oidc';
import { Type } from '@angular/core';
export declare namespace Config {
    interface State {
        [key: string]: any;
    }
    interface Environment {
        application: Application;
        production: boolean;
        oAuthConfig: AuthConfig;
        apis: Apis;
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
}
