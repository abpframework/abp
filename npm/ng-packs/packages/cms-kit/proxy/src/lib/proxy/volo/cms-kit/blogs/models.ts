import type { ExtensibleEntityDto } from '@abp/ng.core';

export interface BlogFeatureDto extends ExtensibleEntityDto<string> {
  featureName?: string;
  isEnabled: boolean;
}
