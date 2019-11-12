/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/permission.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, Input, Optional, Renderer2 } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
var PermissionDirective = /** @class */ (function () {
    function PermissionDirective(elRef, renderer, store) {
        this.elRef = elRef;
        this.renderer = renderer;
        this.store = store;
    }
    /**
     * @return {?}
     */
    PermissionDirective.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.condition) {
            this.store
                .select(ConfigState.getGrantedPolicy(this.condition))
                .pipe(takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} isGranted
             * @return {?}
             */
            function (isGranted) {
                if (!isGranted) {
                    _this.renderer.removeChild(((/** @type {?} */ (_this.elRef.nativeElement))).parentElement, _this.elRef.nativeElement);
                }
            }));
        }
    };
    /**
     * @return {?}
     */
    PermissionDirective.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    PermissionDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[abpPermission]',
                },] }
    ];
    /** @nocollapse */
    PermissionDirective.ctorParameters = function () { return [
        { type: ElementRef, decorators: [{ type: Optional }] },
        { type: Renderer2 },
        { type: Store }
    ]; };
    PermissionDirective.propDecorators = {
        condition: [{ type: Input, args: ['abpPermission',] }]
    };
    return PermissionDirective;
}());
export { PermissionDirective };
if (false) {
    /** @type {?} */
    PermissionDirective.prototype.condition;
    /**
     * @type {?}
     * @private
     */
    PermissionDirective.prototype.elRef;
    /**
     * @type {?}
     * @private
     */
    PermissionDirective.prototype.renderer;
    /**
     * @type {?}
     * @private
     */
    PermissionDirective.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9wZXJtaXNzaW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLEtBQUssRUFBcUIsUUFBUSxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNyRyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFDeEMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBRTVDO0lBTUUsNkJBQWdDLEtBQWlCLEVBQVUsUUFBbUIsRUFBVSxLQUFZO1FBQXBFLFVBQUssR0FBTCxLQUFLLENBQVk7UUFBVSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFeEcsc0NBQVE7OztJQUFSO1FBQUEsaUJBY0M7UUFiQyxJQUFJLElBQUksQ0FBQyxTQUFTLEVBQUU7WUFDbEIsSUFBSSxDQUFDLEtBQUs7aUJBQ1AsTUFBTSxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUM7aUJBQ3BELElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FBQztpQkFDNUIsU0FBUzs7OztZQUFDLFVBQUEsU0FBUztnQkFDbEIsSUFBSSxDQUFDLFNBQVMsRUFBRTtvQkFDZCxLQUFJLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FDdkIsQ0FBQyxtQkFBQSxLQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBZSxDQUFDLENBQUMsYUFBYSxFQUN2RCxLQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FDekIsQ0FBQztpQkFDSDtZQUNILENBQUMsRUFBQyxDQUFDO1NBQ047SUFDSCxDQUFDOzs7O0lBRUQseUNBQVc7OztJQUFYLGNBQXFCLENBQUM7O2dCQXhCdkIsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxpQkFBaUI7aUJBQzVCOzs7O2dCQVBtQixVQUFVLHVCQVdmLFFBQVE7Z0JBWDZDLFNBQVM7Z0JBQ3BFLEtBQUs7Ozs0QkFRWCxLQUFLLFNBQUMsZUFBZTs7SUFxQnhCLDBCQUFDO0NBQUEsQUF6QkQsSUF5QkM7U0F0QlksbUJBQW1COzs7SUFDOUIsd0NBQTBDOzs7OztJQUU5QixvQ0FBcUM7Ozs7O0lBQUUsdUNBQTJCOzs7OztJQUFFLG9DQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgRWxlbWVudFJlZiwgSW5wdXQsIE9uRGVzdHJveSwgT25Jbml0LCBPcHRpb25hbCwgUmVuZGVyZXIyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XHJcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICcuLi91dGlscyc7XHJcblxyXG5ARGlyZWN0aXZlKHtcclxuICBzZWxlY3RvcjogJ1thYnBQZXJtaXNzaW9uXScsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uRGlyZWN0aXZlIGltcGxlbWVudHMgT25Jbml0LCBPbkRlc3Ryb3kge1xyXG4gIEBJbnB1dCgnYWJwUGVybWlzc2lvbicpIGNvbmRpdGlvbjogc3RyaW5nO1xyXG5cclxuICBjb25zdHJ1Y3RvcihAT3B0aW9uYWwoKSBwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmLCBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpIHtcclxuICAgIGlmICh0aGlzLmNvbmRpdGlvbikge1xyXG4gICAgICB0aGlzLnN0b3JlXHJcbiAgICAgICAgLnNlbGVjdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KHRoaXMuY29uZGl0aW9uKSlcclxuICAgICAgICAucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKVxyXG4gICAgICAgIC5zdWJzY3JpYmUoaXNHcmFudGVkID0+IHtcclxuICAgICAgICAgIGlmICghaXNHcmFudGVkKSB7XHJcbiAgICAgICAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlQ2hpbGQoXHJcbiAgICAgICAgICAgICAgKHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudCBhcyBIVE1MRWxlbWVudCkucGFyZW50RWxlbWVudCxcclxuICAgICAgICAgICAgICB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQsXHJcbiAgICAgICAgICAgICk7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBuZ09uRGVzdHJveSgpOiB2b2lkIHt9XHJcbn1cclxuIl19