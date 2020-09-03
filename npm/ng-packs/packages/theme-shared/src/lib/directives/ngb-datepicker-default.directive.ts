import { Directive, HostBinding, Input, Injector, ElementRef } from '@angular/core';
import { NgbDatepicker, NgbInputDatepicker, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { THEME_SHARED_OPTIONS } from '../tokens/options.token';

@Directive({
  // tslint:disable-next-line
  selector: 'ngb-datepicker, input[ngbDatepicker]',
})
export class NgbDatepickerDefaultDirective {
  @Input() minDate: NgbDateStruct;
  @Input() maxDate: NgbDateStruct;

  datepicker: NgbDatepicker | NgbInputDatepicker;

  constructor(private injector: Injector) {
    const {
      ngbDatepickerOptions: { minDate, maxDate },
    } = this.injector.get(THEME_SHARED_OPTIONS);

    this.datepicker =
      this.injector.get(NgbInputDatepicker, null) || this.injector.get(NgbDatepicker, null);
    if (!this.datepicker) return;

    this.datepicker.minDate = this.minDate || minDate;
    this.datepicker.maxDate = this.maxDate || maxDate;
  }
}
