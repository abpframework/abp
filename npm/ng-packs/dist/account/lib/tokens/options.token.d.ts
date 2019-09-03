import { InjectionToken } from '@angular/core';
import { Options } from '../models/options';
export declare function optionsFactory(options: Options): {
    redirectUrl: string;
};
export declare const ACCOUNT_OPTIONS: InjectionToken<unknown>;
