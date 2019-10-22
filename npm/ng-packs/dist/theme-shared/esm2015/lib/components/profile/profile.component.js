/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                phoneNumber: [profile.phoneNumber || '', [maxLength(16)]]
            });
        }));
    }
    /**
     * @return {?}
     */
    submit() {
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
                template: "<abp-modal [(visible)]=\"visible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h4>{{ 'AbpIdentity::PersonalInfo' | abpLocalization }}</h4>\r\n  </ng-template>\r\n  <ng-template #abpBody>\r\n    <form novalidate *ngIf=\"form\" [formGroup]=\"form\" (ngSubmit)=\"submit()\">\r\n      <div class=\"form-group\">\r\n        <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\r\n        ><span> * </span><input type=\"text\" id=\"username\" class=\"form-control\" formControlName=\"userName\" autofocus />\r\n      </div>\r\n      <div class=\"row\">\r\n        <div class=\"col col-md-6\">\r\n          <div class=\"form-group\">\r\n            <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\r\n            ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\r\n          </div>\r\n        </div>\r\n        <div class=\"col col-md-6\">\r\n          <div class=\"form-group\">\r\n            <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\r\n            ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:Email' | abpLocalization }}</label\r\n        ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\r\n        ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary color-white\">\r\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" buttonClass=\"btn btn-primary color-white\" (click)=\"submit()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3Byb2ZpbGUvcHJvZmlsZS5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQVcsVUFBVSxFQUFFLFlBQVksRUFBRSxhQUFhLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDaEYsT0FBTyxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFhLE1BQU0sRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFDakcsT0FBTyxFQUFFLFdBQVcsRUFBYSxVQUFVLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNwRSxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxJQUFJLEVBQUUsY0FBYyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7TUFFaEQsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxHQUFHLFVBQVU7QUFNakQsTUFBTSxPQUFPLGdCQUFnQjs7Ozs7SUFzQjNCLFlBQW9CLEVBQWUsRUFBVSxLQUFZO1FBQXJDLE9BQUUsR0FBRixFQUFFLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBVHRDLGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQU8vRCxjQUFTLEdBQUcsS0FBSyxDQUFDO0lBRTBDLENBQUM7Ozs7SUFuQjdELElBQ0ksT0FBTztRQUNULE9BQU8sSUFBSSxDQUFDLFFBQVEsQ0FBQztJQUN2QixDQUFDOzs7OztJQUVELElBQUksT0FBTyxDQUFDLEtBQWM7UUFDeEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxLQUFLLENBQUM7UUFDdEIsSUFBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7SUFDakMsQ0FBQzs7OztJQWFELFNBQVM7UUFDUCxJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDO2FBQzFCLElBQUksQ0FDSCxjQUFjLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUM3QixJQUFJLENBQUMsQ0FBQyxDQUFDLENBQ1I7YUFDQSxTQUFTOzs7O1FBQUMsQ0FBQyxDQUFDLEVBQUUsT0FBTyxDQUFDLEVBQUUsRUFBRTtZQUN6QixJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDO2dCQUN4QixRQUFRLEVBQUUsQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUMsUUFBUSxFQUFFLFNBQVMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO2dCQUN4RCxLQUFLLEVBQUUsQ0FBQyxPQUFPLENBQUMsS0FBSyxFQUFFLENBQUMsUUFBUSxFQUFFLEtBQUssRUFBRSxTQUFTLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztnQkFDekQsSUFBSSxFQUFFLENBQUMsT0FBTyxDQUFDLElBQUksSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztnQkFDM0MsT0FBTyxFQUFFLENBQUMsT0FBTyxDQUFDLE9BQU8sSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztnQkFDakQsV0FBVyxFQUFFLENBQUMsT0FBTyxDQUFDLFdBQVcsSUFBSSxFQUFFLEVBQUUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQzthQUMxRCxDQUFDLENBQUM7UUFDTCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCxNQUFNO1FBQ0osSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU87WUFBRSxPQUFPO1FBQzlCLElBQUksQ0FBQyxTQUFTLEdBQUcsSUFBSSxDQUFDO1FBRXRCLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksYUFBYSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUU7WUFDckUsSUFBSSxDQUFDLFNBQVMsR0FBRyxLQUFLLENBQUM7WUFDdkIsSUFBSSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUM7WUFDckIsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLEVBQUUsQ0FBQztRQUNwQixDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7Ozs7SUFFRCxTQUFTO1FBQ1AsSUFBSSxDQUFDLFNBQVMsRUFBRSxDQUFDO1FBQ2pCLElBQUksQ0FBQyxPQUFPLEdBQUcsSUFBSSxDQUFDO0lBQ3RCLENBQUM7Ozs7O0lBRUQsV0FBVyxDQUFDLEVBQUUsT0FBTyxFQUFpQjtRQUNwQyxJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFFckIsSUFBSSxPQUFPLENBQUMsWUFBWSxFQUFFO1lBQ3hCLElBQUksQ0FBQyxTQUFTLEVBQUUsQ0FBQztTQUNsQjthQUFNLElBQUksT0FBTyxDQUFDLFlBQVksS0FBSyxLQUFLLElBQUksSUFBSSxDQUFDLE9BQU8sRUFBRTtZQUN6RCxJQUFJLENBQUMsT0FBTyxHQUFHLEtBQUssQ0FBQztTQUN0QjtJQUNILENBQUM7OztZQXRFRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGFBQWE7Z0JBQ3ZCLHdzRUFBdUM7YUFDeEM7Ozs7WUFWUSxXQUFXO1lBQ0gsS0FBSzs7O3NCQWFuQixLQUFLOzRCQVVMLE1BQU07O0FBR1A7SUFEQyxNQUFNLENBQUMsWUFBWSxDQUFDLFVBQVUsQ0FBQztzQ0FDdEIsVUFBVTtrREFBbUI7Ozs7OztJQWZ2QyxvQ0FBbUI7O0lBWW5CLHlDQUErRDs7SUFFL0Qsb0NBQ3VDOztJQUV2QyxnQ0FBZ0I7O0lBRWhCLHFDQUFrQjs7Ozs7SUFFTiw4QkFBdUI7Ozs7O0lBQUUsaUNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUHJvZmlsZSwgR2V0UHJvZmlsZSwgUHJvZmlsZVN0YXRlLCBVcGRhdGVQcm9maWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPbkNoYW5nZXMsIE91dHB1dCwgU2ltcGxlQ2hhbmdlcyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xyXG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IHRha2UsIHdpdGhMYXRlc3RGcm9tIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5cclxuY29uc3QgeyBtYXhMZW5ndGgsIHJlcXVpcmVkLCBlbWFpbCB9ID0gVmFsaWRhdG9ycztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXByb2ZpbGUnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9wcm9maWxlLmNvbXBvbmVudC5odG1sJ1xyXG59KVxyXG5leHBvcnQgY2xhc3MgUHJvZmlsZUNvbXBvbmVudCBpbXBsZW1lbnRzIE9uQ2hhbmdlcyB7XHJcbiAgcHJvdGVjdGVkIF92aXNpYmxlO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGdldCB2aXNpYmxlKCk6IGJvb2xlYW4ge1xyXG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XHJcbiAgfVxyXG5cclxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xyXG4gICAgdGhpcy5fdmlzaWJsZSA9IHZhbHVlO1xyXG4gICAgdGhpcy52aXNpYmxlQ2hhbmdlLmVtaXQodmFsdWUpO1xyXG4gIH1cclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IHZpc2libGVDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPGJvb2xlYW4+KCk7XHJcblxyXG4gIEBTZWxlY3QoUHJvZmlsZVN0YXRlLmdldFByb2ZpbGUpXHJcbiAgcHJvZmlsZSQ6IE9ic2VydmFibGU8UHJvZmlsZS5SZXNwb25zZT47XHJcblxyXG4gIGZvcm06IEZvcm1Hcm91cDtcclxuXHJcbiAgbW9kYWxCdXN5ID0gZmFsc2U7XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgYnVpbGRGb3JtKCkge1xyXG4gICAgdGhpcy5zdG9yZVxyXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFByb2ZpbGUoKSlcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgd2l0aExhdGVzdEZyb20odGhpcy5wcm9maWxlJCksXHJcbiAgICAgICAgdGFrZSgxKVxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoKFssIHByb2ZpbGVdKSA9PiB7XHJcbiAgICAgICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cCh7XHJcbiAgICAgICAgICB1c2VyTmFtZTogW3Byb2ZpbGUudXNlck5hbWUsIFtyZXF1aXJlZCwgbWF4TGVuZ3RoKDI1NildXSxcclxuICAgICAgICAgIGVtYWlsOiBbcHJvZmlsZS5lbWFpbCwgW3JlcXVpcmVkLCBlbWFpbCwgbWF4TGVuZ3RoKDI1NildXSxcclxuICAgICAgICAgIG5hbWU6IFtwcm9maWxlLm5hbWUgfHwgJycsIFttYXhMZW5ndGgoNjQpXV0sXHJcbiAgICAgICAgICBzdXJuYW1lOiBbcHJvZmlsZS5zdXJuYW1lIHx8ICcnLCBbbWF4TGVuZ3RoKDY0KV1dLFxyXG4gICAgICAgICAgcGhvbmVOdW1iZXI6IFtwcm9maWxlLnBob25lTnVtYmVyIHx8ICcnLCBbbWF4TGVuZ3RoKDE2KV1dXHJcbiAgICAgICAgfSk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgc3VibWl0KCkge1xyXG4gICAgaWYgKHRoaXMuZm9ybS5pbnZhbGlkKSByZXR1cm47XHJcbiAgICB0aGlzLm1vZGFsQnVzeSA9IHRydWU7XHJcblxyXG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgVXBkYXRlUHJvZmlsZSh0aGlzLmZvcm0udmFsdWUpKS5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xyXG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcclxuICAgICAgdGhpcy5mb3JtLnJlc2V0KCk7XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIG9wZW5Nb2RhbCgpIHtcclxuICAgIHRoaXMuYnVpbGRGb3JtKCk7XHJcbiAgICB0aGlzLnZpc2libGUgPSB0cnVlO1xyXG4gIH1cclxuXHJcbiAgbmdPbkNoYW5nZXMoeyB2aXNpYmxlIH06IFNpbXBsZUNoYW5nZXMpOiB2b2lkIHtcclxuICAgIGlmICghdmlzaWJsZSkgcmV0dXJuO1xyXG5cclxuICAgIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSkge1xyXG4gICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xyXG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy52aXNpYmxlKSB7XHJcbiAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=