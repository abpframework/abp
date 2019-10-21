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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBRWhFO0lBQUE7SUE2QkEsQ0FBQzs7Ozs7OztJQXhCQyw0QkFBUzs7Ozs7O0lBQVQsVUFBVSxLQUFZLEVBQUUsU0FBcUMsRUFBRSxPQUFnQjtRQUF2RCwwQkFBQSxFQUFBLGlCQUFxQztRQUMzRCxTQUFTLEdBQUcsU0FBUyxJQUFJLENBQUMsbUJBQUEsU0FBUyxDQUFDLFdBQVcsRUFBRSxFQUFPLENBQUMsQ0FBQztRQUUxRCxJQUFJLENBQUMsS0FBSyxJQUFJLENBQUMsU0FBUyxLQUFLLEtBQUssSUFBSSxTQUFTLEtBQUssTUFBTSxDQUFDO1lBQUUsT0FBTyxLQUFLLENBQUM7O1lBRXRFLFdBQVcsR0FBRyxFQUFFOztZQUNoQixXQUFXLEdBQUcsRUFBRTtRQUVwQixJQUFJLENBQUMsT0FBTyxFQUFFO1lBQ1osV0FBVyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksS0FBSyxRQUFRLEVBQXhCLENBQXdCLEVBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztZQUNwRSxXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxVQUFBLElBQUksSUFBSSxPQUFBLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBeEIsQ0FBd0IsRUFBQyxDQUFDLElBQUksRUFBRSxDQUFDO1NBQ3JFO2FBQU07WUFDTCxXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxVQUFBLElBQUksSUFBSSxPQUFBLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQyxLQUFLLFFBQVEsRUFBakMsQ0FBaUMsRUFBQyxDQUFDLElBQUk7Ozs7O1lBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQyxJQUFLLE9BQUEsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUMsRUFBdkIsQ0FBdUIsRUFBQyxDQUFDO1lBQzlHLFdBQVcsR0FBRyxLQUFLO2lCQUNoQixNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUMsS0FBSyxRQUFRLEVBQWpDLENBQWlDLEVBQUM7aUJBQ2pELElBQUk7Ozs7O1lBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQztnQkFDVCxJQUFJLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDO29CQUFFLE9BQU8sQ0FBQyxDQUFDLENBQUM7cUJBQ2xDLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUM7b0JBQUUsT0FBTyxDQUFDLENBQUM7O29CQUN0QyxPQUFPLENBQUMsQ0FBQztZQUNoQixDQUFDLEVBQUMsQ0FBQztTQUNOOztZQUNLLE1BQU0sR0FBRyxXQUFXLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQztRQUM5QyxPQUFPLFNBQVMsS0FBSyxLQUFLLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLE9BQU8sRUFBRSxDQUFDO0lBQ3pELENBQUM7O2dCQTVCRixVQUFVO2dCQUNWLElBQUksU0FBQztvQkFDSixJQUFJLEVBQUUsU0FBUztpQkFDaEI7O0lBMEJELGVBQUM7Q0FBQSxBQTdCRCxJQTZCQztTQXpCWSxRQUFRIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgUGlwZSwgUGlwZVRyYW5zZm9ybSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuZXhwb3J0IHR5cGUgU29ydE9yZGVyID0gJ2FzYycgfCAnZGVzYyc7XG5ASW5qZWN0YWJsZSgpXG5AUGlwZSh7XG4gIG5hbWU6ICdhYnBTb3J0Jyxcbn0pXG5leHBvcnQgY2xhc3MgU29ydFBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtIHtcbiAgdHJhbnNmb3JtKHZhbHVlOiBhbnlbXSwgc29ydE9yZGVyOiBTb3J0T3JkZXIgfCBzdHJpbmcgPSAnYXNjJywgc29ydEtleT86IHN0cmluZyk6IGFueSB7XG4gICAgc29ydE9yZGVyID0gc29ydE9yZGVyICYmIChzb3J0T3JkZXIudG9Mb3dlckNhc2UoKSBhcyBhbnkpO1xuXG4gICAgaWYgKCF2YWx1ZSB8fCAoc29ydE9yZGVyICE9PSAnYXNjJyAmJiBzb3J0T3JkZXIgIT09ICdkZXNjJykpIHJldHVybiB2YWx1ZTtcblxuICAgIGxldCBudW1iZXJBcnJheSA9IFtdO1xuICAgIGxldCBzdHJpbmdBcnJheSA9IFtdO1xuXG4gICAgaWYgKCFzb3J0S2V5KSB7XG4gICAgICBudW1iZXJBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtID09PSAnbnVtYmVyJykuc29ydCgpO1xuICAgICAgc3RyaW5nQXJyYXkgPSB2YWx1ZS5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbSA9PT0gJ3N0cmluZycpLnNvcnQoKTtcbiAgICB9IGVsc2Uge1xuICAgICAgbnVtYmVyQXJyYXkgPSB2YWx1ZS5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbVtzb3J0S2V5XSA9PT0gJ251bWJlcicpLnNvcnQoKGEsIGIpID0+IGFbc29ydEtleV0gLSBiW3NvcnRLZXldKTtcbiAgICAgIHN0cmluZ0FycmF5ID0gdmFsdWVcbiAgICAgICAgLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtW3NvcnRLZXldID09PSAnc3RyaW5nJylcbiAgICAgICAgLnNvcnQoKGEsIGIpID0+IHtcbiAgICAgICAgICBpZiAoYVtzb3J0S2V5XSA8IGJbc29ydEtleV0pIHJldHVybiAtMTtcbiAgICAgICAgICBlbHNlIGlmIChhW3NvcnRLZXldID4gYltzb3J0S2V5XSkgcmV0dXJuIDE7XG4gICAgICAgICAgZWxzZSByZXR1cm4gMDtcbiAgICAgICAgfSk7XG4gICAgfVxuICAgIGNvbnN0IHNvcnRlZCA9IG51bWJlckFycmF5LmNvbmNhdChzdHJpbmdBcnJheSk7XG4gICAgcmV0dXJuIHNvcnRPcmRlciA9PT0gJ2FzYycgPyBzb3J0ZWQgOiBzb3J0ZWQucmV2ZXJzZSgpO1xuICB9XG59XG4iXX0=