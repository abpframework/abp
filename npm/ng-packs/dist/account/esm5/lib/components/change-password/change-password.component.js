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
import { finalize } from 'rxjs/operators';
var minLength = Validators.minLength,
  required = Validators.required;
/** @type {?} */
var PASSWORD_FIELDS = ['newPassword', 'repeatNewPassword'];
var ChangePasswordComponent = /** @class */ (function() {
  function ChangePasswordComponent(fb, store, toasterService) {
    this.fb = fb;
    this.store = store;
    this.toasterService = toasterService;
    this.mapErrorsFn
    /**
     * @param {?} errors
     * @param {?} groupErrors
     * @param {?} control
     * @return {?}
     */ = function(errors, groupErrors, control) {
      if (PASSWORD_FIELDS.indexOf(control.name) < 0) return errors;
      return errors.concat(
        groupErrors.filter(
          /**
           * @param {?} __0
           * @return {?}
           */
          function(_a) {
            var key = _a.key;
            return key === 'passwordMismatch';
          },
        ),
      );
    };
  }
  /**
   * @return {?}
   */
  ChangePasswordComponent.prototype.ngOnInit
  /**
   * @return {?}
   */ = function() {
    this.form = this.fb.group(
      {
        password: ['', required],
        newPassword: ['', required],
        repeatNewPassword: ['', required],
      },
      {
        validators: [comparePasswords(PASSWORD_FIELDS)],
      },
    );
  };
  /**
   * @return {?}
   */
  ChangePasswordComponent.prototype.onSubmit
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    if (this.form.invalid) return;
    this.inProgress = true;
    this.store
      .dispatch(
        new ChangePassword({
          currentPassword: this.form.get('password').value,
          newPassword: this.form.get('newPassword').value,
        }),
      )
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          function() {
            return (_this.inProgress = false);
          },
        ),
      )
      .subscribe({
        /**
         * @return {?}
         */
        next: function() {
          _this.form.reset();
          _this.toasterService.success('AbpAccount::PasswordChangedMessage', 'Success', { life: 5000 });
        },
        /**
         * @param {?} err
         * @return {?}
         */
        error: function(err) {
          _this.toasterService.error(
            snq(
              /**
               * @return {?}
               */
              function() {
                return err.error.error.message;
              },
              'AbpAccount::DefaultErrorMessage',
            ),
            'Error',
            {
              life: 7000,
            },
          );
        },
      });
  };
  ChangePasswordComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-change-password-form',
          template:
            '<form [formGroup]="form" (ngSubmit)="onSubmit()" [mapErrorsFn]="mapErrorsFn">\n  <div class="form-group">\n    <label for="current-password">{{ \'AbpIdentity::DisplayName:CurrentPassword\' | abpLocalization }}</label\n    ><span> * </span\n    ><input type="password" id="current-password" class="form-control" formControlName="password" autofocus />\n  </div>\n  <div class="form-group">\n    <label for="new-password">{{ \'AbpIdentity::DisplayName:NewPassword\' | abpLocalization }}</label\n    ><span> * </span><input type="password" id="new-password" class="form-control" formControlName="newPassword" />\n  </div>\n  <div class="form-group">\n    <label for="confirm-new-password">{{ \'AbpIdentity::DisplayName:NewPasswordConfirm\' | abpLocalization }}</label\n    ><span> * </span\n    ><input type="password" id="confirm-new-password" class="form-control" formControlName="repeatNewPassword" />\n  </div>\n  <abp-button\n    iconClass="fa fa-check"\n    buttonClass="btn btn-primary color-white"\n    buttonType="submit"\n    [loading]="inProgress"\n    >{{ \'AbpIdentity::Save\' | abpLocalization }}</abp-button\n  >\n</form>\n',
        },
      ],
    },
  ];
  /** @nocollapse */
  ChangePasswordComponent.ctorParameters = function() {
    return [{ type: FormBuilder }, { type: Store }, { type: ToasterService }];
  };
  return ChangePasswordComponent;
})();
export { ChangePasswordComponent };
if (false) {
  /** @type {?} */
  ChangePasswordComponent.prototype.form;
  /** @type {?} */
  ChangePasswordComponent.prototype.inProgress;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2NoYW5nZS1wYXNzd29yZC9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzlDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLGdCQUFnQixFQUFjLE1BQU0sb0JBQW9CLENBQUM7QUFDbEUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRWxDLElBQUEsZ0NBQVMsRUFBRSw4QkFBUTs7SUFFckIsZUFBZSxHQUFHLENBQUMsYUFBYSxFQUFFLG1CQUFtQixDQUFDO0FBRTVEO0lBZUUsaUNBQW9CLEVBQWUsRUFBVSxLQUFZLEVBQVUsY0FBOEI7UUFBN0UsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFOakcsZ0JBQVc7Ozs7OztRQUEyQixVQUFDLE1BQU0sRUFBRSxXQUFXLEVBQUUsT0FBTztZQUNqRSxJQUFJLGVBQWUsQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUM7Z0JBQUUsT0FBTyxNQUFNLENBQUM7WUFFN0QsT0FBTyxNQUFNLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxNQUFNOzs7O1lBQUMsVUFBQyxFQUFPO29CQUFMLFlBQUc7Z0JBQU8sT0FBQSxHQUFHLEtBQUssa0JBQWtCO1lBQTFCLENBQTBCLEVBQUMsQ0FBQyxDQUFDO1FBQ3BGLENBQUMsRUFBQztJQUVrRyxDQUFDOzs7O0lBRXJHLDBDQUFROzs7SUFBUjtRQUNFLElBQUksQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQ3ZCO1lBQ0UsUUFBUSxFQUFFLENBQUMsRUFBRSxFQUFFLFFBQVEsQ0FBQztZQUN4QixXQUFXLEVBQUUsQ0FBQyxFQUFFLEVBQUUsUUFBUSxDQUFDO1lBQzNCLGlCQUFpQixFQUFFLENBQUMsRUFBRSxFQUFFLFFBQVEsQ0FBQztTQUNsQyxFQUNEO1lBQ0UsVUFBVSxFQUFFLENBQUMsZ0JBQWdCLENBQUMsZUFBZSxDQUFDLENBQUM7U0FDaEQsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7OztJQUVELDBDQUFROzs7SUFBUjtRQUFBLGlCQXNCQztRQXJCQyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFDOUIsSUFBSSxDQUFDLFVBQVUsR0FBRyxJQUFJLENBQUM7UUFDdkIsSUFBSSxDQUFDLEtBQUs7YUFDUCxRQUFRLENBQ1AsSUFBSSxjQUFjLENBQUM7WUFDakIsZUFBZSxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLFVBQVUsQ0FBQyxDQUFDLEtBQUs7WUFDaEQsV0FBVyxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLGFBQWEsQ0FBQyxDQUFDLEtBQUs7U0FDaEQsQ0FBQyxDQUNIO2FBQ0EsSUFBSSxDQUFDLFFBQVE7OztRQUFDLGNBQU0sT0FBQSxDQUFDLEtBQUksQ0FBQyxVQUFVLEdBQUcsS0FBSyxDQUFDLEVBQXpCLENBQXlCLEVBQUMsQ0FBQzthQUMvQyxTQUFTLENBQUM7WUFDVCxJQUFJOzs7WUFBRTtnQkFDSixLQUFJLENBQUMsSUFBSSxDQUFDLEtBQUssRUFBRSxDQUFDO2dCQUNsQixLQUFJLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxvQ0FBb0MsRUFBRSxTQUFTLEVBQUUsRUFBRSxJQUFJLEVBQUUsSUFBSSxFQUFFLENBQUMsQ0FBQztZQUMvRixDQUFDLENBQUE7WUFDRCxLQUFLOzs7O1lBQUUsVUFBQSxHQUFHO2dCQUNSLEtBQUksQ0FBQyxjQUFjLENBQUMsS0FBSyxDQUFDLEdBQUc7OztnQkFBQyxjQUFNLE9BQUEsR0FBRyxDQUFDLEtBQUssQ0FBQyxLQUFLLENBQUMsT0FBTyxFQUF2QixDQUF1QixHQUFFLGlDQUFpQyxDQUFDLEVBQUUsT0FBTyxFQUFFO29CQUN4RyxJQUFJLEVBQUUsSUFBSTtpQkFDWCxDQUFDLENBQUM7WUFDTCxDQUFDLENBQUE7U0FDRixDQUFDLENBQUM7SUFDUCxDQUFDOztnQkFwREYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSwwQkFBMEI7b0JBQ3BDLHFxQ0FBK0M7aUJBQ2hEOzs7O2dCQWJRLFdBQVc7Z0JBRVgsS0FBSztnQkFKTCxjQUFjOztJQWlFdkIsOEJBQUM7Q0FBQSxBQXJERCxJQXFEQztTQWpEWSx1QkFBdUI7OztJQUNsQyx1Q0FBZ0I7O0lBRWhCLDZDQUFvQjs7SUFFcEIsOENBSUU7Ozs7O0lBRVUscUNBQXVCOzs7OztJQUFFLHdDQUFvQjs7Ozs7SUFBRSxpREFBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDaGFuZ2VQYXNzd29yZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgY29tcGFyZVBhc3N3b3JkcywgVmFsaWRhdGlvbiB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IGZpbmFsaXplIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5jb25zdCB7IG1pbkxlbmd0aCwgcmVxdWlyZWQgfSA9IFZhbGlkYXRvcnM7XG5cbmNvbnN0IFBBU1NXT1JEX0ZJRUxEUyA9IFsnbmV3UGFzc3dvcmQnLCAncmVwZWF0TmV3UGFzc3dvcmQnXTtcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWNoYW5nZS1wYXNzd29yZC1mb3JtJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2NoYW5nZS1wYXNzd29yZC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIENoYW5nZVBhc3N3b3JkQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIGluUHJvZ3Jlc3M6IGJvb2xlYW47XG5cbiAgbWFwRXJyb3JzRm46IFZhbGlkYXRpb24uTWFwRXJyb3JzRm4gPSAoZXJyb3JzLCBncm91cEVycm9ycywgY29udHJvbCkgPT4ge1xuICAgIGlmIChQQVNTV09SRF9GSUVMRFMuaW5kZXhPZihjb250cm9sLm5hbWUpIDwgMCkgcmV0dXJuIGVycm9ycztcblxuICAgIHJldHVybiBlcnJvcnMuY29uY2F0KGdyb3VwRXJyb3JzLmZpbHRlcigoeyBrZXkgfSkgPT4ga2V5ID09PSAncGFzc3dvcmRNaXNtYXRjaCcpKTtcbiAgfTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGZiOiBGb3JtQnVpbGRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgdG9hc3RlclNlcnZpY2U6IFRvYXN0ZXJTZXJ2aWNlKSB7fVxuXG4gIG5nT25Jbml0KCk6IHZvaWQge1xuICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoXG4gICAgICB7XG4gICAgICAgIHBhc3N3b3JkOiBbJycsIHJlcXVpcmVkXSxcbiAgICAgICAgbmV3UGFzc3dvcmQ6IFsnJywgcmVxdWlyZWRdLFxuICAgICAgICByZXBlYXROZXdQYXNzd29yZDogWycnLCByZXF1aXJlZF0sXG4gICAgICB9LFxuICAgICAge1xuICAgICAgICB2YWxpZGF0b3JzOiBbY29tcGFyZVBhc3N3b3JkcyhQQVNTV09SRF9GSUVMRFMpXSxcbiAgICAgIH0sXG4gICAgKTtcbiAgfVxuXG4gIG9uU3VibWl0KCkge1xuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xuICAgIHRoaXMuaW5Qcm9ncmVzcyA9IHRydWU7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKFxuICAgICAgICBuZXcgQ2hhbmdlUGFzc3dvcmQoe1xuICAgICAgICAgIGN1cnJlbnRQYXNzd29yZDogdGhpcy5mb3JtLmdldCgncGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgICAgICBuZXdQYXNzd29yZDogdGhpcy5mb3JtLmdldCgnbmV3UGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiAodGhpcy5pblByb2dyZXNzID0gZmFsc2UpKSlcbiAgICAgIC5zdWJzY3JpYmUoe1xuICAgICAgICBuZXh0OiAoKSA9PiB7XG4gICAgICAgICAgdGhpcy5mb3JtLnJlc2V0KCk7XG4gICAgICAgICAgdGhpcy50b2FzdGVyU2VydmljZS5zdWNjZXNzKCdBYnBBY2NvdW50OjpQYXNzd29yZENoYW5nZWRNZXNzYWdlJywgJ1N1Y2Nlc3MnLCB7IGxpZmU6IDUwMDAgfSk7XG4gICAgICAgIH0sXG4gICAgICAgIGVycm9yOiBlcnIgPT4ge1xuICAgICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2UuZXJyb3Ioc25xKCgpID0+IGVyci5lcnJvci5lcnJvci5tZXNzYWdlLCAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZScpLCAnRXJyb3InLCB7XG4gICAgICAgICAgICBsaWZlOiA3MDAwLFxuICAgICAgICAgIH0pO1xuICAgICAgICB9LFxuICAgICAgfSk7XG4gIH1cbn1cbiJdfQ==
