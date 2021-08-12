import { Component, EventEmitter, Input, Output } from '@angular/core';

/**
 * @deprecated To be deleted in v5.0. Use ngx-datatale instead.
 */
@Component({
  selector: 'abp-sort-order-icon',
  templateUrl: './sort-order-icon.component.html',
})
export class SortOrderIconComponent {
  private _order: 'asc' | 'desc' | '';
  private _selectedSortKey: string;

  @Input()
  sortKey: string;

  @Input()
  set selectedSortKey(value: string) {
    this._selectedSortKey = value;
    this.selectedSortKeyChange.emit(value);
  }
  get selectedSortKey(): string {
    return this._selectedSortKey;
  }

  @Input()
  set order(value: 'asc' | 'desc' | '') {
    this._order = value;
    this.orderChange.emit(value);
  }
  get order(): 'asc' | 'desc' | '' {
    return this._order;
  }

  @Output() readonly orderChange = new EventEmitter<string>();
  @Output() readonly selectedSortKeyChange = new EventEmitter<string>();

  @Input()
  iconClass: string;

  get icon(): string {
    if (this.selectedSortKey === this.sortKey) return `sorting_${this.order}`;
    else return 'sorting';
  }

  sort(key: string) {
    this.selectedSortKey = key;
    switch (this.order) {
      case '':
        this.order = 'asc';
        this.orderChange.emit('asc');
        break;
      case 'asc':
        this.order = 'desc';
        this.orderChange.emit('desc');
        break;
      case 'desc':
        this.order = '';
        this.orderChange.emit('');
        break;
    }
  }
}
