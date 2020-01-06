import {
  Component,
  OnInit,
  Input,
  TemplateRef,
  Output,
  EventEmitter,
  TrackByFunction,
} from '@angular/core';

@Component({
  selector: 'abp-table',
  templateUrl: 'table.component.html',
})
export class TableComponent implements OnInit {
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

  @Output()
  readonly pageChange = new EventEmitter<number>();

  page = 1;

  bodyScrollLeft = 0;

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

  ngOnInit() {}
}
