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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdmFsaWRhdGlvbi1lcnJvci92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsU0FBUyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3RGLE9BQU8sRUFBYyx3QkFBd0IsSUFBSSxjQUFjLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUU1RjtJQVU4QyxvREFBYztJQVY1RDs7SUE4QkEsQ0FBQztJQW5CQyxzQkFBSSwrQ0FBUzs7OztRQUFiO1lBQ0UsSUFBSSxDQUFDLElBQUksQ0FBQyxNQUFNLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07Z0JBQUUsT0FBTyxFQUFFLENBQUM7WUFFbkQsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUc7Ozs7WUFBQyxVQUFBLEtBQUs7Z0JBQzFCLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTztvQkFBRSxPQUFPLEtBQUssQ0FBQzs7b0JBRTNCLEtBQUssR0FBRyxLQUFLLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUM7Z0JBRXhDLElBQUksS0FBSyxHQUFHLENBQUMsQ0FBQyxFQUFFO29CQUNkLDRCQUNLLEtBQUssSUFDUixPQUFPLEVBQUUsS0FBSyxDQUFDLE9BQU8sQ0FBQyxLQUFLLENBQUMsQ0FBQyxFQUFFLEtBQUssQ0FBQyxFQUN0QyxpQkFBaUIsRUFBRSxLQUFLLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQyxLQUFLLEdBQUcsQ0FBQyxFQUFFLEtBQUssQ0FBQyxPQUFPLENBQUMsTUFBTSxHQUFHLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsSUFDdEY7aUJBQ0g7Z0JBRUQsT0FBTyxLQUFLLENBQUM7WUFDZixDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUM7OztPQUFBOztnQkE3QkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxzQkFBc0I7b0JBQ2hDLFFBQVEsRUFBRSxvTEFJVDtvQkFDRCxlQUFlLEVBQUUsdUJBQXVCLENBQUMsTUFBTTtvQkFDL0MsYUFBYSxFQUFFLGlCQUFpQixDQUFDLElBQUk7aUJBQ3RDOztJQXFCRCwrQkFBQztDQUFBLEFBOUJELENBVThDLGNBQWMsR0FvQjNEO1NBcEJZLHdCQUF3QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENoYW5nZURldGVjdGlvblN0cmF0ZWd5LCBDb21wb25lbnQsIFZpZXdFbmNhcHN1bGF0aW9uIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFZhbGlkYXRpb24sIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCBhcyBFcnJvckNvbXBvbmVudCB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC12YWxpZGF0aW9uLWVycm9yJyxcclxuICB0ZW1wbGF0ZTogYFxyXG4gICAgPGRpdiBjbGFzcz1cImludmFsaWQtZmVlZGJhY2tcIiAqbmdGb3I9XCJsZXQgZXJyb3Igb2YgYWJwRXJyb3JzOyB0cmFja0J5OiB0cmFja0J5Rm5cIj5cclxuICAgICAge3sgZXJyb3IubWVzc2FnZSB8IGFicExvY2FsaXphdGlvbjogZXJyb3IuaW50ZXJwb2xpdGVQYXJhbXMgfX1cclxuICAgIDwvZGl2PlxyXG4gIGAsXHJcbiAgY2hhbmdlRGV0ZWN0aW9uOiBDaGFuZ2VEZXRlY3Rpb25TdHJhdGVneS5PblB1c2gsXHJcbiAgZW5jYXBzdWxhdGlvbjogVmlld0VuY2Fwc3VsYXRpb24uTm9uZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCBleHRlbmRzIEVycm9yQ29tcG9uZW50IHtcclxuICBnZXQgYWJwRXJyb3JzKCk6IFZhbGlkYXRpb24uRXJyb3JbXSAmIHsgaW50ZXJwb2xpdGVQYXJhbXM/OiBzdHJpbmdbXSB9IHtcclxuICAgIGlmICghdGhpcy5lcnJvcnMgfHwgIXRoaXMuZXJyb3JzLmxlbmd0aCkgcmV0dXJuIFtdO1xyXG5cclxuICAgIHJldHVybiB0aGlzLmVycm9ycy5tYXAoZXJyb3IgPT4ge1xyXG4gICAgICBpZiAoIWVycm9yLm1lc3NhZ2UpIHJldHVybiBlcnJvcjtcclxuXHJcbiAgICAgIGNvbnN0IGluZGV4ID0gZXJyb3IubWVzc2FnZS5pbmRleE9mKCdbJyk7XHJcblxyXG4gICAgICBpZiAoaW5kZXggPiAtMSkge1xyXG4gICAgICAgIHJldHVybiB7XHJcbiAgICAgICAgICAuLi5lcnJvcixcclxuICAgICAgICAgIG1lc3NhZ2U6IGVycm9yLm1lc3NhZ2Uuc2xpY2UoMCwgaW5kZXgpLFxyXG4gICAgICAgICAgaW50ZXJwb2xpdGVQYXJhbXM6IGVycm9yLm1lc3NhZ2Uuc2xpY2UoaW5kZXggKyAxLCBlcnJvci5tZXNzYWdlLmxlbmd0aCAtIDEpLnNwbGl0KCcsJyksXHJcbiAgICAgICAgfTtcclxuICAgICAgfVxyXG5cclxuICAgICAgcmV0dXJuIGVycm9yO1xyXG4gICAgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==