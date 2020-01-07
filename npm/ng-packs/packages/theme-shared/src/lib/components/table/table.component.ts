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
} from '@angular/core';

@Component({
  selector: 'abp-table',
  templateUrl: 'table.component.html',
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

  colspan = 1;

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
    if (!this.rows || this.rows > this.value.length) {
      return this.value;
    }

    const start = (this.page - 1) * this.rows;
    return this.value.slice(start, start + this.rows);
  }

  constructor() {}

  ngAfterViewInit() {
    this.colspan = this.wrapperRef.nativeElement.querySelectorAll('th').length;
  }
}
