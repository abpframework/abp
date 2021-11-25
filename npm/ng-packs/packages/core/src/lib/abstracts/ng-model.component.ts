import { ChangeDetectorRef, Component, Injector, Input } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';

// Not an abstract class on purpose. Do not change!
@Component({ template: '' })
export class AbstractNgModelComponent<T = any, U = T> implements ControlValueAccessor {
  protected _value: T;
  protected cdRef: ChangeDetectorRef;
  onChange: (value: T) => void;
  onTouched: () => void;

  @Input()
  disabled: boolean;

  @Input()
  readonly: boolean;

  @Input()
  valueFn: (value: U, previousValue?: T) => T = value => value as any as T;

  @Input()
  valueLimitFn: (value: T, previousValue?: T) => any = value => false;

  @Input()
  set value(value: T) {
    value = this.valueFn(value as any as U, this._value);

    if (this.valueLimitFn(value, this._value) !== false || this.readonly) return;

    this._value = value;
    this.notifyValueChange();
  }

  get value(): T {
    return this._value || this.defaultValue;
  }

  get defaultValue(): T {
    return this._value;
  }

  constructor(public injector: Injector) {
    this.cdRef = injector.get(ChangeDetectorRef);
  }

  notifyValueChange(): void {
    if (this.onChange) {
      this.onChange(this.value);
    }
  }

  writeValue(value: T): void {
    this._value = this.valueLimitFn(value, this._value) || value;
    setTimeout(() => this.cdRef.markForCheck(), 0);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
