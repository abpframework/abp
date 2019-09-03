/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                selector: '[input.debounce]',
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGVib3VuY2UuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZGVib3VuY2UuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFVLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDdEcsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqQyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDOUMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFLdEQsTUFBTSxPQUFPLDJCQUEyQjs7Ozs7SUFLdEMsWUFBb0IsUUFBbUIsRUFBVSxFQUFjO1FBQTNDLGFBQVEsR0FBUixRQUFRLENBQVc7UUFBVSxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBSnRELGFBQVEsR0FBVyxHQUFHLENBQUM7UUFFTixrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFTLENBQUM7SUFFRixDQUFDOzs7O0lBRW5FLFFBQVE7UUFDTixTQUFTLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxhQUFhLEVBQUUsT0FBTyxDQUFDO2FBQ3RDLElBQUksQ0FDSCxZQUFZLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUMzQixnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FDdkI7YUFDQSxTQUFTOzs7O1FBQUMsQ0FBQyxLQUFZLEVBQUUsRUFBRTtZQUMxQixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7OztZQW5CRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjthQUM3Qjs7OztZQVAyQixTQUFTO1lBQUUsVUFBVTs7O3VCQVM5QyxLQUFLOzRCQUVMLE1BQU0sU0FBQyxnQkFBZ0I7Ozs7SUFGeEIsK0NBQWdDOztJQUVoQyxvREFBb0U7Ozs7O0lBRXhELCtDQUEyQjs7Ozs7SUFBRSx5Q0FBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIE91dHB1dCwgUmVuZGVyZXIyLCBFbGVtZW50UmVmLCBPbkluaXQsIEV2ZW50RW1pdHRlciwgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IGZyb21FdmVudCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZGVib3VuY2VUaW1lIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1tpbnB1dC5kZWJvdW5jZV0nLFxufSlcbmV4cG9ydCBjbGFzcyBJbnB1dEV2ZW50RGVib3VuY2VEaXJlY3RpdmUgaW1wbGVtZW50cyBPbkluaXQge1xuICBASW5wdXQoKSBkZWJvdW5jZTogbnVtYmVyID0gMzAwO1xuXG4gIEBPdXRwdXQoJ2lucHV0LmRlYm91bmNlJykgZGVib3VuY2VFdmVudCA9IG5ldyBFdmVudEVtaXR0ZXI8RXZlbnQ+KCk7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyLCBwcml2YXRlIGVsOiBFbGVtZW50UmVmKSB7fVxuXG4gIG5nT25Jbml0KCk6IHZvaWQge1xuICAgIGZyb21FdmVudCh0aGlzLmVsLm5hdGl2ZUVsZW1lbnQsICdpbnB1dCcpXG4gICAgICAucGlwZShcbiAgICAgICAgZGVib3VuY2VUaW1lKHRoaXMuZGVib3VuY2UpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoZXZlbnQ6IEV2ZW50KSA9PiB7XG4gICAgICAgIHRoaXMuZGVib3VuY2VFdmVudC5lbWl0KGV2ZW50KTtcbiAgICAgIH0pO1xuICB9XG59XG4iXX0=