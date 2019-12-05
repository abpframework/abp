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
        if (typeof this.filterBy !== 'undefined' && typeof this.filterVal !== 'undefined' && this.filterVal !== '') {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9yLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2Zvci5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUVULEtBQUssRUFJTCxlQUFlLEVBRWYsV0FBVyxFQUVYLGdCQUFnQixHQUNqQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxLQUFLLE1BQU0sWUFBWSxDQUFDO0FBSS9CLE1BQU0sYUFBYTs7Ozs7OztJQUNqQixZQUFtQixTQUFjLEVBQVMsS0FBYSxFQUFTLEtBQWEsRUFBUyxJQUFXO1FBQTlFLGNBQVMsR0FBVCxTQUFTLENBQUs7UUFBUyxVQUFLLEdBQUwsS0FBSyxDQUFRO1FBQVMsVUFBSyxHQUFMLEtBQUssQ0FBUTtRQUFTLFNBQUksR0FBSixJQUFJLENBQU87SUFBRyxDQUFDO0NBQ3RHOzs7SUFEYSxrQ0FBcUI7O0lBQUUsOEJBQW9COztJQUFFLDhCQUFvQjs7SUFBRSw2QkFBa0I7O0FBR25HLE1BQU0sVUFBVTs7Ozs7SUFDZCxZQUFtQixNQUFpQyxFQUFTLElBQW9DO1FBQTlFLFdBQU0sR0FBTixNQUFNLENBQTJCO1FBQVMsU0FBSSxHQUFKLElBQUksQ0FBZ0M7SUFBRyxDQUFDO0NBQ3RHOzs7SUFEYSw0QkFBd0M7O0lBQUUsMEJBQTJDOztBQU1uRyxNQUFNLE9BQU8sWUFBWTs7Ozs7O0lBcUN2QixZQUNVLE9BQW1DLEVBQ25DLEtBQXVCLEVBQ3ZCLE9BQXdCO1FBRnhCLFlBQU8sR0FBUCxPQUFPLENBQTRCO1FBQ25DLFVBQUssR0FBTCxLQUFLLENBQWtCO1FBQ3ZCLFlBQU8sR0FBUCxPQUFPLENBQWlCO0lBQy9CLENBQUM7Ozs7SUFaSixJQUFJLFNBQVM7UUFDWCxPQUFPLElBQUksQ0FBQyxTQUFTLElBQUksT0FBTyxDQUFDO0lBQ25D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WUFFUjtnQkFDRSxJQUFJLENBQUMsWUFBWSxDQUFDLEtBQUssQ0FBQyxDQUFDO1NBQzVCO0lBQ0gsQ0FBQzs7O1lBdEpGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsVUFBVTthQUNyQjs7OztZQW5CQyxXQUFXO1lBRVgsZ0JBQWdCO1lBSmhCLGVBQWU7OztvQkF1QmQsS0FBSyxTQUFDLFVBQVU7c0JBR2hCLEtBQUssU0FBQyxlQUFlO3VCQUdyQixLQUFLLFNBQUMsZ0JBQWdCO3VCQUd0QixLQUFLLFNBQUMsZ0JBQWdCO3dCQUd0QixLQUFLLFNBQUMsaUJBQWlCO3NCQUd2QixLQUFLLFNBQUMsZUFBZTt3QkFHckIsS0FBSyxTQUFDLGlCQUFpQjt1QkFHdkIsS0FBSyxTQUFDLGdCQUFnQjs7OztJQXJCdkIsNkJBQ2E7O0lBRWIsK0JBQ2dCOztJQUVoQixnQ0FDeUI7O0lBRXpCLGdDQUNpQjs7SUFFakIsaUNBQ2U7O0lBRWYsK0JBQ1E7O0lBRVIsaUNBQ3FCOztJQUVyQixnQ0FDMkI7Ozs7O0lBRTNCLDhCQUFvQzs7Ozs7SUFFcEMsc0NBQWdDOzs7OztJQVc5QiwrQkFBMkM7Ozs7O0lBQzNDLDZCQUErQjs7Ozs7SUFDL0IsK0JBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgRGlyZWN0aXZlLFxuICBFbWJlZGRlZFZpZXdSZWYsXG4gIElucHV0LFxuICBJdGVyYWJsZUNoYW5nZVJlY29yZCxcbiAgSXRlcmFibGVDaGFuZ2VzLFxuICBJdGVyYWJsZURpZmZlcixcbiAgSXRlcmFibGVEaWZmZXJzLFxuICBPbkNoYW5nZXMsXG4gIFRlbXBsYXRlUmVmLFxuICBUcmFja0J5RnVuY3Rpb24sXG4gIFZpZXdDb250YWluZXJSZWYsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IGNvbXBhcmUgZnJvbSAnanVzdC1jb21wYXJlJztcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcblxuZXhwb3J0IHR5cGUgQ29tcGFyZUZuPFQgPSBhbnk+ID0gKHZhbHVlOiBULCBjb21wYXJpc29uOiBUKSA9PiBib29sZWFuO1xuXG5jbGFzcyBBYnBGb3JDb250ZXh0IHtcbiAgY29uc3RydWN0b3IocHVibGljICRpbXBsaWNpdDogYW55LCBwdWJsaWMgaW5kZXg6IG51bWJlciwgcHVibGljIGNvdW50OiBudW1iZXIsIHB1YmxpYyBsaXN0OiBhbnlbXSkge31cbn1cblxuY2xhc3MgUmVjb3JkVmlldyB7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyByZWNvcmQ6IEl0ZXJhYmxlQ2hhbmdlUmVjb3JkPGFueT4sIHB1YmxpYyB2aWV3OiBFbWJlZGRlZFZpZXdSZWY8QWJwRm9yQ29udGV4dD4pIHt9XG59XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1thYnBGb3JdJyxcbn0pXG5leHBvcnQgY2xhc3MgRm9yRGlyZWN0aXZlIGltcGxlbWVudHMgT25DaGFuZ2VzIHtcbiAgQElucHV0KCdhYnBGb3JPZicpXG4gIGl0ZW1zOiBhbnlbXTtcblxuICBASW5wdXQoJ2FicEZvck9yZGVyQnknKVxuICBvcmRlckJ5OiBzdHJpbmc7XG5cbiAgQElucHV0KCdhYnBGb3JPcmRlckRpcicpXG4gIG9yZGVyRGlyOiAnQVNDJyB8ICdERVNDJztcblxuICBASW5wdXQoJ2FicEZvckZpbHRlckJ5JylcbiAgZmlsdGVyQnk6IHN0cmluZztcblxuICBASW5wdXQoJ2FicEZvckZpbHRlclZhbCcpXG4gIGZpbHRlclZhbDogYW55O1xuXG4gIEBJbnB1dCgnYWJwRm9yVHJhY2tCeScpXG4gIHRyYWNrQnk7XG5cbiAgQElucHV0KCdhYnBGb3JDb21wYXJlQnknKVxuICBjb21wYXJlQnk6IENvbXBhcmVGbjtcblxuICBASW5wdXQoJ2FicEZvckVtcHR5UmVmJylcbiAgZW1wdHlSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgcHJpdmF0ZSBkaWZmZXI6IEl0ZXJhYmxlRGlmZmVyPGFueT47XG5cbiAgcHJpdmF0ZSBpc1Nob3dFbXB0eVJlZjogYm9vbGVhbjtcblxuICBnZXQgY29tcGFyZUZuKCk6IENvbXBhcmVGbiB7XG4gICAgcmV0dXJuIHRoaXMuY29tcGFyZUJ5IHx8IGNvbXBhcmU7XG4gIH1cblxuICBnZXQgdHJhY2tCeUZuKCk6IFRyYWNrQnlGdW5jdGlvbjxhbnk+IHtcbiAgICByZXR1cm4gdGhpcy50cmFja0J5IHx8ICgoaW5kZXg6IG51bWJlciwgaXRlbTogYW55KSA9PiAoaXRlbSBhcyBhbnkpLmlkIHx8IGluZGV4KTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKFxuICAgIHByaXZhdGUgdGVtcFJlZjogVGVtcGxhdGVSZWY8QWJwRm9yQ29udGV4dD4sXG4gICAgcHJpdmF0ZSB2Y1JlZjogVmlld0NvbnRhaW5lclJlZixcbiAgICBwcml2YXRlIGRpZmZlcnM6IEl0ZXJhYmxlRGlmZmVycyxcbiAgKSB7fVxuXG4gIHByaXZhdGUgaXRlcmF0ZU92ZXJBcHBsaWVkT3BlcmF0aW9ucyhjaGFuZ2VzOiBJdGVyYWJsZUNoYW5nZXM8YW55Pikge1xuICAgIGNvbnN0IHJ3OiBSZWNvcmRWaWV3W10gPSBbXTtcblxuICAgIGNoYW5nZXMuZm9yRWFjaE9wZXJhdGlvbigocmVjb3JkOiBJdGVyYWJsZUNoYW5nZVJlY29yZDxhbnk+LCBwcmV2aW91c0luZGV4OiBudW1iZXIsIGN1cnJlbnRJbmRleDogbnVtYmVyKSA9PiB7XG4gICAgICBpZiAocmVjb3JkLnByZXZpb3VzSW5kZXggPT0gbnVsbCkge1xuICAgICAgICBjb25zdCB2aWV3ID0gdGhpcy52Y1JlZi5jcmVhdGVFbWJlZGRlZFZpZXcoXG4gICAgICAgICAgdGhpcy50ZW1wUmVmLFxuICAgICAgICAgIG5ldyBBYnBGb3JDb250ZXh0KG51bGwsIC0xLCAtMSwgdGhpcy5pdGVtcyksXG4gICAgICAgICAgY3VycmVudEluZGV4LFxuICAgICAgICApO1xuXG4gICAgICAgIHJ3LnB1c2gobmV3IFJlY29yZFZpZXcocmVjb3JkLCB2aWV3KSk7XG4gICAgICB9IGVsc2UgaWYgKGN1cnJlbnRJbmRleCA9PSBudWxsKSB7XG4gICAgICAgIHRoaXMudmNSZWYucmVtb3ZlKHByZXZpb3VzSW5kZXgpO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgY29uc3QgdmlldyA9IHRoaXMudmNSZWYuZ2V0KHByZXZpb3VzSW5kZXgpO1xuICAgICAgICB0aGlzLnZjUmVmLm1vdmUodmlldywgY3VycmVudEluZGV4KTtcblxuICAgICAgICBydy5wdXNoKG5ldyBSZWNvcmRWaWV3KHJlY29yZCwgdmlldyBhcyBFbWJlZGRlZFZpZXdSZWY8QWJwRm9yQ29udGV4dD4pKTtcbiAgICAgIH1cbiAgICB9KTtcblxuICAgIGZvciAobGV0IGkgPSAwLCBsID0gcncubGVuZ3RoOyBpIDwgbDsgaSsrKSB7XG4gICAgICByd1tpXS52aWV3LmNvbnRleHQuJGltcGxpY2l0ID0gcndbaV0ucmVjb3JkLml0ZW07XG4gICAgfVxuICB9XG5cbiAgcHJpdmF0ZSBpdGVyYXRlT3ZlckF0dGFjaGVkVmlld3MoY2hhbmdlczogSXRlcmFibGVDaGFuZ2VzPGFueT4pIHtcbiAgICBmb3IgKGxldCBpID0gMCwgbCA9IHRoaXMudmNSZWYubGVuZ3RoOyBpIDwgbDsgaSsrKSB7XG4gICAgICBjb25zdCB2aWV3UmVmID0gdGhpcy52Y1JlZi5nZXQoaSkgYXMgRW1iZWRkZWRWaWV3UmVmPEFicEZvckNvbnRleHQ+O1xuICAgICAgdmlld1JlZi5jb250ZXh0LmluZGV4ID0gaTtcbiAgICAgIHZpZXdSZWYuY29udGV4dC5jb3VudCA9IGw7XG4gICAgICB2aWV3UmVmLmNvbnRleHQubGlzdCA9IHRoaXMuaXRlbXM7XG4gICAgfVxuXG4gICAgY2hhbmdlcy5mb3JFYWNoSWRlbnRpdHlDaGFuZ2UoKHJlY29yZDogSXRlcmFibGVDaGFuZ2VSZWNvcmQ8YW55PikgPT4ge1xuICAgICAgY29uc3Qgdmlld1JlZiA9IHRoaXMudmNSZWYuZ2V0KHJlY29yZC5jdXJyZW50SW5kZXgpIGFzIEVtYmVkZGVkVmlld1JlZjxBYnBGb3JDb250ZXh0PjtcbiAgICAgIHZpZXdSZWYuY29udGV4dC4kaW1wbGljaXQgPSByZWNvcmQuaXRlbTtcbiAgICB9KTtcbiAgfVxuXG4gIHByaXZhdGUgcHJvamVjdEl0ZW1zKGl0ZW1zOiBhbnlbXSk6IHZvaWQge1xuICAgIGlmICghaXRlbXMubGVuZ3RoICYmIHRoaXMuZW1wdHlSZWYpIHtcbiAgICAgIHRoaXMudmNSZWYuY2xlYXIoKTtcbiAgICAgIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tdW51c2VkLWV4cHJlc3Npb25cbiAgICAgIHRoaXMudmNSZWYuY3JlYXRlRW1iZWRkZWRWaWV3KHRoaXMuZW1wdHlSZWYpLnJvb3ROb2RlcztcbiAgICAgIHRoaXMuaXNTaG93RW1wdHlSZWYgPSB0cnVlO1xuICAgICAgdGhpcy5kaWZmZXIgPSBudWxsO1xuXG4gICAgICByZXR1cm47XG4gICAgfVxuXG4gICAgaWYgKHRoaXMuZW1wdHlSZWYgJiYgdGhpcy5pc1Nob3dFbXB0eVJlZikge1xuICAgICAgdGhpcy52Y1JlZi5jbGVhcigpO1xuICAgICAgdGhpcy5pc1Nob3dFbXB0eVJlZiA9IGZhbHNlO1xuICAgIH1cblxuICAgIGlmICghdGhpcy5kaWZmZXIgJiYgaXRlbXMpIHtcbiAgICAgIHRoaXMuZGlmZmVyID0gdGhpcy5kaWZmZXJzLmZpbmQoaXRlbXMpLmNyZWF0ZSh0aGlzLnRyYWNrQnlGbik7XG4gICAgfVxuXG4gICAgaWYgKHRoaXMuZGlmZmVyKSB7XG4gICAgICBjb25zdCBjaGFuZ2VzID0gdGhpcy5kaWZmZXIuZGlmZihpdGVtcyk7XG5cbiAgICAgIGlmIChjaGFuZ2VzKSB7XG4gICAgICAgIHRoaXMuaXRlcmF0ZU92ZXJBcHBsaWVkT3BlcmF0aW9ucyhjaGFuZ2VzKTtcbiAgICAgICAgdGhpcy5pdGVyYXRlT3ZlckF0dGFjaGVkVmlld3MoY2hhbmdlcyk7XG4gICAgICB9XG4gICAgfVxuICB9XG5cbiAgcHJpdmF0ZSBzb3J0SXRlbXMoaXRlbXM6IGFueVtdKSB7XG4gICAgaWYgKHRoaXMub3JkZXJCeSkge1xuICAgICAgaXRlbXMuc29ydCgoYSwgYikgPT4gKGFbdGhpcy5vcmRlckJ5XSA+IGJbdGhpcy5vcmRlckJ5XSA/IDEgOiBhW3RoaXMub3JkZXJCeV0gPCBiW3RoaXMub3JkZXJCeV0gPyAtMSA6IDApKTtcbiAgICB9IGVsc2Uge1xuICAgICAgaXRlbXMuc29ydCgpO1xuICAgIH1cbiAgfVxuXG4gIG5nT25DaGFuZ2VzKCkge1xuICAgIGxldCBpdGVtcyA9IGNsb25lKHRoaXMuaXRlbXMpIGFzIGFueVtdO1xuICAgIGlmICghQXJyYXkuaXNBcnJheShpdGVtcykpIHJldHVybjtcblxuICAgIGNvbnN0IGNvbXBhcmVGbiA9IHRoaXMuY29tcGFyZUZuO1xuXG4gICAgaWYgKHR5cGVvZiB0aGlzLmZpbHRlckJ5ICE9PSAndW5kZWZpbmVkJyAmJiB0eXBlb2YgdGhpcy5maWx0ZXJWYWwgIT09ICd1bmRlZmluZWQnICYmIHRoaXMuZmlsdGVyVmFsICE9PSAnJykge1xuICAgICAgaXRlbXMgPSBpdGVtcy5maWx0ZXIoaXRlbSA9PiBjb21wYXJlRm4oaXRlbVt0aGlzLmZpbHRlckJ5XSwgdGhpcy5maWx0ZXJWYWwpKTtcbiAgICB9XG5cbiAgICBzd2l0Y2ggKHRoaXMub3JkZXJEaXIpIHtcbiAgICAgIGNhc2UgJ0FTQyc6XG4gICAgICAgIHRoaXMuc29ydEl0ZW1zKGl0ZW1zKTtcbiAgICAgICAgdGhpcy5wcm9qZWN0SXRlbXMoaXRlbXMpO1xuICAgICAgICBicmVhaztcblxuICAgICAgY2FzZSAnREVTQyc6XG4gICAgICAgIHRoaXMuc29ydEl0ZW1zKGl0ZW1zKTtcbiAgICAgICAgaXRlbXMucmV2ZXJzZSgpO1xuICAgICAgICB0aGlzLnByb2plY3RJdGVtcyhpdGVtcyk7XG4gICAgICAgIGJyZWFrO1xuXG4gICAgICBkZWZhdWx0OlxuICAgICAgICB0aGlzLnByb2plY3RJdGVtcyhpdGVtcyk7XG4gICAgfVxuICB9XG59XG4iXX0=