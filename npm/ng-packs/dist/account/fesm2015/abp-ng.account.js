import { ConfigState, GetAppConfiguration, RestService, DynamicLayoutComponent, ChangePassword, GetProfile, UpdateProfile, ProfileState, SessionState, SetTenant, CoreModule } from '@abp/ng.core';
import { ToasterService, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, Optional, Inject, Injectable, ɵɵdefineInjectable, ɵɵinject, NgModule, InjectionToken } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { comparePasswords, NgxValidateCoreModule } from '@ngx-validate/core';
import { TableModule } from 'primeng/table';
import { RouterModule } from '@angular/router';
import { Validators, FormBuilder } from '@angular/forms';
import { Navigate } from '@ngxs/router-plugin';
import { Store, Select } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, throwError, Observable } from 'rxjs';
import { switchMap, tap, catchError, finalize, take, withLatestFrom } from 'rxjs/operators';
import snq from 'snq';
import { __decorate, __metadata } from 'tslib';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const { maxLength, minLength, required } = Validators;
class LoginComponent {
    /**
     * @param {?} fb
     * @param {?} oauthService
     * @param {?} store
     * @param {?} toasterService
     * @param {?} options
     */
    constructor(fb, oauthService, store, toasterService, options) {
        this.fb = fb;
        this.oauthService = oauthService;
        this.store = store;
        this.toasterService = toasterService;
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
        // this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);
        this.inProgress = true;
        from(this.oauthService.fetchTokenUsingPasswordFlow(this.form.get('username').value, this.form.get('password').value))
            .pipe(switchMap((/**
         * @return {?}
         */
        () => this.store.dispatch(new GetAppConfiguration()))), tap((/**
         * @return {?}
         */
        () => {
            /** @type {?} */
            const redirectUrl = snq((/**
             * @return {?}
             */
            () => window.history.state)).redirectUrl || (this.options || {}).redirectUrl || '/';
            this.store.dispatch(new Navigate([redirectUrl]));
        })), catchError((/**
         * @param {?} err
         * @return {?}
         */
        err => {
            this.toasterService.error(snq((/**
             * @return {?}
             */
            () => err.error.error_description)) ||
                snq((/**
                 * @return {?}
                 */
                () => err.error.error.message), 'AbpAccount::DefaultErrorMessage'), 'Error', { life: 7000 });
            return throwError(err);
        })), finalize((/**
         * @return {?}
         */
        () => (this.inProgress = false))))
            .subscribe();
    }
}
LoginComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-login',
                template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Login' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"login-input-user-name-or-email-address\">{{\n            'AbpAccount::UserNameOrEmailAddress' | abpLocalization\n          }}</label>\n          <input\n            class=\"form-control\"\n            type=\"text\"\n            id=\"login-input-user-name-or-email-address\"\n            formControlName=\"username\"\n            autofocus\n          />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\n          <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\n        </div>\n        <div class=\"form-check\" validationTarget validationStyle>\n          <label class=\"form-check-label\" for=\"login-input-remember-me\">\n            <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\n            {{ 'AbpAccount::RememberMe' | abpLocalization }}\n          </label>\n        </div>\n        <div class=\"mt-2\">\n          <abp-button [loading]=\"inProgress\" type=\"submit\">\n            {{ 'AbpAccount::Login' | abpLocalization }}\n          </abp-button>\n        </div>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
LoginComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: OAuthService },
    { type: Store },
    { type: ToasterService },
    { type: undefined, decorators: [{ type: Optional }, { type: Inject, args: ['ACCOUNT_OPTIONS',] }] }
];
if (false) {
    /** @type {?} */
    LoginComponent.prototype.form;
    /** @type {?} */
    LoginComponent.prototype.inProgress;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.oauthService;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.toasterService;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.options;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class ManageProfileComponent {
    constructor() {
        this.selectedTab = 0;
    }
    /**
     * @return {?}
     */
    ngOnInit() { }
}
ManageProfileComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-manage-profile',
                template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\"></div>\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\"></div>\n  <div class=\"col\"></div>\n</div>\n\n<div id=\"ManageProfileWrapper\">\n  <div class=\"row\">\n    <div class=\"col-3\">\n      <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n        <li class=\"nav-item pointer\" (click)=\"selectedTab = 0\">\n          <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 0 }\" role=\"tab\">{{\n            'AbpUi::ChangePassword' | abpLocalization\n          }}</a>\n        </li>\n        <li class=\"nav-item pointer\" (click)=\"selectedTab = 1\">\n          <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 1 }\" role=\"tab\">{{\n            'AbpAccount::PersonalSettings' | abpLocalization\n          }}</a>\n        </li>\n      </ul>\n    </div>\n    <div class=\"col-9\">\n      <div class=\"tab-content\" *ngIf=\"selectedTab === 0\">\n        <div class=\"tab-pane fade show active\" role=\"tabpanel\">\n          <abp-change-password-form></abp-change-password-form>\n        </div>\n      </div>\n      <div class=\"tab-content\" *ngIf=\"selectedTab === 1\">\n        <div class=\"tab-pane fade show active\" role=\"tabpanel\">\n          <abp-personal-settings-form></abp-personal-settings-form>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
ManageProfileComponent.ctorParameters = () => [];
if (false) {
    /** @type {?} */
    ManageProfileComponent.prototype.selectedTab;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class AccountService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @param {?} tenantName
     * @return {?}
     */
    findTenant(tenantName) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/abp/multi-tenancy/tenants/by-name/${tenantName}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    register(body) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: '/api/account/register',
            body,
        };
        return this.rest.request(request, { skipHandleError: true });
    }
}
AccountService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
AccountService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ AccountService.ngInjectableDef = ɵɵdefineInjectable({ factory: function AccountService_Factory() { return new AccountService(ɵɵinject(RestService)); }, token: AccountService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    AccountService.prototype.rest;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const { maxLength: maxLength$1, minLength: minLength$1, required: required$1, email } = Validators;
class RegisterComponent {
    /**
     * @param {?} fb
     * @param {?} accountService
     * @param {?} oauthService
     * @param {?} store
     * @param {?} toasterService
     */
    constructor(fb, accountService, oauthService, store, toasterService) {
        this.fb = fb;
        this.accountService = accountService;
        this.oauthService = oauthService;
        this.store = store;
        this.toasterService = toasterService;
        this.oauthService.configure(this.store.selectSnapshot(ConfigState.getOne('environment')).oAuthConfig);
        this.oauthService.loadDiscoveryDocument();
        this.form = this.fb.group({
            username: ['', [required$1, maxLength$1(255)]],
            password: ['', [required$1, maxLength$1(32)]],
            email: ['', [required$1, email]],
        });
    }
    /**
     * @return {?}
     */
    onSubmit() {
        if (this.form.invalid)
            return;
        this.inProgress = true;
        /** @type {?} */
        const newUser = (/** @type {?} */ ({
            userName: this.form.get('username').value,
            password: this.form.get('password').value,
            emailAddress: this.form.get('email').value,
            appName: 'Angular',
        }));
        this.accountService
            .register(newUser)
            .pipe(switchMap((/**
         * @return {?}
         */
        () => from(this.oauthService.fetchTokenUsingPasswordFlow(newUser.userName, newUser.password)))), switchMap((/**
         * @return {?}
         */
        () => this.store.dispatch(new GetAppConfiguration()))), tap((/**
         * @return {?}
         */
        () => this.store.dispatch(new Navigate(['/'])))), take(1), catchError((/**
         * @param {?} err
         * @return {?}
         */
        err => {
            this.toasterService.error(snq((/**
             * @return {?}
             */
            () => err.error.error_description)) ||
                snq((/**
                 * @return {?}
                 */
                () => err.error.error.message), 'AbpAccount::DefaultErrorMessage'), 'Error', { life: 7000 });
            return throwError(err);
        })), finalize((/**
         * @return {?}
         */
        () => (this.inProgress = false))))
            .subscribe();
    }
}
RegisterComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-register',
                template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Register' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"input-user-name\">{{ 'AbpAccount::UserName' | abpLocalization }}</label\n          ><span> * </span\n          ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-email-address\">{{ 'AbpAccount::EmailAddress' | abpLocalization }}</label\n          ><span> * </span><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label\n          ><span> * </span><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n        </div>\n        <abp-button [loading]=\"inProgress\" type=\"submit\">\n          {{ 'AbpAccount::Register' | abpLocalization }}\n        </abp-button>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/login\">{{ 'AbpAccount::Login' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
RegisterComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: AccountService },
    { type: OAuthService },
    { type: Store },
    { type: ToasterService }
];
if (false) {
    /** @type {?} */
    RegisterComponent.prototype.form;
    /** @type {?} */
    RegisterComponent.prototype.inProgress;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.accountService;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.oauthService;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.toasterService;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
const routes = [
    { path: '', pathMatch: 'full', redirectTo: 'login' },
    {
        path: '',
        component: DynamicLayoutComponent,
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            {
                path: 'manage-profile',
                component: ManageProfileComponent,
            },
        ],
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
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const { minLength: minLength$2, required: required$2 } = Validators;
/** @type {?} */
const PASSWORD_FIELDS = ['newPassword', 'repeatNewPassword'];
class ChangePasswordComponent {
    /**
     * @param {?} fb
     * @param {?} store
     * @param {?} toasterService
     */
    constructor(fb, store, toasterService) {
        this.fb = fb;
        this.store = store;
        this.toasterService = toasterService;
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
    ngOnInit() {
        this.form = this.fb.group({
            password: ['', required$2],
            newPassword: ['', required$2],
            repeatNewPassword: ['', required$2],
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
        this.store
            .dispatch(new ChangePassword({
            currentPassword: this.form.get('password').value,
            newPassword: this.form.get('newPassword').value,
        }))
            .subscribe({
            next: (/**
             * @return {?}
             */
            () => {
                this.form.reset();
                this.toasterService.success('AbpAccount::PasswordChangedMessage', 'Success', { life: 5000 });
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
}
ChangePasswordComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-change-password-form',
                template: "<form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" [mapErrorsFn]=\"mapErrorsFn\">\n  <div class=\"form-group\">\n    <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\n    ><span> * </span\n    ><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" autofocus />\n  </div>\n  <div class=\"form-group\">\n    <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\n    ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\n  </div>\n  <div class=\"form-group\">\n    <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\n    ><span> * </span\n    ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\n  </div>\n  <abp-button iconClass=\"fa fa-check\" buttonClass=\"btn btn-primary color-white\" (click)=\"onSubmit()\">{{\n    'AbpIdentity::Save' | abpLocalization\n  }}</abp-button>\n</form>\n"
            }] }
];
/** @nocollapse */
ChangePasswordComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: Store },
    { type: ToasterService }
];
if (false) {
    /** @type {?} */
    ChangePasswordComponent.prototype.form;
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

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const { maxLength: maxLength$2, required: required$3, email: email$1 } = Validators;
class PersonalSettingsComponent {
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
                userName: [profile.userName, [required$3, maxLength$2(256)]],
                email: [profile.email, [required$3, email$1, maxLength$2(256)]],
                name: [profile.name || '', [maxLength$2(64)]],
                surname: [profile.surname || '', [maxLength$2(64)]],
                phoneNumber: [profile.phoneNumber || '', [maxLength$2(16)]],
            });
        }));
    }
    /**
     * @return {?}
     */
    submit() {
        if (this.form.invalid)
            return;
        this.store.dispatch(new UpdateProfile(this.form.value)).subscribe((/**
         * @return {?}
         */
        () => {
            this.toasterService.success('AbpAccount::PersonalSettingsSaved', 'Success', { life: 5000 });
        }));
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.buildForm();
    }
}
PersonalSettingsComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-personal-settings-form',
                template: "<form novalidate *ngIf=\"form\" [formGroup]=\"form\" (ngSubmit)=\"submit()\">\n  <div class=\"form-group\">\n    <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\n    ><span> * </span><input type=\"text\" id=\"username\" class=\"form-control\" formControlName=\"userName\" autofocus />\n  </div>\n  <div class=\"row\">\n    <div class=\"col col-md-6\">\n      <div class=\"form-group\">\n        <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\n        ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n      </div>\n    </div>\n    <div class=\"col col-md-6\">\n      <div class=\"form-group\">\n        <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\n        ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n      </div>\n    </div>\n  </div>\n  <div class=\"form-group\">\n    <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:Email' | abpLocalization }}</label\n    ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\n  </div>\n  <div class=\"form-group\">\n    <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\n    ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n  </div>\n  <abp-button iconClass=\"fa fa-check\" buttonClass=\"btn btn-primary color-white\" (click)=\"submit()\">\n    {{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n  >\n</form>\n"
            }] }
];
/** @nocollapse */
PersonalSettingsComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: Store },
    { type: ToasterService }
];
__decorate([
    Select(ProfileState.getProfile),
    __metadata("design:type", Observable)
], PersonalSettingsComponent.prototype, "profile$", void 0);
if (false) {
    /** @type {?} */
    PersonalSettingsComponent.prototype.profile$;
    /** @type {?} */
    PersonalSettingsComponent.prototype.form;
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

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class TenantBoxComponent {
    /**
     * @param {?} store
     * @param {?} toasterService
     * @param {?} accountService
     */
    constructor(store, toasterService, accountService) {
        this.store = store;
        this.toasterService = toasterService;
        this.accountService = accountService;
        this.tenant = (/** @type {?} */ ({}));
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.tenant =
            this.store.selectSnapshot(SessionState.getTenant) ||
                ((/** @type {?} */ ({})));
        this.tenantName = this.tenant.name || '';
    }
    /**
     * @return {?}
     */
    onSwitch() {
        this.isModalVisible = true;
    }
    /**
     * @return {?}
     */
    save() {
        if (this.tenant.name) {
            this.accountService
                .findTenant(this.tenant.name)
                .pipe(take(1), catchError((/**
             * @param {?} err
             * @return {?}
             */
            err => {
                this.toasterService.error(snq((/**
                 * @return {?}
                 */
                () => err.error.error_description), 'AbpUi::DefaultErrorMessage'), 'AbpUi::Error');
                return throwError(err);
            })))
                .subscribe((/**
             * @param {?} __0
             * @return {?}
             */
            ({ success, tenantId }) => {
                if (success) {
                    this.tenant = {
                        id: tenantId,
                        name: this.tenant.name
                    };
                    this.tenantName = this.tenant.name;
                    this.isModalVisible = false;
                }
                else {
                    this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
                        messageLocalizationParams: [this.tenant.name]
                    });
                    this.tenant = (/** @type {?} */ ({}));
                }
                this.store.dispatch(new SetTenant(success ? this.tenant : null));
            }));
        }
        else {
            this.store.dispatch(new SetTenant(null));
            this.tenantName = null;
            this.isModalVisible = false;
        }
    }
}
TenantBoxComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenant-box',
                template: "<div\n  class=\"tenant-switch-box\"\n  style=\"background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;\"\n>\n  <span style=\"color: #666;\">{{ 'AbpUiMultiTenancy::Tenant' | abpLocalization }}: </span>\n  <strong>\n    <i>{{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}</i>\n  </strong>\n  (<a id=\"abp-tenant-switch-link\" style=\"color: #333; cursor: pointer\" (click)=\"onSwitch()\">{{\n    'AbpUiMultiTenancy::Switch' | abpLocalization\n  }}</a\n  >)\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" size=\"md\">\n  <ng-template #abpHeader>\n    <h5>Switch Tenant</h5>\n  </ng-template>\n  <ng-template #abpBody>\n    <form (ngSubmit)=\"save()\">\n      <div class=\"mt-2\">\n        <div class=\"form-group\">\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\n        </div>\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"save()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n"
            }] }
];
/** @nocollapse */
TenantBoxComponent.ctorParameters = () => [
    { type: Store },
    { type: ToasterService },
    { type: AccountService }
];
if (false) {
    /** @type {?} */
    TenantBoxComponent.prototype.tenant;
    /** @type {?} */
    TenantBoxComponent.prototype.tenantName;
    /** @type {?} */
    TenantBoxComponent.prototype.isModalVisible;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.toasterService;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.accountService;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class AccountModule {
}
AccountModule.decorators = [
    { type: NgModule, args: [{
                declarations: [
                    LoginComponent,
                    RegisterComponent,
                    TenantBoxComponent,
                    ChangePasswordComponent,
                    ManageProfileComponent,
                    PersonalSettingsComponent,
                ],
                imports: [CoreModule, AccountRoutingModule, ThemeSharedModule, TableModule, NgbDropdownModule, NgxValidateCoreModule],
                exports: [],
            },] }
];
/**
 *
 * @deprecated since version 0.9
 * @param {?=} options
 * @return {?}
 */
function AccountProviders(options = (/** @type {?} */ ({}))) {
    return [
        { provide: ACCOUNT_OPTIONS, useValue: options },
        {
            provide: 'ACCOUNT_OPTIONS',
            useFactory: optionsFactory,
            deps: [ACCOUNT_OPTIONS],
        },
    ];
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 *
 * @deprecated since version 0.9
 * @type {?}
 */
const ACCOUNT_ROUTES = {
    routes: (/** @type {?} */ ([
        {
            name: 'Account',
            path: 'account',
            invisible: true,
            layout: "application" /* application */,
            children: [{ path: 'login', name: 'Login', order: 1 }, { path: 'register', name: 'Register', order: 2 }],
        },
    ])),
};

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @record
 */
function Options() { }
if (false) {
    /** @type {?|undefined} */
    Options.prototype.redirectUrl;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @record
 */
function RegisterRequest() { }
if (false) {
    /** @type {?} */
    RegisterRequest.prototype.userName;
    /** @type {?} */
    RegisterRequest.prototype.emailAddress;
    /** @type {?} */
    RegisterRequest.prototype.password;
    /** @type {?|undefined} */
    RegisterRequest.prototype.appName;
}
/**
 * @record
 */
function RegisterResponse() { }
if (false) {
    /** @type {?} */
    RegisterResponse.prototype.tenantId;
    /** @type {?} */
    RegisterResponse.prototype.userName;
    /** @type {?} */
    RegisterResponse.prototype.name;
    /** @type {?} */
    RegisterResponse.prototype.surname;
    /** @type {?} */
    RegisterResponse.prototype.email;
    /** @type {?} */
    RegisterResponse.prototype.emailConfirmed;
    /** @type {?} */
    RegisterResponse.prototype.phoneNumber;
    /** @type {?} */
    RegisterResponse.prototype.phoneNumberConfirmed;
    /** @type {?} */
    RegisterResponse.prototype.twoFactorEnabled;
    /** @type {?} */
    RegisterResponse.prototype.lockoutEnabled;
    /** @type {?} */
    RegisterResponse.prototype.lockoutEnd;
    /** @type {?} */
    RegisterResponse.prototype.concurrencyStamp;
    /** @type {?} */
    RegisterResponse.prototype.isDeleted;
    /** @type {?} */
    RegisterResponse.prototype.deleterId;
    /** @type {?} */
    RegisterResponse.prototype.deletionTime;
    /** @type {?} */
    RegisterResponse.prototype.lastModificationTime;
    /** @type {?} */
    RegisterResponse.prototype.lastModifierId;
    /** @type {?} */
    RegisterResponse.prototype.creationTime;
    /** @type {?} */
    RegisterResponse.prototype.creatorId;
    /** @type {?} */
    RegisterResponse.prototype.id;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @record
 */
function TenantIdResponse() { }
if (false) {
    /** @type {?} */
    TenantIdResponse.prototype.success;
    /** @type {?} */
    TenantIdResponse.prototype.tenantId;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { ACCOUNT_OPTIONS, ACCOUNT_ROUTES, AccountModule, AccountProviders, ChangePasswordComponent, LoginComponent, ManageProfileComponent, PersonalSettingsComponent, RegisterComponent, optionsFactory, LoginComponent as ɵa, RegisterComponent as ɵc, AccountService as ɵd, TenantBoxComponent as ɵe, ChangePasswordComponent as ɵf, ManageProfileComponent as ɵg, PersonalSettingsComponent as ɵh, AccountRoutingModule as ɵi, optionsFactory as ɵj, ACCOUNT_OPTIONS as ɵk };
//# sourceMappingURL=abp-ng.account.js.map
