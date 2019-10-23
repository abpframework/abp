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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtc29ydC5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy90YWJsZS1zb3J0LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLElBQUksRUFBNEIsTUFBTSxlQUFlLENBQUM7QUFDM0YsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN0QyxPQUFPLEVBQUUsUUFBUSxFQUFhLE1BQU0sb0JBQW9CLENBQUM7QUFDekQsT0FBTyxLQUFLLE1BQU0sWUFBWSxDQUFDOzs7O0FBQy9CLHNDQUdDOzs7SUFGQywrQkFBWTs7SUFDWixpQ0FBaUI7O0FBRW5CO0lBU0UsNEJBQXdDLEtBQVksRUFBVSxRQUFrQjtRQUF4QyxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsYUFBUSxHQUFSLFFBQVEsQ0FBVTtRQURoRixVQUFLLEdBQVUsRUFBRSxDQUFDO0lBQ2lFLENBQUM7Ozs7O0lBQ3BGLHdDQUFXOzs7O0lBQVgsVUFBWSxFQUFzQztZQUFwQyxnQkFBSyxFQUFFLDhCQUFZO1FBQy9CLElBQUksS0FBSyxJQUFJLFlBQVksRUFBRTtZQUN6QixJQUFJLENBQUMsWUFBWSxHQUFHLElBQUksQ0FBQyxZQUFZLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQW9CLENBQUMsQ0FBQztZQUNsRSxJQUFJLENBQUMsS0FBSyxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUMsUUFBUSxDQUFDLFNBQVMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxFQUFFLElBQUksQ0FBQyxZQUFZLENBQUMsS0FBSyxFQUFFLElBQUksQ0FBQyxZQUFZLENBQUMsR0FBRyxDQUFDLENBQUM7U0FDL0c7SUFDSCxDQUFDOztnQkFmRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtvQkFDMUIsU0FBUyxFQUFFLENBQUMsUUFBUSxDQUFDO2lCQUN0Qjs7OztnQkFWUSxLQUFLLHVCQWdCQyxRQUFRLFlBQUksSUFBSTtnQkFmdEIsUUFBUTs7OytCQVdkLEtBQUs7d0JBRUwsS0FBSzs7SUFTUix5QkFBQztDQUFBLEFBaEJELElBZ0JDO1NBWlksa0JBQWtCOzs7SUFDN0IsMENBQytCOztJQUMvQixtQ0FDa0I7Ozs7O0lBQ04sbUNBQXdDOzs7OztJQUFFLHNDQUEwQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgSW5wdXQsIE9wdGlvbmFsLCBTZWxmLCBTaW1wbGVDaGFuZ2VzLCBPbkNoYW5nZXMgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgVGFibGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcclxuaW1wb3J0IHsgU29ydFBpcGUsIFNvcnRPcmRlciB9IGZyb20gJy4uL3BpcGVzL3NvcnQucGlwZSc7XHJcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcclxuZXhwb3J0IGludGVyZmFjZSBUYWJsZVNvcnRPcHRpb25zIHtcclxuICBrZXk6IHN0cmluZztcclxuICBvcmRlcjogU29ydE9yZGVyO1xyXG59XHJcbkBEaXJlY3RpdmUoe1xyXG4gIHNlbGVjdG9yOiAnW2FicFRhYmxlU29ydF0nLFxyXG4gIHByb3ZpZGVyczogW1NvcnRQaXBlXSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFRhYmxlU29ydERpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uQ2hhbmdlcyB7XHJcbiAgQElucHV0KClcclxuICBhYnBUYWJsZVNvcnQ6IFRhYmxlU29ydE9wdGlvbnM7XHJcbiAgQElucHV0KClcclxuICB2YWx1ZTogYW55W10gPSBbXTtcclxuICBjb25zdHJ1Y3RvcihAT3B0aW9uYWwoKSBAU2VsZigpIHByaXZhdGUgdGFibGU6IFRhYmxlLCBwcml2YXRlIHNvcnRQaXBlOiBTb3J0UGlwZSkge31cclxuICBuZ09uQ2hhbmdlcyh7IHZhbHVlLCBhYnBUYWJsZVNvcnQgfTogU2ltcGxlQ2hhbmdlcykge1xyXG4gICAgaWYgKHZhbHVlIHx8IGFicFRhYmxlU29ydCkge1xyXG4gICAgICB0aGlzLmFicFRhYmxlU29ydCA9IHRoaXMuYWJwVGFibGVTb3J0IHx8ICh7fSBhcyBUYWJsZVNvcnRPcHRpb25zKTtcclxuICAgICAgdGhpcy50YWJsZS52YWx1ZSA9IHRoaXMuc29ydFBpcGUudHJhbnNmb3JtKGNsb25lKHRoaXMudmFsdWUpLCB0aGlzLmFicFRhYmxlU29ydC5vcmRlciwgdGhpcy5hYnBUYWJsZVNvcnQua2V5KTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19