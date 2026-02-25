import { Component, OnInit, inject } from '@angular/core';
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

  nodes: any[] = [];
  selectedNode: MenuItemDto | null = null;
  expandedKeys: string[] = [];
  draggable = true;
  isModalVisible = false;
  selectedMenuItem: MenuItemDto | MenuItemWithDetailsDto | null = null;
  parentId: string | null = null;

  ngOnInit() {
    this.loadMenuItems();
  }

  private loadMenuItems() {
    this.menuItemService.getList().subscribe(result => {
      if (result.items && result.items.length > 0) {
        this.nodes = this.buildTreeNodes(result.items);
        // Expand all nodes by default
        this.expandedKeys = this.nodes.map(n => n.key);
      } else {
        this.nodes = [];
      }
    });
  }

  private buildTreeNodes(items: MenuItemDto[]): any[] {
    const nodeMap = new Map<string, any>();
    const rootNodes: any[] = [];

    // First pass: create all nodes
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

    // Second pass: build tree structure
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

    // Sort by order
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
    this.selectedNode = node?.entity || null;
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
            next: () => {
              this.loadMenuItems();
            },
            error: () => {
              // Reload to rollback
              this.loadMenuItems();
            },
          });
        } else {
          // Reload to rollback
          this.loadMenuItems();
        }
      });
  }

  beforeDrop = (event: any) => {
    return of(true);
  };

  add() {
    this.selectedMenuItem = null;
    this.parentId = null;
    this.isModalVisible = true;
  }

  addSubMenuItem(parentId?: string) {
    this.selectedMenuItem = null;
    this.parentId = parentId || null;
    this.isModalVisible = true;
  }

  edit(id: string) {
    this.menuItemService.get(id).subscribe(menuItem => {
      this.selectedMenuItem = menuItem;
      this.parentId = null;
      this.isModalVisible = true;
    });
  }

  onVisibleModalChange(visibilityChange: MenuItemModalVisibleChange) {
    if (visibilityChange.visible) {
      return;
    }
    if (visibilityChange.refresh) {
      this.loadMenuItems();
    }
    this.selectedMenuItem = null;
    this.parentId = null;
    this.isModalVisible = false;
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
            next: () => {
              this.loadMenuItems();
            },
          });
        }
      });
  }
}
