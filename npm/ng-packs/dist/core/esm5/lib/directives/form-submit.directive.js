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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9ybS1zdWJtaXQuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZm9ybS1zdWJtaXQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQ0wsaUJBQWlCLEVBQ2pCLFNBQVMsRUFDVCxVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sSUFBSSxFQUNMLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBMEIsa0JBQWtCLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUM1RSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2pDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdEQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBSTVDO0lBWUUsNkJBQ2tCLGtCQUFzQyxFQUM5QyxJQUFpQyxFQUNqQyxLQUF3QjtRQUZoQix1QkFBa0IsR0FBbEIsa0JBQWtCLENBQW9CO1FBQzlDLFNBQUksR0FBSixJQUFJLENBQTZCO1FBQ2pDLFVBQUssR0FBTCxLQUFLLENBQW1CO1FBUGYsYUFBUSxHQUFHLElBQUksWUFBWSxFQUFFLENBQUM7UUFFakQscUJBQWdCLEdBQUcsS0FBSyxDQUFDO0lBTXRCLENBQUM7Ozs7SUFFSixzQ0FBUTs7O0lBQVI7UUFBQSxpQkE4QkM7UUE3QkMsSUFBSSxDQUFDLGtCQUFrQixDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxTQUFTOzs7UUFBQztZQUN0RSxLQUFJLENBQUMsV0FBVyxFQUFFLENBQUM7WUFDbkIsS0FBSSxDQUFDLGdCQUFnQixHQUFHLElBQUksQ0FBQztRQUMvQixDQUFDLEVBQUMsQ0FBQztRQUVILFNBQVMsQ0FBQyxtQkFBQSxJQUFJLENBQUMsSUFBSSxDQUFDLGFBQWEsRUFBZSxFQUFFLE9BQU8sQ0FBQzthQUN2RCxJQUFJLENBQ0gsWUFBWSxDQUFDLEdBQUcsQ0FBQyxFQUNqQixNQUFNOzs7O1FBQUMsVUFBQyxHQUFrQixJQUFLLE9BQUEsR0FBRyxJQUFJLEdBQUcsQ0FBQyxHQUFHLEtBQUssT0FBTyxFQUExQixDQUEwQixFQUFDLEVBQzFELGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUN2QjthQUNBLFNBQVM7OztRQUFDO1lBQ1QsSUFBSSxDQUFDLEtBQUksQ0FBQyxnQkFBZ0IsRUFBRTtnQkFDMUIsS0FBSSxDQUFDLElBQUksQ0FBQyxhQUFhLENBQUMsYUFBYSxDQUFDLElBQUksS0FBSyxDQUFDLFFBQVEsRUFBRSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxDQUFDLENBQUMsQ0FBQzthQUNqRztZQUVELEtBQUksQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7UUFDaEMsQ0FBQyxFQUFDLENBQUM7UUFFTCxTQUFTLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxhQUFhLEVBQUUsUUFBUSxDQUFDO2FBQ3pDLElBQUksQ0FDSCxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsRUFDdEIsTUFBTTs7O1FBQUMsY0FBTSxPQUFBLENBQUMsS0FBSSxDQUFDLG1CQUFtQixJQUFJLE9BQU8sS0FBSSxDQUFDLG1CQUFtQixLQUFLLFFBQVEsRUFBekUsQ0FBeUUsRUFBQyxDQUN4RjthQUNBLFNBQVM7OztRQUFDO1lBQ1QsSUFBSSxDQUFDLEtBQUksQ0FBQyxnQkFBZ0IsRUFBRTtnQkFDMUIsS0FBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO2FBQ3BCO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQseUNBQVc7OztJQUFYLGNBQXFCLENBQUM7Ozs7SUFFdEIseUNBQVc7OztJQUFYO1FBQ1UsSUFBQSxtQ0FBSTtRQUVaLFFBQVEsQ0FBQyxtQkFBQSxJQUFJLENBQUMsUUFBUSxFQUFrQyxDQUFDLENBQUM7UUFDMUQsSUFBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO1FBRW5CLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLENBQUM7SUFDN0IsQ0FBQzs7Z0JBM0RGLFNBQVMsU0FBQzs7b0JBRVQsUUFBUSxFQUFFLDJCQUEyQjtpQkFDdEM7Ozs7Z0JBVmdDLGtCQUFrQix1QkFvQjlDLElBQUk7Z0JBNUJQLFVBQVU7Z0JBRlYsaUJBQWlCOzs7c0NBc0JoQixLQUFLOzJCQUdMLE1BQU07O0lBb0RULDBCQUFDO0NBQUEsQUE1REQsSUE0REM7U0F4RFksbUJBQW1COzs7SUFDOUIsa0RBQ3NDOztJQUV0Qyx1Q0FBaUQ7O0lBRWpELCtDQUF5Qjs7Ozs7SUFHdkIsaURBQXNEOzs7OztJQUN0RCxtQ0FBeUM7Ozs7O0lBQ3pDLG9DQUFnQzs7Ozs7O0FBK0NwQyxTQUFTLFFBQVEsQ0FBQyxRQUFrQjtJQUNsQyxJQUFJLEtBQUssQ0FBQyxPQUFPLENBQUMsUUFBUSxDQUFDLEVBQUU7UUFDM0IsUUFBUSxDQUFDLE9BQU87Ozs7UUFBQyxVQUFBLEtBQUs7WUFDcEIsUUFBUSxDQUFDLG1CQUFBLEtBQUssQ0FBQyxRQUFRLEVBQWtDLENBQUMsQ0FBQztRQUM3RCxDQUFDLEVBQUMsQ0FBQztRQUNILE9BQU87S0FDUjtJQUVELE1BQU0sQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsT0FBTzs7OztJQUFDLFVBQUEsR0FBRztRQUMvQixRQUFRLENBQUMsR0FBRyxDQUFDLENBQUMsV0FBVyxFQUFFLENBQUM7UUFDNUIsUUFBUSxDQUFDLEdBQUcsQ0FBQyxDQUFDLHNCQUFzQixFQUFFLENBQUM7SUFDekMsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQ2hhbmdlRGV0ZWN0b3JSZWYsXG4gIERpcmVjdGl2ZSxcbiAgRWxlbWVudFJlZixcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25EZXN0cm95LFxuICBPbkluaXQsXG4gIE91dHB1dCxcbiAgU2VsZlxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEZvcm1Db250cm9sLCBGb3JtR3JvdXAsIEZvcm1Hcm91cERpcmVjdGl2ZSB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IGZyb21FdmVudCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaWx0ZXIgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnLi4vdXRpbHMnO1xuXG50eXBlIENvbnRyb2xzID0geyBba2V5OiBzdHJpbmddOiBGb3JtQ29udHJvbCB9IHwgRm9ybUdyb3VwW107XG5cbkBEaXJlY3RpdmUoe1xuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGRpcmVjdGl2ZS1zZWxlY3RvclxuICBzZWxlY3RvcjogJ2Zvcm1bbmdTdWJtaXRdW2Zvcm1Hcm91cF0nXG59KVxuZXhwb3J0IGNsYXNzIEZvcm1TdWJtaXREaXJlY3RpdmUgaW1wbGVtZW50cyBPbkluaXQsIE9uRGVzdHJveSB7XG4gIEBJbnB1dCgpXG4gIG5vdFZhbGlkYXRlT25TdWJtaXQ6IHN0cmluZyB8IGJvb2xlYW47XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IG5nU3VibWl0ID0gbmV3IEV2ZW50RW1pdHRlcigpO1xuXG4gIGV4ZWN1dGVkTmdTdWJtaXQgPSBmYWxzZTtcblxuICBjb25zdHJ1Y3RvcihcbiAgICBAU2VsZigpIHByaXZhdGUgZm9ybUdyb3VwRGlyZWN0aXZlOiBGb3JtR3JvdXBEaXJlY3RpdmUsXG4gICAgcHJpdmF0ZSBob3N0OiBFbGVtZW50UmVmPEhUTUxGb3JtRWxlbWVudD4sXG4gICAgcHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWZcbiAgKSB7fVxuXG4gIG5nT25Jbml0KCkge1xuICAgIHRoaXMuZm9ybUdyb3VwRGlyZWN0aXZlLm5nU3VibWl0LnBpcGUodGFrZVVudGlsRGVzdHJveSh0aGlzKSkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIHRoaXMubWFya0FzRGlydHkoKTtcbiAgICAgIHRoaXMuZXhlY3V0ZWROZ1N1Ym1pdCA9IHRydWU7XG4gICAgfSk7XG5cbiAgICBmcm9tRXZlbnQodGhpcy5ob3N0Lm5hdGl2ZUVsZW1lbnQgYXMgSFRNTEVsZW1lbnQsICdrZXl1cCcpXG4gICAgICAucGlwZShcbiAgICAgICAgZGVib3VuY2VUaW1lKDIwMCksXG4gICAgICAgIGZpbHRlcigoa2V5OiBLZXlib2FyZEV2ZW50KSA9PiBrZXkgJiYga2V5LmtleSA9PT0gJ0VudGVyJyksXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcylcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICBpZiAoIXRoaXMuZXhlY3V0ZWROZ1N1Ym1pdCkge1xuICAgICAgICAgIHRoaXMuaG9zdC5uYXRpdmVFbGVtZW50LmRpc3BhdGNoRXZlbnQobmV3IEV2ZW50KCdzdWJtaXQnLCB7IGJ1YmJsZXM6IHRydWUsIGNhbmNlbGFibGU6IHRydWUgfSkpO1xuICAgICAgICB9XG5cbiAgICAgICAgdGhpcy5leGVjdXRlZE5nU3VibWl0ID0gZmFsc2U7XG4gICAgICB9KTtcblxuICAgIGZyb21FdmVudCh0aGlzLmhvc3QubmF0aXZlRWxlbWVudCwgJ3N1Ym1pdCcpXG4gICAgICAucGlwZShcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgICAgZmlsdGVyKCgpID0+ICF0aGlzLm5vdFZhbGlkYXRlT25TdWJtaXQgJiYgdHlwZW9mIHRoaXMubm90VmFsaWRhdGVPblN1Ym1pdCAhPT0gJ3N0cmluZycpXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgaWYgKCF0aGlzLmV4ZWN1dGVkTmdTdWJtaXQpIHtcbiAgICAgICAgICB0aGlzLm1hcmtBc0RpcnR5KCk7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKTogdm9pZCB7fVxuXG4gIG1hcmtBc0RpcnR5KCkge1xuICAgIGNvbnN0IHsgZm9ybSB9ID0gdGhpcy5mb3JtR3JvdXBEaXJlY3RpdmU7XG5cbiAgICBzZXREaXJ0eShmb3JtLmNvbnRyb2xzIGFzIHsgW2tleTogc3RyaW5nXTogRm9ybUNvbnRyb2wgfSk7XG4gICAgZm9ybS5tYXJrQXNEaXJ0eSgpO1xuXG4gICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gIH1cbn1cblxuZnVuY3Rpb24gc2V0RGlydHkoY29udHJvbHM6IENvbnRyb2xzKSB7XG4gIGlmIChBcnJheS5pc0FycmF5KGNvbnRyb2xzKSkge1xuICAgIGNvbnRyb2xzLmZvckVhY2goZ3JvdXAgPT4ge1xuICAgICAgc2V0RGlydHkoZ3JvdXAuY29udHJvbHMgYXMgeyBba2V5OiBzdHJpbmddOiBGb3JtQ29udHJvbCB9KTtcbiAgICB9KTtcbiAgICByZXR1cm47XG4gIH1cblxuICBPYmplY3Qua2V5cyhjb250cm9scykuZm9yRWFjaChrZXkgPT4ge1xuICAgIGNvbnRyb2xzW2tleV0ubWFya0FzRGlydHkoKTtcbiAgICBjb250cm9sc1trZXldLnVwZGF0ZVZhbHVlQW5kVmFsaWRpdHkoKTtcbiAgfSk7XG59XG4iXX0=