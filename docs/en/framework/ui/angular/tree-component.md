```json
//[doc-seo]
{
    "Description": "Learn how to use the ABP Angular tree component for hierarchical data, selection, templates and drag-and-drop."
}
```

# Tree Component

`TreeComponent` is a standalone tree control exported by `@abp/ng.components/tree`. It supports selection, checkboxes, expansion, context-menu templates and drag-and-drop.

The `@abp/ng.components` package is included in the Angular application templates. Install it if the application does not already reference it:

```bash
npm install @abp/ng.components
```

Import it into a standalone component and provide nodes in the format expected by the underlying NG-ZORRO tree:

```ts
import { Component, signal } from '@angular/core';
import {
  DropEvent,
  ExpandedIconTemplateDirective,
  TreeComponent,
  TreeNodeTemplateDirective,
} from '@abp/ng.components/tree';
import { of } from 'rxjs';

interface Category {
  id: string;
  name: string;
}

@Component({
  selector: 'app-category-tree',
  templateUrl: './category-tree.component.html',
  imports: [
    TreeComponent,
    TreeNodeTemplateDirective,
    ExpandedIconTemplateDirective,
  ],
})
export class CategoryTreeComponent {
  readonly nodes = signal([
    {
      key: 'books',
      title: 'Books',
      entity: { id: 'books', name: 'Books' } as Category,
      children: [],
      isLeaf: true,
    },
  ]);

  readonly expandedKeys = signal<string[]>([]);
  readonly selectedCategory = signal<Category | null>(null);

  readonly allowDrop = () => of(true);

  handleDrop(event: DropEvent) {
    console.log(event.dragNode?.key);
  }

  edit(key: string) {
    console.log(key);
  }
}
```

```html
<abp-tree
  [nodes]="nodes()"
  [expandedKeys]="expandedKeys()"
  [selectedNode]="selectedCategory()"
  [draggable]="true"
  [beforeDrop]="allowDrop"
  (expandedKeysChange)="expandedKeys.set($event)"
  (selectedNodeChange)="selectedCategory.set($event)"
  (dropOver)="handleDrop($event)"
>
  <ng-template #menu let-node>
    <button type="button" class="dropdown-item" (click)="edit(node.key)">
      Edit
    </button>
  </ng-template>
</abp-tree>
```

The main inputs are:

- `nodes`, `checkedKeys`, `expandedKeys` and `selectedNode` for tree state.
- `draggable`, `checkable`, `checkStrictly` and `noAnimation` for tree behavior.
- `changeCheckboxWithNode` to update checked keys when a node is selected.
- `isNodeSelected` to replace the default selected-node comparison.
- `beforeDrop` to approve or reject a drag-and-drop operation.

State changes are exposed through `checkedKeysChange`, `expandedKeysChange`, `selectedNodeChange`, `dropOver` and `nzExpandChange`.

The default `beforeDrop` handler rejects drops. Supply a handler that returns an observable accepted by the underlying tree control when drag-and-drop is enabled.

## Templates

Use the `#menu` template, as in the previous example, to add a context menu for each node. Use `abpTreeNodeTemplate` and `abpTreeExpandedIconTemplate` to replace the node and expanded-icon templates:

{%{
```html
<abp-tree [nodes]="nodes()">
  <ng-template abpTreeNodeTemplate let-node>
    <strong>{{ node.title }}</strong>
  </ng-template>

  <ng-template abpTreeExpandedIconTemplate let-node>
    <span>{{ node.isExpanded ? '−' : '+' }}</span>
  </ng-template>
</abp-tree>
```
}%}

The component example imports `TreeNodeTemplateDirective` and `ExpandedIconTemplateDirective` because Angular must see each directive used by a standalone component template. If you use only one of these templates, import only its corresponding directive.

## Flat-List Adapter

`TreeAdapter<T>` converts a flat list of `BaseNode` values into tree nodes. Each item requires an `id` and a nullable `parentId`; its `displayName` or `name` becomes the default node title. Use `getTree()`, `handleDrop()`, `handleRemove()` and `handleUpdate()` to keep the flat list and tree representations synchronized.

## Style Loading

The component loads `ng-zorro-antd-tree.css` when it is initialized. Provide `DISABLE_TREE_STYLE_LOADING_TOKEN` with `true` only when the application already includes that stylesheet:

```ts
import { DISABLE_TREE_STYLE_LOADING_TOKEN } from '@abp/ng.components/tree';

export const providers = [
  {
    provide: DISABLE_TREE_STYLE_LOADING_TOKEN,
    useValue: true,
  },
];
```
