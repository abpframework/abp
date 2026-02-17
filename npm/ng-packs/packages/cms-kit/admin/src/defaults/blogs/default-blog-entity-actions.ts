import { BlogDto } from '@abp/ng.cms-kit/proxy';
import { EntityAction } from '@abp/ng.components/extensible';
import { BlogListComponent } from '../../components/blogs/blog-list/blog-list.component';

export const DEFAULT_BLOG_ENTITY_ACTIONS = EntityAction.createMany<BlogDto>([
  {
    text: 'CmsKit::Features',
    action: data => {
      const component = data.getInjected(BlogListComponent);
      component.openFeatures(data.record.id!);
    },
    permission: 'CmsKit.Blogs.Features',
  },
  {
    text: 'AbpUi::Edit',
    action: data => {
      const component = data.getInjected(BlogListComponent);
      component.edit(data.record.id!);
    },
    permission: 'CmsKit.Blogs.Update',
  },
  {
    text: 'AbpUi::Delete',
    action: data => {
      const component = data.getInjected(BlogListComponent);
      component.delete(data.record.id!, data.record.name!);
    },
    permission: 'CmsKit.Blogs.Delete',
  },
]);
