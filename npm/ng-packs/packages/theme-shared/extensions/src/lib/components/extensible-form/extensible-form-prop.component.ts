import { ABP, TrackByService } from '@abp/ng.core';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  Optional,
  SimpleChanges,
  SkipSelf,
  OnChanges,
} from '@angular/core';
import { ControlContainer, Validators, ValidatorFn } from '@angular/forms';
import { NgbDateAdapter, NgbTimeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { DateAdapter } from '../../adapters/date.adapter';
import { TimeAdapter } from '../../adapters/time.adapter';
import { ePropType } from '../../enums/props.enum';
import { FormProp } from '../../models/form-props';
import { PropData } from '../../models/props';
import { selfFactory } from '../../utils/factory.util';

@Component({
  selector: 'abp-extensible-form-prop',
  templateUrl: './extensible-form-prop.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: selfFactory,
      deps: [[new Optional(), new SkipSelf(), ControlContainer]],
    },
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
})
export class ExtensibleFormPropComponent implements OnChanges {
  @Input() data: PropData;

  @Input() prop: FormProp;

  options$: Observable<ABP.Option<any>[]> = of([]);

  validators: ValidatorFn[] = [];

  readonly: boolean;

  disabled: boolean;

  constructor(public readonly cdRef: ChangeDetectorRef, public readonly track: TrackByService) {}

  get asterisk(): string {
    return this.validators.some(validator => validator === Validators.required) ? '*' : '';
  }

  getComponent(prop: FormProp): string {
    if (prop.options) return 'select';

    switch (prop.type) {
      case ePropType.Boolean:
        return 'checkbox';
      case ePropType.Date:
        return 'date';
      case ePropType.DateTime:
        return 'dateTime';
      case ePropType.Text:
        return 'textarea';
      case ePropType.Time:
        return 'time';
      default:
        return 'input';
    }
  }

  getType(prop: FormProp): string {
    switch (prop.type) {
      case ePropType.Date:
      case ePropType.String:
        return 'text';
      case ePropType.Boolean:
        return 'checkbox';
      case ePropType.Number:
        return 'number';
      case ePropType.Email:
        return 'email';
      case ePropType.Password:
        return 'password';
      default:
        return 'hidden';
    }
  }

  ngOnChanges({ prop }: SimpleChanges) {
    const options = prop.currentValue.options;
    const readonly = prop.currentValue.readonly;
    const disabled = prop.currentValue.disabled;
    const validators = prop.currentValue.validators;

    if (options) this.options$ = options(this.data);
    if (readonly) this.readonly = readonly(this.data);
    if (disabled) this.disabled = disabled(this.data);
    if (validators) this.validators = validators(this.data);
  }
}
