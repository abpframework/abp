import { Injectable } from '@angular/core';
import { RestService, Rest } from '@abp/ng.core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { FeatureManagement } from '../models';

@Injectable({
  providedIn: 'root',
})
export class FeatureManagementService {
  constructor(private rest: RestService, private store: Store) {}

  getFeatures(params: FeatureManagement.Provider): Observable<FeatureManagement.Features> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/abp/features',
      params,
    };
    return this.rest.request<FeatureManagement.Provider, FeatureManagement.Features>(request);
  }

  updateFeatures({
    features,
    providerKey,
    providerName,
  }: FeatureManagement.Provider & FeatureManagement.Features): Observable<null> {
    const request: Rest.Request<FeatureManagement.Features> = {
      method: 'PUT',
      url: '/api/abp/features',
      body: { features },
      params: { providerKey, providerName },
    };
    return this.rest.request<FeatureManagement.Features, null>(request);
  }
}
