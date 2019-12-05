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
                template: "<form validateOnSubmit *ngIf=\"form\" [formGroup]=\"form\" (ngSubmit)=\"submit()\">\n  <div class=\"form-group\">\n    <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\n    ><span> * </span\n    ><input\n      type=\"text\"\n      id=\"username\"\n      class=\"form-control\"\n      formControlName=\"userName\"\n      autofocus\n      (keydown.space)=\"$event.preventDefault()\"\n    />\n  </div>\n  <div class=\"row\">\n    <div class=\"col col-md-6\">\n      <div class=\"form-group\">\n        <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\n        ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n      </div>\n    </div>\n    <div class=\"col col-md-6\">\n      <div class=\"form-group\">\n        <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\n        ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n      </div>\n    </div>\n  </div>\n  <div class=\"form-group\">\n    <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:Email' | abpLocalization }}</label\n    ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\n  </div>\n  <div class=\"form-group\">\n    <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\n    ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n  </div>\n  <abp-button\n    buttonType=\"submit\"\n    iconClass=\"fa fa-check\"\n    buttonClass=\"btn btn-primary color-white\"\n    [loading]=\"inProgress\"\n    [disabled]=\"form?.invalid || form?.pristine\"\n  >\n    {{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n  >\n</form>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVyc29uYWwtc2V0dGluZ3MuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVyc29uYWwtc2V0dGluZ3MvcGVyc29uYWwtc2V0dGluZ3MuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQVcsWUFBWSxFQUFFLGFBQWEsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRixPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEVBQUUsSUFBSSxFQUFFLGNBQWMsRUFBRSxRQUFRLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7TUFFaEQsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxHQUFHLFVBQVU7QUFNakQsTUFBTSxPQUFPLHlCQUF5Qjs7Ozs7O0lBUXBDLFlBQW9CLEVBQWUsRUFBVSxLQUFZLEVBQVUsY0FBOEI7UUFBN0UsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7SUFBRyxDQUFDOzs7O0lBRXJHLFFBQVE7UUFDTixJQUFJLENBQUMsU0FBUyxFQUFFLENBQUM7SUFDbkIsQ0FBQzs7OztJQUVELFNBQVM7UUFDUCxJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDO2FBQzFCLElBQUksQ0FDSCxjQUFjLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUM3QixJQUFJLENBQUMsQ0FBQyxDQUFDLENBQ1I7YUFDQSxTQUFTOzs7O1FBQUMsQ0FBQyxDQUFDLEVBQUUsT0FBTyxDQUFDLEVBQUUsRUFBRTtZQUN6QixJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDO2dCQUN4QixRQUFRLEVBQUUsQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUMsUUFBUSxFQUFFLFNBQVMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO2dCQUN4RCxLQUFLLEVBQUUsQ0FBQyxPQUFPLENBQUMsS0FBSyxFQUFFLENBQUMsUUFBUSxFQUFFLEtBQUssRUFBRSxTQUFTLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztnQkFDekQsSUFBSSxFQUFFLENBQUMsT0FBTyxDQUFDLElBQUksSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztnQkFDM0MsT0FBTyxFQUFFLENBQUMsT0FBTyxDQUFDLE9BQU8sSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztnQkFDakQsV0FBVyxFQUFFLENBQUMsT0FBTyxDQUFDLFdBQVcsSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQzthQUMxRCxDQUFDLENBQUM7UUFDTCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCxNQUFNO1FBQ0osSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU87WUFBRSxPQUFPO1FBQzlCLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDO1FBQ3ZCLElBQUksQ0FBQyxLQUFLO2FBQ1AsUUFBUSxDQUFDLElBQUksYUFBYSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7YUFDNUMsSUFBSSxDQUFDLFFBQVE7OztRQUFDLEdBQUcsRUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLFVBQVUsR0FBRyxLQUFLLENBQUMsRUFBQyxDQUFDO2FBQy9DLFNBQVM7OztRQUFDLEdBQUcsRUFBRTtZQUNkLElBQUksQ0FBQyxjQUFjLENBQUMsT0FBTyxDQUFDLG1DQUFtQyxFQUFFLFNBQVMsRUFBRSxFQUFFLElBQUksRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDO1FBQzlGLENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7O1lBN0NGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsNEJBQTRCO2dCQUN0QyxxekRBQWlEO2FBQ2xEOzs7O1lBWFEsV0FBVztZQUNILEtBQUs7WUFHYixjQUFjOztBQVVyQjtJQURDLE1BQU0sQ0FBQyxZQUFZLENBQUMsVUFBVSxDQUFDO3NDQUN0QixVQUFVOzJEQUFtQjs7O0lBRHZDLDZDQUN1Qzs7SUFFdkMseUNBQWdCOztJQUVoQiwrQ0FBb0I7Ozs7O0lBRVIsdUNBQXVCOzs7OztJQUFFLDBDQUFvQjs7Ozs7SUFBRSxtREFBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBHZXRQcm9maWxlLCBQcm9maWxlLCBQcm9maWxlU3RhdGUsIFVwZGF0ZVByb2ZpbGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdGFrZSwgd2l0aExhdGVzdEZyb20sIGZpbmFsaXplIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgVG9hc3RlclNlcnZpY2UgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5cbmNvbnN0IHsgbWF4TGVuZ3RoLCByZXF1aXJlZCwgZW1haWwgfSA9IFZhbGlkYXRvcnM7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1wZXJzb25hbC1zZXR0aW5ncy1mb3JtJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3BlcnNvbmFsLXNldHRpbmdzLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUGVyc29uYWxTZXR0aW5nc0NvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIEBTZWxlY3QoUHJvZmlsZVN0YXRlLmdldFByb2ZpbGUpXG4gIHByb2ZpbGUkOiBPYnNlcnZhYmxlPFByb2ZpbGUuUmVzcG9uc2U+O1xuXG4gIGZvcm06IEZvcm1Hcm91cDtcblxuICBpblByb2dyZXNzOiBib29sZWFuO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSB0b2FzdGVyU2VydmljZTogVG9hc3RlclNlcnZpY2UpIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgdGhpcy5idWlsZEZvcm0oKTtcbiAgfVxuXG4gIGJ1aWxkRm9ybSgpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFByb2ZpbGUoKSlcbiAgICAgIC5waXBlKFxuICAgICAgICB3aXRoTGF0ZXN0RnJvbSh0aGlzLnByb2ZpbGUkKSxcbiAgICAgICAgdGFrZSgxKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKFssIHByb2ZpbGVdKSA9PiB7XG4gICAgICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgICAgIHVzZXJOYW1lOiBbcHJvZmlsZS51c2VyTmFtZSwgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMjU2KV1dLFxuICAgICAgICAgIGVtYWlsOiBbcHJvZmlsZS5lbWFpbCwgW3JlcXVpcmVkLCBlbWFpbCwgbWF4TGVuZ3RoKDI1NildXSxcbiAgICAgICAgICBuYW1lOiBbcHJvZmlsZS5uYW1lIHx8ICcnLCBbbWF4TGVuZ3RoKDY0KV1dLFxuICAgICAgICAgIHN1cm5hbWU6IFtwcm9maWxlLnN1cm5hbWUgfHwgJycsIFttYXhMZW5ndGgoNjQpXV0sXG4gICAgICAgICAgcGhvbmVOdW1iZXI6IFtwcm9maWxlLnBob25lTnVtYmVyIHx8ICcnLCBbbWF4TGVuZ3RoKDE2KV1dLFxuICAgICAgICB9KTtcbiAgICAgIH0pO1xuICB9XG5cbiAgc3VibWl0KCkge1xuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xuICAgIHRoaXMuaW5Qcm9ncmVzcyA9IHRydWU7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBVcGRhdGVQcm9maWxlKHRoaXMuZm9ybS52YWx1ZSkpXG4gICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiAodGhpcy5pblByb2dyZXNzID0gZmFsc2UpKSlcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLnN1Y2Nlc3MoJ0FicEFjY291bnQ6OlBlcnNvbmFsU2V0dGluZ3NTYXZlZCcsICdTdWNjZXNzJywgeyBsaWZlOiA1MDAwIH0pO1xuICAgICAgfSk7XG4gIH1cbn1cbiJdfQ==