import { LazyLoadService } from '@abp/ng.core';
export declare class InitialService {
    private lazyLoadService;
    constructor(lazyLoadService: LazyLoadService);
    appendStyle(): import("rxjs").Observable<void>;
}
