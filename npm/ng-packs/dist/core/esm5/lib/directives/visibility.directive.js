/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, Optional, ElementRef, Renderer2 } from '@angular/core';
import { Subject } from 'rxjs';
import snq from 'snq';
var VisibilityDirective = /** @class */ (function () {
    function VisibilityDirective(elRef, renderer) {
        this.elRef = elRef;
        this.renderer = renderer;
        this.mutationObserverEnabled = true;
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
        if (this.mutationObserverEnabled) {
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
                        _this.disconnect();
                    }
                    else {
                        setTimeout((/**
                         * @return {?}
                         */
                        function () {
                            _this.disconnect();
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
        }
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
        this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
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
        focusedElement: [{ type: Input, args: ['abpVisibility',] }],
        mutationObserverEnabled: [{ type: Input }]
    };
    return VisibilityDirective;
}());
export { VisibilityDirective };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmlzaWJpbGl0eS5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy92aXNpYmlsaXR5LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBQ2pHLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDL0IsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBRXRCO0lBWUUsNkJBQWdDLEtBQWlCLEVBQVUsUUFBbUI7UUFBOUMsVUFBSyxHQUFMLEtBQUssQ0FBWTtRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUFKOUUsNEJBQXVCLEdBQUcsSUFBSSxDQUFDO1FBRS9CLGVBQVUsR0FBRyxJQUFJLE9BQU8sRUFBVyxDQUFDO0lBRTZDLENBQUM7Ozs7SUFFbEYsNkNBQWU7OztJQUFmO1FBQUEsaUJBMENDO1FBekNDLElBQUksQ0FBQyxJQUFJLENBQUMsY0FBYyxJQUFJLElBQUksQ0FBQyxLQUFLLEVBQUU7WUFDdEMsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQztTQUNoRDs7WUFFRyxRQUEwQjtRQUM5QixJQUFJLElBQUksQ0FBQyx1QkFBdUIsRUFBRTtZQUNoQyxRQUFRLEdBQUcsSUFBSSxnQkFBZ0I7Ozs7WUFBQyxVQUFBLFNBQVM7Z0JBQ3ZDLFNBQVMsQ0FBQyxPQUFPOzs7O2dCQUFDLFVBQUEsUUFBUTtvQkFDeEIsSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNO3dCQUFFLE9BQU87O3dCQUV2QixTQUFTLEdBQUcsR0FBRzs7O29CQUNuQixjQUFNLE9BQUEsS0FBSyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU07Ozs7b0JBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxJQUFJLFlBQVksV0FBVyxFQUEzQixDQUEyQixFQUFDLEVBQWxGLENBQWtGLEdBQ3hGLEVBQUUsQ0FDSDtvQkFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU0sRUFBRTt3QkFDckIsS0FBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO3dCQUNyQixLQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7cUJBQ25CO3lCQUFNO3dCQUNMLFVBQVU7Ozt3QkFBQzs0QkFDVCxLQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7d0JBQ3BCLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztxQkFDUDtnQkFDSCxDQUFDLEVBQUMsQ0FBQztZQUNMLENBQUMsRUFBQyxDQUFDO1lBRUgsUUFBUSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsY0FBYyxFQUFFO2dCQUNwQyxTQUFTLEVBQUUsSUFBSTthQUNoQixDQUFDLENBQUM7U0FDSjthQUFNO1lBQ0wsVUFBVTs7O1lBQUM7O29CQUNILFNBQVMsR0FBRyxHQUFHOzs7Z0JBQ25CLGNBQU0sT0FBQSxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUksQ0FBQyxjQUFjLENBQUMsVUFBVSxDQUFDLENBQUMsTUFBTTs7OztnQkFBQyxVQUFBLElBQUksSUFBSSxPQUFBLElBQUksWUFBWSxXQUFXLEVBQTNCLENBQTJCLEVBQUMsRUFBdEYsQ0FBc0YsR0FDNUYsRUFBRSxDQUNIO2dCQUVELElBQUksQ0FBQyxTQUFTLENBQUMsTUFBTTtvQkFBRSxLQUFJLENBQUMsYUFBYSxFQUFFLENBQUM7WUFDOUMsQ0FBQyxHQUFFLENBQUMsQ0FBQyxDQUFDO1NBQ1A7UUFFRCxJQUFJLENBQUMsVUFBVSxDQUFDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxRQUFRLENBQUMsVUFBVSxFQUFFLEVBQXJCLENBQXFCLEVBQUMsQ0FBQztJQUN6RCxDQUFDOzs7O0lBRUQsd0NBQVU7OztJQUFWO1FBQ0UsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLEVBQUUsQ0FBQztRQUN2QixJQUFJLENBQUMsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQzdCLENBQUM7Ozs7SUFFRCwyQ0FBYTs7O0lBQWI7UUFDRSxJQUFJLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQyxhQUFhLEVBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQUMsQ0FBQztJQUM5RixDQUFDOztnQkFqRUYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxpQkFBaUI7aUJBQzVCOzs7O2dCQU5vQyxVQUFVLHVCQWdCaEMsUUFBUTtnQkFoQjBCLFNBQVM7OztpQ0FRdkQsS0FBSyxTQUFDLGVBQWU7MENBR3JCLEtBQUs7O0lBMkRSLDBCQUFDO0NBQUEsQUFsRUQsSUFrRUM7U0EvRFksbUJBQW1COzs7SUFDOUIsNkNBQzRCOztJQUU1QixzREFDK0I7O0lBRS9CLHlDQUFvQzs7Ozs7SUFFeEIsb0NBQXFDOzs7OztJQUFFLHVDQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgSW5wdXQsIE9wdGlvbmFsLCBFbGVtZW50UmVmLCBSZW5kZXJlcjIsIEFmdGVyVmlld0luaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XHJcblxyXG5ARGlyZWN0aXZlKHtcclxuICBzZWxlY3RvcjogJ1thYnBWaXNpYmlsaXR5XScsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBWaXNpYmlsaXR5RGlyZWN0aXZlIGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCB7XHJcbiAgQElucHV0KCdhYnBWaXNpYmlsaXR5JylcclxuICBmb2N1c2VkRWxlbWVudDogSFRNTEVsZW1lbnQ7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgbXV0YXRpb25PYnNlcnZlckVuYWJsZWQgPSB0cnVlO1xyXG5cclxuICBjb21wbGV0ZWQkID0gbmV3IFN1YmplY3Q8Ym9vbGVhbj4oKTtcclxuXHJcbiAgY29uc3RydWN0b3IoQE9wdGlvbmFsKCkgcHJpdmF0ZSBlbFJlZjogRWxlbWVudFJlZiwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxyXG5cclxuICBuZ0FmdGVyVmlld0luaXQoKSB7XHJcbiAgICBpZiAoIXRoaXMuZm9jdXNlZEVsZW1lbnQgJiYgdGhpcy5lbFJlZikge1xyXG4gICAgICB0aGlzLmZvY3VzZWRFbGVtZW50ID0gdGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50O1xyXG4gICAgfVxyXG5cclxuICAgIGxldCBvYnNlcnZlcjogTXV0YXRpb25PYnNlcnZlcjtcclxuICAgIGlmICh0aGlzLm11dGF0aW9uT2JzZXJ2ZXJFbmFibGVkKSB7XHJcbiAgICAgIG9ic2VydmVyID0gbmV3IE11dGF0aW9uT2JzZXJ2ZXIobXV0YXRpb25zID0+IHtcclxuICAgICAgICBtdXRhdGlvbnMuZm9yRWFjaChtdXRhdGlvbiA9PiB7XHJcbiAgICAgICAgICBpZiAoIW11dGF0aW9uLnRhcmdldCkgcmV0dXJuO1xyXG5cclxuICAgICAgICAgIGNvbnN0IGh0bWxOb2RlcyA9IHNucShcclxuICAgICAgICAgICAgKCkgPT4gQXJyYXkuZnJvbShtdXRhdGlvbi50YXJnZXQuY2hpbGROb2RlcykuZmlsdGVyKG5vZGUgPT4gbm9kZSBpbnN0YW5jZW9mIEhUTUxFbGVtZW50KSxcclxuICAgICAgICAgICAgW10sXHJcbiAgICAgICAgICApO1xyXG5cclxuICAgICAgICAgIGlmICghaHRtbE5vZGVzLmxlbmd0aCkge1xyXG4gICAgICAgICAgICB0aGlzLnJlbW92ZUZyb21ET00oKTtcclxuICAgICAgICAgICAgdGhpcy5kaXNjb25uZWN0KCk7XHJcbiAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgICAgICAgICB0aGlzLmRpc2Nvbm5lY3QoKTtcclxuICAgICAgICAgICAgfSwgMCk7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICAgIH0pO1xyXG5cclxuICAgICAgb2JzZXJ2ZXIub2JzZXJ2ZSh0aGlzLmZvY3VzZWRFbGVtZW50LCB7XHJcbiAgICAgICAgY2hpbGRMaXN0OiB0cnVlLFxyXG4gICAgICB9KTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xyXG4gICAgICAgIGNvbnN0IGh0bWxOb2RlcyA9IHNucShcclxuICAgICAgICAgICgpID0+IEFycmF5LmZyb20odGhpcy5mb2N1c2VkRWxlbWVudC5jaGlsZE5vZGVzKS5maWx0ZXIobm9kZSA9PiBub2RlIGluc3RhbmNlb2YgSFRNTEVsZW1lbnQpLFxyXG4gICAgICAgICAgW10sXHJcbiAgICAgICAgKTtcclxuXHJcbiAgICAgICAgaWYgKCFodG1sTm9kZXMubGVuZ3RoKSB0aGlzLnJlbW92ZUZyb21ET00oKTtcclxuICAgICAgfSwgMCk7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5jb21wbGV0ZWQkLnN1YnNjcmliZSgoKSA9PiBvYnNlcnZlci5kaXNjb25uZWN0KCkpO1xyXG4gIH1cclxuXHJcbiAgZGlzY29ubmVjdCgpIHtcclxuICAgIHRoaXMuY29tcGxldGVkJC5uZXh0KCk7XHJcbiAgICB0aGlzLmNvbXBsZXRlZCQuY29tcGxldGUoKTtcclxuICB9XHJcblxyXG4gIHJlbW92ZUZyb21ET00oKSB7XHJcbiAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNoaWxkKHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudC5wYXJlbnRFbGVtZW50LCB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQpO1xyXG4gIH1cclxufVxyXG4iXX0=