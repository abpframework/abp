/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Pipe } from '@angular/core';
var SortPipe = /** @class */ (function () {
    function SortPipe() {
    }
    /**
     * @param {?} value
     * @param {?} sortOrder
     * @return {?}
     */
    SortPipe.prototype.transform = /**
     * @param {?} value
     * @param {?} sortOrder
     * @return {?}
     */
    function (value, sortOrder) {
        sortOrder = sortOrder.toLowerCase();
        if (sortOrder === "desc")
            return value.reverse();
        else
            return value;
    };
    SortPipe.decorators = [
        { type: Pipe, args: [{
                    name: 'abpSort',
                    pure: false
                },] }
    ];
    return SortPipe;
}());
export { SortPipe };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC5waXBlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BpcGVzL3NvcnQucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLElBQUksRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFFcEQ7SUFBQTtJQVVBLENBQUM7Ozs7OztJQUxHLDRCQUFTOzs7OztJQUFULFVBQVUsS0FBWSxFQUFFLFNBQWlCO1FBQ3JDLFNBQVMsR0FBRyxTQUFTLENBQUMsV0FBVyxFQUFFLENBQUM7UUFDcEMsSUFBRyxTQUFTLEtBQUssTUFBTTtZQUFFLE9BQU8sS0FBSyxDQUFDLE9BQU8sRUFBRSxDQUFDOztZQUMzQyxPQUFPLEtBQUssQ0FBQztJQUN0QixDQUFDOztnQkFUSixJQUFJLFNBQUM7b0JBQ0YsSUFBSSxFQUFFLFNBQVM7b0JBQ2YsSUFBSSxFQUFFLEtBQUs7aUJBQ2Q7O0lBT0QsZUFBQztDQUFBLEFBVkQsSUFVQztTQU5ZLFFBQVEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBQaXBlLCBQaXBlVHJhbnNmb3JtIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBQaXBlKHtcbiAgICBuYW1lOiAnYWJwU29ydCcsXG4gICAgcHVyZTogZmFsc2Vcbn0pXG5leHBvcnQgY2xhc3MgU29ydFBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtIHtcbiAgICB0cmFuc2Zvcm0odmFsdWU6IGFueVtdLCBzb3J0T3JkZXI6IHN0cmluZyk6IGFueSB7XG4gICAgICAgIHNvcnRPcmRlciA9IHNvcnRPcmRlci50b0xvd2VyQ2FzZSgpO1xuICAgICAgICBpZihzb3J0T3JkZXIgPT09IFwiZGVzY1wiKSByZXR1cm4gdmFsdWUucmV2ZXJzZSgpO1xuICAgICAgICBlbHNlIHJldHVybiB2YWx1ZTtcbiAgICB9XG59Il19