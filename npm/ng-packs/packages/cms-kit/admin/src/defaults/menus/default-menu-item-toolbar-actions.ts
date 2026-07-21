import { MenuItemDto } from '@abp/ng.cms-kit/proxy';
import { ToolbarAction } from '@abp/ng.components/extensible';
import { MenuItemListComponent } from '../../components/menus/menu-item-list/menu-item-list.component';

export const DEFAULT_MENU_ITEM_TOOLBAR_ACTIONS = ToolbarAction.createMany<MenuItemDto[]>([
  {
    text: 'CmsKit::NewMenuItem',
    action: data => {
      const component = data.getInjected(MenuItemListComponent);
      component.add();
    },
    permission: 'CmsKit.Menus.Create',
    icon: 'fa fa-plus',
  },
]);
