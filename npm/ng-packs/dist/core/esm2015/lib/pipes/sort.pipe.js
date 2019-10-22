/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBTWhFLE1BQU0sT0FBTyxRQUFROzs7Ozs7O0lBQ25CLFNBQVMsQ0FBQyxLQUFZLEVBQUUsWUFBZ0MsS0FBSyxFQUFFLE9BQWdCO1FBQzdFLFNBQVMsR0FBRyxTQUFTLElBQUksQ0FBQyxtQkFBQSxTQUFTLENBQUMsV0FBVyxFQUFFLEVBQU8sQ0FBQyxDQUFDO1FBRTFELElBQUksQ0FBQyxLQUFLLElBQUksQ0FBQyxTQUFTLEtBQUssS0FBSyxJQUFJLFNBQVMsS0FBSyxNQUFNLENBQUM7WUFBRSxPQUFPLEtBQUssQ0FBQzs7WUFFdEUsV0FBVyxHQUFHLEVBQUU7O1lBQ2hCLFdBQVcsR0FBRyxFQUFFO1FBRXBCLElBQUksQ0FBQyxPQUFPLEVBQUU7WUFDWixXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBQyxDQUFDLElBQUksRUFBRSxDQUFDO1lBQ3BFLFdBQVcsR0FBRyxLQUFLLENBQUMsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLEtBQUssUUFBUSxFQUFDLENBQUMsSUFBSSxFQUFFLENBQUM7U0FDckU7YUFBTTtZQUNMLFdBQVcsR0FBRyxLQUFLLENBQUMsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFDLENBQUMsSUFBSTs7Ozs7WUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDLEVBQUMsQ0FBQztZQUM5RyxXQUFXLEdBQUcsS0FBSztpQkFDaEIsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFDO2lCQUNqRCxJQUFJOzs7OztZQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFO2dCQUNiLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUM7b0JBQUUsT0FBTyxDQUFDLENBQUMsQ0FBQztxQkFDbEMsSUFBSSxDQUFDLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE9BQU8sQ0FBQztvQkFBRSxPQUFPLENBQUMsQ0FBQzs7b0JBQ3RDLE9BQU8sQ0FBQyxDQUFDO1lBQ2hCLENBQUMsRUFBQyxDQUFDO1NBQ047O2NBQ0ssTUFBTSxHQUFHLFdBQVcsQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDO1FBQzlDLE9BQU8sU0FBUyxLQUFLLEtBQUssQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsT0FBTyxFQUFFLENBQUM7SUFDekQsQ0FBQzs7O1lBNUJGLFVBQVU7WUFDVixJQUFJLFNBQUM7Z0JBQ0osSUFBSSxFQUFFLFNBQVM7YUFDaEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlLCBQaXBlLCBQaXBlVHJhbnNmb3JtIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmV4cG9ydCB0eXBlIFNvcnRPcmRlciA9ICdhc2MnIHwgJ2Rlc2MnO1xyXG5ASW5qZWN0YWJsZSgpXHJcbkBQaXBlKHtcclxuICBuYW1lOiAnYWJwU29ydCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTb3J0UGlwZSBpbXBsZW1lbnRzIFBpcGVUcmFuc2Zvcm0ge1xyXG4gIHRyYW5zZm9ybSh2YWx1ZTogYW55W10sIHNvcnRPcmRlcjogU29ydE9yZGVyIHwgc3RyaW5nID0gJ2FzYycsIHNvcnRLZXk/OiBzdHJpbmcpOiBhbnkge1xyXG4gICAgc29ydE9yZGVyID0gc29ydE9yZGVyICYmIChzb3J0T3JkZXIudG9Mb3dlckNhc2UoKSBhcyBhbnkpO1xyXG5cclxuICAgIGlmICghdmFsdWUgfHwgKHNvcnRPcmRlciAhPT0gJ2FzYycgJiYgc29ydE9yZGVyICE9PSAnZGVzYycpKSByZXR1cm4gdmFsdWU7XHJcblxyXG4gICAgbGV0IG51bWJlckFycmF5ID0gW107XHJcbiAgICBsZXQgc3RyaW5nQXJyYXkgPSBbXTtcclxuXHJcbiAgICBpZiAoIXNvcnRLZXkpIHtcclxuICAgICAgbnVtYmVyQXJyYXkgPSB2YWx1ZS5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbSA9PT0gJ251bWJlcicpLnNvcnQoKTtcclxuICAgICAgc3RyaW5nQXJyYXkgPSB2YWx1ZS5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbSA9PT0gJ3N0cmluZycpLnNvcnQoKTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgIG51bWJlckFycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW1bc29ydEtleV0gPT09ICdudW1iZXInKS5zb3J0KChhLCBiKSA9PiBhW3NvcnRLZXldIC0gYltzb3J0S2V5XSk7XHJcbiAgICAgIHN0cmluZ0FycmF5ID0gdmFsdWVcclxuICAgICAgICAuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW1bc29ydEtleV0gPT09ICdzdHJpbmcnKVxyXG4gICAgICAgIC5zb3J0KChhLCBiKSA9PiB7XHJcbiAgICAgICAgICBpZiAoYVtzb3J0S2V5XSA8IGJbc29ydEtleV0pIHJldHVybiAtMTtcclxuICAgICAgICAgIGVsc2UgaWYgKGFbc29ydEtleV0gPiBiW3NvcnRLZXldKSByZXR1cm4gMTtcclxuICAgICAgICAgIGVsc2UgcmV0dXJuIDA7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcbiAgICBjb25zdCBzb3J0ZWQgPSBudW1iZXJBcnJheS5jb25jYXQoc3RyaW5nQXJyYXkpO1xyXG4gICAgcmV0dXJuIHNvcnRPcmRlciA9PT0gJ2FzYycgPyBzb3J0ZWQgOiBzb3J0ZWQucmV2ZXJzZSgpO1xyXG4gIH1cclxufVxyXG4iXX0=