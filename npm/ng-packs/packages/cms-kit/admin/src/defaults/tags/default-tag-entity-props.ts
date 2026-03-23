import { TagDto } from '@abp/ng.cms-kit/proxy';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';

export const DEFAULT_TAG_ENTITY_PROPS = EntityProp.createMany<TagDto>([
  {
    type: ePropType.String,
    name: 'entityType',
    displayName: 'CmsKit::EntityType',
    sortable: true,
    columnWidth: 200,
  },
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'CmsKit::Name',
    sortable: true,
    columnWidth: 250,
  },
]);
