/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild, } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store, Select } from '@ngxs/store';
import { from, Observable } from 'rxjs';
import { take, withLatestFrom } from 'rxjs/operators';
import { ProfileGet, ProfileState, ProfileUpdate } from '@abp/ng.core';
const { maxLength, required, email } = Validators;
export class ProfileComponent {
    /**
     * @param {?} fb
     * @param {?} modalService
     * @param {?} store
     */
    constructor(fb, modalService, store) {
        this.fb = fb;
        this.modalService = modalService;
        this.store = store;
        this.visibleChange = new EventEmitter();
    }
    /**
     * @return {?}
     */
    buildForm() {
        this.store
            .dispatch(new ProfileGet())
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
        this.store.dispatch(new ProfileUpdate(this.form.value)).subscribe((/**
         * @return {?}
         */
        () => this.modalRef.close()));
    }
    /**
     * @return {?}
     */
    openModal() {
        this.buildForm();
        this.modalRef = this.modalService.open(this.modalContent);
        this.visibleChange.emit(true);
        from(this.modalRef.result)
            .pipe(take(1))
            .subscribe((/**
         * @param {?} data
         * @return {?}
         */
        data => {
            this.setVisible(false);
        }), (/**
         * @param {?} reason
         * @return {?}
         */
        reason => {
            this.setVisible(false);
        }));
    }
    /**
     * @param {?} value
     * @return {?}
     */
    setVisible(value) {
        this.visible = value;
        this.visibleChange.emit(value);
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
        else if (visible.currentValue === false && this.modalService.hasOpenModals()) {
            this.modalRef.close();
        }
    }
}
ProfileComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-profile',
                template: "<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h4 class=\"modal-title\" id=\"modal-basic-title\">{{ 'AbpIdentity::PersonalInfo' | abpLocalization }}</h4>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form *ngIf=\"form\" [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n    <div class=\"modal-body\">\n      <div class=\"form-group\">\n        <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"username\" class=\"form-control\" formControlName=\"userName\" />\n      </div>\n      <div class=\"row\">\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\n            ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n          </div>\n        </div>\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\n            ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n          </div>\n        </div>\n      </div>\n      <div class=\"form-group\">\n        <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:EmailAddress' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\n        ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n"
            }] }
];
/** @nocollapse */
ProfileComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: NgbModal },
    { type: Store }
];
ProfileComponent.propDecorators = {
    visible: [{ type: Input }],
    visibleChange: [{ type: Output }],
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};
tslib_1.__decorate([
    Select(ProfileState.getProfile),
    tslib_1.__metadata("design:type", Observable)
], ProfileComponent.prototype, "profile$", void 0);
if (false) {
    /** @type {?} */
    ProfileComponent.prototype.visible;
    /** @type {?} */
    ProfileComponent.prototype.visibleChange;
    /** @type {?} */
    ProfileComponent.prototype.modalContent;
    /** @type {?} */
    ProfileComponent.prototype.profile$;
    /** @type {?} */
    ProfileComponent.prototype.form;
    /** @type {?} */
    ProfileComponent.prototype.modalRef;
    /**
     * @type {?}
     * @private
     */
    ProfileComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    ProfileComponent.prototype.modalService;
    /**
     * @type {?}
     * @private
     */
    ProfileComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcHJvZmlsZS9wcm9maWxlLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBRU4sV0FBVyxFQUNYLFNBQVMsR0FDVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxRQUFRLEVBQWUsTUFBTSw0QkFBNEIsQ0FBQztBQUNuRSxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsSUFBSSxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUN4QyxPQUFPLEVBQUUsSUFBSSxFQUFFLGNBQWMsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxVQUFVLEVBQUUsWUFBWSxFQUFXLGFBQWEsRUFBRSxNQUFNLGNBQWMsQ0FBQztNQUUxRSxFQUFFLFNBQVMsRUFBRSxRQUFRLEVBQUUsS0FBSyxFQUFFLEdBQUcsVUFBVTtBQU1qRCxNQUFNLE9BQU8sZ0JBQWdCOzs7Ozs7SUFpQjNCLFlBQW9CLEVBQWUsRUFBVSxZQUFzQixFQUFVLEtBQVk7UUFBckUsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLGlCQUFZLEdBQVosWUFBWSxDQUFVO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQVp6RixrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFXLENBQUM7SUFZZ0QsQ0FBQzs7OztJQUU3RixTQUFTO1FBQ1AsSUFBSSxDQUFDLEtBQUs7YUFDUCxRQUFRLENBQUMsSUFBSSxVQUFVLEVBQUUsQ0FBQzthQUMxQixJQUFJLENBQ0gs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QUNSLEtBQUs7OztzQkFZWCxLQUFLOzRCQUdMLE1BQU07MkJBR04sU0FBUyxTQUFDLGNBQWMsRUFBRSxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUU7O0FBSTVDO0lBREMsTUFBTSxDQUFDLFlBQVksQ0FBQyxVQUFVLENBQUM7c0NBQ3RCLFVBQVU7a0RBQW1COzs7SUFWdkMsbUNBQ2lCOztJQUVqQix5Q0FDNEM7O0lBRTVDLHdDQUMrQjs7SUFFL0Isb0NBQ3VDOztJQUV2QyxnQ0FBZ0I7O0lBRWhCLG9DQUFzQjs7Ozs7SUFFViw4QkFBdUI7Ozs7O0lBQUUsd0NBQThCOzs7OztJQUFFLGlDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIENvbXBvbmVudCxcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25DaGFuZ2VzLFxuICBPbkluaXQsXG4gIE91dHB1dCxcbiAgU2ltcGxlQ2hhbmdlcyxcbiAgVGVtcGxhdGVSZWYsXG4gIFZpZXdDaGlsZCxcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgTmdiTW9kYWwsIE5nYk1vZGFsUmVmIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgU3RvcmUsIFNlbGVjdCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IGZyb20sIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHRha2UsIHdpdGhMYXRlc3RGcm9tIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgUHJvZmlsZUdldCwgUHJvZmlsZVN0YXRlLCBQcm9maWxlLCBQcm9maWxlVXBkYXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuY29uc3QgeyBtYXhMZW5ndGgsIHJlcXVpcmVkLCBlbWFpbCB9ID0gVmFsaWRhdG9ycztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXByb2ZpbGUnLFxuICB0ZW1wbGF0ZVVybDogJy4vcHJvZmlsZS5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFByb2ZpbGVDb21wb25lbnQgaW1wbGVtZW50cyBPbkNoYW5nZXMge1xuICBASW5wdXQoKVxuICB2aXNpYmxlOiBib29sZWFuO1xuXG4gIEBPdXRwdXQoKVxuICB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBWaWV3Q2hpbGQoJ21vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtb2RhbENvbnRlbnQ6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQFNlbGVjdChQcm9maWxlU3RhdGUuZ2V0UHJvZmlsZSlcbiAgcHJvZmlsZSQ6IE9ic2VydmFibGU8UHJvZmlsZS5SZXNwb25zZT47XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIG1vZGFsUmVmOiBOZ2JNb2RhbFJlZjtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGZiOiBGb3JtQnVpbGRlciwgcHJpdmF0ZSBtb2RhbFNlcnZpY2U6IE5nYk1vZGFsLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBidWlsZEZvcm0oKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBQcm9maWxlR2V0KCkpXG4gICAgICAucGlwZShcbiAgICAgICAgd2l0aExhdGVzdEZyb20odGhpcy5wcm9maWxlJCksXG4gICAgICAgIHRha2UoMSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKChbLCBwcm9maWxlXSkgPT4ge1xuICAgICAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgICAgICB1c2VyTmFtZTogW3Byb2ZpbGUudXNlck5hbWUsIFtyZXF1aXJlZCwgbWF4TGVuZ3RoKDI1NildXSxcbiAgICAgICAgICBlbWFpbDogW3Byb2ZpbGUuZW1haWwsIFtyZXF1aXJlZCwgZW1haWwsIG1heExlbmd0aCgyNTYpXV0sXG4gICAgICAgICAgbmFtZTogW3Byb2ZpbGUubmFtZSB8fCAnJywgW21heExlbmd0aCg2NCldXSxcbiAgICAgICAgICBzdXJuYW1lOiBbcHJvZmlsZS5zdXJuYW1lIHx8ICcnLCBbbWF4TGVuZ3RoKDY0KV1dLFxuICAgICAgICAgIHBob25lTnVtYmVyOiBbcHJvZmlsZS5waG9uZU51bWJlciB8fCAnJywgW21heExlbmd0aCgxNildXSxcbiAgICAgICAgfSk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uU3VibWl0KCkge1xuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xuXG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgUHJvZmlsZVVwZGF0ZSh0aGlzLmZvcm0udmFsdWUpKS5zdWJzY3JpYmUoKCkgPT4gdGhpcy5tb2RhbFJlZi5jbG9zZSgpKTtcbiAgfVxuXG4gIG9wZW5Nb2RhbCgpIHtcbiAgICB0aGlzLmJ1aWxkRm9ybSgpO1xuXG4gICAgdGhpcy5tb2RhbFJlZiA9IHRoaXMubW9kYWxTZXJ2aWNlLm9wZW4odGhpcy5tb2RhbENvbnRlbnQpO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHRydWUpO1xuXG4gICAgZnJvbSh0aGlzLm1vZGFsUmVmLnJlc3VsdClcbiAgICAgIC5waXBlKHRha2UoMSkpXG4gICAgICAuc3Vic2NyaWJlKFxuICAgICAgICBkYXRhID0+IHtcbiAgICAgICAgICB0aGlzLnNldFZpc2libGUoZmFsc2UpO1xuICAgICAgICB9LFxuICAgICAgICByZWFzb24gPT4ge1xuICAgICAgICAgIHRoaXMuc2V0VmlzaWJsZShmYWxzZSk7XG4gICAgICAgIH0sXG4gICAgICApO1xuICB9XG5cbiAgc2V0VmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIHRoaXMudmlzaWJsZSA9IHZhbHVlO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgfVxuXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XG4gICAgaWYgKCF2aXNpYmxlKSByZXR1cm47XG5cbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcbiAgICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy5tb2RhbFNlcnZpY2UuaGFzT3Blbk1vZGFscygpKSB7XG4gICAgICB0aGlzLm1vZGFsUmVmLmNsb3NlKCk7XG4gICAgfVxuICB9XG59XG4iXX0=