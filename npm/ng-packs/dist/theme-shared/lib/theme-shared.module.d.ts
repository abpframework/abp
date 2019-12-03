import { Injector, ModuleWithProviders } from '@angular/core';
import { ErrorHandler } from './handlers/error.handler';
import { RootParams } from './models/common';
export declare function appendScript(injector: Injector): () => Promise<void>;
export declare class ThemeSharedModule {
    private errorHandler;
    constructor(errorHandler: ErrorHandler);
    static forRoot(options?: RootParams): ModuleWithProviders;
}
