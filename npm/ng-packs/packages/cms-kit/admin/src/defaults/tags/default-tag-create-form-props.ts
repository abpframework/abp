import { TagCreateDto, TagAdminService, TagDefinitionDto } from '@abp/ng.cms-kit/proxy';
import { FormProp, ePropType } from '@abp/ng.components/extensible';
import { Validators } from '@angular/forms';
import { map } from 'rxjs/operators';

export const DEFAULT_TAG_CREATE_FORM_PROPS = FormProp.createMany<TagCreateDto>([
  {
    type: ePropType.Enum,
    name: 'entityType',
    displayName: 'CmsKit::EntityType',
    id: 'entityType',
    validators: () => [Validators.required],
    options: data => {
      const tagService = data.getInjected(TagAdminService);
      return tagService.getTagDefinitions().pipe(
        map((definitions: TagDefinitionDto[]) =>
          definitions.map(def => ({
            key: def.displayName || def.entityType || '',
            value: def.entityType || '',
          })),
        ),
      );
    },
  },
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'CmsKit::Name',
    id: 'name',
    validators: () => [Validators.required],
  },
]);

export const DEFAULT_TAG_EDIT_FORM_PROPS = DEFAULT_TAG_CREATE_FORM_PROPS.filter(
  prop => prop.name !== 'entityType',
);
