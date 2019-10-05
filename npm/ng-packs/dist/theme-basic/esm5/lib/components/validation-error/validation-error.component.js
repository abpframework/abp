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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdmFsaWRhdGlvbi1lcnJvci92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxTQUFTLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDdEYsT0FBTyxFQUFjLHdCQUF3QixJQUFJLGNBQWMsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBRTVGO0lBVThDLG9EQUFjO0lBVjVEOztJQThCQSxDQUFDO0lBbkJDLHNCQUFJLCtDQUFTOzs7O1FBQWI7WUFDRSxJQUFJLENBQUMsSUFBSSxDQUFDLE1BQU0sSUFBSSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTTtnQkFBRSxPQUFPLEVBQUUsQ0FBQztZQUVuRCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRzs7OztZQUFDLFVBQUEsS0FBSztnQkFDMUIsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPO29CQUFFLE9BQU8sS0FBSyxDQUFDOztvQkFFM0IsS0FBSyxHQUFHLEtBQUssQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQztnQkFFeEMsSUFBSSxLQUFLLEdBQUcsQ0FBQyxDQUFDLEVBQUU7b0JBQ2QsNEJBQ0ssS0FBSyxJQUNSLE9BQU8sRUFBRSxLQUFLLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQyxDQUFDLEVBQUUsS0FBSyxDQUFDLEVBQ3RDLGlCQUFpQixFQUFFLEtBQUssQ0FBQyxPQUFPLENBQUMsS0FBSyxDQUFDLEtBQUssR0FBRyxDQUFDLEVBQUUsS0FBSyxDQUFDLE9BQU8sQ0FBQyxNQUFNLEdBQUcsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxJQUN0RjtpQkFDSDtnQkFFRCxPQUFPLEtBQUssQ0FBQztZQUNmLENBQUMsRUFBQyxDQUFDO1FBQ0wsQ0FBQzs7O09BQUE7O2dCQTdCRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLHNCQUFzQjtvQkFDaEMsUUFBUSxFQUFFLG9MQUlUO29CQUNELGVBQWUsRUFBRSx1QkFBdUIsQ0FBQyxNQUFNO29CQUMvQyxhQUFhLEVBQUUsaUJBQWlCLENBQUMsSUFBSTtpQkFDdEM7O0lBcUJELCtCQUFDO0NBQUEsQUE5QkQsQ0FVOEMsY0FBYyxHQW9CM0Q7U0FwQlksd0JBQXdCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3ksIENvbXBvbmVudCwgVmlld0VuY2Fwc3VsYXRpb24gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFZhbGlkYXRpb24sIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCBhcyBFcnJvckNvbXBvbmVudCB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC12YWxpZGF0aW9uLWVycm9yJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8ZGl2IGNsYXNzPVwiaW52YWxpZC1mZWVkYmFja1wiICpuZ0Zvcj1cImxldCBlcnJvciBvZiBhYnBFcnJvcnM7IHRyYWNrQnk6IHRyYWNrQnlGblwiPlxuICAgICAge3sgZXJyb3IubWVzc2FnZSB8IGFicExvY2FsaXphdGlvbjogZXJyb3IuaW50ZXJwb2xpdGVQYXJhbXMgfX1cbiAgICA8L2Rpdj5cbiAgYCxcbiAgY2hhbmdlRGV0ZWN0aW9uOiBDaGFuZ2VEZXRlY3Rpb25TdHJhdGVneS5PblB1c2gsXG4gIGVuY2Fwc3VsYXRpb246IFZpZXdFbmNhcHN1bGF0aW9uLk5vbmUsXG59KVxuZXhwb3J0IGNsYXNzIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCBleHRlbmRzIEVycm9yQ29tcG9uZW50IHtcbiAgZ2V0IGFicEVycm9ycygpOiBWYWxpZGF0aW9uLkVycm9yW10gJiB7IGludGVycG9saXRlUGFyYW1zPzogc3RyaW5nW10gfSB7XG4gICAgaWYgKCF0aGlzLmVycm9ycyB8fCAhdGhpcy5lcnJvcnMubGVuZ3RoKSByZXR1cm4gW107XG5cbiAgICByZXR1cm4gdGhpcy5lcnJvcnMubWFwKGVycm9yID0+IHtcbiAgICAgIGlmICghZXJyb3IubWVzc2FnZSkgcmV0dXJuIGVycm9yO1xuXG4gICAgICBjb25zdCBpbmRleCA9IGVycm9yLm1lc3NhZ2UuaW5kZXhPZignWycpO1xuXG4gICAgICBpZiAoaW5kZXggPiAtMSkge1xuICAgICAgICByZXR1cm4ge1xuICAgICAgICAgIC4uLmVycm9yLFxuICAgICAgICAgIG1lc3NhZ2U6IGVycm9yLm1lc3NhZ2Uuc2xpY2UoMCwgaW5kZXgpLFxuICAgICAgICAgIGludGVycG9saXRlUGFyYW1zOiBlcnJvci5tZXNzYWdlLnNsaWNlKGluZGV4ICsgMSwgZXJyb3IubWVzc2FnZS5sZW5ndGggLSAxKS5zcGxpdCgnLCcpLFxuICAgICAgICB9O1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gZXJyb3I7XG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==