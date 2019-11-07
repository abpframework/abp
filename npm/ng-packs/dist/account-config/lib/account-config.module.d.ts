import { InjectionToken, ModuleWithProviders } from '@angular/core';
export interface AccountConfigOptions {
    redirectUrl?: string;
}
export declare function accountOptionsFactory(options: AccountConfigOptions): {
    redirectUrl: string;
};
export declare const ACCOUNT_OPTIONS: InjectionToken<unknown>;
export declare class AccountConfigModule {
    static forRoot(options?: AccountConfigOptions): ModuleWithProviders;
}
