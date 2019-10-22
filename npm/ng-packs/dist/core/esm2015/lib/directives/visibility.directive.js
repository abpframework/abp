/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        if (!this.focusedElement && this.elRef) {
            this.focusedElement = this.elRef.nativeElement;
        }
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmlzaWJpbGl0eS5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy92aXNpYmlsaXR5LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBQ2pHLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDL0IsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBS3RCLE1BQU0sT0FBTyxtQkFBbUI7Ozs7O0lBUzlCLFlBQWdDLEtBQWlCLEVBQVUsUUFBbUI7UUFBOUMsVUFBSyxHQUFMLEtBQUssQ0FBWTtRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUFKOUUsNEJBQXVCLEdBQUcsSUFBSSxDQUFDO1FBRS9CLGVBQVUsR0FBRyxJQUFJLE9BQU8sRUFBVyxDQUFDO0lBRTZDLENBQUM7Ozs7SUFFbEYsZUFBZTtRQUNiLElBQUksQ0FBQyxJQUFJLENBQUMsY0FBYyxJQUFJLElBQUksQ0FBQyxLQUFLLEVBQUU7WUFDdEMsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQztTQUNoRDs7WUFFRyxRQUEwQjtRQUM5QixJQUFJLElBQUksQ0FBQyx1QkFBdUIsRUFBRTtZQUNoQyxRQUFRLEdBQUcsSUFBSSxnQkFBZ0I7Ozs7WUFBQyxTQUFTLENBQUMsRUFBRTtnQkFDMUMsU0FBUyxDQUFDLE9BQU87Ozs7Z0JBQUMsUUFBUSxDQUFDLEVBQUU7b0JBQzNCLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTTt3QkFBRSxPQUFPOzswQkFFdkIsU0FBUyxHQUFHLEdBQUc7OztvQkFDbkIsR0FBRyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU07Ozs7b0JBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLFlBQVksV0FBVyxFQUFDLEdBQ3hGLEVBQUUsQ0FDSDtvQkFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU0sRUFBRTt3QkFDckIsSUFBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO3dCQUNyQixJQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7cUJBQ25CO3lCQUFNO3dCQUNMLFVBQVU7Ozt3QkFBQyxHQUFHLEVBQUU7NEJBQ2QsSUFBSSxDQUFDLFVBQVUsRUFBRSxDQUFDO3dCQUNwQixDQUFDLEdBQUUsQ0FBQyxDQUFDLENBQUM7cUJBQ1A7Z0JBQ0gsQ0FBQyxFQUFDLENBQUM7WUFDTCxDQUFDLEVBQUMsQ0FBQztZQUVILFFBQVEsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLGNBQWMsRUFBRTtnQkFDcEMsU0FBUyxFQUFFLElBQUk7YUFDaEIsQ0FBQyxDQUFDO1NBQ0o7YUFBTTtZQUNMLFVBQVU7OztZQUFDLEdBQUcsRUFBRTs7c0JBQ1IsU0FBUyxHQUFHLEdBQUc7OztnQkFDbkIsR0FBRyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsY0FBYyxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU07Ozs7Z0JBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLFlBQVksV0FBVyxFQUFDLEdBQzVGLEVBQUUsQ0FDSDtnQkFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU07b0JBQUUsSUFBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO1lBQzlDLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztTQUNQO1FBRUQsSUFBSSxDQUFDLFVBQVUsQ0FBQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxRQUFRLENBQUMsVUFBVSxFQUFFLEVBQUMsQ0FBQztJQUN6RCxDQUFDOzs7O0lBRUQsVUFBVTtRQUNSLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxFQUFFLENBQUM7UUFDdkIsSUFBSSxDQUFDLFVBQVUsQ0FBQyxRQUFRLEVBQUUsQ0FBQztJQUM3QixDQUFDOzs7O0lBRUQsYUFBYTtRQUNYLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxDQUFDLGFBQWEsRUFBRSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQyxDQUFDO0lBQzlGLENBQUM7OztZQWpFRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGlCQUFpQjthQUM1Qjs7OztZQU5vQyxVQUFVLHVCQWdCaEMsUUFBUTtZQWhCMEIsU0FBUzs7OzZCQVF2RCxLQUFLLFNBQUMsZUFBZTtzQ0FHckIsS0FBSzs7OztJQUhOLDZDQUM0Qjs7SUFFNUIsc0RBQytCOztJQUUvQix5Q0FBb0M7Ozs7O0lBRXhCLG9DQUFxQzs7Ozs7SUFBRSx1Q0FBMkIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIElucHV0LCBPcHRpb25hbCwgRWxlbWVudFJlZiwgUmVuZGVyZXIyLCBBZnRlclZpZXdJbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFN1YmplY3QgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5cclxuQERpcmVjdGl2ZSh7XHJcbiAgc2VsZWN0b3I6ICdbYWJwVmlzaWJpbGl0eV0nLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVmlzaWJpbGl0eURpcmVjdGl2ZSBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQge1xyXG4gIEBJbnB1dCgnYWJwVmlzaWJpbGl0eScpXHJcbiAgZm9jdXNlZEVsZW1lbnQ6IEhUTUxFbGVtZW50O1xyXG5cclxuICBASW5wdXQoKVxyXG4gIG11dGF0aW9uT2JzZXJ2ZXJFbmFibGVkID0gdHJ1ZTtcclxuXHJcbiAgY29tcGxldGVkJCA9IG5ldyBTdWJqZWN0PGJvb2xlYW4+KCk7XHJcblxyXG4gIGNvbnN0cnVjdG9yKEBPcHRpb25hbCgpIHByaXZhdGUgZWxSZWY6IEVsZW1lbnRSZWYsIHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMikge31cclxuXHJcbiAgbmdBZnRlclZpZXdJbml0KCkge1xyXG4gICAgaWYgKCF0aGlzLmZvY3VzZWRFbGVtZW50ICYmIHRoaXMuZWxSZWYpIHtcclxuICAgICAgdGhpcy5mb2N1c2VkRWxlbWVudCA9IHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudDtcclxuICAgIH1cclxuXHJcbiAgICBsZXQgb2JzZXJ2ZXI6IE11dGF0aW9uT2JzZXJ2ZXI7XHJcbiAgICBpZiAodGhpcy5tdXRhdGlvbk9ic2VydmVyRW5hYmxlZCkge1xyXG4gICAgICBvYnNlcnZlciA9IG5ldyBNdXRhdGlvbk9ic2VydmVyKG11dGF0aW9ucyA9PiB7XHJcbiAgICAgICAgbXV0YXRpb25zLmZvckVhY2gobXV0YXRpb24gPT4ge1xyXG4gICAgICAgICAgaWYgKCFtdXRhdGlvbi50YXJnZXQpIHJldHVybjtcclxuXHJcbiAgICAgICAgICBjb25zdCBodG1sTm9kZXMgPSBzbnEoXHJcbiAgICAgICAgICAgICgpID0+IEFycmF5LmZyb20obXV0YXRpb24udGFyZ2V0LmNoaWxkTm9kZXMpLmZpbHRlcihub2RlID0+IG5vZGUgaW5zdGFuY2VvZiBIVE1MRWxlbWVudCksXHJcbiAgICAgICAgICAgIFtdLFxyXG4gICAgICAgICAgKTtcclxuXHJcbiAgICAgICAgICBpZiAoIWh0bWxOb2Rlcy5sZW5ndGgpIHtcclxuICAgICAgICAgICAgdGhpcy5yZW1vdmVGcm9tRE9NKCk7XHJcbiAgICAgICAgICAgIHRoaXMuZGlzY29ubmVjdCgpO1xyXG4gICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7XHJcbiAgICAgICAgICAgICAgdGhpcy5kaXNjb25uZWN0KCk7XHJcbiAgICAgICAgICAgIH0sIDApO1xyXG4gICAgICAgICAgfVxyXG4gICAgICAgIH0pO1xyXG4gICAgICB9KTtcclxuXHJcbiAgICAgIG9ic2VydmVyLm9ic2VydmUodGhpcy5mb2N1c2VkRWxlbWVudCwge1xyXG4gICAgICAgIGNoaWxkTGlzdDogdHJ1ZSxcclxuICAgICAgfSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgICBjb25zdCBodG1sTm9kZXMgPSBzbnEoXHJcbiAgICAgICAgICAoKSA9PiBBcnJheS5mcm9tKHRoaXMuZm9jdXNlZEVsZW1lbnQuY2hpbGROb2RlcykuZmlsdGVyKG5vZGUgPT4gbm9kZSBpbnN0YW5jZW9mIEhUTUxFbGVtZW50KSxcclxuICAgICAgICAgIFtdLFxyXG4gICAgICAgICk7XHJcblxyXG4gICAgICAgIGlmICghaHRtbE5vZGVzLmxlbmd0aCkgdGhpcy5yZW1vdmVGcm9tRE9NKCk7XHJcbiAgICAgIH0sIDApO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuY29tcGxldGVkJC5zdWJzY3JpYmUoKCkgPT4gb2JzZXJ2ZXIuZGlzY29ubmVjdCgpKTtcclxuICB9XHJcblxyXG4gIGRpc2Nvbm5lY3QoKSB7XHJcbiAgICB0aGlzLmNvbXBsZXRlZCQubmV4dCgpO1xyXG4gICAgdGhpcy5jb21wbGV0ZWQkLmNvbXBsZXRlKCk7XHJcbiAgfVxyXG5cclxuICByZW1vdmVGcm9tRE9NKCkge1xyXG4gICAgdGhpcy5yZW5kZXJlci5yZW1vdmVDaGlsZCh0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQucGFyZW50RWxlbWVudCwgdGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50KTtcclxuICB9XHJcbn1cclxuIl19