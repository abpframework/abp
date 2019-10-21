import { InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { NgxsNextPluginFn, NgxsPlugin } from '@ngxs/store';
import { ABP } from '../../models';
export declare const NGXS_CONFIG_PLUGIN_OPTIONS: InjectionToken<unknown>;
export declare class ConfigPlugin implements NgxsPlugin {
    private options;
    private router;
    private initialized;
    constructor(options: ABP.Root, router: Router);
    handle(state: any, event: any, next: NgxsNextPluginFn): any;
}
