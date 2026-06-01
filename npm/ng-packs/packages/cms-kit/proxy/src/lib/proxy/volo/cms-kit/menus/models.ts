import type { ExtensibleAuditedEntityDto } from '@abp/ng.core';

export interface MenuItemDto extends ExtensibleAuditedEntityDto<string> {
  parentId?: string;
  displayName?: string;
  isActive: boolean;
  url?: string;
  icon?: string;
  order: number;
  target?: string;
  elementId?: string;
  cssClass?: string;
  pageId?: string;
  requiredPermissionName?: string;
  concurrencyStamp?: string;
}
