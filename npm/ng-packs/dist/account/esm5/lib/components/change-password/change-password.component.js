/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangePassword } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { comparePasswords } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import snq from 'snq';
var minLength = Validators.minLength, required = Validators.required;
/** @type {?} */
var PASSWORD_FIELDS = ['newPassword', 'repeatNewPassword'];
var ChangePasswordComponent = /** @class */ (function () {
    function ChangePasswordComponent(fb, store, toasterService) {
        this.fb = fb;
        this.store = store;
        this.toasterService = toasterService;
        this.mapErrorsFn = (/**
         * @param {?} errors
         * @param {?} groupErrors
         * @param {?} control
         * @return {?}
         */
        function (errors, groupErrors, control) {
            if (PASSWORD_FIELDS.indexOf(control.name) < 0)
                return errors;
            return errors.concat(groupErrors.filter((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var key = _a.key;
                return key === 'passwordMismatch';
            })));
        });
    }
    /**
     * @return {?}
     */
    ChangePasswordComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        this.form = this.fb.group({
            password: ['', required],
            newPassword: ['', required],
            repeatNewPassword: ['', required],
        }, {
            validators: [comparePasswords(PASSWORD_FIELDS)],
        });
    };
    /**
     * @return {?}
     */
    ChangePasswordComponent.prototype.onSubmit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.form.invalid)
            return;
        this.store
            .dispatch(new ChangePassword({
            currentPassword: this.form.get('password').value,
            newPassword: this.form.get('newPassword').value,
        }))
            .subscribe({
            next: (/**
             * @return {?}
             */
            function () {
                _this.form.reset();
                _this.toasterService.success('AbpAccount::PasswordChangedMessage', 'Success', { life: 5000 });
            }),
            error: (/**
             * @param {?} err
             * @return {?}
             */
            function (err) {
                _this.toasterService.error(snq((/**
                 * @return {?}
                 */
                function () { return err.error.error.message; }), 'AbpAccount::DefaultErrorMessage'), 'Error', {
                    life: 7000,
                });
            }),
        });
    };
    ChangePasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-change-password-form',
                    template: "<form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" [mapErrorsFn]=\"mapErrorsFn\">\n  <div class=\"form-group\">\n    <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\n    ><span> * </span\n    ><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" autofocus />\n  </div>\n  <div class=\"form-group\">\n    <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\n    ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\n  </div>\n  <div class=\"form-group\">\n    <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\n    ><span> * </span\n    ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\n  </div>\n  <abp-button iconClass=\"fa fa-check\" buttonClass=\"btn btn-primary color-white\" (click)=\"onSubmit()\">{{\n    'AbpIdentity::Save' | abpLocalization\n  }}</abp-button>\n</form>\n"
                }] }
    ];
    /** @nocollapse */
    ChangePasswordComponent.ctorParameters = function () { return [
        { type: FormBuilder },
        { type: Store },
        { type: ToasterService }
    ]; };
    return ChangePasswordComponent;
}());
export { ChangePasswordComponent };
if (false) {
    /** @type {?} */
    ChangePasswordComponent.prototype.form;
    /** @type {?} */
    ChangePasswordComponent.prototype.mapErrorsFn;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.toasterService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2NoYW5nZS1wYXNzd29yZC9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzlDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLGdCQUFnQixFQUFjLE1BQU0sb0JBQW9CLENBQUM7QUFDbEUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFFZCxJQUFBLGdDQUFTLEVBQUUsOEJBQVE7O0lBRXJCLGVBQWUsR0FBRyxDQUFDLGFBQWEsRUFBRSxtQkFBbUIsQ0FBQztBQUU1RDtJQWFFLGlDQUFvQixFQUFlLEVBQVUsS0FBWSxFQUFVLGNBQThCO1FBQTdFLE9BQUUsR0FBRixFQUFFLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBTmpHLGdCQUFXOzs7Ozs7UUFBMkIsVUFBQyxNQUFNLEVBQUUsV0FBVyxFQUFFLE9BQU87WUFDakUsSUFBSSxlQUFlLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDO2dCQUFFLE9BQU8sTUFBTSxDQUFDO1lBRTdELE9BQU8sTUFBTSxDQUFDLE1BQU0sQ0FBQyxXQUFXLENBQUMsTUFBTTs7OztZQUFDLFVBQUMsRUFBTztvQkFBTCxZQUFHO2dCQUFPLE9BQUEsR0FBRyxLQUFLLGtCQUFrQjtZQUExQixDQUEwQixFQUFDLENBQUMsQ0FBQztRQUNwRixDQUFDLEVBQUE7SUFFbUcsQ0FBQzs7OztJQUVyRywwQ0FBUTs7O0lBQVI7UUFDRSxJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUN2QjtZQUNFLFFBQVEsRUFBRSxDQUFDLEVBQUUsRUFBRSxRQUFRLENBQUM7WUFDeEIsV0FBVyxFQUFFLENBQUMsRUFBRSxFQUFFLFFBQVEsQ0FBQztZQUMzQixpQkFBaUIsRUFBRSxDQUFDLEVBQUUsRUFBRSxRQUFRLENBQUM7U0FDbEMsRUFDRDtZQUNFLFVBQVUsRUFBRSxDQUFDLGdCQUFnQixDQUFDLGVBQWUsQ0FBQyxDQUFDO1NBQ2hELENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7SUFFRCwwQ0FBUTs7O0lBQVI7UUFBQSxpQkFxQkM7UUFwQkMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU87WUFBRSxPQUFPO1FBRTlCLElBQUksQ0FBQyxLQUFLO2FBQ1AsUUFBUSxDQUNQLElBQUksY0FBYyxDQUFDO1lBQ2pCLGVBQWUsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsQ0FBQyxLQUFLO1lBQ2hELFdBQVcsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxhQUFhLENBQUMsQ0FBQyxLQUFLO1NBQ2hELENBQUMsQ0FDSDthQUNBLFNBQVMsQ0FBQztZQUNULElBQUk7OztZQUFFO2dCQUNKLEtBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxFQUFFLENBQUM7Z0JBQ2xCLEtBQUksQ0FBQyxjQUFjLENBQUMsT0FBTyxDQUFDLG9DQUFvQyxFQUFFLFNBQVMsRUFBRSxFQUFFLElBQUksRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDO1lBQy9GLENBQUMsQ0FBQTtZQUNELEtBQUs7Ozs7WUFBRSxVQUFBLEdBQUc7Z0JBQ1IsS0FBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsR0FBRzs7O2dCQUFDLGNBQU0sT0FBQSxHQUFHLENBQUMsS0FBSyxDQUFDLEtBQUssQ0FBQyxPQUFPLEVBQXZCLENBQXVCLEdBQUUsaUNBQWlDLENBQUMsRUFBRSxPQUFPLEVBQUU7b0JBQ3hHLElBQUksRUFBRSxJQUFJO2lCQUNYLENBQUMsQ0FBQztZQUNMLENBQUMsQ0FBQTtTQUNGLENBQUMsQ0FBQztJQUNQLENBQUM7O2dCQWpERixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLDBCQUEwQjtvQkFDcEMsdW5DQUErQztpQkFDaEQ7Ozs7Z0JBWlEsV0FBVztnQkFFWCxLQUFLO2dCQUpMLGNBQWM7O0lBNkR2Qiw4QkFBQztDQUFBLEFBbERELElBa0RDO1NBOUNZLHVCQUF1Qjs7O0lBQ2xDLHVDQUFnQjs7SUFFaEIsOENBSUM7Ozs7O0lBRVcscUNBQXVCOzs7OztJQUFFLHdDQUFvQjs7Ozs7SUFBRSxpREFBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDaGFuZ2VQYXNzd29yZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgY29tcGFyZVBhc3N3b3JkcywgVmFsaWRhdGlvbiB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuY29uc3QgeyBtaW5MZW5ndGgsIHJlcXVpcmVkIH0gPSBWYWxpZGF0b3JzO1xuXG5jb25zdCBQQVNTV09SRF9GSUVMRFMgPSBbJ25ld1Bhc3N3b3JkJywgJ3JlcGVhdE5ld1Bhc3N3b3JkJ107XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1jaGFuZ2UtcGFzc3dvcmQtZm9ybScsXG4gIHRlbXBsYXRlVXJsOiAnLi9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIGZvcm06IEZvcm1Hcm91cDtcblxuICBtYXBFcnJvcnNGbjogVmFsaWRhdGlvbi5NYXBFcnJvcnNGbiA9IChlcnJvcnMsIGdyb3VwRXJyb3JzLCBjb250cm9sKSA9PiB7XG4gICAgaWYgKFBBU1NXT1JEX0ZJRUxEUy5pbmRleE9mKGNvbnRyb2wubmFtZSkgPCAwKSByZXR1cm4gZXJyb3JzO1xuXG4gICAgcmV0dXJuIGVycm9ycy5jb25jYXQoZ3JvdXBFcnJvcnMuZmlsdGVyKCh7IGtleSB9KSA9PiBrZXkgPT09ICdwYXNzd29yZE1pc21hdGNoJykpO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIHRvYXN0ZXJTZXJ2aWNlOiBUb2FzdGVyU2VydmljZSkge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKFxuICAgICAge1xuICAgICAgICBwYXNzd29yZDogWycnLCByZXF1aXJlZF0sXG4gICAgICAgIG5ld1Bhc3N3b3JkOiBbJycsIHJlcXVpcmVkXSxcbiAgICAgICAgcmVwZWF0TmV3UGFzc3dvcmQ6IFsnJywgcmVxdWlyZWRdLFxuICAgICAgfSxcbiAgICAgIHtcbiAgICAgICAgdmFsaWRhdG9yczogW2NvbXBhcmVQYXNzd29yZHMoUEFTU1dPUkRfRklFTERTKV0sXG4gICAgICB9LFxuICAgICk7XG4gIH1cblxuICBvblN1Ym1pdCgpIHtcbiAgICBpZiAodGhpcy5mb3JtLmludmFsaWQpIHJldHVybjtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgbmV3IENoYW5nZVBhc3N3b3JkKHtcbiAgICAgICAgICBjdXJyZW50UGFzc3dvcmQ6IHRoaXMuZm9ybS5nZXQoJ3Bhc3N3b3JkJykudmFsdWUsXG4gICAgICAgICAgbmV3UGFzc3dvcmQ6IHRoaXMuZm9ybS5nZXQoJ25ld1Bhc3N3b3JkJykudmFsdWUsXG4gICAgICAgIH0pLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSh7XG4gICAgICAgIG5leHQ6ICgpID0+IHtcbiAgICAgICAgICB0aGlzLmZvcm0ucmVzZXQoKTtcbiAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLnN1Y2Nlc3MoJ0FicEFjY291bnQ6OlBhc3N3b3JkQ2hhbmdlZE1lc3NhZ2UnLCAnU3VjY2VzcycsIHsgbGlmZTogNTAwMCB9KTtcbiAgICAgICAgfSxcbiAgICAgICAgZXJyb3I6IGVyciA9PiB7XG4gICAgICAgICAgdGhpcy50b2FzdGVyU2VydmljZS5lcnJvcihzbnEoKCkgPT4gZXJyLmVycm9yLmVycm9yLm1lc3NhZ2UsICdBYnBBY2NvdW50OjpEZWZhdWx0RXJyb3JNZXNzYWdlJyksICdFcnJvcicsIHtcbiAgICAgICAgICAgIGxpZmU6IDcwMDAsXG4gICAgICAgICAgfSk7XG4gICAgICAgIH0sXG4gICAgICB9KTtcbiAgfVxufVxuIl19