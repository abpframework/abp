/**
 * @fileoverview added by tsickle
 * Generated from: lib/abstracts/ng-model.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Component, Injector, Input } from '@angular/core';
/**
 * @template T
 */
var AbstractNgModelComponent = /** @class */ (function () {
    function AbstractNgModelComponent(injector) {
        this.injector = injector;
        this.cdRef = injector.get((/** @type {?} */ (ChangeDetectorRef)));
    }
    Object.defineProperty(AbstractNgModelComponent.prototype, "value", {
        get: /**
         * @return {?}
         */
        function () {
            return this._value;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this._value = value;
            this.notifyValueChange();
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    AbstractNgModelComponent.prototype.notifyValueChange = /**
     * @return {?}
     */
    function () {
        if (this.onChange) {
            this.onChange(this.value);
        }
    };
    /**
     * @param {?} value
     * @return {?}
     */
    AbstractNgModelComponent.prototype.writeValue = /**
     * @param {?} value
     * @return {?}
     */
    function (value) {
        var _this = this;
        this._value = value;
        setTimeout((/**
         * @return {?}
         */
        function () { return _this.cdRef.detectChanges(); }), 0);
    };
    /**
     * @param {?} fn
     * @return {?}
     */
    AbstractNgModelComponent.prototype.registerOnChange = /**
     * @param {?} fn
     * @return {?}
     */
    function (fn) {
        this.onChange = fn;
    };
    /**
     * @param {?} fn
     * @return {?}
     */
    AbstractNgModelComponent.prototype.registerOnTouched = /**
     * @param {?} fn
     * @return {?}
     */
    function (fn) {
        this.onTouched = fn;
    };
    /**
     * @param {?} isDisabled
     * @return {?}
     */
    AbstractNgModelComponent.prototype.setDisabledState = /**
     * @param {?} isDisabled
     * @return {?}
     */
    function (isDisabled) {
        this.disabled = isDisabled;
    };
    AbstractNgModelComponent.decorators = [
        { type: Component, args: [{ selector: 'abp-abstract-ng-model', template: '' }] }
    ];
    /** @nocollapse */
    AbstractNgModelComponent.ctorParameters = function () { return [
        { type: Injector }
    ]; };
    AbstractNgModelComponent.propDecorators = {
        disabled: [{ type: Input }],
        value: [{ type: Input }]
    };
    return AbstractNgModelComponent;
}());
export { AbstractNgModelComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmctbW9kZWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy9uZy1tb2RlbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFDQSxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsU0FBUyxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQVEsTUFBTSxlQUFlLENBQUM7Ozs7QUFFcEY7SUFtQkUsa0NBQW1CLFFBQWtCO1FBQWxCLGFBQVEsR0FBUixRQUFRLENBQVU7UUFDbkMsSUFBSSxDQUFDLEtBQUssR0FBRyxRQUFRLENBQUMsR0FBRyxDQUFvQixtQkFBQSxpQkFBaUIsRUFBMkIsQ0FBQyxDQUFDO0lBQzdGLENBQUM7SUFqQkQsc0JBQWEsMkNBQUs7Ozs7UUFLbEI7WUFDRSxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUM7UUFDckIsQ0FBQzs7Ozs7UUFQRCxVQUFtQixLQUFRO1lBQ3pCLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1lBQ3BCLElBQUksQ0FBQyxpQkFBaUIsRUFBRSxDQUFDO1FBQzNCLENBQUM7OztPQUFBOzs7O0lBZ0JELG9EQUFpQjs7O0lBQWpCO1FBQ0UsSUFBSSxJQUFJLENBQUMsUUFBUSxFQUFFO1lBQ2pCLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1NBQzNCO0lBQ0gsQ0FBQzs7Ozs7SUFFRCw2Q0FBVTs7OztJQUFWLFVBQVcsS0FBUTtRQUFuQixpQkFHQztRQUZDLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1FBQ3BCLFVBQVU7OztRQUFDLGNBQU0sT0FBQSxLQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBRSxFQUExQixDQUEwQixHQUFFLENBQUMsQ0FBQyxDQUFDO0lBQ2xELENBQUM7Ozs7O0lBRUQsbURBQWdCOzs7O0lBQWhCLFVBQWlCLEVBQU87UUFDdEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxFQUFFLENBQUM7SUFDckIsQ0FBQzs7Ozs7SUFFRCxvREFBaUI7Ozs7SUFBakIsVUFBa0IsRUFBTztRQUN2QixJQUFJLENBQUMsU0FBUyxHQUFHLEVBQUUsQ0FBQztJQUN0QixDQUFDOzs7OztJQUVELG1EQUFnQjs7OztJQUFoQixVQUFpQixVQUFtQjtRQUNsQyxJQUFJLENBQUMsUUFBUSxHQUFHLFVBQVUsQ0FBQztJQUM3QixDQUFDOztnQkE1Q0YsU0FBUyxTQUFDLEVBQUUsUUFBUSxFQUFFLHVCQUF1QixFQUFFLFFBQVEsRUFBRSxFQUFFLEVBQUU7Ozs7Z0JBRnZCLFFBQVE7OzsyQkFJNUMsS0FBSzt3QkFFTCxLQUFLOztJQXlDUiwrQkFBQztDQUFBLEFBN0NELElBNkNDO1NBNUNZLHdCQUF3Qjs7O0lBQ25DLDRDQUEyQjs7SUFXM0IsNENBQTJCOztJQUMzQiw2Q0FBb0I7Ozs7O0lBRXBCLDBDQUFvQjs7Ozs7SUFDcEIseUNBQW1DOztJQUV2Qiw0Q0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb250cm9sVmFsdWVBY2Nlc3NvciB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IENoYW5nZURldGVjdG9yUmVmLCBDb21wb25lbnQsIEluamVjdG9yLCBJbnB1dCwgVHlwZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHsgc2VsZWN0b3I6ICdhYnAtYWJzdHJhY3QtbmctbW9kZWwnLCB0ZW1wbGF0ZTogJycgfSlcbmV4cG9ydCBjbGFzcyBBYnN0cmFjdE5nTW9kZWxDb21wb25lbnQ8VCA9IGFueT4gaW1wbGVtZW50cyBDb250cm9sVmFsdWVBY2Nlc3NvciB7XG4gIEBJbnB1dCgpIGRpc2FibGVkOiBib29sZWFuO1xuXG4gIEBJbnB1dCgpIHNldCB2YWx1ZSh2YWx1ZTogVCkge1xuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XG4gICAgdGhpcy5ub3RpZnlWYWx1ZUNoYW5nZSgpO1xuICB9XG5cbiAgZ2V0IHZhbHVlKCk6IFQge1xuICAgIHJldHVybiB0aGlzLl92YWx1ZTtcbiAgfVxuXG4gIG9uQ2hhbmdlOiAodmFsdWU6IFQpID0+IHt9O1xuICBvblRvdWNoZWQ6ICgpID0+IHt9O1xuXG4gIHByb3RlY3RlZCBfdmFsdWU6IFQ7XG4gIHByb3RlY3RlZCBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWY7XG5cbiAgY29uc3RydWN0b3IocHVibGljIGluamVjdG9yOiBJbmplY3Rvcikge1xuICAgIHRoaXMuY2RSZWYgPSBpbmplY3Rvci5nZXQ8Q2hhbmdlRGV0ZWN0b3JSZWY+KENoYW5nZURldGVjdG9yUmVmIGFzIFR5cGU8Q2hhbmdlRGV0ZWN0b3JSZWY+KTtcbiAgfVxuXG4gIG5vdGlmeVZhbHVlQ2hhbmdlKCk6IHZvaWQge1xuICAgIGlmICh0aGlzLm9uQ2hhbmdlKSB7XG4gICAgICB0aGlzLm9uQ2hhbmdlKHRoaXMudmFsdWUpO1xuICAgIH1cbiAgfVxuXG4gIHdyaXRlVmFsdWUodmFsdWU6IFQpOiB2b2lkIHtcbiAgICB0aGlzLl92YWx1ZSA9IHZhbHVlO1xuICAgIHNldFRpbWVvdXQoKCkgPT4gdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCksIDApO1xuICB9XG5cbiAgcmVnaXN0ZXJPbkNoYW5nZShmbjogYW55KTogdm9pZCB7XG4gICAgdGhpcy5vbkNoYW5nZSA9IGZuO1xuICB9XG5cbiAgcmVnaXN0ZXJPblRvdWNoZWQoZm46IGFueSk6IHZvaWQge1xuICAgIHRoaXMub25Ub3VjaGVkID0gZm47XG4gIH1cblxuICBzZXREaXNhYmxlZFN0YXRlKGlzRGlzYWJsZWQ6IGJvb2xlYW4pOiB2b2lkIHtcbiAgICB0aGlzLmRpc2FibGVkID0gaXNEaXNhYmxlZDtcbiAgfVxufVxuIl19