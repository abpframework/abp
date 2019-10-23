/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9wZXJtaXNzaW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsS0FBSyxFQUFxQixRQUFRLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3JHLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUN4QyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxVQUFVLENBQUM7QUFFNUM7SUFNRSw2QkFBZ0MsS0FBaUIsRUFBVSxRQUFtQixFQUFVLEtBQVk7UUFBcEUsVUFBSyxHQUFMLEtBQUssQ0FBWTtRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7OztJQUV4RyxzQ0FBUTs7O0lBQVI7UUFBQSxpQkFjQztRQWJDLElBQUksSUFBSSxDQUFDLFNBQVMsRUFBRTtZQUNsQixJQUFJLENBQUMsS0FBSztpQkFDUCxNQUFNLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxTQUFTLENBQUMsQ0FBQztpQkFDcEQsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUFDO2lCQUM1QixTQUFTOzs7O1lBQUMsVUFBQSxTQUFTO2dCQUNsQixJQUFJLENBQUMsU0FBUyxFQUFFO29CQUNkLEtBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUN2QixDQUFDLG1CQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFlLENBQUMsQ0FBQyxhQUFhLEVBQ3ZELEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxDQUN6QixDQUFDO2lCQUNIO1lBQ0gsQ0FBQyxFQUFDLENBQUM7U0FDTjtJQUNILENBQUM7Ozs7SUFFRCx5Q0FBVzs7O0lBQVgsY0FBcUIsQ0FBQzs7Z0JBeEJ2QixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGlCQUFpQjtpQkFDNUI7Ozs7Z0JBUG1CLFVBQVUsdUJBV2YsUUFBUTtnQkFYNkMsU0FBUztnQkFDcEUsS0FBSzs7OzRCQVFYLEtBQUssU0FBQyxlQUFlOztJQXFCeEIsMEJBQUM7Q0FBQSxBQXpCRCxJQXlCQztTQXRCWSxtQkFBbUI7OztJQUM5Qix3Q0FBMEM7Ozs7O0lBRTlCLG9DQUFxQzs7Ozs7SUFBRSx1Q0FBMkI7Ozs7O0lBQUUsb0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBFbGVtZW50UmVmLCBJbnB1dCwgT25EZXN0cm95LCBPbkluaXQsIE9wdGlvbmFsLCBSZW5kZXJlcjIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcclxuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzJztcclxuXHJcbkBEaXJlY3RpdmUoe1xyXG4gIHNlbGVjdG9yOiAnW2FicFBlcm1pc3Npb25dJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25EaXJlY3RpdmUgaW1wbGVtZW50cyBPbkluaXQsIE9uRGVzdHJveSB7XHJcbiAgQElucHV0KCdhYnBQZXJtaXNzaW9uJykgY29uZGl0aW9uOiBzdHJpbmc7XHJcblxyXG4gIGNvbnN0cnVjdG9yKEBPcHRpb25hbCgpIHByaXZhdGUgZWxSZWY6IEVsZW1lbnRSZWYsIHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMiwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XHJcblxyXG4gIG5nT25Jbml0KCkge1xyXG4gICAgaWYgKHRoaXMuY29uZGl0aW9uKSB7XHJcbiAgICAgIHRoaXMuc3RvcmVcclxuICAgICAgICAuc2VsZWN0KENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3kodGhpcy5jb25kaXRpb24pKVxyXG4gICAgICAgIC5waXBlKHRha2VVbnRpbERlc3Ryb3kodGhpcykpXHJcbiAgICAgICAgLnN1YnNjcmliZShpc0dyYW50ZWQgPT4ge1xyXG4gICAgICAgICAgaWYgKCFpc0dyYW50ZWQpIHtcclxuICAgICAgICAgICAgdGhpcy5yZW5kZXJlci5yZW1vdmVDaGlsZChcclxuICAgICAgICAgICAgICAodGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50IGFzIEhUTUxFbGVtZW50KS5wYXJlbnRFbGVtZW50LFxyXG4gICAgICAgICAgICAgIHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudCxcclxuICAgICAgICAgICAgKTtcclxuICAgICAgICAgIH1cclxuICAgICAgICB9KTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge31cclxufVxyXG4iXX0=