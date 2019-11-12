/**
 * @fileoverview added by tsickle
 * Generated from: lib/pipes/sort.pipe.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable, Pipe } from '@angular/core';
export class SortPipe {
    /**
     * @param {?} value
     * @param {?=} sortOrder
     * @param {?=} sortKey
     * @return {?}
     */
    transform(value, sortOrder = 'asc', sortKey) {
        sortOrder = sortOrder && ((/** @type {?} */ (sortOrder.toLowerCase())));
        if (!value || (sortOrder !== 'asc' && sortOrder !== 'desc'))
            return value;
        /** @type {?} */
        let numberArray = [];
        /** @type {?} */
        let stringArray = [];
        if (!sortKey) {
            numberArray = value.filter((/**
             * @param {?} item
             * @return {?}
             */
            item => typeof item === 'number')).sort();
            stringArray = value.filter((/**
             * @param {?} item
             * @return {?}
             */
            item => typeof item === 'string')).sort();
        }
        else {
            numberArray = value.filter((/**
             * @param {?} item
             * @return {?}
             */
            item => typeof item[sortKey] === 'number')).sort((/**
             * @param {?} a
             * @param {?} b
             * @return {?}
             */
            (a, b) => a[sortKey] - b[sortKey]));
            stringArray = value
                .filter((/**
             * @param {?} item
             * @return {?}
             */
            item => typeof item[sortKey] === 'string'))
                .sort((/**
             * @param {?} a
             * @param {?} b
             * @return {?}
             */
            (a, b) => {
                if (a[sortKey] < b[sortKey])
                    return -1;
                else if (a[sortKey] > b[sortKey])
                    return 1;
                else
                    return 0;
            }));
        }
        /** @type {?} */
        const sorted = numberArray.concat(stringArray);
        return sortOrder === 'asc' ? sorted : sorted.reverse();
    }
}
SortPipe.decorators = [
    { type: Injectable },
    { type: Pipe, args: [{
                name: 'abpSort',
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQU1oRSxNQUFNLE9BQU8sUUFBUTs7Ozs7OztJQUNuQixTQUFTLENBQUMsS0FBWSxFQUFFLFlBQWdDLEtBQUssRUFBRSxPQUFnQjtRQUM3RSxTQUFTLEdBQUcsU0FBUyxJQUFJLENBQUMsbUJBQUEsU0FBUyxDQUFDLFdBQVcsRUFBRSxFQUFPLENBQUMsQ0FBQztRQUUxRCxJQUFJLENBQUMsS0FBSyxJQUFJLENBQUMsU0FBUyxLQUFLLEtBQUssSUFBSSxTQUFTLEtBQUssTUFBTSxDQUFDO1lBQUUsT0FBTyxLQUFLLENBQUM7O1lBRXRFLFdBQVcsR0FBRyxFQUFFOztZQUNoQixXQUFXLEdBQUcsRUFBRTtRQUVwQixJQUFJLENBQUMsT0FBTyxFQUFFO1lBQ1osV0FBVyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxPQUFPLElBQUksS0FBSyxRQUFRLEVBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztZQUNwRSxXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBQyxDQUFDLElBQUksRUFBRSxDQUFDO1NBQ3JFO2FBQU07WUFDTCxXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQyxLQUFLLFFBQVEsRUFBQyxDQUFDLElBQUk7Ozs7O1lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxFQUFDLENBQUM7WUFDOUcsV0FBVyxHQUFHLEtBQUs7aUJBQ2hCLE1BQU07Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQyxLQUFLLFFBQVEsRUFBQztpQkFDakQsSUFBSTs7Ozs7WUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRTtnQkFDYixJQUFJLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDO29CQUFFLE9BQU8sQ0FBQyxDQUFDLENBQUM7cUJBQ2xDLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUM7b0JBQUUsT0FBTyxDQUFDLENBQUM7O29CQUN0QyxPQUFPLENBQUMsQ0FBQztZQUNoQixDQUFDLEVBQUMsQ0FBQztTQUNOOztjQUNLLE1BQU0sR0FBRyxXQUFXLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQztRQUM5QyxPQUFPLFNBQVMsS0FBSyxLQUFLLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLE9BQU8sRUFBRSxDQUFDO0lBQ3pELENBQUM7OztZQTVCRixVQUFVO1lBQ1YsSUFBSSxTQUFDO2dCQUNKLElBQUksRUFBRSxTQUFTO2FBQ2hCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgUGlwZSwgUGlwZVRyYW5zZm9ybSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5leHBvcnQgdHlwZSBTb3J0T3JkZXIgPSAnYXNjJyB8ICdkZXNjJztcclxuQEluamVjdGFibGUoKVxyXG5AUGlwZSh7XHJcbiAgbmFtZTogJ2FicFNvcnQnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgU29ydFBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtIHtcclxuICB0cmFuc2Zvcm0odmFsdWU6IGFueVtdLCBzb3J0T3JkZXI6IFNvcnRPcmRlciB8IHN0cmluZyA9ICdhc2MnLCBzb3J0S2V5Pzogc3RyaW5nKTogYW55IHtcclxuICAgIHNvcnRPcmRlciA9IHNvcnRPcmRlciAmJiAoc29ydE9yZGVyLnRvTG93ZXJDYXNlKCkgYXMgYW55KTtcclxuXHJcbiAgICBpZiAoIXZhbHVlIHx8IChzb3J0T3JkZXIgIT09ICdhc2MnICYmIHNvcnRPcmRlciAhPT0gJ2Rlc2MnKSkgcmV0dXJuIHZhbHVlO1xyXG5cclxuICAgIGxldCBudW1iZXJBcnJheSA9IFtdO1xyXG4gICAgbGV0IHN0cmluZ0FycmF5ID0gW107XHJcblxyXG4gICAgaWYgKCFzb3J0S2V5KSB7XHJcbiAgICAgIG51bWJlckFycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW0gPT09ICdudW1iZXInKS5zb3J0KCk7XHJcbiAgICAgIHN0cmluZ0FycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW0gPT09ICdzdHJpbmcnKS5zb3J0KCk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBudW1iZXJBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtW3NvcnRLZXldID09PSAnbnVtYmVyJykuc29ydCgoYSwgYikgPT4gYVtzb3J0S2V5XSAtIGJbc29ydEtleV0pO1xyXG4gICAgICBzdHJpbmdBcnJheSA9IHZhbHVlXHJcbiAgICAgICAgLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtW3NvcnRLZXldID09PSAnc3RyaW5nJylcclxuICAgICAgICAuc29ydCgoYSwgYikgPT4ge1xyXG4gICAgICAgICAgaWYgKGFbc29ydEtleV0gPCBiW3NvcnRLZXldKSByZXR1cm4gLTE7XHJcbiAgICAgICAgICBlbHNlIGlmIChhW3NvcnRLZXldID4gYltzb3J0S2V5XSkgcmV0dXJuIDE7XHJcbiAgICAgICAgICBlbHNlIHJldHVybiAwO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG4gICAgY29uc3Qgc29ydGVkID0gbnVtYmVyQXJyYXkuY29uY2F0KHN0cmluZ0FycmF5KTtcclxuICAgIHJldHVybiBzb3J0T3JkZXIgPT09ICdhc2MnID8gc29ydGVkIDogc29ydGVkLnJldmVyc2UoKTtcclxuICB9XHJcbn1cclxuIl19