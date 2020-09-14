import { FeatureManagement } from '../models';

/**
 * @deprecated To be deleted in v4.0.
 */
export class GetFeatures {
  static readonly type = '[FeatureManagement] Get Features';
  constructor(public payload: FeatureManagement.Provider) {}
}

/**
 * @deprecated To be deleted in v4.0.
 */
export class UpdateFeatures {
  static readonly type = '[FeatureManagement] Update Features';
  constructor(public payload: FeatureManagement.Provider & FeatureManagement.Features) {}
}
