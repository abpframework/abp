import { Router } from '@angular/router';
import { EntityAction } from '@abp/ng.components/extensible';
import { BlogPostListDto } from '@abp/ng.cms-kit/proxy';
import { BlogPostListComponent } from '../../components/blog-posts/blog-post-list/blog-post-list.component';

export const DEFAULT_BLOG_POST_ENTITY_ACTIONS = EntityAction.createMany<BlogPostListDto>([
  {
    text: 'AbpUi::Edit',
    action: data => {
      const router = data.getInjected(Router);
      router.navigate(['/cms/blog-posts/update', data.record.id]);
    },
    permission: 'CmsKit.BlogPosts.Update',
  },
  {
    text: 'AbpUi::Delete',
    action: data => {
      const component = data.getInjected(BlogPostListComponent);
      component.delete(data.record.id!, data.record.title!);
    },
    permission: 'CmsKit.BlogPosts.Delete',
  },
]);
