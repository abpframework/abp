/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ProfileChangePassword } from '@abp/ng.core';
import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild, } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { comparePasswords, validatePassword } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
var minLength = Validators.minLength, required = Validators.required;
var ChangePasswordComponent = /** @class */ (function () {
    function ChangePasswordComponent(fb, store) {
        this.fb = fb;
        this.store = store;
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
            password: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
            newPassword: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
            repeatNewPassword: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
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
            .dispatch(new ProfileChangePassword({
            currentPassword: this.form.get('password').value,
            newPassword: this.form.get('newPassword').value,
        }))
            .subscribe((/**
         * @return {?}
         */
        function () {
            _this.visible = false;
        }));
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
                    template: "<abp-modal *ngIf=\"visible\" [(visible)]=\"visible\">\n  <ng-template #abpHeader>\n    <h4>{{ 'AbpIdentity::ChangePassword' | abpLocalization }}</h4>\n  </ng-template>\n  <ng-template #abpBody>\n    <form [formGroup]=\"form\" novalidate>\n      <div class=\"form-group\">\n        <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\n      </div>\n      <div class=\"form-group\" [class.is-invalid]=\"form.errors?.passwordMismatch\">\n        <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\n        ><span> * </span\n        ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\n        <div *ngIf=\"form.errors?.passwordMismatch\" class=\"invalid-feedback\">\n          {{ 'AbpIdentity::Identity.PasswordConfirmationFailed' | abpLocalization }}\n        </div>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"onSubmit()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n"
                }] }
    ];
    /** @nocollapse */
    ChangePasswordComponent.ctorParameters = function () { return [
        { type: FormBuilder },
        { type: Store }
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
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFuZ2UtcGFzc3dvcmQvY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3JELE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBRU4sV0FBVyxFQUNYLFNBQVMsR0FDVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQ3hFLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFFNUIsSUFBQSxnQ0FBUyxFQUFFLDhCQUFRO0FBRTNCO0lBeUJFLGlDQUFvQixFQUFlLEVBQVUsS0FBWTtRQUFyQyxPQUFFLEdBQUYsRUFBRSxDQUFhO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQVB6RCxrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFXLENBQUM7SUFPZ0IsQ0FBQztJQWxCN0Qsc0JBQ0ksNENBQU87Ozs7UUFEWDtZQUVFLE9BQU8sSUFBSSxDQUFDLFFBQVEsQ0FBQztRQUN2QixDQUFDOzs7OztRQUVELFVBQVksS0FBYztZQUN4QixJQUFJLENBQUMsUUFBUSxHQUFHLEtBQUssQ0FBQztZQUN0QixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDOzs7T0FMQTs7OztJQWlCRCwwQ0FBUTs7O0lBQVI7UUFDRSxJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUN2QjtZQUNFLFFBQVEsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLFFBQVEsRUFBRSxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUUsZ0JBQWdCLENBQUMsQ0FBQyxPQUFPLEVBQUUsU0FBUyxFQUFFLFFBQVEsRUFBRSxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUM7WUFDckcsV0FBVyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsUUFBUSxFQUFFLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBRSxnQkFBZ0IsQ0FBQyxDQUFDLE9BQU8sRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQztZQUN4RyxpQkFBaUIsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLFFBQVEsRUFBRSxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUUsZ0JBQWdCLENBQUMsQ0FBQyxPQUFPLEVBQUUsU0FBUyxFQUFFLFFBQVEsRUFBRSxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDL0csRUFDRDtZQUNFLFVBQVUsRUFBRSxDQUFDLGdCQUFnQixDQUFDLENBQUMsYUFBYSxFQUFFLG1CQUFtQixDQUFDLENBQUMsQ0FBQztTQUNyRSxDQUNGLENBQUM7SUFDSixDQUFDOzs7O0lBRUQsMENBQVE7OztJQUFSO1FBQUEsaUJBYUM7UUFaQyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFFOUIsSUFBSSxDQUFDLEtBQUs7YUFDUCxRQUFRLENBQ1AsSUFBSSxxQkFBcUIsQ0FBQztZQUN4QixlQUFlLEVBQUUsSUFBSSxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsVUFBVSxDQUFDLENBQUMsS0FBSztZQUNoRCxXQUFXLEVBQUUsSUFBSSxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsYUFBYSxDQUFDLENBQUMsS0FBSztTQUNoRCxDQUFDLENBQ0g7YUFDQSxTQUFTOzs7UUFBQztZQUNULEtBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO1FBQ3ZCLENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7OztJQUVELDJDQUFTOzs7SUFBVDtRQUNFLElBQUksQ0FBQyxPQUFPLEdBQUcsSUFBSSxDQUFDO0lBQ3RCLENBQUM7Ozs7O0lBRUQsNkNBQVc7Ozs7SUFBWCxVQUFZLEVBQTBCO1lBQXhCLG9CQUFPO1FBQ25CLElBQUksQ0FBQyxPQUFPO1lBQUUsT0FBTztRQUVyQixJQUFJLE9BQU8sQ0FBQyxZQUFZLEVBQUU7WUFDeEIsSUFBSSxDQUFDLFNBQVMsRUFBRSxDQUFDO1NBQ2xCO2FBQU0sSUFBSSxPQUFPLENBQUMsWUFBWSxLQUFLLEtBQUssSUFBSSxJQUFJLENBQUMsT0FBTyxFQUFFO1lBQ3pELElBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO1NBQ3RCO0lBQ0gsQ0FBQzs7Z0JBbkVGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUscUJBQXFCO29CQUMvQixneERBQStDO2lCQUNoRDs7OztnQkFUUSxXQUFXO2dCQUVYLEtBQUs7OzswQkFXWCxLQUFLO2dDQVVMLE1BQU07K0JBR04sU0FBUyxTQUFDLGNBQWMsRUFBRSxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUU7O0lBZ0Q5Qyw4QkFBQztDQUFBLEFBcEVELElBb0VDO1NBaEVZLHVCQUF1Qjs7Ozs7O0lBQ2xDLDJDQUFtQjs7SUFZbkIsZ0RBQzRDOztJQUU1QywrQ0FDK0I7O0lBRS9CLHVDQUFnQjs7Ozs7SUFFSixxQ0FBdUI7Ozs7O0lBQUUsd0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUHJvZmlsZUNoYW5nZVBhc3N3b3JkIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7XG4gIENvbXBvbmVudCxcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25DaGFuZ2VzLFxuICBPbkluaXQsXG4gIE91dHB1dCxcbiAgU2ltcGxlQ2hhbmdlcyxcbiAgVGVtcGxhdGVSZWYsXG4gIFZpZXdDaGlsZCxcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgY29tcGFyZVBhc3N3b3JkcywgdmFsaWRhdGVQYXNzd29yZCB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcblxuY29uc3QgeyBtaW5MZW5ndGgsIHJlcXVpcmVkIH0gPSBWYWxpZGF0b3JzO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtY2hhbmdlLXBhc3N3b3JkJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2NoYW5nZS1wYXNzd29yZC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIENoYW5nZVBhc3N3b3JkQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0LCBPbkNoYW5nZXMge1xuICBwcm90ZWN0ZWQgX3Zpc2libGU7XG5cbiAgQElucHV0KClcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XG4gIH1cblxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gIH1cblxuICBAT3V0cHV0KClcbiAgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcblxuICBAVmlld0NoaWxkKCdtb2RhbENvbnRlbnQnLCB7IHN0YXRpYzogZmFsc2UgfSlcbiAgbW9kYWxDb250ZW50OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIGZvcm06IEZvcm1Hcm91cDtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGZiOiBGb3JtQnVpbGRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7XG4gICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cChcbiAgICAgIHtcbiAgICAgICAgcGFzc3dvcmQ6IFsnJywgW3JlcXVpcmVkLCBtaW5MZW5ndGgoNiksIHZhbGlkYXRlUGFzc3dvcmQoWydzbWFsbCcsICdjYXBpdGFsJywgJ251bWJlcicsICdzcGVjaWFsJ10pXV0sXG4gICAgICAgIG5ld1Bhc3N3b3JkOiBbJycsIFtyZXF1aXJlZCwgbWluTGVuZ3RoKDYpLCB2YWxpZGF0ZVBhc3N3b3JkKFsnc21hbGwnLCAnY2FwaXRhbCcsICdudW1iZXInLCAnc3BlY2lhbCddKV1dLFxuICAgICAgICByZXBlYXROZXdQYXNzd29yZDogWycnLCBbcmVxdWlyZWQsIG1pbkxlbmd0aCg2KSwgdmFsaWRhdGVQYXNzd29yZChbJ3NtYWxsJywgJ2NhcGl0YWwnLCAnbnVtYmVyJywgJ3NwZWNpYWwnXSldXSxcbiAgICAgIH0sXG4gICAgICB7XG4gICAgICAgIHZhbGlkYXRvcnM6IFtjb21wYXJlUGFzc3dvcmRzKFsnbmV3UGFzc3dvcmQnLCAncmVwZWF0TmV3UGFzc3dvcmQnXSldLFxuICAgICAgfSxcbiAgICApO1xuICB9XG5cbiAgb25TdWJtaXQoKSB7XG4gICAgaWYgKHRoaXMuZm9ybS5pbnZhbGlkKSByZXR1cm47XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBQcm9maWxlQ2hhbmdlUGFzc3dvcmQoe1xuICAgICAgICAgIGN1cnJlbnRQYXNzd29yZDogdGhpcy5mb3JtLmdldCgncGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgICAgICBuZXdQYXNzd29yZDogdGhpcy5mb3JtLmdldCgnbmV3UGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgICB9KTtcbiAgfVxuXG4gIG9wZW5Nb2RhbCgpIHtcbiAgICB0aGlzLnZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgbmdPbkNoYW5nZXMoeyB2aXNpYmxlIH06IFNpbXBsZUNoYW5nZXMpOiB2b2lkIHtcbiAgICBpZiAoIXZpc2libGUpIHJldHVybjtcblxuICAgIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSkge1xuICAgICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgICB9IGVsc2UgaWYgKHZpc2libGUuY3VycmVudFZhbHVlID09PSBmYWxzZSAmJiB0aGlzLnZpc2libGUpIHtcbiAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgIH1cbiAgfVxufVxuIl19