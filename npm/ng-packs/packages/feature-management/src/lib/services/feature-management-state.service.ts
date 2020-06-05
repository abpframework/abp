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

  dispatchGetFeatures(...args: ConstructorParameters<typeof GetFeatures>) {
    return this.store.dispatch(new GetFeatures(...args));
  }

  dispatchUpdateFeatures(...args: ConstructorParameters<typeof UpdateFeatures>) {
    return this.store.dispatch(new UpdateFeatures(...args));
  }
}
