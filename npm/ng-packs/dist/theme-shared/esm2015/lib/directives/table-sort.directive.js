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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtc29ydC5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL3RhYmxlLXNvcnQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBRSxRQUFRLEVBQUUsSUFBSSxFQUE0QixNQUFNLGVBQWUsQ0FBQztBQUMzRixPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3RDLE9BQU8sS0FBSyxNQUFNLFlBQVksQ0FBQztBQUMvQixPQUFPLEVBQUUsUUFBUSxFQUFhLE1BQU0sY0FBYyxDQUFDOzs7O0FBRW5ELHNDQUdDOzs7SUFGQywrQkFBWTs7SUFDWixpQ0FBaUI7O0FBT25CLE1BQU0sT0FBTyxrQkFBa0I7Ozs7O0lBSzdCLFlBQXdDLEtBQVksRUFBVSxRQUFrQjtRQUF4QyxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsYUFBUSxHQUFSLFFBQVEsQ0FBVTtRQURoRixVQUFLLEdBQVUsRUFBRSxDQUFDO0lBQ2lFLENBQUM7Ozs7O0lBQ3BGLFdBQVcsQ0FBQyxFQUFFLEtBQUssRUFBRSxZQUFZLEVBQWlCO1FBQ2hELElBQUksS0FBSyxJQUFJLFlBQVksRUFBRTtZQUN6QixJQUFJLENBQUMsWUFBWSxHQUFHLElBQUksQ0FBQyxZQUFZLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQW9CLENBQUMsQ0FBQztZQUNsRSxJQUFJLENBQUMsS0FBSyxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUMsUUFBUSxDQUFDLFNBQVMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxFQUFFLElBQUksQ0FBQyxZQUFZLENBQUMsS0FBSyxFQUFFLElBQUksQ0FBQyxZQUFZLENBQUMsR0FBRyxDQUFDLENBQUM7U0FDL0c7SUFDSCxDQUFDOzs7WUFmRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtnQkFDMUIsU0FBUyxFQUFFLENBQUMsUUFBUSxDQUFDO2FBQ3RCOzs7O1lBWlEsS0FBSyx1QkFrQkMsUUFBUSxZQUFJLElBQUk7WUFoQnRCLFFBQVE7OzsyQkFZZCxLQUFLO29CQUVMLEtBQUs7Ozs7SUFGTiwwQ0FDK0I7O0lBQy9CLG1DQUNrQjs7Ozs7SUFDTixtQ0FBd0M7Ozs7O0lBQUUsc0NBQTBCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBJbnB1dCwgT3B0aW9uYWwsIFNlbGYsIFNpbXBsZUNoYW5nZXMsIE9uQ2hhbmdlcyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgVGFibGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcbmltcG9ydCB7IFNvcnRQaXBlLCBTb3J0T3JkZXIgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5leHBvcnQgaW50ZXJmYWNlIFRhYmxlU29ydE9wdGlvbnMge1xuICBrZXk6IHN0cmluZztcbiAgb3JkZXI6IFNvcnRPcmRlcjtcbn1cblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicFRhYmxlU29ydF0nLFxuICBwcm92aWRlcnM6IFtTb3J0UGlwZV0sXG59KVxuZXhwb3J0IGNsYXNzIFRhYmxlU29ydERpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uQ2hhbmdlcyB7XG4gIEBJbnB1dCgpXG4gIGFicFRhYmxlU29ydDogVGFibGVTb3J0T3B0aW9ucztcbiAgQElucHV0KClcbiAgdmFsdWU6IGFueVtdID0gW107XG4gIGNvbnN0cnVjdG9yKEBPcHRpb25hbCgpIEBTZWxmKCkgcHJpdmF0ZSB0YWJsZTogVGFibGUsIHByaXZhdGUgc29ydFBpcGU6IFNvcnRQaXBlKSB7fVxuICBuZ09uQ2hhbmdlcyh7IHZhbHVlLCBhYnBUYWJsZVNvcnQgfTogU2ltcGxlQ2hhbmdlcykge1xuICAgIGlmICh2YWx1ZSB8fCBhYnBUYWJsZVNvcnQpIHtcbiAgICAgIHRoaXMuYWJwVGFibGVTb3J0ID0gdGhpcy5hYnBUYWJsZVNvcnQgfHwgKHt9IGFzIFRhYmxlU29ydE9wdGlvbnMpO1xuICAgICAgdGhpcy50YWJsZS52YWx1ZSA9IHRoaXMuc29ydFBpcGUudHJhbnNmb3JtKGNsb25lKHRoaXMudmFsdWUpLCB0aGlzLmFicFRhYmxlU29ydC5vcmRlciwgdGhpcy5hYnBUYWJsZVNvcnQua2V5KTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==