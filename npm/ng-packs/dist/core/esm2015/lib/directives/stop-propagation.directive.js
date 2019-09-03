/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                selector: '[click.stop]',
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic3RvcC1wcm9wYWdhdGlvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9zdG9wLXByb3BhZ2F0aW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsWUFBWSxFQUFVLE1BQU0sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDL0YsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqQyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUt0RCxNQUFNLE9BQU8sa0NBQWtDOzs7OztJQUc3QyxZQUFvQixRQUFtQixFQUFVLEVBQWM7UUFBM0MsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQUFVLE9BQUUsR0FBRixFQUFFLENBQVk7UUFGekMsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDO0lBRUgsQ0FBQzs7OztJQUVuRSxRQUFRO1FBQ04sU0FBUyxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsYUFBYSxFQUFFLE9BQU8sQ0FBQzthQUN0QyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQUM7YUFDNUIsU0FBUzs7OztRQUFDLENBQUMsS0FBaUIsRUFBRSxFQUFFO1lBQy9CLEtBQUssQ0FBQyxlQUFlLEVBQUUsQ0FBQztZQUN4QixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7OztZQWZGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsY0FBYzthQUN6Qjs7OztZQU42RCxTQUFTO1lBQW5ELFVBQVU7Ozs0QkFRM0IsTUFBTSxTQUFDLFlBQVk7Ozs7SUFBcEIsMkRBQXFFOzs7OztJQUV6RCxzREFBMkI7Ozs7O0lBQUUsZ0RBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBFbGVtZW50UmVmLCBFdmVudEVtaXR0ZXIsIE9uSW5pdCwgT3V0cHV0LCBSZW5kZXJlcjIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IGZyb21FdmVudCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1tjbGljay5zdG9wXScsXG59KVxuZXhwb3J0IGNsYXNzIENsaWNrRXZlbnRTdG9wUHJvcGFnYXRpb25EaXJlY3RpdmUgaW1wbGVtZW50cyBPbkluaXQge1xuICBAT3V0cHV0KCdjbGljay5zdG9wJykgc3RvcFByb3BFdmVudCA9IG5ldyBFdmVudEVtaXR0ZXI8TW91c2VFdmVudD4oKTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIsIHByaXZhdGUgZWw6IEVsZW1lbnRSZWYpIHt9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7XG4gICAgZnJvbUV2ZW50KHRoaXMuZWwubmF0aXZlRWxlbWVudCwgJ2NsaWNrJylcbiAgICAgIC5waXBlKHRha2VVbnRpbERlc3Ryb3kodGhpcykpXG4gICAgICAuc3Vic2NyaWJlKChldmVudDogTW91c2VFdmVudCkgPT4ge1xuICAgICAgICBldmVudC5zdG9wUHJvcGFnYXRpb24oKTtcbiAgICAgICAgdGhpcy5zdG9wUHJvcEV2ZW50LmVtaXQoZXZlbnQpO1xuICAgICAgfSk7XG4gIH1cbn1cbiJdfQ==