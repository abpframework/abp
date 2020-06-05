import { FeatureManagement } from '../models';

export class GetFeatures {
  static readonly type = '[FeatureManagement] Get Features';
  constructor(public payload: FeatureManagement.Provider) {}
}

export class UpdateFeatures {
  static readonly type = '[FeatureManagement] Update Features';
  constructor(public payload: FeatureManagement.Provider & FeatureManagement.Features) {}
}
