import type { ExtensibleObject } from '@abp/ng.core';

export interface GlobalResourcesDto extends ExtensibleObject {
  styleContent?: string;
  scriptContent?: string;
}

export interface GlobalResourcesUpdateDto extends ExtensibleObject {
  style?: string;
  script?: string;
}
