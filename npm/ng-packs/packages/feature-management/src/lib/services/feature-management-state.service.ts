import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { FeatureManagementState } from '../states';
import { FeatureManagement } from '../models';
import { GetFeatures, UpdateFeatures } from '../actions';

@Injectable({
  providedIn: 'root',
})
export class FeatureManagementStateService {
  constructor(private store: Store) {}

  getFeatures() {
    return this.store.selectSnapshot(FeatureManagementState.getFeatures);
  }

  fetchFeatures(payload: FeatureManagement.Provider) {
    return this.store.dispatch(new GetFeatures(payload));
  }

  updateFeatures(payload: FeatureManagement.Provider & FeatureManagement.Features) {
    return this.store.dispatch(new UpdateFeatures(payload));
  }
}
