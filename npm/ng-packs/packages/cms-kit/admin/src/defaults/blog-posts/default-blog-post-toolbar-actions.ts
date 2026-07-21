import { Router } from '@angular/router';
import { BlogPostListDto } from '@abp/ng.cms-kit/proxy';
import { ToolbarAction } from '@abp/ng.components/extensible';

export const DEFAULT_BLOG_POST_TOOLBAR_ACTIONS = ToolbarAction.createMany<BlogPostListDto[]>([
  {
    text: 'CmsKit::NewBlogPost',
    action: data => {
      const router = data.getInjected(Router);
      router.navigate(['/cms/blog-posts/create']);
    },
    permission: 'CmsKit.BlogPosts.Create',
    icon: 'fa fa-plus',
  },
]);
