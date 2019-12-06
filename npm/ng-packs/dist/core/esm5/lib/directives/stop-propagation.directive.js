/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/stop-propagation.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, EventEmitter, Output } from '@angular/core';
import { fromEvent } from 'rxjs';
import { takeUntilDestroy } from '@ngx-validate/core';
var ClickEventStopPropagationDirective = /** @class */ (function () {
    function ClickEventStopPropagationDirective(el) {
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
    /**
     * @return {?}
     */
    ClickEventStopPropagationDirective.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    ClickEventStopPropagationDirective.decorators = [
        { type: Directive, args: [{
                    // tslint:disable-next-line: directive-selector
                    selector: '[click.stop]',
                },] }
    ];
    /** @nocollapse */
    ClickEventStopPropagationDirective.ctorParameters = function () { return [
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
    ClickEventStopPropagationDirective.prototype.el;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic3RvcC1wcm9wYWdhdGlvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9zdG9wLXByb3BhZ2F0aW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLFlBQVksRUFBVSxNQUFNLEVBQXdCLE1BQU0sZUFBZSxDQUFDO0FBQzFHLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDakMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFFdEQ7SUFPRSw0Q0FBb0IsRUFBYztRQUFkLE9BQUUsR0FBRixFQUFFLENBQVk7UUFGSCxrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7SUFFekMsQ0FBQzs7OztJQUV0QyxxREFBUTs7O0lBQVI7UUFBQSxpQkFPQztRQU5DLFNBQVMsQ0FBQyxJQUFJLENBQUMsRUFBRSxDQUFDLGFBQWEsRUFBRSxPQUFPLENBQUM7YUFDdEMsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUFDO2FBQzVCLFNBQVM7Ozs7UUFBQyxVQUFDLEtBQWlCO1lBQzNCLEtBQUssQ0FBQyxlQUFlLEVBQUUsQ0FBQztZQUN4QixLQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCx3REFBVzs7O0lBQVgsY0FBcUIsQ0FBQzs7Z0JBbEJ2QixTQUFTLFNBQUM7O29CQUVULFFBQVEsRUFBRSxjQUFjO2lCQUN6Qjs7OztnQkFQbUIsVUFBVTs7O2dDQVMzQixNQUFNLFNBQUMsWUFBWTs7SUFjdEIseUNBQUM7Q0FBQSxBQW5CRCxJQW1CQztTQWZZLGtDQUFrQzs7O0lBQzdDLDJEQUE4RTs7Ozs7SUFFbEUsZ0RBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBFbGVtZW50UmVmLCBFdmVudEVtaXR0ZXIsIE9uSW5pdCwgT3V0cHV0LCBSZW5kZXJlcjIsIE9uRGVzdHJveSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcblxuQERpcmVjdGl2ZSh7XG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogZGlyZWN0aXZlLXNlbGVjdG9yXG4gIHNlbGVjdG9yOiAnW2NsaWNrLnN0b3BdJyxcbn0pXG5leHBvcnQgY2xhc3MgQ2xpY2tFdmVudFN0b3BQcm9wYWdhdGlvbkRpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uSW5pdCwgT25EZXN0cm95IHtcbiAgQE91dHB1dCgnY2xpY2suc3RvcCcpIHJlYWRvbmx5IHN0b3BQcm9wRXZlbnQgPSBuZXcgRXZlbnRFbWl0dGVyPE1vdXNlRXZlbnQ+KCk7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBlbDogRWxlbWVudFJlZikge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICBmcm9tRXZlbnQodGhpcy5lbC5uYXRpdmVFbGVtZW50LCAnY2xpY2snKVxuICAgICAgLnBpcGUodGFrZVVudGlsRGVzdHJveSh0aGlzKSlcbiAgICAgIC5zdWJzY3JpYmUoKGV2ZW50OiBNb3VzZUV2ZW50KSA9PiB7XG4gICAgICAgIGV2ZW50LnN0b3BQcm9wYWdhdGlvbigpO1xuICAgICAgICB0aGlzLnN0b3BQcm9wRXZlbnQuZW1pdChldmVudCk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge31cbn1cbiJdfQ==