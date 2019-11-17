/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/for.directive.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9yLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2Zvci5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUVULEtBQUssRUFJTCxlQUFlLEVBRWYsV0FBVyxFQUVYLGdCQUFnQixHQUNqQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxLQUFLLE1BQU0sWUFBWSxDQUFDO0FBSS9CLE1BQU0sYUFBYTs7Ozs7OztJQUNqQixZQUFtQixTQUFjLEVBQVMsS0FBYSxFQUFTLEtBQWEsRUFBUyxJQUFXO1FBQTlFLGNBQVMsR0FBVCxTQUFTLENBQUs7UUFBUyxVQUFLLEdBQUwsS0FBSyxDQUFRO1FBQVMsVUFBSyxHQUFMLEtBQUssQ0FBUTtRQUFTLFNBQUksR0FBSixJQUFJLENBQU87SUFBRyxDQUFDO0NBQ3RHOzs7SUFEYSxrQ0FBcUI7O0lBQUUsOEJBQW9COztJQUFFLDhCQUFvQjs7SUFBRSw2QkFBa0I7O0FBR25HLE1BQU0sVUFBVTs7Ozs7SUFDZCxZQUFtQixNQUFpQyxFQUFTLElBQW9DO1FBQTlFLFdBQU0sR0FBTixNQUFNLENBQTJCO1FBQVMsU0FBSSxHQUFKLElBQUksQ0FBZ0M7SUFBRyxDQUFDO0NBQ3RHOzs7SUFEYSw0QkFBd0M7O0lBQUUsMEJBQTJDOztBQU1uRyxNQUFNLE9BQU8sWUFBWTs7Ozs7O0lBcUN2QixZQUNVLE9BQW1DLEVBQ25DLEtBQXVCLEVBQ3ZCLE9BQXdCO1FBRnhCLFlBQU8sR0FBUCxPQUFPLENBQTRCO1FBQ25DLFVBQUssR0FBTCxLQUFLLENBQWtCO1FBQ3ZCLFlBQU8sR0FBUCxPQUFPLENBQWlCO0lBQy9CLENBQUM7Ozs7SUFaSixJQUFJLFNBQVM7UUFDWCxPQUFPLElBQUksQ0FBQyxTQUFTLElBQUksT0FBTyxDQUFDO0lBQ25D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O1NBQzVCO0lBQ0gsQ0FBQzs7O1lBdEpGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsVUFBVTthQUNyQjs7OztZQW5CQyxXQUFXO1lBRVgsZ0JBQWdCO1lBSmhCLGVBQWU7OztvQkF1QmQsS0FBSyxTQUFDLFVBQVU7c0JBR2hCLEtBQUssU0FBQyxlQUFlO3VCQUdyQixLQUFLLFNBQUMsZ0JBQWdCO3VCQUd0QixLQUFLLFNBQUMsZ0JBQWdCO3dCQUd0QixLQUFLLFNBQUMsaUJBQWlCO3NCQUd2QixLQUFLLFNBQUMsZUFBZTt3QkFHckIsS0FBSyxTQUFDLGlCQUFpQjt1QkFHdkIsS0FBSyxTQUFDLGdCQUFnQjs7OztJQXJCdkIsNkJBQ2E7O0lBRWIsK0JBQ2dCOztJQUVoQixnQ0FDeUI7O0lBRXpCLGdDQUNpQjs7SUFFakIsaUNBQ2U7O0lBRWYsK0JBQ1E7O0lBRVIsaUNBQ3FCOztJQUVyQixnQ0FDMkI7Ozs7O0lBRTNCLDhCQUFvQzs7Ozs7SUFFcEMsc0NBQWdDOzs7OztJQVc5QiwrQkFBMkM7Ozs7O0lBQzNDLDZCQUErQjs7Ozs7SUFDL0IsK0JBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBEaXJlY3RpdmUsXHJcbiAgRW1iZWRkZWRWaWV3UmVmLFxyXG4gIElucHV0LFxyXG4gIEl0ZXJhYmxlQ2hhbmdlUmVjb3JkLFxyXG4gIEl0ZXJhYmxlQ2hhbmdlcyxcclxuICBJdGVyYWJsZURpZmZlcixcclxuICBJdGVyYWJsZURpZmZlcnMsXHJcbiAgT25DaGFuZ2VzLFxyXG4gIFRlbXBsYXRlUmVmLFxyXG4gIFRyYWNrQnlGdW5jdGlvbixcclxuICBWaWV3Q29udGFpbmVyUmVmLFxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgY29tcGFyZSBmcm9tICdqdXN0LWNvbXBhcmUnO1xyXG5pbXBvcnQgY2xvbmUgZnJvbSAnanVzdC1jbG9uZSc7XHJcblxyXG5leHBvcnQgdHlwZSBDb21wYXJlRm48VCA9IGFueT4gPSAodmFsdWU6IFQsIGNvbXBhcmlzb246IFQpID0+IGJvb2xlYW47XHJcblxyXG5jbGFzcyBBYnBGb3JDb250ZXh0IHtcclxuICBjb25zdHJ1Y3RvcihwdWJsaWMgJGltcGxpY2l0OiBhbnksIHB1YmxpYyBpbmRleDogbnVtYmVyLCBwdWJsaWMgY291bnQ6IG51bWJlciwgcHVibGljIGxpc3Q6IGFueVtdKSB7fVxyXG59XHJcblxyXG5jbGFzcyBSZWNvcmRWaWV3IHtcclxuICBjb25zdHJ1Y3RvcihwdWJsaWMgcmVjb3JkOiBJdGVyYWJsZUNoYW5nZVJlY29yZDxhbnk+LCBwdWJsaWMgdmlldzogRW1iZWRkZWRWaWV3UmVmPEFicEZvckNvbnRleHQ+KSB7fVxyXG59XHJcblxyXG5ARGlyZWN0aXZlKHtcclxuICBzZWxlY3RvcjogJ1thYnBGb3JdJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIEZvckRpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uQ2hhbmdlcyB7XHJcbiAgQElucHV0KCdhYnBGb3JPZicpXHJcbiAgaXRlbXM6IGFueVtdO1xyXG5cclxuICBASW5wdXQoJ2FicEZvck9yZGVyQnknKVxyXG4gIG9yZGVyQnk6IHN0cmluZztcclxuXHJcbiAgQElucHV0KCdhYnBGb3JPcmRlckRpcicpXHJcbiAgb3JkZXJEaXI6ICdBU0MnIHwgJ0RFU0MnO1xyXG5cclxuICBASW5wdXQoJ2FicEZvckZpbHRlckJ5JylcclxuICBmaWx0ZXJCeTogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoJ2FicEZvckZpbHRlclZhbCcpXHJcbiAgZmlsdGVyVmFsOiBhbnk7XHJcblxyXG4gIEBJbnB1dCgnYWJwRm9yVHJhY2tCeScpXHJcbiAgdHJhY2tCeTtcclxuXHJcbiAgQElucHV0KCdhYnBGb3JDb21wYXJlQnknKVxyXG4gIGNvbXBhcmVCeTogQ29tcGFyZUZuO1xyXG5cclxuICBASW5wdXQoJ2FicEZvckVtcHR5UmVmJylcclxuICBlbXB0eVJlZjogVGVtcGxhdGVSZWY8YW55PjtcclxuXHJcbiAgcHJpdmF0ZSBkaWZmZXI6IEl0ZXJhYmxlRGlmZmVyPGFueT47XHJcblxyXG4gIHByaXZhdGUgaXNTaG93RW1wdHlSZWY6IGJvb2xlYW47XHJcblxyXG4gIGdldCBjb21wYXJlRm4oKTogQ29tcGFyZUZuIHtcclxuICAgIHJldHVybiB0aGlzLmNvbXBhcmVCeSB8fCBjb21wYXJlO1xyXG4gIH1cclxuXHJcbiAgZ2V0IHRyYWNrQnlGbigpOiBUcmFja0J5RnVuY3Rpb248YW55PiB7XHJcbiAgICByZXR1cm4gdGhpcy50cmFja0J5IHx8ICgoaW5kZXg6IG51bWJlciwgaXRlbTogYW55KSA9PiAoaXRlbSBhcyBhbnkpLmlkIHx8IGluZGV4KTtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHJpdmF0ZSB0ZW1wUmVmOiBUZW1wbGF0ZVJlZjxBYnBGb3JDb250ZXh0PixcclxuICAgIHByaXZhdGUgdmNSZWY6IFZpZXdDb250YWluZXJSZWYsXHJcbiAgICBwcml2YXRlIGRpZmZlcnM6IEl0ZXJhYmxlRGlmZmVycyxcclxuICApIHt9XHJcblxyXG4gIHByaXZhdGUgaXRlcmF0ZU92ZXJBcHBsaWVkT3BlcmF0aW9ucyhjaGFuZ2VzOiBJdGVyYWJsZUNoYW5nZXM8YW55Pikge1xyXG4gICAgY29uc3Qgcnc6IFJlY29yZFZpZXdbXSA9IFtdO1xyXG5cclxuICAgIGNoYW5nZXMuZm9yRWFjaE9wZXJhdGlvbigocmVjb3JkOiBJdGVyYWJsZUNoYW5nZVJlY29yZDxhbnk+LCBwcmV2aW91c0luZGV4OiBudW1iZXIsIGN1cnJlbnRJbmRleDogbnVtYmVyKSA9PiB7XHJcbiAgICAgIGlmIChyZWNvcmQucHJldmlvdXNJbmRleCA9PSBudWxsKSB7XHJcbiAgICAgICAgY29uc3QgdmlldyA9IHRoaXMudmNSZWYuY3JlYXRlRW1iZWRkZWRWaWV3KFxyXG4gICAgICAgICAgdGhpcy50ZW1wUmVmLFxyXG4gICAgICAgICAgbmV3IEFicEZvckNvbnRleHQobnVsbCwgLTEsIC0xLCB0aGlzLml0ZW1zKSxcclxuICAgICAgICAgIGN1cnJlbnRJbmRleCxcclxuICAgICAgICApO1xyXG5cclxuICAgICAgICBydy5wdXNoKG5ldyBSZWNvcmRWaWV3KHJlY29yZCwgdmlldykpO1xyXG4gICAgICB9IGVsc2UgaWYgKGN1cnJlbnRJbmRleCA9PSBudWxsKSB7XHJcbiAgICAgICAgdGhpcy52Y1JlZi5yZW1vdmUocHJldmlvdXNJbmRleCk7XHJcbiAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgY29uc3QgdmlldyA9IHRoaXMudmNSZWYuZ2V0KHByZXZpb3VzSW5kZXgpO1xyXG4gICAgICAgIHRoaXMudmNSZWYubW92ZSh2aWV3LCBjdXJyZW50SW5kZXgpO1xyXG5cclxuICAgICAgICBydy5wdXNoKG5ldyBSZWNvcmRWaWV3KHJlY29yZCwgdmlldyBhcyBFbWJlZGRlZFZpZXdSZWY8QWJwRm9yQ29udGV4dD4pKTtcclxuICAgICAgfVxyXG4gICAgfSk7XHJcblxyXG4gICAgZm9yIChsZXQgaSA9IDAsIGwgPSBydy5sZW5ndGg7IGkgPCBsOyBpKyspIHtcclxuICAgICAgcndbaV0udmlldy5jb250ZXh0LiRpbXBsaWNpdCA9IHJ3W2ldLnJlY29yZC5pdGVtO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBpdGVyYXRlT3ZlckF0dGFjaGVkVmlld3MoY2hhbmdlczogSXRlcmFibGVDaGFuZ2VzPGFueT4pIHtcclxuICAgIGZvciAobGV0IGkgPSAwLCBsID0gdGhpcy52Y1JlZi5sZW5ndGg7IGkgPCBsOyBpKyspIHtcclxuICAgICAgY29uc3Qgdmlld1JlZiA9IHRoaXMudmNSZWYuZ2V0KGkpIGFzIEVtYmVkZGVkVmlld1JlZjxBYnBGb3JDb250ZXh0PjtcclxuICAgICAgdmlld1JlZi5jb250ZXh0LmluZGV4ID0gaTtcclxuICAgICAgdmlld1JlZi5jb250ZXh0LmNvdW50ID0gbDtcclxuICAgICAgdmlld1JlZi5jb250ZXh0Lmxpc3QgPSB0aGlzLml0ZW1zO1xyXG4gICAgfVxyXG5cclxuICAgIGNoYW5nZXMuZm9yRWFjaElkZW50aXR5Q2hhbmdlKChyZWNvcmQ6IEl0ZXJhYmxlQ2hhbmdlUmVjb3JkPGFueT4pID0+IHtcclxuICAgICAgY29uc3Qgdmlld1JlZiA9IHRoaXMudmNSZWYuZ2V0KHJlY29yZC5jdXJyZW50SW5kZXgpIGFzIEVtYmVkZGVkVmlld1JlZjxBYnBGb3JDb250ZXh0PjtcclxuICAgICAgdmlld1JlZi5jb250ZXh0LiRpbXBsaWNpdCA9IHJlY29yZC5pdGVtO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBwcml2YXRlIHByb2plY3RJdGVtcyhpdGVtczogYW55W10pOiB2b2lkIHtcclxuICAgIGlmICghaXRlbXMubGVuZ3RoICYmIHRoaXMuZW1wdHlSZWYpIHtcclxuICAgICAgdGhpcy52Y1JlZi5jbGVhcigpO1xyXG4gICAgICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLXVudXNlZC1leHByZXNzaW9uXHJcbiAgICAgIHRoaXMudmNSZWYuY3JlYXRlRW1iZWRkZWRWaWV3KHRoaXMuZW1wdHlSZWYpLnJvb3ROb2RlcztcclxuICAgICAgdGhpcy5pc1Nob3dFbXB0eVJlZiA9IHRydWU7XHJcbiAgICAgIHRoaXMuZGlmZmVyID0gbnVsbDtcclxuXHJcbiAgICAgIHJldHVybjtcclxuICAgIH1cclxuXHJcbiAgICBpZiAodGhpcy5lbXB0eVJlZiAmJiB0aGlzLmlzU2hvd0VtcHR5UmVmKSB7XHJcbiAgICAgIHRoaXMudmNSZWYuY2xlYXIoKTtcclxuICAgICAgdGhpcy5pc1Nob3dFbXB0eVJlZiA9IGZhbHNlO1xyXG4gICAgfVxyXG5cclxuICAgIGlmICghdGhpcy5kaWZmZXIgJiYgaXRlbXMpIHtcclxuICAgICAgdGhpcy5kaWZmZXIgPSB0aGlzLmRpZmZlcnMuZmluZChpdGVtcykuY3JlYXRlKHRoaXMudHJhY2tCeUZuKTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAodGhpcy5kaWZmZXIpIHtcclxuICAgICAgY29uc3QgY2hhbmdlcyA9IHRoaXMuZGlmZmVyLmRpZmYoaXRlbXMpO1xyXG5cclxuICAgICAgaWYgKGNoYW5nZXMpIHtcclxuICAgICAgICB0aGlzLml0ZXJhdGVPdmVyQXBwbGllZE9wZXJhdGlvbnMoY2hhbmdlcyk7XHJcbiAgICAgICAgdGhpcy5pdGVyYXRlT3ZlckF0dGFjaGVkVmlld3MoY2hhbmdlcyk7XHJcbiAgICAgIH1cclxuICAgIH1cclxuICB9XHJcblxyXG4gIHByaXZhdGUgc29ydEl0ZW1zKGl0ZW1zOiBhbnlbXSkge1xyXG4gICAgaWYgKHRoaXMub3JkZXJCeSkge1xyXG4gICAgICBpdGVtcy5zb3J0KChhLCBiKSA9PiAoYVt0aGlzLm9yZGVyQnldID4gYlt0aGlzLm9yZGVyQnldID8gMSA6IGFbdGhpcy5vcmRlckJ5XSA8IGJbdGhpcy5vcmRlckJ5XSA/IC0xIDogMCkpO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgaXRlbXMuc29ydCgpO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgbmdPbkNoYW5nZXMoKSB7XHJcbiAgICBsZXQgaXRlbXMgPSBjbG9uZSh0aGlzLml0ZW1zKSBhcyBhbnlbXTtcclxuICAgIGlmICghQXJyYXkuaXNBcnJheShpdGVtcykpIHJldHVybjtcclxuXHJcbiAgICBjb25zdCBjb21wYXJlRm4gPSB0aGlzLmNvbXBhcmVGbjtcclxuXHJcbiAgICBpZiAodHlwZW9mIHRoaXMuZmlsdGVyQnkgIT09ICd1bmRlZmluZWQnICYmIHRoaXMuZmlsdGVyVmFsKSB7XHJcbiAgICAgIGl0ZW1zID0gaXRlbXMuZmlsdGVyKGl0ZW0gPT4gY29tcGFyZUZuKGl0ZW1bdGhpcy5maWx0ZXJCeV0sIHRoaXMuZmlsdGVyVmFsKSk7XHJcbiAgICB9XHJcblxyXG4gICAgc3dpdGNoICh0aGlzLm9yZGVyRGlyKSB7XHJcbiAgICAgIGNhc2UgJ0FTQyc6XHJcbiAgICAgICAgdGhpcy5zb3J0SXRlbXMoaXRlbXMpO1xyXG4gICAgICAgIHRoaXMucHJvamVjdEl0ZW1zKGl0ZW1zKTtcclxuICAgICAgICBicmVhaztcclxuXHJcbiAgICAgIGNhc2UgJ0RFU0MnOlxyXG4gICAgICAgIHRoaXMuc29ydEl0ZW1zKGl0ZW1zKTtcclxuICAgICAgICBpdGVtcy5yZXZlcnNlKCk7XHJcbiAgICAgICAgdGhpcy5wcm9qZWN0SXRlbXMoaXRlbXMpO1xyXG4gICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgZGVmYXVsdDpcclxuICAgICAgICB0aGlzLnByb2plY3RJdGVtcyhpdGVtcyk7XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==