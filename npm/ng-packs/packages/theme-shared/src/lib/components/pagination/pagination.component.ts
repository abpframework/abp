import { Component, Input, OnInit, Output, EventEmitter, TrackByFunction } from '@angular/core';

@Component({
  selector: 'abp-pagination',
  templateUrl: 'pagination.component.html',
})
export class PaginationComponent implements OnInit {
  private _value = 1;
  @Input()
  get value(): number {
    return this._value;
  }
  set value(newValue: number) {
    if (this._value === newValue) return;

    this._value = newValue;
    this.valueChange.emit(newValue);
  }

  @Output()
  readonly valueChange = new EventEmitter<number>();

  @Input()
  totalPages = 0;

  get pageArray(): number[] {
    const count = this.totalPages < 5 ? this.totalPages : 5;

    if (this.value === 1 || this.value === 2) {
      return Array.from(new Array(count)).map((_, index) => index + 1);
    } else if (this.value === this.totalPages || this.value === this.totalPages - 1) {
      return Array.from(new Array(count)).map((_, index) => this.totalPages - count + 1 + index);
    } else {
      return [this.value - 2, this.value - 1, this.value, this.value + 1, this.value + 2];
    }
  }

  trackByFn: TrackByFunction<number> = (_, page) => page;

  ngOnInit() {
    if (!this.value || this.value < 1 || this.value > this.totalPages) {
      this.value = 1;
    }
  }

  changePage(page: number) {
    if (page < 1) return;
    else if (page > this.totalPages) return;

    this.value = page;
  }
}
