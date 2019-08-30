import { Injector } from '@angular/core';
export declare function getInitialData(injector: Injector): () => Promise<any>;
export declare function localeInitializer(injector: Injector): () => Promise<unknown>;
export declare function registerLocale(locale: string): Promise<void>;
