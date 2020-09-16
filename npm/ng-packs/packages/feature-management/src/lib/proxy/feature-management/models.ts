import type { IStringValueType } from '../validation/string-values/models';

export interface FeatureDto {
  name: string;
  displayName: string;
  value: string;
  provider: FeatureProviderDto;
  description: string;
  valueType: IStringValueType;
  depth: number;
  parentName: string;
}

export interface FeatureGroupDto {
  name: string;
  displayName: string;
  features: FeatureDto[];
}

export interface FeatureProviderDto {
  name: string;
  key: string;
}

export interface GetFeatureListResultDto {
  groups: FeatureGroupDto[];
}

export interface UpdateFeatureDto {
  name: string;
  value: string;
}

export interface UpdateFeaturesDto {
  features: UpdateFeatureDto[];
}
