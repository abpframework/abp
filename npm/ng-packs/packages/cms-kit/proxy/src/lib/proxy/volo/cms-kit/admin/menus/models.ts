import type { EntityDto, ExtensibleObject, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { MenuItemDto } from '../../menus/models';
import type { PageStatus } from '../../pages/page-status.enum';

export interface MenuItemCreateInput extends ExtensibleObject {
  parentId?: string;
  displayName: string;
  isActive: boolean;
  url?: string;
  icon?: string;
  order: number;
  target?: string;
  elementId?: string;
  cssClass?: string;
  pageId?: string;
  requiredPermissionName?: string;
}

export interface MenuItemMoveInput {
  newParentId?: string;
  position: number;
}

export interface MenuItemUpdateInput extends ExtensibleObject {
  displayName: string;
  isActive: boolean;
  url?: string;
  icon?: string;
  target?: string;
  elementId?: string;
  cssClass?: string;
  pageId?: string;
  requiredPermissionName?: string;
  concurrencyStamp?: string;
}

export interface MenuItemWithDetailsDto extends MenuItemDto {
  pageTitle?: string;
}

export interface PageLookupDto extends EntityDto<string> {
  title?: string;
  slug?: string;
}

export interface PageLookupInputDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  status?: PageStatus;
}

export interface PermissionLookupDto {
  name?: string;
  displayName?: string;
}

export interface PermissionLookupInputDto {
  filter?: string;
}
