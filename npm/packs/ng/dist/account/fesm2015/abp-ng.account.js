import { ConfigState, ConfigGetAppConfiguration, DynamicLayoutComponent, CoreModule } from '@abp/ng.core';
import { Component, Optional, Inject, NgModule, ViewChild, InjectionToken } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from } from 'rxjs';
import { validatePassword, NgxValidateCoreModule } from '@ngx-validate/core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgbModal, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { TableModule } from 'primeng/table';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const { maxLength, minLength, required } = Validators;
class LoginComponent {
    /**
     * @param {?} fb
     * @param {?} oauthService
     * @param {?} store
     * @param {?} options
     */
    constructor(fb, oauthService, store, options) {
        this.fb = fb;
        this.oauthService = oauthService;
        this.store = store;
        this.options = options;
        this.oauthService.configure(this.store.selectSnapshot(ConfigState.getOne('environment')).oAuthConfig);
        this.oauthService.loadDiscoveryDocument();
        this.form = this.fb.group({
            username: ['', [required, maxLength(255)]],
            password: ['', [required, maxLength(32)]],
            remember: [false],
        });
    }
    /**
     * @return {?}
     */
    onSubmit() {
        if (this.form.invalid)
            return;
        this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);
        from(this.oauthService.fetchTokenUsingPasswordFlow(this.form.get('username').value, this.form.get('password').value)).subscribe({
            next: (/**
             * @return {?}
             */
            () => {
                /** @type {?} */
                const redirectUrl = window.history.state.redirectUrl || this.options.redirectUrl;
                this.store
                    .dispatch(new ConfigGetAppConfiguration())
                    .subscribe((/**
                 * @return {?}
                 */
                () => this.store.dispatch(new Navigate([redirectUrl || '/']))));
            }),
            error: (/**
             * @return {?}
             */
            () => console.error('an error occured')),
        });
    }
}
LoginComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-login',
                template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Login' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"login-input-user-name-or-email-address\">{{\n            'AbpAccount::UserNameOrEmailAddress' | abpLocalization\n          }}</label>\n          <input\n            class=\"form-control\"\n            type=\"text\"\n            id=\"login-input-user-name-or-email-address\"\n            formControlName=\"username\"\n          />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\n          <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\n        </div>\n        <div class=\"form-check\" validationTarget validationStyle>\n          <label class=\"form-check-label\" for=\"login-input-remember-me\">\n            <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\n            {{ 'AbpAccount::RememberMe' | abpLocalization }}\n          </label>\n        </div>\n        <div class=\"mt-2\">\n          <button type=\"button\" name=\"Action\" value=\"Cancel\" class=\"btn btn-secondary\">\n            {{ 'AbpAccount::Cancel' | abpLocalization }}\n          </button>\n          <button type=\"submit\" name=\"Action\" value=\"Login\" class=\"btn btn-primary ml-1\">\n            {{ 'AbpAccount::Login' | abpLocalization }}\n          </button>\n        </div>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
LoginComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: OAuthService },
    { type: Store },
    { type: undefined, decorators: [{ type: Optional }, { type: Inject, args: ['ACCOUNT_OPTIONS',] }] }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const { maxLength: maxLength$1, minLength: minLength$1, required: required$1, email } = Validators;
class RegisterComponent {
    /**
     * @param {?} fb
     * @param {?} oauthService
     * @param {?} router
     */
    constructor(fb, oauthService, router) {
        this.fb = fb;
        this.oauthService = oauthService;
        this.router = router;
        this.form = this.fb.group({
            username: ['', [required$1, maxLength$1(255)]],
            password: [
                '',
                [required$1, maxLength$1(32), minLength$1(6), validatePassword(['small', 'capital', 'number', 'special'])],
            ],
            email: ['', [required$1, email]],
        });
    }
    /**
     * @return {?}
     */
    onSubmit() {
        if (this.form.invalid)
            return;
    }
}
RegisterComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-register',
                template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>Register</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"input-user-name\">User name</label><span> * </span\n          ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-email-address\">Email address</label><span> * </span\n          ><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-password\">Password</label><span> * </span\n          ><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n        </div>\n        <button type=\"submit\" class=\"btn btn-primary\">Register</button>\n      </form>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
RegisterComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: OAuthService },
    { type: Router }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
const routes = [
    { path: '', pathMatch: 'full', redirectTo: 'login' },
    {
        path: '',
        component: DynamicLayoutComponent,
        children: [{ path: 'login', component: LoginComponent }, { path: 'register', component: RegisterComponent }],
    },
];
class AccountRoutingModule {
}
AccountRoutingModule.decorators = [
    { type: NgModule, args: [{
                imports: [RouterModule.forChild(routes)],
                exports: [RouterModule],
            },] }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class TenantBoxComponent {
    /**
     * @param {?} modalService
     * @param {?} fb
     */
    constructor(modalService, fb) {
        this.modalService = modalService;
        this.fb = fb;
    }
    /**
     * @return {?}
     */
    createForm() {
        this.form = this.fb.group({
            name: [this.selected.name],
        });
    }
    /**
     * @return {?}
     */
    openModal() {
        this.createForm();
        this.modalService.open(this.modalContent);
    }
    /**
     * @return {?}
     */
    onSwitch() {
        this.selected = (/** @type {?} */ ({}));
        this.openModal();
    }
    /**
     * @return {?}
     */
    save() {
        this.selected = this.form.value;
        this.modalService.dismissAll();
    }
}
TenantBoxComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenant-box',
                template: "<div\n  class=\"tenant-switch-box\"\n  style=\"background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;\"\n>\n  <span style=\"color: #666;\">{{ 'AbpUiMultiTenancy::Tenant' | abpLocalization }}: </span>\n  <strong>\n    <i>{{ selected?.name ? selected.name : ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}</i>\n  </strong>\n  (<a id=\"abp-tenant-switch-link\" style=\"color: #333; cursor: pointer\" (click)=\"onSwitch()\">{{\n    'AbpUiMultiTenancy::Switch' | abpLocalization\n  }}</a\n  >)\n</div>\n\n<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      SwitchTenant\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n    <div class=\"modal-body\">\n      <div class=\"mt-2\">\n        <div class=\"form-group\">\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\n          <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n        </div>\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n"
            }] }
];
/** @nocollapse */
TenantBoxComponent.ctorParameters = () => [
    { type: NgbModal },
    { type: FormBuilder }
];
TenantBoxComponent.propDecorators = {
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @param {?} options
 * @return {?}
 */
function optionsFactory(options) {
    return Object.assign({ redirectUrl: '/' }, options);
}
/** @type {?} */
const ACCOUNT_OPTIONS = new InjectionToken('ACCOUNT_OPTIONS');

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class AccountModule {
    /**
     * @param {?=} options
     * @return {?}
     */
    static forRoot(options = (/** @type {?} */ ({}))) {
        return {
            ngModule: AccountModule,
            providers: [
                { provide: ACCOUNT_OPTIONS, useValue: options },
                {
                    provide: 'ACCOUNT_OPTIONS',
                    useFactory: optionsFactory,
                    deps: [ACCOUNT_OPTIONS],
                },
            ],
        };
    }
}
AccountModule.decorators = [
    { type: NgModule, args: [{
                declarations: [LoginComponent, RegisterComponent, TenantBoxComponent],
                imports: [CoreModule, AccountRoutingModule, ThemeSharedModule, TableModule, NgbDropdownModule, NgxValidateCoreModule],
                exports: [],
            },] }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
const ACCOUNT_ROUTES = (/** @type {?} */ ([
    {
        name: 'Account',
        path: 'account',
        invisible: true,
        layout: "application" /* application */,
        children: [{ path: 'login', name: 'Login', order: 1 }, { path: 'register', name: 'Register', order: 2 }],
    },
]));

export { ACCOUNT_OPTIONS, ACCOUNT_ROUTES, AccountModule, LoginComponent, RegisterComponent, optionsFactory, LoginComponent as ɵa, RegisterComponent as ɵc, TenantBoxComponent as ɵd, AccountRoutingModule as ɵe, optionsFactory as ɵf, ACCOUNT_OPTIONS as ɵg };
//# sourceMappingURL=abp-ng.account.js.map
