import { TagDto } from '@abp/ng.cms-kit/proxy';
import { EntityAction } from '@abp/ng.components/extensible';
import { TagListComponent } from '../../components/tags/tag-list/tag-list.component';

export const DEFAULT_TAG_ENTITY_ACTIONS = EntityAction.createMany<TagDto>([
  {
    text: 'AbpUi::Edit',
    action: data => {
      const component = data.getInjected(TagListComponent);
      component.edit(data.record.id!);
    },
    permission: 'CmsKit.Tags.Update',
  },
  {
    text: 'AbpUi::Delete',
    action: data => {
      const component = data.getInjected(TagListComponent);
      const { id, name } = data.record;
      component.delete(id!, name!);
    },
    permission: 'CmsKit.Tags.Delete',
  },
]);
