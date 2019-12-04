/**
 * @fileoverview added by tsickle
 * Generated from: lib/utils/date-parser-formatter.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable, Optional } from '@angular/core';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';
/**
 * @param {?} value
 * @return {?}
 */
function padNumber(value) {
    if (isNumber(value)) {
        return `0${value}`.slice(-2);
    }
    else {
        return '';
    }
}
/**
 * @param {?} value
 * @return {?}
 */
function isNumber(value) {
    return !isNaN(toInteger(value));
}
/**
 * @param {?} value
 * @return {?}
 */
function toInteger(value) {
    return parseInt(`${value}`, 10);
}
export class DateParserFormatter extends NgbDateParserFormatter {
    /**
     * @param {?} datePipe
     */
    constructor(datePipe) {
        super();
        this.datePipe = datePipe;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    parse(value) {
        if (value) {
            /** @type {?} */
            const dateParts = value.trim().split('-');
            if (dateParts.length === 1 && isNumber(dateParts[0])) {
                return { year: toInteger(dateParts[0]), month: null, day: null };
            }
            else if (dateParts.length === 2 && isNumber(dateParts[0]) && isNumber(dateParts[1])) {
                return { year: toInteger(dateParts[0]), month: toInteger(dateParts[1]), day: null };
            }
            else if (dateParts.length === 3 && isNumber(dateParts[0]) && isNumber(dateParts[1]) && isNumber(dateParts[2])) {
                return { year: toInteger(dateParts[0]), month: toInteger(dateParts[1]), day: toInteger(dateParts[2]) };
            }
        }
        return null;
    }
    /**
     * @param {?} date
     * @return {?}
     */
    format(date) {
        if (date && this.datePipe) {
            return this.datePipe.transform(new Date(date.year, date.month, date.day), 'shortDate');
        }
        else {
            return date
                ? `${date.year}-${isNumber(date.month) ? padNumber(date.month) : ''}-${isNumber(date.day) ? padNumber(date.day) : ''}`
                : '';
        }
    }
}
DateParserFormatter.decorators = [
    { type: Injectable }
];
/** @nocollapse */
DateParserFormatter.ctorParameters = () => [
    { type: DatePipe, decorators: [{ type: Optional }] }
];
if (false) {
    /**
     * @type {?}
     * @private
     */
    DateParserFormatter.prototype.datePipe;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGF0ZS1wYXJzZXItZm9ybWF0dGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvZGF0ZS1wYXJzZXItZm9ybWF0dGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDckQsT0FBTyxFQUFFLHNCQUFzQixFQUFpQixNQUFNLDRCQUE0QixDQUFDO0FBQ25GLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQzs7Ozs7QUFFM0MsU0FBUyxTQUFTLENBQUMsS0FBYTtJQUM5QixJQUFJLFFBQVEsQ0FBQyxLQUFLLENBQUMsRUFBRTtRQUNuQixPQUFPLElBQUksS0FBSyxFQUFFLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7S0FDOUI7U0FBTTtRQUNMLE9BQU8sRUFBRSxDQUFDO0tBQ1g7QUFDSCxDQUFDOzs7OztBQUVELFNBQVMsUUFBUSxDQUFDLEtBQVU7SUFDMUIsT0FBTyxDQUFDLEtBQUssQ0FBQyxTQUFTLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQztBQUNsQyxDQUFDOzs7OztBQUVELFNBQVMsU0FBUyxDQUFDLEtBQVU7SUFDM0IsT0FBTyxRQUFRLENBQUMsR0FBRyxLQUFLLEVBQUUsRUFBRSxFQUFFLENBQUMsQ0FBQztBQUNsQyxDQUFDO0FBR0QsTUFBTSxPQUFPLG1CQUFvQixTQUFRLHNCQUFzQjs7OztJQUM3RCxZQUFnQyxRQUFrQjtRQUNoRCxLQUFLLEVBQUUsQ0FBQztRQURzQixhQUFRLEdBQVIsUUFBUSxDQUFVO0lBRWxELENBQUM7Ozs7O0lBRUQsS0FBSyxDQUFDLEtBQWE7UUFDakIsSUFBSSxLQUFLLEVBQUU7O2tCQUNILFNBQVMsR0FBRyxLQUFLLENBQUMsSUFBSSxFQUFFLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQztZQUN6QyxJQUFJLFNBQVMsQ0FBQyxNQUFNLEtBQUssQ0FBQyxJQUFJLFFBQVEsQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBRTtnQkFDcEQsT0FBTyxFQUFFLElBQUksRUFBRSxTQUFTLENBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUUsS0FBSyxFQUFFLElBQUksRUFBRSxHQUFHLEVBQUUsSUFBSSxFQUFFLENBQUM7YUFDbEU7aUJBQU0sSUFBSSxTQUFTLENBQUMsTUFBTSxLQUFLLENBQUMsSUFBSSxRQUFRLENBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksUUFBUSxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFFO2dCQUNyRixPQUFPLEVBQUUsSUFBSSxFQUFFLFNBQVMsQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBRSxLQUFLLEVBQUUsU0FBUyxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFFLEdBQUcsRUFBRSxJQUFJLEVBQUUsQ0FBQzthQUNyRjtpQkFBTSxJQUFJLFNBQVMsQ0FBQyxNQUFNLEtBQUssQ0FBQyxJQUFJLFFBQVEsQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxRQUFRLENBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksUUFBUSxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFFO2dCQUMvRyxPQUFPLEVBQUUsSUFBSSxFQUFFLFNBQVMsQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBRSxLQUFLLEVBQUUsU0FBUyxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFFLEdBQUcsRUFBRSxTQUFTLENBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQzthQUN4RztTQUNGO1FBQ0QsT0FBTyxJQUFJLENBQUM7SUFDZCxDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxJQUFtQjtRQUN4QixJQUFJLElBQUksSUFBSSxJQUFJLENBQUMsUUFBUSxFQUFFO1lBQ3pCLE9BQU8sSUFBSSxDQUFDLFFBQVEsQ0FBQyxTQUFTLENBQUMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLElBQUksRUFBRSxJQUFJLENBQUMsS0FBSyxFQUFFLElBQUksQ0FBQyxHQUFHLENBQUMsRUFBRSxXQUFXLENBQUMsQ0FBQztTQUN4RjthQUFNO1lBQ0wsT0FBTyxJQUFJO2dCQUNULENBQUMsQ0FBQyxHQUFHLElBQUksQ0FBQyxJQUFJLElBQUksUUFBUSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLENBQUMsU0FBUyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBRSxJQUMvRCxRQUFRLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxTQUFTLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUM3QyxFQUFFO2dCQUNKLENBQUMsQ0FBQyxFQUFFLENBQUM7U0FDUjtJQUNILENBQUM7OztZQTlCRixVQUFVOzs7O1lBbEJGLFFBQVEsdUJBb0JGLFFBQVE7Ozs7Ozs7SUFBVCx1Q0FBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlLCBPcHRpb25hbCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiRGF0ZVBhcnNlckZvcm1hdHRlciwgTmdiRGF0ZVN0cnVjdCB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcbmltcG9ydCB7IERhdGVQaXBlIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uJztcblxuZnVuY3Rpb24gcGFkTnVtYmVyKHZhbHVlOiBudW1iZXIpIHtcbiAgaWYgKGlzTnVtYmVyKHZhbHVlKSkge1xuICAgIHJldHVybiBgMCR7dmFsdWV9YC5zbGljZSgtMik7XG4gIH0gZWxzZSB7XG4gICAgcmV0dXJuICcnO1xuICB9XG59XG5cbmZ1bmN0aW9uIGlzTnVtYmVyKHZhbHVlOiBhbnkpOiBib29sZWFuIHtcbiAgcmV0dXJuICFpc05hTih0b0ludGVnZXIodmFsdWUpKTtcbn1cblxuZnVuY3Rpb24gdG9JbnRlZ2VyKHZhbHVlOiBhbnkpOiBudW1iZXIge1xuICByZXR1cm4gcGFyc2VJbnQoYCR7dmFsdWV9YCwgMTApO1xufVxuXG5ASW5qZWN0YWJsZSgpXG5leHBvcnQgY2xhc3MgRGF0ZVBhcnNlckZvcm1hdHRlciBleHRlbmRzIE5nYkRhdGVQYXJzZXJGb3JtYXR0ZXIge1xuICBjb25zdHJ1Y3RvcihAT3B0aW9uYWwoKSBwcml2YXRlIGRhdGVQaXBlOiBEYXRlUGlwZSkge1xuICAgIHN1cGVyKCk7XG4gIH1cblxuICBwYXJzZSh2YWx1ZTogc3RyaW5nKTogTmdiRGF0ZVN0cnVjdCB7XG4gICAgaWYgKHZhbHVlKSB7XG4gICAgICBjb25zdCBkYXRlUGFydHMgPSB2YWx1ZS50cmltKCkuc3BsaXQoJy0nKTtcbiAgICAgIGlmIChkYXRlUGFydHMubGVuZ3RoID09PSAxICYmIGlzTnVtYmVyKGRhdGVQYXJ0c1swXSkpIHtcbiAgICAgICAgcmV0dXJuIHsgeWVhcjogdG9JbnRlZ2VyKGRhdGVQYXJ0c1swXSksIG1vbnRoOiBudWxsLCBkYXk6IG51bGwgfTtcbiAgICAgIH0gZWxzZSBpZiAoZGF0ZVBhcnRzLmxlbmd0aCA9PT0gMiAmJiBpc051bWJlcihkYXRlUGFydHNbMF0pICYmIGlzTnVtYmVyKGRhdGVQYXJ0c1sxXSkpIHtcbiAgICAgICAgcmV0dXJuIHsgeWVhcjogdG9JbnRlZ2VyKGRhdGVQYXJ0c1swXSksIG1vbnRoOiB0b0ludGVnZXIoZGF0ZVBhcnRzWzFdKSwgZGF5OiBudWxsIH07XG4gICAgICB9IGVsc2UgaWYgKGRhdGVQYXJ0cy5sZW5ndGggPT09IDMgJiYgaXNOdW1iZXIoZGF0ZVBhcnRzWzBdKSAmJiBpc051bWJlcihkYXRlUGFydHNbMV0pICYmIGlzTnVtYmVyKGRhdGVQYXJ0c1syXSkpIHtcbiAgICAgICAgcmV0dXJuIHsgeWVhcjogdG9JbnRlZ2VyKGRhdGVQYXJ0c1swXSksIG1vbnRoOiB0b0ludGVnZXIoZGF0ZVBhcnRzWzFdKSwgZGF5OiB0b0ludGVnZXIoZGF0ZVBhcnRzWzJdKSB9O1xuICAgICAgfVxuICAgIH1cbiAgICByZXR1cm4gbnVsbDtcbiAgfVxuXG4gIGZvcm1hdChkYXRlOiBOZ2JEYXRlU3RydWN0KTogc3RyaW5nIHtcbiAgICBpZiAoZGF0ZSAmJiB0aGlzLmRhdGVQaXBlKSB7XG4gICAgICByZXR1cm4gdGhpcy5kYXRlUGlwZS50cmFuc2Zvcm0obmV3IERhdGUoZGF0ZS55ZWFyLCBkYXRlLm1vbnRoLCBkYXRlLmRheSksICdzaG9ydERhdGUnKTtcbiAgICB9IGVsc2Uge1xuICAgICAgcmV0dXJuIGRhdGVcbiAgICAgICAgPyBgJHtkYXRlLnllYXJ9LSR7aXNOdW1iZXIoZGF0ZS5tb250aCkgPyBwYWROdW1iZXIoZGF0ZS5tb250aCkgOiAnJ30tJHtcbiAgICAgICAgICAgIGlzTnVtYmVyKGRhdGUuZGF5KSA/IHBhZE51bWJlcihkYXRlLmRheSkgOiAnJ1xuICAgICAgICAgIH1gXG4gICAgICAgIDogJyc7XG4gICAgfVxuICB9XG59XG4iXX0=