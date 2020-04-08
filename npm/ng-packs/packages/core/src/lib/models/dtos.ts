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
      if (initialValues.hasOwnProperty(key)) {
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

export class AuditedEntityWithUserDto<TUserDto, TPrimaryKey = string> extends AuditedEntityDto<
  TPrimaryKey
> {
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
