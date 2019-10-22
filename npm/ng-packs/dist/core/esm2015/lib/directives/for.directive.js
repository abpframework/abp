/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, IterableDiffers, TemplateRef, ViewContainerRef, } from '@angular/core';
import compare from 'just-compare';
import clone from 'just-clone';
class AbpForContext {
    /**
     * @param {?} $implicit
     * @param {?} index
     * @param {?} count
     * @param {?} list
     */
    constructor($implicit, index, count, list) {
        this.$implicit = $implicit;
        this.index = index;
        this.count = count;
        this.list = list;
    }
}
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
class RecordView {
    /**
     * @param {?} record
     * @param {?} view
     */
    constructor(record, view) {
        this.record = record;
        this.view = view;
    }
}
if (false) {
    /** @type {?} */
    RecordView.prototype.record;
    /** @type {?} */
    RecordView.prototype.view;
}
export class ForDirective {
    /**
     * @param {?} tempRef
     * @param {?} vcRef
     * @param {?} differs
     */
    constructor(tempRef, vcRef, differs) {
        this.tempRef = tempRef;
        this.vcRef = vcRef;
        this.differs = differs;
    }
    /**
     * @return {?}
     */
    get compareFn() {
        return this.compareBy || compare;
    }
    /**
     * @return {?}
     */
    get trackByFn() {
        return this.trackBy || ((/**
         * @param {?} index
         * @param {?} item
         * @return {?}
         */
        (index, item) => ((/** @type {?} */ (item))).id || index));
    }
    /**
     * @private
     * @param {?} changes
     * @return {?}
     */
    iterateOverAppliedOperations(changes) {
        /** @type {?} */
        const rw = [];
        changes.forEachOperation((/**
         * @param {?} record
         * @param {?} previousIndex
         * @param {?} currentIndex
         * @return {?}
         */
        (record, previousIndex, currentIndex) => {
            if (record.previousIndex == null) {
                /** @type {?} */
                const view = this.vcRef.createEmbeddedView(this.tempRef, new AbpForContext(null, -1, -1, this.items), currentIndex);
                rw.push(new RecordView(record, view));
            }
            else if (currentIndex == null) {
                this.vcRef.remove(previousIndex);
            }
            else {
                /** @type {?} */
                const view = this.vcRef.get(previousIndex);
                this.vcRef.move(view, currentIndex);
                rw.push(new RecordView(record, (/** @type {?} */ (view))));
            }
        }));
        for (let i = 0, l = rw.length; i < l; i++) {
            rw[i].view.context.$implicit = rw[i].record.item;
        }
    }
    /**
     * @private
     * @param {?} changes
     * @return {?}
     */
    iterateOverAttachedViews(changes) {
        for (let i = 0, l = this.vcRef.length; i < l; i++) {
            /** @type {?} */
            const viewRef = (/** @type {?} */ (this.vcRef.get(i)));
            viewRef.context.index = i;
            viewRef.context.count = l;
            viewRef.context.list = this.items;
        }
        changes.forEachIdentityChange((/**
         * @param {?} record
         * @return {?}
         */
        (record) => {
            /** @type {?} */
            const viewRef = (/** @type {?} */ (this.vcRef.get(record.currentIndex)));
            viewRef.context.$implicit = record.item;
        }));
    }
    /**
     * @private
     * @param {?} items
     * @return {?}
     */
    projectItems(items) {
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
            const changes = this.differ.diff(items);
            if (changes) {
                this.iterateOverAppliedOperations(changes);
                this.iterateOverAttachedViews(changes);
            }
        }
    }
    /**
     * @private
     * @param {?} items
     * @return {?}
     */
    sortItems(items) {
        if (this.orderBy) {
            items.sort((/**
             * @param {?} a
             * @param {?} b
             * @return {?}
             */
            (a, b) => (a[this.orderBy] > b[this.orderBy] ? 1 : a[this.orderBy] < b[this.orderBy] ? -1 : 0)));
        }
        else {
            items.sort();
        }
    }
    /**
     * @return {?}
     */
    ngOnChanges() {
        /** @type {?} */
        let items = (/** @type {?} */ (clone(this.items)));
        if (!Array.isArray(items))
            return;
        /** @type {?} */
        const compareFn = this.compareFn;
        if (typeof this.filterBy !== 'undefined' && this.filterVal) {
            items = items.filter((/**
             * @param {?} item
             * @return {?}
             */
            item => compareFn(item[this.filterBy], this.filterVal)));
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
    }
}
ForDirective.decorators = [
    { type: Directive, args: [{
                selector: '[abpFor]',
            },] }
];
/** @nocollapse */
ForDirective.ctorParameters = () => [
    { type: TemplateRef },
    { type: ViewContainerRef },
    { type: IterableDiffers }
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9yLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2Zvci5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBRVQsS0FBSyxFQUlMLGVBQWUsRUFFZixXQUFXLEVBRVgsZ0JBQWdCLEdBQ2pCLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sT0FBTyxNQUFNLGNBQWMsQ0FBQztBQUNuQyxPQUFPLEtBQUssTUFBTSxZQUFZLENBQUM7QUFJL0IsTUFBTSxhQUFhOzs7Ozs7O0lBQ2pCLFlBQW1CLFNBQWMsRUFBUyxLQUFhLEVBQVMsS0FBYSxFQUFTLElBQVc7UUFBOUUsY0FBUyxHQUFULFNBQVMsQ0FBSztRQUFTLFVBQUssR0FBTCxLQUFLLENBQVE7UUFBUyxVQUFLLEdBQUwsS0FBSyxDQUFRO1FBQVMsU0FBSSxHQUFKLElBQUksQ0FBTztJQUFHLENBQUM7Q0FDdEc7OztJQURhLGtDQUFxQjs7SUFBRSw4QkFBb0I7O0lBQUUsOEJBQW9COztJQUFFLDZCQUFrQjs7QUFHbkcsTUFBTSxVQUFVOzs7OztJQUNkLFlBQW1CLE1BQWlDLEVBQVMsSUFBb0M7UUFBOUUsV0FBTSxHQUFOLE1BQU0sQ0FBMkI7UUFBUyxTQUFJLEdBQUosSUFBSSxDQUFnQztJQUFHLENBQUM7Q0FDdEc7OztJQURhLDRCQUF3Qzs7SUFBRSwwQkFBMkM7O0FBTW5HLE1BQU0sT0FBTyxZQUFZOzs7Ozs7SUFxQ3ZCLFlBQ1UsT0FBbUMsRUFDbkMsS0FBdUIsRUFDdkIsT0FBd0I7UUFGeEIsWUFBTyxHQUFQLE9BQU8sQ0FBNEI7UUFDbkMsVUFBSyxHQUFMLEtBQUssQ0FBa0I7UUFDdkIsWUFBTyxHQUFQLE9BQU8sQ0FBaUI7SUFDL0IsQ0FBQzs7OztJQVpKLElBQUksU0FBUztRQUNYLE9BQU8sSUFBSSxDQUFDLFNBQVMsSUFBSSxPQUFPLENBQUM7SUFDbkMs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U0FDNUI7SUFDSCxDQUFDOzs7WUF0SkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxVQUFVO2FBQ3JCOzs7O1lBbkJDLFdBQVc7WUFFWCxnQkFBZ0I7WUFKaEIsZUFBZTs7O29CQXVCZCxLQUFLLFNBQUMsVUFBVTtzQkFHaEIsS0FBSyxTQUFDLGVBQWU7dUJBR3JCLEtBQUssU0FBQyxnQkFBZ0I7dUJBR3RCLEtBQUssU0FBQyxnQkFBZ0I7d0JBR3RCLEtBQUssU0FBQyxpQkFBaUI7c0JBR3ZCLEtBQUssU0FBQyxlQUFlO3dCQUdyQixLQUFLLFNBQUMsaUJBQWlCO3VCQUd2QixLQUFLLFNBQUMsZ0JBQWdCOzs7O0lBckJ2Qiw2QkFDYTs7SUFFYiwrQkFDZ0I7O0lBRWhCLGdDQUN5Qjs7SUFFekIsZ0NBQ2lCOztJQUVqQixpQ0FDZTs7SUFFZiwrQkFDUTs7SUFFUixpQ0FDcUI7O0lBRXJCLGdDQUMyQjs7Ozs7SUFFM0IsOEJBQW9DOzs7OztJQUVwQyxzQ0FBZ0M7Ozs7O0lBVzlCLCtCQUEyQzs7Ozs7SUFDM0MsNkJBQStCOzs7OztJQUMvQiwrQkFBZ0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xyXG4gIERpcmVjdGl2ZSxcclxuICBFbWJlZGRlZFZpZXdSZWYsXHJcbiAgSW5wdXQsXHJcbiAgSXRlcmFibGVDaGFuZ2VSZWNvcmQsXHJcbiAgSXRlcmFibGVDaGFuZ2VzLFxyXG4gIEl0ZXJhYmxlRGlmZmVyLFxyXG4gIEl0ZXJhYmxlRGlmZmVycyxcclxuICBPbkNoYW5nZXMsXHJcbiAgVGVtcGxhdGVSZWYsXHJcbiAgVHJhY2tCeUZ1bmN0aW9uLFxyXG4gIFZpZXdDb250YWluZXJSZWYsXHJcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCBjb21wYXJlIGZyb20gJ2p1c3QtY29tcGFyZSc7XHJcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcclxuXHJcbmV4cG9ydCB0eXBlIENvbXBhcmVGbjxUID0gYW55PiA9ICh2YWx1ZTogVCwgY29tcGFyaXNvbjogVCkgPT4gYm9vbGVhbjtcclxuXHJcbmNsYXNzIEFicEZvckNvbnRleHQge1xyXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyAkaW1wbGljaXQ6IGFueSwgcHVibGljIGluZGV4OiBudW1iZXIsIHB1YmxpYyBjb3VudDogbnVtYmVyLCBwdWJsaWMgbGlzdDogYW55W10pIHt9XHJcbn1cclxuXHJcbmNsYXNzIFJlY29yZFZpZXcge1xyXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyByZWNvcmQ6IEl0ZXJhYmxlQ2hhbmdlUmVjb3JkPGFueT4sIHB1YmxpYyB2aWV3OiBFbWJlZGRlZFZpZXdSZWY8QWJwRm9yQ29udGV4dD4pIHt9XHJcbn1cclxuXHJcbkBEaXJlY3RpdmUoe1xyXG4gIHNlbGVjdG9yOiAnW2FicEZvcl0nLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgRm9yRGlyZWN0aXZlIGltcGxlbWVudHMgT25DaGFuZ2VzIHtcclxuICBASW5wdXQoJ2FicEZvck9mJylcclxuICBpdGVtczogYW55W107XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yT3JkZXJCeScpXHJcbiAgb3JkZXJCeTogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoJ2FicEZvck9yZGVyRGlyJylcclxuICBvcmRlckRpcjogJ0FTQycgfCAnREVTQyc7XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yRmlsdGVyQnknKVxyXG4gIGZpbHRlckJ5OiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yRmlsdGVyVmFsJylcclxuICBmaWx0ZXJWYWw6IGFueTtcclxuXHJcbiAgQElucHV0KCdhYnBGb3JUcmFja0J5JylcclxuICB0cmFja0J5O1xyXG5cclxuICBASW5wdXQoJ2FicEZvckNvbXBhcmVCeScpXHJcbiAgY29tcGFyZUJ5OiBDb21wYXJlRm47XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yRW1wdHlSZWYnKVxyXG4gIGVtcHR5UmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xyXG5cclxuICBwcml2YXRlIGRpZmZlcjogSXRlcmFibGVEaWZmZXI8YW55PjtcclxuXHJcbiAgcHJpdmF0ZSBpc1Nob3dFbXB0eVJlZjogYm9vbGVhbjtcclxuXHJcbiAgZ2V0IGNvbXBhcmVGbigpOiBDb21wYXJlRm4ge1xyXG4gICAgcmV0dXJuIHRoaXMuY29tcGFyZUJ5IHx8IGNvbXBhcmU7XHJcbiAgfVxyXG5cclxuICBnZXQgdHJhY2tCeUZuKCk6IFRyYWNrQnlGdW5jdGlvbjxhbnk+IHtcclxuICAgIHJldHVybiB0aGlzLnRyYWNrQnkgfHwgKChpbmRleDogbnVtYmVyLCBpdGVtOiBhbnkpID0+IChpdGVtIGFzIGFueSkuaWQgfHwgaW5kZXgpO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IoXHJcbiAgICBwcml2YXRlIHRlbXBSZWY6IFRlbXBsYXRlUmVmPEFicEZvckNvbnRleHQ+LFxyXG4gICAgcHJpdmF0ZSB2Y1JlZjogVmlld0NvbnRhaW5lclJlZixcclxuICAgIHByaXZhdGUgZGlmZmVyczogSXRlcmFibGVEaWZmZXJzLFxyXG4gICkge31cclxuXHJcbiAgcHJpdmF0ZSBpdGVyYXRlT3ZlckFwcGxpZWRPcGVyYXRpb25zKGNoYW5nZXM6IEl0ZXJhYmxlQ2hhbmdlczxhbnk+KSB7XHJcbiAgICBjb25zdCBydzogUmVjb3JkVmlld1tdID0gW107XHJcblxyXG4gICAgY2hhbmdlcy5mb3JFYWNoT3BlcmF0aW9uKChyZWNvcmQ6IEl0ZXJhYmxlQ2hhbmdlUmVjb3JkPGFueT4sIHByZXZpb3VzSW5kZXg6IG51bWJlciwgY3VycmVudEluZGV4OiBudW1iZXIpID0+IHtcclxuICAgICAgaWYgKHJlY29yZC5wcmV2aW91c0luZGV4ID09IG51bGwpIHtcclxuICAgICAgICBjb25zdCB2aWV3ID0gdGhpcy52Y1JlZi5jcmVhdGVFbWJlZGRlZFZpZXcoXHJcbiAgICAgICAgICB0aGlzLnRlbXBSZWYsXHJcbiAgICAgICAgICBuZXcgQWJwRm9yQ29udGV4dChudWxsLCAtMSwgLTEsIHRoaXMuaXRlbXMpLFxyXG4gICAgICAgICAgY3VycmVudEluZGV4LFxyXG4gICAgICAgICk7XHJcblxyXG4gICAgICAgIHJ3LnB1c2gobmV3IFJlY29yZFZpZXcocmVjb3JkLCB2aWV3KSk7XHJcbiAgICAgIH0gZWxzZSBpZiAoY3VycmVudEluZGV4ID09IG51bGwpIHtcclxuICAgICAgICB0aGlzLnZjUmVmLnJlbW92ZShwcmV2aW91c0luZGV4KTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICBjb25zdCB2aWV3ID0gdGhpcy52Y1JlZi5nZXQocHJldmlvdXNJbmRleCk7XHJcbiAgICAgICAgdGhpcy52Y1JlZi5tb3ZlKHZpZXcsIGN1cnJlbnRJbmRleCk7XHJcblxyXG4gICAgICAgIHJ3LnB1c2gobmV3IFJlY29yZFZpZXcocmVjb3JkLCB2aWV3IGFzIEVtYmVkZGVkVmlld1JlZjxBYnBGb3JDb250ZXh0PikpO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuXHJcbiAgICBmb3IgKGxldCBpID0gMCwgbCA9IHJ3Lmxlbmd0aDsgaSA8IGw7IGkrKykge1xyXG4gICAgICByd1tpXS52aWV3LmNvbnRleHQuJGltcGxpY2l0ID0gcndbaV0ucmVjb3JkLml0ZW07XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBwcml2YXRlIGl0ZXJhdGVPdmVyQXR0YWNoZWRWaWV3cyhjaGFuZ2VzOiBJdGVyYWJsZUNoYW5nZXM8YW55Pikge1xyXG4gICAgZm9yIChsZXQgaSA9IDAsIGwgPSB0aGlzLnZjUmVmLmxlbmd0aDsgaSA8IGw7IGkrKykge1xyXG4gICAgICBjb25zdCB2aWV3UmVmID0gdGhpcy52Y1JlZi5nZXQoaSkgYXMgRW1iZWRkZWRWaWV3UmVmPEFicEZvckNvbnRleHQ+O1xyXG4gICAgICB2aWV3UmVmLmNvbnRleHQuaW5kZXggPSBpO1xyXG4gICAgICB2aWV3UmVmLmNvbnRleHQuY291bnQgPSBsO1xyXG4gICAgICB2aWV3UmVmLmNvbnRleHQubGlzdCA9IHRoaXMuaXRlbXM7XHJcbiAgICB9XHJcblxyXG4gICAgY2hhbmdlcy5mb3JFYWNoSWRlbnRpdHlDaGFuZ2UoKHJlY29yZDogSXRlcmFibGVDaGFuZ2VSZWNvcmQ8YW55PikgPT4ge1xyXG4gICAgICBjb25zdCB2aWV3UmVmID0gdGhpcy52Y1JlZi5nZXQocmVjb3JkLmN1cnJlbnRJbmRleCkgYXMgRW1iZWRkZWRWaWV3UmVmPEFicEZvckNvbnRleHQ+O1xyXG4gICAgICB2aWV3UmVmLmNvbnRleHQuJGltcGxpY2l0ID0gcmVjb3JkLml0ZW07XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHByaXZhdGUgcHJvamVjdEl0ZW1zKGl0ZW1zOiBhbnlbXSk6IHZvaWQge1xyXG4gICAgaWYgKCFpdGVtcy5sZW5ndGggJiYgdGhpcy5lbXB0eVJlZikge1xyXG4gICAgICB0aGlzLnZjUmVmLmNsZWFyKCk7XHJcbiAgICAgIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tdW51c2VkLWV4cHJlc3Npb25cclxuICAgICAgdGhpcy52Y1JlZi5jcmVhdGVFbWJlZGRlZFZpZXcodGhpcy5lbXB0eVJlZikucm9vdE5vZGVzO1xyXG4gICAgICB0aGlzLmlzU2hvd0VtcHR5UmVmID0gdHJ1ZTtcclxuICAgICAgdGhpcy5kaWZmZXIgPSBudWxsO1xyXG5cclxuICAgICAgcmV0dXJuO1xyXG4gICAgfVxyXG5cclxuICAgIGlmICh0aGlzLmVtcHR5UmVmICYmIHRoaXMuaXNTaG93RW1wdHlSZWYpIHtcclxuICAgICAgdGhpcy52Y1JlZi5jbGVhcigpO1xyXG4gICAgICB0aGlzLmlzU2hvd0VtcHR5UmVmID0gZmFsc2U7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKCF0aGlzLmRpZmZlciAmJiBpdGVtcykge1xyXG4gICAgICB0aGlzLmRpZmZlciA9IHRoaXMuZGlmZmVycy5maW5kKGl0ZW1zKS5jcmVhdGUodGhpcy50cmFja0J5Rm4pO1xyXG4gICAgfVxyXG5cclxuICAgIGlmICh0aGlzLmRpZmZlcikge1xyXG4gICAgICBjb25zdCBjaGFuZ2VzID0gdGhpcy5kaWZmZXIuZGlmZihpdGVtcyk7XHJcblxyXG4gICAgICBpZiAoY2hhbmdlcykge1xyXG4gICAgICAgIHRoaXMuaXRlcmF0ZU92ZXJBcHBsaWVkT3BlcmF0aW9ucyhjaGFuZ2VzKTtcclxuICAgICAgICB0aGlzLml0ZXJhdGVPdmVyQXR0YWNoZWRWaWV3cyhjaGFuZ2VzKTtcclxuICAgICAgfVxyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBzb3J0SXRlbXMoaXRlbXM6IGFueVtdKSB7XHJcbiAgICBpZiAodGhpcy5vcmRlckJ5KSB7XHJcbiAgICAgIGl0ZW1zLnNvcnQoKGEsIGIpID0+IChhW3RoaXMub3JkZXJCeV0gPiBiW3RoaXMub3JkZXJCeV0gPyAxIDogYVt0aGlzLm9yZGVyQnldIDwgYlt0aGlzLm9yZGVyQnldID8gLTEgOiAwKSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBpdGVtcy5zb3J0KCk7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBuZ09uQ2hhbmdlcygpIHtcclxuICAgIGxldCBpdGVtcyA9IGNsb25lKHRoaXMuaXRlbXMpIGFzIGFueVtdO1xyXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGl0ZW1zKSkgcmV0dXJuO1xyXG5cclxuICAgIGNvbnN0IGNvbXBhcmVGbiA9IHRoaXMuY29tcGFyZUZuO1xyXG5cclxuICAgIGlmICh0eXBlb2YgdGhpcy5maWx0ZXJCeSAhPT0gJ3VuZGVmaW5lZCcgJiYgdGhpcy5maWx0ZXJWYWwpIHtcclxuICAgICAgaXRlbXMgPSBpdGVtcy5maWx0ZXIoaXRlbSA9PiBjb21wYXJlRm4oaXRlbVt0aGlzLmZpbHRlckJ5XSwgdGhpcy5maWx0ZXJWYWwpKTtcclxuICAgIH1cclxuXHJcbiAgICBzd2l0Y2ggKHRoaXMub3JkZXJEaXIpIHtcclxuICAgICAgY2FzZSAnQVNDJzpcclxuICAgICAgICB0aGlzLnNvcnRJdGVtcyhpdGVtcyk7XHJcbiAgICAgICAgdGhpcy5wcm9qZWN0SXRlbXMoaXRlbXMpO1xyXG4gICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgY2FzZSAnREVTQyc6XHJcbiAgICAgICAgdGhpcy5zb3J0SXRlbXMoaXRlbXMpO1xyXG4gICAgICAgIGl0ZW1zLnJldmVyc2UoKTtcclxuICAgICAgICB0aGlzLnByb2plY3RJdGVtcyhpdGVtcyk7XHJcbiAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICBkZWZhdWx0OlxyXG4gICAgICAgIHRoaXMucHJvamVjdEl0ZW1zKGl0ZW1zKTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19