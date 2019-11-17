import { FeatureManagement } from '../models';
export declare class GetFeatures {
    payload: FeatureManagement.Provider;
    static readonly type = "[FeatureManagement] Get Features";
    constructor(payload: FeatureManagement.Provider);
}
export declare class UpdateFeatures {
    payload: FeatureManagement.Provider & FeatureManagement.Features;
    static readonly type = "[FeatureManagement] Update Features";
    constructor(payload: FeatureManagement.Provider & FeatureManagement.Features);
}
