import type { ExtensibleEntityDto, ExtensibleObject, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetTenantsInput extends PagedAndSortedResultRequestDto {
  filter: string;
}

export interface TenantCreateDto extends TenantCreateOrUpdateDtoBase {
  adminEmailAddress: string;
  adminPassword: string;
}

export interface TenantCreateOrUpdateDtoBase extends ExtensibleObject {
  name: string;
}

export interface TenantDto extends ExtensibleEntityDto<string> {
  name: string;
}

// tslint:disable-next-line: no-empty-interface
export interface TenantUpdateDto extends TenantCreateOrUpdateDtoBase {
}
