import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { of } from 'rxjs';
import { PageComponent } from '@abp/ng.components/page';
import { ListService, LocalizationPipe, PermissionDirective } from '@abp/ng.core';
import { TreeComponent } from '@abp/ng.components/tree';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import {
  MenuItemAdminService,
  MenuItemDto,
  MenuItemWithDetailsDto,
  MenuItemMoveInput,
} from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';
import {
  MenuItemModalComponent,
  MenuItemModalVisibleChange,
} from '../menu-item-modal/menu-item-modal.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-menu-item-list',
  templateUrl: './menu-item-list.component.html',
  imports: [
    PageComponent,
    TreeComponent,
    LocalizationPipe,
    CommonModule,
    MenuItemModalComponent,
    PermissionDirective,
  ],
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.Menus,
    },
  ],
})
export class MenuItemListComponent implements OnInit {
  private menuItemService = inject(MenuItemAdminService);
  private confirmationService = inject(ConfirmationService);

  readonly nodes = signal<any[]>([]);
  readonly selectedNode = signal<MenuItemDto | null>(null);
  readonly expandedKeys = signal<string[]>([]);
  readonly draggable = signal(true);
  readonly isModalVisible = signal(false);
  readonly selectedMenuItem = signal<MenuItemDto | MenuItemWithDetailsDto | null>(null);
  readonly parentId = signal<string | null>(null);

  ngOnInit() {
    this.loadMenuItems();
  }

  private loadMenuItems() {
    this.menuItemService.getList().subscribe(result => {
      if (result.items && result.items.length > 0) {
        const treeNodes = this.buildTreeNodes(result.items);
        this.nodes.set(treeNodes);
        this.expandedKeys.set(treeNodes.map(n => n.key));
      } else {
        this.nodes.set([]);
        this.expandedKeys.set([]);
      }
    });
  }

  private buildTreeNodes(items: MenuItemDto[]): any[] {
    const nodeMap = new Map<string, any>();
    const rootNodes: any[] = [];

    items.forEach(item => {
      const node: any = {
        key: item.id,
        title: item.displayName || '',
        entity: item,
        children: [],
        isLeaf: false,
      };
      nodeMap.set(item.id!, node);
    });

    items.forEach(item => {
      const node = nodeMap.get(item.id!);
      if (item.parentId) {
        const parent = nodeMap.get(item.parentId);
        if (parent) {
          parent.children.push(node);
          parent.isLeaf = false;
        } else {
          rootNodes.push(node);
        }
      } else {
        rootNodes.push(node);
      }
    });

    const sortByOrder = (nodes: any[]) => {
      nodes.sort((a, b) => (a.entity.order || 0) - (b.entity.order || 0));
      nodes.forEach(node => {
        if (node.children && node.children.length > 0) {
          sortByOrder(node.children);
        }
      });
    };

    sortByOrder(rootNodes);
    return rootNodes;
  }

  onSelectedNodeChange(node: any) {
    this.selectedNode.set(node?.entity || null);
  }

  onDrop(event: any) {
    const node = event.dragNode?.origin?.entity;
    if (!node) {
      return;
    }

    const newParentId = event.dragNode?.parent?.key === '0' ? null : event.dragNode?.parent?.key;
    const position = event.dragNode?.pos || 0;

    const parentNodeName =
      !newParentId || newParentId === '0'
        ? 'Root'
        : event.dragNode?.parent?.origin?.entity?.displayName || 'Root';

    this.confirmationService
      .warn('CmsKit::MenuItemMoveConfirmMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [node.displayName || '', parentNodeName],
        yesText: 'AbpUi::Yes',
        cancelText: 'AbpUi::Cancel',
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          const input: MenuItemMoveInput = {
            newParentId: newParentId === '0' ? null : newParentId,
            position: position,
          };

          this.menuItemService.moveMenuItem(node.id!, input).subscribe({
            next: () => this.loadMenuItems(),
            error: () => this.loadMenuItems(),
          });
        } else {
          this.loadMenuItems();
        }
      });
  }

  beforeDrop = (event: any) => {
    return of(true);
  };

  add() {
    this.selectedMenuItem.set(null);
    this.parentId.set(null);
    this.isModalVisible.set(true);
  }

  addSubMenuItem(parentId?: string) {
    this.selectedMenuItem.set(null);
    this.parentId.set(parentId || null);
    this.isModalVisible.set(true);
  }

  edit(id: string) {
    this.menuItemService.get(id).subscribe(menuItem => {
      this.selectedMenuItem.set(menuItem);
      this.parentId.set(null);
      this.isModalVisible.set(true);
    });
  }

  onVisibleModalChange(visibilityChange: MenuItemModalVisibleChange) {
    if (visibilityChange.visible) {
      return;
    }
    if (visibilityChange.refresh) {
      this.loadMenuItems();
    }
    this.selectedMenuItem.set(null);
    this.parentId.set(null);
    this.isModalVisible.set(false);
  }

  delete(id: string, displayName?: string) {
    this.confirmationService
      .warn('CmsKit::MenuItemDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [displayName || ''],
        yesText: 'AbpUi::Yes',
        cancelText: 'AbpUi::Cancel',
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.menuItemService.delete(id).subscribe({
            next: () => this.loadMenuItems(),
          });
        }
      });
  }
}
