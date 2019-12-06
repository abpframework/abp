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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQU1oRSxNQUFNLE9BQU8sUUFBUTs7Ozs7OztJQUNuQixTQUFTLENBQ1AsS0FBWSxFQUNaLFlBQWdDLEtBQUssRUFDckMsT0FBZ0I7UUFFaEIsU0FBUyxHQUFHLFNBQVMsSUFBSSxDQUFDLG1CQUFBLFNBQVMsQ0FBQyxXQUFXLEVBQUUsRUFBTyxDQUFDLENBQUM7UUFFMUQsSUFBSSxDQUFDLEtBQUssSUFBSSxDQUFDLFNBQVMsS0FBSyxLQUFLLElBQUksU0FBUyxLQUFLLE1BQU0sQ0FBQztZQUFFLE9BQU8sS0FBSyxDQUFDOztZQUV0RSxXQUFXLEdBQUcsRUFBRTs7WUFDaEIsV0FBVyxHQUFHLEVBQUU7UUFFcEIsSUFBSSxDQUFDLE9BQU8sRUFBRTtZQUNaLFdBQVcsR0FBRyxLQUFLLENBQUMsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLEtBQUssUUFBUSxFQUFDLENBQUMsSUFBSSxFQUFFLENBQUM7WUFDcEUsV0FBVyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxPQUFPLElBQUksS0FBSyxRQUFRLEVBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztTQUNyRTthQUFNO1lBQ0wsV0FBVyxHQUFHLEtBQUs7aUJBQ2hCLE1BQU07Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQyxLQUFLLFFBQVEsRUFBQztpQkFDakQsSUFBSTs7Ozs7WUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDLEVBQUMsQ0FBQztZQUMzQyxXQUFXLEdBQUcsS0FBSztpQkFDaEIsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFDO2lCQUNqRCxJQUFJOzs7OztZQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFO2dCQUNiLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUM7b0JBQUUsT0FBTyxDQUFDLENBQUMsQ0FBQztxQkFDbEMsSUFBSSxDQUFDLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE9BQU8sQ0FBQztvQkFBRSxPQUFPLENBQUMsQ0FBQzs7b0JBQ3RDLE9BQU8sQ0FBQyxDQUFDO1lBQ2hCLENBQUMsRUFBQyxDQUFDO1NBQ047O2NBQ0ssTUFBTSxHQUFHO1lBQ2IsR0FBRyxXQUFXO1lBQ2QsR0FBRyxXQUFXO1lBQ2QsR0FBRyxLQUFLLENBQUMsTUFBTTs7OztZQUNiLElBQUksQ0FBQyxFQUFFLENBQ0wsT0FBTyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxRQUFRO2dCQUNwRCxPQUFPLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLFFBQVEsRUFDdkQ7U0FDRjtRQUNELE9BQU8sU0FBUyxLQUFLLEtBQUssQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsT0FBTyxFQUFFLENBQUM7SUFDekQsQ0FBQzs7O1lBMUNGLFVBQVU7WUFDVixJQUFJLFNBQUM7Z0JBQ0osSUFBSSxFQUFFLFNBQVM7YUFDaEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlLCBQaXBlLCBQaXBlVHJhbnNmb3JtIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5leHBvcnQgdHlwZSBTb3J0T3JkZXIgPSAnYXNjJyB8ICdkZXNjJztcbkBJbmplY3RhYmxlKClcbkBQaXBlKHtcbiAgbmFtZTogJ2FicFNvcnQnLFxufSlcbmV4cG9ydCBjbGFzcyBTb3J0UGlwZSBpbXBsZW1lbnRzIFBpcGVUcmFuc2Zvcm0ge1xuICB0cmFuc2Zvcm0oXG4gICAgdmFsdWU6IGFueVtdLFxuICAgIHNvcnRPcmRlcjogU29ydE9yZGVyIHwgc3RyaW5nID0gJ2FzYycsXG4gICAgc29ydEtleT86IHN0cmluZyxcbiAgKTogYW55IHtcbiAgICBzb3J0T3JkZXIgPSBzb3J0T3JkZXIgJiYgKHNvcnRPcmRlci50b0xvd2VyQ2FzZSgpIGFzIGFueSk7XG5cbiAgICBpZiAoIXZhbHVlIHx8IChzb3J0T3JkZXIgIT09ICdhc2MnICYmIHNvcnRPcmRlciAhPT0gJ2Rlc2MnKSkgcmV0dXJuIHZhbHVlO1xuXG4gICAgbGV0IG51bWJlckFycmF5ID0gW107XG4gICAgbGV0IHN0cmluZ0FycmF5ID0gW107XG5cbiAgICBpZiAoIXNvcnRLZXkpIHtcbiAgICAgIG51bWJlckFycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW0gPT09ICdudW1iZXInKS5zb3J0KCk7XG4gICAgICBzdHJpbmdBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtID09PSAnc3RyaW5nJykuc29ydCgpO1xuICAgIH0gZWxzZSB7XG4gICAgICBudW1iZXJBcnJheSA9IHZhbHVlXG4gICAgICAgIC5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbVtzb3J0S2V5XSA9PT0gJ251bWJlcicpXG4gICAgICAgIC5zb3J0KChhLCBiKSA9PiBhW3NvcnRLZXldIC0gYltzb3J0S2V5XSk7XG4gICAgICBzdHJpbmdBcnJheSA9IHZhbHVlXG4gICAgICAgIC5maWx0ZXIoaXRlbSA9PiB0eXBlb2YgaXRlbVtzb3J0S2V5XSA9PT0gJ3N0cmluZycpXG4gICAgICAgIC5zb3J0KChhLCBiKSA9PiB7XG4gICAgICAgICAgaWYgKGFbc29ydEtleV0gPCBiW3NvcnRLZXldKSByZXR1cm4gLTE7XG4gICAgICAgICAgZWxzZSBpZiAoYVtzb3J0S2V5XSA+IGJbc29ydEtleV0pIHJldHVybiAxO1xuICAgICAgICAgIGVsc2UgcmV0dXJuIDA7XG4gICAgICAgIH0pO1xuICAgIH1cbiAgICBjb25zdCBzb3J0ZWQgPSBbXG4gICAgICAuLi5udW1iZXJBcnJheSxcbiAgICAgIC4uLnN0cmluZ0FycmF5LFxuICAgICAgLi4udmFsdWUuZmlsdGVyKFxuICAgICAgICBpdGVtID0+XG4gICAgICAgICAgdHlwZW9mIChzb3J0S2V5ID8gaXRlbVtzb3J0S2V5XSA6IGl0ZW0pICE9PSAnbnVtYmVyJyAmJlxuICAgICAgICAgIHR5cGVvZiAoc29ydEtleSA/IGl0ZW1bc29ydEtleV0gOiBpdGVtKSAhPT0gJ3N0cmluZycsXG4gICAgICApLFxuICAgIF07XG4gICAgcmV0dXJuIHNvcnRPcmRlciA9PT0gJ2FzYycgPyBzb3J0ZWQgOiBzb3J0ZWQucmV2ZXJzZSgpO1xuICB9XG59XG4iXX0=