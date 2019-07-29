import { Resolve } from '@angular/router';
import { Store } from '@ngxs/store';
import { TenantManagement } from '../models/tenant-management';
export declare class TenantsResolver implements Resolve<TenantManagement.State> {
    private store;
    constructor(store: Store);
    resolve(): import("rxjs").Observable<any>;
}
