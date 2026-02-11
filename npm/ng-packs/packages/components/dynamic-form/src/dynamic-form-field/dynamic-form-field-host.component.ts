import {
  Component,
  ViewContainerRef,
  ChangeDetectionStrategy,
  forwardRef,
  Type,
  effect,
  DestroyRef,
  inject,
  input,
  viewChild
} from '@angular/core';
import {
  ControlValueAccessor, NG_VALUE_ACCESSOR, FormControl, ReactiveFormsModule
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

type controlValueAccessorLike = Partial<ControlValueAccessor> & { setDisabledState?(d: boolean): void };
type acceptsFormControl = { formControl?: FormControl };

@Component({
  selector: 'abp-dynamic-form-field-host',
  imports: [CommonModule, ReactiveFormsModule],
  template: `<ng-template #vcRef></ng-template>`,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DynamicFieldHostComponent),
    multi: true
  }]
})
export class DynamicFieldHostComponent implements ControlValueAccessor {
  component = input<Type<ControlValueAccessor>>();
  inputs = input<Record<string, any>>({});

  readonly viewContainerRef = viewChild.required('vcRef', { read: ViewContainerRef });
  private componentRef?: any;

  private value: any;
  private disabled = false;

  // if child has not implemented ControlValueAccessor. Create form control
  private innerControl = new FormControl<any>(null);
  readonly destroyRef = inject(DestroyRef);

  constructor() {
    effect(() => {
      if (this.component()) {
        this.createChild();
      } else if (this.componentRef && this.inputs()) {
        this.applyInputs();
      }
    });
  }

  private createChild() {
    this.viewContainerRef().clear();
    if (!this.component()) return;

    this.componentRef = this.viewContainerRef().createComponent(this.component());
    this.applyInputs();

    const instance: any = this.componentRef.instance as controlValueAccessorLike & acceptsFormControl;

    if (this.isCVA(instance)) {
      // Child CVA ise wrapper -> child delege
      instance.registerOnChange?.((v: any) => this.onChange(v));
      instance.registerOnTouched?.(() => this.onTouched());
      if (this.disabled && instance.setDisabledState) {
        instance.setDisabledState(true);
      }
      // set initial value
      if (this.value !== undefined) {
        instance.writeValue?.(this.value);
      }
    } else {
      // No CVA -> use form control
      if ('formControl' in instance) {
        instance.formControl = this.innerControl;
        // apply initial value/disabled state
        if (this.value !== undefined) {
          this.innerControl.setValue(this.value, { emitEvent: false });
        }
        this.innerControl.valueChanges.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(v => this.onChange(v));
        this.innerControl.disabled ? null : (this.disabled && this.innerControl.disable({ emitEvent: false }));
      }
    }
  }

  private applyInputs() {
    if (!this.componentRef) return;
    const inst = this.componentRef.instance;
    for (const [k, v] of Object.entries(this.inputs ?? {})) {
      inst[k] = v;
    }
    this.componentRef.changeDetectorRef?.markForCheck?.();
  }

  private isCVA(obj: any): obj is controlValueAccessorLike {
    return obj && typeof obj.writeValue === 'function' && typeof obj.registerOnChange === 'function';
  }

  writeValue(obj: any): void {
    this.value = obj;
    if (!this.componentRef) return;

    const inst: any = this.componentRef.instance as controlValueAccessorLike & acceptsFormControl;

    if (this.isCVA(inst)) {
      inst.writeValue?.(obj);
    } else if ('formControl' in inst && inst.formControl instanceof FormControl) {
      inst.formControl.setValue(obj, { emitEvent: false });
    }
  }

  private onChange: (v: any) => void = () => {};
  private onTouched: () => void = () => {};

  registerOnChange(fn: any): void { this.onChange = fn; }
  registerOnTouched(fn: any): void { this.onTouched = fn; }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    if (!this.componentRef) return;

    const inst = this.componentRef.instance as controlValueAccessorLike & acceptsFormControl;

    if (this.isCVA(inst) && inst.setDisabledState) {
      inst.setDisabledState(isDisabled);
    } else if ('formControl' in inst && inst.formControl instanceof FormControl) {
      isDisabled ? inst.formControl.disable({ emitEvent: false }) : inst.formControl.enable({ emitEvent: false });
    }
  }
}
