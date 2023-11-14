import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  Optional,
  SkipSelf,
  ViewChild,
} from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import {
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbInputDatepicker,
  NgbTimeAdapter,
  NgbTimepicker,
  NgbTimepickerModule,
} from '@ng-bootstrap/ng-bootstrap';
import { FormProp } from '../../models/form-props';
import { selfFactory } from '../../utils/factory.util';
import { DateTimeAdapter } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';

@Component({
  exportAs: 'abpExtensibleDateTimePicker',
  standalone: true,
  imports: [CommonModule, NgbDatepickerModule, ReactiveFormsModule, NgbTimepickerModule],
  selector: 'abp-extensible-date-time-picker',
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
export class ExtensibleDateTimePickerComponent {
  @Input() prop!: FormProp;
  @Input() meridian = false;

  @ViewChild(NgbInputDatepicker) date!: NgbInputDatepicker;
  @ViewChild(NgbTimepicker) time!: NgbTimepicker;

  constructor(public readonly cdRef: ChangeDetectorRef) {}

  setDate(dateStr: string) {
    this.date.writeValue(dateStr);
  }

  setTime(dateStr: string) {
    this.time.writeValue(dateStr);
  }
}
