/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, Input, Optional, Renderer2 } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
export class PermissionDirective {
    /**
     * @param {?} elRef
     * @param {?} renderer
     * @param {?} store
     */
    constructor(elRef, renderer, store) {
        this.elRef = elRef;
        this.renderer = renderer;
        this.store = store;
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        if (this.condition) {
            this.store
                .select(ConfigState.getGrantedPolicy(this.condition))
                .pipe(takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} isGranted
             * @return {?}
             */
            isGranted => {
                if (!isGranted) {
                    this.renderer.removeChild(((/** @type {?} */ (this.elRef.nativeElement))).parentElement, this.elRef.nativeElement);
                }
            }));
        }
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
}
PermissionDirective.decorators = [
    { type: Directive, args: [{
                selector: '[abpPermission]',
            },] }
];
/** @nocollapse */
PermissionDirective.ctorParameters = () => [
    { type: ElementRef, decorators: [{ type: Optional }] },
    { type: Renderer2 },
    { type: Store }
];
PermissionDirective.propDecorators = {
    condition: [{ type: Input, args: ['abpPermission',] }]
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9wZXJtaXNzaW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsS0FBSyxFQUFxQixRQUFRLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3JHLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUN4QyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxVQUFVLENBQUM7QUFLNUMsTUFBTSxPQUFPLG1CQUFtQjs7Ozs7O0lBRzlCLFlBQWdDLEtBQWlCLEVBQVUsUUFBbUIsRUFBVSxLQUFZO1FBQXBFLFVBQUssR0FBTCxLQUFLLENBQVk7UUFBVSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFeEcsUUFBUTtRQUNOLElBQUksSUFBSSxDQUFDLFNBQVMsRUFBRTtZQUNsQixJQUFJLENBQUMsS0FBSztpQkFDUCxNQUFNLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxTQUFTLENBQUMsQ0FBQztpQkFDcEQsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUFDO2lCQUM1QixTQUFTOzs7O1lBQUMsU0FBUyxDQUFDLEVBQUU7Z0JBQ3JCLElBQUksQ0FBQyxTQUFTLEVBQUU7b0JBQ2QsSUFBSSxDQUFDLFFBQVEsQ0FBQyxXQUFXLENBQ3ZCLENBQUMsbUJBQUEsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLEVBQWUsQ0FBQyxDQUFDLGFBQWEsRUFDdkQsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQ3pCLENBQUM7aUJBQ0g7WUFDSCxDQUFDLEVBQUMsQ0FBQztTQUNOO0lBQ0gsQ0FBQzs7OztJQUVELFdBQVcsS0FBVSxDQUFDOzs7WUF4QnZCLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsaUJBQWlCO2FBQzVCOzs7O1lBUG1CLFVBQVUsdUJBV2YsUUFBUTtZQVg2QyxTQUFTO1lBQ3BFLEtBQUs7Ozt3QkFRWCxLQUFLLFNBQUMsZUFBZTs7OztJQUF0Qix3Q0FBMEM7Ozs7O0lBRTlCLG9DQUFxQzs7Ozs7SUFBRSx1Q0FBMkI7Ozs7O0lBQUUsb0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBFbGVtZW50UmVmLCBJbnB1dCwgT25EZXN0cm95LCBPbkluaXQsIE9wdGlvbmFsLCBSZW5kZXJlcjIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzJztcblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicFBlcm1pc3Npb25dJyxcbn0pXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbkRpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uSW5pdCwgT25EZXN0cm95IHtcbiAgQElucHV0KCdhYnBQZXJtaXNzaW9uJykgY29uZGl0aW9uOiBzdHJpbmc7XG5cbiAgY29uc3RydWN0b3IoQE9wdGlvbmFsKCkgcHJpdmF0ZSBlbFJlZjogRWxlbWVudFJlZiwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICBpZiAodGhpcy5jb25kaXRpb24pIHtcbiAgICAgIHRoaXMuc3RvcmVcbiAgICAgICAgLnNlbGVjdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KHRoaXMuY29uZGl0aW9uKSlcbiAgICAgICAgLnBpcGUodGFrZVVudGlsRGVzdHJveSh0aGlzKSlcbiAgICAgICAgLnN1YnNjcmliZShpc0dyYW50ZWQgPT4ge1xuICAgICAgICAgIGlmICghaXNHcmFudGVkKSB7XG4gICAgICAgICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNoaWxkKFxuICAgICAgICAgICAgICAodGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50IGFzIEhUTUxFbGVtZW50KS5wYXJlbnRFbGVtZW50LFxuICAgICAgICAgICAgICB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQsXG4gICAgICAgICAgICApO1xuICAgICAgICAgIH1cbiAgICAgICAgfSk7XG4gICAgfVxuICB9XG5cbiAgbmdPbkRlc3Ryb3koKTogdm9pZCB7fVxufVxuIl19