import { CreateBlogDto } from '@abp/ng.cms-kit/proxy';
import { FormProp, ePropType } from '@abp/ng.components/extensible';
import { Validators } from '@angular/forms';

export const DEFAULT_BLOG_CREATE_FORM_PROPS = FormProp.createMany<CreateBlogDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'CmsKit::Name',
    id: 'name',
    validators: () => [Validators.required],
  },
  {
    type: ePropType.String,
    name: 'slug',
    displayName: 'CmsKit::Slug',
    id: 'slug',
    validators: () => [Validators.required],
    tooltip: {
      text: 'CmsKit::BlogSlugInformation',
      // params: ['blogs'],
    },
  },
]);

export const DEFAULT_BLOG_EDIT_FORM_PROPS = DEFAULT_BLOG_CREATE_FORM_PROPS;
