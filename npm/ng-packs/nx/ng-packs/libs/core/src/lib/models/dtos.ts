import { ABP } from './common';

export class ListResultDto<T> {
  items?: T[];

  constructor(initialValues: Partial<ListResultDto<T>> = {}) {
    for (const key in initialValues) {
      if (initialValues.hasOwnProperty(key)) {
        this[key] = initialValues[key];
      }
    }
  }
}

export class PagedResultDto<T> extends ListResultDto<T> {
  totalCount?: number;

  constructor(initialValues: Partial<PagedResultDto<T>> = {}) {
    super(initialValues);
  }
}

export class LimitedResultRequestDto {
  maxResultCount = 10;

  constructor(initialValues: Partial<LimitedResultRequestDto> = {}) {
    for (const key in initialValues) {
      if (initialValues.hasOwnProperty(key) && initialValues[key] !== undefined) {
        this[key] = initialValues[key];
      }
    }
  }
}

export class PagedResultRequestDto extends LimitedResultRequestDto {
  skipCount?: number;

  constructor(initialValues: Partial<PagedResultRequestDto> = {}) {
    super(initialValues);
  }
}

export class PagedAndSortedResultRequestDto extends PagedResultRequestDto {
  sorting?: string;

  constructor(initialValues: Partial<PagedAndSortedResultRequestDto> = {}) {
    super(initialValues);
  }
}

export class EntityDto<TKey = string> {
  id?: TKey;

  constructor(initialValues: Partial<EntityDto<TKey>> = {}) {
    for (const key in initialValues) {
      if (initialValues.hasOwnProperty(key)) {
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
  TUserDto,
  TPrimaryKey = string
> extends CreationAuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;

  constructor(
    initialValues: Partial<CreationAuditedEntityWithUserDto<TUserDto, TPrimaryKey>> = {},
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

export class AuditedEntityWithUserDto<
  TUserDto,
  TPrimaryKey = string
> extends AuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;
  lastModifier?: TUserDto;

  constructor(initialValues: Partial<AuditedEntityWithUserDto<TUserDto, TPrimaryKey>> = {}) {
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

export class FullAuditedEntityWithUserDto<
  TUserDto,
  TPrimaryKey = string
> extends FullAuditedEntityDto<TPrimaryKey> {
  creator?: TUserDto;
  lastModifier?: TUserDto;
  deleter?: TUserDto;

  constructor(initialValues: Partial<FullAuditedEntityWithUserDto<TUserDto, TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleObject {
  extraProperties: ABP.Dictionary<any>;

  constructor(initialValues: Partial<ExtensibleObject> = {}) {
    for (const key in initialValues) {
      if (initialValues.hasOwnProperty(key)) {
        this[key] = initialValues[key];
      }
    }
  }
}

export class ExtensibleEntityDto<TKey = string> extends ExtensibleObject {
  id: TKey;

  constructor(initialValues: Partial<ExtensibleEntityDto<TKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleCreationAuditedEntityDto<
  TPrimaryKey = string
> extends ExtensibleEntityDto<TPrimaryKey> {
  creationTime: Date | string;
  creatorId?: string;

  constructor(initialValues: Partial<ExtensibleCreationAuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleAuditedEntityDto<
  TPrimaryKey = string
> extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  lastModificationTime?: Date | string;
  lastModifierId?: string;

  constructor(initialValues: Partial<ExtensibleAuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleAuditedEntityWithUserDto<
  TPrimaryKey = string,
  TUserDto = any
> extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;

  constructor(initialValues: Partial<ExtensibleAuditedEntityWithUserDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleCreationAuditedEntityWithUserDto<
  TPrimaryKey = string,
  TUserDto = any
> extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;

  constructor(
    initialValues: Partial<ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey>> = {},
  ) {
    super(initialValues);
  }
}

export class ExtensibleFullAuditedEntityDto<
  TPrimaryKey = string
> extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  isDeleted: boolean;
  deleterId?: string;
  deletionTime: Date | string;

  constructor(initialValues: Partial<ExtensibleFullAuditedEntityDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}

export class ExtensibleFullAuditedEntityWithUserDto<
  TPrimaryKey = string,
  TUserDto = any
> extends ExtensibleFullAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
  deleter: TUserDto;

  constructor(initialValues: Partial<ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey>> = {}) {
    super(initialValues);
  }
}
