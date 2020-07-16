import {
  Component,
  ContentChild,
  EventEmitter,
  Input,
  Output,
  TemplateRef,
  ViewEncapsulation,
} from '@angular/core';

@Component({
  selector: 'abp-tree',
  templateUrl: 'tree.component.html',
  styleUrls: [
    '../../../../../../node_modules/ng-zorro-antd/tree/style/index.min.css',
    'tree.component.scss',
  ],
  encapsulation: ViewEncapsulation.None,
})
export class TreeComponent {
  @ContentChild('menu') menu: TemplateRef<any>;
  @Input() checkable: boolean;
  @Input() checkedKeys = [];
  @Output() checkedKeysChange = new EventEmitter();
  @Input() nodes = [];
  @Input() expandedKeys: string[] = [];
  @Output() expandedKeysChange = new EventEmitter<string[]>();
  @Input() isNodeSelected = node => this.selectedNode?.id === node.key;
  @Input() selectedNode: any;
  @Output() selectedNodeChange = new EventEmitter();

  onSelectedNodeChange(node) {
    this.selectedNode = node.origin.entity;
    this.selectedNodeChange.emit(node.origin.entity);
  }

  onCheckboxChange(event) {
    console.log(event);
    this.checkedKeys = event.checkedKeys;
    this.checkedKeysChange.emit(event.checkedKeys);
  }

  onExpandedKeysChange(event) {
    this.expandedKeys = event.keys;
    this.expandedKeysChange.emit(event.keys);
  }
}
