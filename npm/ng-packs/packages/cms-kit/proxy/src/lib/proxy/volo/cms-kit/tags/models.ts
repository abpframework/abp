import type { ExtensibleEntityDto } from '@abp/ng.core';

export interface TagDto extends ExtensibleEntityDto<string> {
  entityType?: string;
  name?: string;
  concurrencyStamp?: string;
}
