/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { GetProfile, ProfileState, UpdateProfile } from '@abp/ng.core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { take, withLatestFrom } from 'rxjs/operators';
const { maxLength, required, email } = Validators;
export class ProfileComponent {
    /**
     * @param {?} fb
     * @param {?} store
     */
    constructor(fb, store) {
        this.fb = fb;
        this.store = store;
        this.visibleChange = new EventEmitter();
        this.modalBusy = false;
    }
    /**
     * @return {?}
     */
    get visible() {
        return this._visible;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set visible(value) {
        this._visible = value;
        this.visibleChange.emit(value);
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
    onSubmit() {
        if (this.form.invalid)
            return;
        this.modalBusy = true;
        this.store.dispatch(new UpdateProfile(this.form.value)).subscribe((/**
         * @return {?}
         */
        () => {
            this.modalBusy = false;
            this.visible = false;
            this.form.reset();
        }));
    }
    /**
     * @return {?}
     */
    openModal() {
        this.buildForm();
        this.visible = true;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    ngOnChanges({ visible }) {
        if (!visible)
            return;
        if (visible.currentValue) {
            this.openModal();
        }
        else if (visible.currentValue === false && this.visible) {
            this.visible = false;
        }
    }
}
ProfileComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-profile',
                template: "<abp-modal [(visible)]=\"visible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h4>{{ 'AbpIdentity::PersonalInfo' | abpLocalization }}</h4>\n  </ng-template>\n  <ng-template #abpBody>\n    <form novalidate *ngIf=\"form\" [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\">\n      <div class=\"form-group\">\n        <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"username\" class=\"form-control\" formControlName=\"userName\" autofocus />\n      </div>\n      <div class=\"row\">\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\n            ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n          </div>\n        </div>\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\n            ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n          </div>\n        </div>\n      </div>\n      <div class=\"form-group\">\n        <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:Email' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\n        ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <abp-button iconClass=\"fa fa-check\" (click)=\"onSubmit()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\n  </ng-template>\n</abp-modal>\n"
            }] }
];
/** @nocollapse */
ProfileComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: Store }
];
ProfileComponent.propDecorators = {
    visible: [{ type: Input }],
    visibleChange: [{ type: Output }]
};
tslib_1.__decorate([
    Select(ProfileState.getProfile),
    tslib_1.__metadata("design:type", Observable)
], ProfileComponent.prototype, "profile$", void 0);
if (false) {
    /**
     * @type {?}
     * @protected
     */
    ProfileComponent.prototype._visible;
    /** @type {?} */
    ProfileComponent.prototype.visibleChange;
    /** @type {?} */
    ProfileComponent.prototype.profile$;
    /** @type {?} */
    ProfileComponent.prototype.form;
    /** @type {?} */
    ProfileComponent.prototype.modalBusy;
    /**
     * @type {?}
     * @private
     */
    ProfileComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    ProfileComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcHJvZmlsZS9wcm9maWxlLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBVyxVQUFVLEVBQUUsWUFBWSxFQUFFLGFBQWEsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRixPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQWEsTUFBTSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUNqRyxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLElBQUksRUFBRSxjQUFjLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztNQUVoRCxFQUFFLFNBQVMsRUFBRSxRQUFRLEVBQUUsS0FBSyxFQUFFLEdBQUcsVUFBVTtBQU1qRCxNQUFNLE9BQU8sZ0JBQWdCOzs7OztJQXVCM0IsWUFBb0IsRUFBZSxFQUFVLEtBQVk7UUFBckMsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFUekQsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBTzVDLGNBQVMsR0FBWSxLQUFLLENBQUM7SUFFaUMsQ0FBQzs7OztJQXBCN0QsSUFDSSxPQUFPO1FBQ1QsT0FBTyxJQUFJLENBQUMsUUFBUSxDQUFDO0lBQ3ZCLENBQUM7Ozs7O0lBRUQsSUFBSSxPQUFPLENBQUMsS0FBYztRQUN4QixJQUFJLENBQUMsUUFBUSxHQUFHLEtBQUssQ0FBQztRQUN0QixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUNqQyxDQUFDOzs7O0lBY0QsU0FBUztRQUNQLElBQUksQ0FBQyxLQUFLO2FBQ1AsUUFBUSxDQUFDLElBQUksVUFBVSxFQUFFLENBQUM7YUFDMUIsSUFBSSxDQUNILGNBQWMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQzdCLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FDUjthQUNBLFNBQVM7Ozs7UUFBQyxDQUFDLENBQUMsRUFBRSxPQUFPLENBQUMsRUFBRSxFQUFFO1lBQ3pCLElBQUksQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUM7Z0JBQ3hCLFFBQVEsRUFBRSxDQUFDLE9BQU8sQ0FBQyxRQUFRLEVBQUUsQ0FBQyxRQUFRLEVBQUUsU0FBUyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7Z0JBQ3hELEtBQUssRUFBRSxDQUFDLE9BQU8sQ0FBQyxLQUFLLEVBQUUsQ0FBQyxRQUFRLEVBQUUsS0FBSyxFQUFFLFNBQVMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO2dCQUN6RCxJQUFJLEVBQUUsQ0FBQyxPQUFPLENBQUMsSUFBSSxJQUFJLEVBQUUsRUFBRSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDO2dCQUMzQyxPQUFPLEVBQUUsQ0FBQyxPQUFPLENBQUMsT0FBTyxJQUFJLEVBQUUsRUFBRSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDO2dCQUNqRCxXQUFXLEVBQUUsQ0FBQyxPQUFPLENBQUMsV0FBVyxJQUFJLEVBQUUsRUFBRSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDO2FBQzFELENBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7OztJQUVELFFBQVE7UUFDTixJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFDOUIsSUFBSSxDQUFDLFNBQVMsR0FBRyxJQUFJLENBQUM7UUFFdEIsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxhQUFhLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLFNBQVM7OztRQUFDLEdBQUcsRUFBRTtZQUNyRSxJQUFJLENBQUMsU0FBUyxHQUFHLEtBQUssQ0FBQztZQUN2QixJQUFJLENBQUMsT0FBTyxHQUFHLEtBQUssQ0FBQztZQUNyQixJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUssRUFBRSxDQUFDO1FBQ3BCLENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7OztJQUVELFNBQVM7UUFDUCxJQUFJLENBQUMsU0FBUyxFQUFFLENBQUM7UUFDakIsSUFBSSxDQUFDLE9BQU8sR0FBRyxJQUFJLENBQUM7SUFDdEIsQ0FBQzs7Ozs7SUFFRCxXQUFXLENBQUMsRUFBRSxPQUFPLEVBQWlCO1FBQ3BDLElBQUksQ0FBQyxPQUFPO1lBQUUsT0FBTztRQUVyQixJQUFJLE9BQU8sQ0FBQyxZQUFZLEVBQUU7WUFDeEIsSUFBSSxDQUFDLFNBQVMsRUFBRSxDQUFDO1NBQ2xCO2FBQU0sSUFBSSxPQUFPLENBQUMsWUFBWSxLQUFLLEtBQUssSUFBSSxJQUFJLENBQUMsT0FBTyxFQUFFO1lBQ3pELElBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO1NBQ3RCO0lBQ0gsQ0FBQzs7O1lBdkVGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsYUFBYTtnQkFDdkIsa2tFQUF1QzthQUN4Qzs7OztZQVZRLFdBQVc7WUFDSCxLQUFLOzs7c0JBYW5CLEtBQUs7NEJBVUwsTUFBTTs7QUFJUDtJQURDLE1BQU0sQ0FBQyxZQUFZLENBQUMsVUFBVSxDQUFDO3NDQUN0QixVQUFVO2tEQUFtQjs7Ozs7O0lBaEJ2QyxvQ0FBbUI7O0lBWW5CLHlDQUM0Qzs7SUFFNUMsb0NBQ3VDOztJQUV2QyxnQ0FBZ0I7O0lBRWhCLHFDQUEyQjs7Ozs7SUFFZiw4QkFBdUI7Ozs7O0lBQUUsaUNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUHJvZmlsZSwgR2V0UHJvZmlsZSwgUHJvZmlsZVN0YXRlLCBVcGRhdGVQcm9maWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT25DaGFuZ2VzLCBPdXRwdXQsIFNpbXBsZUNoYW5nZXMgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdGFrZSwgd2l0aExhdGVzdEZyb20gfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbmNvbnN0IHsgbWF4TGVuZ3RoLCByZXF1aXJlZCwgZW1haWwgfSA9IFZhbGlkYXRvcnM7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1wcm9maWxlJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3Byb2ZpbGUuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBQcm9maWxlQ29tcG9uZW50IGltcGxlbWVudHMgT25DaGFuZ2VzIHtcbiAgcHJvdGVjdGVkIF92aXNpYmxlO1xuXG4gIEBJbnB1dCgpXG4gIGdldCB2aXNpYmxlKCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLl92aXNpYmxlO1xuICB9XG5cbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcbiAgICB0aGlzLl92aXNpYmxlID0gdmFsdWU7XG4gICAgdGhpcy52aXNpYmxlQ2hhbmdlLmVtaXQodmFsdWUpO1xuICB9XG5cbiAgQE91dHB1dCgpXG4gIHZpc2libGVDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPGJvb2xlYW4+KCk7XG5cbiAgQFNlbGVjdChQcm9maWxlU3RhdGUuZ2V0UHJvZmlsZSlcbiAgcHJvZmlsZSQ6IE9ic2VydmFibGU8UHJvZmlsZS5SZXNwb25zZT47XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIG1vZGFsQnVzeTogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBidWlsZEZvcm0oKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRQcm9maWxlKCkpXG4gICAgICAucGlwZShcbiAgICAgICAgd2l0aExhdGVzdEZyb20odGhpcy5wcm9maWxlJCksXG4gICAgICAgIHRha2UoMSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKChbLCBwcm9maWxlXSkgPT4ge1xuICAgICAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgICAgICB1c2VyTmFtZTogW3Byb2ZpbGUudXNlck5hbWUsIFtyZXF1aXJlZCwgbWF4TGVuZ3RoKDI1NildXSxcbiAgICAgICAgICBlbWFpbDogW3Byb2ZpbGUuZW1haWwsIFtyZXF1aXJlZCwgZW1haWwsIG1heExlbmd0aCgyNTYpXV0sXG4gICAgICAgICAgbmFtZTogW3Byb2ZpbGUubmFtZSB8fCAnJywgW21heExlbmd0aCg2NCldXSxcbiAgICAgICAgICBzdXJuYW1lOiBbcHJvZmlsZS5zdXJuYW1lIHx8ICcnLCBbbWF4TGVuZ3RoKDY0KV1dLFxuICAgICAgICAgIHBob25lTnVtYmVyOiBbcHJvZmlsZS5waG9uZU51bWJlciB8fCAnJywgW21heExlbmd0aCgxNildXSxcbiAgICAgICAgfSk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uU3VibWl0KCkge1xuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcblxuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFVwZGF0ZVByb2ZpbGUodGhpcy5mb3JtLnZhbHVlKSkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIHRoaXMubW9kYWxCdXN5ID0gZmFsc2U7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICAgIHRoaXMuZm9ybS5yZXNldCgpO1xuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIHRoaXMuYnVpbGRGb3JtKCk7XG4gICAgdGhpcy52aXNpYmxlID0gdHJ1ZTtcbiAgfVxuXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XG4gICAgaWYgKCF2aXNpYmxlKSByZXR1cm47XG5cbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcbiAgICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy52aXNpYmxlKSB7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==