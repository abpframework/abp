import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'abp-sort-order-icon',
  templateUrl: './sort-order-icon.component.html',
})
export class SortOrderIconComponent implements OnInit {
  @Input()
  selectedKey: string;

  @Input()
  key: string;

  @Input()
  order: string;

  @Input()
  iconClass: string;

  get icon(): string {
    if (!this.selectedKey) return 'fa-sort';
    if (this.selectedKey === this.key) return `fa-sort-${this.order}`;
    else return '';
  }

  constructor() {}

  ngOnInit(): void {}
}
