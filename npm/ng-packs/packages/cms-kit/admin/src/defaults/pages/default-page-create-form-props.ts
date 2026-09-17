import { Validators } from '@angular/forms';
import { of } from 'rxjs';
import { CreatePageInputDto, UpdatePageInputDto } from '@abp/ng.cms-kit/proxy';
import { FormProp, ePropType } from '@abp/ng.components/extensible';
import { pageStatusOptions } from '@abp/ng.cms-kit/proxy';
import { LAYOUT_CONSTANTS } from './layout-constants';

export const DEFAULT_PAGE_CREATE_FORM_PROPS = FormProp.createMany<CreatePageInputDto>([
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
      text: 'CmsKit::PageSlugInformation',
    },
  },
  {
    type: ePropType.Enum,
    name: 'layoutName',
    displayName: 'CmsKit::SelectLayout',
    id: 'layoutName',
    options: () =>
      of(
        Object.values(LAYOUT_CONSTANTS).map(layout => ({
          key: layout,
          value: layout.toUpperCase(),
        })),
      ),
    validators: () => [Validators.required],
  },
  {
    type: ePropType.Enum,
    name: 'status',
    displayName: 'CmsKit::Status',
    id: 'status',
    options: () => of(pageStatusOptions),
    validators: () => [Validators.required],
  },
]);

export const DEFAULT_PAGE_EDIT_FORM_PROPS: FormProp<UpdatePageInputDto>[] =
  DEFAULT_PAGE_CREATE_FORM_PROPS;
