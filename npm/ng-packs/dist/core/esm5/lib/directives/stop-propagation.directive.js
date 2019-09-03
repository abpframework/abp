/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, EventEmitter, Output, Renderer2 } from '@angular/core';
import { fromEvent } from 'rxjs';
import { takeUntilDestroy } from '@ngx-validate/core';
var ClickEventStopPropagationDirective = /** @class */ (function () {
    function ClickEventStopPropagationDirective(renderer, el) {
        this.renderer = renderer;
        this.el = el;
        this.stopPropEvent = new EventEmitter();
    }
    /**
     * @return {?}
     */
    ClickEventStopPropagationDirective.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        fromEvent(this.el.nativeElement, 'click')
            .pipe(takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            event.stopPropagation();
            _this.stopPropEvent.emit(event);
        }));
    };
    ClickEventStopPropagationDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[click.stop]',
                },] }
    ];
    /** @nocollapse */
    ClickEventStopPropagationDirective.ctorParameters = function () { return [
        { type: Renderer2 },
        { type: ElementRef }
    ]; };
    ClickEventStopPropagationDirective.propDecorators = {
        stopPropEvent: [{ type: Output, args: ['click.stop',] }]
    };
    return ClickEventStopPropagationDirective;
}());
export { ClickEventStopPropagationDirective };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic3RvcC1wcm9wYWdhdGlvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9zdG9wLXByb3BhZ2F0aW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsWUFBWSxFQUFVLE1BQU0sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDL0YsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqQyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUV0RDtJQU1FLDRDQUFvQixRQUFtQixFQUFVLEVBQWM7UUFBM0MsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQUFVLE9BQUUsR0FBRixFQUFFLENBQVk7UUFGekMsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDO0lBRUgsQ0FBQzs7OztJQUVuRSxxREFBUTs7O0lBQVI7UUFBQSxpQkFPQztRQU5DLFNBQVMsQ0FBQyxJQUFJLENBQUMsRUFBRSxDQUFDLGFBQWEsRUFBRSxPQUFPLENBQUM7YUFDdEMsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUFDO2FBQzVCLFNBQVM7Ozs7UUFBQyxVQUFDLEtBQWlCO1lBQzNCLEtBQUssQ0FBQyxlQUFlLEVBQUUsQ0FBQztZQUN4QixLQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7O2dCQWZGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsY0FBYztpQkFDekI7Ozs7Z0JBTjZELFNBQVM7Z0JBQW5ELFVBQVU7OztnQ0FRM0IsTUFBTSxTQUFDLFlBQVk7O0lBWXRCLHlDQUFDO0NBQUEsQUFoQkQsSUFnQkM7U0FiWSxrQ0FBa0M7OztJQUM3QywyREFBcUU7Ozs7O0lBRXpELHNEQUEyQjs7Ozs7SUFBRSxnREFBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIEVsZW1lbnRSZWYsIEV2ZW50RW1pdHRlciwgT25Jbml0LCBPdXRwdXQsIFJlbmRlcmVyMiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2NsaWNrLnN0b3BdJyxcbn0pXG5leHBvcnQgY2xhc3MgQ2xpY2tFdmVudFN0b3BQcm9wYWdhdGlvbkRpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIEBPdXRwdXQoJ2NsaWNrLnN0b3AnKSBzdG9wUHJvcEV2ZW50ID0gbmV3IEV2ZW50RW1pdHRlcjxNb3VzZUV2ZW50PigpO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMiwgcHJpdmF0ZSBlbDogRWxlbWVudFJlZikge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICBmcm9tRXZlbnQodGhpcy5lbC5uYXRpdmVFbGVtZW50LCAnY2xpY2snKVxuICAgICAgLnBpcGUodGFrZVVudGlsRGVzdHJveSh0aGlzKSlcbiAgICAgIC5zdWJzY3JpYmUoKGV2ZW50OiBNb3VzZUV2ZW50KSA9PiB7XG4gICAgICAgIGV2ZW50LnN0b3BQcm9wYWdhdGlvbigpO1xuICAgICAgICB0aGlzLnN0b3BQcm9wRXZlbnQuZW1pdChldmVudCk7XG4gICAgICB9KTtcbiAgfVxufVxuIl19