import { Config } from '@abp/ng.core';
import { Store } from '@ngxs/store';
export declare class LayoutComponent {
    private store;
    isCollapsed: boolean;
    readonly appInfo: Config.Application;
    constructor(store: Store);
}
