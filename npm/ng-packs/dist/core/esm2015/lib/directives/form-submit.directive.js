/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/form-submit.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                // tslint:disable-next-line: directive-selector
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9ybS1zdWJtaXQuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZm9ybS1zdWJtaXQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUNMLGlCQUFpQixFQUNqQixTQUFTLEVBQ1QsVUFBVSxFQUNWLFlBQVksRUFDWixLQUFLLEVBR0wsTUFBTSxFQUNOLElBQUksR0FDTCxNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQTBCLGtCQUFrQixFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDNUUsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqQyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLFVBQVUsQ0FBQztBQVE1QyxNQUFNLE9BQU8sbUJBQW1COzs7Ozs7SUFROUIsWUFDa0Isa0JBQXNDLEVBQzlDLElBQWlDLEVBQ2pDLEtBQXdCO1FBRmhCLHVCQUFrQixHQUFsQixrQkFBa0IsQ0FBb0I7UUFDOUMsU0FBSSxHQUFKLElBQUksQ0FBNkI7UUFDakMsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFQZixhQUFRLEdBQUcsSUFBSSxZQUFZLEVBQUUsQ0FBQztRQUVqRCxxQkFBZ0IsR0FBRyxLQUFLLENBQUM7SUFNdEIsQ0FBQzs7OztJQUVKLFFBQVE7UUFDTixJQUFJLENBQUMsa0JBQWtCLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLFNBQVM7OztRQUFDLEdBQUcsRUFBRTtZQUMzRSxJQUFJLENBQUMsV0FBVyxFQUFFLENBQUM7WUFDbkIsSUFBSSxDQUFDLGdCQUFnQixHQUFHLElBQUksQ0FBQztRQUMvQixDQUFDLEVBQUMsQ0FBQztRQUVILFNBQVMsQ0FBQyxtQkFBQSxJQUFJLENBQUMsSUFBSSxDQUFDLGFBQWEsRUFBZSxFQUFFLE9BQU8sQ0FBQzthQUN2RCxJQUFJLENBQ0gsWUFBWSxDQUFDLEdBQUcsQ0FBQyxFQUNqQixNQUFNOzs7O1FBQUMsQ0FBQyxHQUFrQixFQUFFLEVBQUUsQ0FBQyxHQUFHLElBQUksR0FBRyxDQUFDLEdBQUcsS0FBSyxPQUFPLEVBQUMsRUFDMUQsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQ3ZCO2FBQ0EsU0FBUzs7O1FBQUMsR0FBRyxFQUFFO1lBQ2QsSUFBSSxDQUFDLElBQUksQ0FBQyxnQkFBZ0IsRUFBRTtnQkFDMUIsSUFBSSxDQUFDLElBQUksQ0FBQyxhQUFhLENBQUMsYUFBYSxDQUFDLElBQUksS0FBSyxDQUFDLFFBQVEsRUFBRSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxDQUFDLENBQUMsQ0FBQzthQUNqRztZQUVELElBQUksQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7UUFDaEMsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsV0FBVyxLQUFVLENBQUM7Ozs7SUFFdEIsV0FBVztjQUNILEVBQUUsSUFBSSxFQUFFLEdBQUcsSUFBSSxDQUFDLGtCQUFrQjtRQUV4QyxRQUFRLENBQUMsbUJBQUEsSUFBSSxDQUFDLFFBQVEsRUFBa0MsQ0FBQyxDQUFDO1FBQzFELElBQUksQ0FBQyxXQUFXLEVBQUUsQ0FBQztRQUVuQixJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBRSxDQUFDO0lBQzdCLENBQUM7OztZQWhERixTQUFTLFNBQUM7O2dCQUVULFFBQVEsRUFBRSwyQkFBMkI7YUFDdEM7Ozs7WUFWZ0Msa0JBQWtCLHVCQW9COUMsSUFBSTtZQTVCUCxVQUFVO1lBRlYsaUJBQWlCOzs7a0NBc0JoQixLQUFLO3VCQUdMLE1BQU07Ozs7SUFIUCxrREFDc0M7O0lBRXRDLHVDQUFpRDs7SUFFakQsK0NBQXlCOzs7OztJQUd2QixpREFBc0Q7Ozs7O0lBQ3RELG1DQUF5Qzs7Ozs7SUFDekMsb0NBQWdDOzs7Ozs7QUFvQ3BDLFNBQVMsUUFBUSxDQUFDLFFBQWtCO0lBQ2xDLElBQUksS0FBSyxDQUFDLE9BQU8sQ0FBQyxRQUFRLENBQUMsRUFBRTtRQUMzQixRQUFRLENBQUMsT0FBTzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFO1lBQ3ZCLFFBQVEsQ0FBQyxtQkFBQSxLQUFLLENBQUMsUUFBUSxFQUFrQyxDQUFDLENBQUM7UUFDN0QsQ0FBQyxFQUFDLENBQUM7UUFDSCxPQUFPO0tBQ1I7SUFFRCxNQUFNLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDLE9BQU87Ozs7SUFBQyxHQUFHLENBQUMsRUFBRTtRQUNsQyxRQUFRLENBQUMsR0FBRyxDQUFDLENBQUMsV0FBVyxFQUFFLENBQUM7UUFDNUIsUUFBUSxDQUFDLEdBQUcsQ0FBQyxDQUFDLHNCQUFzQixFQUFFLENBQUM7SUFDekMsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBDaGFuZ2VEZXRlY3RvclJlZixcclxuICBEaXJlY3RpdmUsXHJcbiAgRWxlbWVudFJlZixcclxuICBFdmVudEVtaXR0ZXIsXHJcbiAgSW5wdXQsXHJcbiAgT25EZXN0cm95LFxyXG4gIE9uSW5pdCxcclxuICBPdXRwdXQsXHJcbiAgU2VsZixcclxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgRm9ybUNvbnRyb2wsIEZvcm1Hcm91cCwgRm9ybUdyb3VwRGlyZWN0aXZlIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xyXG5pbXBvcnQgeyBmcm9tRXZlbnQgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaWx0ZXIgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICcuLi91dGlscyc7XHJcblxyXG50eXBlIENvbnRyb2xzID0geyBba2V5OiBzdHJpbmddOiBGb3JtQ29udHJvbCB9IHwgRm9ybUdyb3VwW107XHJcblxyXG5ARGlyZWN0aXZlKHtcclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGRpcmVjdGl2ZS1zZWxlY3RvclxyXG4gIHNlbGVjdG9yOiAnZm9ybVtuZ1N1Ym1pdF1bZm9ybUdyb3VwXScsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBGb3JtU3VibWl0RGlyZWN0aXZlIGltcGxlbWVudHMgT25Jbml0LCBPbkRlc3Ryb3kge1xyXG4gIEBJbnB1dCgpXHJcbiAgbm90VmFsaWRhdGVPblN1Ym1pdDogc3RyaW5nIHwgYm9vbGVhbjtcclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IG5nU3VibWl0ID0gbmV3IEV2ZW50RW1pdHRlcigpO1xyXG5cclxuICBleGVjdXRlZE5nU3VibWl0ID0gZmFsc2U7XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgQFNlbGYoKSBwcml2YXRlIGZvcm1Hcm91cERpcmVjdGl2ZTogRm9ybUdyb3VwRGlyZWN0aXZlLFxyXG4gICAgcHJpdmF0ZSBob3N0OiBFbGVtZW50UmVmPEhUTUxGb3JtRWxlbWVudD4sXHJcbiAgICBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZixcclxuICApIHt9XHJcblxyXG4gIG5nT25Jbml0KCkge1xyXG4gICAgdGhpcy5mb3JtR3JvdXBEaXJlY3RpdmUubmdTdWJtaXQucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKS5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICB0aGlzLm1hcmtBc0RpcnR5KCk7XHJcbiAgICAgIHRoaXMuZXhlY3V0ZWROZ1N1Ym1pdCA9IHRydWU7XHJcbiAgICB9KTtcclxuXHJcbiAgICBmcm9tRXZlbnQodGhpcy5ob3N0Lm5hdGl2ZUVsZW1lbnQgYXMgSFRNTEVsZW1lbnQsICdrZXl1cCcpXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIGRlYm91bmNlVGltZSgyMDApLFxyXG4gICAgICAgIGZpbHRlcigoa2V5OiBLZXlib2FyZEV2ZW50KSA9PiBrZXkgJiYga2V5LmtleSA9PT0gJ0VudGVyJyksXHJcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcclxuICAgICAgICBpZiAoIXRoaXMuZXhlY3V0ZWROZ1N1Ym1pdCkge1xyXG4gICAgICAgICAgdGhpcy5ob3N0Lm5hdGl2ZUVsZW1lbnQuZGlzcGF0Y2hFdmVudChuZXcgRXZlbnQoJ3N1Ym1pdCcsIHsgYnViYmxlczogdHJ1ZSwgY2FuY2VsYWJsZTogdHJ1ZSB9KSk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLmV4ZWN1dGVkTmdTdWJtaXQgPSBmYWxzZTtcclxuICAgICAgfSk7XHJcbiAgfVxyXG5cclxuICBuZ09uRGVzdHJveSgpOiB2b2lkIHt9XHJcblxyXG4gIG1hcmtBc0RpcnR5KCkge1xyXG4gICAgY29uc3QgeyBmb3JtIH0gPSB0aGlzLmZvcm1Hcm91cERpcmVjdGl2ZTtcclxuXHJcbiAgICBzZXREaXJ0eShmb3JtLmNvbnRyb2xzIGFzIHsgW2tleTogc3RyaW5nXTogRm9ybUNvbnRyb2wgfSk7XHJcbiAgICBmb3JtLm1hcmtBc0RpcnR5KCk7XHJcblxyXG4gICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XHJcbiAgfVxyXG59XHJcblxyXG5mdW5jdGlvbiBzZXREaXJ0eShjb250cm9sczogQ29udHJvbHMpIHtcclxuICBpZiAoQXJyYXkuaXNBcnJheShjb250cm9scykpIHtcclxuICAgIGNvbnRyb2xzLmZvckVhY2goZ3JvdXAgPT4ge1xyXG4gICAgICBzZXREaXJ0eShncm91cC5jb250cm9scyBhcyB7IFtrZXk6IHN0cmluZ106IEZvcm1Db250cm9sIH0pO1xyXG4gICAgfSk7XHJcbiAgICByZXR1cm47XHJcbiAgfVxyXG5cclxuICBPYmplY3Qua2V5cyhjb250cm9scykuZm9yRWFjaChrZXkgPT4ge1xyXG4gICAgY29udHJvbHNba2V5XS5tYXJrQXNEaXJ0eSgpO1xyXG4gICAgY29udHJvbHNba2V5XS51cGRhdGVWYWx1ZUFuZFZhbGlkaXR5KCk7XHJcbiAgfSk7XHJcbn1cclxuIl19