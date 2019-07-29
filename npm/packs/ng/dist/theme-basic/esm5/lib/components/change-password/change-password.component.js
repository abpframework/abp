/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ProfileChangePassword } from '@abp/ng.core';
import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild, } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { comparePasswords, validatePassword } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import { from } from 'rxjs';
import { take } from 'rxjs/operators';
var minLength = Validators.minLength, required = Validators.required;
var ChangePasswordComponent = /** @class */ (function () {
    function ChangePasswordComponent(fb, modalService, store) {
        this.fb = fb;
        this.modalService = modalService;
        this.store = store;
        this.visibleChange = new EventEmitter();
    }
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
        function () { return _this.modalRef.close(); }));
    };
    /**
     * @return {?}
     */
    ChangePasswordComponent.prototype.openModal = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.modalRef = this.modalService.open(this.modalContent);
        this.visibleChange.emit(true);
        from(this.modalRef.result)
            .pipe(take(1))
            .subscribe((/**
         * @param {?} data
         * @return {?}
         */
        function (data) {
            _this.setVisible(false);
        }), (/**
         * @param {?} reason
         * @return {?}
         */
        function (reason) {
            _this.setVisible(false);
        }));
    };
    /**
     * @param {?} value
     * @return {?}
     */
    ChangePasswordComponent.prototype.setVisible = /**
     * @param {?} value
     * @return {?}
     */
    function (value) {
        this.visible = value;
        this.visibleChange.emit(value);
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
        else if (visible.currentValue === false && this.modalService.hasOpenModals()) {
            this.modalRef.close();
        }
    };
    ChangePasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-change-password',
                    template: "<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h4 class=\"modal-title\" id=\"modal-basic-title\">{{ 'AbpIdentity::ChangePassword' | abpLocalization }}</h4>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n    <div class=\"modal-body\">\n      <div class=\"form-group\">\n        <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\n      </div>\n      <div class=\"form-group\" [class.is-invalid]=\"form.errors?.passwordMismatch\">\n        <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\n        ><span> * </span\n        ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\n        <div *ngIf=\"form.errors?.passwordMismatch\" class=\"invalid-feedback\">\n          {{ 'AbpIdentity::Identity.PasswordConfirmationFailed' | abpLocalization }}\n        </div>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n"
                }] }
    ];
    /** @nocollapse */
    ChangePasswordComponent.ctorParameters = function () { return [
        { type: FormBuilder },
        { type: NgbModal },
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
    /** @type {?} */
    ChangePasswordComponent.prototype.visible;
    /** @type {?} */
    ChangePasswordComponent.prototype.visibleChange;
    /** @type {?} */
    ChangePasswordComponent.prototype.modalContent;
    /** @type {?} */
    ChangePasswordComponent.prototype.form;
    /** @type {?} */
    ChangePasswordComponent.prototype.modalRef;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.modalService;
    /**
     * @type {?}
     * @private
     */
    ChangePasswordComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFuZ2UtcGFzc3dvcmQvY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3JELE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBRU4sV0FBVyxFQUNYLFNBQVMsR0FDVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBa0IsUUFBUSxFQUFlLE1BQU0sNEJBQTRCLENBQUM7QUFDbkYsT0FBTyxFQUFFLGdCQUFnQixFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDeEUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsSUFBSSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzVCLE9BQU8sRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUU5QixJQUFBLGdDQUFTLEVBQUUsOEJBQVE7QUFFM0I7SUFrQkUsaUNBQW9CLEVBQWUsRUFBVSxZQUFzQixFQUFVLEtBQVk7UUFBckUsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLGlCQUFZLEdBQVosWUFBWSxDQUFVO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQVR6RixrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFXLENBQUM7SUFTZ0QsQ0FBQzs7OztJQUU3RiwwQ0FBUTs7O0lBQVI7UUFDRSxJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUN2QjtZQUNFLFFBQVEsRUFBRSxDQUFD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QkFFeEIsS0FBSzs7OzBCQVdYLEtBQUs7Z0NBR0wsTUFBTTsrQkFHTixTQUFTLFNBQUMsY0FBYyxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRTs7SUFpRTlDLDhCQUFDO0NBQUEsQUE1RUQsSUE0RUM7U0F4RVksdUJBQXVCOzs7SUFDbEMsMENBQ2lCOztJQUVqQixnREFDNEM7O0lBRTVDLCtDQUMrQjs7SUFFL0IsdUNBQWdCOztJQUVoQiwyQ0FBc0I7Ozs7O0lBRVYscUNBQXVCOzs7OztJQUFFLCtDQUE4Qjs7Ozs7SUFBRSx3Q0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBQcm9maWxlQ2hhbmdlUGFzc3dvcmQgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHtcbiAgQ29tcG9uZW50LFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkNoYW5nZXMsXG4gIE9uSW5pdCxcbiAgT3V0cHV0LFxuICBTaW1wbGVDaGFuZ2VzLFxuICBUZW1wbGF0ZVJlZixcbiAgVmlld0NoaWxkLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBOZ2JBY3RpdmVNb2RhbCwgTmdiTW9kYWwsIE5nYk1vZGFsUmVmIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgY29tcGFyZVBhc3N3b3JkcywgdmFsaWRhdGVQYXNzd29yZCB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IGZyb20gfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbmNvbnN0IHsgbWluTGVuZ3RoLCByZXF1aXJlZCB9ID0gVmFsaWRhdG9ycztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWNoYW5nZS1wYXNzd29yZCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCwgT25DaGFuZ2VzIHtcbiAgQElucHV0KClcbiAgdmlzaWJsZTogYm9vbGVhbjtcblxuICBAT3V0cHV0KClcbiAgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcblxuICBAVmlld0NoaWxkKCdtb2RhbENvbnRlbnQnLCB7IHN0YXRpYzogZmFsc2UgfSlcbiAgbW9kYWxDb250ZW50OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIGZvcm06IEZvcm1Hcm91cDtcblxuICBtb2RhbFJlZjogTmdiTW9kYWxSZWY7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgbW9kYWxTZXJ2aWNlOiBOZ2JNb2RhbCwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7XG4gICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cChcbiAgICAgIHtcbiAgICAgICAgcGFzc3dvcmQ6IFsnJywgW3JlcXVpcmVkLCBtaW5MZW5ndGgoNiksIHZhbGlkYXRlUGFzc3dvcmQoWydzbWFsbCcsICdjYXBpdGFsJywgJ251bWJlcicsICdzcGVjaWFsJ10pXV0sXG4gICAgICAgIG5ld1Bhc3N3b3JkOiBbJycsIFtyZXF1aXJlZCwgbWluTGVuZ3RoKDYpLCB2YWxpZGF0ZVBhc3N3b3JkKFsnc21hbGwnLCAnY2FwaXRhbCcsICdudW1iZXInLCAnc3BlY2lhbCddKV1dLFxuICAgICAgICByZXBlYXROZXdQYXNzd29yZDogWycnLCBbcmVxdWlyZWQsIG1pbkxlbmd0aCg2KSwgdmFsaWRhdGVQYXNzd29yZChbJ3NtYWxsJywgJ2NhcGl0YWwnLCAnbnVtYmVyJywgJ3NwZWNpYWwnXSldXSxcbiAgICAgIH0sXG4gICAgICB7XG4gICAgICAgIHZhbGlkYXRvcnM6IFtjb21wYXJlUGFzc3dvcmRzKFsnbmV3UGFzc3dvcmQnLCAncmVwZWF0TmV3UGFzc3dvcmQnXSldLFxuICAgICAgfSxcbiAgICApO1xuICB9XG5cbiAgb25TdWJtaXQoKSB7XG4gICAgaWYgKHRoaXMuZm9ybS5pbnZhbGlkKSByZXR1cm47XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBQcm9maWxlQ2hhbmdlUGFzc3dvcmQoe1xuICAgICAgICAgIGN1cnJlbnRQYXNzd29yZDogdGhpcy5mb3JtLmdldCgncGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgICAgICBuZXdQYXNzd29yZDogdGhpcy5mb3JtLmdldCgnbmV3UGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHRoaXMubW9kYWxSZWYuY2xvc2UoKSk7XG4gIH1cblxuICBvcGVuTW9kYWwoKSB7XG4gICAgdGhpcy5tb2RhbFJlZiA9IHRoaXMubW9kYWxTZXJ2aWNlLm9wZW4odGhpcy5tb2RhbENvbnRlbnQpO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHRydWUpO1xuXG4gICAgZnJvbSh0aGlzLm1vZGFsUmVmLnJlc3VsdClcbiAgICAgIC5waXBlKHRha2UoMSkpXG4gICAgICAuc3Vic2NyaWJlKFxuICAgICAgICBkYXRhID0+IHtcbiAgICAgICAgICB0aGlzLnNldFZpc2libGUoZmFsc2UpO1xuICAgICAgICB9LFxuICAgICAgICByZWFzb24gPT4ge1xuICAgICAgICAgIHRoaXMuc2V0VmlzaWJsZShmYWxzZSk7XG4gICAgICAgIH0sXG4gICAgICApO1xuICB9XG5cbiAgc2V0VmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIHRoaXMudmlzaWJsZSA9IHZhbHVlO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgfVxuXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XG4gICAgaWYgKCF2aXNpYmxlKSByZXR1cm47XG5cbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcbiAgICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy5tb2RhbFNlcnZpY2UuaGFzT3Blbk1vZGFscygpKSB7XG4gICAgICB0aGlzLm1vZGFsUmVmLmNsb3NlKCk7XG4gICAgfVxuICB9XG59XG4iXX0=