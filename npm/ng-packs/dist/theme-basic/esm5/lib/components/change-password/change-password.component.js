/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangePassword } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild, } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { comparePasswords } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import snq from 'snq';
var minLength = Validators.minLength, required = Validators.required;
var ChangePasswordComponent = /** @class */ (function () {
    function ChangePasswordComponent(fb, store, toasterService) {
        this.fb = fb;
        this.store = store;
        this.toasterService = toasterService;
        this.visibleChange = new EventEmitter();
    }
    Object.defineProperty(ChangePasswordComponent.prototype, "visible", {
        get: /**
         * @return {?}
         */
        function () {
            return this._visible;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this._visible = value;
            this.visibleChange.emit(value);
        },
        enumerable: true,
        configurable: true
    });
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
            validators: [comparePasswords(['newPassword', 'repeatNewPassword'])],
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
                _this.visible = false;
                _this.form.reset();
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
    /**
     * @return {?}
     */
    ChangePasswordComponent.prototype.openModal = /**
     * @return {?}
     */
    function () {
        this.visible = true;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    ChangePasswordComponent.prototype.ngOnChanges = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var visible = _a.visible;
        if (!visible)
            return;
        if (visible.currentValue) {
            this.openModal();
        }
        else if (visible.currentValue === false && this.visible) {
            this.visible = false;
        }
    };
    ChangePasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-change-password',
                    template: "<abp-modal [(visible)]=\"visible\">\n  <ng-template #abpHeader>\n    <h4>{{ 'AbpIdentity::ChangePassword' | abpLocalization }}</h4>\n  </ng-template>\n  <ng-template #abpBody>\n    <form [formGroup]=\"form\" novalidate>\n      <div class=\"form-group\">\n        <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\n        ><span> * </span\n        ><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" autofocus />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\n      </div>\n      <div class=\"form-group\" [class.is-invalid]=\"form.errors?.passwordMismatch\">\n        <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\n        ><span> * </span\n        ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\n        <div *ngIf=\"form.errors?.passwordMismatch\" class=\"invalid-feedback\">\n          {{ 'AbpIdentity::Identity.PasswordConfirmationFailed' | abpLocalization }}\n        </div>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <abp-button\n      [requestType]=\"['POST']\"\n      requestURLContainSearchValue=\"change-password\"\n      iconClass=\"fa fa-check\"\n      (click)=\"onSubmit()\"\n      >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n    >\n  </ng-template>\n</abp-modal>\n"
                }] }
    ];
    /** @nocollapse */
    ChangePasswordComponent.ctorParameters = function () { return [
        { type: FormBuilder },
        { type: Store },
        { type: ToasterService }
    ]; };
    ChangePasswordComponent.propDecorators = {
        visible: [{ type: Input }],
        visibleChange: [{ type: Output }],
        modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
    };
    return ChangePasswordComponent;
}());
export { ChangePasswordComponent };
if (false) {
    /**
     * @type {?}
     * @protected
     */
    ChangePasswordComponent.prototype._visible;
    /** @type {?} */
    ChangePasswordComponent.prototype.visibleChange;
    /** @type {?} */
    ChangePasswordComponent.prototype.modalContent;
    /** @type {?} */
    ChangePasswordComponent.prototype.form;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFuZ2UtcGFzc3dvcmQvY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUM5QyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUNMLFNBQVMsRUFDVCxZQUFZLEVBQ1osS0FBSyxFQUdMLE1BQU0sRUFFTixXQUFXLEVBQ1gsU0FBUyxHQUNWLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDdEQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFFZCxJQUFBLGdDQUFTLEVBQUUsOEJBQVE7QUFFM0I7SUF5QkUsaUNBQW9CLEVBQWUsRUFBVSxLQUFZLEVBQVUsY0FBOEI7UUFBN0UsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFQakcsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO0lBT3dELENBQUM7SUFsQnJHLHNCQUNJLDRDQUFPOzs7O1FBRFg7WUFFRSxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUM7UUFDdkIsQ0FBQzs7Ozs7UUFFRCxVQUFZLEtBQWM7WUFDeEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxLQUFLLENBQUM7WUFDdEIsSUFBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7UUFDakMsQ0FBQzs7O09BTEE7Ozs7SUFpQkQsMENBQVE7OztJQUFSO1FBQ0UsSUFBSSxDQUFDLElBQUksR0FBRyxJQUFJLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FDdkI7WUFDRSxRQUFRLEVBQUUsQ0FBQyxFQUFFLEVBQUUsUUFBUSxDQUFDO1lBQ3hCLFdBQVcsRUFBRSxDQUFDLEVBQUUsRUFBRSxRQUFRLENBQUM7WUFDM0IsaUJBQWlCLEVBQUUsQ0FBQyxFQUFFLEVBQUUsUUFBUSxDQUFDO1NBQ2xDLEVBQ0Q7WUFDRSxVQUFVLEVBQUUsQ0FBQyxnQkFBZ0IsQ0FBQyxDQUFDLGFBQWEsRUFBRSxtQkFBbUIsQ0FBQyxDQUFDLENBQUM7U0FDckUsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7OztJQUVELDBDQUFROzs7SUFBUjtRQUFBLGlCQXFCQztRQXBCQyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFFOUIsSUFBSSxDQUFDLEtBQUs7YUFDUCxRQUFRLENBQ1AsSUFBSSxjQUFjLENBQUM7WUFDakIsZUFBZSxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLFVBQVUsQ0FBQyxDQUFDLEtBQUs7WUFDaEQsV0FBVyxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLGFBQWEsQ0FBQyxDQUFDLEtBQUs7U0FDaEQsQ0FBQyxDQUNIO2FBQ0EsU0FBUyxDQUFDO1lBQ1QsSUFBSTs7O1lBQUU7Z0JBQ0osS0FBSSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUM7Z0JBQ3JCLEtBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxFQUFFLENBQUM7WUFDcEIsQ0FBQyxDQUFBO1lBQ0QsS0FBSzs7OztZQUFFLFVBQUEsR0FBRztnQkFDUixLQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyxHQUFHOzs7Z0JBQUMsY0FBTSxPQUFBLEdBQUcsQ0FBQyxLQUFLLENBQUMsS0FBSyxDQUFDLE9BQU8sRUFBdkIsQ0FBdUIsR0FBRSxpQ0FBaUMsQ0FBQyxFQUFFLE9BQU8sRUFBRTtvQkFDeEcsSUFBSSxFQUFFLElBQUk7aUJBQ1gsQ0FBQyxDQUFDO1lBQ0wsQ0FBQyxDQUFBO1NBQ0YsQ0FBQyxDQUFDO0lBQ1AsQ0FBQzs7OztJQUVELDJDQUFTOzs7SUFBVDtRQUNFLElBQUksQ0FBQyxPQUFPLEdBQUcsSUFBSSxDQUFDO0lBQ3RCLENBQUM7Ozs7O0lBRUQsNkNBQVc7Ozs7SUFBWCxVQUFZLEVBQTBCO1lBQXhCLG9CQUFPO1FBQ25CLElBQUksQ0FBQyxPQUFPO1lBQUUsT0FBTztRQUVyQixJQUFJLE9BQU8sQ0FBQyxZQUFZLEVBQUU7WUFDeEIsSUFBSSxDQUFDLFNBQVMsRUFBRSxDQUFDO1NBQ2xCO2FBQU0sSUFBSSxPQUFPLENBQUMsWUFBWSxLQUFLLEtBQUssSUFBSSxJQUFJLENBQUMsT0FBTyxFQUFFO1lBQ3pELElBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO1NBQ3RCO0lBQ0gsQ0FBQzs7Z0JBM0VGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUscUJBQXFCO29CQUMvQixrMERBQStDO2lCQUNoRDs7OztnQkFWUSxXQUFXO2dCQUVYLEtBQUs7Z0JBZEwsY0FBYzs7OzBCQTBCcEIsS0FBSztnQ0FVTCxNQUFNOytCQUdOLFNBQVMsU0FBQyxjQUFjLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOztJQXdEOUMsOEJBQUM7Q0FBQSxBQTVFRCxJQTRFQztTQXhFWSx1QkFBdUI7Ozs7OztJQUNsQywyQ0FBbUI7O0lBWW5CLGdEQUM0Qzs7SUFFNUMsK0NBQytCOztJQUUvQix1Q0FBZ0I7Ozs7O0lBRUoscUNBQXVCOzs7OztJQUFFLHdDQUFvQjs7Ozs7SUFBRSxpREFBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDaGFuZ2VQYXNzd29yZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7XG4gIENvbXBvbmVudCxcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25DaGFuZ2VzLFxuICBPbkluaXQsXG4gIE91dHB1dCxcbiAgU2ltcGxlQ2hhbmdlcyxcbiAgVGVtcGxhdGVSZWYsXG4gIFZpZXdDaGlsZCxcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgY29tcGFyZVBhc3N3b3JkcyB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuY29uc3QgeyBtaW5MZW5ndGgsIHJlcXVpcmVkIH0gPSBWYWxpZGF0b3JzO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtY2hhbmdlLXBhc3N3b3JkJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2NoYW5nZS1wYXNzd29yZC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIENoYW5nZVBhc3N3b3JkQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0LCBPbkNoYW5nZXMge1xuICBwcm90ZWN0ZWQgX3Zpc2libGU7XG5cbiAgQElucHV0KClcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XG4gIH1cblxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gIH1cblxuICBAT3V0cHV0KClcbiAgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcblxuICBAVmlld0NoaWxkKCdtb2RhbENvbnRlbnQnLCB7IHN0YXRpYzogZmFsc2UgfSlcbiAgbW9kYWxDb250ZW50OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIGZvcm06IEZvcm1Hcm91cDtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGZiOiBGb3JtQnVpbGRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgdG9hc3RlclNlcnZpY2U6IFRvYXN0ZXJTZXJ2aWNlKSB7fVxuXG4gIG5nT25Jbml0KCk6IHZvaWQge1xuICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoXG4gICAgICB7XG4gICAgICAgIHBhc3N3b3JkOiBbJycsIHJlcXVpcmVkXSxcbiAgICAgICAgbmV3UGFzc3dvcmQ6IFsnJywgcmVxdWlyZWRdLFxuICAgICAgICByZXBlYXROZXdQYXNzd29yZDogWycnLCByZXF1aXJlZF0sXG4gICAgICB9LFxuICAgICAge1xuICAgICAgICB2YWxpZGF0b3JzOiBbY29tcGFyZVBhc3N3b3JkcyhbJ25ld1Bhc3N3b3JkJywgJ3JlcGVhdE5ld1Bhc3N3b3JkJ10pXSxcbiAgICAgIH0sXG4gICAgKTtcbiAgfVxuXG4gIG9uU3VibWl0KCkge1xuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xuXG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKFxuICAgICAgICBuZXcgQ2hhbmdlUGFzc3dvcmQoe1xuICAgICAgICAgIGN1cnJlbnRQYXNzd29yZDogdGhpcy5mb3JtLmdldCgncGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgICAgICBuZXdQYXNzd29yZDogdGhpcy5mb3JtLmdldCgnbmV3UGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKHtcbiAgICAgICAgbmV4dDogKCkgPT4ge1xuICAgICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICAgIHRoaXMuZm9ybS5yZXNldCgpO1xuICAgICAgICB9LFxuICAgICAgICBlcnJvcjogZXJyID0+IHtcbiAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKHNucSgoKSA9PiBlcnIuZXJyb3IuZXJyb3IubWVzc2FnZSwgJ0FicEFjY291bnQ6OkRlZmF1bHRFcnJvck1lc3NhZ2UnKSwgJ0Vycm9yJywge1xuICAgICAgICAgICAgbGlmZTogNzAwMCxcbiAgICAgICAgICB9KTtcbiAgICAgICAgfSxcbiAgICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIHRoaXMudmlzaWJsZSA9IHRydWU7XG4gIH1cblxuICBuZ09uQ2hhbmdlcyh7IHZpc2libGUgfTogU2ltcGxlQ2hhbmdlcyk6IHZvaWQge1xuICAgIGlmICghdmlzaWJsZSkgcmV0dXJuO1xuXG4gICAgaWYgKHZpc2libGUuY3VycmVudFZhbHVlKSB7XG4gICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgIH0gZWxzZSBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUgPT09IGZhbHNlICYmIHRoaXMudmlzaWJsZSkge1xuICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG59XG4iXX0=