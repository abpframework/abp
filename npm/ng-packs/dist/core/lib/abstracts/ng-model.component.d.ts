import { ControlValueAccessor } from '@angular/forms';
import { ChangeDetectorRef, Injector } from '@angular/core';
export declare class AbstractNgModelComponent<T = any> implements ControlValueAccessor {
    injector: Injector;
    disabled: boolean;
    value: T;
    onChange: (value: T) => {};
    onTouched: () => {};
    protected _value: T;
    protected cdRef: ChangeDetectorRef;
    constructor(injector: Injector);
    notifyValueChange(): void;
    writeValue(value: T): void;
    registerOnChange(fn: any): void;
    registerOnTouched(fn: any): void;
    setDisabledState(isDisabled: boolean): void;
}
