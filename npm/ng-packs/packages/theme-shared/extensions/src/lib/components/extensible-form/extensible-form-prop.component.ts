import { ABP, TrackByService } from '@abp/ng.core';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnChanges,
  Optional,
  SimpleChanges,
  SkipSelf,
} from '@angular/core';
import {
  ControlContainer,
  FormGroup,
  FormGroupDirective,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { NgbDateAdapter, NgbTimeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import snq from 'snq';
import { DateAdapter } from '../../adapters/date.adapter';
import { TimeAdapter } from '../../adapters/time.adapter';
import { EXTRA_PROPERTIES_KEY } from '../../constants/extra-properties';
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

  asterisk = '';

  options$: Observable<ABP.Option<any>[]> = of([]);

  validators: ValidatorFn[] = [];

  readonly: boolean;

  disabled: boolean;

  private readonly form: FormGroup;

  typeaheadModel: any;

  setTypeaheadValue(selectedOption: ABP.Option<string>) {
    this.typeaheadModel = selectedOption || { key: null, value: null };
    const { key, value } = this.typeaheadModel;
    const [keyControl, valueControl] = this.getTypeaheadControls();
    if (valueControl.value && !value) valueControl.markAsDirty();
    keyControl.setValue(key);
    valueControl.setValue(value);
  }

  search = (text$: Observable<string>) =>
    text$
      ? text$.pipe(
          debounceTime(300),
          distinctUntilChanged(),
          switchMap(text => this.prop.options(this.data, text)),
        )
      : of([]);

  typeaheadFormatter = (option: ABP.Option<any>) => option.key;

  get isInvalid() {
    const control = this.form.get(this.prop.name);
    return control.touched && control.invalid;
  }

  constructor(
    public readonly cdRef: ChangeDetectorRef,
    public readonly track: TrackByService,
    groupDirective: FormGroupDirective,
  ) {
    this.form = groupDirective.form;
  }

  private getTypeaheadControls() {
    const { name } = this.prop;
    const textSuffix = '_Text';
    const extraPropName = `${EXTRA_PROPERTIES_KEY}.${name}`;
    const keyControl =
      this.form.get(extraPropName + textSuffix) || this.form.get(name + textSuffix);
    const valueControl = this.form.get(extraPropName) || this.form.get(name);
    return [keyControl, valueControl];
  }

  private setAsterisk() {
    this.asterisk = this.validators.some(isRequired) ? '*' : '';
  }

  getComponent(prop: FormProp): string {
    switch (prop.type) {
      case ePropType.Boolean:
        return 'checkbox';
      case ePropType.Date:
        return 'date';
      case ePropType.DateTime:
        return 'dateTime';
      case ePropType.Hidden:
        return 'hidden';
      case ePropType.MultiSelect:
        return 'multiselect';
      case ePropType.Text:
        return 'textarea';
      case ePropType.Time:
        return 'time';
      case ePropType.Typeahead:
        return 'typeahead';
      default:
        return prop.options ? 'select' : 'input';
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
    const currentProp = snq<FormProp>(() => prop.currentValue);
    const { options, readonly, disabled, validators } = currentProp || {};

    if (options) this.options$ = options(this.data);
    if (readonly) this.readonly = readonly(this.data);
    if (disabled) this.disabled = disabled(this.data);
    if (validators) {
      this.validators = validators(this.data);
      this.setAsterisk();
    }

    const [keyControl, valueControl] = this.getTypeaheadControls();
    if (keyControl && valueControl)
      this.typeaheadModel = { key: keyControl.value, value: valueControl.value };
  }
}

function isRequired(validator: ValidatorFn) {
  return validator === Validators.required || validator.toString().includes('required');
}
