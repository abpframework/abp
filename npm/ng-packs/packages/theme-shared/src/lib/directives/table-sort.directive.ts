import { SortOrder, SortPipe } from '@abp/ng.core';
import {
  ChangeDetectorRef,
  Directive,
  Host,
  Input,
  OnChanges,
  Optional,
  Self,
  SimpleChanges,
} from '@angular/core';
import clone from 'just-clone';
import snq from 'snq';
import { TableComponent } from '../components/table/table.component';

export interface TableSortOptions {
  key: string;
  order: SortOrder;
}

/**
 *
 * @deprecated To be deleted in v5.0
 */
@Directive({
  selector: '[abpTableSort]',
  providers: [SortPipe],
})
export class TableSortDirective implements OnChanges {
  @Input()
  abpTableSort: TableSortOptions;

  @Input()
  value: any[] = [];

  get table(): TableComponent | any {
    return (
      this.abpTable || snq(() => this.cdRef['_view'].component) || snq(() => this.cdRef['context']) // 'context' for ivy
    );
  }

  constructor(
    @Host() @Optional() @Self() private abpTable: TableComponent,
    private sortPipe: SortPipe,
    private cdRef: ChangeDetectorRef,
  ) {}

  ngOnChanges({ value, abpTableSort }: SimpleChanges) {
    if (this.table && (value || abpTableSort)) {
      this.abpTableSort = this.abpTableSort || ({} as TableSortOptions);
      this.table.value = this.sortPipe.transform(
        clone(this.value),
        this.abpTableSort.order,
        this.abpTableSort.key,
      );
    }
  }
}
