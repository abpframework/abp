import {
  Component,
  ContentChild,
  EventEmitter,
  Inject,
  Input,
  OnInit,
  Optional,
  Output,
  TemplateRef,
  ViewEncapsulation,
} from '@angular/core';
import { NzFormatBeforeDropEvent, NzFormatEmitEvent, NzTreeNode } from 'ng-zorro-antd/tree';
import { of } from 'rxjs';
import { TreeNodeTemplateDirective } from '../templates/tree-node-template.directive';
import { ExpandedIconTemplateDirective } from '../templates/expanded-icon-template.directive';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
import { LazyLoadService, LOADING_STRATEGY, SubscriptionService } from '@abp/ng.core';
import { DISABLE_TREE_STYLE_LOADING_TOKEN } from '../disable-tree-style-loading.token';

export type DropEvent = NzFormatEmitEvent & { pos: number };

@Component({
  selector: 'abp-tree',
  templateUrl: 'tree.component.html',
  styleUrls: ['tree.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [SubscriptionService],
})
export class TreeComponent implements OnInit {
  dropPosition: number;

  dropdowns = {} as { [key: string]: NgbDropdown };

  constructor(
    private lazyLoadService: LazyLoadService,
    private subscriptionService: SubscriptionService,
    @Optional()
    @Inject(DISABLE_TREE_STYLE_LOADING_TOKEN)
    private disableTreeStyleLoading: boolean | undefined,
  ) {}

  @ContentChild('menu') menu: TemplateRef<any>;
  @ContentChild(TreeNodeTemplateDirective) customNodeTemplate: TreeNodeTemplateDirective;
  @ContentChild(ExpandedIconTemplateDirective) expandedIconTemplate: ExpandedIconTemplateDirective;
  @Output() readonly checkedKeysChange = new EventEmitter();
  @Output() readonly expandedKeysChange = new EventEmitter<string[]>();
  @Output() readonly selectedNodeChange = new EventEmitter();
  @Output() readonly dropOver = new EventEmitter<DropEvent>();
  @Output() readonly nzExpandChange = new EventEmitter<NzFormatEmitEvent>();
  @Input() noAnimation = true;
  @Input() draggable: boolean;
  @Input() checkable: boolean;
  @Input() checkStrictly: boolean;
  @Input() checkedKeys = [];
  @Input() nodes = [];
  @Input() expandedKeys: string[] = [];
  @Input() selectedNode: any;
  @Input() changeCheckboxWithNode: boolean;
  @Input() isNodeSelected = node => this.selectedNode?.id === node.key;
  @Input() beforeDrop = (event: NzFormatBeforeDropEvent) => {
    this.dropPosition = event.pos;
    return of(false);
  };

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

  onSelectedNodeChange(node: NzTreeNode) {
    this.selectedNode = node.origin.entity;
    if (this.changeCheckboxWithNode) {
      let newVal;
      if (node.isChecked) {
        newVal = this.checkedKeys.filter(x => x !== node.key);
      } else {
        newVal = [...this.checkedKeys, node.key];
      }
      this.selectedNodeChange.emit(node);
      this.checkedKeys = newVal;
      this.checkedKeysChange.emit(newVal);
    } else {
      this.selectedNodeChange.emit(node.origin.entity);
    }
  }

  onCheckboxChange(event) {
    this.checkedKeys = [...event.keys];
    this.checkedKeysChange.emit(event.keys);
  }

  onExpandedKeysChange(event) {
    this.expandedKeys = [...event.keys];
    this.expandedKeysChange.emit(event.keys);
    this.nzExpandChange.emit(event);
  }

  onDrop(event: DropEvent) {
    event.event.stopPropagation();
    event.event.preventDefault();
    event.pos = this.dropPosition;

    this.dropOver.emit(event);
  }

  initDropdown(key: string, dropdown: NgbDropdown) {
    this.dropdowns[key] = dropdown;
  }
}
