import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  TemplateRef,
  TrackByFunction,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';

@Component({
  selector: 'abp-table',
  templateUrl: 'table.component.html',
  styles: [
    `
      .ui-table .ui-table-tbody > tr:nth-child(even):hover,
      .ui-table .ui-table-tbody > tr:hover {
        filter: brightness(90%);
      }

      .ui-table .ui-table-tbody > tr.empty-row:hover {
        filter: none;
      }

      .ui-table .ui-table-tbody > tr.empty-row > div.empty-row-content {
        padding: 10px;
        text-align: center;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class TableComponent {
  private _totalRecords: number;
  bodyScrollLeft = 0;

  @Input()
  value: any[];

  @Input()
  headerTemplate: TemplateRef<any>;

  @Input()
  bodyTemplate: TemplateRef<any>;

  @Input()
  colgroupTemplate: TemplateRef<any>;

  @Input()
  scrollHeight: string;

  @Input()
  scrollable: boolean;

  @Input()
  rows: number;

  @Input()
  page = 1;

  @Input()
  trackingProp = 'id';

  @Input()
  emptyMessage = 'AbpAccount::NoDataAvailableInDatatable';

  @Output()
  readonly pageChange = new EventEmitter<number>();

  @ViewChild('wrapper', { read: ElementRef, static: false })
  wrapperRef: ElementRef<HTMLDivElement>;

  @Input()
  get totalRecords(): number {
    return this._totalRecords || this.value.length;
  }
  set totalRecords(newValue: number) {
    if (newValue < 0) this._totalRecords = 0;

    this._totalRecords = newValue;
  }

  get totalPages(): number {
    if (!this.rows) {
      return;
    }

    return Math.ceil(this.totalRecords / this.rows);
  }

  get slicedValue(): any[] {
    if (!this.rows || this.rows >= this.value.length) {
      return this.value;
    }

    const start = (this.page - 1) * this.rows;
    return this.value.slice(start, start + this.rows);
  }

  trackByFn: TrackByFunction<any> = (_, value) => {
    return typeof value === 'object' ? value[this.trackingProp] || value : value;
  };
}
