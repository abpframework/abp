/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangePassword } from '@abp/ng.core';
import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild, } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { comparePasswords } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import snq from 'snq';
import { finalize } from 'rxjs/operators';
import { ToasterService } from '../../services/toaster.service';
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
        this.visibleChange = new EventEmitter();
        this.modalBusy = false;
        this.mapErrorsFn = (/**
         * @param {?} errors
         * @param {?} groupErrors
         * @param {?} control
         * @return {?}
         */
        (errors, groupErrors, control) => {
            if (PASSWORD_FIELDS.indexOf(control.name) < 0)
                return errors;
            return errors.concat(groupErrors.filter((/**
             * @param {?} __0
             * @return {?}
             */
            ({ key }) => key === 'passwordMismatch')));
        });
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
    ngOnInit() {
        this.form = this.fb.group({
            password: ['', required],
            newPassword: ['', required],
            repeatNewPassword: ['', required],
        }, {
            validators: [comparePasswords(PASSWORD_FIELDS)],
        });
    }
    /**
     * @return {?}
     */
    onSubmit() {
        if (this.form.invalid)
            return;
        this.modalBusy = true;
        this.store
            .dispatch(new ChangePassword({
            currentPassword: this.form.get('password').value,
            newPassword: this.form.get('newPassword').value,
        }))
            .pipe(finalize((/**
         * @return {?}
         */
        () => {
            this.modalBusy = false;
        })))
            .subscribe({
            next: (/**
             * @return {?}
             */
            () => {
                this.visible = false;
                this.form.reset();
            }),
            error: (/**
             * @param {?} err
             * @return {?}
             */
            err => {
                this.toasterService.error(snq((/**
                 * @return {?}
                 */
                () => err.error.error.message), 'AbpAccount::DefaultErrorMessage'), 'Error', {
                    life: 7000,
                });
            }),
        });
    }
    /**
     * @return {?}
     */
    openModal() {
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
ChangePasswordComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-change-password',
                template: "<abp-modal [(visible)]=\"visible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h4>{{ 'AbpIdentity::ChangePassword' | abpLocalization }}</h4>\r\n  </ng-template>\r\n  <ng-template #abpBody>\r\n    <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" [mapErrorsFn]=\"mapErrorsFn\">\r\n      <div class=\"form-group\">\r\n        <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\r\n        ><span> * </span\r\n        ><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" autofocus />\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\r\n        ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\r\n        ><span> * </span\r\n        ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #abpFooter>\r\n    <button type=\"button\" class=\"btn btn-secondary color-white\" #abpClose>\r\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" buttonClass=\"btn btn-primary color-white\" (click)=\"onSubmit()\">{{\r\n      'AbpIdentity::Save' | abpLocalization\r\n    }}</abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
            }] }
];
/** @nocollapse */
ChangePasswordComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: Store },
    { type: ToasterService }
];
ChangePasswordComponent.propDecorators = {
    visible: [{ type: Input }],
    visibleChange: [{ type: Output }],
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};
if (false) {
    /**
     * @type {?}
     * @protected
     */
    ChangePasswordComponent.prototype._visible;
    /** @type {?} */
    ChangePasswordComponent.prototype.visibleChange;
    /** @type {?} */
    ChangePasswordComponent.prototype.modalContent;
    /** @type {?} */
    ChangePasswordComponent.prototype.form;
    /** @type {?} */
    ChangePasswordComponent.prototype.modalBusy;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhbmdlLXBhc3N3b3JkLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY2hhbmdlLXBhc3N3b3JkL2NoYW5nZS1wYXNzd29yZC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDOUMsT0FBTyxFQUNMLFNBQVMsRUFDVCxZQUFZLEVBQ1osS0FBSyxFQUdMLE1BQU0sRUFFTixXQUFXLEVBQ1gsU0FBUyxHQUNWLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLGdCQUFnQixFQUFjLE1BQU0sb0JBQW9CLENBQUM7QUFDbEUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQzFDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxnQ0FBZ0MsQ0FBQztNQUUxRCxFQUFFLFNBQVMsRUFBRSxRQUFRLEVBQUUsR0FBRyxVQUFVOztNQUVwQyxlQUFlLEdBQUcsQ0FBQyxhQUFhLEVBQUUsbUJBQW1CLENBQUM7QUFNNUQsTUFBTSxPQUFPLHVCQUF1Qjs7Ozs7O0lBNEJsQyxZQUFvQixFQUFlLEVBQVUsS0FBWSxFQUFVLGNBQThCO1FBQTdFLE9BQUUsR0FBRixFQUFFLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBZjlFLGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQU8vRCxjQUFTLEdBQUcsS0FBSyxDQUFDO1FBRWxCLGdCQUFXOzs7Ozs7UUFBMkIsQ0FB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Qzs7O1lBeEZGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUscUJBQXFCO2dCQUMvQixtcURBQStDO2FBQ2hEOzs7O1lBZFEsV0FBVztZQUVYLEtBQUs7WUFHTCxjQUFjOzs7c0JBYXBCLEtBQUs7NEJBVUwsTUFBTTsyQkFFTixTQUFTLFNBQUMsY0FBYyxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRTs7Ozs7OztJQWQ1QywyQ0FBbUI7O0lBWW5CLGdEQUErRDs7SUFFL0QsK0NBQytCOztJQUUvQix1Q0FBZ0I7O0lBRWhCLDRDQUFrQjs7SUFFbEIsOENBSUU7Ozs7O0lBRVUscUNBQXVCOzs7OztJQUFFLHdDQUFvQjs7Ozs7SUFBRSxpREFBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDaGFuZ2VQYXNzd29yZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7XHJcbiAgQ29tcG9uZW50LFxyXG4gIEV2ZW50RW1pdHRlcixcclxuICBJbnB1dCxcclxuICBPbkNoYW5nZXMsXHJcbiAgT25Jbml0LFxyXG4gIE91dHB1dCxcclxuICBTaW1wbGVDaGFuZ2VzLFxyXG4gIFRlbXBsYXRlUmVmLFxyXG4gIFZpZXdDaGlsZCxcclxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcclxuaW1wb3J0IHsgY29tcGFyZVBhc3N3b3JkcywgVmFsaWRhdGlvbiB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XHJcbmltcG9ydCB7IGZpbmFsaXplIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL3RvYXN0ZXIuc2VydmljZSc7XHJcblxyXG5jb25zdCB7IG1pbkxlbmd0aCwgcmVxdWlyZWQgfSA9IFZhbGlkYXRvcnM7XHJcblxyXG5jb25zdCBQQVNTV09SRF9GSUVMRFMgPSBbJ25ld1Bhc3N3b3JkJywgJ3JlcGVhdE5ld1Bhc3N3b3JkJ107XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1jaGFuZ2UtcGFzc3dvcmQnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50Lmh0bWwnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQ2hhbmdlUGFzc3dvcmRDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQsIE9uQ2hhbmdlcyB7XHJcbiAgcHJvdGVjdGVkIF92aXNpYmxlO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGdldCB2aXNpYmxlKCk6IGJvb2xlYW4ge1xyXG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XHJcbiAgfVxyXG5cclxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xyXG4gICAgdGhpcy5fdmlzaWJsZSA9IHZhbHVlO1xyXG4gICAgdGhpcy52aXNpYmxlQ2hhbmdlLmVtaXQodmFsdWUpO1xyXG4gIH1cclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IHZpc2libGVDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPGJvb2xlYW4+KCk7XHJcblxyXG4gIEBWaWV3Q2hpbGQoJ21vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KVxyXG4gIG1vZGFsQ29udGVudDogVGVtcGxhdGVSZWY8YW55PjtcclxuXHJcbiAgZm9ybTogRm9ybUdyb3VwO1xyXG5cclxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcclxuXHJcbiAgbWFwRXJyb3JzRm46IFZhbGlkYXRpb24uTWFwRXJyb3JzRm4gPSAoZXJyb3JzLCBncm91cEVycm9ycywgY29udHJvbCkgPT4ge1xyXG4gICAgaWYgKFBBU1NXT1JEX0ZJRUxEUy5pbmRleE9mKGNvbnRyb2wubmFtZSkgPCAwKSByZXR1cm4gZXJyb3JzO1xyXG5cclxuICAgIHJldHVybiBlcnJvcnMuY29uY2F0KGdyb3VwRXJyb3JzLmZpbHRlcigoeyBrZXkgfSkgPT4ga2V5ID09PSAncGFzc3dvcmRNaXNtYXRjaCcpKTtcclxuICB9O1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGZiOiBGb3JtQnVpbGRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgdG9hc3RlclNlcnZpY2U6IFRvYXN0ZXJTZXJ2aWNlKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpOiB2b2lkIHtcclxuICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoXHJcbiAgICAgIHtcclxuICAgICAgICBwYXNzd29yZDogWycnLCByZXF1aXJlZF0sXHJcbiAgICAgICAgbmV3UGFzc3dvcmQ6IFsnJywgcmVxdWlyZWRdLFxyXG4gICAgICAgIHJlcGVhdE5ld1Bhc3N3b3JkOiBbJycsIHJlcXVpcmVkXSxcclxuICAgICAgfSxcclxuICAgICAge1xyXG4gICAgICAgIHZhbGlkYXRvcnM6IFtjb21wYXJlUGFzc3dvcmRzKFBBU1NXT1JEX0ZJRUxEUyldLFxyXG4gICAgICB9LFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIG9uU3VibWl0KCkge1xyXG4gICAgaWYgKHRoaXMuZm9ybS5pbnZhbGlkKSByZXR1cm47XHJcbiAgICB0aGlzLm1vZGFsQnVzeSA9IHRydWU7XHJcblxyXG4gICAgdGhpcy5zdG9yZVxyXG4gICAgICAuZGlzcGF0Y2goXHJcbiAgICAgICAgbmV3IENoYW5nZVBhc3N3b3JkKHtcclxuICAgICAgICAgIGN1cnJlbnRQYXNzd29yZDogdGhpcy5mb3JtLmdldCgncGFzc3dvcmQnKS52YWx1ZSxcclxuICAgICAgICAgIG5ld1Bhc3N3b3JkOiB0aGlzLmZvcm0uZ2V0KCduZXdQYXNzd29yZCcpLnZhbHVlLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIGZpbmFsaXplKCgpID0+IHtcclxuICAgICAgICAgIHRoaXMubW9kYWxCdXN5ID0gZmFsc2U7XHJcbiAgICAgICAgfSksXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZSh7XHJcbiAgICAgICAgbmV4dDogKCkgPT4ge1xyXG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XHJcbiAgICAgICAgICB0aGlzLmZvcm0ucmVzZXQoKTtcclxuICAgICAgICB9LFxyXG4gICAgICAgIGVycm9yOiBlcnIgPT4ge1xyXG4gICAgICAgICAgdGhpcy50b2FzdGVyU2VydmljZS5lcnJvcihzbnEoKCkgPT4gZXJyLmVycm9yLmVycm9yLm1lc3NhZ2UsICdBYnBBY2NvdW50OjpEZWZhdWx0RXJyb3JNZXNzYWdlJyksICdFcnJvcicsIHtcclxuICAgICAgICAgICAgbGlmZTogNzAwMCxcclxuICAgICAgICAgIH0pO1xyXG4gICAgICAgIH0sXHJcbiAgICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgb3Blbk1vZGFsKCkge1xyXG4gICAgdGhpcy52aXNpYmxlID0gdHJ1ZTtcclxuICB9XHJcblxyXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XHJcbiAgICBpZiAoIXZpc2libGUpIHJldHVybjtcclxuXHJcbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcclxuICAgICAgdGhpcy5vcGVuTW9kYWwoKTtcclxuICAgIH0gZWxzZSBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUgPT09IGZhbHNlICYmIHRoaXMudmlzaWJsZSkge1xyXG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19