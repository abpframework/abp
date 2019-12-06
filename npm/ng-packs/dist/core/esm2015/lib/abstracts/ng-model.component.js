/**
 * @fileoverview added by tsickle
 * Generated from: lib/abstracts/ng-model.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Component, Injector, Input } from '@angular/core';
/**
 * @template T
 */
export class AbstractNgModelComponent {
    /**
     * @param {?} injector
     */
    constructor(injector) {
        this.injector = injector;
        this.cdRef = injector.get((/** @type {?} */ (ChangeDetectorRef)));
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set value(value) {
        this._value = value;
        this.notifyValueChange();
    }
    /**
     * @return {?}
     */
    get value() {
        return this._value;
    }
    /**
     * @return {?}
     */
    notifyValueChange() {
        if (this.onChange) {
            this.onChange(this.value);
        }
    }
    /**
     * @param {?} value
     * @return {?}
     */
    writeValue(value) {
        this._value = value;
        setTimeout((/**
         * @return {?}
         */
        () => this.cdRef.detectChanges()), 0);
    }
    /**
     * @param {?} fn
     * @return {?}
     */
    registerOnChange(fn) {
        this.onChange = fn;
    }
    /**
     * @param {?} fn
     * @return {?}
     */
    registerOnTouched(fn) {
        this.onTouched = fn;
    }
    /**
     * @param {?} isDisabled
     * @return {?}
     */
    setDisabledState(isDisabled) {
        this.disabled = isDisabled;
    }
}
AbstractNgModelComponent.decorators = [
    { type: Component, args: [{ selector: 'abp-abstract-ng-model', template: '' }] }
];
/** @nocollapse */
AbstractNgModelComponent.ctorParameters = () => [
    { type: Injector }
];
AbstractNgModelComponent.propDecorators = {
    disabled: [{ type: Input }],
    value: [{ type: Input }]
};
if (false) {
    /** @type {?} */
    AbstractNgModelComponent.prototype.disabled;
    /** @type {?} */
    AbstractNgModelComponent.prototype.onChange;
    /** @type {?} */
    AbstractNgModelComponent.prototype.onTouched;
    /**
     * @type {?}
     * @protected
     */
    AbstractNgModelComponent.prototype._value;
    /**
     * @type {?}
     * @protected
     */
    AbstractNgModelComponent.prototype.cdRef;
    /** @type {?} */
    AbstractNgModelComponent.prototype.injector;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmctbW9kZWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy9uZy1tb2RlbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFDQSxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsU0FBUyxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQVEsTUFBTSxlQUFlLENBQUM7Ozs7QUFHcEYsTUFBTSxPQUFPLHdCQUF3Qjs7OztJQWtCbkMsWUFBbUIsUUFBa0I7UUFBbEIsYUFBUSxHQUFSLFFBQVEsQ0FBVTtRQUNuQyxJQUFJLENBQUMsS0FBSyxHQUFHLFFBQVEsQ0FBQyxHQUFHLENBQW9CLG1CQUFBLGlCQUFpQixFQUEyQixDQUFDLENBQUM7SUFDN0YsQ0FBQzs7Ozs7SUFqQkQsSUFBYSxLQUFLLENBQUMsS0FBUTtRQUN6QixJQUFJLENBQUMsTUFBTSxHQUFHLEtBQUssQ0FBQztRQUNwQixJQUFJLENBQUMsaUJBQWlCLEVBQUUsQ0FBQztJQUMzQixDQUFDOzs7O0lBRUQsSUFBSSxLQUFLO1FBQ1AsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDO0lBQ3JCLENBQUM7Ozs7SUFZRCxpQkFBaUI7UUFDZixJQUFJLElBQUksQ0FBQyxRQUFRLEVBQUU7WUFDakIsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7U0FDM0I7SUFDSCxDQUFDOzs7OztJQUVELFVBQVUsQ0FBQyxLQUFRO1FBQ2pCLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1FBQ3BCLFVBQVU7OztRQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLEdBQUUsQ0FBQyxDQUFDLENBQUM7SUFDbEQsQ0FBQzs7Ozs7SUFFRCxnQkFBZ0IsQ0FBQyxFQUFPO1FBQ3RCLElBQUksQ0FBQyxRQUFRLEdBQUcsRUFBRSxDQUFDO0lBQ3JCLENBQUM7Ozs7O0lBRUQsaUJBQWlCLENBQUMsRUFBTztRQUN2QixJQUFJLENBQUMsU0FBUyxHQUFHLEVBQUUsQ0FBQztJQUN0QixDQUFDOzs7OztJQUVELGdCQUFnQixDQUFDLFVBQW1CO1FBQ2xDLElBQUksQ0FBQyxRQUFRLEdBQUcsVUFBVSxDQUFDO0lBQzdCLENBQUM7OztZQTVDRixTQUFTLFNBQUMsRUFBRSxRQUFRLEVBQUUsdUJBQXVCLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRTs7OztZQUZ2QixRQUFROzs7dUJBSTVDLEtBQUs7b0JBRUwsS0FBSzs7OztJQUZOLDRDQUEyQjs7SUFXM0IsNENBQTJCOztJQUMzQiw2Q0FBb0I7Ozs7O0lBRXBCLDBDQUFvQjs7Ozs7SUFDcEIseUNBQW1DOztJQUV2Qiw0Q0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb250cm9sVmFsdWVBY2Nlc3NvciB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IENoYW5nZURldGVjdG9yUmVmLCBDb21wb25lbnQsIEluamVjdG9yLCBJbnB1dCwgVHlwZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHsgc2VsZWN0b3I6ICdhYnAtYWJzdHJhY3QtbmctbW9kZWwnLCB0ZW1wbGF0ZTogJycgfSlcbmV4cG9ydCBjbGFzcyBBYnN0cmFjdE5nTW9kZWxDb21wb25lbnQ8VCA9IGFueT4gaW1wbGVtZW50cyBDb250cm9sVmFsdWVBY2Nlc3NvciB7XG4gIEBJbnB1dCgpIGRpc2FibGVkOiBib29sZWFuO1xuXG4gIEBJbnB1dCgpIHNldCB2YWx1ZSh2YWx1ZTogVCkge1xuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XG4gICAgdGhpcy5ub3RpZnlWYWx1ZUNoYW5nZSgpO1xuICB9XG5cbiAgZ2V0IHZhbHVlKCk6IFQge1xuICAgIHJldHVybiB0aGlzLl92YWx1ZTtcbiAgfVxuXG4gIG9uQ2hhbmdlOiAodmFsdWU6IFQpID0+IHt9O1xuICBvblRvdWNoZWQ6ICgpID0+IHt9O1xuXG4gIHByb3RlY3RlZCBfdmFsdWU6IFQ7XG4gIHByb3RlY3RlZCBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWY7XG5cbiAgY29uc3RydWN0b3IocHVibGljIGluamVjdG9yOiBJbmplY3Rvcikge1xuICAgIHRoaXMuY2RSZWYgPSBpbmplY3Rvci5nZXQ8Q2hhbmdlRGV0ZWN0b3JSZWY+KENoYW5nZURldGVjdG9yUmVmIGFzIFR5cGU8Q2hhbmdlRGV0ZWN0b3JSZWY+KTtcbiAgfVxuXG4gIG5vdGlmeVZhbHVlQ2hhbmdlKCk6IHZvaWQge1xuICAgIGlmICh0aGlzLm9uQ2hhbmdlKSB7XG4gICAgICB0aGlzLm9uQ2hhbmdlKHRoaXMudmFsdWUpO1xuICAgIH1cbiAgfVxuXG4gIHdyaXRlVmFsdWUodmFsdWU6IFQpOiB2b2lkIHtcbiAgICB0aGlzLl92YWx1ZSA9IHZhbHVlO1xuICAgIHNldFRpbWVvdXQoKCkgPT4gdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCksIDApO1xuICB9XG5cbiAgcmVnaXN0ZXJPbkNoYW5nZShmbjogYW55KTogdm9pZCB7XG4gICAgdGhpcy5vbkNoYW5nZSA9IGZuO1xuICB9XG5cbiAgcmVnaXN0ZXJPblRvdWNoZWQoZm46IGFueSk6IHZvaWQge1xuICAgIHRoaXMub25Ub3VjaGVkID0gZm47XG4gIH1cblxuICBzZXREaXNhYmxlZFN0YXRlKGlzRGlzYWJsZWQ6IGJvb2xlYW4pOiB2b2lkIHtcbiAgICB0aGlzLmRpc2FibGVkID0gaXNEaXNhYmxlZDtcbiAgfVxufVxuIl19