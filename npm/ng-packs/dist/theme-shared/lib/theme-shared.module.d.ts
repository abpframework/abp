import { Injector, ModuleWithProviders } from '@angular/core';
import { RootParams } from './models/common';
export declare function appendScript(injector: Injector): () => Promise<void[]>;
export declare class ThemeSharedModule {
    static forRoot(options?: RootParams): ModuleWithProviders;
}
