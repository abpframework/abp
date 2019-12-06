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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtc29ydC5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL3RhYmxlLXNvcnQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLElBQUksRUFBNEIsTUFBTSxlQUFlLENBQUM7QUFDM0YsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN0QyxPQUFPLEtBQUssTUFBTSxZQUFZLENBQUM7QUFDL0IsT0FBTyxFQUFFLFFBQVEsRUFBYSxNQUFNLGNBQWMsQ0FBQzs7OztBQUVuRCxzQ0FHQzs7O0lBRkMsK0JBQVk7O0lBQ1osaUNBQWlCOztBQUduQjtJQVNFLDRCQUF3QyxLQUFZLEVBQVUsUUFBa0I7UUFBeEMsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLGFBQVEsR0FBUixRQUFRLENBQVU7UUFEaEYsVUFBSyxHQUFVLEVBQUUsQ0FBQztJQUNpRSxDQUFDOzs7OztJQUNwRix3Q0FBVzs7OztJQUFYLFVBQVksRUFBc0M7WUFBcEMsZ0JBQUssRUFBRSw4QkFBWTtRQUMvQixJQUFJLEtBQUssSUFBSSxZQUFZLEVBQUU7WUFDekIsSUFBSSxDQUFDLFlBQVksR0FBRyxJQUFJLENBQUMsWUFBWSxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFvQixDQUFDLENBQUM7WUFDbEUsSUFBSSxDQUFDLEtBQUssQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxTQUFTLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsRUFBRSxJQUFJLENBQUMsWUFBWSxDQUFDLEtBQUssRUFBRSxJQUFJLENBQUMsWUFBWSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1NBQy9HO0lBQ0gsQ0FBQzs7Z0JBZkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxnQkFBZ0I7b0JBQzFCLFNBQVMsRUFBRSxDQUFDLFFBQVEsQ0FBQztpQkFDdEI7Ozs7Z0JBWlEsS0FBSyx1QkFrQkMsUUFBUSxZQUFJLElBQUk7Z0JBaEJ0QixRQUFROzs7K0JBWWQsS0FBSzt3QkFFTCxLQUFLOztJQVNSLHlCQUFDO0NBQUEsQUFoQkQsSUFnQkM7U0FaWSxrQkFBa0I7OztJQUM3QiwwQ0FDK0I7O0lBQy9CLG1DQUNrQjs7Ozs7SUFDTixtQ0FBd0M7Ozs7O0lBQUUsc0NBQTBCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBJbnB1dCwgT3B0aW9uYWwsIFNlbGYsIFNpbXBsZUNoYW5nZXMsIE9uQ2hhbmdlcyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgVGFibGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcbmltcG9ydCB7IFNvcnRQaXBlLCBTb3J0T3JkZXIgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5leHBvcnQgaW50ZXJmYWNlIFRhYmxlU29ydE9wdGlvbnMge1xuICBrZXk6IHN0cmluZztcbiAgb3JkZXI6IFNvcnRPcmRlcjtcbn1cblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicFRhYmxlU29ydF0nLFxuICBwcm92aWRlcnM6IFtTb3J0UGlwZV0sXG59KVxuZXhwb3J0IGNsYXNzIFRhYmxlU29ydERpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uQ2hhbmdlcyB7XG4gIEBJbnB1dCgpXG4gIGFicFRhYmxlU29ydDogVGFibGVTb3J0T3B0aW9ucztcbiAgQElucHV0KClcbiAgdmFsdWU6IGFueVtdID0gW107XG4gIGNvbnN0cnVjdG9yKEBPcHRpb25hbCgpIEBTZWxmKCkgcHJpdmF0ZSB0YWJsZTogVGFibGUsIHByaXZhdGUgc29ydFBpcGU6IFNvcnRQaXBlKSB7fVxuICBuZ09uQ2hhbmdlcyh7IHZhbHVlLCBhYnBUYWJsZVNvcnQgfTogU2ltcGxlQ2hhbmdlcykge1xuICAgIGlmICh2YWx1ZSB8fCBhYnBUYWJsZVNvcnQpIHtcbiAgICAgIHRoaXMuYWJwVGFibGVTb3J0ID0gdGhpcy5hYnBUYWJsZVNvcnQgfHwgKHt9IGFzIFRhYmxlU29ydE9wdGlvbnMpO1xuICAgICAgdGhpcy50YWJsZS52YWx1ZSA9IHRoaXMuc29ydFBpcGUudHJhbnNmb3JtKGNsb25lKHRoaXMudmFsdWUpLCB0aGlzLmFicFRhYmxlU29ydC5vcmRlciwgdGhpcy5hYnBUYWJsZVNvcnQua2V5KTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==