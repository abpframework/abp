/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, EventEmitter, Output, Renderer2 } from '@angular/core';
import { fromEvent } from 'rxjs';
import { takeUntilDestroy } from '@ngx-validate/core';
export class ClickEventStopPropagationDirective {
    /**
     * @param {?} renderer
     * @param {?} el
     */
    constructor(renderer, el) {
        this.renderer = renderer;
        this.el = el;
        this.stopPropEvent = new EventEmitter();
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        fromEvent(this.el.nativeElement, 'click')
            .pipe(takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        (event) => {
            event.stopPropagation();
            this.stopPropEvent.emit(event);
        }));
    }
}
ClickEventStopPropagationDirective.decorators = [
    { type: Directive, args: [{
                // tslint:disable-next-line: directive-selector
                selector: '[click.stop]'
            },] }
];
/** @nocollapse */
ClickEventStopPropagationDirective.ctorParameters = () => [
    { type: Renderer2 },
    { type: ElementRef }
];
ClickEventStopPropagationDirective.propDecorators = {
    stopPropEvent: [{ type: Output, args: ['click.stop',] }]
};
if (false) {
    /** @type {?} */
    ClickEventStopPropagationDirective.prototype.stopPropEvent;
    /**
     * @type {?}
     * @private
     */
    ClickEventStopPropagationDirective.prototype.renderer;
    /**
     * @type {?}
     * @private
     */
    ClickEventStopPropagationDirective.prototype.el;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic3RvcC1wcm9wYWdhdGlvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9zdG9wLXByb3BhZ2F0aW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsWUFBWSxFQUFVLE1BQU0sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDL0YsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqQyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQU10RCxNQUFNLE9BQU8sa0NBQWtDOzs7OztJQUc3QyxZQUFvQixRQUFtQixFQUFVLEVBQWM7UUFBM0MsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQUFVLE9BQUUsR0FBRixFQUFFLENBQVk7UUFGaEMsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDO0lBRVosQ0FBQzs7OztJQUVuRSxRQUFRO1FBQ04sU0FBUyxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsYUFBYSxFQUFFLE9BQU8sQ0FBQzthQUN0QyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQUM7YUFDNUIsU0FBUzs7OztRQUFDLENBQUMsS0FBaUIsRUFBRSxFQUFFO1lBQy9CLEtBQUssQ0FBQyxlQUFlLEVBQUUsQ0FBQztZQUN4QixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7OztZQWhCRixTQUFTLFNBQUM7O2dCQUVULFFBQVEsRUFBRSxjQUFjO2FBQ3pCOzs7O1lBUDZELFNBQVM7WUFBbkQsVUFBVTs7OzRCQVMzQixNQUFNLFNBQUMsWUFBWTs7OztJQUFwQiwyREFBOEU7Ozs7O0lBRWxFLHNEQUEyQjs7Ozs7SUFBRSxnREFBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIEVsZW1lbnRSZWYsIEV2ZW50RW1pdHRlciwgT25Jbml0LCBPdXRwdXQsIFJlbmRlcmVyMiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcblxuQERpcmVjdGl2ZSh7XG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogZGlyZWN0aXZlLXNlbGVjdG9yXG4gIHNlbGVjdG9yOiAnW2NsaWNrLnN0b3BdJ1xufSlcbmV4cG9ydCBjbGFzcyBDbGlja0V2ZW50U3RvcFByb3BhZ2F0aW9uRGlyZWN0aXZlIGltcGxlbWVudHMgT25Jbml0IHtcbiAgQE91dHB1dCgnY2xpY2suc3RvcCcpIHJlYWRvbmx5IHN0b3BQcm9wRXZlbnQgPSBuZXcgRXZlbnRFbWl0dGVyPE1vdXNlRXZlbnQ+KCk7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyLCBwcml2YXRlIGVsOiBFbGVtZW50UmVmKSB7fVxuXG4gIG5nT25Jbml0KCk6IHZvaWQge1xuICAgIGZyb21FdmVudCh0aGlzLmVsLm5hdGl2ZUVsZW1lbnQsICdjbGljaycpXG4gICAgICAucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKVxuICAgICAgLnN1YnNjcmliZSgoZXZlbnQ6IE1vdXNlRXZlbnQpID0+IHtcbiAgICAgICAgZXZlbnQuc3RvcFByb3BhZ2F0aW9uKCk7XG4gICAgICAgIHRoaXMuc3RvcFByb3BFdmVudC5lbWl0KGV2ZW50KTtcbiAgICAgIH0pO1xuICB9XG59XG4iXX0=