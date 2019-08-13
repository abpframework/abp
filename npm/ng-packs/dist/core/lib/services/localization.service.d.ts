import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
export declare class LocalizationService {
    private store;
    constructor(store: Store);
    get(keys: string, ...interpolateParams: string[]): Observable<string>;
    instant(keys: string, ...interpolateParams: string[]): string;
}
