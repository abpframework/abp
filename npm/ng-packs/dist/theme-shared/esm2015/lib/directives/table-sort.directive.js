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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtc29ydC5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL3RhYmxlLXNvcnQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLElBQUksRUFBNEIsTUFBTSxlQUFlLENBQUM7QUFDM0YsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN0QyxPQUFPLEtBQUssTUFBTSxZQUFZLENBQUM7QUFDL0IsT0FBTyxFQUFFLFFBQVEsRUFBYSxNQUFNLGNBQWMsQ0FBQzs7OztBQUVuRCxzQ0FHQzs7O0lBRkMsK0JBQVk7O0lBQ1osaUNBQWlCOztBQU9uQixNQUFNLE9BQU8sa0JBQWtCOzs7OztJQUs3QixZQUF3QyxLQUFZLEVBQVUsUUFBa0I7UUFBeEMsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLGFBQVEsR0FBUixRQUFRLENBQVU7UUFEaEYsVUFBSyxHQUFVLEVBQUUsQ0FBQztJQUNpRSxDQUFDOzs7OztJQUNwRixXQUFXLENBQUMsRUFBRSxLQUFLLEVBQUUsWUFBWSxFQUFpQjtRQUNoRCxJQUFJLEtBQUssSUFBSSxZQUFZLEVBQUU7WUFDekIsSUFBSSxDQUFDLFlBQVksR0FBRyxJQUFJLENBQUMsWUFBWSxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFvQixDQUFDLENBQUM7WUFDbEUsSUFBSSxDQUFDLEtBQUssQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxTQUFTLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsRUFBRSxJQUFJLENBQUMsWUFBWSxDQUFDLEtBQUssRUFBRSxJQUFJLENBQUMsWUFBWSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1NBQy9HO0lBQ0gsQ0FBQzs7O1lBZkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxnQkFBZ0I7Z0JBQzFCLFNBQVMsRUFBRSxDQUFDLFFBQVEsQ0FBQzthQUN0Qjs7OztZQVpRLEtBQUssdUJBa0JDLFFBQVEsWUFBSSxJQUFJO1lBaEJ0QixRQUFROzs7MkJBWWQsS0FBSztvQkFFTCxLQUFLOzs7O0lBRk4sMENBQytCOztJQUMvQixtQ0FDa0I7Ozs7O0lBQ04sbUNBQXdDOzs7OztJQUFFLHNDQUEwQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgSW5wdXQsIE9wdGlvbmFsLCBTZWxmLCBTaW1wbGVDaGFuZ2VzLCBPbkNoYW5nZXMgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFRhYmxlIH0gZnJvbSAncHJpbWVuZy90YWJsZSc7XG5pbXBvcnQgY2xvbmUgZnJvbSAnanVzdC1jbG9uZSc7XG5pbXBvcnQgeyBTb3J0UGlwZSwgU29ydE9yZGVyIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuZXhwb3J0IGludGVyZmFjZSBUYWJsZVNvcnRPcHRpb25zIHtcbiAga2V5OiBzdHJpbmc7XG4gIG9yZGVyOiBTb3J0T3JkZXI7XG59XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1thYnBUYWJsZVNvcnRdJyxcbiAgcHJvdmlkZXJzOiBbU29ydFBpcGVdLFxufSlcbmV4cG9ydCBjbGFzcyBUYWJsZVNvcnREaXJlY3RpdmUgaW1wbGVtZW50cyBPbkNoYW5nZXMge1xuICBASW5wdXQoKVxuICBhYnBUYWJsZVNvcnQ6IFRhYmxlU29ydE9wdGlvbnM7XG4gIEBJbnB1dCgpXG4gIHZhbHVlOiBhbnlbXSA9IFtdO1xuICBjb25zdHJ1Y3RvcihAT3B0aW9uYWwoKSBAU2VsZigpIHByaXZhdGUgdGFibGU6IFRhYmxlLCBwcml2YXRlIHNvcnRQaXBlOiBTb3J0UGlwZSkge31cbiAgbmdPbkNoYW5nZXMoeyB2YWx1ZSwgYWJwVGFibGVTb3J0IH06IFNpbXBsZUNoYW5nZXMpIHtcbiAgICBpZiAodmFsdWUgfHwgYWJwVGFibGVTb3J0KSB7XG4gICAgICB0aGlzLmFicFRhYmxlU29ydCA9IHRoaXMuYWJwVGFibGVTb3J0IHx8ICh7fSBhcyBUYWJsZVNvcnRPcHRpb25zKTtcbiAgICAgIHRoaXMudGFibGUudmFsdWUgPSB0aGlzLnNvcnRQaXBlLnRyYW5zZm9ybShjbG9uZSh0aGlzLnZhbHVlKSwgdGhpcy5hYnBUYWJsZVNvcnQub3JkZXIsIHRoaXMuYWJwVGFibGVTb3J0LmtleSk7XG4gICAgfVxuICB9XG59XG4iXX0=