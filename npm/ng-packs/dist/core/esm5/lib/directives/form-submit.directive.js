/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Directive, ElementRef, EventEmitter, Input, Output, Self } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';
import { fromEvent } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { takeUntilDestroy } from '../utils';
var FormSubmitDirective = /** @class */ (function () {
    function FormSubmitDirective(formGroupDirective, host, cdRef) {
        this.formGroupDirective = formGroupDirective;
        this.host = host;
        this.cdRef = cdRef;
        this.ngSubmit = new EventEmitter();
        this.executedNgSubmit = false;
    }
    /**
     * @return {?}
     */
    FormSubmitDirective.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.formGroupDirective.ngSubmit.pipe(takeUntilDestroy(this)).subscribe((/**
         * @return {?}
         */
        function () {
            _this.markAsDirty();
            _this.executedNgSubmit = true;
        }));
        fromEvent((/** @type {?} */ (this.host.nativeElement)), 'keyup')
            .pipe(debounceTime(200), filter((/**
         * @param {?} key
         * @return {?}
         */
        function (key) { return key && key.key === 'Enter'; })), takeUntilDestroy(this))
            .subscribe((/**
         * @return {?}
         */
        function () {
            if (!_this.executedNgSubmit) {
                _this.host.nativeElement.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
            }
            _this.executedNgSubmit = false;
        }));
        fromEvent(this.host.nativeElement, 'submit')
            .pipe(takeUntilDestroy(this), filter((/**
         * @return {?}
         */
        function () { return !_this.notValidateOnSubmit && typeof _this.notValidateOnSubmit !== 'string'; })))
            .subscribe((/**
         * @return {?}
         */
        function () {
            if (!_this.executedNgSubmit) {
                _this.markAsDirty();
            }
        }));
    };
    /**
     * @return {?}
     */
    FormSubmitDirective.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    /**
     * @return {?}
     */
    FormSubmitDirective.prototype.markAsDirty = /**
     * @return {?}
     */
    function () {
        var form = this.formGroupDirective.form;
        setDirty((/** @type {?} */ (form.controls)));
        form.markAsDirty();
        this.cdRef.detectChanges();
    };
    FormSubmitDirective.decorators = [
        { type: Directive, args: [{
                    // tslint:disable-next-line: directive-selector
                    selector: 'form[ngSubmit][formGroup]'
                },] }
    ];
    /** @nocollapse */
    FormSubmitDirective.ctorParameters = function () { return [
        { type: FormGroupDirective, decorators: [{ type: Self }] },
        { type: ElementRef },
        { type: ChangeDetectorRef }
    ]; };
    FormSubmitDirective.propDecorators = {
        notValidateOnSubmit: [{ type: Input }],
        ngSubmit: [{ type: Output }]
    };
    return FormSubmitDirective;
}());
export { FormSubmitDirective };
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
        function (group) {
            setDirty((/** @type {?} */ (group.controls)));
        }));
        return;
    }
    Object.keys(controls).forEach((/**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        controls[key].markAsDirty();
        controls[key].updateValueAndValidity();
    }));
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9ybS1zdWJtaXQuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZm9ybS1zdWJtaXQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQ0wsaUJBQWlCLEVBQ2pCLFNBQVMsRUFDVCxVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sSUFBSSxFQUNMLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBMEIsa0JBQWtCLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUM1RSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2pDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdEQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBSTVDO0lBWUUsNkJBQ2tCLGtCQUFzQyxFQUM5QyxJQUFpQyxFQUNqQyxLQUF3QjtRQUZoQix1QkFBa0IsR0FBbEIsa0JBQWtCLENBQW9CO1FBQzlDLFNBQUksR0FBSixJQUFJLENBQTZCO1FBQ2pDLFVBQUssR0FBTCxLQUFLLENBQW1CO1FBUGYsYUFBUSxHQUFHLElBQUksWUFBWSxFQUFFLENBQUM7UUFFakQscUJBQWdCLEdBQUcsS0FBSyxDQUFDO0lBTXRCLENBQUM7Ozs7SUFFSixzQ0FBUTs7O0lBQVI7UUFBQSxpQkE4QkM7UUE3QkMsSUFBSSxDQUFDLGtCQUFrQixDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxTQUFTOzs7UUFBQztZQUN0RSxLQUFJLENBQUMsV0FBVyxFQUFFLENBQUM7WUFDbkIsS0FBSSxDQUFDLGdCQUFnQixHQUFHLElBQUksQ0FBQztRQUMvQixDQUFDLEVBQUMsQ0FBQztRQUVILFNBQVMsQ0FBQyxtQkFBQSxJQUFJLENBQUMsSUFBSSxDQUFDLGFBQWEsRUFBZSxFQUFFLE9BQU8sQ0FBQzthQUN2RCxJQUFJLENBQ0gsWUFBWSxDQUFDLEdBQUcsQ0FBQyxFQUNqQixNQUFNOzs7O1FBQUMsVUFBQyxHQUFrQixJQUFLLE9BQUEsR0FBRyxJQUFJLEdBQUcsQ0FBQyxHQUFHLEtBQUssT0FBTyxFQUExQixDQUEwQixFQUFDLEVBQzFELGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUN2QjthQUNBLFNBQVM7OztRQUFDO1lBQ1QsSUFBSSxDQUFDLEtBQUksQ0FBQyxnQkFBZ0IsRUFBRTtnQkFDMUIsS0FBSSxDQUFDLElBQUksQ0FBQyxhQUFhLENBQUMsYUFBYSxDQUFDLElBQUksS0FBSyxDQUFDLFFBQVEsRUFBRSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxDQUFDLENBQUMsQ0FBQzthQUNqRztZQUVELEtBQUksQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7UUFDaEMsQ0FBQyxFQUFDLENBQUM7UUFFTCxTQUFTLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxhQUFhLEVBQUUsUUFBUSxDQUFDO2FBQ3pDLElBQUksQ0FDSCxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsRUFDdEIsTUFBTTs7O1FBQUMsY0FBTSxPQUFBLENBQUMsS0FBSSxDQUFDLG1CQUFtQixJQUFJLE9BQU8sS0FBSSxDQUFDLG1CQUFtQixLQUFLLFFBQVEsRUFBekUsQ0FBeUUsRUFBQyxDQUN4RjthQUNBLFNBQVM7OztRQUFDO1lBQ1QsSUFBSSxDQUFDLEtBQUksQ0FBQyxnQkFBZ0IsRUFBRTtnQkFDMUIsS0FBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO2FBQ3BCO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQseUNBQVc7OztJQUFYLGNBQXFCLENBQUM7Ozs7SUFFdEIseUNBQVc7OztJQUFYO1FBQ1UsSUFBQSxtQ0FBSTtRQUVaLFFBQVEsQ0FBQyxtQkFBQSxJQUFJLENBQUMsUUFBUSxFQUFrQyxDQUFDLENBQUM7UUFDMUQsSUFBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO1FBRW5CLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLENBQUM7SUFDN0IsQ0FBQzs7Z0JBM0RGLFNBQVMsU0FBQzs7b0JBRVQsUUFBUSxFQUFFLDJCQUEyQjtpQkFDdEM7Ozs7Z0JBVmdDLGtCQUFrQix1QkFvQjlDLElBQUk7Z0JBNUJQLFVBQVU7Z0JBRlYsaUJBQWlCOzs7c0NBc0JoQixLQUFLOzJCQUdMLE1BQU07O0lBb0RULDBCQUFDO0NBQUEsQUE1REQsSUE0REM7U0F4RFksbUJBQW1COzs7SUFDOUIsa0RBQ3NDOztJQUV0Qyx1Q0FBaUQ7O0lBRWpELCtDQUF5Qjs7Ozs7SUFHdkIsaURBQXNEOzs7OztJQUN0RCxtQ0FBeUM7Ozs7O0lBQ3pDLG9DQUFnQzs7Ozs7O0FBK0NwQyxTQUFTLFFBQVEsQ0FBQyxRQUFrQjtJQUNsQyxJQUFJLEtBQUssQ0FBQyxPQUFPLENBQUMsUUFBUSxDQUFDLEVBQUU7UUFDM0IsUUFBUSxDQUFDLE9BQU87Ozs7UUFBQyxVQUFBLEtBQUs7WUFDcEIsUUFBUSxDQUFDLG1CQUFBLEtBQUssQ0FBQyxRQUFRLEVBQWtDLENBQUMsQ0FBQztRQUM3RCxDQUFDLEVBQUMsQ0FBQztRQUNILE9BQU87S0FDUjtJQUVELE1BQU0sQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsT0FBTzs7OztJQUFDLFVBQUEsR0FBRztRQUMvQixRQUFRLENBQUMsR0FBRyxDQUFDLENBQUMsV0FBVyxFQUFFLENBQUM7UUFDNUIsUUFBUSxDQUFDLEdBQUcsQ0FBQyxDQUFDLHNCQUFzQixFQUFFLENBQUM7SUFDekMsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBDaGFuZ2VEZXRlY3RvclJlZixcclxuICBEaXJlY3RpdmUsXHJcbiAgRWxlbWVudFJlZixcclxuICBFdmVudEVtaXR0ZXIsXHJcbiAgSW5wdXQsXHJcbiAgT25EZXN0cm95LFxyXG4gIE9uSW5pdCxcclxuICBPdXRwdXQsXHJcbiAgU2VsZlxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBGb3JtQ29udHJvbCwgRm9ybUdyb3VwLCBGb3JtR3JvdXBEaXJlY3RpdmUgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XHJcbmltcG9ydCB7IGZyb21FdmVudCB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbHRlciB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzJztcclxuXHJcbnR5cGUgQ29udHJvbHMgPSB7IFtrZXk6IHN0cmluZ106IEZvcm1Db250cm9sIH0gfCBGb3JtR3JvdXBbXTtcclxuXHJcbkBEaXJlY3RpdmUoe1xyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogZGlyZWN0aXZlLXNlbGVjdG9yXHJcbiAgc2VsZWN0b3I6ICdmb3JtW25nU3VibWl0XVtmb3JtR3JvdXBdJ1xyXG59KVxyXG5leHBvcnQgY2xhc3MgRm9ybVN1Ym1pdERpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uSW5pdCwgT25EZXN0cm95IHtcclxuICBASW5wdXQoKVxyXG4gIG5vdFZhbGlkYXRlT25TdWJtaXQ6IHN0cmluZyB8IGJvb2xlYW47XHJcblxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBuZ1N1Ym1pdCA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcclxuXHJcbiAgZXhlY3V0ZWROZ1N1Ym1pdCA9IGZhbHNlO1xyXG5cclxuICBjb25zdHJ1Y3RvcihcclxuICAgIEBTZWxmKCkgcHJpdmF0ZSBmb3JtR3JvdXBEaXJlY3RpdmU6IEZvcm1Hcm91cERpcmVjdGl2ZSxcclxuICAgIHByaXZhdGUgaG9zdDogRWxlbWVudFJlZjxIVE1MRm9ybUVsZW1lbnQ+LFxyXG4gICAgcHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWZcclxuICApIHt9XHJcblxyXG4gIG5nT25Jbml0KCkge1xyXG4gICAgdGhpcy5mb3JtR3JvdXBEaXJlY3RpdmUubmdTdWJtaXQucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKS5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICB0aGlzLm1hcmtBc0RpcnR5KCk7XHJcbiAgICAgIHRoaXMuZXhlY3V0ZWROZ1N1Ym1pdCA9IHRydWU7XHJcbiAgICB9KTtcclxuXHJcbiAgICBmcm9tRXZlbnQodGhpcy5ob3N0Lm5hdGl2ZUVsZW1lbnQgYXMgSFRNTEVsZW1lbnQsICdrZXl1cCcpXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIGRlYm91bmNlVGltZSgyMDApLFxyXG4gICAgICAgIGZpbHRlcigoa2V5OiBLZXlib2FyZEV2ZW50KSA9PiBrZXkgJiYga2V5LmtleSA9PT0gJ0VudGVyJyksXHJcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKVxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICAgIGlmICghdGhpcy5leGVjdXRlZE5nU3VibWl0KSB7XHJcbiAgICAgICAgICB0aGlzLmhvc3QubmF0aXZlRWxlbWVudC5kaXNwYXRjaEV2ZW50KG5ldyBFdmVudCgnc3VibWl0JywgeyBidWJibGVzOiB0cnVlLCBjYW5jZWxhYmxlOiB0cnVlIH0pKTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHRoaXMuZXhlY3V0ZWROZ1N1Ym1pdCA9IGZhbHNlO1xyXG4gICAgICB9KTtcclxuXHJcbiAgICBmcm9tRXZlbnQodGhpcy5ob3N0Lm5hdGl2ZUVsZW1lbnQsICdzdWJtaXQnKVxyXG4gICAgICAucGlwZShcclxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxyXG4gICAgICAgIGZpbHRlcigoKSA9PiAhdGhpcy5ub3RWYWxpZGF0ZU9uU3VibWl0ICYmIHR5cGVvZiB0aGlzLm5vdFZhbGlkYXRlT25TdWJtaXQgIT09ICdzdHJpbmcnKVxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICAgIGlmICghdGhpcy5leGVjdXRlZE5nU3VibWl0KSB7XHJcbiAgICAgICAgICB0aGlzLm1hcmtBc0RpcnR5KCk7XHJcbiAgICAgICAgfVxyXG4gICAgICB9KTtcclxuICB9XHJcblxyXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge31cclxuXHJcbiAgbWFya0FzRGlydHkoKSB7XHJcbiAgICBjb25zdCB7IGZvcm0gfSA9IHRoaXMuZm9ybUdyb3VwRGlyZWN0aXZlO1xyXG5cclxuICAgIHNldERpcnR5KGZvcm0uY29udHJvbHMgYXMgeyBba2V5OiBzdHJpbmddOiBGb3JtQ29udHJvbCB9KTtcclxuICAgIGZvcm0ubWFya0FzRGlydHkoKTtcclxuXHJcbiAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcclxuICB9XHJcbn1cclxuXHJcbmZ1bmN0aW9uIHNldERpcnR5KGNvbnRyb2xzOiBDb250cm9scykge1xyXG4gIGlmIChBcnJheS5pc0FycmF5KGNvbnRyb2xzKSkge1xyXG4gICAgY29udHJvbHMuZm9yRWFjaChncm91cCA9PiB7XHJcbiAgICAgIHNldERpcnR5KGdyb3VwLmNvbnRyb2xzIGFzIHsgW2tleTogc3RyaW5nXTogRm9ybUNvbnRyb2wgfSk7XHJcbiAgICB9KTtcclxuICAgIHJldHVybjtcclxuICB9XHJcblxyXG4gIE9iamVjdC5rZXlzKGNvbnRyb2xzKS5mb3JFYWNoKGtleSA9PiB7XHJcbiAgICBjb250cm9sc1trZXldLm1hcmtBc0RpcnR5KCk7XHJcbiAgICBjb250cm9sc1trZXldLnVwZGF0ZVZhbHVlQW5kVmFsaWRpdHkoKTtcclxuICB9KTtcclxufVxyXG4iXX0=