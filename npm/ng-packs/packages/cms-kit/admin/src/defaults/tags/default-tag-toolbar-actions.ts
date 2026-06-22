import { TagDto } from '@abp/ng.cms-kit/proxy';
import { ToolbarAction } from '@abp/ng.components/extensible';
import { TagListComponent } from '../../components/tags/tag-list/tag-list.component';

export const DEFAULT_TAG_TOOLBAR_ACTIONS = ToolbarAction.createMany<TagDto[]>([
  {
    text: 'CmsKit::NewTag',
    action: data => {
      const component = data.getInjected(TagListComponent);
      component.add();
    },
    permission: 'CmsKit.Tags.Create',
    icon: 'fa fa-plus',
  },
]);
