import { PipeTransform, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';
import { Subject } from 'rxjs';
export declare class LocalizationPipe implements PipeTransform, OnDestroy {
    private store;
    initialValue: string;
    value: string;
    destroy$: Subject<unknown>;
    constructor(store: Store);
    transform(value?: string, ...interpolateParams: string[]): string;
    ngOnDestroy(): void;
}
