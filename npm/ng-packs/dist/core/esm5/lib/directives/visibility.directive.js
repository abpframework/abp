/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/visibility.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, Optional, ElementRef, Renderer2 } from '@angular/core';
import { Subject } from 'rxjs';
import snq from 'snq';
var VisibilityDirective = /** @class */ (function () {
    function VisibilityDirective(elRef, renderer) {
        this.elRef = elRef;
        this.renderer = renderer;
        this.completed$ = new Subject();
    }
    /**
     * @return {?}
     */
    VisibilityDirective.prototype.ngAfterViewInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (!this.focusedElement && this.elRef) {
            this.focusedElement = this.elRef.nativeElement;
        }
        /** @type {?} */
        var observer;
        observer = new MutationObserver((/**
         * @param {?} mutations
         * @return {?}
         */
        function (mutations) {
            mutations.forEach((/**
             * @param {?} mutation
             * @return {?}
             */
            function (mutation) {
                if (!mutation.target)
                    return;
                /** @type {?} */
                var htmlNodes = snq((/**
                 * @return {?}
                 */
                function () { return Array.from(mutation.target.childNodes).filter((/**
                 * @param {?} node
                 * @return {?}
                 */
                function (node) { return node instanceof HTMLElement; })); }), []);
                if (!htmlNodes.length) {
                    _this.removeFromDOM();
                }
            }));
        }));
        observer.observe(this.focusedElement, {
            childList: true,
        });
        setTimeout((/**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var htmlNodes = snq((/**
             * @return {?}
             */
            function () { return Array.from(_this.focusedElement.childNodes).filter((/**
             * @param {?} node
             * @return {?}
             */
            function (node) { return node instanceof HTMLElement; })); }), []);
            if (!htmlNodes.length)
                _this.removeFromDOM();
        }), 0);
        this.completed$.subscribe((/**
         * @return {?}
         */
        function () { return observer.disconnect(); }));
    };
    /**
     * @return {?}
     */
    VisibilityDirective.prototype.disconnect = /**
     * @return {?}
     */
    function () {
        this.completed$.next();
        this.completed$.complete();
    };
    /**
     * @return {?}
     */
    VisibilityDirective.prototype.removeFromDOM = /**
     * @return {?}
     */
    function () {
        if (!this.elRef.nativeElement)
            return;
        this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
        this.disconnect();
    };
    VisibilityDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[abpVisibility]',
                },] }
    ];
    /** @nocollapse */
    VisibilityDirective.ctorParameters = function () { return [
        { type: ElementRef, decorators: [{ type: Optional }] },
        { type: Renderer2 }
    ]; };
    VisibilityDirective.propDecorators = {
        focusedElement: [{ type: Input, args: ['abpVisibility',] }]
    };
    return VisibilityDirective;
}());
export { VisibilityDirective };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmlzaWJpbGl0eS5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy92aXNpYmlsaXR5LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxVQUFVLEVBQUUsU0FBUyxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUNqRyxPQUFPLEVBQUUsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQy9CLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUV0QjtJQVNFLDZCQUFnQyxLQUFpQixFQUFVLFFBQW1CO1FBQTlDLFVBQUssR0FBTCxLQUFLLENBQVk7UUFBVSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBRjlFLGVBQVUsR0FBRyxJQUFJLE9BQU8sRUFBVyxDQUFDO0lBRTZDLENBQUM7Ozs7SUFFbEYsNkNBQWU7OztJQUFmO1FBQUEsaUJBbUNDO1FBbENDLElBQUksQ0FBQyxJQUFJLENBQUMsY0FBYyxJQUFJLElBQUksQ0FBQyxLQUFLLEVBQUU7WUFDdEMsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQztTQUNoRDs7WUFFRyxRQUEwQjtRQUM5QixRQUFRLEdBQUcsSUFBSSxnQkFBZ0I7Ozs7UUFBQyxVQUFBLFNBQVM7WUFDdkMsU0FBUyxDQUFDLE9BQU87Ozs7WUFBQyxVQUFBLFFBQVE7Z0JBQ3hCLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTTtvQkFBRSxPQUFPOztvQkFFdkIsU0FBUyxHQUFHLEdBQUc7OztnQkFDbkIsY0FBTSxPQUFBLEtBQUssQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxVQUFVLENBQUMsQ0FBQyxNQUFNOzs7O2dCQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsSUFBSSxZQUFZLFdBQVcsRUFBM0IsQ0FBMkIsRUFBQyxFQUFsRixDQUFrRixHQUN4RixFQUFFLENBQ0g7Z0JBRUQsSUFBSSxDQUFDLFNBQVMsQ0FBQyxNQUFNLEVBQUU7b0JBQ3JCLEtBQUksQ0FBQyxhQUFhLEVBQUUsQ0FBQztpQkFDdEI7WUFDSCxDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO1FBRUgsUUFBUSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsY0FBYyxFQUFFO1lBQ3BDLFNBQVMsRUFBRSxJQUFJO1NBQ2hCLENBQUMsQ0FBQztRQUVILFVBQVU7OztRQUFDOztnQkFDSCxTQUFTLEdBQUcsR0FBRzs7O1lBQ25CLGNBQU0sT0FBQSxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUksQ0FBQyxjQUFjLENBQUMsVUFBVSxDQUFDLENBQUMsTUFBTTs7OztZQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsSUFBSSxZQUFZLFdBQVcsRUFBM0IsQ0FBMkIsRUFBQyxFQUF0RixDQUFzRixHQUM1RixFQUFFLENBQ0g7WUFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU07Z0JBQUUsS0FBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO1FBQzlDLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztRQUVOLElBQUksQ0FBQyxVQUFVLENBQUMsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxVQUFVLEVBQUUsRUFBckIsQ0FBcUIsRUFBQyxDQUFDO0lBQ3pELENBQUM7Ozs7SUFFRCx3Q0FBVTs7O0lBQVY7UUFDRSxJQUFJLENBQUMsVUFBVSxDQUFDLElBQUksRUFBRSxDQUFDO1FBQ3ZCLElBQUksQ0FBQyxVQUFVLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDN0IsQ0FBQzs7OztJQUVELDJDQUFhOzs7SUFBYjtRQUNFLElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWE7WUFBRSxPQUFPO1FBRXRDLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxDQUFDLGFBQWEsRUFBRSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQyxDQUFDO1FBQzVGLElBQUksQ0FBQyxVQUFVLEVBQUUsQ0FBQztJQUNwQixDQUFDOztnQkExREYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxpQkFBaUI7aUJBQzVCOzs7O2dCQU5vQyxVQUFVLHVCQWFoQyxRQUFRO2dCQWIwQixTQUFTOzs7aUNBUXZELEtBQUssU0FBQyxlQUFlOztJQXVEeEIsMEJBQUM7Q0FBQSxBQTNERCxJQTJEQztTQXhEWSxtQkFBbUI7OztJQUM5Qiw2Q0FDNEI7O0lBRTVCLHlDQUFvQzs7Ozs7SUFFeEIsb0NBQXFDOzs7OztJQUFFLHVDQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgSW5wdXQsIE9wdGlvbmFsLCBFbGVtZW50UmVmLCBSZW5kZXJlcjIsIEFmdGVyVmlld0luaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XHJcblxyXG5ARGlyZWN0aXZlKHtcclxuICBzZWxlY3RvcjogJ1thYnBWaXNpYmlsaXR5XScsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBWaXNpYmlsaXR5RGlyZWN0aXZlIGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCB7XHJcbiAgQElucHV0KCdhYnBWaXNpYmlsaXR5JylcclxuICBmb2N1c2VkRWxlbWVudDogSFRNTEVsZW1lbnQ7XHJcblxyXG4gIGNvbXBsZXRlZCQgPSBuZXcgU3ViamVjdDxib29sZWFuPigpO1xyXG5cclxuICBjb25zdHJ1Y3RvcihAT3B0aW9uYWwoKSBwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmLCBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XHJcblxyXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcclxuICAgIGlmICghdGhpcy5mb2N1c2VkRWxlbWVudCAmJiB0aGlzLmVsUmVmKSB7XHJcbiAgICAgIHRoaXMuZm9jdXNlZEVsZW1lbnQgPSB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQ7XHJcbiAgICB9XHJcblxyXG4gICAgbGV0IG9ic2VydmVyOiBNdXRhdGlvbk9ic2VydmVyO1xyXG4gICAgb2JzZXJ2ZXIgPSBuZXcgTXV0YXRpb25PYnNlcnZlcihtdXRhdGlvbnMgPT4ge1xyXG4gICAgICBtdXRhdGlvbnMuZm9yRWFjaChtdXRhdGlvbiA9PiB7XHJcbiAgICAgICAgaWYgKCFtdXRhdGlvbi50YXJnZXQpIHJldHVybjtcclxuXHJcbiAgICAgICAgY29uc3QgaHRtbE5vZGVzID0gc25xKFxyXG4gICAgICAgICAgKCkgPT4gQXJyYXkuZnJvbShtdXRhdGlvbi50YXJnZXQuY2hpbGROb2RlcykuZmlsdGVyKG5vZGUgPT4gbm9kZSBpbnN0YW5jZW9mIEhUTUxFbGVtZW50KSxcclxuICAgICAgICAgIFtdLFxyXG4gICAgICAgICk7XHJcblxyXG4gICAgICAgIGlmICghaHRtbE5vZGVzLmxlbmd0aCkge1xyXG4gICAgICAgICAgdGhpcy5yZW1vdmVGcm9tRE9NKCk7XHJcbiAgICAgICAgfVxyXG4gICAgICB9KTtcclxuICAgIH0pO1xyXG5cclxuICAgIG9ic2VydmVyLm9ic2VydmUodGhpcy5mb2N1c2VkRWxlbWVudCwge1xyXG4gICAgICBjaGlsZExpc3Q6IHRydWUsXHJcbiAgICB9KTtcclxuXHJcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgY29uc3QgaHRtbE5vZGVzID0gc25xKFxyXG4gICAgICAgICgpID0+IEFycmF5LmZyb20odGhpcy5mb2N1c2VkRWxlbWVudC5jaGlsZE5vZGVzKS5maWx0ZXIobm9kZSA9PiBub2RlIGluc3RhbmNlb2YgSFRNTEVsZW1lbnQpLFxyXG4gICAgICAgIFtdLFxyXG4gICAgICApO1xyXG5cclxuICAgICAgaWYgKCFodG1sTm9kZXMubGVuZ3RoKSB0aGlzLnJlbW92ZUZyb21ET00oKTtcclxuICAgIH0sIDApO1xyXG5cclxuICAgIHRoaXMuY29tcGxldGVkJC5zdWJzY3JpYmUoKCkgPT4gb2JzZXJ2ZXIuZGlzY29ubmVjdCgpKTtcclxuICB9XHJcblxyXG4gIGRpc2Nvbm5lY3QoKSB7XHJcbiAgICB0aGlzLmNvbXBsZXRlZCQubmV4dCgpO1xyXG4gICAgdGhpcy5jb21wbGV0ZWQkLmNvbXBsZXRlKCk7XHJcbiAgfVxyXG5cclxuICByZW1vdmVGcm9tRE9NKCkge1xyXG4gICAgaWYgKCF0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQpIHJldHVybjtcclxuXHJcbiAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNoaWxkKHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudC5wYXJlbnRFbGVtZW50LCB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQpO1xyXG4gICAgdGhpcy5kaXNjb25uZWN0KCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==