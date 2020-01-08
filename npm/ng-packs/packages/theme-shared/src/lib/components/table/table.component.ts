import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  TemplateRef,
  TrackByFunction,
  ViewChild,
  ViewEncapsulation,
  AfterViewInit,
} from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

@Component({
  selector: 'abp-table',
  templateUrl: 'table.component.html',
  styles: [
    `
      .ui-table .ui-table-tbody > tr:nth-child(even):hover,
      .ui-table .ui-table-tbody > tr:hover {
        background-color: #eaeaea;
      }

      .ui-table .ui-table-tbody > tr.empty-row:hover {
        background-color: transparent;
      }

      .ui-table .ui-table-tbody > tr.empty-row > div {
        margin: 10px;
        text-align: center;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class TableComponent implements AfterViewInit {
  private _totalRecords: number;

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
  trackingProp = 'id';

  @Input()
  emptyMessage = 'AbpAccount::NoDataAvailableInDatatable';

  @Output()
  readonly pageChange = new EventEmitter<number>();

  @ViewChild('wrapper', { read: ElementRef, static: false })
  wrapperRef: ElementRef<HTMLDivElement>;

  page = 1;

  bodyScrollLeft = 0;

  colspan: number;

  trackByFn: TrackByFunction<any> = (_, value) => {
    return typeof value === 'object' ? value[this.trackingProp] || value : value;
  };

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

  ngAfterViewInit() {
    setTimeout(() => {
      this.colspan = this.wrapperRef.nativeElement.querySelectorAll('th').length;
    }, 0);
  }
}
