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
const { minLength, required } = Validators;
/** @type {?} */
const PASSWORD_FIELDS = ['newPassword', 'repeatNewPassword'];
export class ChangePasswordComponent {
  /**
   * @param {?} fb
   * @param {?} store
   * @param {?} toasterService
   */
  constructor(fb, store, toasterService) {
    this.fb = fb;
    this.store = store;
    this.toasterService = toasterService;
    this.mapErrorsFn
    /**
     * @param {?} errors
     * @param {?} groupErrors
     * @param {?} control
     * @return {?}
     */ = (errors, groupErrors, control) => {
      if (PASSWORD_FIELDS.indexOf(control.name) < 0) return errors;
      return errors.concat(
        groupErrors.filter(
          /**
           * @param {?} __0
           * @return {?}
           */
          ({ key }) => key === 'passwordMismatch',
        ),
      );
    };
  }
  /**
   * @return {?}
   */
  ngOnInit() {
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
  }
  /**
   * @return {?}
   */
  onSubmit() {
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
          () => (this.inProgress = false),
        ),
      )
      .subscribe({
        /**
         * @return {?}
         */
        next: () => {
          this.form.reset();
          this.toasterService.success('AbpAccount::PasswordChangedMessage', 'Success', { life: 5000 });
        },
        /**
         * @param {?} err
         * @return {?}
         */
        error: err => {
          this.toasterService.error(
            snq(
              /**
               * @return {?}
               */
              () => err.error.error.message,
              'AbpAccount::DefaultErrorMessage',
            ),
            'Error',
            {
              life: 7000,
            },
          );
        },
      });
  }
}
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
ChangePasswordComponent.ctorParameters = () => [{ type: FormBuilder }, { type: Store }, { type: ToasterService }];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2NoYW5nZS1wYXNzd29yZC9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzlDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLGdCQUFnQixFQUFjLE1BQU0sb0JBQW9CLENBQUM7QUFDbEUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO01BRXBDLEVBQUUsU0FBUyxFQUFFLFFBQVEsRUFBRSxHQUFHLFVBQVU7O01BRXBDLGVBQWUsR0FBRyxDQUFDLGFBQWEsRUFBRSxtQkFBbUIsQ0FBQztBQU01RCxNQUFNLE9BQU8sdUJBQXVCOzs7Ozs7SUFXbEMsWUFBb0IsRUFBZSxFQUFVLEtBQVksRUFBVSxjQUE4QjtRQUE3RSxPQUFFLEdBQUYsRUFBRSxDQUFhO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQU5qRyxnQkFBVzs7Ozs7O1FBQTJCLENBQUMsTUFBTSxFQUFFLFdBQVcsRUFBRSxPQUFPLEVBQUUsRUFBRTtZQUNyRSxJQUFJLGVBQWUsQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUM7Z0JBQUUsT0FBTyxNQUFNLENBQUM7WUFFN0QsT0FBTyxNQUFNLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxNQUFNOzs7O1lBQUMsQ0FBQyxFQUFFLEdBQUcsRUFBRSxFQUFFLEVBQUUsQ0FBQyxHQUFHLEtBQUssa0JBQWtCLEVBQUMsQ0FBQyxDQUFDO1FBQ3BGLENBQUMsRUFBQztJQUVrRyxDQUFDOzs7O0lBRXJHLFFBQVE7UUFDTixJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUN2QjtZQUNFLFFBQVEsRUFBRSxDQUFDLEVBQUUsRUFBRSxRQUFRLENBQUM7WUFDeEIsV0FBVyxFQUFFLENBQUMsRUFBRSxFQUFFLFFBQVEsQ0FBQztZQUMzQixpQkFBaUIsRUFBRSxDQUFDLEVBQUUsRUFBRSxRQUFRLENBQUM7U0FDbEMsRUFDRDtZQUNFLFVBQVUsRUFBRSxDQUFDLGdCQUFnQixDQUFDLGVBQWUsQ0FBQyxDQUFDO1NBQ2hELENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7SUFFRCxRQUFRO1FBQ04sSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU87WUFBRSxPQUFPO1FBQzlCLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDO1FBQ3ZCLElBQUksQ0FBQyxLQUFLO2FBQ1AsUUFBUSxDQUNQLElBQUksY0FBYyxDQUFDO1lBQ2pCLGVBQWUsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsQ0FBQyxLQUFLO1lBQ2hELFdBQVcsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxhQUFhLENBQUMsQ0FBQyxLQUFLO1NBQ2hELENBQUMsQ0FDSDthQUNBLElBQUksQ0FBQyxRQUFROzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxDQUFDLElBQUksQ0FBQyxVQUFVLEdBQUcsS0FBSyxDQUFDLEVBQUMsQ0FBQzthQUMvQyxTQUFTLENBQUM7WUFDVCxJQUFJOzs7WUFBRSxHQUFHLEVBQUU7Z0JBQ1QsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLEVBQUUsQ0FBQztnQkFDbEIsSUFBSSxDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsb0NBQW9DLEVBQUUsU0FBUyxFQUFFLEVBQUUsSUFBSSxFQUFFLElBQUksRUFBRSxDQUFDLENBQUM7WUFDL0YsQ0FBQyxDQUFBO1lBQ0QsS0FBSzs7OztZQUFFLEdBQUcsQ0FBQyxFQUFFO2dCQUNYLElBQUksQ0FBQyxjQUFjLENBQUMsS0FBSyxDQUFDLEdBQUc7OztnQkFBQyxHQUFHLEVBQUUsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLEtBQUssQ0FBQyxPQUFPLEdBQUUsaUNBQWlDLENBQUMsRUFBRSxPQUFPLEVBQUU7b0JBQ3hHLElBQUksRUFBRSxJQUFJO2lCQUNYLENBQUMsQ0FBQztZQUNMLENBQUMsQ0FBQTtTQUNGLENBQUMsQ0FBQztJQUNQLENBQUM7OztZQXBERixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLDBCQUEwQjtnQkFDcEMscXFDQUErQzthQUNoRDs7OztZQWJRLFdBQVc7WUFFWCxLQUFLO1lBSkwsY0FBYzs7OztJQWlCckIsdUNBQWdCOztJQUVoQiw2Q0FBb0I7O0lBRXBCLDhDQUlFOzs7OztJQUVVLHFDQUF1Qjs7Ozs7SUFBRSx3Q0FBb0I7Ozs7O0lBQUUsaURBQXNDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ2hhbmdlUGFzc3dvcmQgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVG9hc3RlclNlcnZpY2UgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IGNvbXBhcmVQYXNzd29yZHMsIFZhbGlkYXRpb24gfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBmaW5hbGl6ZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuY29uc3QgeyBtaW5MZW5ndGgsIHJlcXVpcmVkIH0gPSBWYWxpZGF0b3JzO1xuXG5jb25zdCBQQVNTV09SRF9GSUVMRFMgPSBbJ25ld1Bhc3N3b3JkJywgJ3JlcGVhdE5ld1Bhc3N3b3JkJ107XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1jaGFuZ2UtcGFzc3dvcmQtZm9ybScsXG4gIHRlbXBsYXRlVXJsOiAnLi9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIGZvcm06IEZvcm1Hcm91cDtcblxuICBpblByb2dyZXNzOiBib29sZWFuO1xuXG4gIG1hcEVycm9yc0ZuOiBWYWxpZGF0aW9uLk1hcEVycm9yc0ZuID0gKGVycm9ycywgZ3JvdXBFcnJvcnMsIGNvbnRyb2wpID0+IHtcbiAgICBpZiAoUEFTU1dPUkRfRklFTERTLmluZGV4T2YoY29udHJvbC5uYW1lKSA8IDApIHJldHVybiBlcnJvcnM7XG5cbiAgICByZXR1cm4gZXJyb3JzLmNvbmNhdChncm91cEVycm9ycy5maWx0ZXIoKHsga2V5IH0pID0+IGtleSA9PT0gJ3Bhc3N3b3JkTWlzbWF0Y2gnKSk7XG4gIH07XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIHRvYXN0ZXJTZXJ2aWNlOiBUb2FzdGVyU2VydmljZSkge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKFxuICAgICAge1xuICAgICAgICBwYXNzd29yZDogWycnLCByZXF1aXJlZF0sXG4gICAgICAgIG5ld1Bhc3N3b3JkOiBbJycsIHJlcXVpcmVkXSxcbiAgICAgICAgcmVwZWF0TmV3UGFzc3dvcmQ6IFsnJywgcmVxdWlyZWRdLFxuICAgICAgfSxcbiAgICAgIHtcbiAgICAgICAgdmFsaWRhdG9yczogW2NvbXBhcmVQYXNzd29yZHMoUEFTU1dPUkRfRklFTERTKV0sXG4gICAgICB9LFxuICAgICk7XG4gIH1cblxuICBvblN1Ym1pdCgpIHtcbiAgICBpZiAodGhpcy5mb3JtLmludmFsaWQpIHJldHVybjtcbiAgICB0aGlzLmluUHJvZ3Jlc3MgPSB0cnVlO1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgbmV3IENoYW5nZVBhc3N3b3JkKHtcbiAgICAgICAgICBjdXJyZW50UGFzc3dvcmQ6IHRoaXMuZm9ybS5nZXQoJ3Bhc3N3b3JkJykudmFsdWUsXG4gICAgICAgICAgbmV3UGFzc3dvcmQ6IHRoaXMuZm9ybS5nZXQoJ25ld1Bhc3N3b3JkJykudmFsdWUsXG4gICAgICAgIH0pLFxuICAgICAgKVxuICAgICAgLnBpcGUoZmluYWxpemUoKCkgPT4gKHRoaXMuaW5Qcm9ncmVzcyA9IGZhbHNlKSkpXG4gICAgICAuc3Vic2NyaWJlKHtcbiAgICAgICAgbmV4dDogKCkgPT4ge1xuICAgICAgICAgIHRoaXMuZm9ybS5yZXNldCgpO1xuICAgICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2Uuc3VjY2VzcygnQWJwQWNjb3VudDo6UGFzc3dvcmRDaGFuZ2VkTWVzc2FnZScsICdTdWNjZXNzJywgeyBsaWZlOiA1MDAwIH0pO1xuICAgICAgICB9LFxuICAgICAgICBlcnJvcjogZXJyID0+IHtcbiAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKHNucSgoKSA9PiBlcnIuZXJyb3IuZXJyb3IubWVzc2FnZSwgJ0FicEFjY291bnQ6OkRlZmF1bHRFcnJvck1lc3NhZ2UnKSwgJ0Vycm9yJywge1xuICAgICAgICAgICAgbGlmZTogNzAwMCxcbiAgICAgICAgICB9KTtcbiAgICAgICAgfSxcbiAgICAgIH0pO1xuICB9XG59XG4iXX0=
