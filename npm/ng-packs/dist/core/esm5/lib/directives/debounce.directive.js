/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Output, Renderer2, ElementRef, EventEmitter, Input } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { takeUntilDestroy } from '@ngx-validate/core';
var InputEventDebounceDirective = /** @class */ (function () {
    function InputEventDebounceDirective(renderer, el) {
        this.renderer = renderer;
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
    InputEventDebounceDirective.decorators = [
        { type: Directive, args: [{
                    // tslint:disable-next-line: directive-selector
                    selector: '[input.debounce]'
                },] }
    ];
    /** @nocollapse */
    InputEventDebounceDirective.ctorParameters = function () { return [
        { type: Renderer2 },
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
    InputEventDebounceDirective.prototype.renderer;
    /**
     * @type {?}
     * @private
     */
    InputEventDebounceDirective.prototype.el;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGVib3VuY2UuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZGVib3VuY2UuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFVLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDdEcsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqQyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDOUMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFFdEQ7SUFTRSxxQ0FBb0IsUUFBbUIsRUFBVSxFQUFjO1FBQTNDLGFBQVEsR0FBUixRQUFRLENBQVc7UUFBVSxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBSnRELGFBQVEsR0FBRyxHQUFHLENBQUM7UUFFVyxrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFTLENBQUM7SUFFWCxDQUFDOzs7O0lBRW5FLDhDQUFROzs7SUFBUjtRQUFBLGlCQVNDO1FBUkMsU0FBUyxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsYUFBYSxFQUFFLE9BQU8sQ0FBQzthQUN0QyxJQUFJLENBQ0gsWUFBWSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsRUFDM0IsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQ3ZCO2FBQ0EsU0FBUzs7OztRQUFDLFVBQUMsS0FBWTtZQUN0QixLQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7O2dCQXBCRixTQUFTLFNBQUM7O29CQUVULFFBQVEsRUFBRSxrQkFBa0I7aUJBQzdCOzs7O2dCQVIyQixTQUFTO2dCQUFFLFVBQVU7OzsyQkFVOUMsS0FBSztnQ0FFTCxNQUFNLFNBQUMsZ0JBQWdCOztJQWMxQixrQ0FBQztDQUFBLEFBckJELElBcUJDO1NBakJZLDJCQUEyQjs7O0lBQ3RDLCtDQUF3Qjs7SUFFeEIsb0RBQTZFOzs7OztJQUVqRSwrQ0FBMkI7Ozs7O0lBQUUseUNBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBPdXRwdXQsIFJlbmRlcmVyMiwgRWxlbWVudFJlZiwgT25Jbml0LCBFdmVudEVtaXR0ZXIsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBmcm9tRXZlbnQgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGRlYm91bmNlVGltZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuXG5ARGlyZWN0aXZlKHtcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBkaXJlY3RpdmUtc2VsZWN0b3JcbiAgc2VsZWN0b3I6ICdbaW5wdXQuZGVib3VuY2VdJ1xufSlcbmV4cG9ydCBjbGFzcyBJbnB1dEV2ZW50RGVib3VuY2VEaXJlY3RpdmUgaW1wbGVtZW50cyBPbkluaXQge1xuICBASW5wdXQoKSBkZWJvdW5jZSA9IDMwMDtcblxuICBAT3V0cHV0KCdpbnB1dC5kZWJvdW5jZScpIHJlYWRvbmx5IGRlYm91bmNlRXZlbnQgPSBuZXcgRXZlbnRFbWl0dGVyPEV2ZW50PigpO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMiwgcHJpdmF0ZSBlbDogRWxlbWVudFJlZikge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICBmcm9tRXZlbnQodGhpcy5lbC5uYXRpdmVFbGVtZW50LCAnaW5wdXQnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIGRlYm91bmNlVGltZSh0aGlzLmRlYm91bmNlKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKVxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoZXZlbnQ6IEV2ZW50KSA9PiB7XG4gICAgICAgIHRoaXMuZGVib3VuY2VFdmVudC5lbWl0KGV2ZW50KTtcbiAgICAgIH0pO1xuICB9XG59XG4iXX0=