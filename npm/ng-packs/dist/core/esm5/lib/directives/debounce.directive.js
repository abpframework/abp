/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/debounce.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, EventEmitter, Input, Output } from '@angular/core';
import { takeUntilDestroy } from '@ngx-validate/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
var InputEventDebounceDirective = /** @class */ (function () {
    function InputEventDebounceDirective(el) {
        this.el = el;
        this.debounce = 300;
        this.debounceEvent = new EventEmitter();
    }
    /**
     * @return {?}
     */
    InputEventDebounceDirective.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        fromEvent(this.el.nativeElement, 'input')
            .pipe(debounceTime(this.debounce), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            _this.debounceEvent.emit(event);
        }));
    };
    /**
     * @return {?}
     */
    InputEventDebounceDirective.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    InputEventDebounceDirective.decorators = [
        { type: Directive, args: [{
                    // tslint:disable-next-line: directive-selector
                    selector: '[input.debounce]',
                },] }
    ];
    /** @nocollapse */
    InputEventDebounceDirective.ctorParameters = function () { return [
        { type: ElementRef }
    ]; };
    InputEventDebounceDirective.propDecorators = {
        debounce: [{ type: Input }],
        debounceEvent: [{ type: Output, args: ['input.debounce',] }]
    };
    return InputEventDebounceDirective;
}());
export { InputEventDebounceDirective };
if (false) {
    /** @type {?} */
    InputEventDebounceDirective.prototype.debounce;
    /** @type {?} */
    InputEventDebounceDirective.prototype.debounceEvent;
    /**
     * @type {?}
     * @private
     */
    InputEventDebounceDirective.prototype.el;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGVib3VuY2UuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZGVib3VuY2UuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBcUIsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3RHLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDakMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRTlDO0lBU0UscUNBQW9CLEVBQWM7UUFBZCxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBSnpCLGFBQVEsR0FBRyxHQUFHLENBQUM7UUFFVyxrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFTLENBQUM7SUFFeEMsQ0FBQzs7OztJQUV0Qyw4Q0FBUTs7O0lBQVI7UUFBQSxpQkFTQztRQVJDLFNBQVMsQ0FBQyxJQUFJLENBQUMsRUFBRSxDQUFDLGFBQWEsRUFBRSxPQUFPLENBQUM7YUFDdEMsSUFBSSxDQUNILFlBQVksQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQzNCLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUN2QjthQUNBLFNBQVM7Ozs7UUFBQyxVQUFDLEtBQVk7WUFDdEIsS0FBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7UUFDakMsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsaURBQVc7OztJQUFYLGNBQXFCLENBQUM7O2dCQXRCdkIsU0FBUyxTQUFDOztvQkFFVCxRQUFRLEVBQUUsa0JBQWtCO2lCQUM3Qjs7OztnQkFSbUIsVUFBVTs7OzJCQVUzQixLQUFLO2dDQUVMLE1BQU0sU0FBQyxnQkFBZ0I7O0lBZ0IxQixrQ0FBQztDQUFBLEFBdkJELElBdUJDO1NBbkJZLDJCQUEyQjs7O0lBQ3RDLCtDQUF3Qjs7SUFFeEIsb0RBQTZFOzs7OztJQUVqRSx5Q0FBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIEVsZW1lbnRSZWYsIEV2ZW50RW1pdHRlciwgSW5wdXQsIE9uRGVzdHJveSwgT25Jbml0LCBPdXRwdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBEaXJlY3RpdmUoe1xuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGRpcmVjdGl2ZS1zZWxlY3RvclxuICBzZWxlY3RvcjogJ1tpbnB1dC5kZWJvdW5jZV0nLFxufSlcbmV4cG9ydCBjbGFzcyBJbnB1dEV2ZW50RGVib3VuY2VEaXJlY3RpdmUgaW1wbGVtZW50cyBPbkluaXQsIE9uRGVzdHJveSB7XG4gIEBJbnB1dCgpIGRlYm91bmNlID0gMzAwO1xuXG4gIEBPdXRwdXQoJ2lucHV0LmRlYm91bmNlJykgcmVhZG9ubHkgZGVib3VuY2VFdmVudCA9IG5ldyBFdmVudEVtaXR0ZXI8RXZlbnQ+KCk7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBlbDogRWxlbWVudFJlZikge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICBmcm9tRXZlbnQodGhpcy5lbC5uYXRpdmVFbGVtZW50LCAnaW5wdXQnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIGRlYm91bmNlVGltZSh0aGlzLmRlYm91bmNlKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKGV2ZW50OiBFdmVudCkgPT4ge1xuICAgICAgICB0aGlzLmRlYm91bmNlRXZlbnQuZW1pdChldmVudCk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge31cbn1cbiJdfQ==