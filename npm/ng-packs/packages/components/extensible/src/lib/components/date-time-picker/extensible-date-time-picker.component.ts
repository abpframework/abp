import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  inject,
  input,
  Optional,
  SkipSelf,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import {
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbInputDatepicker,
  NgbTimeAdapter,
  NgbTimepicker,
  NgbTimepickerModule,
  Placement,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { DateTimeAdapter } from '@abp/ng.theme.shared';
import { FormProp } from '../../models/form-props';
import { selfFactory } from '../../utils/factory.util';

@Component({
  exportAs: 'abpExtensibleDateTimePicker',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgxValidateCoreModule,
  ],
  selector: 'abp-extensible-date-time-picker',
  template: `
    <input
      [id]="prop().id"
      [formControlName]="prop().name"
      (ngModelChange)="setTime($event)"
      (click)="datepicker.open()"
      (keyup.space)="datepicker.open()"
      ngbDatepicker
      #datepicker="ngbDatepicker"
      type="text"
      class="form-control"
      [placement]="placement()"
    />
    <ngb-timepicker
      #timepicker
      [formControlName]="prop().name"
      (ngModelChange)="setDate($event)"
      [meridian]="meridian()"
    />
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
  public readonly cdRef = inject(ChangeDetectorRef);

  prop = input<FormProp>();
  meridian = input<boolean>(false);
  placement = input<Placement>('bottom-left');

  @ViewChild(NgbInputDatepicker) date!: NgbInputDatepicker;
  @ViewChild(NgbTimepicker) time!: NgbTimepicker;

  setDate(dateStr: string) {
    this.date.writeValue(dateStr);
  }

  setTime(dateStr: string) {
    this.time.writeValue(dateStr);
  }
}
