import { Resolve } from '@angular/router';
import { Store } from '@ngxs/store';
import { Identity } from '../models/identity';
export declare class UserResolver implements Resolve<Identity.State> {
    private store;
    constructor(store: Store);
    resolve(): import("rxjs").Observable<any>;
}
