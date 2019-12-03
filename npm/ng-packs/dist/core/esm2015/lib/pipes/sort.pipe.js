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
            numberArray = value
                .filter((/**
             * @param {?} item
             * @return {?}
             */
            item => typeof item[sortKey] === 'number'))
                .sort((/**
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
        const sorted = [
            ...numberArray,
            ...stringArray,
            ...value.filter((/**
             * @param {?} item
             * @return {?}
             */
            item => typeof (sortKey ? item[sortKey] : item) !== 'number' &&
                typeof (sortKey ? item[sortKey] : item) !== 'string')),
        ];
        return sortOrder === 'asc' ? sorted : sorted.reverse();
    }
}
SortPipe.decorators = [
    { type: Injectable },
    { type: Pipe, args: [{
                name: 'abpSort',
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBTWhFLE1BQU0sT0FBTyxRQUFROzs7Ozs7O0lBQ25CLFNBQVMsQ0FDUCxLQUFZLEVBQ1osWUFBZ0MsS0FBSyxFQUNyQyxPQUFnQjtRQUVoQixTQUFTLEdBQUcsU0FBUyxJQUFJLENBQUMsbUJBQUEsU0FBUyxDQUFDLFdBQVcsRUFBRSxFQUFPLENBQUMsQ0FBQztRQUUxRCxJQUFJLENBQUMsS0FBSyxJQUFJLENBQUMsU0FBUyxLQUFLLEtBQUssSUFBSSxTQUFTLEtBQUssTUFBTSxDQUFDO1lBQUUsT0FBTyxLQUFLLENBQUM7O1lBRXRFLFdBQVcsR0FBRyxFQUFFOztZQUNoQixXQUFXLEdBQUcsRUFBRTtRQUVwQixJQUFJLENBQUMsT0FBTyxFQUFFO1lBQ1osV0FBVyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxPQUFPLElBQUksS0FBSyxRQUFRLEVBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztZQUNwRSxXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBQyxDQUFDLElBQUksRUFBRSxDQUFDO1NBQ3JFO2FBQU07WUFDTCxXQUFXLEdBQUcsS0FBSztpQkFDaEIsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFDO2lCQUNqRCxJQUFJOzs7OztZQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUMsRUFBQyxDQUFDO1lBQzNDLFdBQVcsR0FBRyxLQUFLO2lCQUNoQixNQUFNOzs7O1lBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUMsS0FBSyxRQUFRLEVBQUM7aUJBQ2pELElBQUk7Ozs7O1lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFFLEVBQUU7Z0JBQ2IsSUFBSSxDQUFDLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE9BQU8sQ0FBQztvQkFBRSxPQUFPLENBQUMsQ0FBQyxDQUFDO3FCQUNsQyxJQUFJLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDO29CQUFFLE9BQU8sQ0FBQyxDQUFDOztvQkFDdEMsT0FBTyxDQUFDLENBQUM7WUFDaEIsQ0FBQyxFQUFDLENBQUM7U0FDTjs7Y0FDSyxNQUFNLEdBQUc7WUFDYixHQUFHLFdBQVc7WUFDZCxHQUFHLFdBQVc7WUFDZCxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQ2IsSUFBSSxDQUFDLEVBQUUsQ0FDTCxPQUFPLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLFFBQVE7Z0JBQ3BELE9BQU8sQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLEtBQUssUUFBUSxFQUN2RDtTQUNGO1FBQ0QsT0FBTyxTQUFTLEtBQUssS0FBSyxDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxPQUFPLEVBQUUsQ0FBQztJQUN6RCxDQUFDOzs7WUExQ0YsVUFBVTtZQUNWLElBQUksU0FBQztnQkFDSixJQUFJLEVBQUUsU0FBUzthQUNoQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIFBpcGUsIFBpcGVUcmFuc2Zvcm0gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuZXhwb3J0IHR5cGUgU29ydE9yZGVyID0gJ2FzYycgfCAnZGVzYyc7XHJcbkBJbmplY3RhYmxlKClcclxuQFBpcGUoe1xyXG4gIG5hbWU6ICdhYnBTb3J0JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFNvcnRQaXBlIGltcGxlbWVudHMgUGlwZVRyYW5zZm9ybSB7XHJcbiAgdHJhbnNmb3JtKFxyXG4gICAgdmFsdWU6IGFueVtdLFxyXG4gICAgc29ydE9yZGVyOiBTb3J0T3JkZXIgfCBzdHJpbmcgPSAnYXNjJyxcclxuICAgIHNvcnRLZXk/OiBzdHJpbmcsXHJcbiAgKTogYW55IHtcclxuICAgIHNvcnRPcmRlciA9IHNvcnRPcmRlciAmJiAoc29ydE9yZGVyLnRvTG93ZXJDYXNlKCkgYXMgYW55KTtcclxuXHJcbiAgICBpZiAoIXZhbHVlIHx8IChzb3J0T3JkZXIgIT09ICdhc2MnICYmIHNvcnRPcmRlciAhPT0gJ2Rlc2MnKSkgcmV0dXJuIHZhbHVlO1xyXG5cclxuICAgIGxldCBudW1iZXJBcnJheSA9IFtdO1xyXG4gICAgbGV0IHN0cmluZ0FycmF5ID0gW107XHJcblxyXG4gICAgaWYgKCFzb3J0S2V5KSB7XHJcbiAgICAgIG51bWJlckFycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW0gPT09ICdudW1iZXInKS5zb3J0KCk7XHJcbiAgICAgIHN0cmluZ0FycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW0gPT09ICdzdHJpbmcnKS5zb3J0KCk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBudW1iZXJBcnJheSA9IHZhbHVlXHJcbiAgICAgICAgLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtW3NvcnRLZXldID09PSAnbnVtYmVyJylcclxuICAgICAgICAuc29ydCgoYSwgYikgPT4gYVtzb3J0S2V5XSAtIGJbc29ydEtleV0pO1xyXG4gICAgICBzdHJpbmdBcnJheSA9IHZhbHVlXHJcbiAgICAgICAgLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtW3NvcnRLZXldID09PSAnc3RyaW5nJylcclxuICAgICAgICAuc29ydCgoYSwgYikgPT4ge1xyXG4gICAgICAgICAgaWYgKGFbc29ydEtleV0gPCBiW3NvcnRLZXldKSByZXR1cm4gLTE7XHJcbiAgICAgICAgICBlbHNlIGlmIChhW3NvcnRLZXldID4gYltzb3J0S2V5XSkgcmV0dXJuIDE7XHJcbiAgICAgICAgICBlbHNlIHJldHVybiAwO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG4gICAgY29uc3Qgc29ydGVkID0gW1xyXG4gICAgICAuLi5udW1iZXJBcnJheSxcclxuICAgICAgLi4uc3RyaW5nQXJyYXksXHJcbiAgICAgIC4uLnZhbHVlLmZpbHRlcihcclxuICAgICAgICBpdGVtID0+XHJcbiAgICAgICAgICB0eXBlb2YgKHNvcnRLZXkgPyBpdGVtW3NvcnRLZXldIDogaXRlbSkgIT09ICdudW1iZXInICYmXHJcbiAgICAgICAgICB0eXBlb2YgKHNvcnRLZXkgPyBpdGVtW3NvcnRLZXldIDogaXRlbSkgIT09ICdzdHJpbmcnLFxyXG4gICAgICApLFxyXG4gICAgXTtcclxuICAgIHJldHVybiBzb3J0T3JkZXIgPT09ICdhc2MnID8gc29ydGVkIDogc29ydGVkLnJldmVyc2UoKTtcclxuICB9XHJcbn1cclxuIl19