/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUVoRTtJQUFBO0lBMkNBLENBQUM7Ozs7Ozs7SUF0Q0MsNEJBQVM7Ozs7OztJQUFULFVBQ0UsS0FBWSxFQUNaLFNBQXFDLEVBQ3JDLE9BQWdCO1FBRGhCLDBCQUFBLEVBQUEsaUJBQXFDO1FBR3JDLFNBQVMsR0FBRyxTQUFTLElBQUksQ0FBQyxtQkFBQSxTQUFTLENBQUMsV0FBVyxFQUFFLEVBQU8sQ0FBQyxDQUFDO1FBRTFELElBQUksQ0FBQyxLQUFLLElBQUksQ0FBQyxTQUFTLEtBQUssS0FBSyxJQUFJLFNBQVMsS0FBSyxNQUFNLENBQUM7WUFBRSxPQUFPLEtBQUssQ0FBQzs7WUFFdEUsV0FBVyxHQUFHLEVBQUU7O1lBQ2hCLFdBQVcsR0FBRyxFQUFFO1FBRXBCLElBQUksQ0FBQyxPQUFPLEVBQUU7WUFDWixXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxVQUFBLElBQUksSUFBSSxPQUFBLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBeEIsQ0FBd0IsRUFBQyxDQUFDLElBQUksRUFBRSxDQUFDO1lBQ3BFLFdBQVcsR0FBRyxLQUFLLENBQUMsTUFBTTs7OztZQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsT0FBTyxJQUFJLEtBQUssUUFBUSxFQUF4QixDQUF3QixFQUFDLENBQUMsSUFBSSxFQUFFLENBQUM7U0FDckU7YUFBTTtZQUNMLFdBQVcsR0FBRyxLQUFLO2lCQUNoQixNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUMsS0FBSyxRQUFRLEVBQWpDLENBQWlDLEVBQUM7aUJBQ2pELElBQUk7Ozs7O1lBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQyxJQUFLLE9BQUEsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUMsRUFBdkIsQ0FBdUIsRUFBQyxDQUFDO1lBQzNDLFdBQVcsR0FBRyxLQUFLO2lCQUNoQixNQUFNOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUMsS0FBSyxRQUFRLEVBQWpDLENBQWlDLEVBQUM7aUJBQ2pELElBQUk7Ozs7O1lBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQztnQkFDVCxJQUFJLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDO29CQUFFLE9BQU8sQ0FBQyxDQUFDLENBQUM7cUJBQ2xDLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUM7b0JBQUUsT0FBTyxDQUFDLENBQUM7O29CQUN0QyxPQUFPLENBQUMsQ0FBQztZQUNoQixDQUFDLEVBQUMsQ0FBQztTQUNOOztZQUNLLE1BQU0sb0JBQ1AsV0FBVyxFQUNYLFdBQVcsRUFDWCxLQUFLLENBQUMsTUFBTTs7OztRQUNiLFVBQUEsSUFBSTtZQUNGLE9BQUEsT0FBTyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxRQUFRO2dCQUNwRCxPQUFPLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLFFBQVE7UUFEcEQsQ0FDb0QsRUFDdkQsQ0FDRjtRQUNELE9BQU8sU0FBUyxLQUFLLEtBQUssQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsT0FBTyxFQUFFLENBQUM7SUFDekQsQ0FBQzs7Z0JBMUNGLFVBQVU7Z0JBQ1YsSUFBSSxTQUFDO29CQUNKLElBQUksRUFBRSxTQUFTO2lCQUNoQjs7SUF3Q0QsZUFBQztDQUFBLEFBM0NELElBMkNDO1NBdkNZLFFBQVEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlLCBQaXBlLCBQaXBlVHJhbnNmb3JtIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmV4cG9ydCB0eXBlIFNvcnRPcmRlciA9ICdhc2MnIHwgJ2Rlc2MnO1xyXG5ASW5qZWN0YWJsZSgpXHJcbkBQaXBlKHtcclxuICBuYW1lOiAnYWJwU29ydCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTb3J0UGlwZSBpbXBsZW1lbnRzIFBpcGVUcmFuc2Zvcm0ge1xyXG4gIHRyYW5zZm9ybShcclxuICAgIHZhbHVlOiBhbnlbXSxcclxuICAgIHNvcnRPcmRlcjogU29ydE9yZGVyIHwgc3RyaW5nID0gJ2FzYycsXHJcbiAgICBzb3J0S2V5Pzogc3RyaW5nLFxyXG4gICk6IGFueSB7XHJcbiAgICBzb3J0T3JkZXIgPSBzb3J0T3JkZXIgJiYgKHNvcnRPcmRlci50b0xvd2VyQ2FzZSgpIGFzIGFueSk7XHJcblxyXG4gICAgaWYgKCF2YWx1ZSB8fCAoc29ydE9yZGVyICE9PSAnYXNjJyAmJiBzb3J0T3JkZXIgIT09ICdkZXNjJykpIHJldHVybiB2YWx1ZTtcclxuXHJcbiAgICBsZXQgbnVtYmVyQXJyYXkgPSBbXTtcclxuICAgIGxldCBzdHJpbmdBcnJheSA9IFtdO1xyXG5cclxuICAgIGlmICghc29ydEtleSkge1xyXG4gICAgICBudW1iZXJBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtID09PSAnbnVtYmVyJykuc29ydCgpO1xyXG4gICAgICBzdHJpbmdBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtID09PSAnc3RyaW5nJykuc29ydCgpO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgbnVtYmVyQXJyYXkgPSB2YWx1ZVxyXG4gICAgICAgIC5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbVtzb3J0S2V5XSA9PT0gJ251bWJlcicpXHJcbiAgICAgICAgLnNvcnQoKGEsIGIpID0+IGFbc29ydEtleV0gLSBiW3NvcnRLZXldKTtcclxuICAgICAgc3RyaW5nQXJyYXkgPSB2YWx1ZVxyXG4gICAgICAgIC5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbVtzb3J0S2V5XSA9PT0gJ3N0cmluZycpXHJcbiAgICAgICAgLnNvcnQoKGEsIGIpID0+IHtcclxuICAgICAgICAgIGlmIChhW3NvcnRLZXldIDwgYltzb3J0S2V5XSkgcmV0dXJuIC0xO1xyXG4gICAgICAgICAgZWxzZSBpZiAoYVtzb3J0S2V5XSA+IGJbc29ydEtleV0pIHJldHVybiAxO1xyXG4gICAgICAgICAgZWxzZSByZXR1cm4gMDtcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuICAgIGNvbnN0IHNvcnRlZCA9IFtcclxuICAgICAgLi4ubnVtYmVyQXJyYXksXHJcbiAgICAgIC4uLnN0cmluZ0FycmF5LFxyXG4gICAgICAuLi52YWx1ZS5maWx0ZXIoXHJcbiAgICAgICAgaXRlbSA9PlxyXG4gICAgICAgICAgdHlwZW9mIChzb3J0S2V5ID8gaXRlbVtzb3J0S2V5XSA6IGl0ZW0pICE9PSAnbnVtYmVyJyAmJlxyXG4gICAgICAgICAgdHlwZW9mIChzb3J0S2V5ID8gaXRlbVtzb3J0S2V5XSA6IGl0ZW0pICE9PSAnc3RyaW5nJyxcclxuICAgICAgKSxcclxuICAgIF07XHJcbiAgICByZXR1cm4gc29ydE9yZGVyID09PSAnYXNjJyA/IHNvcnRlZCA6IHNvcnRlZC5yZXZlcnNlKCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==