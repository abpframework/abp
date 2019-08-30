/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Directive, ElementRef, EventEmitter, Input, Output, Self, } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';
import { fromEvent } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { takeUntilDestroy } from '../utils';
export class FormSubmitDirective {
    /**
     * @param {?} formGroupDirective
     * @param {?} host
     * @param {?} cdRef
     */
    constructor(formGroupDirective, host, cdRef) {
        this.formGroupDirective = formGroupDirective;
        this.host = host;
        this.cdRef = cdRef;
        this.ngSubmit = new EventEmitter();
        this.executedNgSubmit = false;
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.formGroupDirective.ngSubmit.pipe(takeUntilDestroy(this)).subscribe((/**
         * @return {?}
         */
        () => {
            this.markAsDirty();
            this.executedNgSubmit = true;
        }));
        fromEvent((/** @type {?} */ (this.host.nativeElement)), 'keyup')
            .pipe(debounceTime(200), filter((/**
         * @param {?} key
         * @return {?}
         */
        (key) => key && key.key === 'Enter')), takeUntilDestroy(this))
            .subscribe((/**
         * @return {?}
         */
        () => {
            if (!this.executedNgSubmit) {
                this.host.nativeElement.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
            }
            this.executedNgSubmit = false;
        }));
        fromEvent(this.host.nativeElement, 'submit')
            .pipe(takeUntilDestroy(this), filter((/**
         * @return {?}
         */
        () => !this.notValidateOnSubmit && typeof this.notValidateOnSubmit !== 'string')))
            .subscribe((/**
         * @return {?}
         */
        () => {
            if (!this.executedNgSubmit) {
                this.markAsDirty();
            }
        }));
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
    /**
     * @return {?}
     */
    markAsDirty() {
        const { form } = this.formGroupDirective;
        setDirty((/** @type {?} */ (form.controls)));
        form.markAsDirty();
        this.cdRef.detectChanges();
    }
}
FormSubmitDirective.decorators = [
    { type: Directive, args: [{
                selector: 'form[ngSubmit][formGroup]',
            },] }
];
/** @nocollapse */
FormSubmitDirective.ctorParameters = () => [
    { type: FormGroupDirective, decorators: [{ type: Self }] },
    { type: ElementRef },
    { type: ChangeDetectorRef }
];
FormSubmitDirective.propDecorators = {
    notValidateOnSubmit: [{ type: Input }],
    ngSubmit: [{ type: Output }]
};
if (false) {
    /** @type {?} */
    FormSubmitDirective.prototype.notValidateOnSubmit;
    /** @type {?} */
    FormSubmitDirective.prototype.ngSubmit;
    /** @type {?} */
    FormSubmitDirective.prototype.executedNgSubmit;
    /**
     * @type {?}
     * @private
     */
    FormSubmitDirective.prototype.formGroupDirective;
    /**
     * @type {?}
     * @private
     */
    FormSubmitDirective.prototype.host;
    /**
     * @type {?}
     * @private
     */
    FormSubmitDirective.prototype.cdRef;
}
/**
 * @param {?} controls
 * @return {?}
 */
function setDirty(controls) {
    if (Array.isArray(controls)) {
        controls.forEach((/**
         * @param {?} group
         * @return {?}
         */
        group => {
            setDirty((/** @type {?} */ (group.controls)));
        }));
        return;
    }
    Object.keys(controls).forEach((/**
     * @param {?} key
     * @return {?}
     */
    key => {
        controls[key].markAsDirty();
        controls[key].updateValueAndValidity();
    }));
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9ybS1zdWJtaXQuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZm9ybS1zdWJtaXQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQ0wsaUJBQWlCLEVBQ2pCLFNBQVMsRUFDVCxVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sSUFBSSxHQUNMLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBMEIsa0JBQWtCLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUM1RSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2pDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdEQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBTzVDLE1BQU0sT0FBTyxtQkFBbUI7Ozs7OztJQVM5QixZQUNrQixrQkFBc0MsRUFDOUMsSUFBaUMsRUFDakMsS0FBd0I7UUFGaEIsdUJBQWtCLEdBQWxCLGtCQUFrQixDQUFvQjtRQUM5QyxTQUFJLEdBQUosSUFBSSxDQUE2QjtRQUNqQyxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQVBsQyxhQUFRLEdBQUcsSUFBSSxZQUFZLEVBQUUsQ0FBQztRQUU5QixxQkFBZ0IsR0FBWSxLQUFLLENBQUM7SUFNL0IsQ0FBQzs7OztJQUVKLFFBQVE7UUFDTixJQUFJLENBQUMsa0JBQWtCLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLFNBQVM7OztRQUFDLEdBQUcsRUFBRTtZQUMzRSxJQUFJLENBQUMsV0FBVyxFQUFFLENBQUM7WUFDbkIsSUFBSSxDQUFDLGdCQUFnQixHQUFHLElBQUksQ0FBQztRQUMvQixDQUFDLEVBQUMsQ0FBQztRQUVILFNBQVMsQ0FBQyxtQkFBQSxJQUFJLENBQUMsSUFBSSxDQUFDLGFBQWEsRUFBZSxFQUFFLE9BQU8sQ0FBQzthQUN2RCxJQUFJLENBQ0gsWUFBWSxDQUFDLEdBQUcsQ0FBQyxFQUNqQixNQUFNOzs7O1FBQUMsQ0FBQyxHQUFrQixFQUFFLEVBQUUsQ0FBQyxHQUFHLElBQUksR0FBRyxDQUFDLEdBQUcsS0FBSyxPQUFPLEVBQUMsRUFDMUQsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQ3ZCO2FBQ0EsU0FBUzs7O1FBQUMsR0FBRyxFQUFFO1lBQ2QsSUFBSSxDQUFDLElBQUksQ0FBQyxnQkFBZ0IsRUFBRTtnQkFDMUIsSUFBSSxDQUFDLElBQUksQ0FBQyxhQUFhLENBQUMsYUFBYSxDQUFDLElBQUksS0FBSyxDQUFDLFFBQVEsRUFBRSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxDQUFDLENBQUMsQ0FBQzthQUNqRztZQUVELElBQUksQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7UUFDaEMsQ0FBQyxFQUFDLENBQUM7UUFFTCxTQUFTLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxhQUFhLEVBQUUsUUFBUSxDQUFDO2FBQ3pDLElBQUksQ0FDSCxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsRUFDdEIsTUFBTTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsbUJBQW1CLElBQUksT0FBTyxJQUFJLENBQUMsbUJBQW1CLEtBQUssUUFBUSxFQUFDLENBQ3hGO2FBQ0EsU0FBUzs7O1FBQUMsR0FBRyxFQUFFO1lBQ2QsSUFBSSxDQUFDLElBQUksQ0FBQyxnQkFBZ0IsRUFBRTtnQkFDMUIsSUFBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO2FBQ3BCO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsV0FBVyxLQUFVLENBQUM7Ozs7SUFFdEIsV0FBVztjQUNILEVBQUUsSUFBSSxFQUFFLEdBQUcsSUFBSSxDQUFDLGtCQUFrQjtRQUV4QyxRQUFRLENBQUMsbUJBQUEsSUFBSSxDQUFDLFFBQVEsRUFBa0MsQ0FBQyxDQUFDO1FBQzFELElBQUksQ0FBQyxXQUFXLEVBQUUsQ0FBQztRQUVuQixJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBRSxDQUFDO0lBQzdCLENBQUM7OztZQTNERixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLDJCQUEyQjthQUN0Qzs7OztZQVRnQyxrQkFBa0IsdUJBb0I5QyxJQUFJO1lBNUJQLFVBQVU7WUFGVixpQkFBaUI7OztrQ0FxQmhCLEtBQUs7dUJBR0wsTUFBTTs7OztJQUhQLGtEQUNzQzs7SUFFdEMsdUNBQzhCOztJQUU5QiwrQ0FBa0M7Ozs7O0lBR2hDLGlEQUFzRDs7Ozs7SUFDdEQsbUNBQXlDOzs7OztJQUN6QyxvQ0FBZ0M7Ozs7OztBQStDcEMsU0FBUyxRQUFRLENBQUMsUUFBa0I7SUFDbEMsSUFBSSxLQUFLLENBQUMsT0FBTyxDQUFDLFFBQVEsQ0FBQyxFQUFFO1FBQzNCLFFBQVEsQ0FBQyxPQUFPOzs7O1FBQUMsS0FBSyxDQUFDLEVBQUU7WUFDdkIsUUFBUSxDQUFDLG1CQUFBLEtBQUssQ0FBQyxRQUFRLEVBQWtDLENBQUMsQ0FBQztRQUM3RCxDQUFDLEVBQUMsQ0FBQztRQUNILE9BQU87S0FDUjtJQUVELE1BQU0sQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsT0FBTzs7OztJQUFDLEdBQUcsQ0FBQyxFQUFFO1FBQ2xDLFFBQVEsQ0FBQyxHQUFHLENBQUMsQ0FBQyxXQUFXLEVBQUUsQ0FBQztRQUM1QixRQUFRLENBQUMsR0FBRyxDQUFDLENBQUMsc0JBQXNCLEVBQUUsQ0FBQztJQUN6QyxDQUFDLEVBQUMsQ0FBQztBQUNMLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBDaGFuZ2VEZXRlY3RvclJlZixcbiAgRGlyZWN0aXZlLFxuICBFbGVtZW50UmVmLFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE9uSW5pdCxcbiAgT3V0cHV0LFxuICBTZWxmLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEZvcm1Db250cm9sLCBGb3JtR3JvdXAsIEZvcm1Hcm91cERpcmVjdGl2ZSB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IGZyb21FdmVudCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaWx0ZXIgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnLi4vdXRpbHMnO1xuXG50eXBlIENvbnRyb2xzID0geyBba2V5OiBzdHJpbmddOiBGb3JtQ29udHJvbCB9IHwgRm9ybUdyb3VwW107XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ2Zvcm1bbmdTdWJtaXRdW2Zvcm1Hcm91cF0nLFxufSlcbmV4cG9ydCBjbGFzcyBGb3JtU3VibWl0RGlyZWN0aXZlIGltcGxlbWVudHMgT25Jbml0LCBPbkRlc3Ryb3kge1xuICBASW5wdXQoKVxuICBub3RWYWxpZGF0ZU9uU3VibWl0OiBzdHJpbmcgfCBib29sZWFuO1xuXG4gIEBPdXRwdXQoKVxuICBuZ1N1Ym1pdCA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcblxuICBleGVjdXRlZE5nU3VibWl0OiBib29sZWFuID0gZmFsc2U7XG5cbiAgY29uc3RydWN0b3IoXG4gICAgQFNlbGYoKSBwcml2YXRlIGZvcm1Hcm91cERpcmVjdGl2ZTogRm9ybUdyb3VwRGlyZWN0aXZlLFxuICAgIHByaXZhdGUgaG9zdDogRWxlbWVudFJlZjxIVE1MRm9ybUVsZW1lbnQ+LFxuICAgIHByaXZhdGUgY2RSZWY6IENoYW5nZURldGVjdG9yUmVmLFxuICApIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgdGhpcy5mb3JtR3JvdXBEaXJlY3RpdmUubmdTdWJtaXQucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgdGhpcy5tYXJrQXNEaXJ0eSgpO1xuICAgICAgdGhpcy5leGVjdXRlZE5nU3VibWl0ID0gdHJ1ZTtcbiAgICB9KTtcblxuICAgIGZyb21FdmVudCh0aGlzLmhvc3QubmF0aXZlRWxlbWVudCBhcyBIVE1MRWxlbWVudCwgJ2tleXVwJylcbiAgICAgIC5waXBlKFxuICAgICAgICBkZWJvdW5jZVRpbWUoMjAwKSxcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkua2V5ID09PSAnRW50ZXInKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICBpZiAoIXRoaXMuZXhlY3V0ZWROZ1N1Ym1pdCkge1xuICAgICAgICAgIHRoaXMuaG9zdC5uYXRpdmVFbGVtZW50LmRpc3BhdGNoRXZlbnQobmV3IEV2ZW50KCdzdWJtaXQnLCB7IGJ1YmJsZXM6IHRydWUsIGNhbmNlbGFibGU6IHRydWUgfSkpO1xuICAgICAgICB9XG5cbiAgICAgICAgdGhpcy5leGVjdXRlZE5nU3VibWl0ID0gZmFsc2U7XG4gICAgICB9KTtcblxuICAgIGZyb21FdmVudCh0aGlzLmhvc3QubmF0aXZlRWxlbWVudCwgJ3N1Ym1pdCcpXG4gICAgICAucGlwZShcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgICAgZmlsdGVyKCgpID0+ICF0aGlzLm5vdFZhbGlkYXRlT25TdWJtaXQgJiYgdHlwZW9mIHRoaXMubm90VmFsaWRhdGVPblN1Ym1pdCAhPT0gJ3N0cmluZycpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIGlmICghdGhpcy5leGVjdXRlZE5nU3VibWl0KSB7XG4gICAgICAgICAgdGhpcy5tYXJrQXNEaXJ0eSgpO1xuICAgICAgICB9XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge31cblxuICBtYXJrQXNEaXJ0eSgpIHtcbiAgICBjb25zdCB7IGZvcm0gfSA9IHRoaXMuZm9ybUdyb3VwRGlyZWN0aXZlO1xuXG4gICAgc2V0RGlydHkoZm9ybS5jb250cm9scyBhcyB7IFtrZXk6IHN0cmluZ106IEZvcm1Db250cm9sIH0pO1xuICAgIGZvcm0ubWFya0FzRGlydHkoKTtcblxuICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICB9XG59XG5cbmZ1bmN0aW9uIHNldERpcnR5KGNvbnRyb2xzOiBDb250cm9scykge1xuICBpZiAoQXJyYXkuaXNBcnJheShjb250cm9scykpIHtcbiAgICBjb250cm9scy5mb3JFYWNoKGdyb3VwID0+IHtcbiAgICAgIHNldERpcnR5KGdyb3VwLmNvbnRyb2xzIGFzIHsgW2tleTogc3RyaW5nXTogRm9ybUNvbnRyb2wgfSk7XG4gICAgfSk7XG4gICAgcmV0dXJuO1xuICB9XG5cbiAgT2JqZWN0LmtleXMoY29udHJvbHMpLmZvckVhY2goa2V5ID0+IHtcbiAgICBjb250cm9sc1trZXldLm1hcmtBc0RpcnR5KCk7XG4gICAgY29udHJvbHNba2V5XS51cGRhdGVWYWx1ZUFuZFZhbGlkaXR5KCk7XG4gIH0pO1xufVxuIl19