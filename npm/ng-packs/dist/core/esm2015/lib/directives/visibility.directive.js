/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, Optional, ElementRef, Renderer2 } from '@angular/core';
import { Subject } from 'rxjs';
import snq from 'snq';
export class VisibilityDirective {
    /**
     * @param {?} elRef
     * @param {?} renderer
     */
    constructor(elRef, renderer) {
        this.elRef = elRef;
        this.renderer = renderer;
        this.mutationObserverEnabled = true;
        this.completed$ = new Subject();
    }
    /**
     * @return {?}
     */
    ngAfterViewInit() {
        /** @type {?} */
        let observer;
        if (this.mutationObserverEnabled) {
            observer = new MutationObserver((/**
             * @param {?} mutations
             * @return {?}
             */
            mutations => {
                mutations.forEach((/**
                 * @param {?} mutation
                 * @return {?}
                 */
                mutation => {
                    if (!mutation.target)
                        return;
                    /** @type {?} */
                    const htmlNodes = snq((/**
                     * @return {?}
                     */
                    () => Array.from(mutation.target.childNodes).filter((/**
                     * @param {?} node
                     * @return {?}
                     */
                    node => node instanceof HTMLElement))), []);
                    if (!htmlNodes.length) {
                        this.removeFromDOM();
                        this.disconnect();
                    }
                    else {
                        setTimeout((/**
                         * @return {?}
                         */
                        () => {
                            this.disconnect();
                        }), 0);
                    }
                }));
            }));
            observer.observe(this.focusedElement, {
                childList: true,
            });
        }
        else {
            setTimeout((/**
             * @return {?}
             */
            () => {
                /** @type {?} */
                const htmlNodes = snq((/**
                 * @return {?}
                 */
                () => Array.from(this.focusedElement.childNodes).filter((/**
                 * @param {?} node
                 * @return {?}
                 */
                node => node instanceof HTMLElement))), []);
                if (!htmlNodes.length)
                    this.removeFromDOM();
            }), 0);
        }
        this.completed$.subscribe((/**
         * @return {?}
         */
        () => observer.disconnect()));
    }
    /**
     * @return {?}
     */
    disconnect() {
        this.completed$.next();
        this.completed$.complete();
    }
    /**
     * @return {?}
     */
    removeFromDOM() {
        this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
    }
}
VisibilityDirective.decorators = [
    { type: Directive, args: [{
                selector: '[abpVisibility]',
            },] }
];
/** @nocollapse */
VisibilityDirective.ctorParameters = () => [
    { type: ElementRef, decorators: [{ type: Optional }] },
    { type: Renderer2 }
];
VisibilityDirective.propDecorators = {
    focusedElement: [{ type: Input, args: ['abpVisibility',] }],
    mutationObserverEnabled: [{ type: Input }]
};
if (false) {
    /** @type {?} */
    VisibilityDirective.prototype.focusedElement;
    /** @type {?} */
    VisibilityDirective.prototype.mutationObserverEnabled;
    /** @type {?} */
    VisibilityDirective.prototype.completed$;
    /**
     * @type {?}
     * @private
     */
    VisibilityDirective.prototype.elRef;
    /**
     * @type {?}
     * @private
     */
    VisibilityDirective.prototype.renderer;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmlzaWJpbGl0eS5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy92aXNpYmlsaXR5LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBQ2pHLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDL0IsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBS3RCLE1BQU0sT0FBTyxtQkFBbUI7Ozs7O0lBUzlCLFlBQWdDLEtBQWlCLEVBQVUsUUFBbUI7UUFBOUMsVUFBSyxHQUFMLEtBQUssQ0FBWTtRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUFKOUUsNEJBQXVCLEdBQVksSUFBSSxDQUFDO1FBRXhDLGVBQVUsR0FBRyxJQUFJLE9BQU8sRUFBVyxDQUFDO0lBRTZDLENBQUM7Ozs7SUFFbEYsZUFBZTs7WUFDVCxRQUEwQjtRQUM5QixJQUFJLElBQUksQ0FBQyx1QkFBdUIsRUFBRTtZQUNoQyxRQUFRLEdBQUcsSUFBSSxnQkFBZ0I7Ozs7WUFBQyxTQUFTLENBQUMsRUFBRTtnQkFDMUMsU0FBUyxDQUFDLE9BQU87Ozs7Z0JBQUMsUUFBUSxDQUFDLEVBQUU7b0JBQzNCLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTTt3QkFBRSxPQUFPOzswQkFFdkIsU0FBUyxHQUFHLEdBQUc7OztvQkFDbkIsR0FBRyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU07Ozs7b0JBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLFlBQVksV0FBVyxFQUFDLEdBQ3hGLEVBQUUsQ0FDSDtvQkFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU0sRUFBRTt3QkFDckIsSUFBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO3dCQUNyQixJQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7cUJBQ25CO3lCQUFNO3dCQUNMLFVBQVU7Ozt3QkFBQyxHQUFHLEVBQUU7NEJBQ2QsSUFBSSxDQUFDLFVBQVUsRUFBRSxDQUFDO3dCQUNwQixDQUFDLEdBQUUsQ0FBQyxDQUFDLENBQUM7cUJBQ1A7Z0JBQ0gsQ0FBQyxFQUFDLENBQUM7WUFDTCxDQUFDLEVBQUMsQ0FBQztZQUVILFFBQVEsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLGNBQWMsRUFBRTtnQkFDcEMsU0FBUyxFQUFFLElBQUk7YUFDaEIsQ0FBQyxDQUFDO1NBQ0o7YUFBTTtZQUNMLFVBQVU7OztZQUFDLEdBQUcsRUFBRTs7c0JBQ1IsU0FBUyxHQUFHLEdBQUc7OztnQkFDbkIsR0FBRyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsY0FBYyxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU07Ozs7Z0JBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLFlBQVksV0FBVyxFQUFDLEdBQzVGLEVBQUUsQ0FDSDtnQkFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU07b0JBQUUsSUFBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO1lBQzlDLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztTQUNQO1FBRUQsSUFBSSxDQUFDLFVBQVUsQ0FBQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxRQUFRLENBQUMsVUFBVSxFQUFFLEVBQUMsQ0FBQztJQUN6RCxDQUFDOzs7O0lBRUQsVUFBVTtRQUNSLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxFQUFFLENBQUM7UUFDdkIsSUFBSSxDQUFDLFVBQVUsQ0FBQyxRQUFRLEVBQUUsQ0FBQztJQUM3QixDQUFDOzs7O0lBRUQsYUFBYTtRQUNYLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxDQUFDLGFBQWEsRUFBRSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQyxDQUFDO0lBQzlGLENBQUM7OztZQTdERixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGlCQUFpQjthQUM1Qjs7OztZQU5vQyxVQUFVLHVCQWdCaEMsUUFBUTtZQWhCMEIsU0FBUzs7OzZCQVF2RCxLQUFLLFNBQUMsZUFBZTtzQ0FHckIsS0FBSzs7OztJQUhOLDZDQUM0Qjs7SUFFNUIsc0RBQ3dDOztJQUV4Qyx5Q0FBb0M7Ozs7O0lBRXhCLG9DQUFxQzs7Ozs7SUFBRSx1Q0FBMkIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIElucHV0LCBPcHRpb25hbCwgRWxlbWVudFJlZiwgUmVuZGVyZXIyLCBBZnRlclZpZXdJbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1thYnBWaXNpYmlsaXR5XScsXG59KVxuZXhwb3J0IGNsYXNzIFZpc2liaWxpdHlEaXJlY3RpdmUgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0IHtcbiAgQElucHV0KCdhYnBWaXNpYmlsaXR5JylcbiAgZm9jdXNlZEVsZW1lbnQ6IEhUTUxFbGVtZW50O1xuXG4gIEBJbnB1dCgpXG4gIG11dGF0aW9uT2JzZXJ2ZXJFbmFibGVkOiBib29sZWFuID0gdHJ1ZTtcblxuICBjb21wbGV0ZWQkID0gbmV3IFN1YmplY3Q8Ym9vbGVhbj4oKTtcblxuICBjb25zdHJ1Y3RvcihAT3B0aW9uYWwoKSBwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmLCBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIGxldCBvYnNlcnZlcjogTXV0YXRpb25PYnNlcnZlcjtcbiAgICBpZiAodGhpcy5tdXRhdGlvbk9ic2VydmVyRW5hYmxlZCkge1xuICAgICAgb2JzZXJ2ZXIgPSBuZXcgTXV0YXRpb25PYnNlcnZlcihtdXRhdGlvbnMgPT4ge1xuICAgICAgICBtdXRhdGlvbnMuZm9yRWFjaChtdXRhdGlvbiA9PiB7XG4gICAgICAgICAgaWYgKCFtdXRhdGlvbi50YXJnZXQpIHJldHVybjtcblxuICAgICAgICAgIGNvbnN0IGh0bWxOb2RlcyA9IHNucShcbiAgICAgICAgICAgICgpID0+IEFycmF5LmZyb20obXV0YXRpb24udGFyZ2V0LmNoaWxkTm9kZXMpLmZpbHRlcihub2RlID0+IG5vZGUgaW5zdGFuY2VvZiBIVE1MRWxlbWVudCksXG4gICAgICAgICAgICBbXSxcbiAgICAgICAgICApO1xuXG4gICAgICAgICAgaWYgKCFodG1sTm9kZXMubGVuZ3RoKSB7XG4gICAgICAgICAgICB0aGlzLnJlbW92ZUZyb21ET00oKTtcbiAgICAgICAgICAgIHRoaXMuZGlzY29ubmVjdCgpO1xuICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgICAgICAgdGhpcy5kaXNjb25uZWN0KCk7XG4gICAgICAgICAgICB9LCAwKTtcbiAgICAgICAgICB9XG4gICAgICAgIH0pO1xuICAgICAgfSk7XG5cbiAgICAgIG9ic2VydmVyLm9ic2VydmUodGhpcy5mb2N1c2VkRWxlbWVudCwge1xuICAgICAgICBjaGlsZExpc3Q6IHRydWUsXG4gICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgIGNvbnN0IGh0bWxOb2RlcyA9IHNucShcbiAgICAgICAgICAoKSA9PiBBcnJheS5mcm9tKHRoaXMuZm9jdXNlZEVsZW1lbnQuY2hpbGROb2RlcykuZmlsdGVyKG5vZGUgPT4gbm9kZSBpbnN0YW5jZW9mIEhUTUxFbGVtZW50KSxcbiAgICAgICAgICBbXSxcbiAgICAgICAgKTtcblxuICAgICAgICBpZiAoIWh0bWxOb2Rlcy5sZW5ndGgpIHRoaXMucmVtb3ZlRnJvbURPTSgpO1xuICAgICAgfSwgMCk7XG4gICAgfVxuXG4gICAgdGhpcy5jb21wbGV0ZWQkLnN1YnNjcmliZSgoKSA9PiBvYnNlcnZlci5kaXNjb25uZWN0KCkpO1xuICB9XG5cbiAgZGlzY29ubmVjdCgpIHtcbiAgICB0aGlzLmNvbXBsZXRlZCQubmV4dCgpO1xuICAgIHRoaXMuY29tcGxldGVkJC5jb21wbGV0ZSgpO1xuICB9XG5cbiAgcmVtb3ZlRnJvbURPTSgpIHtcbiAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNoaWxkKHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudC5wYXJlbnRFbGVtZW50LCB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQpO1xuICB9XG59XG4iXX0=