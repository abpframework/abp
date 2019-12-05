/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/validation-error/validation-error.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { ValidationErrorComponent as ErrorComponent } from '@ngx-validate/core';
var ValidationErrorComponent = /** @class */ (function (_super) {
    tslib_1.__extends(ValidationErrorComponent, _super);
    function ValidationErrorComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Object.defineProperty(ValidationErrorComponent.prototype, "abpErrors", {
        get: /**
         * @return {?}
         */
        function () {
            if (!this.errors || !this.errors.length)
                return [];
            return this.errors.map((/**
             * @param {?} error
             * @return {?}
             */
            function (error) {
                if (!error.message)
                    return error;
                /** @type {?} */
                var index = error.message.indexOf('[');
                if (index > -1) {
                    return tslib_1.__assign({}, error, { message: error.message.slice(0, index), interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(',') });
                }
                return error;
            }));
        },
        enumerable: true,
        configurable: true
    });
    ValidationErrorComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-validation-error',
                    template: "\n    <div class=\"invalid-feedback\" *ngFor=\"let error of abpErrors; trackBy: trackByFn\">\n      {{ error.message | abpLocalization: error.interpoliteParams }}\n    </div>\n  ",
                    changeDetection: ChangeDetectionStrategy.OnPush,
                    encapsulation: ViewEncapsulation.None
                }] }
    ];
    return ValidationErrorComponent;
}(ErrorComponent));
export { ValidationErrorComponent };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdmFsaWRhdGlvbi1lcnJvci92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsU0FBUyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3RGLE9BQU8sRUFBYyx3QkFBd0IsSUFBSSxjQUFjLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUU1RjtJQVU4QyxvREFBYztJQVY1RDs7SUE4QkEsQ0FBQztJQW5CQyxzQkFBSSwrQ0FBUzs7OztRQUFiO1lBQ0UsSUFBSSxDQUFDLElBQUksQ0FBQyxNQUFNLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07Z0JBQUUsT0FBTyxFQUFFLENBQUM7WUFFbkQsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUc7Ozs7WUFBQyxVQUFBLEtBQUs7Z0JBQzFCLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTztvQkFBRSxPQUFPLEtBQUssQ0FBQzs7b0JBRTNCLEtBQUssR0FBRyxLQUFLLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUM7Z0JBRXhDLElBQUksS0FBSyxHQUFHLENBQUMsQ0FBQyxFQUFFO29CQUNkLDRCQUNLLEtBQUssSUFDUixPQUFPLEVBQUUsS0FBSyxDQUFDLE9BQU8sQ0FBQyxLQUFLLENBQUMsQ0FBQyxFQUFFLEtBQUssQ0FBQyxFQUN0QyxpQkFBaUIsRUFBRSxLQUFLLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQyxLQUFLLEdBQUcsQ0FBQyxFQUFFLEtBQUssQ0FBQyxPQUFPLENBQUMsTUFBTSxHQUFHLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsSUFDdEY7aUJBQ0g7Z0JBRUQsT0FBTyxLQUFLLENBQUM7WUFDZixDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUM7OztPQUFBOztnQkE3QkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxzQkFBc0I7b0JBQ2hDLFFBQVEsRUFBRSxvTEFJVDtvQkFDRCxlQUFlLEVBQUUsdUJBQXVCLENBQUMsTUFBTTtvQkFDL0MsYUFBYSxFQUFFLGlCQUFpQixDQUFDLElBQUk7aUJBQ3RDOztJQXFCRCwrQkFBQztDQUFBLEFBOUJELENBVThDLGNBQWMsR0FvQjNEO1NBcEJZLHdCQUF3QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENoYW5nZURldGVjdGlvblN0cmF0ZWd5LCBDb21wb25lbnQsIFZpZXdFbmNhcHN1bGF0aW9uIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBWYWxpZGF0aW9uLCBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQgYXMgRXJyb3JDb21wb25lbnQgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdmFsaWRhdGlvbi1lcnJvcicsXG4gIHRlbXBsYXRlOiBgXG4gICAgPGRpdiBjbGFzcz1cImludmFsaWQtZmVlZGJhY2tcIiAqbmdGb3I9XCJsZXQgZXJyb3Igb2YgYWJwRXJyb3JzOyB0cmFja0J5OiB0cmFja0J5Rm5cIj5cbiAgICAgIHt7IGVycm9yLm1lc3NhZ2UgfCBhYnBMb2NhbGl6YXRpb246IGVycm9yLmludGVycG9saXRlUGFyYW1zIH19XG4gICAgPC9kaXY+XG4gIGAsXG4gIGNoYW5nZURldGVjdGlvbjogQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3kuT25QdXNoLFxuICBlbmNhcHN1bGF0aW9uOiBWaWV3RW5jYXBzdWxhdGlvbi5Ob25lLFxufSlcbmV4cG9ydCBjbGFzcyBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQgZXh0ZW5kcyBFcnJvckNvbXBvbmVudCB7XG4gIGdldCBhYnBFcnJvcnMoKTogVmFsaWRhdGlvbi5FcnJvcltdICYgeyBpbnRlcnBvbGl0ZVBhcmFtcz86IHN0cmluZ1tdIH0ge1xuICAgIGlmICghdGhpcy5lcnJvcnMgfHwgIXRoaXMuZXJyb3JzLmxlbmd0aCkgcmV0dXJuIFtdO1xuXG4gICAgcmV0dXJuIHRoaXMuZXJyb3JzLm1hcChlcnJvciA9PiB7XG4gICAgICBpZiAoIWVycm9yLm1lc3NhZ2UpIHJldHVybiBlcnJvcjtcblxuICAgICAgY29uc3QgaW5kZXggPSBlcnJvci5tZXNzYWdlLmluZGV4T2YoJ1snKTtcblxuICAgICAgaWYgKGluZGV4ID4gLTEpIHtcbiAgICAgICAgcmV0dXJuIHtcbiAgICAgICAgICAuLi5lcnJvcixcbiAgICAgICAgICBtZXNzYWdlOiBlcnJvci5tZXNzYWdlLnNsaWNlKDAsIGluZGV4KSxcbiAgICAgICAgICBpbnRlcnBvbGl0ZVBhcmFtczogZXJyb3IubWVzc2FnZS5zbGljZShpbmRleCArIDEsIGVycm9yLm1lc3NhZ2UubGVuZ3RoIC0gMSkuc3BsaXQoJywnKSxcbiAgICAgICAgfTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIGVycm9yO1xuICAgIH0pO1xuICB9XG59XG4iXX0=