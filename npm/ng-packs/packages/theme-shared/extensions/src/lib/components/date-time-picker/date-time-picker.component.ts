import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  Optional,
  SkipSelf,
  ViewChild,
} from '@angular/core';
import { ControlContainer } from '@angular/forms';
import {
  NgbDateAdapter,
  NgbInputDatepicker,
  NgbTimeAdapter,
  NgbTimepicker,
} from '@ng-bootstrap/ng-bootstrap';
import { DateTimeAdapter } from '../../adapters/date-time.adapter';
import { FormProp } from '../../models/form-props';
import { selfFactory } from '../../utils/factory.util';

@Component({
  exportAs: 'abpDateTimePicker',
  selector: 'abp-date-time-picker',
  template: `
    <input
      [id]="prop.id"
      [formControlName]="prop.name"
      (ngModelChange)="setTime($event)"
      (click)="datepicker.open()"
      (keyup.space)="datepicker.open()"
      ngbDatepicker
      #datepicker="ngbDatepicker"
      type="text"
      class="form-control"
    />
    <ngb-timepicker
      #timepicker
      [formControlName]="prop.name"
      (ngModelChange)="setDate($event)"
      [meridian]="meridian"
    ></ngb-timepicker>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: selfFactory,
      deps: [[new Optional(), new SkipSelf(), ControlContainer]],
    },
    {
      provide: NgbDateAdapter,
      useClass: DateTimeAdapter,
    },
    {
      provide: NgbTimeAdapter,
      useClass: DateTimeAdapter,
    },
  ],
})
export class DateTimePickerComponent {
  @Input() prop: FormProp;
  @Input() meridian = false;

  @ViewChild(NgbInputDatepicker) date: NgbInputDatepicker;
  @ViewChild(NgbTimepicker) time: NgbTimepicker;

  constructor(public readonly cdRef: ChangeDetectorRef) {}

  setDate(datestr: string) {
    this.date.writeValue(datestr);
  }

  setTime(datestr: string) {
    this.time.writeValue(datestr);
  }
}
