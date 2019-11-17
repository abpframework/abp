import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { FeatureManagementState } from '../states';

@Injectable({
  providedIn: 'root',
})
export class FeatureManagementStateService {
  constructor(private store: Store) {}

  getFeatures() {
    return this.store.selectSnapshot(FeatureManagementState.getFeatures);
  }
}
