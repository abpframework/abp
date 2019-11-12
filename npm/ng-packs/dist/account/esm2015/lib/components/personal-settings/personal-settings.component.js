/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/personal-settings/personal-settings.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
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
            .pipe(withLatestFrom(this.profile$), take(1))
            .subscribe((/**
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
        }));
    }
    /**
     * @return {?}
     */
    submit() {
        if (this.form.invalid)
            return;
        this.inProgress = true;
        this.store
            .dispatch(new UpdateProfile(this.form.value))
            .pipe(finalize((/**
         * @return {?}
         */
        () => (this.inProgress = false))))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.toasterService.success('AbpAccount::PersonalSettingsSaved', 'Success', { life: 5000 });
        }));
    }
}
PersonalSettingsComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-personal-settings-form',
                template: "<form validateOnSubmit *ngIf=\"form\" [formGroup]=\"form\" (ngSubmit)=\"submit()\">\r\n  <div class=\"form-group\">\r\n    <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\r\n    ><span> * </span\r\n    ><input\r\n      type=\"text\"\r\n      id=\"username\"\r\n      class=\"form-control\"\r\n      formControlName=\"userName\"\r\n      autofocus\r\n      (keydown.space)=\"$event.preventDefault()\"\r\n    />\r\n  </div>\r\n  <div class=\"row\">\r\n    <div class=\"col col-md-6\">\r\n      <div class=\"form-group\">\r\n        <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\r\n        ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\r\n      </div>\r\n    </div>\r\n    <div class=\"col col-md-6\">\r\n      <div class=\"form-group\">\r\n        <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\r\n        ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\r\n      </div>\r\n    </div>\r\n  </div>\r\n  <div class=\"form-group\">\r\n    <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:Email' | abpLocalization }}</label\r\n    ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\r\n  </div>\r\n  <div class=\"form-group\">\r\n    <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\r\n    ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\r\n  </div>\r\n  <abp-button\r\n    buttonType=\"submit\"\r\n    iconClass=\"fa fa-check\"\r\n    buttonClass=\"btn btn-primary color-white\"\r\n    [loading]=\"inProgress\"\r\n    [disabled]=\"form?.invalid || form?.pristine\"\r\n  >\r\n    {{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\r\n  >\r\n</form>\r\n"
            }] }
];
/** @nocollapse */
PersonalSettingsComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: Store },
    { type: ToasterService }
];
tslib_1.__decorate([
    Select(ProfileState.getProfile),
    tslib_1.__metadata("design:type", Observable)
], PersonalSettingsComponent.prototype, "profile$", void 0);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVyc29uYWwtc2V0dGluZ3MuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVyc29uYWwtc2V0dGluZ3MvcGVyc29uYWwtc2V0dGluZ3MuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQVcsWUFBWSxFQUFFLGFBQWEsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRixPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEVBQUUsSUFBSSxFQUFFLGNBQWMsRUFBRSxRQUFRLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7TUFFaEQsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxHQUFHLFVBQVU7QUFNakQsTUFBTSxPQUFPLHlCQUF5Qjs7Ozs7O0lBUXBDLFlBQW9CLEVBQWUsRUFBVSxLQUFZLEVBQVUsY0FBOEI7UUFBN0UsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7SUFBRyxDQUFDOzs7O0lBRXJHLFFBQVE7UUFDTixJQUFJLENBQUMsU0FBUyxFQUFFLENBQUM7SUFDbkIsQ0FBQzs7OztJQUVELFNBQVM7UUFDUCxJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDO2FBQzFCLElBQUksQ0FDSCxjQUFjLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUM3QixJQUFJLENBQUMsQ0FBQyxDQUFDLENBQ1I7YUFDQSxTQUFTOzs7O1FBQUMsQ0FBQyxDQUFDLEVBQUUsT0FBTyxDQUFDLEVBQUUsRUFBRTtZQUN6QixJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDO2dCQUN4QixRQUFRLEVBQUUsQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUMsUUFBUSxFQUFFLFNBQVMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO2dCQUN4RCxLQUFLLEVBQUUsQ0FBQyxPQUFPLENBQUMsS0FBSyxFQUFFLENBQUMsUUFBUSxFQUFFLEtBQUssRUFBRSxTQUFTLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztnQkFDekQsSUFBSSxFQUFFLENBQUMsT0FBTyxDQUFDLElBQUksSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztnQkFDM0MsT0FBTyxFQUFFLENBQUMsT0FBTyxDQUFDLE9BQU8sSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztnQkFDakQsV0FBVyxFQUFFLENBQUMsT0FBTyxDQUFDLFdBQVcsSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQzthQUMxRCxDQUFDLENBQUM7UUFDTCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCxNQUFNO1FBQ0osSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU87WUFBRSxPQUFPO1FBQzlCLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDO1FBQ3ZCLElBQUksQ0FBQyxLQUFLO2FBQ1AsUUFBUSxDQUFDLElBQUksYUFBYSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7YUFDNUMsSUFBSSxDQUFDLFFBQVE7OztRQUFDLEdBQUcsRUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLFVBQVUsR0FBRyxLQUFLLENBQUMsRUFBQyxDQUFDO2FBQy9DLFNBQVM7OztRQUFDLEdBQUcsRUFBRTtZQUNkLElBQUksQ0FBQyxjQUFjLENBQUMsT0FBTyxDQUFDLG1DQUFtQyxFQUFFLFNBQVMsRUFBRSxFQUFFLElBQUksRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDO1FBQzlGLENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7O1lBN0NGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsNEJBQTRCO2dCQUN0QywrNERBQWlEO2FBQ2xEOzs7O1lBWFEsV0FBVztZQUNILEtBQUs7WUFHYixjQUFjOztBQVVyQjtJQURDLE1BQU0sQ0FBQyxZQUFZLENBQUMsVUFBVSxDQUFDO3NDQUN0QixVQUFVOzJEQUFtQjs7O0lBRHZDLDZDQUN1Qzs7SUFFdkMseUNBQWdCOztJQUVoQiwrQ0FBb0I7Ozs7O0lBRVIsdUNBQXVCOzs7OztJQUFFLDBDQUFvQjs7Ozs7SUFBRSxtREFBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBHZXRQcm9maWxlLCBQcm9maWxlLCBQcm9maWxlU3RhdGUsIFVwZGF0ZVByb2ZpbGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xyXG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IHRha2UsIHdpdGhMYXRlc3RGcm9tLCBmaW5hbGl6ZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHsgVG9hc3RlclNlcnZpY2UgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcblxyXG5jb25zdCB7IG1heExlbmd0aCwgcmVxdWlyZWQsIGVtYWlsIH0gPSBWYWxpZGF0b3JzO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtcGVyc29uYWwtc2V0dGluZ3MtZm9ybScsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL3BlcnNvbmFsLXNldHRpbmdzLmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFBlcnNvbmFsU2V0dGluZ3NDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xyXG4gIEBTZWxlY3QoUHJvZmlsZVN0YXRlLmdldFByb2ZpbGUpXHJcbiAgcHJvZmlsZSQ6IE9ic2VydmFibGU8UHJvZmlsZS5SZXNwb25zZT47XHJcblxyXG4gIGZvcm06IEZvcm1Hcm91cDtcclxuXHJcbiAgaW5Qcm9ncmVzczogYm9vbGVhbjtcclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIHRvYXN0ZXJTZXJ2aWNlOiBUb2FzdGVyU2VydmljZSkge31cclxuXHJcbiAgbmdPbkluaXQoKSB7XHJcbiAgICB0aGlzLmJ1aWxkRm9ybSgpO1xyXG4gIH1cclxuXHJcbiAgYnVpbGRGb3JtKCkge1xyXG4gICAgdGhpcy5zdG9yZVxyXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFByb2ZpbGUoKSlcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgd2l0aExhdGVzdEZyb20odGhpcy5wcm9maWxlJCksXHJcbiAgICAgICAgdGFrZSgxKSxcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKChbLCBwcm9maWxlXSkgPT4ge1xyXG4gICAgICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xyXG4gICAgICAgICAgdXNlck5hbWU6IFtwcm9maWxlLnVzZXJOYW1lLCBbcmVxdWlyZWQsIG1heExlbmd0aCgyNTYpXV0sXHJcbiAgICAgICAgICBlbWFpbDogW3Byb2ZpbGUuZW1haWwsIFtyZXF1aXJlZCwgZW1haWwsIG1heExlbmd0aCgyNTYpXV0sXHJcbiAgICAgICAgICBuYW1lOiBbcHJvZmlsZS5uYW1lIHx8ICcnLCBbbWF4TGVuZ3RoKDY0KV1dLFxyXG4gICAgICAgICAgc3VybmFtZTogW3Byb2ZpbGUuc3VybmFtZSB8fCAnJywgW21heExlbmd0aCg2NCldXSxcclxuICAgICAgICAgIHBob25lTnVtYmVyOiBbcHJvZmlsZS5waG9uZU51bWJlciB8fCAnJywgW21heExlbmd0aCgxNildXSxcclxuICAgICAgICB9KTtcclxuICAgICAgfSk7XHJcbiAgfVxyXG5cclxuICBzdWJtaXQoKSB7XHJcbiAgICBpZiAodGhpcy5mb3JtLmludmFsaWQpIHJldHVybjtcclxuICAgIHRoaXMuaW5Qcm9ncmVzcyA9IHRydWU7XHJcbiAgICB0aGlzLnN0b3JlXHJcbiAgICAgIC5kaXNwYXRjaChuZXcgVXBkYXRlUHJvZmlsZSh0aGlzLmZvcm0udmFsdWUpKVxyXG4gICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiAodGhpcy5pblByb2dyZXNzID0gZmFsc2UpKSlcclxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgICAgdGhpcy50b2FzdGVyU2VydmljZS5zdWNjZXNzKCdBYnBBY2NvdW50OjpQZXJzb25hbFNldHRpbmdzU2F2ZWQnLCAnU3VjY2VzcycsIHsgbGlmZTogNTAwMCB9KTtcclxuICAgICAgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==