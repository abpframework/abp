import { ABP } from './common';
import { checkHasProp } from '../utils/common-utils';

export class ListResultDto<T> {
  items?: T[];

  constructor(initialValues: Partial<ListResultDto<T>> = {}) {
    for (const key in initialValues) {
      if (checkHasProp(initialValues, key)) {
        this[key] = initialValues[key];
      }
    }
  }
}
type ValueOf<T> = T[keyof T];
export class PagedResultDto<T> extends ListResultDto<T> {
  totalCount?: number;

  constructor(initialValues: Partial<PagedResultDto<T>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleObject {
  extraProperties?: ABP.Dictionary<any>;

  constructor(initialValues: Partial<ExtensibleObject> = {}) {
    for (const key in initialValues) {
      if (checkHasProp(initialValues, key) && initialValues[key] !== undefined) {
        this[key] = initialValues[key];
      }
    }
  }
}

export class ExtensibleEntityDto<TKey = string> extends ExtensibleObject {
  id?: TKey;

  constructor(initialValues: Partial<ExtensibleEntityDto<TKey>> = {}) {
    super(initialValues);
  }
}

export class LimitedResultRequestDto {
  maxResultCount = 10;

  constructor(initialValues: Partial<LimitedResultRequestDto> = {}) {
    for (const key in initialValues) {
      if (checkHasProp(initialValues, key) && initialValues[key] !== undefined) {
        this[key] = initialValues[key] as ValueOf<LimitedResultRequestDto>;
      }
    }
  }
}

export class ExtensibleLimitedResultRequestDto extends ExtensibleEntityDto {
  maxResultCount = 10;

  constructor(initialValues: Partial<ExtensibleLimitedResultRequestDto> = {}) {
    super(initialValues);
  }
}

export class PagedResultRequestDto extends LimitedResultRequestDto {
  skipCount?: number;

  constructor(initialValues: Partial<PagedResultRequestDto> = {}) {
    super(initialValues);
  }
}

export class ExtensiblePagedResultRequestDto extends ExtensibleLimitedResultRequestDto {
  skipCount?: number;

  constructor(initialValues: Partial<ExtensiblePagedResultRequestDto> = {}) {
    super(initialValues);
  }
}

export class PagedAndSortedResultRequestDto extends PagedResultRequestDto {
  sorting?: string;

  constructor(initialValues: Partial<PagedAndSortedResultRequestDto> = {}) {
    super(initialValues);
  }
}

export class ExtensiblePagedAndSortedResultRequestDto extends ExtensiblePagedResultRequestDto {
  sorting?: string;

  constructor(initialValues: Partial<ExtensiblePagedAndSortedResultRequestDto> = {}) {
    super(initialValues);
  }
}

export class EntityDto<TKey = string> {
  id?: TKey;

  constructor(initialValues: Partial<EntityDto<TKey>> = {}) {
    for (const key in initialValues) {
      if (checkHasProp(initialValues, key)) {
        this[key] = initialValues[key];
      }
    }
  }
}

export class CreationAuditedEntityDto<TPrimaryKey = string> extends EntityDto<TPrimaryKey> {
  creationTime?: string | Date;
  creatorId?: string;

  constructor(initialValues: Partial<CreationAuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class CreationAuditedEntityWithUserDto<  
  TPrimaryKey = string,
  TUserDto = any
> extends CreationAuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;

  constructor(
    initialValues: Partial<CreationAuditedEntityWithUserDto<TPrimaryKey,TUserDto>> = {},
  ) {
    super(initialValues);
  }
}

export class AuditedEntityDto<TPrimaryKey = string> extends CreationAuditedEntityDto<TPrimaryKey> {
  lastModificationTime?: string | Date;
  lastModifierId?: string;

  constructor(initialValues: Partial<AuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

/** @deprecated the class signature will change in v8.0 */ 
export class AuditedEntityWithUserDto<
  TPrimaryKey = string,
  TUserDto = any,
> extends AuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;
  lastModifier?: TUserDto;

  constructor(initialValues: Partial<AuditedEntityWithUserDto< TPrimaryKey,TUserDto>> = {}) {
    super(initialValues);
  }
}

export class FullAuditedEntityDto<TPrimaryKey = string> extends AuditedEntityDto<TPrimaryKey> {
  isDeleted?: boolean;
  deleterId?: string;
  deletionTime?: Date | string;

  constructor(initialValues: Partial<FullAuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}
/** @deprecated the class signature will change in v8.0 */ 
export class FullAuditedEntityWithUserDto<
  TPrimaryKey = string,
  TUserDto = any
> extends FullAuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;
  lastModifier?: TUserDto;
  deleter?: TUserDto;

  constructor(initialValues: Partial<FullAuditedEntityWithUserDto< TPrimaryKey,TUserDto>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleCreationAuditedEntityDto<
  TPrimaryKey = string,
> extends ExtensibleEntityDto<TPrimaryKey> {
  creationTime?: Date | string;
  creatorId?: string;

  constructor(initialValues: Partial<ExtensibleCreationAuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleAuditedEntityDto<
  TPrimaryKey = string,
> extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  lastModificationTime?: Date | string;
  lastModifierId?: string;

  constructor(initialValues: Partial<ExtensibleAuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleAuditedEntityWithUserDto<
  TPrimaryKey = string,
  TUserDto = any,
> extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;
  lastModifier?: TUserDto;

  constructor(initialValues: Partial<ExtensibleAuditedEntityWithUserDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleCreationAuditedEntityWithUserDto<
  TPrimaryKey = string,
  TUserDto = any,
> extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;

  constructor(
    initialValues: Partial<ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey>> = {},
  ) {
    super(initialValues);
  }
}

export class ExtensibleFullAuditedEntityDto<
  TPrimaryKey = string,
> extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  isDeleted?: boolean;
  deleterId?: string;
  deletionTime?: Date | string;

  constructor(initialValues: Partial<ExtensibleFullAuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleFullAuditedEntityWithUserDto<
  TPrimaryKey = string,
  TUserDto = any,
> extends ExtensibleFullAuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;
  lastModifier?: TUserDto;
  deleter?: TUserDto;

  constructor(initialValues: Partial<ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}
