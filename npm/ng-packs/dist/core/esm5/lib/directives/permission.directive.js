/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, Input, Renderer2, ViewContainerRef, TemplateRef, Optional, } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
var PermissionDirective = /** @class */ (function () {
    function PermissionDirective(elRef, renderer, store, templateRef, vcRef) {
        this.elRef = elRef;
        this.renderer = renderer;
        this.store = store;
        this.templateRef = templateRef;
        this.vcRef = vcRef;
    }
    /**
     * @private
     * @return {?}
     */
    PermissionDirective.prototype.check = /**
     * @private
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
        this.subscription = this.store
            .select(ConfigState.getGrantedPolicy(this.condition))
            .pipe(takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} isGranted
         * @return {?}
         */
        function (isGranted) {
            if (_this.templateRef && isGranted) {
                _this.vcRef.clear();
                _this.vcRef.createEmbeddedView(_this.templateRef);
            }
            else if (_this.templateRef && !isGranted) {
                _this.vcRef.clear();
            }
            else if (!isGranted && !_this.templateRef) {
                _this.renderer.removeChild(((/** @type {?} */ (_this.elRef.nativeElement))).parentElement, _this.elRef.nativeElement);
            }
        }));
    };
    /**
     * @return {?}
     */
    PermissionDirective.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        if (this.templateRef && !this.condition) {
            this.vcRef.createEmbeddedView(this.templateRef);
        }
    };
    /**
     * @return {?}
     */
    PermissionDirective.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionDirective.prototype.ngOnChanges = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var condition = _a.condition;
        if ((condition || { currentValue: null }).currentValue) {
            this.check();
        }
    };
    PermissionDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[abpPermission]',
                },] }
    ];
    /** @nocollapse */
    PermissionDirective.ctorParameters = function () { return [
        { type: ElementRef },
        { type: Renderer2 },
        { type: Store },
        { type: TemplateRef, decorators: [{ type: Optional }] },
        { type: ViewContainerRef }
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
    /** @type {?} */
    PermissionDirective.prototype.subscription;
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
    /**
     * @type {?}
     * @private
     */
    PermissionDirective.prototype.templateRef;
    /**
     * @type {?}
     * @private
     */
    PermissionDirective.prototype.vcRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9wZXJtaXNzaW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUNMLFNBQVMsRUFDVCxVQUFVLEVBQ1YsS0FBSyxFQUdMLFNBQVMsRUFDVCxnQkFBZ0IsRUFDaEIsV0FBVyxFQUNYLFFBQVEsR0FHVCxNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFDeEMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBRzVDO0lBUUUsNkJBQ1UsS0FBaUIsRUFDakIsUUFBbUIsRUFDbkIsS0FBWSxFQUNBLFdBQTZCLEVBQ3pDLEtBQXVCO1FBSnZCLFVBQUssR0FBTCxLQUFLLENBQVk7UUFDakIsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQUNuQixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQ0EsZ0JBQVcsR0FBWCxXQUFXLENBQWtCO1FBQ3pDLFVBQUssR0FBTCxLQUFLLENBQWtCO0lBQzlCLENBQUM7Ozs7O0lBRUksbUNBQUs7Ozs7SUFBYjtRQUFBLGlCQWtCQztRQWpCQyxJQUFJLElBQUksQ0FBQyxZQUFZLEVBQUU7WUFDckIsSUFBSSxDQUFDLFlBQVksQ0FBQyxXQUFXLEVBQUUsQ0FBQztTQUNqQztRQUVELElBQUksQ0FBQyxZQUFZLEdBQUcsSUFBSSxDQUFDLEtBQUs7YUFDM0IsTUFBTSxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUM7YUFDcEQsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUFDO2FBQzVCLFNBQVM7Ozs7UUFBQyxVQUFBLFNBQVM7WUFDbEIsSUFBSSxLQUFJLENBQUMsV0FBVyxJQUFJLFNBQVMsRUFBRTtnQkFDakMsS0FBSSxDQUFDLEtBQUssQ0FBQyxLQUFLLEVBQUUsQ0FBQztnQkFDbkIsS0FBSSxDQUFDLEtBQUssQ0FBQyxrQkFBa0IsQ0FBQyxLQUFJLENBQUMsV0FBVyxDQUFDLENBQUM7YUFDakQ7aUJBQU0sSUFBSSxLQUFJLENBQUMsV0FBVyxJQUFJLENBQUMsU0FBUyxFQUFFO2dCQUN6QyxLQUFJLENBQUMsS0FBSyxDQUFDLEtBQUssRUFBRSxDQUFDO2FBQ3BCO2lCQUFNLElBQUksQ0FBQyxTQUFTLElBQUksQ0FBQyxLQUFJLENBQUMsV0FBVyxFQUFFO2dCQUMxQyxLQUFJLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxDQUFDLG1CQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFlLENBQUMsQ0FBQyxhQUFhLEVBQUUsS0FBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQUMsQ0FBQzthQUM5RztRQUNILENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7OztJQUVELHNDQUFROzs7SUFBUjtRQUNFLElBQUksSUFBSSxDQUFDLFdBQVcsSUFBSSxDQUFDLElBQUksQ0FBQyxTQUFTLEVBQUU7WUFDdkMsSUFBSSxDQUFDLEtBQUssQ0FBQyxrQkFBa0IsQ0FBQyxJQUFJLENBQUMsV0FBVyxDQUFDLENBQUM7U0FDakQ7SUFDSCxDQUFDOzs7O0lBRUQseUNBQVc7OztJQUFYLGNBQXFCLENBQUM7Ozs7O0lBRXRCLHlDQUFXOzs7O0lBQVgsVUFBWSxFQUE0QjtZQUExQix3QkFBUztRQUNyQixJQUFJLENBQUMsU0FBUyxJQUFJLEVBQUUsWUFBWSxFQUFFLElBQUksRUFBRSxDQUFDLENBQUMsWUFBWSxFQUFFO1lBQ3RELElBQUksQ0FBQyxLQUFLLEVBQUUsQ0FBQztTQUNkO0lBQ0gsQ0FBQzs7Z0JBaERGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsaUJBQWlCO2lCQUM1Qjs7OztnQkFsQkMsVUFBVTtnQkFJVixTQUFTO2dCQU9GLEtBQUs7Z0JBTFosV0FBVyx1QkFzQlIsUUFBUTtnQkF2QlgsZ0JBQWdCOzs7NEJBZWYsS0FBSyxTQUFDLGVBQWU7O0lBNkN4QiwwQkFBQztDQUFBLEFBakRELElBaURDO1NBOUNZLG1CQUFtQjs7O0lBQzlCLHdDQUEwQzs7SUFFMUMsMkNBQTJCOzs7OztJQUd6QixvQ0FBeUI7Ozs7O0lBQ3pCLHVDQUEyQjs7Ozs7SUFDM0Isb0NBQW9COzs7OztJQUNwQiwwQ0FBaUQ7Ozs7O0lBQ2pELG9DQUErQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XHJcbiAgRGlyZWN0aXZlLFxyXG4gIEVsZW1lbnRSZWYsXHJcbiAgSW5wdXQsXHJcbiAgT25EZXN0cm95LFxyXG4gIE9uSW5pdCxcclxuICBSZW5kZXJlcjIsXHJcbiAgVmlld0NvbnRhaW5lclJlZixcclxuICBUZW1wbGF0ZVJlZixcclxuICBPcHRpb25hbCxcclxuICBTaW1wbGVDaGFuZ2VzLFxyXG4gIE9uQ2hhbmdlcyxcclxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcclxuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzJztcclxuaW1wb3J0IHsgU3Vic2NyaXB0aW9uIH0gZnJvbSAncnhqcyc7XHJcblxyXG5ARGlyZWN0aXZlKHtcclxuICBzZWxlY3RvcjogJ1thYnBQZXJtaXNzaW9uXScsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uRGlyZWN0aXZlIGltcGxlbWVudHMgT25Jbml0LCBPbkRlc3Ryb3ksIE9uQ2hhbmdlcyB7XHJcbiAgQElucHV0KCdhYnBQZXJtaXNzaW9uJykgY29uZGl0aW9uOiBzdHJpbmc7XHJcblxyXG4gIHN1YnNjcmlwdGlvbjogU3Vic2NyaXB0aW9uO1xyXG5cclxuICBjb25zdHJ1Y3RvcihcclxuICAgIHByaXZhdGUgZWxSZWY6IEVsZW1lbnRSZWYsXHJcbiAgICBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIsXHJcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcclxuICAgIEBPcHRpb25hbCgpIHByaXZhdGUgdGVtcGxhdGVSZWY6IFRlbXBsYXRlUmVmPGFueT4sXHJcbiAgICBwcml2YXRlIHZjUmVmOiBWaWV3Q29udGFpbmVyUmVmLFxyXG4gICkge31cclxuXHJcbiAgcHJpdmF0ZSBjaGVjaygpIHtcclxuICAgIGlmICh0aGlzLnN1YnNjcmlwdGlvbikge1xyXG4gICAgICB0aGlzLnN1YnNjcmlwdGlvbi51bnN1YnNjcmliZSgpO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuc3Vic2NyaXB0aW9uID0gdGhpcy5zdG9yZVxyXG4gICAgICAuc2VsZWN0KENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3kodGhpcy5jb25kaXRpb24pKVxyXG4gICAgICAucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKVxyXG4gICAgICAuc3Vic2NyaWJlKGlzR3JhbnRlZCA9PiB7XHJcbiAgICAgICAgaWYgKHRoaXMudGVtcGxhdGVSZWYgJiYgaXNHcmFudGVkKSB7XHJcbiAgICAgICAgICB0aGlzLnZjUmVmLmNsZWFyKCk7XHJcbiAgICAgICAgICB0aGlzLnZjUmVmLmNyZWF0ZUVtYmVkZGVkVmlldyh0aGlzLnRlbXBsYXRlUmVmKTtcclxuICAgICAgICB9IGVsc2UgaWYgKHRoaXMudGVtcGxhdGVSZWYgJiYgIWlzR3JhbnRlZCkge1xyXG4gICAgICAgICAgdGhpcy52Y1JlZi5jbGVhcigpO1xyXG4gICAgICAgIH0gZWxzZSBpZiAoIWlzR3JhbnRlZCAmJiAhdGhpcy50ZW1wbGF0ZVJlZikge1xyXG4gICAgICAgICAgdGhpcy5yZW5kZXJlci5yZW1vdmVDaGlsZCgodGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50IGFzIEhUTUxFbGVtZW50KS5wYXJlbnRFbGVtZW50LCB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQpO1xyXG4gICAgICAgIH1cclxuICAgICAgfSk7XHJcbiAgfVxyXG5cclxuICBuZ09uSW5pdCgpIHtcclxuICAgIGlmICh0aGlzLnRlbXBsYXRlUmVmICYmICF0aGlzLmNvbmRpdGlvbikge1xyXG4gICAgICB0aGlzLnZjUmVmLmNyZWF0ZUVtYmVkZGVkVmlldyh0aGlzLnRlbXBsYXRlUmVmKTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge31cclxuXHJcbiAgbmdPbkNoYW5nZXMoeyBjb25kaXRpb24gfTogU2ltcGxlQ2hhbmdlcykge1xyXG4gICAgaWYgKChjb25kaXRpb24gfHwgeyBjdXJyZW50VmFsdWU6IG51bGwgfSkuY3VycmVudFZhbHVlKSB7XHJcbiAgICAgIHRoaXMuY2hlY2soKTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19