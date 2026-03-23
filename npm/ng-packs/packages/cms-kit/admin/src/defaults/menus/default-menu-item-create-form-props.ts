import { Validators } from '@angular/forms';
import { map } from 'rxjs/operators';
import {
  MenuItemCreateInput,
  MenuItemAdminService,
  PermissionLookupDto,
} from '@abp/ng.cms-kit/proxy';
import { FormProp, ePropType } from '@abp/ng.components/extensible';

export const DEFAULT_MENU_ITEM_CREATE_FORM_PROPS = FormProp.createMany<MenuItemCreateInput>([
  {
    type: ePropType.String,
    name: 'displayName',
    displayName: 'CmsKit::DisplayName',
    id: 'displayName',
    validators: () => [Validators.required],
  },
  {
    type: ePropType.Boolean,
    name: 'isActive',
    displayName: 'CmsKit::IsActive',
    id: 'isActive',
    defaultValue: true,
  },
  {
    type: ePropType.String,
    name: 'icon',
    displayName: 'CmsKit::Icon',
    id: 'icon',
  },
  {
    type: ePropType.String,
    name: 'target',
    displayName: 'CmsKit::Target',
    id: 'target',
  },
  {
    type: ePropType.String,
    name: 'elementId',
    displayName: 'CmsKit::ElementId',
    id: 'elementId',
  },
  {
    type: ePropType.String,
    name: 'cssClass',
    displayName: 'CmsKit::CssClass',
    id: 'cssClass',
  },
  {
    type: ePropType.Enum,
    name: 'requiredPermissionName',
    displayName: 'CmsKit::RequiredPermissionName',
    id: 'requiredPermissionName',
    options: data => {
      const menuItemService = data.getInjected(MenuItemAdminService);
      return menuItemService
        .getPermissionLookup({
          filter: '',
        })
        .pipe(
          map((result: { items: PermissionLookupDto[] }) =>
            result.items.map(permission => ({
              key: permission.displayName || permission.name || '',
              value: permission.name || '',
            })),
          ),
        );
    },
  },
]);

export const DEFAULT_MENU_ITEM_EDIT_FORM_PROPS = DEFAULT_MENU_ITEM_CREATE_FORM_PROPS;
