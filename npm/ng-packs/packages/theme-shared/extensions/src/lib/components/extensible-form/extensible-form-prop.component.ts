import { EXTENSIONS_FORM_PROP, EXTENSIONS_FORM_PROP_DATA } from './../../tokens/extensions.token';
import { ABP, AbpValidators, ConfigStateService, TrackByService } from '@abp/ng.core';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Injector,
  Input,
  OnChanges,
  Optional,
  SimpleChanges,
  SkipSelf,
  ViewChild,
} from '@angular/core';
import {
  ControlContainer,
  FormGroupDirective,
  UntypedFormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { NgbDateAdapter, NgbTimeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { DateAdapter } from '../../adapters/date.adapter';
import { TimeAdapter } from '../../adapters/time.adapter';
import { EXTRA_PROPERTIES_KEY } from '../../constants/extra-properties';
import { ePropType } from '../../enums/props.enum';
import { FormProp } from '../../models/form-props';
import { PropData } from '../../models/props';
import { selfFactory } from '../../utils/factory.util';
import { addTypeaheadTextSuffix } from '../../utils/typeahead.util';
import { eThemeSharedComponents } from '../../enums/components';

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
export class ExtensibleFormPropComponent implements OnChanges, AfterViewInit {
  @Input() data!: PropData;

  @Input() prop!: FormProp;

  @Input() first?: boolean;

  @ViewChild('field') private fieldRef!: ElementRef<HTMLElement>;

  public injectorForCustomComponent: Injector;

  asterisk = '';

  containerClassName = 'mb-3';

  options$: Observable<ABP.Option<any>[]> = of([]);

  validators: ValidatorFn[] = [];

  readonly!: boolean;

  typeaheadModel: any;

  passwordKey = eThemeSharedComponents.PasswordComponent;

  private readonly form: UntypedFormGroup;

  disabledFn = (data: PropData) => false;

  get disabled() {
    return this.disabledFn(this.data);
  }

  setTypeaheadValue(selectedOption: ABP.Option<string>) {
    this.typeaheadModel = selectedOption || { key: null, value: null };
    const { key, value } = this.typeaheadModel;
    const [keyControl, valueControl] = this.getTypeaheadControls();
    if (valueControl?.value && !value) valueControl.markAsDirty();
    keyControl?.setValue(key);
    valueControl?.setValue(value);
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

  get meridian() {
    return (
      this.configState.getDeep('localization.currentCulture.dateTimeFormat.shortTimePattern') || ''
    ).includes('tt');
  }

  get isInvalid() {
    const control = this.form.get(this.prop.name);
    return control.touched && control.invalid;
  }

  constructor(
    public readonly cdRef: ChangeDetectorRef,
    public readonly track: TrackByService,
    protected configState: ConfigStateService,
    groupDirective: FormGroupDirective,
    private injector: Injector,
  ) {
    this.form = groupDirective.form;
  }

  private getTypeaheadControls() {
    const { name } = this.prop;
    const extraPropName = `${EXTRA_PROPERTIES_KEY}.${name}`;
    const keyControl =
      this.form.get(addTypeaheadTextSuffix(extraPropName)) ||
      this.form.get(addTypeaheadTextSuffix(name));
    const valueControl = this.form.get(extraPropName) || this.form.get(name);
    return [keyControl, valueControl];
  }

  private setAsterisk() {
    this.asterisk = this.validators.some(isRequired) ? '*' : '';
  }

  ngAfterViewInit() {
    if (this.first && this.fieldRef) {
      this.fieldRef.nativeElement.focus();
      this.cdRef.detectChanges();
    }
  }

  getComponent(prop: FormProp): string {
    if (prop.template) {
      return 'template';
    }
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
      case ePropType.PasswordInputGroup:
        return 'passwordinputgroup';
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
      case ePropType.PasswordInputGroup:
        return 'passwordinputgroup';
      default:
        return 'hidden';
    }
  }

  ngOnChanges({ prop, data }: SimpleChanges) {
    const currentProp = prop?.currentValue as FormProp;
    const { options, readonly, disabled, validators, className, template } = currentProp || {};
    if (template) {
      this.injectorForCustomComponent = Injector.create({
        providers: [
          {
            provide: EXTENSIONS_FORM_PROP,
            useValue: currentProp,
          },
          {
            provide: EXTENSIONS_FORM_PROP_DATA,
            useValue: (data?.currentValue as PropData)?.record,
          },
          { provide: ControlContainer, useExisting: FormGroupDirective },
        ],
        parent: this.injector,
      });
    }

    if (options) this.options$ = options(this.data);
    if (readonly) this.readonly = readonly(this.data);

    if (disabled) {
      this.disabledFn = disabled;
    }
    if (validators) {
      this.validators = validators(this.data);
      this.setAsterisk();
    }
    if (className !== undefined) {
      this.containerClassName = className;
    }

    const [keyControl, valueControl] = this.getTypeaheadControls();
    if (keyControl && valueControl)
      this.typeaheadModel = { key: keyControl.value, value: valueControl.value };
  }
}

function isRequired(validator: ValidatorFn) {
  return (
    validator === Validators.required ||
    validator === AbpValidators.required ||
    validator.name === 'required'
  );
}
