/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/permission.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, Input, Renderer2, ViewContainerRef, TemplateRef, Optional, } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
export class PermissionDirective {
    /**
     * @param {?} elRef
     * @param {?} renderer
     * @param {?} store
     * @param {?} templateRef
     * @param {?} vcRef
     */
    constructor(elRef, renderer, store, templateRef, vcRef) {
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
    check() {
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
        isGranted => {
            if (this.templateRef && isGranted) {
                this.vcRef.clear();
                this.vcRef.createEmbeddedView(this.templateRef);
            }
            else if (this.templateRef && !isGranted) {
                this.vcRef.clear();
            }
            else if (!isGranted && !this.templateRef) {
                this.renderer.removeChild(((/** @type {?} */ (this.elRef.nativeElement))).parentElement, this.elRef.nativeElement);
            }
        }));
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        if (this.templateRef && !this.condition) {
            this.vcRef.createEmbeddedView(this.templateRef);
        }
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
    /**
     * @param {?} __0
     * @return {?}
     */
    ngOnChanges({ condition }) {
        if ((condition || { currentValue: null }).currentValue) {
            this.check();
        }
    }
}
PermissionDirective.decorators = [
    { type: Directive, args: [{
                selector: '[abpPermission]',
            },] }
];
/** @nocollapse */
PermissionDirective.ctorParameters = () => [
    { type: ElementRef },
    { type: Renderer2 },
    { type: Store },
    { type: TemplateRef, decorators: [{ type: Optional }] },
    { type: ViewContainerRef }
];
PermissionDirective.propDecorators = {
    condition: [{ type: Input, args: ['abpPermission',] }]
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9wZXJtaXNzaW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsVUFBVSxFQUNWLEtBQUssRUFHTCxTQUFTLEVBQ1QsZ0JBQWdCLEVBQ2hCLFdBQVcsRUFDWCxRQUFRLEdBR1QsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLFVBQVUsQ0FBQztBQU01QyxNQUFNLE9BQU8sbUJBQW1COzs7Ozs7OztJQUs5QixZQUNVLEtBQWlCLEVBQ2pCLFFBQW1CLEVBQ25CLEtBQVksRUFDQSxXQUE2QixFQUN6QyxLQUF1QjtRQUp2QixVQUFLLEdBQUwsS0FBSyxDQUFZO1FBQ2pCLGFBQVEsR0FBUixRQUFRLENBQVc7UUFDbkIsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUNBLGdCQUFXLEdBQVgsV0FBVyxDQUFrQjtRQUN6QyxVQUFLLEdBQUwsS0FBSyxDQUFrQjtJQUM5QixDQUFDOzs7OztJQUVJLEtBQUs7UUFDWCxJQUFJLElBQUksQ0FBQyxZQUFZLEVBQUU7WUFDckIsSUFBSSxDQUFDLFlBQVksQ0FBQyxXQUFXLEVBQUUsQ0FBQztTQUNqQztRQUVELElBQUksQ0FBQyxZQUFZLEdBQUcsSUFBSSxDQUFDLEtBQUs7YUFDM0IsTUFBTSxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUM7YUFDcEQsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUFDO2FBQzVCLFNBQVM7Ozs7UUFBQyxTQUFTLENBQUMsRUFBRTtZQUNyQixJQUFJLElBQUksQ0FBQyxXQUFXLElBQUksU0FBUyxFQUFFO2dCQUNqQyxJQUFJLENBQUMsS0FBSyxDQUFDLEtBQUssRUFBRSxDQUFDO2dCQUNuQixJQUFJLENBQUMsS0FBSyxDQUFDLGtCQUFrQixDQUFDLElBQUksQ0FBQyxXQUFXLENBQUMsQ0FBQzthQUNqRDtpQkFBTSxJQUFJLElBQUksQ0FBQyxXQUFXLElBQUksQ0FBQyxTQUFTLEVBQUU7Z0JBQ3pDLElBQUksQ0FBQyxLQUFLLENBQUMsS0FBSyxFQUFFLENBQUM7YUFDcEI7aUJBQU0sSUFBSSxDQUFDLFNBQVMsSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUU7Z0JBQzFDLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLENBQUMsbUJBQUEsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLEVBQWUsQ0FBQyxDQUFDLGFBQWEsRUFBRSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQyxDQUFDO2FBQzlHO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsUUFBUTtRQUNOLElBQUksSUFBSSxDQUFDLFdBQVcsSUFBSSxDQUFDLElBQUksQ0FBQyxTQUFTLEVBQUU7WUFDdkMsSUFBSSxDQUFDLEtBQUssQ0FBQyxrQkFBa0IsQ0FBQyxJQUFJLENBQUMsV0FBVyxDQUFDLENBQUM7U0FDakQ7SUFDSCxDQUFDOzs7O0lBRUQsV0FBVyxLQUFVLENBQUM7Ozs7O0lBRXRCLFdBQVcsQ0FBQyxFQUFFLFNBQVMsRUFBaUI7UUFDdEMsSUFBSSxDQUFDLFNBQVMsSUFBSSxFQUFFLFlBQVksRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDLFlBQVksRUFBRTtZQUN0RCxJQUFJLENBQUMsS0FBSyxFQUFFLENBQUM7U0FDZDtJQUNILENBQUM7OztZQWhERixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGlCQUFpQjthQUM1Qjs7OztZQWxCQyxVQUFVO1lBSVYsU0FBUztZQU9GLEtBQUs7WUFMWixXQUFXLHVCQXNCUixRQUFRO1lBdkJYLGdCQUFnQjs7O3dCQWVmLEtBQUssU0FBQyxlQUFlOzs7O0lBQXRCLHdDQUEwQzs7SUFFMUMsMkNBQTJCOzs7OztJQUd6QixvQ0FBeUI7Ozs7O0lBQ3pCLHVDQUEyQjs7Ozs7SUFDM0Isb0NBQW9COzs7OztJQUNwQiwwQ0FBaUQ7Ozs7O0lBQ2pELG9DQUErQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIERpcmVjdGl2ZSxcbiAgRWxlbWVudFJlZixcbiAgSW5wdXQsXG4gIE9uRGVzdHJveSxcbiAgT25Jbml0LFxuICBSZW5kZXJlcjIsXG4gIFZpZXdDb250YWluZXJSZWYsXG4gIFRlbXBsYXRlUmVmLFxuICBPcHRpb25hbCxcbiAgU2ltcGxlQ2hhbmdlcyxcbiAgT25DaGFuZ2VzLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzJztcbmltcG9ydCB7IFN1YnNjcmlwdGlvbiB9IGZyb20gJ3J4anMnO1xuXG5ARGlyZWN0aXZlKHtcbiAgc2VsZWN0b3I6ICdbYWJwUGVybWlzc2lvbl0nLFxufSlcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uRGlyZWN0aXZlIGltcGxlbWVudHMgT25Jbml0LCBPbkRlc3Ryb3ksIE9uQ2hhbmdlcyB7XG4gIEBJbnB1dCgnYWJwUGVybWlzc2lvbicpIGNvbmRpdGlvbjogc3RyaW5nO1xuXG4gIHN1YnNjcmlwdGlvbjogU3Vic2NyaXB0aW9uO1xuXG4gIGNvbnN0cnVjdG9yKFxuICAgIHByaXZhdGUgZWxSZWY6IEVsZW1lbnRSZWYsXG4gICAgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyLFxuICAgIHByaXZhdGUgc3RvcmU6IFN0b3JlLFxuICAgIEBPcHRpb25hbCgpIHByaXZhdGUgdGVtcGxhdGVSZWY6IFRlbXBsYXRlUmVmPGFueT4sXG4gICAgcHJpdmF0ZSB2Y1JlZjogVmlld0NvbnRhaW5lclJlZixcbiAgKSB7fVxuXG4gIHByaXZhdGUgY2hlY2soKSB7XG4gICAgaWYgKHRoaXMuc3Vic2NyaXB0aW9uKSB7XG4gICAgICB0aGlzLnN1YnNjcmlwdGlvbi51bnN1YnNjcmliZSgpO1xuICAgIH1cblxuICAgIHRoaXMuc3Vic2NyaXB0aW9uID0gdGhpcy5zdG9yZVxuICAgICAgLnNlbGVjdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KHRoaXMuY29uZGl0aW9uKSlcbiAgICAgIC5waXBlKHRha2VVbnRpbERlc3Ryb3kodGhpcykpXG4gICAgICAuc3Vic2NyaWJlKGlzR3JhbnRlZCA9PiB7XG4gICAgICAgIGlmICh0aGlzLnRlbXBsYXRlUmVmICYmIGlzR3JhbnRlZCkge1xuICAgICAgICAgIHRoaXMudmNSZWYuY2xlYXIoKTtcbiAgICAgICAgICB0aGlzLnZjUmVmLmNyZWF0ZUVtYmVkZGVkVmlldyh0aGlzLnRlbXBsYXRlUmVmKTtcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnRlbXBsYXRlUmVmICYmICFpc0dyYW50ZWQpIHtcbiAgICAgICAgICB0aGlzLnZjUmVmLmNsZWFyKCk7XG4gICAgICAgIH0gZWxzZSBpZiAoIWlzR3JhbnRlZCAmJiAhdGhpcy50ZW1wbGF0ZVJlZikge1xuICAgICAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlQ2hpbGQoKHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudCBhcyBIVE1MRWxlbWVudCkucGFyZW50RWxlbWVudCwgdGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50KTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBuZ09uSW5pdCgpIHtcbiAgICBpZiAodGhpcy50ZW1wbGF0ZVJlZiAmJiAhdGhpcy5jb25kaXRpb24pIHtcbiAgICAgIHRoaXMudmNSZWYuY3JlYXRlRW1iZWRkZWRWaWV3KHRoaXMudGVtcGxhdGVSZWYpO1xuICAgIH1cbiAgfVxuXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge31cblxuICBuZ09uQ2hhbmdlcyh7IGNvbmRpdGlvbiB9OiBTaW1wbGVDaGFuZ2VzKSB7XG4gICAgaWYgKChjb25kaXRpb24gfHwgeyBjdXJyZW50VmFsdWU6IG51bGwgfSkuY3VycmVudFZhbHVlKSB7XG4gICAgICB0aGlzLmNoZWNrKCk7XG4gICAgfVxuICB9XG59XG4iXX0=