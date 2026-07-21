import { BlogDto } from '@abp/ng.cms-kit/proxy';
import { ToolbarAction } from '@abp/ng.components/extensible';
import { BlogListComponent } from '../../components/blogs/blog-list/blog-list.component';

export const DEFAULT_BLOG_TOOLBAR_ACTIONS = ToolbarAction.createMany<BlogDto[]>([
  {
    text: 'CmsKit::NewBlog',
    action: data => {
      const component = data.getInjected(BlogListComponent);
      component.add();
    },
    permission: 'CmsKit.Blogs.Create',
    icon: 'fa fa-plus',
  },
]);
