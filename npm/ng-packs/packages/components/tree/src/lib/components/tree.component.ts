import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  contentChild,
  inject,
  OnInit,
  TemplateRef,
  ViewEncapsulation,
  input,
  output,
  signal,
  effect
} from '@angular/core';
import { NgbDropdown, NgbDropdownMenu, NgbDropdownToggle } from '@ng-bootstrap/ng-bootstrap';
import {
  NzFormatBeforeDropEvent,
  NzFormatEmitEvent,
  NzTreeComponent,
  NzTreeNode,
} from 'ng-zorro-antd/tree';
import {
  InitDirective,
  LazyLoadService,
  LOADING_STRATEGY,
  SubscriptionService,
} from '@abp/ng.core';
import { of } from 'rxjs';
import { DISABLE_TREE_STYLE_LOADING_TOKEN } from '../disable-tree-style-loading.token';
import { TreeNodeTemplateDirective } from '../templates/tree-node-template.directive';
import { ExpandedIconTemplateDirective } from '../templates/expanded-icon-template.directive';
import { NgTemplateOutlet } from '@angular/common';
import { NzNoAnimationDirective } from 'ng-zorro-antd/core/animation';

export type DropEvent = NzFormatEmitEvent & { pos: number };

@Component({
  selector: 'abp-tree',
  templateUrl: 'tree.component.html',
  styleUrls: ['tree.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [SubscriptionService],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    NgTemplateOutlet,
    NzTreeComponent,
    NgbDropdown,
    NgbDropdownMenu,
    NgbDropdownToggle,
    InitDirective,
    NzNoAnimationDirective,
  ],
})
export class TreeComponent implements OnInit {
  private lazyLoadService = inject(LazyLoadService);
  private subscriptionService = inject(SubscriptionService);
  private cdr = inject(ChangeDetectorRef);
  private disableTreeStyleLoading = inject(DISABLE_TREE_STYLE_LOADING_TOKEN, { optional: true });

  dropPosition!: number;

  dropdowns = {} as { [key: string]: NgbDropdown };

  readonly menu = contentChild<TemplateRef<any>>('menu');
  readonly customNodeTemplate = contentChild(TreeNodeTemplateDirective);
  readonly expandedIconTemplate = contentChild(ExpandedIconTemplateDirective);
  readonly checkedKeysChange = output<any>();
  readonly expandedKeysChange = output<string[]>();
  readonly selectedNodeChange = output<any>();
  readonly dropOver = output<DropEvent>();
  readonly nzExpandChange = output<NzFormatEmitEvent>();
  
  // Input signals
  readonly noAnimation = input(true);
  readonly draggable = input<boolean | undefined>(undefined);
  readonly checkable = input<boolean | undefined>(undefined);
  readonly checkStrictly = input<boolean | undefined>(undefined);
  readonly checkedKeysInput = input<any[]>([], { alias: 'checkedKeys' });
  readonly nodesInput = input<any[]>([], { alias: 'nodes' });
  readonly expandedKeysInput = input<string[]>([], { alias: 'expandedKeys' });
  readonly selectedNodeInput = input<any>(undefined, { alias: 'selectedNode' });
  readonly changeCheckboxWithNode = input<boolean | undefined>(undefined);
  readonly isNodeSelectedFn = input<(node: any) => boolean>(
    (node) => this._selectedNode()?.id === node.key,
    { alias: 'isNodeSelected' }
  );
  readonly beforeDropFn = input<(event: NzFormatBeforeDropEvent) => any>(
    (event: NzFormatBeforeDropEvent) => {
      this.dropPosition = event.pos;
      return of(false);
    },
    { alias: 'beforeDrop' }
  );

  // Internal signals for two-way binding
  protected readonly _checkedKeys = signal<any[]>([]);
  protected readonly _expandedKeys = signal<string[]>([]);
  protected readonly _selectedNode = signal<any>(undefined);
  protected readonly _nodes = signal<any[]>([]);

  // Getters for template access
  get checkedKeys() { return this._checkedKeys(); }
  get expandedKeys() { return this._expandedKeys(); }
  get selectedNode() { return this._selectedNode(); }
  get nodes() { return this._nodes(); }
  get isNodeSelected() { return this.isNodeSelectedFn(); }
  get beforeDrop() { return this.beforeDropFn(); }

  constructor() {
    // Sync input signals to internal signals
    effect(() => {
      this._checkedKeys.set(this.checkedKeysInput());
      this._expandedKeys.set(this.expandedKeysInput());
      this._selectedNode.set(this.selectedNodeInput());
      this._nodes.set(this.nodesInput());
    });
  }

  ngOnInit() {
    this.loadStyle();
  }

  private loadStyle() {
    if (this.disableTreeStyleLoading) {
      return;
    }
    const loaded$ = this.lazyLoadService.load(
      LOADING_STRATEGY.AppendAnonymousStyleToHead('ng-zorro-antd-tree.css'),
    );
    this.subscriptionService.addOne(loaded$);
  }

  private findNode(target: any, nodes: any[]): any {
    for (const node of nodes) {
      if (node.key === target.id) {
        return node;
      }
      if (node.children) {
        const res: any = this.findNode(target, node.children);
        if (res) {
          return res;
        }
      }
    }
    return null;
  }

  onSelectedNodeChange(node: NzTreeNode) {
    this._selectedNode.set(node.origin.entity);
    if (this.changeCheckboxWithNode()) {
      const keys = this._checkedKeys();
      let newVal;
      if (node.isChecked) {
        newVal = keys.filter(x => x !== node.key);
      } else {
        newVal = [...keys, node.key];
      }
      this.selectedNodeChange.emit(node);
      this._checkedKeys.set(newVal);
      this.checkedKeysChange.emit(newVal);
    } else {
      this.selectedNodeChange.emit(node.origin.entity);
    }
  }

  onCheckboxChange(event: { keys: any[] }) {
    this._checkedKeys.set([...event.keys]);
    this.checkedKeysChange.emit(event.keys);
  }

  onExpandedKeysChange(event: { keys: string[] } & NzFormatEmitEvent) {
    this._expandedKeys.set([...event.keys]);
    this.expandedKeysChange.emit(event.keys);
    this.nzExpandChange.emit(event);
  }

  onDrop(event: DropEvent) {
    event.event?.stopPropagation();
    event.event?.preventDefault();
    event.pos = this.dropPosition;

    this.dropOver.emit(event);
  }

  initDropdown(key: string, dropdown: NgbDropdown) {
    this.dropdowns[key] = dropdown;
  }

  onContextMenuChange(event: NzFormatEmitEvent) {
    const dropdownKey = event.node?.key;

    Object.entries(this.dropdowns).forEach(([key, dropdown]) => {
      if (key !== dropdownKey && dropdown?.isOpen()) {
        dropdown.close();
      }
    });
    if (dropdownKey) {
      this.dropdowns[dropdownKey]?.toggle();
    }
  }

  setSelectedNode(node: any) {
    const newSelectedNode = this.findNode(node, this._nodes());
    this._selectedNode.set({ ...newSelectedNode });
    this.cdr.markForCheck();
  }
}
