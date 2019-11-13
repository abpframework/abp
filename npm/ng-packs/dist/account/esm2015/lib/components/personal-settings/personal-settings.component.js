/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { GetProfile, ProfileState, UpdateProfile } from '@abp/ng.core';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { take, withLatestFrom, finalize } from 'rxjs/operators';
import { ToasterService } from '@abp/ng.theme.shared';
const { maxLength, required, email } = Validators;
export class PersonalSettingsComponent {
  /**
   * @param {?} fb
   * @param {?} store
   * @param {?} toasterService
   */
  constructor(fb, store, toasterService) {
    this.fb = fb;
    this.store = store;
    this.toasterService = toasterService;
  }
  /**
   * @return {?}
   */
  ngOnInit() {
    this.buildForm();
  }
  /**
   * @return {?}
   */
  buildForm() {
    this.store
      .dispatch(new GetProfile())
      .pipe(
        withLatestFrom(this.profile$),
        take(1),
      )
      .subscribe(
        /**
         * @param {?} __0
         * @return {?}
         */
        ([, profile]) => {
          this.form = this.fb.group({
            userName: [profile.userName, [required, maxLength(256)]],
            email: [profile.email, [required, email, maxLength(256)]],
            name: [profile.name || '', [maxLength(64)]],
            surname: [profile.surname || '', [maxLength(64)]],
            phoneNumber: [profile.phoneNumber || '', [maxLength(16)]],
          });
        },
      );
  }
  /**
   * @return {?}
   */
  submit() {
    if (this.form.invalid) return;
    this.inProgress = true;
    this.store
      .dispatch(new UpdateProfile(this.form.value))
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          () => (this.inProgress = false),
        ),
      )
      .subscribe(
        /**
         * @return {?}
         */
        () => {
          this.toasterService.success('AbpAccount::PersonalSettingsSaved', 'Success', { life: 5000 });
        },
      );
  }
}
PersonalSettingsComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-personal-settings-form',
        template:
          '<form novalidate *ngIf="form" [formGroup]="form" (ngSubmit)="submit()">\n  <div class="form-group">\n    <label for="username">{{ \'AbpIdentity::DisplayName:UserName\' | abpLocalization }}</label\n    ><span> * </span\n    ><input\n      type="text"\n      id="username"\n      class="form-control"\n      formControlName="userName"\n      autofocus\n      (keydown.space)="$event.preventDefault()"\n    />\n  </div>\n  <div class="row">\n    <div class="col col-md-6">\n      <div class="form-group">\n        <label for="name">{{ \'AbpIdentity::DisplayName:Name\' | abpLocalization }}</label\n        ><input type="text" id="name" class="form-control" formControlName="name" />\n      </div>\n    </div>\n    <div class="col col-md-6">\n      <div class="form-group">\n        <label for="surname">{{ \'AbpIdentity::DisplayName:Surname\' | abpLocalization }}</label\n        ><input type="text" id="surname" class="form-control" formControlName="surname" />\n      </div>\n    </div>\n  </div>\n  <div class="form-group">\n    <label for="email-address">{{ \'AbpIdentity::DisplayName:Email\' | abpLocalization }}</label\n    ><span> * </span><input type="text" id="email-address" class="form-control" formControlName="email" />\n  </div>\n  <div class="form-group">\n    <label for="phone-number">{{ \'AbpIdentity::DisplayName:PhoneNumber\' | abpLocalization }}</label\n    ><input type="text" id="phone-number" class="form-control" formControlName="phoneNumber" />\n  </div>\n  <abp-button\n    buttonType="submit"\n    iconClass="fa fa-check"\n    buttonClass="btn btn-primary color-white"\n    [loading]="inProgress"\n  >\n    {{ \'AbpIdentity::Save\' | abpLocalization }}</abp-button\n  >\n</form>\n',
      },
    ],
  },
];
/** @nocollapse */
PersonalSettingsComponent.ctorParameters = () => [{ type: FormBuilder }, { type: Store }, { type: ToasterService }];
tslib_1.__decorate(
  [Select(ProfileState.getProfile), tslib_1.__metadata('design:type', Observable)],
  PersonalSettingsComponent.prototype,
  'profile$',
  void 0,
);
if (false) {
  /** @type {?} */
  PersonalSettingsComponent.prototype.profile$;
  /** @type {?} */
  PersonalSettingsComponent.prototype.form;
  /** @type {?} */
  PersonalSettingsComponent.prototype.inProgress;
  /**
   * @type {?}
   * @private
   */
  PersonalSettingsComponent.prototype.fb;
  /**
   * @type {?}
   * @private
   */
  PersonalSettingsComponent.prototype.store;
  /**
   * @type {?}
   * @private
   */
  PersonalSettingsComponent.prototype.toasterService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVyc29uYWwtc2V0dGluZ3MuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVyc29uYWwtc2V0dGluZ3MvcGVyc29uYWwtc2V0dGluZ3MuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBVyxZQUFZLEVBQUUsYUFBYSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2hGLE9BQU8sRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDbEQsT0FBTyxFQUFFLFdBQVcsRUFBYSxVQUFVLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNwRSxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxJQUFJLEVBQUUsY0FBYyxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2hFLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztNQUVoRCxFQUFFLFNBQVMsRUFBRSxRQUFRLEVBQUUsS0FBSyxFQUFFLEdBQUcsVUFBVTtBQU1qRCxNQUFNLE9BQU8seUJBQXlCOzs7Ozs7SUFRcEMsWUFBb0IsRUFBZSxFQUFVLEtBQVksRUFBVSxjQUE4QjtRQUE3RSxPQUFFLEdBQUYsRUFBRSxDQUFhO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUFHLENBQUM7Ozs7SUFFckcsUUFBUTtRQUNOLElBQUksQ0FBQyxTQUFTLEVBQUUsQ0FBQztJQUNuQixDQUFDOzs7O0lBRUQsU0FBUztRQUNQLElBQUksQ0FBQyxLQUFLO2FBQ1AsUUFBUSxDQUFDLElBQUksVUFBVSxFQUFFLENBQUM7YUFDMUIsSUFBSSxDQUNILGNBQWMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQzdCLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FDUjthQUNBLFNBQVM7Ozs7UUFBQyxDQUFDLENBQUMsRUFBRSxPQUFPLENBQUMsRUFBRSxFQUFFO1lBQ3pCLElBQUksQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUM7Z0JBQ3hCLFFBQVEsRUFBRSxDQUFDLE9BQU8sQ0FBQyxRQUFRLEVBQUUsQ0FBQyxRQUFRLEVBQUUsU0FBUyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7Z0JBQ3hELEtBQUssRUFBRSxDQUFDLE9BQU8sQ0FBQyxLQUFLLEVBQUUsQ0FBQyxRQUFRLEVBQUUsS0FBSyxFQUFFLFNBQVMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO2dCQUN6RCxJQUFJLEVBQUUsQ0FBQyxPQUFPLENBQUMsSUFBSSxJQUFJLEVBQUUsRUFBRSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDO2dCQUMzQyxPQUFPLEVBQUUsQ0FBQyxPQUFPLENBQUMsT0FBTyxJQUFJLEVBQUUsRUFBRSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDO2dCQUNqRCxXQUFXLEVBQUUsQ0FBQyxPQUFPLENBQUMsV0FBVyxJQUFJLEVBQUUsRUFBRSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDO2FBQzFELENBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7OztJQUVELE1BQU07UUFDSixJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFDOUIsSUFBSSxDQUFDLFVBQVUsR0FBRyxJQUFJLENBQUM7UUFDdkIsSUFBSSxDQUFDLEtBQUs7YUFDUCxRQUFRLENBQUMsSUFBSSxhQUFhLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQzthQUM1QyxJQUFJLENBQUMsUUFBUTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsVUFBVSxHQUFHLEtBQUssQ0FBQyxFQUFDLENBQUM7YUFDL0MsU0FBUzs7O1FBQUMsR0FBRyxFQUFFO1lBQ2QsSUFBSSxDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsbUNBQW1DLEVBQUUsU0FBUyxFQUFFLEVBQUUsSUFBSSxFQUFFLElBQUksRUFBRSxDQUFDLENBQUM7UUFDOUYsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7WUE3Q0YsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSw0QkFBNEI7Z0JBQ3RDLDJ2REFBaUQ7YUFDbEQ7Ozs7WUFYUSxXQUFXO1lBQ0gsS0FBSztZQUdiLGNBQWM7O0FBVXJCO0lBREMsTUFBTSxDQUFDLFlBQVksQ0FBQyxVQUFVLENBQUM7c0NBQ3RCLFVBQVU7MkRBQW1COzs7SUFEdkMsNkNBQ3VDOztJQUV2Qyx5Q0FBZ0I7O0lBRWhCLCtDQUFvQjs7Ozs7SUFFUix1Q0FBdUI7Ozs7O0lBQUUsMENBQW9COzs7OztJQUFFLG1EQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEdldFByb2ZpbGUsIFByb2ZpbGUsIFByb2ZpbGVTdGF0ZSwgVXBkYXRlUHJvZmlsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB0YWtlLCB3aXRoTGF0ZXN0RnJvbSwgZmluYWxpemUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcblxuY29uc3QgeyBtYXhMZW5ndGgsIHJlcXVpcmVkLCBlbWFpbCB9ID0gVmFsaWRhdG9ycztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXBlcnNvbmFsLXNldHRpbmdzLWZvcm0nLFxuICB0ZW1wbGF0ZVVybDogJy4vcGVyc29uYWwtc2V0dGluZ3MuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBQZXJzb25hbFNldHRpbmdzQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcbiAgQFNlbGVjdChQcm9maWxlU3RhdGUuZ2V0UHJvZmlsZSlcbiAgcHJvZmlsZSQ6IE9ic2VydmFibGU8UHJvZmlsZS5SZXNwb25zZT47XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIGluUHJvZ3Jlc3M6IGJvb2xlYW47XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIHRvYXN0ZXJTZXJ2aWNlOiBUb2FzdGVyU2VydmljZSkge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLmJ1aWxkRm9ybSgpO1xuICB9XG5cbiAgYnVpbGRGb3JtKCkge1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChuZXcgR2V0UHJvZmlsZSgpKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHdpdGhMYXRlc3RGcm9tKHRoaXMucHJvZmlsZSQpLFxuICAgICAgICB0YWtlKDEpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoWywgcHJvZmlsZV0pID0+IHtcbiAgICAgICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICAgICAgdXNlck5hbWU6IFtwcm9maWxlLnVzZXJOYW1lLCBbcmVxdWlyZWQsIG1heExlbmd0aCgyNTYpXV0sXG4gICAgICAgICAgZW1haWw6IFtwcm9maWxlLmVtYWlsLCBbcmVxdWlyZWQsIGVtYWlsLCBtYXhMZW5ndGgoMjU2KV1dLFxuICAgICAgICAgIG5hbWU6IFtwcm9maWxlLm5hbWUgfHwgJycsIFttYXhMZW5ndGgoNjQpXV0sXG4gICAgICAgICAgc3VybmFtZTogW3Byb2ZpbGUuc3VybmFtZSB8fCAnJywgW21heExlbmd0aCg2NCldXSxcbiAgICAgICAgICBwaG9uZU51bWJlcjogW3Byb2ZpbGUucGhvbmVOdW1iZXIgfHwgJycsIFttYXhMZW5ndGgoMTYpXV0sXG4gICAgICAgIH0pO1xuICAgICAgfSk7XG4gIH1cblxuICBzdWJtaXQoKSB7XG4gICAgaWYgKHRoaXMuZm9ybS5pbnZhbGlkKSByZXR1cm47XG4gICAgdGhpcy5pblByb2dyZXNzID0gdHJ1ZTtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IFVwZGF0ZVByb2ZpbGUodGhpcy5mb3JtLnZhbHVlKSlcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLmluUHJvZ3Jlc3MgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2Uuc3VjY2VzcygnQWJwQWNjb3VudDo6UGVyc29uYWxTZXR0aW5nc1NhdmVkJywgJ1N1Y2Nlc3MnLCB7IGxpZmU6IDUwMDAgfSk7XG4gICAgICB9KTtcbiAgfVxufVxuIl19
