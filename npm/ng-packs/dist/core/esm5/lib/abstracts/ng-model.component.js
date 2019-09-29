/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        { type: Component, args: [{ template: '' }] }
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmctbW9kZWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy9uZy1tb2RlbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUNBLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBUSxNQUFNLGVBQWUsQ0FBQzs7OztBQUVwRjtJQXFCRSxrQ0FBbUIsUUFBa0I7UUFBbEIsYUFBUSxHQUFSLFFBQVEsQ0FBVTtRQUNuQyxJQUFJLENBQUMsS0FBSyxHQUFHLFFBQVEsQ0FBQyxHQUFHLENBQW9CLG1CQUFBLGlCQUFpQixFQUEyQixDQUFDLENBQUM7SUFDN0YsQ0FBQztJQWxCRCxzQkFDSSwyQ0FBSzs7OztRQUtUO1lBQ0UsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDO1FBQ3JCLENBQUM7Ozs7O1FBUkQsVUFDVSxLQUFRO1lBQ2hCLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1lBQ3BCLElBQUksQ0FBQyxpQkFBaUIsRUFBRSxDQUFDO1FBQzNCLENBQUM7OztPQUFBOzs7O0lBZ0JELG9EQUFpQjs7O0lBQWpCO1FBQ0UsSUFBSSxJQUFJLENBQUMsUUFBUSxFQUFFO1lBQ2pCLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1NBQzNCO0lBQ0gsQ0FBQzs7Ozs7SUFFRCw2Q0FBVTs7OztJQUFWLFVBQVcsS0FBUTtRQUFuQixpQkFHQztRQUZDLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1FBQ3BCLFVBQVU7OztRQUFDLGNBQU0sT0FBQSxLQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBRSxFQUExQixDQUEwQixHQUFFLENBQUMsQ0FBQyxDQUFDO0lBQ2xELENBQUM7Ozs7O0lBRUQsbURBQWdCOzs7O0lBQWhCLFVBQWlCLEVBQU87UUFDdEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxFQUFFLENBQUM7SUFDckIsQ0FBQzs7Ozs7SUFFRCxvREFBaUI7Ozs7SUFBakIsVUFBa0IsRUFBTztRQUN2QixJQUFJLENBQUMsU0FBUyxHQUFHLEVBQUUsQ0FBQztJQUN0QixDQUFDOzs7OztJQUVELG1EQUFnQjs7OztJQUFoQixVQUFpQixVQUFtQjtRQUNsQyxJQUFJLENBQUMsUUFBUSxHQUFHLFVBQVUsQ0FBQztJQUM3QixDQUFDOztnQkE5Q0YsU0FBUyxTQUFDLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRTs7OztnQkFGWSxRQUFROzs7MkJBSTVDLEtBQUs7d0JBR0wsS0FBSzs7SUEwQ1IsK0JBQUM7Q0FBQSxBQS9DRCxJQStDQztTQTlDWSx3QkFBd0I7OztJQUNuQyw0Q0FDa0I7O0lBWWxCLDRDQUEyQjs7SUFDM0IsNkNBQW9COzs7OztJQUVwQiwwQ0FBb0I7Ozs7O0lBQ3BCLHlDQUFtQzs7SUFFdkIsNENBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29udHJvbFZhbHVlQWNjZXNzb3IgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBDaGFuZ2VEZXRlY3RvclJlZiwgQ29tcG9uZW50LCBJbmplY3RvciwgSW5wdXQsIFR5cGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7IHRlbXBsYXRlOiAnJyB9KVxuZXhwb3J0IGNsYXNzIEFic3RyYWN0TmdNb2RlbENvbXBvbmVudDxUID0gYW55PiBpbXBsZW1lbnRzIENvbnRyb2xWYWx1ZUFjY2Vzc29yIHtcbiAgQElucHV0KClcbiAgZGlzYWJsZWQ6IGJvb2xlYW47XG5cbiAgQElucHV0KClcbiAgc2V0IHZhbHVlKHZhbHVlOiBUKSB7XG4gICAgdGhpcy5fdmFsdWUgPSB2YWx1ZTtcbiAgICB0aGlzLm5vdGlmeVZhbHVlQ2hhbmdlKCk7XG4gIH1cblxuICBnZXQgdmFsdWUoKTogVCB7XG4gICAgcmV0dXJuIHRoaXMuX3ZhbHVlO1xuICB9XG5cbiAgb25DaGFuZ2U6ICh2YWx1ZTogVCkgPT4ge307XG4gIG9uVG91Y2hlZDogKCkgPT4ge307XG5cbiAgcHJvdGVjdGVkIF92YWx1ZTogVDtcbiAgcHJvdGVjdGVkIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZjtcblxuICBjb25zdHJ1Y3RvcihwdWJsaWMgaW5qZWN0b3I6IEluamVjdG9yKSB7XG4gICAgdGhpcy5jZFJlZiA9IGluamVjdG9yLmdldDxDaGFuZ2VEZXRlY3RvclJlZj4oQ2hhbmdlRGV0ZWN0b3JSZWYgYXMgVHlwZTxDaGFuZ2VEZXRlY3RvclJlZj4pO1xuICB9XG5cbiAgbm90aWZ5VmFsdWVDaGFuZ2UoKTogdm9pZCB7XG4gICAgaWYgKHRoaXMub25DaGFuZ2UpIHtcbiAgICAgIHRoaXMub25DaGFuZ2UodGhpcy52YWx1ZSk7XG4gICAgfVxuICB9XG5cbiAgd3JpdGVWYWx1ZSh2YWx1ZTogVCk6IHZvaWQge1xuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XG4gICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKSwgMCk7XG4gIH1cblxuICByZWdpc3Rlck9uQ2hhbmdlKGZuOiBhbnkpOiB2b2lkIHtcbiAgICB0aGlzLm9uQ2hhbmdlID0gZm47XG4gIH1cblxuICByZWdpc3Rlck9uVG91Y2hlZChmbjogYW55KTogdm9pZCB7XG4gICAgdGhpcy5vblRvdWNoZWQgPSBmbjtcbiAgfVxuXG4gIHNldERpc2FibGVkU3RhdGUoaXNEaXNhYmxlZDogYm9vbGVhbik6IHZvaWQge1xuICAgIHRoaXMuZGlzYWJsZWQgPSBpc0Rpc2FibGVkO1xuICB9XG59XG4iXX0=