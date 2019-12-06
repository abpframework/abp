/**
 * @fileoverview added by tsickle
 * Generated from: lib/pipes/sort.pipe.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
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
            numberArray = value
                .filter((/**
             * @param {?} item
             * @return {?}
             */
            function (item) { return typeof item[sortKey] === 'number'; }))
                .sort((/**
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
        var sorted = tslib_1.__spread(numberArray, stringArray, value.filter((/**
         * @param {?} item
         * @return {?}
         */
        function (item) {
            return typeof (sortKey ? item[sortKey] : item) !== 'number' &&
                typeof (sortKey ? item[sortKey] : item) !== 'string';
        })));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFFaEU7SUFBQTtJQTJDQSxDQUFDOzs7Ozs7O0lBdENDLDRCQUFTOzs7Ozs7SUFBVCxVQUNFLEtBQVksRUFDWixTQUFxQyxFQUNyQyxPQUFnQjtRQURoQiwwQkFBQSxFQUFBLGlCQUFxQztRQUdyQyxTQUFTLEdBQUcsU0FBUyxJQUFJLENBQUMsbUJBQUEsU0FBUyxDQUFDLFdBQVcsRUFBRSxFQUFPLENBQUMsQ0FBQztRQUUxRCxJQUFJLENBQUMsS0FBSyxJQUFJLENBQUMsU0FBUyxLQUFLLEtBQUssSUFBSSxTQUFTLEtBQUssTUFBTSxDQUFDO1lBQUUsT0FBTyxLQUFLLENBQUM7O1lBRXRFLFdBQVcsR0FBRyxFQUFFOztZQUNoQixXQUFXLEdBQUcsRUFBRTtRQUVwQixJQUFJLENBQUMsT0FBTyxFQUFFO1lBQ1osV0FBVyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksS0FBSyxRQUFRLEVBQXhCLENBQXdCLEVBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztZQUNwRSxXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxVQUFBLElBQUksSUFBSSxPQUFBLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBeEIsQ0FBd0IsRUFBQyxDQUFDLElBQUksRUFBRSxDQUFDO1NBQ3JFO2FBQU07WUFDTCxXQUFXLEdBQUcsS0FBSztpQkFDaEIsTUFBTTs7OztZQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFqQyxDQUFpQyxFQUFDO2lCQUNqRCxJQUFJOzs7OztZQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDLEVBQXZCLENBQXVCLEVBQUMsQ0FBQztZQUMzQyxXQUFXLEdBQUcsS0FBSztpQkFDaEIsTUFBTTs7OztZQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFqQyxDQUFpQyxFQUFDO2lCQUNqRCxJQUFJOzs7OztZQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUM7Z0JBQ1QsSUFBSSxDQUFDLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE9BQU8sQ0FBQztvQkFBRSxPQUFPLENBQUMsQ0FBQyxDQUFDO3FCQUNsQyxJQUFJLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDO29CQUFFLE9BQU8sQ0FBQyxDQUFDOztvQkFDdEMsT0FBTyxDQUFDLENBQUM7WUFDaEIsQ0FBQyxFQUFDLENBQUM7U0FDTjs7WUFDSyxNQUFNLG9CQUNQLFdBQVcsRUFDWCxXQUFXLEVBQ1gsS0FBSyxDQUFDLE1BQU07Ozs7UUFDYixVQUFBLElBQUk7WUFDRixPQUFBLE9BQU8sQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLEtBQUssUUFBUTtnQkFDcEQsT0FBTyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxRQUFRO1FBRHBELENBQ29ELEVBQ3ZELENBQ0Y7UUFDRCxPQUFPLFNBQVMsS0FBSyxLQUFLLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLE9BQU8sRUFBRSxDQUFDO0lBQ3pELENBQUM7O2dCQTFDRixVQUFVO2dCQUNWLElBQUksU0FBQztvQkFDSixJQUFJLEVBQUUsU0FBUztpQkFDaEI7O0lBd0NELGVBQUM7Q0FBQSxBQTNDRCxJQTJDQztTQXZDWSxRQUFRIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgUGlwZSwgUGlwZVRyYW5zZm9ybSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5leHBvcnQgdHlwZSBTb3J0T3JkZXIgPSAnYXNjJyB8ICdkZXNjJztcclxuQEluamVjdGFibGUoKVxyXG5AUGlwZSh7XHJcbiAgbmFtZTogJ2FicFNvcnQnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgU29ydFBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtIHtcclxuICB0cmFuc2Zvcm0oXHJcbiAgICB2YWx1ZTogYW55W10sXHJcbiAgICBzb3J0T3JkZXI6IFNvcnRPcmRlciB8IHN0cmluZyA9ICdhc2MnLFxyXG4gICAgc29ydEtleT86IHN0cmluZyxcclxuICApOiBhbnkge1xyXG4gICAgc29ydE9yZGVyID0gc29ydE9yZGVyICYmIChzb3J0T3JkZXIudG9Mb3dlckNhc2UoKSBhcyBhbnkpO1xyXG5cclxuICAgIGlmICghdmFsdWUgfHwgKHNvcnRPcmRlciAhPT0gJ2FzYycgJiYgc29ydE9yZGVyICE9PSAnZGVzYycpKSByZXR1cm4gdmFsdWU7XHJcblxyXG4gICAgbGV0IG51bWJlckFycmF5ID0gW107XHJcbiAgICBsZXQgc3RyaW5nQXJyYXkgPSBbXTtcclxuXHJcbiAgICBpZiAoIXNvcnRLZXkpIHtcclxuICAgICAgbnVtYmVyQXJyYXkgPSB2YWx1ZS5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbSA9PT0gJ251bWJlcicpLnNvcnQoKTtcclxuICAgICAgc3RyaW5nQXJyYXkgPSB2YWx1ZS5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbSA9PT0gJ3N0cmluZycpLnNvcnQoKTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgIG51bWJlckFycmF5ID0gdmFsdWVcclxuICAgICAgICAuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW1bc29ydEtleV0gPT09ICdudW1iZXInKVxyXG4gICAgICAgIC5zb3J0KChhLCBiKSA9PiBhW3NvcnRLZXldIC0gYltzb3J0S2V5XSk7XHJcbiAgICAgIHN0cmluZ0FycmF5ID0gdmFsdWVcclxuICAgICAgICAuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW1bc29ydEtleV0gPT09ICdzdHJpbmcnKVxyXG4gICAgICAgIC5zb3J0KChhLCBiKSA9PiB7XHJcbiAgICAgICAgICBpZiAoYVtzb3J0S2V5XSA8IGJbc29ydEtleV0pIHJldHVybiAtMTtcclxuICAgICAgICAgIGVsc2UgaWYgKGFbc29ydEtleV0gPiBiW3NvcnRLZXldKSByZXR1cm4gMTtcclxuICAgICAgICAgIGVsc2UgcmV0dXJuIDA7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcbiAgICBjb25zdCBzb3J0ZWQgPSBbXHJcbiAgICAgIC4uLm51bWJlckFycmF5LFxyXG4gICAgICAuLi5zdHJpbmdBcnJheSxcclxuICAgICAgLi4udmFsdWUuZmlsdGVyKFxyXG4gICAgICAgIGl0ZW0gPT5cclxuICAgICAgICAgIHR5cGVvZiAoc29ydEtleSA/IGl0ZW1bc29ydEtleV0gOiBpdGVtKSAhPT0gJ251bWJlcicgJiZcclxuICAgICAgICAgIHR5cGVvZiAoc29ydEtleSA/IGl0ZW1bc29ydEtleV0gOiBpdGVtKSAhPT0gJ3N0cmluZycsXHJcbiAgICAgICksXHJcbiAgICBdO1xyXG4gICAgcmV0dXJuIHNvcnRPcmRlciA9PT0gJ2FzYycgPyBzb3J0ZWQgOiBzb3J0ZWQucmV2ZXJzZSgpO1xyXG4gIH1cclxufVxyXG4iXX0=