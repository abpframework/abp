import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  TemplateRef,
  TrackByFunction,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';

/**
 *
 * @deprecated To be deleted in v5.0. Use ngx-datatale instead.
 */
@Component({
  selector: 'abp-table',
  templateUrl: 'table.component.html',
  styleUrls: ['table.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class TableComponent implements OnInit {
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

  @ViewChild('wrapper', { read: ElementRef })
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

  marginCalculator: MarginCalculator;

  trackByFn: TrackByFunction<any> = (_, value) => {
    return typeof value === 'object' ? value[this.trackingProp] || value : value;
  };

  ngOnInit() {
    this.marginCalculator = document.body.dir === 'rtl' ? rtlCalculator : ltrCalculator;
  }
}

function ltrCalculator(div: HTMLDivElement): string {
  return `0 auto 0 -${div.scrollLeft}px`;
}

function rtlCalculator(div: HTMLDivElement): string {
  return `0 ${-(div.scrollWidth - div.clientWidth - div.scrollLeft)}px 0 auto`;
}

type MarginCalculator = (div: HTMLDivElement) => string;
