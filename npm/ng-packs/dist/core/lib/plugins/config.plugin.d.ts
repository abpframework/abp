import { InjectionToken } from '@angular/core';
import { NgxsPlugin, NgxsNextPluginFn } from '@ngxs/store';
import { Router } from '@angular/router';
import { ABP } from '../models';
export declare const NGXS_CONFIG_PLUGIN_OPTIONS: InjectionToken<{}>;
export declare class ConfigPlugin implements NgxsPlugin {
    private options;
    private router;
    private initialized;
    constructor(options: ABP.Root, router: Router);
    handle(state: any, event: any, next: NgxsNextPluginFn): any;
}
