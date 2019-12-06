/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/table-sort.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, Optional, Self } from '@angular/core';
import { Table } from 'primeng/table';
import clone from 'just-clone';
import { SortPipe } from '@abp/ng.core';
/**
 * @record
 */
export function TableSortOptions() { }
if (false) {
    /** @type {?} */
    TableSortOptions.prototype.key;
    /** @type {?} */
    TableSortOptions.prototype.order;
}
export class TableSortDirective {
    /**
     * @param {?} table
     * @param {?} sortPipe
     */
    constructor(table, sortPipe) {
        this.table = table;
        this.sortPipe = sortPipe;
        this.value = [];
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    ngOnChanges({ value, abpTableSort }) {
        if (value || abpTableSort) {
            this.abpTableSort = this.abpTableSort || ((/** @type {?} */ ({})));
            this.table.value = this.sortPipe.transform(clone(this.value), this.abpTableSort.order, this.abpTableSort.key);
        }
    }
}
TableSortDirective.decorators = [
    { type: Directive, args: [{
                selector: '[abpTableSort]',
                providers: [SortPipe],
            },] }
];
/** @nocollapse */
TableSortDirective.ctorParameters = () => [
    { type: Table, decorators: [{ type: Optional }, { type: Self }] },
    { type: SortPipe }
];
TableSortDirective.propDecorators = {
    abpTableSort: [{ type: Input }],
    value: [{ type: Input }]
};
if (false) {
    /** @type {?} */
    TableSortDirective.prototype.abpTableSort;
    /** @type {?} */
    TableSortDirective.prototype.value;
    /**
     * @type {?}
     * @private
     */
    TableSortDirective.prototype.table;
    /**
     * @type {?}
     * @private
     */
    TableSortDirective.prototype.sortPipe;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtc29ydC5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL3RhYmxlLXNvcnQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLElBQUksRUFBNEIsTUFBTSxlQUFlLENBQUM7QUFDM0YsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN0QyxPQUFPLEtBQUssTUFBTSxZQUFZLENBQUM7QUFDL0IsT0FBTyxFQUFFLFFBQVEsRUFBYSxNQUFNLGNBQWMsQ0FBQzs7OztBQUVuRCxzQ0FHQzs7O0lBRkMsK0JBQVk7O0lBQ1osaUNBQWlCOztBQU9uQixNQUFNLE9BQU8sa0JBQWtCOzs7OztJQUs3QixZQUF3QyxLQUFZLEVBQVUsUUFBa0I7UUFBeEMsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLGFBQVEsR0FBUixRQUFRLENBQVU7UUFEaEYsVUFBSyxHQUFVLEVBQUUsQ0FBQztJQUNpRSxDQUFDOzs7OztJQUNwRixXQUFXLENBQUMsRUFBRSxLQUFLLEVBQUUsWUFBWSxFQUFpQjtRQUNoRCxJQUFJLEtBQUssSUFBSSxZQUFZLEVBQUU7WUFDekIsSUFBSSxDQUFDLFlBQVksR0FBRyxJQUFJLENBQUMsWUFBWSxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFvQixDQUFDLENBQUM7WUFDbEUsSUFBSSxDQUFDLEtBQUssQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxTQUFTLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsRUFBRSxJQUFJLENBQUMsWUFBWSxDQUFDLEtBQUssRUFBRSxJQUFJLENBQUMsWUFBWSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1NBQy9HO0lBQ0gsQ0FBQzs7O1lBZkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxnQkFBZ0I7Z0JBQzFCLFNBQVMsRUFBRSxDQUFDLFFBQVEsQ0FBQzthQUN0Qjs7OztZQVpRLEtBQUssdUJBa0JDLFFBQVEsWUFBSSxJQUFJO1lBaEJ0QixRQUFROzs7MkJBWWQsS0FBSztvQkFFTCxLQUFLOzs7O0lBRk4sMENBQytCOztJQUMvQixtQ0FDa0I7Ozs7O0lBQ04sbUNBQXdDOzs7OztJQUFFLHNDQUEwQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgSW5wdXQsIE9wdGlvbmFsLCBTZWxmLCBTaW1wbGVDaGFuZ2VzLCBPbkNoYW5nZXMgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgVGFibGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcclxuaW1wb3J0IGNsb25lIGZyb20gJ2p1c3QtY2xvbmUnO1xyXG5pbXBvcnQgeyBTb3J0UGlwZSwgU29ydE9yZGVyIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbmV4cG9ydCBpbnRlcmZhY2UgVGFibGVTb3J0T3B0aW9ucyB7XHJcbiAga2V5OiBzdHJpbmc7XHJcbiAgb3JkZXI6IFNvcnRPcmRlcjtcclxufVxyXG5cclxuQERpcmVjdGl2ZSh7XHJcbiAgc2VsZWN0b3I6ICdbYWJwVGFibGVTb3J0XScsXHJcbiAgcHJvdmlkZXJzOiBbU29ydFBpcGVdLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVGFibGVTb3J0RGlyZWN0aXZlIGltcGxlbWVudHMgT25DaGFuZ2VzIHtcclxuICBASW5wdXQoKVxyXG4gIGFicFRhYmxlU29ydDogVGFibGVTb3J0T3B0aW9ucztcclxuICBASW5wdXQoKVxyXG4gIHZhbHVlOiBhbnlbXSA9IFtdO1xyXG4gIGNvbnN0cnVjdG9yKEBPcHRpb25hbCgpIEBTZWxmKCkgcHJpdmF0ZSB0YWJsZTogVGFibGUsIHByaXZhdGUgc29ydFBpcGU6IFNvcnRQaXBlKSB7fVxyXG4gIG5nT25DaGFuZ2VzKHsgdmFsdWUsIGFicFRhYmxlU29ydCB9OiBTaW1wbGVDaGFuZ2VzKSB7XHJcbiAgICBpZiAodmFsdWUgfHwgYWJwVGFibGVTb3J0KSB7XHJcbiAgICAgIHRoaXMuYWJwVGFibGVTb3J0ID0gdGhpcy5hYnBUYWJsZVNvcnQgfHwgKHt9IGFzIFRhYmxlU29ydE9wdGlvbnMpO1xyXG4gICAgICB0aGlzLnRhYmxlLnZhbHVlID0gdGhpcy5zb3J0UGlwZS50cmFuc2Zvcm0oY2xvbmUodGhpcy52YWx1ZSksIHRoaXMuYWJwVGFibGVTb3J0Lm9yZGVyLCB0aGlzLmFicFRhYmxlU29ydC5rZXkpO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=