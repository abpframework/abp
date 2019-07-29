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
const { minLength, required } = Validators;
export class ChangePasswordComponent {
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
    ngOnInit() {
        this.form = this.fb.group({
            password: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
            newPassword: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
            repeatNewPassword: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
        }, {
            validators: [comparePasswords(['newPassword', 'repeatNewPassword'])],
        });
    }
    /**
     * @return {?}
     */
    onSubmit() {
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
        () => this.modalRef.close()));
    }
    /**
     * @return {?}
     */
    openModal() {
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
ChangePasswordComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-change-password',
                template: "<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h4 class=\"modal-title\" id=\"modal-basic-title\">{{ 'AbpIdentity::ChangePassword' | abpLocalization }}</h4>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n    <div class=\"modal-body\">\n      <div class=\"form-group\">\n        <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\n      </div>\n      <div class=\"form-group\" [class.is-invalid]=\"form.errors?.passwordMismatch\">\n        <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\n        ><span> * </span\n        ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\n        <div *ngIf=\"form.errors?.passwordMismatch\" class=\"invalid-feedback\">\n          {{ 'AbpIdentity::Identity.PasswordConfirmationFailed' | abpLocalization }}\n        </div>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n"
            }] }
];
/** @nocollapse */
ChangePasswordComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: NgbModal },
    { type: Store }
];
ChangePasswordComponent.propDecorators = {
    visible: [{ type: Input }],
    visibleChange: [{ type: Output }],
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFuZ2UtcGFzc3dvcmQvY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3JELE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBRU4sV0FBVyxFQUNYLFNBQVMsR0FDVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBa0IsUUFBUSxFQUFlLE1BQU0sNEJBQTRCLENBQUM7QUFDbkYsT0FBTyxFQUFFLGdCQUFnQixFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDeEUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsSUFBSSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzVCLE9BQU8sRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztNQUVoQyxFQUFFLFNBQVMsRUFBRSxRQUFRLEVBQUUsR0FBRyxVQUFVO0FBTTFDLE1BQU0sT0FBTyx1QkFBdUI7Ozs7OztJQWNsQyxZQUFvQixFQUFlLEVBQVUsWUFBc0IsRUFBVSxLQUFZO1FBQXJFLE9BQUUsR0FBRixFQUFFLENBQWE7UUFBVSxpQkFBWSxHQUFaLFlBQVksQ0FBVTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFUekYsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO0lBU2dELENBQUM7Ozs7SUFFN0YsUUFBUTtRQUNOLElBQUksQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEVBQUUsQ0FBQyxLQUFL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TTsyQkFHTixTQUFTLFNBQUMsY0FBYyxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRTs7OztJQU41QywwQ0FDaUI7O0lBRWpCLGdEQUM0Qzs7SUFFNUMsK0NBQytCOztJQUUvQix1Q0FBZ0I7O0lBRWhCLDJDQUFzQjs7Ozs7SUFFVixxQ0FBdUI7Ozs7O0lBQUUsK0NBQThCOzs7OztJQUFFLHdDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFByb2ZpbGVDaGFuZ2VQYXNzd29yZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQge1xuICBDb21wb25lbnQsXG4gIEV2ZW50RW1pdHRlcixcbiAgSW5wdXQsXG4gIE9uQ2hhbmdlcyxcbiAgT25Jbml0LFxuICBPdXRwdXQsXG4gIFNpbXBsZUNoYW5nZXMsXG4gIFRlbXBsYXRlUmVmLFxuICBWaWV3Q2hpbGQsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IE5nYkFjdGl2ZU1vZGFsLCBOZ2JNb2RhbCwgTmdiTW9kYWxSZWYgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBjb21wYXJlUGFzc3dvcmRzLCB2YWxpZGF0ZVBhc3N3b3JkIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgZnJvbSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuY29uc3QgeyBtaW5MZW5ndGgsIHJlcXVpcmVkIH0gPSBWYWxpZGF0b3JzO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtY2hhbmdlLXBhc3N3b3JkJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2NoYW5nZS1wYXNzd29yZC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIENoYW5nZVBhc3N3b3JkQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0LCBPbkNoYW5nZXMge1xuICBASW5wdXQoKVxuICB2aXNpYmxlOiBib29sZWFuO1xuXG4gIEBPdXRwdXQoKVxuICB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBWaWV3Q2hpbGQoJ21vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtb2RhbENvbnRlbnQ6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIG1vZGFsUmVmOiBOZ2JNb2RhbFJlZjtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGZiOiBGb3JtQnVpbGRlciwgcHJpdmF0ZSBtb2RhbFNlcnZpY2U6IE5nYk1vZGFsLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKFxuICAgICAge1xuICAgICAgICBwYXNzd29yZDogWycnLCBbcmVxdWlyZWQsIG1pbkxlbmd0aCg2KSwgdmFsaWRhdGVQYXNzd29yZChbJ3NtYWxsJywgJ2NhcGl0YWwnLCAnbnVtYmVyJywgJ3NwZWNpYWwnXSldXSxcbiAgICAgICAgbmV3UGFzc3dvcmQ6IFsnJywgW3JlcXVpcmVkLCBtaW5MZW5ndGgoNiksIHZhbGlkYXRlUGFzc3dvcmQoWydzbWFsbCcsICdjYXBpdGFsJywgJ251bWJlcicsICdzcGVjaWFsJ10pXV0sXG4gICAgICAgIHJlcGVhdE5ld1Bhc3N3b3JkOiBbJycsIFtyZXF1aXJlZCwgbWluTGVuZ3RoKDYpLCB2YWxpZGF0ZVBhc3N3b3JkKFsnc21hbGwnLCAnY2FwaXRhbCcsICdudW1iZXInLCAnc3BlY2lhbCddKV1dLFxuICAgICAgfSxcbiAgICAgIHtcbiAgICAgICAgdmFsaWRhdG9yczogW2NvbXBhcmVQYXNzd29yZHMoWyduZXdQYXNzd29yZCcsICdyZXBlYXROZXdQYXNzd29yZCddKV0sXG4gICAgICB9LFxuICAgICk7XG4gIH1cblxuICBvblN1Ym1pdCgpIHtcbiAgICBpZiAodGhpcy5mb3JtLmludmFsaWQpIHJldHVybjtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgbmV3IFByb2ZpbGVDaGFuZ2VQYXNzd29yZCh7XG4gICAgICAgICAgY3VycmVudFBhc3N3b3JkOiB0aGlzLmZvcm0uZ2V0KCdwYXNzd29yZCcpLnZhbHVlLFxuICAgICAgICAgIG5ld1Bhc3N3b3JkOiB0aGlzLmZvcm0uZ2V0KCduZXdQYXNzd29yZCcpLnZhbHVlLFxuICAgICAgICB9KSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4gdGhpcy5tb2RhbFJlZi5jbG9zZSgpKTtcbiAgfVxuXG4gIG9wZW5Nb2RhbCgpIHtcbiAgICB0aGlzLm1vZGFsUmVmID0gdGhpcy5tb2RhbFNlcnZpY2Uub3Blbih0aGlzLm1vZGFsQ29udGVudCk7XG4gICAgdGhpcy52aXNpYmxlQ2hhbmdlLmVtaXQodHJ1ZSk7XG5cbiAgICBmcm9tKHRoaXMubW9kYWxSZWYucmVzdWx0KVxuICAgICAgLnBpcGUodGFrZSgxKSlcbiAgICAgIC5zdWJzY3JpYmUoXG4gICAgICAgIGRhdGEgPT4ge1xuICAgICAgICAgIHRoaXMuc2V0VmlzaWJsZShmYWxzZSk7XG4gICAgICAgIH0sXG4gICAgICAgIHJlYXNvbiA9PiB7XG4gICAgICAgICAgdGhpcy5zZXRWaXNpYmxlKGZhbHNlKTtcbiAgICAgICAgfSxcbiAgICAgICk7XG4gIH1cblxuICBzZXRWaXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgdGhpcy52aXNpYmxlID0gdmFsdWU7XG4gICAgdGhpcy52aXNpYmxlQ2hhbmdlLmVtaXQodmFsdWUpO1xuICB9XG5cbiAgbmdPbkNoYW5nZXMoeyB2aXNpYmxlIH06IFNpbXBsZUNoYW5nZXMpOiB2b2lkIHtcbiAgICBpZiAoIXZpc2libGUpIHJldHVybjtcblxuICAgIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSkge1xuICAgICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgICB9IGVsc2UgaWYgKHZpc2libGUuY3VycmVudFZhbHVlID09PSBmYWxzZSAmJiB0aGlzLm1vZGFsU2VydmljZS5oYXNPcGVuTW9kYWxzKCkpIHtcbiAgICAgIHRoaXMubW9kYWxSZWYuY2xvc2UoKTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==