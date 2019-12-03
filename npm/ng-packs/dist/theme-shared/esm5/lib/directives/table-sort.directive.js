/**
 * @fileoverview added by tsickle
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
var TableSortDirective = /** @class */ (function () {
    function TableSortDirective(table, sortPipe) {
        this.table = table;
        this.sortPipe = sortPipe;
        this.value = [];
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    TableSortDirective.prototype.ngOnChanges = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var value = _a.value, abpTableSort = _a.abpTableSort;
        if (value || abpTableSort) {
            this.abpTableSort = this.abpTableSort || ((/** @type {?} */ ({})));
            this.table.value = this.sortPipe.transform(clone(this.value), this.abpTableSort.order, this.abpTableSort.key);
        }
    };
    TableSortDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[abpTableSort]',
                    providers: [SortPipe],
                },] }
    ];
    /** @nocollapse */
    TableSortDirective.ctorParameters = function () { return [
        { type: Table, decorators: [{ type: Optional }, { type: Self }] },
        { type: SortPipe }
    ]; };
    TableSortDirective.propDecorators = {
        abpTableSort: [{ type: Input }],
        value: [{ type: Input }]
    };
    return TableSortDirective;
}());
export { TableSortDirective };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtc29ydC5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL3RhYmxlLXNvcnQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBRSxRQUFRLEVBQUUsSUFBSSxFQUE0QixNQUFNLGVBQWUsQ0FBQztBQUMzRixPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3RDLE9BQU8sS0FBSyxNQUFNLFlBQVksQ0FBQztBQUMvQixPQUFPLEVBQUUsUUFBUSxFQUFhLE1BQU0sY0FBYyxDQUFDOzs7O0FBRW5ELHNDQUdDOzs7SUFGQywrQkFBWTs7SUFDWixpQ0FBaUI7O0FBR25CO0lBU0UsNEJBQXdDLEtBQVksRUFBVSxRQUFrQjtRQUF4QyxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsYUFBUSxHQUFSLFFBQVEsQ0FBVTtRQURoRixVQUFLLEdBQVUsRUFBRSxDQUFDO0lBQ2lFLENBQUM7Ozs7O0lBQ3BGLHdDQUFXOzs7O0lBQVgsVUFBWSxFQUFzQztZQUFwQyxnQkFBSyxFQUFFLDhCQUFZO1FBQy9CLElBQUksS0FBSyxJQUFJLFlBQVksRUFBRTtZQUN6QixJQUFJLENBQUMsWUFBWSxHQUFHLElBQUksQ0FBQyxZQUFZLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQW9CLENBQUMsQ0FBQztZQUNsRSxJQUFJLENBQUMsS0FBSyxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUMsUUFBUSxDQUFDLFNBQVMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxFQUFFLElBQUksQ0FBQyxZQUFZLENBQUMsS0FBSyxFQUFFLElBQUksQ0FBQyxZQUFZLENBQUMsR0FBRyxDQUFDLENBQUM7U0FDL0c7SUFDSCxDQUFDOztnQkFmRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtvQkFDMUIsU0FBUyxFQUFFLENBQUMsUUFBUSxDQUFDO2lCQUN0Qjs7OztnQkFaUSxLQUFLLHVCQWtCQyxRQUFRLFlBQUksSUFBSTtnQkFoQnRCLFFBQVE7OzsrQkFZZCxLQUFLO3dCQUVMLEtBQUs7O0lBU1IseUJBQUM7Q0FBQSxBQWhCRCxJQWdCQztTQVpZLGtCQUFrQjs7O0lBQzdCLDBDQUMrQjs7SUFDL0IsbUNBQ2tCOzs7OztJQUNOLG1DQUF3Qzs7Ozs7SUFBRSxzQ0FBMEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIElucHV0LCBPcHRpb25hbCwgU2VsZiwgU2ltcGxlQ2hhbmdlcywgT25DaGFuZ2VzIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBUYWJsZSB9IGZyb20gJ3ByaW1lbmcvdGFibGUnO1xuaW1wb3J0IGNsb25lIGZyb20gJ2p1c3QtY2xvbmUnO1xuaW1wb3J0IHsgU29ydFBpcGUsIFNvcnRPcmRlciB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbmV4cG9ydCBpbnRlcmZhY2UgVGFibGVTb3J0T3B0aW9ucyB7XG4gIGtleTogc3RyaW5nO1xuICBvcmRlcjogU29ydE9yZGVyO1xufVxuXG5ARGlyZWN0aXZlKHtcbiAgc2VsZWN0b3I6ICdbYWJwVGFibGVTb3J0XScsXG4gIHByb3ZpZGVyczogW1NvcnRQaXBlXSxcbn0pXG5leHBvcnQgY2xhc3MgVGFibGVTb3J0RGlyZWN0aXZlIGltcGxlbWVudHMgT25DaGFuZ2VzIHtcbiAgQElucHV0KClcbiAgYWJwVGFibGVTb3J0OiBUYWJsZVNvcnRPcHRpb25zO1xuICBASW5wdXQoKVxuICB2YWx1ZTogYW55W10gPSBbXTtcbiAgY29uc3RydWN0b3IoQE9wdGlvbmFsKCkgQFNlbGYoKSBwcml2YXRlIHRhYmxlOiBUYWJsZSwgcHJpdmF0ZSBzb3J0UGlwZTogU29ydFBpcGUpIHt9XG4gIG5nT25DaGFuZ2VzKHsgdmFsdWUsIGFicFRhYmxlU29ydCB9OiBTaW1wbGVDaGFuZ2VzKSB7XG4gICAgaWYgKHZhbHVlIHx8IGFicFRhYmxlU29ydCkge1xuICAgICAgdGhpcy5hYnBUYWJsZVNvcnQgPSB0aGlzLmFicFRhYmxlU29ydCB8fCAoe30gYXMgVGFibGVTb3J0T3B0aW9ucyk7XG4gICAgICB0aGlzLnRhYmxlLnZhbHVlID0gdGhpcy5zb3J0UGlwZS50cmFuc2Zvcm0oY2xvbmUodGhpcy52YWx1ZSksIHRoaXMuYWJwVGFibGVTb3J0Lm9yZGVyLCB0aGlzLmFicFRhYmxlU29ydC5rZXkpO1xuICAgIH1cbiAgfVxufVxuIl19