import { PipeTransform } from '@angular/core';
import { Store } from '@ngxs/store';
import { Config } from '../models';
export declare class LocalizationPipe implements PipeTransform {
    private store;
    constructor(store: Store);
    transform(value?: string | Config.LocalizationWithDefault, ...interpolateParams: string[]): string;
}
