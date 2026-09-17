import { of } from 'rxjs';
import { BlogPostListDto, BlogPostStatus } from '@abp/ng.cms-kit/proxy';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { LocalizationService } from '@abp/ng.core';

export const DEFAULT_BLOG_POST_ENTITY_PROPS = EntityProp.createMany<BlogPostListDto>([
  {
    type: ePropType.String,
    name: 'blogName',
    displayName: 'CmsKit::Blog',
    sortable: true,
    columnWidth: 150,
  },
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
        case BlogPostStatus.Draft:
          result = localization.instant('CmsKit::CmsKit.BlogPost.Status.0');
          break;
        case BlogPostStatus.Published:
          result = localization.instant('CmsKit::CmsKit.BlogPost.Status.1');
          break;
        case BlogPostStatus.WaitingForReview:
          result = localization.instant('CmsKit::CmsKit.BlogPost.Status.2');
          break;
      }
      return of(result);
    },
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
