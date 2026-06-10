import type { ExtensibleAuditedEntityDto, ExtensibleObject, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { PageStatus } from '../../pages/page-status.enum';

export interface CreatePageInputDto extends ExtensibleObject {
  title: string;
  slug: string;
  layoutName?: string;
  content?: string;
  script?: string;
  style?: string;
  status?: PageStatus;
}

export interface GetPagesInputDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  status?: PageStatus;
}

export interface PageDto extends ExtensibleAuditedEntityDto<string> {
  title?: string;
  slug?: string;
  layoutName?: string;
  content?: string;
  script?: string;
  style?: string;
  isHomePage: boolean;
  status?: PageStatus;
  concurrencyStamp?: string;
}

export interface UpdatePageInputDto extends ExtensibleObject {
  title: string;
  slug: string;
  layoutName?: string;
  content?: string;
  script?: string;
  style?: string;
  status?: PageStatus;
  concurrencyStamp?: string;
}
