import { EXTENSIONS_FORM_PROP, EXTENSIONS_FORM_PROP_DATA } from './../../tokens/extensions.token';
import {
  ABP,
  LocalizationModule,
  PermissionDirective,
  ShowPasswordDirective,
  TrackByService,
} from '@abp/ng.core';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  inject,
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
  FormsModule,
  ReactiveFormsModule,
  ValidatorFn,
} from '@angular/forms';
import {
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTypeaheadModule,
} from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { DateAdapter, DisabledDirective, TimeAdapter } from '@abp/ng.theme.shared';
import { EXTRA_PROPERTIES_KEY } from '../../constants/extra-properties';
import { FormProp } from '../../models/form-props';
import { PropData } from '../../models/props';
import { selfFactory } from '../../utils/factory.util';
import { addTypeaheadTextSuffix } from '../../utils/typeahead.util';
import { eExtensibleComponents } from '../../enums/components';
import { ExtensibleDateTimePickerComponent } from '../date-time-picker/extensible-date-time-picker.component';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ExtensibleFormPropService } from '../../services/extensible-form-prop.service';
import { CreateInjectorPipe } from '../../pipes/create-injector.pipe';
import { CommonModule } from '@angular/common';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'abp-extensible-form-prop',
  templateUrl: './extensible-form-prop.component.html',
  standalone: true,
  imports: [
    ExtensibleDateTimePickerComponent,
    NgbDatepickerModule,
    NgbTimepickerModule,
    ReactiveFormsModule,
    DisabledDirective,
    NgxValidateCoreModule,
    NgbTooltip,
    NgbTypeaheadModule,
    CreateInjectorPipe,
    ShowPasswordDirective,
    PermissionDirective,
    LocalizationModule,
    CommonModule,
    FormsModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ExtensibleFormPropService],
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
  protected service = inject(ExtensibleFormPropService);
  public readonly cdRef = inject(ChangeDetectorRef);
  public readonly track = inject(TrackByService);
  #groupDirective = inject(FormGroupDirective);
  private injector = inject(Injector);
  private readonly form = this.#groupDirective.form;

  @Input() data!: PropData;
  @Input() prop!: FormProp;
  @Input() first?: boolean;
  @Input() isFirstGroup?: boolean;
  @ViewChild('field') private fieldRef!: ElementRef<HTMLElement>;

  injectorForCustomComponent?: Injector;
  asterisk = '';
  containerClassName = 'mb-2';
  showPassword = false;
  options$: Observable<ABP.Option<any>[]> = of([]);
  validators: ValidatorFn[] = [];
  readonly!: boolean;
  typeaheadModel: any;
  passwordKey = eExtensibleComponents.PasswordComponent;

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
          switchMap(text => this.prop?.options?.(this.data, text) || of([])),
        )
      : of([]);

  typeaheadFormatter = (option: ABP.Option<any>) => option.key;

  meridian$ = this.service.meridian$;

  get isInvalid() {
    const control = this.form.get(this.prop.name);
    return control?.touched && control.invalid;
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
    this.asterisk = this.service.calcAsterisks(this.validators);
  }

  ngAfterViewInit() {
    if (this.isFirstGroup && this.first && this.fieldRef) {
      this.fieldRef.nativeElement.focus();
    }
  }

  getComponent(prop: FormProp): string {
    return this.service.getComponent(prop);
  }

  getType(prop: FormProp): string {
    return this.service.getType(prop);
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
