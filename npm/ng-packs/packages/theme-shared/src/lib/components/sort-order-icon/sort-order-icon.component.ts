import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'abp-sort-order-icon',
  templateUrl: './sort-order-icon.component.html',
})
export class SortOrderIconComponent {
  private _order: string;
  private _selectedKey: string;

  @Input()
  set selectedKey(value: string) {
    this._selectedKey = value;
    this.selectedKeyChange.emit(value);
  }
  get selectedKey(): string {
    return this._selectedKey;
  }

  @Output() readonly selectedKeyChange = new EventEmitter<string>();

  @Input()
  key: string;

  @Input()
  set order(value: string) {
    this._order = value;
    this.orderChange.emit(value);
  }
  get order(): string {
    return this._order;
  }

  @Output() readonly orderChange = new EventEmitter<string>();

  @Input()
  iconClass: string;

  get icon(): string {
    if (!this.selectedKey) return 'fa-sort';
    if (this.selectedKey === this.key) return `fa-sort-${this.order}`;
    else return '';
  }

  sort(key: string) {
    this.selectedKey = key;
    switch (this.order) {
      case '':
        this.order = 'asc';
        break;
      case 'asc':
        this.order = 'desc';
        this.orderChange.emit('desc');
        break;
      case 'desc':
        this.order = '';
        this.selectedKey = '';
        break;
    }
  }
}
