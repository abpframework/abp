import { Type } from '@angular/core';
export interface RootParams {
    httpErrorConfig: HttpErrorConfig;
}
export declare type ErrorScreenErrorCodes = 401 | 403 | 404 | 500;
export interface HttpErrorConfig {
    errorScreen?: {
        component: Type<any>;
        forWhichErrors?: [ErrorScreenErrorCodes] | [ErrorScreenErrorCodes, ErrorScreenErrorCodes] | [ErrorScreenErrorCodes, ErrorScreenErrorCodes, ErrorScreenErrorCodes] | [ErrorScreenErrorCodes, ErrorScreenErrorCodes, ErrorScreenErrorCodes, ErrorScreenErrorCodes];
    };
}
