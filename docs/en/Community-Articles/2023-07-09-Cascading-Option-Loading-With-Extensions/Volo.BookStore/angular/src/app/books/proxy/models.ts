import { ExtensibleFullAuditedEntityDto, ExtensibleObject } from '@abp/ng.core';

export interface BookDto extends ExtensibleFullAuditedEntityDto<string> {
  authorId: string;
  name: string;
}

export interface CreateUpdateRentBookDto extends ExtensibleObject {
  authorId: string;
  name: string;
}
