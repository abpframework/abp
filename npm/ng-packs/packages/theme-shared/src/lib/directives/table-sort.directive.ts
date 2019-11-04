import { Directive, Input, Optional, Self, SimpleChanges, OnChanges } from '@angular/core';
import { Table } from 'primeng/table';
import clone from 'just-clone';
import { SortPipe, SortOrder } from '@abp/ng.core';

export interface TableSortOptions {
  key: string;
  order: SortOrder;
}

@Directive({
  selector: '[abpTableSort]',
  providers: [SortPipe],
})
export class TableSortDirective implements OnChanges {
  @Input()
  abpTableSort: TableSortOptions;
  @Input()
  value: any[] = [];
  constructor(@Optional() @Self() private table: Table, private sortPipe: SortPipe) {}
  ngOnChanges({ value, abpTableSort }: SimpleChanges) {
    if (value || abpTableSort) {
      this.abpTableSort = this.abpTableSort || ({} as TableSortOptions);
      this.table.value = this.sortPipe.transform(clone(this.value), this.abpTableSort.order, this.abpTableSort.key);
    }
  }
}
