import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'abp-sort-order-icon',
  templateUrl: './sort-order-icon.component.html',
})
export class SortOrderIconComponent {
  private _order: 'asc' | 'desc' | '';
  private _selectedSortKey: string;

  /**
   * @deprecated use selectedSortKey instead.
   */
  @Input()
  set selectedKey(value: string) {
    this.selectedSortKey = value;
    this.selectedKeyChange.emit(value);
  }
  get selectedKey(): string {
    return this._selectedSortKey;
  }

  @Input()
  set selectedSortKey(value: string) {
    this._selectedSortKey = value;
    this.selectedSortKeyChange.emit(value);
  }
  get selectedSortKey(): string {
    return this._selectedSortKey;
  }

  @Output() readonly selectedKeyChange = new EventEmitter<string>();
  @Output() readonly selectedSortKeyChange = new EventEmitter<string>();

  /**
   * @deprecated use sortKey instead.
   */
  @Input()
  get key(): string {
    return this.sortKey;
  }
  set key(value: string) {
    this.sortKey = value;
  }

  @Input()
  sortKey: string;

  @Input()
  set order(value: 'asc' | 'desc' | '') {
    this._order = value;
    this.orderChange.emit(value);
  }
  get order(): 'asc' | 'desc' | '' {
    return this._order;
  }

  @Output() readonly orderChange = new EventEmitter<string>();

  @Input()
  iconClass: string;

  get icon(): string {
    if (!this.selectedSortKey) return 'fa-sort';
    if (this.selectedSortKey === this.sortKey) return `fa-sort-${this.order}`;
    else return '';
  }

  sort(key: string) {
    this.selectedKey = key; // TODO: To be removed
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
        this.selectedKey = ''; // TODO: To be removed
        this.orderChange.emit('');
        break;
    }
  }
}
