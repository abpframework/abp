import { EXTENSIONS_FORM_PROP, EXTENSIONS_FORM_PROP_DATA } from './../../tokens/extensions.token';
import {
  ABP,
  LocalizationPipe,
  PermissionDirective,
  ShowPasswordDirective,
  TrackByService,
} from '@abp/ng.core';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  computed,
  ElementRef,
  inject,
  Injector,
  Optional,
  SkipSelf,
  viewChild,
  effect,
  input,
  signal,
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
import { ReadonlyPropData } from '../../models/props';
import { selfFactory } from '../../utils/factory.util';
import { addTypeaheadTextSuffix } from '../../utils/typeahead.util';
import { eExtensibleComponents } from '../../enums/components';
import { ExtensibleDateTimePickerComponent } from '../date-time-picker/extensible-date-time-picker.component';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ExtensibleFormPropService } from '../../services/extensible-form-prop.service';
import { AsyncPipe, NgComponentOutlet, NgTemplateOutlet } from '@angular/common';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { ExtensibleFormMultiselectComponent } from '../multi-select/extensible-form-multiselect.component';

@Component({
  selector: 'abp-extensible-form-prop',
  templateUrl: './extensible-form-prop.component.html',
  imports: [
    ExtensibleDateTimePickerComponent,
    ExtensibleFormMultiselectComponent,
    NgbDatepickerModule,
    NgbTimepickerModule,
    ReactiveFormsModule,
    DisabledDirective,
    NgxValidateCoreModule,
    NgbTooltip,
    NgbTypeaheadModule,
    ShowPasswordDirective,
    PermissionDirective,
    LocalizationPipe,
    AsyncPipe,
    NgComponentOutlet,
    NgTemplateOutlet,
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
export class ExtensibleFormPropComponent implements AfterViewInit {
  protected service = inject(ExtensibleFormPropService);
  public readonly track = inject(TrackByService);
  #groupDirective = inject(FormGroupDirective);
  private injector = inject(Injector);
  private readonly form = this.#groupDirective.form;

  readonly data = input.required<ReadonlyPropData>();
  readonly prop = input.required<FormProp>();
  readonly first = input<boolean | undefined>(undefined);
  readonly isFirstGroup = input<boolean | undefined>(undefined);
  private readonly fieldRef = viewChild.required<ElementRef<HTMLElement>>('field');

  readonly injectorForCustomComponent = signal<Injector | undefined>(undefined);
  readonly asterisk = signal('');
  readonly containerClassName = signal('mb-2');
  readonly showPassword = signal(false);
  readonly options$ = signal<Observable<ABP.Option<any>[]>>(of([]));
  readonly validators = signal<ValidatorFn[]>([]);
  readonly isReadonly = signal(false);
  readonly typeaheadModel = signal<any>(undefined);
  passwordKey = eExtensibleComponents.PasswordComponent;
  readonly disabledFn = signal<(data: ReadonlyPropData) => boolean>(() => false);

  readonly disabled = computed(() => {
    const data = this.data();
    if (!data) return false;
    return this.disabledFn()(data);
  });

  constructor() {
    effect(() => {
      const currentProp = this.prop();
      const data = this.data();
      if (!currentProp || !data) return;

      const { options, readonly, disabled, validators, className, template } = currentProp;

      if (template) {
        this.injectorForCustomComponent.set(
          Injector.create({
            providers: [
              {
                provide: EXTENSIONS_FORM_PROP,
                useValue: currentProp,
              },
              {
                provide: EXTENSIONS_FORM_PROP_DATA,
                useValue: data?.record,
              },
              { provide: ControlContainer, useExisting: FormGroupDirective },
            ],
            parent: this.injector,
          }),
        );
      } else {
        this.injectorForCustomComponent.set(undefined);
      }

      if (options) this.options$.set(options(data));
      if (readonly) this.isReadonly.set(readonly(data));

      if (disabled) {
        this.disabledFn.set(disabled);
      } else {
        this.disabledFn.set(() => false);
      }

      if (validators) {
        this.validators.set(validators(data));
        this.asterisk.set(this.service.calcAsterisks(validators(data)));
      } else {
        this.validators.set([]);
        this.asterisk.set('');
      }

      if (className !== undefined) {
        this.containerClassName.set(className);
      } else {
        this.containerClassName.set('mb-2');
      }

      const [keyControl, valueControl] = this.getTypeaheadControls();
      if (keyControl && valueControl) {
        this.typeaheadModel.set({ key: keyControl.value, value: valueControl.value });
      }
    });
  }

  setTypeaheadValue(selectedOption: ABP.Option<string>) {
    const model = selectedOption || { key: null, value: null };
    this.typeaheadModel.set(model);
    const { key, value } = model;
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
          switchMap(text => this.prop()?.options?.(this.data(), text) || of([])),
        )
      : of([]);

  typeaheadFormatter = (option: ABP.Option<any>) => option.key;

  meridian$ = this.service.meridian$;

  get isInvalid() {
    const control = this.form.get(this.prop().name);
    return control?.touched && control.invalid;
  }

  private getTypeaheadControls() {
    const { name } = this.prop();
    const extraPropName = `${EXTRA_PROPERTIES_KEY}.${name}`;
    const keyControl =
      this.form.get(addTypeaheadTextSuffix(extraPropName)) ||
      this.form.get(addTypeaheadTextSuffix(name));
    const valueControl = this.form.get(extraPropName) || this.form.get(name);
    return [keyControl, valueControl];
  }

  ngAfterViewInit() {
    if (this.isFirstGroup() && this.first() && this.fieldRef()) {
      requestAnimationFrame(() => {
        this.fieldRef().nativeElement.focus();
      });
    }
  }

  getComponent(prop: FormProp): string {
    return this.service.getComponent(prop);
  }

  getType(prop: FormProp): string {
    return this.service.getType(prop);
  }
}
