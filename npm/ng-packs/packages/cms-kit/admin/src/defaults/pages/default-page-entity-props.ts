import { of } from 'rxjs';
import { PageDto, PageStatus } from '@abp/ng.cms-kit/proxy';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { LocalizationService } from '@abp/ng.core';

export const DEFAULT_PAGE_ENTITY_PROPS = EntityProp.createMany<PageDto>([
  {
    type: ePropType.String,
    name: 'title',
    displayName: 'CmsKit::Title',
    sortable: true,
    columnWidth: 200,
  },
  {
    type: ePropType.String,
    name: 'slug',
    displayName: 'CmsKit::Slug',
    sortable: true,
    columnWidth: 200,
  },
  {
    type: ePropType.String,
    name: 'status',
    displayName: 'CmsKit::Status',
    sortable: true,
    columnWidth: 120,
    valueResolver: data => {
      const localization = data.getInjected(LocalizationService);
      let result = '';
      switch (data.record.status) {
        case PageStatus.Draft:
          result = localization.instant('CmsKit::Enum:PageStatus:0');
          break;
        case PageStatus.Publish:
          result = localization.instant('CmsKit::Enum:PageStatus:1');
          break;
      }
      return of(result);
    },
  },
  {
    type: ePropType.Boolean,
    name: 'isHomePage',
    displayName: 'CmsKit::IsHomePage',
    sortable: true,
    columnWidth: 120,
  },
  {
    type: ePropType.Date,
    name: 'creationTime',
    displayName: 'AbpIdentity::CreationTime',
    sortable: true,
    columnWidth: 200,
  },
  {
    type: ePropType.Date,
    name: 'lastModificationTime',
    displayName: 'AbpIdentity::LastModificationTime',
    sortable: true,
    columnWidth: 200,
  },
]);
