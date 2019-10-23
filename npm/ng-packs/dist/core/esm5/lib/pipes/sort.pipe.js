/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable, Pipe } from '@angular/core';
var SortPipe = /** @class */ (function () {
    function SortPipe() {
    }
    /**
     * @param {?} value
     * @param {?=} sortOrder
     * @param {?=} sortKey
     * @return {?}
     */
    SortPipe.prototype.transform = /**
     * @param {?} value
     * @param {?=} sortOrder
     * @param {?=} sortKey
     * @return {?}
     */
    function (value, sortOrder, sortKey) {
        if (sortOrder === void 0) { sortOrder = 'asc'; }
        sortOrder = sortOrder && ((/** @type {?} */ (sortOrder.toLowerCase())));
        if (!value || (sortOrder !== 'asc' && sortOrder !== 'desc'))
            return value;
        /** @type {?} */
        var numberArray = [];
        /** @type {?} */
        var stringArray = [];
        if (!sortKey) {
            numberArray = value.filter((/**
             * @param {?} item
             * @return {?}
             */
            function (item) { return typeof item === 'number'; })).sort();
            stringArray = value.filter((/**
             * @param {?} item
             * @return {?}
             */
            function (item) { return typeof item === 'string'; })).sort();
        }
        else {
            numberArray = value.filter((/**
             * @param {?} item
             * @return {?}
             */
            function (item) { return typeof item[sortKey] === 'number'; })).sort((/**
             * @param {?} a
             * @param {?} b
             * @return {?}
             */
            function (a, b) { return a[sortKey] - b[sortKey]; }));
            stringArray = value
                .filter((/**
             * @param {?} item
             * @return {?}
             */
            function (item) { return typeof item[sortKey] === 'string'; }))
                .sort((/**
             * @param {?} a
             * @param {?} b
             * @return {?}
             */
            function (a, b) {
                if (a[sortKey] < b[sortKey])
                    return -1;
                else if (a[sortKey] > b[sortKey])
                    return 1;
                else
                    return 0;
            }));
        }
        /** @type {?} */
        var sorted = numberArray.concat(stringArray);
        return sortOrder === 'asc' ? sorted : sorted.reverse();
    };
    SortPipe.decorators = [
        { type: Injectable },
        { type: Pipe, args: [{
                    name: 'abpSort',
                },] }
    ];
    return SortPipe;
}());
export { SortPipe };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBRWhFO0lBQUE7SUE2QkEsQ0FBQzs7Ozs7OztJQXhCQyw0QkFBUzs7Ozs7O0lBQVQsVUFBVSxLQUFZLEVBQUUsU0FBcUMsRUFBRSxPQUFnQjtRQUF2RCwwQkFBQSxFQUFBLGlCQUFxQztRQUMzRCxTQUFTLEdBQUcsU0FBUyxJQUFJLENBQUMsbUJBQUEsU0FBUyxDQUFDLFdBQVcsRUFBRSxFQUFPLENBQUMsQ0FBQztRQUUxRCxJQUFJLENBQUMsS0FBSyxJQUFJLENBQUMsU0FBUyxLQUFLLEtBQUssSUFBSSxTQUFTLEtBQUssTUFBTSxDQUFDO1lBQUUsT0FBTyxLQUFLLENBQUM7O1lBRXRFLFdBQVcsR0FBRyxFQUFFOztZQUNoQixXQUFXLEdBQUcsRUFBRTtRQUVwQixJQUFJLENBQUMsT0FBTyxFQUFFO1lBQ1osV0FBVyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksS0FBSyxRQUFRLEVBQXhCLENBQXdCLEVBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztZQUNwRSxXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxVQUFBLElBQUksSUFBSSxPQUFBLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBeEIsQ0FBd0IsRUFBQyxDQUFDLElBQUksRUFBRSxDQUFDO1NBQ3JFO2FBQU07WUFDTCxXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxVQUFBLElBQUksSUFBSSxPQUFBLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQyxLQUFLLFFBQVEsRUFBakMsQ0FBaUMsRUFBQyxDQUFDLElBQUk7Ozs7O1lBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQyxJQUFLLE9BQUEsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUMsRUFBdkIsQ0FBdUIsRUFBQyxDQUFDO1lBQzlHLFdBQVcsR0FBRyxLQUFLO2lCQUNoQixNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUMsS0FBSyxRQUFRLEVBQWpDLENBQWlDLEVBQUM7aUJBQ2pELElBQUk7Ozs7O1lBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQztnQkFDVCxJQUFJLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDO29CQUFFLE9BQU8sQ0FBQyxDQUFDLENBQUM7cUJBQ2xDLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUM7b0JBQUUsT0FBTyxDQUFDLENBQUM7O29CQUN0QyxPQUFPLENBQUMsQ0FBQztZQUNoQixDQUFDLEVBQUMsQ0FBQztTQUNOOztZQUNLLE1BQU0sR0FBRyxXQUFXLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQztRQUM5QyxPQUFPLFNBQVMsS0FBSyxLQUFLLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLE9BQU8sRUFBRSxDQUFDO0lBQ3pELENBQUM7O2dCQTVCRixVQUFVO2dCQUNWLElBQUksU0FBQztvQkFDSixJQUFJLEVBQUUsU0FBUztpQkFDaEI7O0lBMEJELGVBQUM7Q0FBQSxBQTdCRCxJQTZCQztTQXpCWSxRQUFRIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgUGlwZSwgUGlwZVRyYW5zZm9ybSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5leHBvcnQgdHlwZSBTb3J0T3JkZXIgPSAnYXNjJyB8ICdkZXNjJztcclxuQEluamVjdGFibGUoKVxyXG5AUGlwZSh7XHJcbiAgbmFtZTogJ2FicFNvcnQnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgU29ydFBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtIHtcclxuICB0cmFuc2Zvcm0odmFsdWU6IGFueVtdLCBzb3J0T3JkZXI6IFNvcnRPcmRlciB8IHN0cmluZyA9ICdhc2MnLCBzb3J0S2V5Pzogc3RyaW5nKTogYW55IHtcclxuICAgIHNvcnRPcmRlciA9IHNvcnRPcmRlciAmJiAoc29ydE9yZGVyLnRvTG93ZXJDYXNlKCkgYXMgYW55KTtcclxuXHJcbiAgICBpZiAoIXZhbHVlIHx8IChzb3J0T3JkZXIgIT09ICdhc2MnICYmIHNvcnRPcmRlciAhPT0gJ2Rlc2MnKSkgcmV0dXJuIHZhbHVlO1xyXG5cclxuICAgIGxldCBudW1iZXJBcnJheSA9IFtdO1xyXG4gICAgbGV0IHN0cmluZ0FycmF5ID0gW107XHJcblxyXG4gICAgaWYgKCFzb3J0S2V5KSB7XHJcbiAgICAgIG51bWJlckFycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW0gPT09ICdudW1iZXInKS5zb3J0KCk7XHJcbiAgICAgIHN0cmluZ0FycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW0gPT09ICdzdHJpbmcnKS5zb3J0KCk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBudW1iZXJBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtW3NvcnRLZXldID09PSAnbnVtYmVyJykuc29ydCgoYSwgYikgPT4gYVtzb3J0S2V5XSAtIGJbc29ydEtleV0pO1xyXG4gICAgICBzdHJpbmdBcnJheSA9IHZhbHVlXHJcbiAgICAgICAgLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtW3NvcnRLZXldID09PSAnc3RyaW5nJylcclxuICAgICAgICAuc29ydCgoYSwgYikgPT4ge1xyXG4gICAgICAgICAgaWYgKGFbc29ydEtleV0gPCBiW3NvcnRLZXldKSByZXR1cm4gLTE7XHJcbiAgICAgICAgICBlbHNlIGlmIChhW3NvcnRLZXldID4gYltzb3J0S2V5XSkgcmV0dXJuIDE7XHJcbiAgICAgICAgICBlbHNlIHJldHVybiAwO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG4gICAgY29uc3Qgc29ydGVkID0gbnVtYmVyQXJyYXkuY29uY2F0KHN0cmluZ0FycmF5KTtcclxuICAgIHJldHVybiBzb3J0T3JkZXIgPT09ICdhc2MnID8gc29ydGVkIDogc29ydGVkLnJldmVyc2UoKTtcclxuICB9XHJcbn1cclxuIl19