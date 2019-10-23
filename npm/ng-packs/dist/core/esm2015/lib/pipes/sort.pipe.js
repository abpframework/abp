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
    sortOrder = sortOrder && /** @type {?} */ (sortOrder.toLowerCase());
    if (!value || (sortOrder !== 'asc' && sortOrder !== 'desc')) return value;
    /** @type {?} */
    let numberArray = [];
    /** @type {?} */
    let stringArray = [];
    if (!sortKey) {
      numberArray = value
        .filter(
          /**
           * @param {?} item
           * @return {?}
           */
          item => typeof item === 'number',
        )
        .sort();
      stringArray = value
        .filter(
          /**
           * @param {?} item
           * @return {?}
           */
          item => typeof item === 'string',
        )
        .sort();
    } else {
      numberArray = value
        .filter(
          /**
           * @param {?} item
           * @return {?}
           */
          item => typeof item[sortKey] === 'number',
        )
        .sort(
          /**
           * @param {?} a
           * @param {?} b
           * @return {?}
           */
          (a, b) => a[sortKey] - b[sortKey],
        );
      stringArray = value
        .filter(
          /**
           * @param {?} item
           * @return {?}
           */
          item => typeof item[sortKey] === 'string',
        )
        .sort(
          /**
           * @param {?} a
           * @param {?} b
           * @return {?}
           */
          (a, b) => {
            if (a[sortKey] < b[sortKey]) return -1;
            else if (a[sortKey] > b[sortKey]) return 1;
            else return 0;
          },
        );
    }
    /** @type {?} */
    const sorted = numberArray.concat(stringArray);
    return sortOrder === 'asc' ? sorted : sorted.reverse();
  }
}
SortPipe.decorators = [
  { type: Injectable },
  {
    type: Pipe,
    args: [
      {
        name: 'abpSort',
      },
    ],
  },
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBTWhFLE1BQU0sT0FBTyxRQUFROzs7Ozs7O0lBQ25CLFNBQVMsQ0FBQyxLQUFZLEVBQUUsWUFBZ0MsS0FBSyxFQUFFLE9BQWdCO1FBQzdFLFNBQVMsR0FBRyxTQUFTLElBQUksQ0FBQyxtQkFBQSxTQUFTLENBQUMsV0FBVyxFQUFFLEVBQU8sQ0FBQyxDQUFDO1FBRTFELElBQUksQ0FBQyxLQUFLLElBQUksQ0FBQyxTQUFTLEtBQUssS0FBSyxJQUFJLFNBQVMsS0FBSyxNQUFNLENBQUM7WUFBRSxPQUFPLEtBQUssQ0FBQzs7WUFFdEUsV0FBVyxHQUFHLEVBQUU7O1lBQ2hCLFdBQVcsR0FBRyxFQUFFO1FBRXBCLElBQUksQ0FBQyxPQUFPLEVBQUU7WUFDWixXQUFXLEdBQUcsS0FBSyxDQUFDLE1BQU07Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBQyxDQUFDLElBQUksRUFBRSxDQUFDO1lBQ3BFLFdBQVcsR0FBRyxLQUFLLENBQUMsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLEtBQUssUUFBUSxFQUFDLENBQUMsSUFBSSxFQUFFLENBQUM7U0FDckU7YUFBTTtZQUNMLFdBQVcsR0FBRyxLQUFLLENBQUMsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFDLENBQUMsSUFBSTs7Ozs7WUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDLEVBQUMsQ0FBQztZQUM5RyxXQUFXLEdBQUcsS0FBSztpQkFDaEIsTUFBTTs7OztZQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLEtBQUssUUFBUSxFQUFDO2lCQUNqRCxJQUFJOzs7OztZQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFO2dCQUNiLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUM7b0JBQUUsT0FBTyxDQUFDLENBQUMsQ0FBQztxQkFDbEMsSUFBSSxDQUFDLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE9BQU8sQ0FBQztvQkFBRSxPQUFPLENBQUMsQ0FBQzs7b0JBQ3RDLE9BQU8sQ0FBQyxDQUFDO1lBQ2hCLENBQUMsRUFBQyxDQUFDO1NBQ047O2NBQ0ssTUFBTSxHQUFHLFdBQVcsQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDO1FBQzlDLE9BQU8sU0FBUyxLQUFLLEtBQUssQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsT0FBTyxFQUFFLENBQUM7SUFDekQsQ0FBQzs7O1lBNUJGLFVBQVU7WUFDVixJQUFJLFNBQUM7Z0JBQ0osSUFBSSxFQUFFLFNBQVM7YUFDaEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlLCBQaXBlLCBQaXBlVHJhbnNmb3JtIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5leHBvcnQgdHlwZSBTb3J0T3JkZXIgPSAnYXNjJyB8ICdkZXNjJztcbkBJbmplY3RhYmxlKClcbkBQaXBlKHtcbiAgbmFtZTogJ2FicFNvcnQnLFxufSlcbmV4cG9ydCBjbGFzcyBTb3J0UGlwZSBpbXBsZW1lbnRzIFBpcGVUcmFuc2Zvcm0ge1xuICB0cmFuc2Zvcm0odmFsdWU6IGFueVtdLCBzb3J0T3JkZXI6IFNvcnRPcmRlciB8IHN0cmluZyA9ICdhc2MnLCBzb3J0S2V5Pzogc3RyaW5nKTogYW55IHtcbiAgICBzb3J0T3JkZXIgPSBzb3J0T3JkZXIgJiYgKHNvcnRPcmRlci50b0xvd2VyQ2FzZSgpIGFzIGFueSk7XG5cbiAgICBpZiAoIXZhbHVlIHx8IChzb3J0T3JkZXIgIT09ICdhc2MnICYmIHNvcnRPcmRlciAhPT0gJ2Rlc2MnKSkgcmV0dXJuIHZhbHVlO1xuXG4gICAgbGV0IG51bWJlckFycmF5ID0gW107XG4gICAgbGV0IHN0cmluZ0FycmF5ID0gW107XG5cbiAgICBpZiAoIXNvcnRLZXkpIHtcbiAgICAgIG51bWJlckFycmF5ID0gdmFsdWUuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW0gPT09ICdudW1iZXInKS5zb3J0KCk7XG4gICAgICBzdHJpbmdBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtID09PSAnc3RyaW5nJykuc29ydCgpO1xuICAgIH0gZWxzZSB7XG4gICAgICBudW1iZXJBcnJheSA9IHZhbHVlLmZpbHRlcihpdGVtID0+IHR5cGVvZiBpdGVtW3NvcnRLZXldID09PSAnbnVtYmVyJykuc29ydCgoYSwgYikgPT4gYVtzb3J0S2V5XSAtIGJbc29ydEtleV0pO1xuICAgICAgc3RyaW5nQXJyYXkgPSB2YWx1ZVxuICAgICAgICAuZmlsdGVyKGl0ZW0gPT4gdHlwZW9mIGl0ZW1bc29ydEtleV0gPT09ICdzdHJpbmcnKVxuICAgICAgICAuc29ydCgoYSwgYikgPT4ge1xuICAgICAgICAgIGlmIChhW3NvcnRLZXldIDwgYltzb3J0S2V5XSkgcmV0dXJuIC0xO1xuICAgICAgICAgIGVsc2UgaWYgKGFbc29ydEtleV0gPiBiW3NvcnRLZXldKSByZXR1cm4gMTtcbiAgICAgICAgICBlbHNlIHJldHVybiAwO1xuICAgICAgICB9KTtcbiAgICB9XG4gICAgY29uc3Qgc29ydGVkID0gbnVtYmVyQXJyYXkuY29uY2F0KHN0cmluZ0FycmF5KTtcbiAgICByZXR1cm4gc29ydE9yZGVyID09PSAnYXNjJyA/IHNvcnRlZCA6IHNvcnRlZC5yZXZlcnNlKCk7XG4gIH1cbn1cbiJdfQ==
