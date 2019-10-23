/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, Optional, Self } from '@angular/core';
import { Table } from 'primeng/table';
import { SortPipe } from '../pipes/sort.pipe';
import clone from 'just-clone';
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtc29ydC5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy90YWJsZS1zb3J0LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLElBQUksRUFBNEIsTUFBTSxlQUFlLENBQUM7QUFDM0YsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN0QyxPQUFPLEVBQUUsUUFBUSxFQUFhLE1BQU0sb0JBQW9CLENBQUM7QUFDekQsT0FBTyxLQUFLLE1BQU0sWUFBWSxDQUFDOzs7O0FBQy9CLHNDQUdDOzs7SUFGQywrQkFBWTs7SUFDWixpQ0FBaUI7O0FBTW5CLE1BQU0sT0FBTyxrQkFBa0I7Ozs7O0lBSzdCLFlBQXdDLEtBQVksRUFBVSxRQUFrQjtRQUF4QyxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsYUFBUSxHQUFSLFFBQVEsQ0FBVTtRQURoRixVQUFLLEdBQVUsRUFBRSxDQUFDO0lBQ2lFLENBQUM7Ozs7O0lBQ3BGLFdBQVcsQ0FBQyxFQUFFLEtBQUssRUFBRSxZQUFZLEVBQWlCO1FBQ2hELElBQUksS0FBSyxJQUFJLFlBQVksRUFBRTtZQUN6QixJQUFJLENBQUMsWUFBWSxHQUFHLElBQUksQ0FBQyxZQUFZLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQW9CLENBQUMsQ0FBQztZQUNsRSxJQUFJLENBQUMsS0FBSyxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUMsUUFBUSxDQUFDLFNBQVMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxFQUFFLElBQUksQ0FBQyxZQUFZLENBQUMsS0FBSyxFQUFFLElBQUksQ0FBQyxZQUFZLENBQUMsR0FBRyxDQUFDLENBQUM7U0FDL0c7SUFDSCxDQUFDOzs7WUFmRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtnQkFDMUIsU0FBUyxFQUFFLENBQUMsUUFBUSxDQUFDO2FBQ3RCOzs7O1lBVlEsS0FBSyx1QkFnQkMsUUFBUSxZQUFJLElBQUk7WUFmdEIsUUFBUTs7OzJCQVdkLEtBQUs7b0JBRUwsS0FBSzs7OztJQUZOLDBDQUMrQjs7SUFDL0IsbUNBQ2tCOzs7OztJQUNOLG1DQUF3Qzs7Ozs7SUFBRSxzQ0FBMEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIElucHV0LCBPcHRpb25hbCwgU2VsZiwgU2ltcGxlQ2hhbmdlcywgT25DaGFuZ2VzIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFRhYmxlIH0gZnJvbSAncHJpbWVuZy90YWJsZSc7XHJcbmltcG9ydCB7IFNvcnRQaXBlLCBTb3J0T3JkZXIgfSBmcm9tICcuLi9waXBlcy9zb3J0LnBpcGUnO1xyXG5pbXBvcnQgY2xvbmUgZnJvbSAnanVzdC1jbG9uZSc7XHJcbmV4cG9ydCBpbnRlcmZhY2UgVGFibGVTb3J0T3B0aW9ucyB7XHJcbiAga2V5OiBzdHJpbmc7XHJcbiAgb3JkZXI6IFNvcnRPcmRlcjtcclxufVxyXG5ARGlyZWN0aXZlKHtcclxuICBzZWxlY3RvcjogJ1thYnBUYWJsZVNvcnRdJyxcclxuICBwcm92aWRlcnM6IFtTb3J0UGlwZV0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUYWJsZVNvcnREaXJlY3RpdmUgaW1wbGVtZW50cyBPbkNoYW5nZXMge1xyXG4gIEBJbnB1dCgpXHJcbiAgYWJwVGFibGVTb3J0OiBUYWJsZVNvcnRPcHRpb25zO1xyXG4gIEBJbnB1dCgpXHJcbiAgdmFsdWU6IGFueVtdID0gW107XHJcbiAgY29uc3RydWN0b3IoQE9wdGlvbmFsKCkgQFNlbGYoKSBwcml2YXRlIHRhYmxlOiBUYWJsZSwgcHJpdmF0ZSBzb3J0UGlwZTogU29ydFBpcGUpIHt9XHJcbiAgbmdPbkNoYW5nZXMoeyB2YWx1ZSwgYWJwVGFibGVTb3J0IH06IFNpbXBsZUNoYW5nZXMpIHtcclxuICAgIGlmICh2YWx1ZSB8fCBhYnBUYWJsZVNvcnQpIHtcclxuICAgICAgdGhpcy5hYnBUYWJsZVNvcnQgPSB0aGlzLmFicFRhYmxlU29ydCB8fCAoe30gYXMgVGFibGVTb3J0T3B0aW9ucyk7XHJcbiAgICAgIHRoaXMudGFibGUudmFsdWUgPSB0aGlzLnNvcnRQaXBlLnRyYW5zZm9ybShjbG9uZSh0aGlzLnZhbHVlKSwgdGhpcy5hYnBUYWJsZVNvcnQub3JkZXIsIHRoaXMuYWJwVGFibGVTb3J0LmtleSk7XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==