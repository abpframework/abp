/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdmFsaWRhdGlvbi1lcnJvci92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxTQUFTLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDdEYsT0FBTyxFQUFjLHdCQUF3QixJQUFJLGNBQWMsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBRTVGO0lBVThDLG9EQUFjO0lBVjVEOztJQThCQSxDQUFDO0lBbkJDLHNCQUFJLCtDQUFTOzs7O1FBQWI7WUFDRSxJQUFJLENBQUMsSUFBSSxDQUFDLE1BQU0sSUFBSSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTTtnQkFBRSxPQUFPLEVBQUUsQ0FBQztZQUVuRCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRzs7OztZQUFDLFVBQUEsS0FBSztnQkFDMUIsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPO29CQUFFLE9BQU8sS0FBSyxDQUFDOztvQkFFM0IsS0FBSyxHQUFHLEtBQUssQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQztnQkFFeEMsSUFBSSxLQUFLLEdBQUcsQ0FBQyxDQUFDLEVBQUU7b0JBQ2QsNEJBQ0ssS0FBSyxJQUNSLE9BQU8sRUFBRSxLQUFLLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQyxDQUFDLEVBQUUsS0FBSyxDQUFDLEVBQ3RDLGlCQUFpQixFQUFFLEtBQUssQ0FBQyxPQUFPLENBQUMsS0FBSyxDQUFDLEtBQUssR0FBRyxDQUFDLEVBQUUsS0FBSyxDQUFDLE9BQU8sQ0FBQyxNQUFNLEdBQUcsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxJQUN0RjtpQkFDSDtnQkFFRCxPQUFPLEtBQUssQ0FBQztZQUNmLENBQUMsRUFBQyxDQUFDO1FBQ0wsQ0FBQzs7O09BQUE7O2dCQTdCRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLHNCQUFzQjtvQkFDaEMsUUFBUSxFQUFFLG9MQUlUO29CQUNELGVBQWUsRUFBRSx1QkFBdUIsQ0FBQyxNQUFNO29CQUMvQyxhQUFhLEVBQUUsaUJBQWlCLENBQUMsSUFBSTtpQkFDdEM7O0lBcUJELCtCQUFDO0NBQUEsQUE5QkQsQ0FVOEMsY0FBYyxHQW9CM0Q7U0FwQlksd0JBQXdCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3ksIENvbXBvbmVudCwgVmlld0VuY2Fwc3VsYXRpb24gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgVmFsaWRhdGlvbiwgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50IGFzIEVycm9yQ29tcG9uZW50IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXZhbGlkYXRpb24tZXJyb3InLFxyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8ZGl2IGNsYXNzPVwiaW52YWxpZC1mZWVkYmFja1wiICpuZ0Zvcj1cImxldCBlcnJvciBvZiBhYnBFcnJvcnM7IHRyYWNrQnk6IHRyYWNrQnlGblwiPlxyXG4gICAgICB7eyBlcnJvci5tZXNzYWdlIHwgYWJwTG9jYWxpemF0aW9uOiBlcnJvci5pbnRlcnBvbGl0ZVBhcmFtcyB9fVxyXG4gICAgPC9kaXY+XHJcbiAgYCxcclxuICBjaGFuZ2VEZXRlY3Rpb246IENoYW5nZURldGVjdGlvblN0cmF0ZWd5Lk9uUHVzaCxcclxuICBlbmNhcHN1bGF0aW9uOiBWaWV3RW5jYXBzdWxhdGlvbi5Ob25lLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50IGV4dGVuZHMgRXJyb3JDb21wb25lbnQge1xyXG4gIGdldCBhYnBFcnJvcnMoKTogVmFsaWRhdGlvbi5FcnJvcltdICYgeyBpbnRlcnBvbGl0ZVBhcmFtcz86IHN0cmluZ1tdIH0ge1xyXG4gICAgaWYgKCF0aGlzLmVycm9ycyB8fCAhdGhpcy5lcnJvcnMubGVuZ3RoKSByZXR1cm4gW107XHJcblxyXG4gICAgcmV0dXJuIHRoaXMuZXJyb3JzLm1hcChlcnJvciA9PiB7XHJcbiAgICAgIGlmICghZXJyb3IubWVzc2FnZSkgcmV0dXJuIGVycm9yO1xyXG5cclxuICAgICAgY29uc3QgaW5kZXggPSBlcnJvci5tZXNzYWdlLmluZGV4T2YoJ1snKTtcclxuXHJcbiAgICAgIGlmIChpbmRleCA+IC0xKSB7XHJcbiAgICAgICAgcmV0dXJuIHtcclxuICAgICAgICAgIC4uLmVycm9yLFxyXG4gICAgICAgICAgbWVzc2FnZTogZXJyb3IubWVzc2FnZS5zbGljZSgwLCBpbmRleCksXHJcbiAgICAgICAgICBpbnRlcnBvbGl0ZVBhcmFtczogZXJyb3IubWVzc2FnZS5zbGljZShpbmRleCArIDEsIGVycm9yLm1lc3NhZ2UubGVuZ3RoIC0gMSkuc3BsaXQoJywnKSxcclxuICAgICAgICB9O1xyXG4gICAgICB9XHJcblxyXG4gICAgICByZXR1cm4gZXJyb3I7XHJcbiAgICB9KTtcclxuICB9XHJcbn1cclxuIl19