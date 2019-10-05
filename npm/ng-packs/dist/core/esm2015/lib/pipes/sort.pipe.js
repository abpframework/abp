/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Pipe } from '@angular/core';
export class SortPipe {
    /**
     * @param {?} value
     * @param {?} sortOrder
     * @return {?}
     */
    transform(value, sortOrder) {
        sortOrder = sortOrder.toLowerCase();
        if (sortOrder === "desc")
            return value.reverse();
        else
            return value;
    }
}
SortPipe.decorators = [
    { type: Pipe, args: [{
                name: 'abpSort',
                pure: false
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLElBQUksRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFNcEQsTUFBTSxPQUFPLFFBQVE7Ozs7OztJQUNqQixTQUFTLENBQUMsS0FBWSxFQUFFLFNBQWlCO1FBQ3JDLFNBQVMsR0FBRyxTQUFTLENBQUMsV0FBVyxFQUFFLENBQUM7UUFDcEMsSUFBRyxTQUFTLEtBQUssTUFBTTtZQUFFLE9BQU8sS0FBSyxDQUFDLE9BQU8sRUFBRSxDQUFDOztZQUMzQyxPQUFPLEtBQUssQ0FBQztJQUN0QixDQUFDOzs7WUFUSixJQUFJLFNBQUM7Z0JBQ0YsSUFBSSxFQUFFLFNBQVM7Z0JBQ2YsSUFBSSxFQUFFLEtBQUs7YUFDZCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFBpcGUsIFBpcGVUcmFuc2Zvcm0gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQFBpcGUoe1xuICAgIG5hbWU6ICdhYnBTb3J0JyxcbiAgICBwdXJlOiBmYWxzZVxufSlcbmV4cG9ydCBjbGFzcyBTb3J0UGlwZSBpbXBsZW1lbnRzIFBpcGVUcmFuc2Zvcm0ge1xuICAgIHRyYW5zZm9ybSh2YWx1ZTogYW55W10sIHNvcnRPcmRlcjogc3RyaW5nKTogYW55IHtcbiAgICAgICAgc29ydE9yZGVyID0gc29ydE9yZGVyLnRvTG93ZXJDYXNlKCk7XG4gICAgICAgIGlmKHNvcnRPcmRlciA9PT0gXCJkZXNjXCIpIHJldHVybiB2YWx1ZS5yZXZlcnNlKCk7XG4gICAgICAgIGVsc2UgcmV0dXJuIHZhbHVlO1xuICAgIH1cbn0iXX0=