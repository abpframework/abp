import { RestService } from '@abp/ng.core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { FeatureManagement } from '../models';
export declare class FeatureManagementService {
    private rest;
    private store;
    constructor(rest: RestService, store: Store);
    getFeatures(params: FeatureManagement.Provider): Observable<FeatureManagement.Features>;
    updateFeatures({ features, providerKey, providerName, }: FeatureManagement.Provider & FeatureManagement.Features): Observable<null>;
}
