/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ProfileGet, ProfileState, ProfileUpdate } from '@abp/ng.core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { take, withLatestFrom } from 'rxjs/operators';
var maxLength = Validators.maxLength, required = Validators.required, email = Validators.email;
var ProfileComponent = /** @class */ (function () {
    function ProfileComponent(fb, store) {
        this.fb = fb;
        this.store = store;
        this.visibleChange = new EventEmitter();
    }
    Object.defineProperty(ProfileComponent.prototype, "visible", {
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
    ProfileComponent.prototype.buildForm = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.store
            .dispatch(new ProfileGet())
            .pipe(withLatestFrom(this.profile$), take(1))
            .subscribe((/**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var _b = tslib_1.__read(_a, 2), profile = _b[1];
            _this.form = _this.fb.group({
                userName: [profile.userName, [required, maxLength(256)]],
                email: [profile.email, [required, email, maxLength(256)]],
                name: [profile.name || '', [maxLength(64)]],
                surname: [profile.surname || '', [maxLength(64)]],
                phoneNumber: [profile.phoneNumber || '', [maxLength(16)]],
            });
        }));
    };
    /**
     * @return {?}
     */
    ProfileComponent.prototype.onSubmit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.form.invalid)
            return;
        this.store.dispatch(new ProfileUpdate(this.form.value)).subscribe((/**
         * @return {?}
         */
        function () {
            _this.visible = false;
        }));
    };
    /**
     * @return {?}
     */
    ProfileComponent.prototype.openModal = /**
     * @return {?}
     */
    function () {
        this.buildForm();
        this.visible = true;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    ProfileComponent.prototype.ngOnChanges = /**
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
    ProfileComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-profile',
                    template: "<abp-modal *ngIf=\"visible\" [(visible)]=\"visible\">\n  <ng-template #abpHeader>\n    <h4>{{ 'AbpIdentity::PersonalInfo' | abpLocalization }}</h4>\n  </ng-template>\n  <ng-template #abpBody>\n    <form *ngIf=\"form\" [formGroup]=\"form\" novalidate>\n      <div class=\"form-group\">\n        <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"username\" class=\"form-control\" formControlName=\"userName\" />\n      </div>\n      <div class=\"row\">\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\n            ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n          </div>\n        </div>\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\n            ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n          </div>\n        </div>\n      </div>\n      <div class=\"form-group\">\n        <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:Email' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\n        ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"onSubmit()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n"
                }] }
    ];
    /** @nocollapse */
    ProfileComponent.ctorParameters = function () { return [
        { type: FormBuilder },
        { type: Store }
    ]; };
    ProfileComponent.propDecorators = {
        visible: [{ type: Input }],
        visibleChange: [{ type: Output }]
    };
    tslib_1.__decorate([
        Select(ProfileState.getProfile),
        tslib_1.__metadata("design:type", Observable)
    ], ProfileComponent.prototype, "profile$", void 0);
    return ProfileComponent;
}());
export { ProfileComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcHJvZmlsZS9wcm9maWxlLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBVyxVQUFVLEVBQUUsWUFBWSxFQUFFLGFBQWEsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRixPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQWEsTUFBTSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUNqRyxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLElBQUksRUFBRSxjQUFjLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUU5QyxJQUFBLGdDQUFTLEVBQUUsOEJBQVEsRUFBRSx3QkFBSztBQUVsQztJQXlCRSwwQkFBb0IsRUFBZSxFQUFVLEtBQVk7UUFBckMsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFQekQsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO0lBT2dCLENBQUM7SUFsQjdELHNCQUNJLHFDQUFPOzs7O1FBRFg7WUFFRSxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUM7UUFDdkIsQ0FBQzs7Ozs7UUFFRCxVQUFZLEtBQWM7WUFDeEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxLQUFLLENBQUM7WUFDdEIsSUFBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7UUFDakMsQ0FBQzs7O09BTEE7Ozs7SUFpQkQsb0NBQVM7OztJQUFUO1FBQUEsaUJBZ0JDO1FBZkMsSUFBSSxDQUFDLEtBQUs7YUFDUCxRQUFRLENBQUMsSUFBSSxVQUFVLEVBQUUsQ0FBQzthQUMxQixJQUFJLENBQ0gsY0FBYyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsRUFDN0IsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUNSO2FBQ0EsU0FBUzs7OztRQUFDLFVBQUMsRUFBVztnQkFBWCwwQkFBVyxFQUFSLGVBQU87WUFDcEIsS0FBSSxDQUFDLElBQUksR0FBRyxLQUFJLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQztnQkFDeEIsUUFBUSxFQUFFLENBQUMsT0FBTyxDQUFDLFFBQVEsRUFBRSxDQUFDLFFBQVEsRUFBRSxTQUFTLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztnQkFDeEQsS0FBSyxFQUFFLENBQUMsT0FBTyxDQUFDLEtBQUssRUFBRSxDQUFDLFFBQVEsRUFBRSxLQUFLLEVBQUUsU0FBUyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7Z0JBQ3pELElBQUksRUFBRSxDQUFDLE9BQU8sQ0FBQyxJQUFJLElBQUksRUFBRSxFQUFFLENBQUMsU0FBUyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUM7Z0JBQzNDLE9BQU8sRUFBRSxDQUFDLE9BQU8sQ0FBQyxPQUFPLElBQUksRUFBRSxFQUFFLENBQUMsU0FBUyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUM7Z0JBQ2pELFdBQVcsRUFBRSxDQUFDLE9BQU8sQ0FBQyxXQUFXLElBQUksRUFBRSxFQUFFLENBQUMsU0FBUyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUM7YUFDMUQsQ0FBQyxDQUFDO1FBQ0wsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsbUNBQVE7OztJQUFSO1FBQUEsaUJBTUM7UUFMQyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFFOUIsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxhQUFhLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLFNBQVM7OztRQUFDO1lBQ2hFLEtBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO1FBQ3ZCLENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7OztJQUVELG9DQUFTOzs7SUFBVDtRQUNFLElBQUksQ0FBQyxTQUFTLEVBQUUsQ0FBQztRQUNqQixJQUFJLENBQUMsT0FBTyxHQUFHLElBQUksQ0FBQztJQUN0QixDQUFDOzs7OztJQUVELHNDQUFXOzs7O0lBQVgsVUFBWSxFQUEwQjtZQUF4QixvQkFBTztRQUNuQixJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFFckIsSUFBSSxPQUFPLENBQUMsWUFBWSxFQUFFO1lBQ3hCLElBQUksQ0FBQyxTQUFTLEVBQUUsQ0FBQztTQUNsQjthQUFNLElBQUksT0FBTyxDQUFDLFlBQVksS0FBSyxLQUFLLElBQUksSUFBSSxDQUFDLE9BQU8sRUFBRTtZQUN6RCxJQUFJLENBQUMsT0FBTyxHQUFHLEtBQUssQ0FBQztTQUN0QjtJQUNILENBQUM7O2dCQWxFRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGFBQWE7b0JBQ3ZCLGltRUFBdUM7aUJBQ3hDOzs7O2dCQVZRLFdBQVc7Z0JBQ0gsS0FBSzs7OzBCQWFuQixLQUFLO2dDQVVMLE1BQU07O0lBSVA7UUFEQyxNQUFNLENBQUMsWUFBWSxDQUFDLFVBQVUsQ0FBQzswQ0FDdEIsVUFBVTtzREFBbUI7SUE4Q3pDLHVCQUFDO0NBQUEsQUFuRUQsSUFtRUM7U0EvRFksZ0JBQWdCOzs7Ozs7SUFDM0Isb0NBQW1COztJQVluQix5Q0FDNEM7O0lBRTVDLG9DQUN1Qzs7SUFFdkMsZ0NBQWdCOzs7OztJQUVKLDhCQUF1Qjs7Ozs7SUFBRSxpQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBQcm9maWxlLCBQcm9maWxlR2V0LCBQcm9maWxlU3RhdGUsIFByb2ZpbGVVcGRhdGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPbkNoYW5nZXMsIE91dHB1dCwgU2ltcGxlQ2hhbmdlcyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB0YWtlLCB3aXRoTGF0ZXN0RnJvbSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuY29uc3QgeyBtYXhMZW5ndGgsIHJlcXVpcmVkLCBlbWFpbCB9ID0gVmFsaWRhdG9ycztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXByb2ZpbGUnLFxuICB0ZW1wbGF0ZVVybDogJy4vcHJvZmlsZS5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFByb2ZpbGVDb21wb25lbnQgaW1wbGVtZW50cyBPbkNoYW5nZXMge1xuICBwcm90ZWN0ZWQgX3Zpc2libGU7XG5cbiAgQElucHV0KClcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XG4gIH1cblxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gIH1cblxuICBAT3V0cHV0KClcbiAgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcblxuICBAU2VsZWN0KFByb2ZpbGVTdGF0ZS5nZXRQcm9maWxlKVxuICBwcm9maWxlJDogT2JzZXJ2YWJsZTxQcm9maWxlLlJlc3BvbnNlPjtcblxuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGJ1aWxkRm9ybSgpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IFByb2ZpbGVHZXQoKSlcbiAgICAgIC5waXBlKFxuICAgICAgICB3aXRoTGF0ZXN0RnJvbSh0aGlzLnByb2ZpbGUkKSxcbiAgICAgICAgdGFrZSgxKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKFssIHByb2ZpbGVdKSA9PiB7XG4gICAgICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgICAgIHVzZXJOYW1lOiBbcHJvZmlsZS51c2VyTmFtZSwgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMjU2KV1dLFxuICAgICAgICAgIGVtYWlsOiBbcHJvZmlsZS5lbWFpbCwgW3JlcXVpcmVkLCBlbWFpbCwgbWF4TGVuZ3RoKDI1NildXSxcbiAgICAgICAgICBuYW1lOiBbcHJvZmlsZS5uYW1lIHx8ICcnLCBbbWF4TGVuZ3RoKDY0KV1dLFxuICAgICAgICAgIHN1cm5hbWU6IFtwcm9maWxlLnN1cm5hbWUgfHwgJycsIFttYXhMZW5ndGgoNjQpXV0sXG4gICAgICAgICAgcGhvbmVOdW1iZXI6IFtwcm9maWxlLnBob25lTnVtYmVyIHx8ICcnLCBbbWF4TGVuZ3RoKDE2KV1dLFxuICAgICAgICB9KTtcbiAgICAgIH0pO1xuICB9XG5cbiAgb25TdWJtaXQoKSB7XG4gICAgaWYgKHRoaXMuZm9ybS5pbnZhbGlkKSByZXR1cm47XG5cbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBQcm9maWxlVXBkYXRlKHRoaXMuZm9ybS52YWx1ZSkpLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICB9KTtcbiAgfVxuXG4gIG9wZW5Nb2RhbCgpIHtcbiAgICB0aGlzLmJ1aWxkRm9ybSgpO1xuICAgIHRoaXMudmlzaWJsZSA9IHRydWU7XG4gIH1cblxuICBuZ09uQ2hhbmdlcyh7IHZpc2libGUgfTogU2ltcGxlQ2hhbmdlcyk6IHZvaWQge1xuICAgIGlmICghdmlzaWJsZSkgcmV0dXJuO1xuXG4gICAgaWYgKHZpc2libGUuY3VycmVudFZhbHVlKSB7XG4gICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgIH0gZWxzZSBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUgPT09IGZhbHNlICYmIHRoaXMudmlzaWJsZSkge1xuICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG59XG4iXX0=