/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, IterableDiffers, TemplateRef, ViewContainerRef, } from '@angular/core';
import compare from 'just-compare';
import clone from 'just-clone';
var AbpForContext = /** @class */ (function () {
    function AbpForContext($implicit, index, count, list) {
        this.$implicit = $implicit;
        this.index = index;
        this.count = count;
        this.list = list;
    }
    return AbpForContext;
}());
if (false) {
    /** @type {?} */
    AbpForContext.prototype.$implicit;
    /** @type {?} */
    AbpForContext.prototype.index;
    /** @type {?} */
    AbpForContext.prototype.count;
    /** @type {?} */
    AbpForContext.prototype.list;
}
var RecordView = /** @class */ (function () {
    function RecordView(record, view) {
        this.record = record;
        this.view = view;
    }
    return RecordView;
}());
if (false) {
    /** @type {?} */
    RecordView.prototype.record;
    /** @type {?} */
    RecordView.prototype.view;
}
var ForDirective = /** @class */ (function () {
    function ForDirective(tempRef, vcRef, differs) {
        this.tempRef = tempRef;
        this.vcRef = vcRef;
        this.differs = differs;
    }
    Object.defineProperty(ForDirective.prototype, "compareFn", {
        get: /**
         * @return {?}
         */
        function () {
            return this.compareBy || compare;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ForDirective.prototype, "trackByFn", {
        get: /**
         * @return {?}
         */
        function () {
            return this.trackBy || ((/**
             * @param {?} index
             * @param {?} item
             * @return {?}
             */
            function (index, item) { return ((/** @type {?} */ (item))).id || index; }));
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @private
     * @param {?} changes
     * @return {?}
     */
    ForDirective.prototype.iterateOverAppliedOperations = /**
     * @private
     * @param {?} changes
     * @return {?}
     */
    function (changes) {
        var _this = this;
        /** @type {?} */
        var rw = [];
        changes.forEachOperation((/**
         * @param {?} record
         * @param {?} previousIndex
         * @param {?} currentIndex
         * @return {?}
         */
        function (record, previousIndex, currentIndex) {
            if (record.previousIndex == null) {
                /** @type {?} */
                var view = _this.vcRef.createEmbeddedView(_this.tempRef, new AbpForContext(null, -1, -1, _this.items), currentIndex);
                rw.push(new RecordView(record, view));
            }
            else if (currentIndex == null) {
                _this.vcRef.remove(previousIndex);
            }
            else {
                /** @type {?} */
                var view = _this.vcRef.get(previousIndex);
                _this.vcRef.move(view, currentIndex);
                rw.push(new RecordView(record, (/** @type {?} */ (view))));
            }
        }));
        for (var i = 0, l = rw.length; i < l; i++) {
            rw[i].view.context.$implicit = rw[i].record.item;
        }
    };
    /**
     * @private
     * @param {?} changes
     * @return {?}
     */
    ForDirective.prototype.iterateOverAttachedViews = /**
     * @private
     * @param {?} changes
     * @return {?}
     */
    function (changes) {
        var _this = this;
        for (var i = 0, l = this.vcRef.length; i < l; i++) {
            /** @type {?} */
            var viewRef = (/** @type {?} */ (this.vcRef.get(i)));
            viewRef.context.index = i;
            viewRef.context.count = l;
            viewRef.context.list = this.items;
        }
        changes.forEachIdentityChange((/**
         * @param {?} record
         * @return {?}
         */
        function (record) {
            /** @type {?} */
            var viewRef = (/** @type {?} */ (_this.vcRef.get(record.currentIndex)));
            viewRef.context.$implicit = record.item;
        }));
    };
    /**
     * @private
     * @param {?} items
     * @return {?}
     */
    ForDirective.prototype.projectItems = /**
     * @private
     * @param {?} items
     * @return {?}
     */
    function (items) {
        if (!items.length && this.emptyRef) {
            this.vcRef.clear();
            // tslint:disable-next-line: no-unused-expression
            this.vcRef.createEmbeddedView(this.emptyRef).rootNodes;
            this.isShowEmptyRef = true;
            this.differ = null;
            return;
        }
        if (this.emptyRef && this.isShowEmptyRef) {
            this.vcRef.clear();
            this.isShowEmptyRef = false;
        }
        if (!this.differ && items) {
            this.differ = this.differs.find(items).create(this.trackByFn);
        }
        if (this.differ) {
            /** @type {?} */
            var changes = this.differ.diff(items);
            if (changes) {
                this.iterateOverAppliedOperations(changes);
                this.iterateOverAttachedViews(changes);
            }
        }
    };
    /**
     * @private
     * @param {?} items
     * @return {?}
     */
    ForDirective.prototype.sortItems = /**
     * @private
     * @param {?} items
     * @return {?}
     */
    function (items) {
        var _this = this;
        if (this.orderBy) {
            items.sort((/**
             * @param {?} a
             * @param {?} b
             * @return {?}
             */
            function (a, b) { return (a[_this.orderBy] > b[_this.orderBy] ? 1 : a[_this.orderBy] < b[_this.orderBy] ? -1 : 0); }));
        }
        else {
            items.sort();
        }
    };
    /**
     * @return {?}
     */
    ForDirective.prototype.ngOnChanges = /**
     * @return {?}
     */
    function () {
        var _this = this;
        /** @type {?} */
        var items = (/** @type {?} */ (clone(this.items)));
        if (!Array.isArray(items))
            return;
        /** @type {?} */
        var compareFn = this.compareFn;
        if (typeof this.filterBy !== 'undefined' && this.filterVal) {
            items = items.filter((/**
             * @param {?} item
             * @return {?}
             */
            function (item) { return compareFn(item[_this.filterBy], _this.filterVal); }));
        }
        switch (this.orderDir) {
            case 'ASC':
                this.sortItems(items);
                this.projectItems(items);
                break;
            case 'DESC':
                this.sortItems(items);
                items.reverse();
                this.projectItems(items);
                break;
            default:
                this.projectItems(items);
        }
    };
    ForDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[abpFor]',
                },] }
    ];
    /** @nocollapse */
    ForDirective.ctorParameters = function () { return [
        { type: TemplateRef },
        { type: ViewContainerRef },
        { type: IterableDiffers }
    ]; };
    ForDirective.propDecorators = {
        items: [{ type: Input, args: ['abpForOf',] }],
        orderBy: [{ type: Input, args: ['abpForOrderBy',] }],
        orderDir: [{ type: Input, args: ['abpForOrderDir',] }],
        filterBy: [{ type: Input, args: ['abpForFilterBy',] }],
        filterVal: [{ type: Input, args: ['abpForFilterVal',] }],
        trackBy: [{ type: Input, args: ['abpForTrackBy',] }],
        compareBy: [{ type: Input, args: ['abpForCompareBy',] }],
        emptyRef: [{ type: Input, args: ['abpForEmptyRef',] }]
    };
    return ForDirective;
}());
export { ForDirective };
if (false) {
    /** @type {?} */
    ForDirective.prototype.items;
    /** @type {?} */
    ForDirective.prototype.orderBy;
    /** @type {?} */
    ForDirective.prototype.orderDir;
    /** @type {?} */
    ForDirective.prototype.filterBy;
    /** @type {?} */
    ForDirective.prototype.filterVal;
    /** @type {?} */
    ForDirective.prototype.trackBy;
    /** @type {?} */
    ForDirective.prototype.compareBy;
    /** @type {?} */
    ForDirective.prototype.emptyRef;
    /**
     * @type {?}
     * @private
     */
    ForDirective.prototype.differ;
    /**
     * @type {?}
     * @private
     */
    ForDirective.prototype.isShowEmptyRef;
    /**
     * @type {?}
     * @private
     */
    ForDirective.prototype.tempRef;
    /**
     * @type {?}
     * @private
     */
    ForDirective.prototype.vcRef;
    /**
     * @type {?}
     * @private
     */
    ForDirective.prototype.differs;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9yLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2Zvci5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBRVQsS0FBSyxFQUlMLGVBQWUsRUFFZixXQUFXLEVBRVgsZ0JBQWdCLEdBQ2pCLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sT0FBTyxNQUFNLGNBQWMsQ0FBQztBQUNuQyxPQUFPLEtBQUssTUFBTSxZQUFZLENBQUM7QUFJL0I7SUFDRSx1QkFBbUIsU0FBYyxFQUFTLEtBQWEsRUFBUyxLQUFhLEVBQVMsSUFBVztRQUE5RSxjQUFTLEdBQVQsU0FBUyxDQUFLO1FBQVMsVUFBSyxHQUFMLEtBQUssQ0FBUTtRQUFTLFVBQUssR0FBTCxLQUFLLENBQVE7UUFBUyxTQUFJLEdBQUosSUFBSSxDQUFPO0lBQUcsQ0FBQztJQUN2RyxvQkFBQztBQUFELENBQUMsQUFGRCxJQUVDOzs7SUFEYSxrQ0FBcUI7O0lBQUUsOEJBQW9COztJQUFFLDhCQUFvQjs7SUFBRSw2QkFBa0I7O0FBR25HO0lBQ0Usb0JBQW1CLE1BQWlDLEVBQVMsSUFBb0M7UUFBOUUsV0FBTSxHQUFOLE1BQU0sQ0FBMkI7UUFBUyxTQUFJLEdBQUosSUFBSSxDQUFnQztJQUFHLENBQUM7SUFDdkcsaUJBQUM7QUFBRCxDQUFDLEFBRkQsSUFFQzs7O0lBRGEsNEJBQXdDOztJQUFFLDBCQUEyQzs7QUFHbkc7SUF3Q0Usc0JBQ1UsT0FBbUMsRUFDbkMsS0FBdUIsRUFDdkIsT0FBd0I7UUFGeEIsWUFBTyxHQUFQLE9BQU8sQ0FBNEI7UUFDbkMsVUFBSyxHQUFMLEtBQUssQ0FBa0I7UUFDdkIsWUFBTyxHQUFQLE9BQU8sQ0FBaUI7SUFDL0IsQ0FBQztJQVpKLHNCQUFJLG1DQUFTOzs7O1FBQWI7WUFDRSxPQUFPLElBQUksQ0FBQyxTQUFTLElBQUksT0FBTyxD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QyxDQUFDO2dCQUN0QixLQUFLLENBQUMsT0FBTyxFQUFFLENBQUM7Z0JBQ2hCLElBQUksQ0FBQyxZQUFZLENBQUMsS0FBSyxDQUFDLENBQUM7Z0JBQ3pCLE1BQU07WUFFUjtnQkFDRSxJQUFJLENBQUMsWUFBWSxDQUFDLEtBQUssQ0FBQyxDQUFDO1NBQzVCO0lBQ0gsQ0FBQzs7Z0JBdEpGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsVUFBVTtpQkFDckI7Ozs7Z0JBbkJDLFdBQVc7Z0JBRVgsZ0JBQWdCO2dCQUpoQixlQUFlOzs7d0JBdUJkLEtBQUssU0FBQyxVQUFVOzBCQUdoQixLQUFLLFNBQUMsZUFBZTsyQkFHckIsS0FBSyxTQUFDLGdCQUFnQjsyQkFHdEIsS0FBSyxTQUFDLGdCQUFnQjs0QkFHdEIsS0FBSyxTQUFDLGlCQUFpQjswQkFHdkIsS0FBSyxTQUFDLGVBQWU7NEJBR3JCLEtBQUssU0FBQyxpQkFBaUI7MkJBR3ZCLEtBQUssU0FBQyxnQkFBZ0I7O0lBOEh6QixtQkFBQztDQUFBLEFBdkpELElBdUpDO1NBcEpZLFlBQVk7OztJQUN2Qiw2QkFDYTs7SUFFYiwrQkFDZ0I7O0lBRWhCLGdDQUN5Qjs7SUFFekIsZ0NBQ2lCOztJQUVqQixpQ0FDZTs7SUFFZiwrQkFDUTs7SUFFUixpQ0FDcUI7O0lBRXJCLGdDQUMyQjs7Ozs7SUFFM0IsOEJBQW9DOzs7OztJQUVwQyxzQ0FBZ0M7Ozs7O0lBVzlCLCtCQUEyQzs7Ozs7SUFDM0MsNkJBQStCOzs7OztJQUMvQiwrQkFBZ0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xyXG4gIERpcmVjdGl2ZSxcclxuICBFbWJlZGRlZFZpZXdSZWYsXHJcbiAgSW5wdXQsXHJcbiAgSXRlcmFibGVDaGFuZ2VSZWNvcmQsXHJcbiAgSXRlcmFibGVDaGFuZ2VzLFxyXG4gIEl0ZXJhYmxlRGlmZmVyLFxyXG4gIEl0ZXJhYmxlRGlmZmVycyxcclxuICBPbkNoYW5nZXMsXHJcbiAgVGVtcGxhdGVSZWYsXHJcbiAgVHJhY2tCeUZ1bmN0aW9uLFxyXG4gIFZpZXdDb250YWluZXJSZWYsXHJcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCBjb21wYXJlIGZyb20gJ2p1c3QtY29tcGFyZSc7XHJcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcclxuXHJcbmV4cG9ydCB0eXBlIENvbXBhcmVGbjxUID0gYW55PiA9ICh2YWx1ZTogVCwgY29tcGFyaXNvbjogVCkgPT4gYm9vbGVhbjtcclxuXHJcbmNsYXNzIEFicEZvckNvbnRleHQge1xyXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyAkaW1wbGljaXQ6IGFueSwgcHVibGljIGluZGV4OiBudW1iZXIsIHB1YmxpYyBjb3VudDogbnVtYmVyLCBwdWJsaWMgbGlzdDogYW55W10pIHt9XHJcbn1cclxuXHJcbmNsYXNzIFJlY29yZFZpZXcge1xyXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyByZWNvcmQ6IEl0ZXJhYmxlQ2hhbmdlUmVjb3JkPGFueT4sIHB1YmxpYyB2aWV3OiBFbWJlZGRlZFZpZXdSZWY8QWJwRm9yQ29udGV4dD4pIHt9XHJcbn1cclxuXHJcbkBEaXJlY3RpdmUoe1xyXG4gIHNlbGVjdG9yOiAnW2FicEZvcl0nLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgRm9yRGlyZWN0aXZlIGltcGxlbWVudHMgT25DaGFuZ2VzIHtcclxuICBASW5wdXQoJ2FicEZvck9mJylcclxuICBpdGVtczogYW55W107XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yT3JkZXJCeScpXHJcbiAgb3JkZXJCeTogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoJ2FicEZvck9yZGVyRGlyJylcclxuICBvcmRlckRpcjogJ0FTQycgfCAnREVTQyc7XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yRmlsdGVyQnknKVxyXG4gIGZpbHRlckJ5OiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yRmlsdGVyVmFsJylcclxuICBmaWx0ZXJWYWw6IGFueTtcclxuXHJcbiAgQElucHV0KCdhYnBGb3JUcmFja0J5JylcclxuICB0cmFja0J5O1xyXG5cclxuICBASW5wdXQoJ2FicEZvckNvbXBhcmVCeScpXHJcbiAgY29tcGFyZUJ5OiBDb21wYXJlRm47XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yRW1wdHlSZWYnKVxyXG4gIGVtcHR5UmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xyXG5cclxuICBwcml2YXRlIGRpZmZlcjogSXRlcmFibGVEaWZmZXI8YW55PjtcclxuXHJcbiAgcHJpdmF0ZSBpc1Nob3dFbXB0eVJlZjogYm9vbGVhbjtcclxuXHJcbiAgZ2V0IGNvbXBhcmVGbigpOiBDb21wYXJlRm4ge1xyXG4gICAgcmV0dXJuIHRoaXMuY29tcGFyZUJ5IHx8IGNvbXBhcmU7XHJcbiAgfVxyXG5cclxuICBnZXQgdHJhY2tCeUZuKCk6IFRyYWNrQnlGdW5jdGlvbjxhbnk+IHtcclxuICAgIHJldHVybiB0aGlzLnRyYWNrQnkgfHwgKChpbmRleDogbnVtYmVyLCBpdGVtOiBhbnkpID0+IChpdGVtIGFzIGFueSkuaWQgfHwgaW5kZXgpO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IoXHJcbiAgICBwcml2YXRlIHRlbXBSZWY6IFRlbXBsYXRlUmVmPEFicEZvckNvbnRleHQ+LFxyXG4gICAgcHJpdmF0ZSB2Y1JlZjogVmlld0NvbnRhaW5lclJlZixcclxuICAgIHByaXZhdGUgZGlmZmVyczogSXRlcmFibGVEaWZmZXJzLFxyXG4gICkge31cclxuXHJcbiAgcHJpdmF0ZSBpdGVyYXRlT3ZlckFwcGxpZWRPcGVyYXRpb25zKGNoYW5nZXM6IEl0ZXJhYmxlQ2hhbmdlczxhbnk+KSB7XHJcbiAgICBjb25zdCBydzogUmVjb3JkVmlld1tdID0gW107XHJcblxyXG4gICAgY2hhbmdlcy5mb3JFYWNoT3BlcmF0aW9uKChyZWNvcmQ6IEl0ZXJhYmxlQ2hhbmdlUmVjb3JkPGFueT4sIHByZXZpb3VzSW5kZXg6IG51bWJlciwgY3VycmVudEluZGV4OiBudW1iZXIpID0+IHtcclxuICAgICAgaWYgKHJlY29yZC5wcmV2aW91c0luZGV4ID09IG51bGwpIHtcclxuICAgICAgICBjb25zdCB2aWV3ID0gdGhpcy52Y1JlZi5jcmVhdGVFbWJlZGRlZFZpZXcoXHJcbiAgICAgICAgICB0aGlzLnRlbXBSZWYsXHJcbiAgICAgICAgICBuZXcgQWJwRm9yQ29udGV4dChudWxsLCAtMSwgLTEsIHRoaXMuaXRlbXMpLFxyXG4gICAgICAgICAgY3VycmVudEluZGV4LFxyXG4gICAgICAgICk7XHJcblxyXG4gICAgICAgIHJ3LnB1c2gobmV3IFJlY29yZFZpZXcocmVjb3JkLCB2aWV3KSk7XHJcbiAgICAgIH0gZWxzZSBpZiAoY3VycmVudEluZGV4ID09IG51bGwpIHtcclxuICAgICAgICB0aGlzLnZjUmVmLnJlbW92ZShwcmV2aW91c0luZGV4KTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICBjb25zdCB2aWV3ID0gdGhpcy52Y1JlZi5nZXQocHJldmlvdXNJbmRleCk7XHJcbiAgICAgICAgdGhpcy52Y1JlZi5tb3ZlKHZpZXcsIGN1cnJlbnRJbmRleCk7XHJcblxyXG4gICAgICAgIHJ3LnB1c2gobmV3IFJlY29yZFZpZXcocmVjb3JkLCB2aWV3IGFzIEVtYmVkZGVkVmlld1JlZjxBYnBGb3JDb250ZXh0PikpO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuXHJcbiAgICBmb3IgKGxldCBpID0gMCwgbCA9IHJ3Lmxlbmd0aDsgaSA8IGw7IGkrKykge1xyXG4gICAgICByd1tpXS52aWV3LmNvbnRleHQuJGltcGxpY2l0ID0gcndbaV0ucmVjb3JkLml0ZW07XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBwcml2YXRlIGl0ZXJhdGVPdmVyQXR0YWNoZWRWaWV3cyhjaGFuZ2VzOiBJdGVyYWJsZUNoYW5nZXM8YW55Pikge1xyXG4gICAgZm9yIChsZXQgaSA9IDAsIGwgPSB0aGlzLnZjUmVmLmxlbmd0aDsgaSA8IGw7IGkrKykge1xyXG4gICAgICBjb25zdCB2aWV3UmVmID0gdGhpcy52Y1JlZi5nZXQoaSkgYXMgRW1iZWRkZWRWaWV3UmVmPEFicEZvckNvbnRleHQ+O1xyXG4gICAgICB2aWV3UmVmLmNvbnRleHQuaW5kZXggPSBpO1xyXG4gICAgICB2aWV3UmVmLmNvbnRleHQuY291bnQgPSBsO1xyXG4gICAgICB2aWV3UmVmLmNvbnRleHQubGlzdCA9IHRoaXMuaXRlbXM7XHJcbiAgICB9XHJcblxyXG4gICAgY2hhbmdlcy5mb3JFYWNoSWRlbnRpdHlDaGFuZ2UoKHJlY29yZDogSXRlcmFibGVDaGFuZ2VSZWNvcmQ8YW55PikgPT4ge1xyXG4gICAgICBjb25zdCB2aWV3UmVmID0gdGhpcy52Y1JlZi5nZXQocmVjb3JkLmN1cnJlbnRJbmRleCkgYXMgRW1iZWRkZWRWaWV3UmVmPEFicEZvckNvbnRleHQ+O1xyXG4gICAgICB2aWV3UmVmLmNvbnRleHQuJGltcGxpY2l0ID0gcmVjb3JkLml0ZW07XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHByaXZhdGUgcHJvamVjdEl0ZW1zKGl0ZW1zOiBhbnlbXSk6IHZvaWQge1xyXG4gICAgaWYgKCFpdGVtcy5sZW5ndGggJiYgdGhpcy5lbXB0eVJlZikge1xyXG4gICAgICB0aGlzLnZjUmVmLmNsZWFyKCk7XHJcbiAgICAgIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tdW51c2VkLWV4cHJlc3Npb25cclxuICAgICAgdGhpcy52Y1JlZi5jcmVhdGVFbWJlZGRlZFZpZXcodGhpcy5lbXB0eVJlZikucm9vdE5vZGVzO1xyXG4gICAgICB0aGlzLmlzU2hvd0VtcHR5UmVmID0gdHJ1ZTtcclxuICAgICAgdGhpcy5kaWZmZXIgPSBudWxsO1xyXG5cclxuICAgICAgcmV0dXJuO1xyXG4gICAgfVxyXG5cclxuICAgIGlmICh0aGlzLmVtcHR5UmVmICYmIHRoaXMuaXNTaG93RW1wdHlSZWYpIHtcclxuICAgICAgdGhpcy52Y1JlZi5jbGVhcigpO1xyXG4gICAgICB0aGlzLmlzU2hvd0VtcHR5UmVmID0gZmFsc2U7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKCF0aGlzLmRpZmZlciAmJiBpdGVtcykge1xyXG4gICAgICB0aGlzLmRpZmZlciA9IHRoaXMuZGlmZmVycy5maW5kKGl0ZW1zKS5jcmVhdGUodGhpcy50cmFja0J5Rm4pO1xyXG4gICAgfVxyXG5cclxuICAgIGlmICh0aGlzLmRpZmZlcikge1xyXG4gICAgICBjb25zdCBjaGFuZ2VzID0gdGhpcy5kaWZmZXIuZGlmZihpdGVtcyk7XHJcblxyXG4gICAgICBpZiAoY2hhbmdlcykge1xyXG4gICAgICAgIHRoaXMuaXRlcmF0ZU92ZXJBcHBsaWVkT3BlcmF0aW9ucyhjaGFuZ2VzKTtcclxuICAgICAgICB0aGlzLml0ZXJhdGVPdmVyQXR0YWNoZWRWaWV3cyhjaGFuZ2VzKTtcclxuICAgICAgfVxyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBzb3J0SXRlbXMoaXRlbXM6IGFueVtdKSB7XHJcbiAgICBpZiAodGhpcy5vcmRlckJ5KSB7XHJcbiAgICAgIGl0ZW1zLnNvcnQoKGEsIGIpID0+IChhW3RoaXMub3JkZXJCeV0gPiBiW3RoaXMub3JkZXJCeV0gPyAxIDogYVt0aGlzLm9yZGVyQnldIDwgYlt0aGlzLm9yZGVyQnldID8gLTEgOiAwKSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBpdGVtcy5zb3J0KCk7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBuZ09uQ2hhbmdlcygpIHtcclxuICAgIGxldCBpdGVtcyA9IGNsb25lKHRoaXMuaXRlbXMpIGFzIGFueVtdO1xyXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGl0ZW1zKSkgcmV0dXJuO1xyXG5cclxuICAgIGNvbnN0IGNvbXBhcmVGbiA9IHRoaXMuY29tcGFyZUZuO1xyXG5cclxuICAgIGlmICh0eXBlb2YgdGhpcy5maWx0ZXJCeSAhPT0gJ3VuZGVmaW5lZCcgJiYgdGhpcy5maWx0ZXJWYWwpIHtcclxuICAgICAgaXRlbXMgPSBpdGVtcy5maWx0ZXIoaXRlbSA9PiBjb21wYXJlRm4oaXRlbVt0aGlzLmZpbHRlckJ5XSwgdGhpcy5maWx0ZXJWYWwpKTtcclxuICAgIH1cclxuXHJcbiAgICBzd2l0Y2ggKHRoaXMub3JkZXJEaXIpIHtcclxuICAgICAgY2FzZSAnQVNDJzpcclxuICAgICAgICB0aGlzLnNvcnRJdGVtcyhpdGVtcyk7XHJcbiAgICAgICAgdGhpcy5wcm9qZWN0SXRlbXMoaXRlbXMpO1xyXG4gICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgY2FzZSAnREVTQyc6XHJcbiAgICAgICAgdGhpcy5zb3J0SXRlbXMoaXRlbXMpO1xyXG4gICAgICAgIGl0ZW1zLnJldmVyc2UoKTtcclxuICAgICAgICB0aGlzLnByb2plY3RJdGVtcyhpdGVtcyk7XHJcbiAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICBkZWZhdWx0OlxyXG4gICAgICAgIHRoaXMucHJvamVjdEl0ZW1zKGl0ZW1zKTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19