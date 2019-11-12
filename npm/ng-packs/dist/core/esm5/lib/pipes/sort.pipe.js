/**
 * @fileoverview added by tsickle
 * Generated from: lib/pipes/sort.pipe.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUVoRTtJQUFBO0lBNkJBLENBQUM7Ozs7Ozs7SUF4QkMsNEJBQVM7Ozs7OztJQUFULFVBQVUsS0FBWSxFQUFFLFNBQXFDLEVBQUUsT0FBZ0I7UUFBdkQsMEJBQUEsRUFBQSxpQkFBcUM7UUFDM0QsU0FBUyxHQUFHLFNBQVMsSUFBSSxDQUFDLG1CQUFBLFNBQVMsQ0FBQyxXQUFXLEVBQUUsRUFBTyxDQUFDLENBQUM7UUFFMUQsSUFBSSxDQUFDLEtBQUssSUFBSSxDQUFDLFNBQVMsS0FBSyxLQUFLLElBQUksU0FBUyxLQUFLLE1BQU0sQ0FBQztZQUFFLE9BQU8sS0FBSyxDQUFDOztZQUV0RSxXQUFXLEdBQUcsRUFBRTs7WUFDaEIsV0FBVyxHQUFHLEVBQUU7UUFFcEIsSUFBSSxDQUFDLE9BQU8sRUFBRTtZQUNaLFdBQVcsR0FBRyxLQUFLLENBQUMsTUFBTTs7OztZQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsT0FBTyxJQUFJLEtBQUssUUFBUSxFQUF4QixDQUF3QixFQUFDLENBQUMsSUFBSSxFQUFFLENBQUM7WUFDcEUsV0FBVyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksS0FBSyxRQUFRLEVBQXhCLENBQXdCLEVBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztTQUNyRTthQUFNO1lBQ0wsV0FBVyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUMsS0FBSyxRQUFRLEVBQWpDLENBQWlDLEVBQUMsQ0FBQyxJQUFJOzs7OztZQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDLEVBQXZCLENBQXVCLEVBQUMsQ0FBQztZQUM5RyxXQUFXLEdBQUcsS0FBSztpQkFDaEIsTUFBTTs7OztZQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFqQyxDQUFpQyxFQUFDO2lCQUNqRCxJQUFJOzs7OztZQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUM7Z0JBQ1QsSUFBSSxDQUFDLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE9BQU8sQ0FBQztvQkFBRSxPQUFPLENBQUMsQ0FBQyxDQUFDO3FCQUNsQyxJQUFJLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDO29CQUFFLE9BQU8sQ0FBQyxDQUFDOztvQkFDdEMsT0FBTyxDQUFDLENBQUM7WUFDaEIsQ0FBQyxFQUFDLENBQUM7U0FDTjs7WUFDSyxNQUFNLEdBQUcsV0FBVyxDQUFDLE1BQU0sQ0FBQyxXQUFXLENBQUM7UUFDOUMsT0FBTyxTQUFTLEtBQUssS0FBSyxDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxPQUFPLEVBQUUsQ0FBQztJQUN6RCxDQUFDOztnQkE1QkYsVUFBVTtnQkFDVixJQUFJLFNBQUM7b0JBQ0osSUFBSSxFQUFFLFNBQVM7aUJBQ2hCOztJQTBCRCxlQUFDO0NBQUEsQUE3QkQsSUE2QkM7U0F6QlksUUFBUSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIFBpcGUsIFBpcGVUcmFuc2Zvcm0gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuZXhwb3J0IHR5cGUgU29ydE9yZGVyID0gJ2FzYycgfCAnZGVzYyc7XHJcbkBJbmplY3RhYmxlKClcclxuQFBpcGUoe1xyXG4gIG5hbWU6ICdhYnBTb3J0JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFNvcnRQaXBlIGltcGxlbWVudHMgUGlwZVRyYW5zZm9ybSB7XHJcbiAgdHJhbnNmb3JtKHZhbHVlOiBhbnlbXSwgc29ydE9yZGVyOiBTb3J0T3JkZXIgfCBzdHJpbmcgPSAnYXNjJywgc29ydEtleT86IHN0cmluZyk6IGFueSB7XHJcbiAgICBzb3J0T3JkZXIgPSBzb3J0T3JkZXIgJiYgKHNvcnRPcmRlci50b0xvd2VyQ2FzZSgpIGFzIGFueSk7XHJcblxyXG4gICAgaWYgKCF2YWx1ZSB8fCAoc29ydE9yZGVyICE9PSAnYXNjJyAmJiBzb3J0T3JkZXIgIT09ICdkZXNjJykpIHJldHVybiB2YWx1ZTtcclxuXHJcbiAgICBsZXQgbnVtYmVyQXJyYXkgPSBbXTtcclxuICAgIGxldCBzdHJpbmdBcnJheSA9IFtdO1xyXG5cclxuICAgIGlmICghc29ydEtleSkge1xyXG4gICAgICBudW1iZXJBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtID09PSAnbnVtYmVyJykuc29ydCgpO1xyXG4gICAgICBzdHJpbmdBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtID09PSAnc3RyaW5nJykuc29ydCgpO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgbnVtYmVyQXJyYXkgPSB2YWx1ZS5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbVtzb3J0S2V5XSA9PT0gJ251bWJlcicpLnNvcnQoKGEsIGIpID0+IGFbc29ydEtleV0gLSBiW3NvcnRLZXldKTtcclxuICAgICAgc3RyaW5nQXJyYXkgPSB2YWx1ZVxyXG4gICAgICAgIC5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbVtzb3J0S2V5XSA9PT0gJ3N0cmluZycpXHJcbiAgICAgICAgLnNvcnQoKGEsIGIpID0+IHtcclxuICAgICAgICAgIGlmIChhW3NvcnRLZXldIDwgYltzb3J0S2V5XSkgcmV0dXJuIC0xO1xyXG4gICAgICAgICAgZWxzZSBpZiAoYVtzb3J0S2V5XSA+IGJbc29ydEtleV0pIHJldHVybiAxO1xyXG4gICAgICAgICAgZWxzZSByZXR1cm4gMDtcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuICAgIGNvbnN0IHNvcnRlZCA9IG51bWJlckFycmF5LmNvbmNhdChzdHJpbmdBcnJheSk7XHJcbiAgICByZXR1cm4gc29ydE9yZGVyID09PSAnYXNjJyA/IHNvcnRlZCA6IHNvcnRlZC5yZXZlcnNlKCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==