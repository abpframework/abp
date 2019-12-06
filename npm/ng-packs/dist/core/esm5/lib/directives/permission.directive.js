/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/permission.directive.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy9wZXJtaXNzaW9uLmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsVUFBVSxFQUNWLEtBQUssRUFHTCxTQUFTLEVBQ1QsZ0JBQWdCLEVBQ2hCLFdBQVcsRUFDWCxRQUFRLEdBR1QsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLFVBQVUsQ0FBQztBQUc1QztJQVFFLDZCQUNVLEtBQWlCLEVBQ2pCLFFBQW1CLEVBQ25CLEtBQVksRUFDQSxXQUE2QixFQUN6QyxLQUF1QjtRQUp2QixVQUFLLEdBQUwsS0FBSyxDQUFZO1FBQ2pCLGFBQVEsR0FBUixRQUFRLENBQVc7UUFDbkIsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUNBLGdCQUFXLEdBQVgsV0FBVyxDQUFrQjtRQUN6QyxVQUFLLEdBQUwsS0FBSyxDQUFrQjtJQUM5QixDQUFDOzs7OztJQUVJLG1DQUFLOzs7O0lBQWI7UUFBQSxpQkFrQkM7UUFqQkMsSUFBSSxJQUFJLENBQUMsWUFBWSxFQUFFO1lBQ3JCLElBQUksQ0FBQyxZQUFZLENBQUMsV0FBVyxFQUFFLENBQUM7U0FDakM7UUFFRCxJQUFJLENBQUMsWUFBWSxHQUFHLElBQUksQ0FBQyxLQUFLO2FBQzNCLE1BQU0sQ0FBQyxXQUFXLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxDQUFDO2FBQ3BELElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FBQzthQUM1QixTQUFTOzs7O1FBQUMsVUFBQSxTQUFTO1lBQ2xCLElBQUksS0FBSSxDQUFDLFdBQVcsSUFBSSxTQUFTLEVBQUU7Z0JBQ2pDLEtBQUksQ0FBQyxLQUFLLENBQUMsS0FBSyxFQUFFLENBQUM7Z0JBQ25CLEtBQUksQ0FBQyxLQUFLLENBQUMsa0JBQWtCLENBQUMsS0FBSSxDQUFDLFdBQVcsQ0FBQyxDQUFDO2FBQ2pEO2lCQUFNLElBQUksS0FBSSxDQUFDLFdBQVcsSUFBSSxDQUFDLFNBQVMsRUFBRTtnQkFDekMsS0FBSSxDQUFDLEtBQUssQ0FBQyxLQUFLLEVBQUUsQ0FBQzthQUNwQjtpQkFBTSxJQUFJLENBQUMsU0FBUyxJQUFJLENBQUMsS0FBSSxDQUFDLFdBQVcsRUFBRTtnQkFDMUMsS0FBSSxDQUFDLFFBQVEsQ0FBQyxXQUFXLENBQUMsQ0FBQyxtQkFBQSxLQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBZSxDQUFDLENBQUMsYUFBYSxFQUFFLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxDQUFDLENBQUM7YUFDOUc7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCxzQ0FBUTs7O0lBQVI7UUFDRSxJQUFJLElBQUksQ0FBQyxXQUFXLElBQUksQ0FBQyxJQUFJLENBQUMsU0FBUyxFQUFFO1lBQ3ZDLElBQUksQ0FBQyxLQUFLLENBQUMsa0JBQWtCLENBQUMsSUFBSSxDQUFDLFdBQVcsQ0FBQyxDQUFDO1NBQ2pEO0lBQ0gsQ0FBQzs7OztJQUVELHlDQUFXOzs7SUFBWCxjQUFxQixDQUFDOzs7OztJQUV0Qix5Q0FBVzs7OztJQUFYLFVBQVksRUFBNEI7WUFBMUIsd0JBQVM7UUFDckIsSUFBSSxDQUFDLFNBQVMsSUFBSSxFQUFFLFlBQVksRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDLFlBQVksRUFBRTtZQUN0RCxJQUFJLENBQUMsS0FBSyxFQUFFLENBQUM7U0FDZDtJQUNILENBQUM7O2dCQWhERixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGlCQUFpQjtpQkFDNUI7Ozs7Z0JBbEJDLFVBQVU7Z0JBSVYsU0FBUztnQkFPRixLQUFLO2dCQUxaLFdBQVcsdUJBc0JSLFFBQVE7Z0JBdkJYLGdCQUFnQjs7OzRCQWVmLEtBQUssU0FBQyxlQUFlOztJQTZDeEIsMEJBQUM7Q0FBQSxBQWpERCxJQWlEQztTQTlDWSxtQkFBbUI7OztJQUM5Qix3Q0FBMEM7O0lBRTFDLDJDQUEyQjs7Ozs7SUFHekIsb0NBQXlCOzs7OztJQUN6Qix1Q0FBMkI7Ozs7O0lBQzNCLG9DQUFvQjs7Ozs7SUFDcEIsMENBQWlEOzs7OztJQUNqRCxvQ0FBK0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBEaXJlY3RpdmUsXG4gIEVsZW1lbnRSZWYsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE9uSW5pdCxcbiAgUmVuZGVyZXIyLFxuICBWaWV3Q29udGFpbmVyUmVmLFxuICBUZW1wbGF0ZVJlZixcbiAgT3B0aW9uYWwsXG4gIFNpbXBsZUNoYW5nZXMsXG4gIE9uQ2hhbmdlcyxcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICcuLi91dGlscyc7XG5pbXBvcnQgeyBTdWJzY3JpcHRpb24gfSBmcm9tICdyeGpzJztcblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicFBlcm1pc3Npb25dJyxcbn0pXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbkRpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uSW5pdCwgT25EZXN0cm95LCBPbkNoYW5nZXMge1xuICBASW5wdXQoJ2FicFBlcm1pc3Npb24nKSBjb25kaXRpb246IHN0cmluZztcblxuICBzdWJzY3JpcHRpb246IFN1YnNjcmlwdGlvbjtcblxuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmLFxuICAgIHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMixcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcbiAgICBAT3B0aW9uYWwoKSBwcml2YXRlIHRlbXBsYXRlUmVmOiBUZW1wbGF0ZVJlZjxhbnk+LFxuICAgIHByaXZhdGUgdmNSZWY6IFZpZXdDb250YWluZXJSZWYsXG4gICkge31cblxuICBwcml2YXRlIGNoZWNrKCkge1xuICAgIGlmICh0aGlzLnN1YnNjcmlwdGlvbikge1xuICAgICAgdGhpcy5zdWJzY3JpcHRpb24udW5zdWJzY3JpYmUoKTtcbiAgICB9XG5cbiAgICB0aGlzLnN1YnNjcmlwdGlvbiA9IHRoaXMuc3RvcmVcbiAgICAgIC5zZWxlY3QoQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeSh0aGlzLmNvbmRpdGlvbikpXG4gICAgICAucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKVxuICAgICAgLnN1YnNjcmliZShpc0dyYW50ZWQgPT4ge1xuICAgICAgICBpZiAodGhpcy50ZW1wbGF0ZVJlZiAmJiBpc0dyYW50ZWQpIHtcbiAgICAgICAgICB0aGlzLnZjUmVmLmNsZWFyKCk7XG4gICAgICAgICAgdGhpcy52Y1JlZi5jcmVhdGVFbWJlZGRlZFZpZXcodGhpcy50ZW1wbGF0ZVJlZik7XG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy50ZW1wbGF0ZVJlZiAmJiAhaXNHcmFudGVkKSB7XG4gICAgICAgICAgdGhpcy52Y1JlZi5jbGVhcigpO1xuICAgICAgICB9IGVsc2UgaWYgKCFpc0dyYW50ZWQgJiYgIXRoaXMudGVtcGxhdGVSZWYpIHtcbiAgICAgICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNoaWxkKCh0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQgYXMgSFRNTEVsZW1lbnQpLnBhcmVudEVsZW1lbnQsIHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudCk7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgaWYgKHRoaXMudGVtcGxhdGVSZWYgJiYgIXRoaXMuY29uZGl0aW9uKSB7XG4gICAgICB0aGlzLnZjUmVmLmNyZWF0ZUVtYmVkZGVkVmlldyh0aGlzLnRlbXBsYXRlUmVmKTtcbiAgICB9XG4gIH1cblxuICBuZ09uRGVzdHJveSgpOiB2b2lkIHt9XG5cbiAgbmdPbkNoYW5nZXMoeyBjb25kaXRpb24gfTogU2ltcGxlQ2hhbmdlcykge1xuICAgIGlmICgoY29uZGl0aW9uIHx8IHsgY3VycmVudFZhbHVlOiBudWxsIH0pLmN1cnJlbnRWYWx1ZSkge1xuICAgICAgdGhpcy5jaGVjaygpO1xuICAgIH1cbiAgfVxufVxuIl19