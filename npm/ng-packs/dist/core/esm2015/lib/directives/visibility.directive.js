/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/visibility.directive.ts
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
                }
            }));
        }));
        observer.observe(this.focusedElement, {
            childList: true,
        });
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
        if (!this.elRef.nativeElement)
            return;
        this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
        this.disconnect();
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
    focusedElement: [{ type: Input, args: ['abpVisibility',] }]
};
if (false) {
    /** @type {?} */
    VisibilityDirective.prototype.focusedElement;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmlzaWJpbGl0eS5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy92aXNpYmlsaXR5LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxVQUFVLEVBQUUsU0FBUyxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUNqRyxPQUFPLEVBQUUsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQy9CLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUt0QixNQUFNLE9BQU8sbUJBQW1COzs7OztJQU05QixZQUFnQyxLQUFpQixFQUFVLFFBQW1CO1FBQTlDLFVBQUssR0FBTCxLQUFLLENBQVk7UUFBVSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBRjlFLGVBQVUsR0FBRyxJQUFJLE9BQU8sRUFBVyxDQUFDO0lBRTZDLENBQUM7Ozs7SUFFbEYsZUFBZTtRQUNiLElBQUksQ0FBQyxJQUFJLENBQUMsY0FBYyxJQUFJLElBQUksQ0FBQyxLQUFLLEVBQUU7WUFDdEMsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQztTQUNoRDs7WUFFRyxRQUEwQjtRQUM5QixRQUFRLEdBQUcsSUFBSSxnQkFBZ0I7Ozs7UUFBQyxTQUFTLENBQUMsRUFBRTtZQUMxQyxTQUFTLENBQUMsT0FBTzs7OztZQUFDLFFBQVEsQ0FBQyxFQUFFO2dCQUMzQixJQUFJLENBQUMsUUFBUSxDQUFDLE1BQU07b0JBQUUsT0FBTzs7c0JBRXZCLFNBQVMsR0FBRyxHQUFHOzs7Z0JBQ25CLEdBQUcsRUFBRSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxVQUFVLENBQUMsQ0FBQyxNQUFNOzs7O2dCQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsSUFBSSxZQUFZLFdBQVcsRUFBQyxHQUN4RixFQUFFLENBQ0g7Z0JBRUQsSUFBSSxDQUFDLFNBQVMsQ0FBQyxNQUFNLEVBQUU7b0JBQ3JCLElBQUksQ0FBQyxhQUFhLEVBQUUsQ0FBQztpQkFDdEI7WUFDSCxDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO1FBRUgsUUFBUSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsY0FBYyxFQUFFO1lBQ3BDLFNBQVMsRUFBRSxJQUFJO1NBQ2hCLENBQUMsQ0FBQztRQUVILFVBQVU7OztRQUFDLEdBQUcsRUFBRTs7a0JBQ1IsU0FBUyxHQUFHLEdBQUc7OztZQUNuQixHQUFHLEVBQUUsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxjQUFjLENBQUMsVUFBVSxDQUFDLENBQUMsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsSUFBSSxZQUFZLFdBQVcsRUFBQyxHQUM1RixFQUFFLENBQ0g7WUFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU07Z0JBQUUsSUFBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO1FBQzlDLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztRQUVOLElBQUksQ0FBQyxVQUFVLENBQUMsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsUUFBUSxDQUFDLFVBQVUsRUFBRSxFQUFDLENBQUM7SUFDekQsQ0FBQzs7OztJQUVELFVBQVU7UUFDUixJQUFJLENBQUMsVUFBVSxDQUFDLElBQUksRUFBRSxDQUFDO1FBQ3ZCLElBQUksQ0FBQyxVQUFVLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDN0IsQ0FBQzs7OztJQUVELGFBQWE7UUFDWCxJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhO1lBQUUsT0FBTztRQUV0QyxJQUFJLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQyxhQUFhLEVBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQUMsQ0FBQztRQUM1RixJQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7SUFDcEIsQ0FBQzs7O1lBMURGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsaUJBQWlCO2FBQzVCOzs7O1lBTm9DLFVBQVUsdUJBYWhDLFFBQVE7WUFiMEIsU0FBUzs7OzZCQVF2RCxLQUFLLFNBQUMsZUFBZTs7OztJQUF0Qiw2Q0FDNEI7O0lBRTVCLHlDQUFvQzs7Ozs7SUFFeEIsb0NBQXFDOzs7OztJQUFFLHVDQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgSW5wdXQsIE9wdGlvbmFsLCBFbGVtZW50UmVmLCBSZW5kZXJlcjIsIEFmdGVyVmlld0luaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicFZpc2liaWxpdHldJyxcbn0pXG5leHBvcnQgY2xhc3MgVmlzaWJpbGl0eURpcmVjdGl2ZSBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQge1xuICBASW5wdXQoJ2FicFZpc2liaWxpdHknKVxuICBmb2N1c2VkRWxlbWVudDogSFRNTEVsZW1lbnQ7XG5cbiAgY29tcGxldGVkJCA9IG5ldyBTdWJqZWN0PGJvb2xlYW4+KCk7XG5cbiAgY29uc3RydWN0b3IoQE9wdGlvbmFsKCkgcHJpdmF0ZSBlbFJlZjogRWxlbWVudFJlZiwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcbiAgICBpZiAoIXRoaXMuZm9jdXNlZEVsZW1lbnQgJiYgdGhpcy5lbFJlZikge1xuICAgICAgdGhpcy5mb2N1c2VkRWxlbWVudCA9IHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudDtcbiAgICB9XG5cbiAgICBsZXQgb2JzZXJ2ZXI6IE11dGF0aW9uT2JzZXJ2ZXI7XG4gICAgb2JzZXJ2ZXIgPSBuZXcgTXV0YXRpb25PYnNlcnZlcihtdXRhdGlvbnMgPT4ge1xuICAgICAgbXV0YXRpb25zLmZvckVhY2gobXV0YXRpb24gPT4ge1xuICAgICAgICBpZiAoIW11dGF0aW9uLnRhcmdldCkgcmV0dXJuO1xuXG4gICAgICAgIGNvbnN0IGh0bWxOb2RlcyA9IHNucShcbiAgICAgICAgICAoKSA9PiBBcnJheS5mcm9tKG11dGF0aW9uLnRhcmdldC5jaGlsZE5vZGVzKS5maWx0ZXIobm9kZSA9PiBub2RlIGluc3RhbmNlb2YgSFRNTEVsZW1lbnQpLFxuICAgICAgICAgIFtdLFxuICAgICAgICApO1xuXG4gICAgICAgIGlmICghaHRtbE5vZGVzLmxlbmd0aCkge1xuICAgICAgICAgIHRoaXMucmVtb3ZlRnJvbURPTSgpO1xuICAgICAgICB9XG4gICAgICB9KTtcbiAgICB9KTtcblxuICAgIG9ic2VydmVyLm9ic2VydmUodGhpcy5mb2N1c2VkRWxlbWVudCwge1xuICAgICAgY2hpbGRMaXN0OiB0cnVlLFxuICAgIH0pO1xuXG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICBjb25zdCBodG1sTm9kZXMgPSBzbnEoXG4gICAgICAgICgpID0+IEFycmF5LmZyb20odGhpcy5mb2N1c2VkRWxlbWVudC5jaGlsZE5vZGVzKS5maWx0ZXIobm9kZSA9PiBub2RlIGluc3RhbmNlb2YgSFRNTEVsZW1lbnQpLFxuICAgICAgICBbXSxcbiAgICAgICk7XG5cbiAgICAgIGlmICghaHRtbE5vZGVzLmxlbmd0aCkgdGhpcy5yZW1vdmVGcm9tRE9NKCk7XG4gICAgfSwgMCk7XG5cbiAgICB0aGlzLmNvbXBsZXRlZCQuc3Vic2NyaWJlKCgpID0+IG9ic2VydmVyLmRpc2Nvbm5lY3QoKSk7XG4gIH1cblxuICBkaXNjb25uZWN0KCkge1xuICAgIHRoaXMuY29tcGxldGVkJC5uZXh0KCk7XG4gICAgdGhpcy5jb21wbGV0ZWQkLmNvbXBsZXRlKCk7XG4gIH1cblxuICByZW1vdmVGcm9tRE9NKCkge1xuICAgIGlmICghdGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50KSByZXR1cm47XG5cbiAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNoaWxkKHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudC5wYXJlbnRFbGVtZW50LCB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQpO1xuICAgIHRoaXMuZGlzY29ubmVjdCgpO1xuICB9XG59XG4iXX0=