import { Validators } from '@angular/forms';
import { map } from 'rxjs/operators';
import { CreateBlogPostDto, BlogAdminService } from '@abp/ng.cms-kit/proxy';
import { FormProp, ePropType } from '@abp/ng.components/extensible';

export const DEFAULT_BLOG_POST_CREATE_FORM_PROPS = FormProp.createMany<CreateBlogPostDto>([
  {
    type: ePropType.Enum,
    name: 'blogId',
    displayName: 'CmsKit::Blog',
    id: 'blogId',
    options: data => {
      const blogService = data.getInjected(BlogAdminService);
      return blogService.getList({ maxResultCount: 1000 }).pipe(
        map(result =>
          result.items.map(blog => ({
            key: blog.name || '',
            value: blog.id || '',
          })),
        ),
      );
    },
    validators: () => [Validators.required],
  },
  {
    type: ePropType.String,
    name: 'title',
    displayName: 'CmsKit::Title',
    id: 'title',
    validators: () => [Validators.required],
  },
  {
    type: ePropType.String,
    name: 'slug',
    displayName: 'CmsKit::Slug',
    id: 'slug',
    validators: () => [Validators.required],
    tooltip: {
      text: 'CmsKit::BlogPostSlugInformation',
    },
  },
  {
    type: ePropType.String,
    name: 'shortDescription',
    displayName: 'CmsKit::ShortDescription',
    id: 'shortDescription',
  },
]);

export const DEFAULT_BLOG_POST_EDIT_FORM_PROPS: FormProp<any>[] =
  DEFAULT_BLOG_POST_CREATE_FORM_PROPS;
