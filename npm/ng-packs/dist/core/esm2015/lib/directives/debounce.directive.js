/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Output, Renderer2, ElementRef, EventEmitter, Input } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { takeUntilDestroy } from '@ngx-validate/core';
export class InputEventDebounceDirective {
    /**
     * @param {?} renderer
     * @param {?} el
     */
    constructor(renderer, el) {
        this.renderer = renderer;
        this.el = el;
        this.debounce = 300;
        this.debounceEvent = new EventEmitter();
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        fromEvent(this.el.nativeElement, 'input')
            .pipe(debounceTime(this.debounce), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        (event) => {
            this.debounceEvent.emit(event);
        }));
    }
}
InputEventDebounceDirective.decorators = [
    { type: Directive, args: [{
                // tslint:disable-next-line: directive-selector
                selector: '[input.debounce]'
            },] }
];
/** @nocollapse */
InputEventDebounceDirective.ctorParameters = () => [
    { type: Renderer2 },
    { type: ElementRef }
];
InputEventDebounceDirective.propDecorators = {
    debounce: [{ type: Input }],
    debounceEvent: [{ type: Output, args: ['input.debounce',] }]
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGVib3VuY2UuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZGVib3VuY2UuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFVLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDdEcsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqQyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDOUMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFNdEQsTUFBTSxPQUFPLDJCQUEyQjs7Ozs7SUFLdEMsWUFBb0IsUUFBbUIsRUFBVSxFQUFjO1FBQTNDLGFBQVEsR0FBUixRQUFRLENBQVc7UUFBVSxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBSnRELGFBQVEsR0FBRyxHQUFHLENBQUM7UUFFVyxrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFTLENBQUM7SUFFWCxDQUFDOzs7O0lBRW5FLFFBQVE7UUFDTixTQUFTLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxhQUFhLEVBQUUsT0FBTyxDQUFDO2FBQ3RDLElBQUksQ0FDSCxZQUFZLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUMzQixnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FDdkI7YUFDQSxTQUFTOzs7O1FBQUMsQ0FBQyxLQUFZLEVBQUUsRUFBRTtZQUMxQixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7OztZQXBCRixTQUFTLFNBQUM7O2dCQUVULFFBQVEsRUFBRSxrQkFBa0I7YUFDN0I7Ozs7WUFSMkIsU0FBUztZQUFFLFVBQVU7Ozt1QkFVOUMsS0FBSzs0QkFFTCxNQUFNLFNBQUMsZ0JBQWdCOzs7O0lBRnhCLCtDQUF3Qjs7SUFFeEIsb0RBQTZFOzs7OztJQUVqRSwrQ0FBMkI7Ozs7O0lBQUUseUNBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBPdXRwdXQsIFJlbmRlcmVyMiwgRWxlbWVudFJlZiwgT25Jbml0LCBFdmVudEVtaXR0ZXIsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IGZyb21FdmVudCB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xyXG5cclxuQERpcmVjdGl2ZSh7XHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBkaXJlY3RpdmUtc2VsZWN0b3JcclxuICBzZWxlY3RvcjogJ1tpbnB1dC5kZWJvdW5jZV0nXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBJbnB1dEV2ZW50RGVib3VuY2VEaXJlY3RpdmUgaW1wbGVtZW50cyBPbkluaXQge1xyXG4gIEBJbnB1dCgpIGRlYm91bmNlID0gMzAwO1xyXG5cclxuICBAT3V0cHV0KCdpbnB1dC5kZWJvdW5jZScpIHJlYWRvbmx5IGRlYm91bmNlRXZlbnQgPSBuZXcgRXZlbnRFbWl0dGVyPEV2ZW50PigpO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIsIHByaXZhdGUgZWw6IEVsZW1lbnRSZWYpIHt9XHJcblxyXG4gIG5nT25Jbml0KCk6IHZvaWQge1xyXG4gICAgZnJvbUV2ZW50KHRoaXMuZWwubmF0aXZlRWxlbWVudCwgJ2lucHV0JylcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgZGVib3VuY2VUaW1lKHRoaXMuZGVib3VuY2UpLFxyXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcylcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKChldmVudDogRXZlbnQpID0+IHtcclxuICAgICAgICB0aGlzLmRlYm91bmNlRXZlbnQuZW1pdChldmVudCk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=