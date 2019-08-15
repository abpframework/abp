/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
            return this.validationErrors.map((/**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2Vycm9ycy92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxTQUFTLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDdEYsT0FBTyxFQUFjLHdCQUF3QixJQUFJLGNBQWMsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBRTVGO0lBVThDLG9EQUFjO0lBVjVEOztJQTRCQSxDQUFDO0lBakJDLHNCQUFJLCtDQUFTOzs7O1FBQWI7WUFDRSxPQUFPLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxHQUFHOzs7O1lBQUMsVUFBQSxLQUFLO2dCQUNwQyxJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU87b0JBQUUsT0FBTyxLQUFLLENBQUM7O29CQUUzQixLQUFLLEdBQUcsS0FBSyxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDO2dCQUV4QyxJQUFJLEtBQUssR0FBRyxDQUFDLENBQUMsRUFBRTtvQkFDZCw0QkFDSyxLQUFLLElBQ1IsT0FBTyxFQUFFLEtBQUssQ0FBQyxPQUFPLENBQUMsS0FBSyxDQUFDLENBQUMsRUFBRSxLQUFLLENBQUMsRUFDdEMsaUJBQWlCLEVBQUUsS0FBSyxDQUFDLE9BQU8sQ0FBQyxLQUFLLENBQUMsS0FBSyxHQUFHLENBQUMsRUFBRSxLQUFLLENBQUMsT0FBTyxDQUFDLE1BQU0sR0FBRyxDQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLElBQ3RGO2lCQUNIO2dCQUVELE9BQU8sS0FBSyxDQUFDO1lBQ2YsQ0FBQyxFQUFDLENBQUM7UUFDTCxDQUFDOzs7T0FBQTs7Z0JBM0JGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsc0JBQXNCO29CQUNoQyxRQUFRLEVBQUUsb0xBSVQ7b0JBQ0QsZUFBZSxFQUFFLHVCQUF1QixDQUFDLE1BQU07b0JBQy9DLGFBQWEsRUFBRSxpQkFBaUIsQ0FBQyxJQUFJO2lCQUN0Qzs7SUFtQkQsK0JBQUM7Q0FBQSxBQTVCRCxDQVU4QyxjQUFjLEdBa0IzRDtTQWxCWSx3QkFBd0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDaGFuZ2VEZXRlY3Rpb25TdHJhdGVneSwgQ29tcG9uZW50LCBWaWV3RW5jYXBzdWxhdGlvbiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgVmFsaWRhdGlvbiwgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50IGFzIEVycm9yQ29tcG9uZW50IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXZhbGlkYXRpb24tZXJyb3InLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxkaXYgY2xhc3M9XCJpbnZhbGlkLWZlZWRiYWNrXCIgKm5nRm9yPVwibGV0IGVycm9yIG9mIGFicEVycm9yczsgdHJhY2tCeTogdHJhY2tCeUZuXCI+XG4gICAgICB7eyBlcnJvci5tZXNzYWdlIHwgYWJwTG9jYWxpemF0aW9uOiBlcnJvci5pbnRlcnBvbGl0ZVBhcmFtcyB9fVxuICAgIDwvZGl2PlxuICBgLFxuICBjaGFuZ2VEZXRlY3Rpb246IENoYW5nZURldGVjdGlvblN0cmF0ZWd5Lk9uUHVzaCxcbiAgZW5jYXBzdWxhdGlvbjogVmlld0VuY2Fwc3VsYXRpb24uTm9uZSxcbn0pXG5leHBvcnQgY2xhc3MgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50IGV4dGVuZHMgRXJyb3JDb21wb25lbnQge1xuICBnZXQgYWJwRXJyb3JzKCk6IFZhbGlkYXRpb24uRXJyb3JbXSAmIHsgaW50ZXJwb2xpdGVQYXJhbXM/OiBzdHJpbmdbXSB9IHtcbiAgICByZXR1cm4gdGhpcy52YWxpZGF0aW9uRXJyb3JzLm1hcChlcnJvciA9PiB7XG4gICAgICBpZiAoIWVycm9yLm1lc3NhZ2UpIHJldHVybiBlcnJvcjtcblxuICAgICAgY29uc3QgaW5kZXggPSBlcnJvci5tZXNzYWdlLmluZGV4T2YoJ1snKTtcblxuICAgICAgaWYgKGluZGV4ID4gLTEpIHtcbiAgICAgICAgcmV0dXJuIHtcbiAgICAgICAgICAuLi5lcnJvcixcbiAgICAgICAgICBtZXNzYWdlOiBlcnJvci5tZXNzYWdlLnNsaWNlKDAsIGluZGV4KSxcbiAgICAgICAgICBpbnRlcnBvbGl0ZVBhcmFtczogZXJyb3IubWVzc2FnZS5zbGljZShpbmRleCArIDEsIGVycm9yLm1lc3NhZ2UubGVuZ3RoIC0gMSkuc3BsaXQoJywnKSxcbiAgICAgICAgfTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIGVycm9yO1xuICAgIH0pO1xuICB9XG59XG4iXX0=