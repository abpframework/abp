import { Action, Selector, State, StateContext } from '@ngxs/store';
import { tap } from 'rxjs/operators';
import { GetFeatures, UpdateFeatures } from '../actions/feature-management.actions';
import { FeatureManagement } from '../models/feature-management';
import { FeatureManagementService } from '../services/feature-management.service';

@State<FeatureManagement.State>({
  name: 'FeatureManagementState',
  defaults: { features: {} } as FeatureManagement.State,
})
export class FeatureManagementState {
  @Selector()
  static getFeatures({ features }: FeatureManagement.State) {
    return features || [];
  }

  constructor(private featureManagementService: FeatureManagementService) {}

  @Action(GetFeatures)
  getFeatures({ patchState }: StateContext<FeatureManagement.State>, { payload }: GetFeatures) {
    return this.featureManagementService.getFeatures(payload).pipe(
      tap(({ features }) =>
        patchState({
          features,
        }),
      ),
    );
  }

  @Action(UpdateFeatures)
  updateFeatures(_, { payload }: UpdateFeatures) {
    return this.featureManagementService.updateFeatures(payload);
  }
}
