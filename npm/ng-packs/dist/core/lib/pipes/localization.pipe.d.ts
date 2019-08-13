import { PipeTransform, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';
export declare class LocalizationPipe implements PipeTransform, OnDestroy {
    private store;
    initialized: boolean;
    value: string;
    constructor(store: Store);
    transform(value: string, ...interpolateParams: string[]): string;
    ngOnDestroy(): void;
}
