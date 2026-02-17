import { BlogDto } from '@abp/ng.cms-kit/proxy';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';

export const DEFAULT_BLOG_ENTITY_PROPS = EntityProp.createMany<BlogDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'CmsKit::Name',
    sortable: true,
    columnWidth: 250,
  },
  {
    type: ePropType.String,
    name: 'slug',
    displayName: 'CmsKit::Slug',
    sortable: true,
    columnWidth: 250,
  },
]);
