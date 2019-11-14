import {
  ConfigState,
  GetAppConfiguration,
  RestService,
  DynamicLayoutComponent,
  ChangePassword,
  GetProfile,
  UpdateProfile,
  ProfileState,
  SessionState,
  SetTenant,
  CoreModule,
} from '@abp/ng.core';
import { ToasterService, fadeIn, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, Optional, Inject, Injectable, ɵɵdefineInjectable, ɵɵinject, NgModule, InjectionToken, Input } from '@angular/core';
/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/login/login.component.ts
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
    if (this.form.invalid) return;
    // this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);
    this.inProgress = true;
    from(
      this.oauthService.fetchTokenUsingPasswordFlow(this.form.get('username').value, this.form.get('password').value),
    )
      .pipe(
        switchMap(
          /**
           * @return {?}
           */
          () => this.store.dispatch(new GetAppConfiguration()),
        ),
        tap(
          /**
           * @return {?}
           */
          () => {
            /** @type {?} */
            const redirectUrl =
              snq(
                /**
                 * @return {?}
                 */
                (() => window.history.state),
              ).redirectUrl ||
              (this.options || {}).redirectUrl ||
              '/';
            this.store.dispatch(new Navigate([redirectUrl]));
          },
        ),
        catchError(
          /**
           * @param {?} err
           * @return {?}
           */
          err => {
            this.toasterService.error(
              snq(
                /**
                 * @return {?}
                 */
                () => err.error.error_description,
              ) ||
                snq(
                  /**
                   * @return {?}
                   */
                  () => err.error.error.message,
                  'AbpAccount::DefaultErrorMessage',
                ),
              'Error',
              { life: 7000 },
            );
            return throwError(err);
          },
        ),
        finalize(
          /**
           * @return {?}
           */
          () => (this.inProgress = false),
        ),
      )
      .subscribe();
  }
}
LoginComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-login',
                template: "<abp-auth-wrapper [mainContentRef]=\"mainContentRef\" [cancelContentRef]=\"cancelContentRef\">\r\n  <ng-template #mainContentRef>\r\n    <h4>{{ 'AbpAccount::Login' | abpLocalization }}</h4>\r\n    <strong>\r\n      {{ 'AbpAccount::AreYouANewUser' | abpLocalization }}\r\n      <a class=\"text-decoration-none\" routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\r\n    </strong>\r\n    <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" validateOnSubmit class=\"mt-4\">\r\n      <div class=\"form-group\">\r\n        <label for=\"login-input-user-name-or-email-address\">{{\r\n          'AbpAccount::UserNameOrEmailAddress' | abpLocalization\r\n        }}</label>\r\n        <input\r\n          class=\"form-control\"\r\n          type=\"text\"\r\n          id=\"login-input-user-name-or-email-address\"\r\n          formControlName=\"username\"\r\n          autofocus\r\n        />\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\r\n        <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\r\n      </div>\r\n      <div class=\"form-check\" validationTarget validationStyle>\r\n        <label class=\"form-check-label\" for=\"login-input-remember-me\">\r\n          <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\r\n          {{ 'AbpAccount::RememberMe' | abpLocalization }}\r\n        </label>\r\n      </div>\r\n      <abp-button\r\n        [loading]=\"inProgress\"\r\n        buttonType=\"submit\"\r\n        name=\"Action\"\r\n        buttonClass=\"btn-block btn-lg mt-3 btn btn-primary\"\r\n      >\r\n        {{ 'AbpAccount::Login' | abpLocalization }}\r\n      </abp-button>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #cancelContentRef>\r\n    <div class=\"card-footer text-center border-0\">\r\n      <a routerLink=\"/\">\r\n        <button type=\"button\" name=\"Action\" value=\"Cancel\" class=\"px-2 py-0 btn btn-link\">\r\n          {{ 'AbpAccount::Cancel' | abpLocalization }}\r\n        </button>\r\n      </a>\r\n    </div>\r\n  </ng-template>\r\n</abp-auth-wrapper>\r\n"
            }] }
];
/** @nocollapse */
LoginComponent.ctorParameters = () => [
  { type: FormBuilder },
  { type: OAuthService },
  { type: Store },
  { type: ToasterService },
  { type: undefined, decorators: [{ type: Optional }, { type: Inject, args: ['ACCOUNT_OPTIONS'] }] },
];
if (false) {
    { type: Component, args: [{
                selector: 'abp-login',
                template: "<abp-auth-wrapper [mainContentRef]=\"mainContentRef\" [cancelContentRef]=\"cancelContentRef\">\r\n  <ng-template #mainContentRef>\r\n    <h4>{{ 'AbpAccount::Login' | abpLocalization }}</h4>\r\n    <strong>\r\n      {{ 'AbpAccount::AreYouANewUser' | abpLocalization }}\r\n      <a class=\"text-decoration-none\" routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\r\n    </strong>\r\n    <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" validateOnSubmit class=\"mt-4\">\r\n      <div class=\"form-group\">\r\n        <label for=\"login-input-user-name-or-email-address\">{{\r\n          'AbpAccount::UserNameOrEmailAddress' | abpLocalization\r\n        }}</label>\r\n        <input\r\n          class=\"form-control\"\r\n          type=\"text\"\r\n          id=\"login-input-user-name-or-email-address\"\r\n          formControlName=\"username\"\r\n          autofocus\r\n        />\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\r\n        <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\r\n      </div>\r\n      <div class=\"form-check\" validationTarget validationStyle>\r\n        <label class=\"form-check-label\" for=\"login-input-remember-me\">\r\n          <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\r\n          {{ 'AbpAccount::RememberMe' | abpLocalization }}\r\n        </label>\r\n      </div>\r\n      <abp-button\r\n        [loading]=\"inProgress\"\r\n        buttonType=\"submit\"\r\n        name=\"Action\"\r\n        buttonClass=\"btn-block btn-lg mt-3 btn btn-primary\"\r\n      >\r\n        {{ 'AbpAccount::Login' | abpLocalization }}\r\n      </abp-button>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #cancelContentRef>\r\n    <div class=\"card-footer text-center border-0\">\r\n      <a routerLink=\"/\">\r\n        <button type=\"button\" name=\"Action\" value=\"Cancel\" class=\"px-2 py-0 btn btn-link\">\r\n          {{ 'AbpAccount::Cancel' | abpLocalization }}\r\n        </button>\r\n      </a>\r\n    </div>\r\n  </ng-template>\r\n</abp-auth-wrapper>\r\n"
            }] }
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
 * Generated from: lib/components/manage-profile/manage-profile.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class ManageProfileComponent {
  constructor() {
    this.selectedTab = 0;
  }
}
ManageProfileComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-manage-profile',
                template: "<div id=\"AbpContentToolbar\"></div>\r\n\r\n<div class=\"card border-0 shadow-sm\">\r\n  <div class=\"card-body\">\r\n    <div class=\"row\">\r\n      <div class=\"col-3\">\r\n        <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\r\n          <li class=\"nav-item\" (click)=\"selectedTab = 0\">\r\n            <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 0 }\" role=\"tab\" href=\"javascript:void(0)\">{{\r\n              'AbpUi::ChangePassword' | abpLocalization\r\n            }}</a>\r\n          </li>\r\n          <li class=\"nav-item\" (click)=\"selectedTab = 1\">\r\n            <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 1 }\" role=\"tab\" href=\"javascript:void(0)\">{{\r\n              'AbpAccount::PersonalSettings' | abpLocalization\r\n            }}</a>\r\n          </li>\r\n        </ul>\r\n      </div>\r\n      <div class=\"col-9\">\r\n        <div class=\"tab-content\" *ngIf=\"selectedTab === 0\" [@fadeIn]>\r\n          <div class=\"tab-pane active\" role=\"tabpanel\">\r\n            <h4>\r\n              {{ 'AbpIdentity::ChangePassword' | abpLocalization }}\r\n              <hr />\r\n            </h4>\r\n            <abp-change-password-form></abp-change-password-form>\r\n          </div>\r\n        </div>\r\n        <div class=\"tab-content\" *ngIf=\"selectedTab === 1\" [@fadeIn]>\r\n          <div class=\"tab-pane active\" role=\"tabpanel\">\r\n            <h4>\r\n              {{ 'AbpIdentity::PersonalSettings' | abpLocalization }}\r\n              <hr />\r\n            </h4>\r\n            <abp-personal-settings-form></abp-personal-settings-form>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
                animations: [trigger('fadeIn', [transition(':enter', useAnimation(fadeIn))])]
            }] }
];
if (false) {
  /** @type {?} */
  ManageProfileComponent.prototype.selectedTab;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/account.service.ts
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
    { type: Component, args: [{
                selector: 'abp-manage-profile',
                template: "<div id=\"AbpContentToolbar\"></div>\r\n\r\n<div class=\"card border-0 shadow-sm\">\r\n  <div class=\"card-body\">\r\n    <div class=\"row\">\r\n      <div class=\"col-3\">\r\n        <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\r\n          <li class=\"nav-item\" (click)=\"selectedTab = 0\">\r\n            <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 0 }\" role=\"tab\" href=\"javascript:void(0)\">{{\r\n              'AbpUi::ChangePassword' | abpLocalization\r\n            }}</a>\r\n          </li>\r\n          <li class=\"nav-item\" (click)=\"selectedTab = 1\">\r\n            <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 1 }\" role=\"tab\" href=\"javascript:void(0)\">{{\r\n              'AbpAccount::PersonalSettings' | abpLocalization\r\n            }}</a>\r\n          </li>\r\n        </ul>\r\n      </div>\r\n      <div class=\"col-9\">\r\n        <div class=\"tab-content\" *ngIf=\"selectedTab === 0\" [@fadeIn]>\r\n          <div class=\"tab-pane active\" role=\"tabpanel\">\r\n            <h4>\r\n              {{ 'AbpIdentity::ChangePassword' | abpLocalization }}\r\n              <hr />\r\n            </h4>\r\n            <abp-change-password-form></abp-change-password-form>\r\n          </div>\r\n        </div>\r\n        <div class=\"tab-content\" *ngIf=\"selectedTab === 1\" [@fadeIn]>\r\n          <div class=\"tab-pane active\" role=\"tabpanel\">\r\n            <h4>\r\n              {{ 'AbpIdentity::PersonalSettings' | abpLocalization }}\r\n              <hr />\r\n            </h4>\r\n            <abp-personal-settings-form></abp-personal-settings-form>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
                animations: [trigger('fadeIn', [transition(':enter', useAnimation(fadeIn))])]
            }] }
    return this.rest.request(request, { skipHandleError: true });
  }
}
AccountService.decorators = [
  {
    type: Injectable,
    args: [
      {
        providedIn: 'root',
      },
    ],
  },
];
/** @nocollapse */
AccountService.ctorParameters = () => [{ type: RestService }];
/** @nocollapse */ AccountService.ngInjectableDef = ɵɵdefineInjectable({
  factory: function AccountService_Factory() {
    return new AccountService(ɵɵinject(RestService));
  },
  token: AccountService,
  providedIn: 'root',
});
if (false) {
  /**
   * @type {?}
   * @private
   */
  AccountService.prototype.rest;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/register/register.component.ts
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
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        /** @type {?} */
        const passwordRules = this.store.selectSnapshot(ConfigState.getSettings('Identity.Password'));
        /** @type {?} */
        const passwordRulesArr = (/** @type {?} */ ([]));
        /** @type {?} */
        let requiredLength = 1;
        if ((passwordRules['Abp.Identity.Password.RequireDigit'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('number');
        }
        if ((passwordRules['Abp.Identity.Password.RequireLowercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('small');
        }
        if ((passwordRules['Abp.Identity.Password.RequireUppercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('capital');
        }
        if (+(passwordRules['Abp.Identity.Password.RequiredUniqueChars'] || 0) > 0) {
            passwordRulesArr.push('special');
        }
        if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
            requiredLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
        }
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
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        /** @type {?} */
        const passwordRules = this.store.selectSnapshot(ConfigState.getSettings('Identity.Password'));
        /** @type {?} */
        const passwordRulesArr = (/** @type {?} */ ([]));
        /** @type {?} */
        let requiredLength = 1;
        if ((passwordRules['Abp.Identity.Password.RequireDigit'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('number');
        }
        if ((passwordRules['Abp.Identity.Password.RequireLowercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('small');
        }
        if ((passwordRules['Abp.Identity.Password.RequireUppercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('capital');
        }
        if (+(passwordRules['Abp.Identity.Password.RequiredUniqueChars'] || 0) > 0) {
            passwordRulesArr.push('special');
        }
        if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
            requiredLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
        }
        this.form = this.fb.group({
            username: ['', [required$1, maxLength$1(255)]],
            password: ['', [required$1, validatePassword(passwordRulesArr), minLength$1(requiredLength), maxLength$1(32)]],
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
 * Generated from: lib/components/change-password/change-password.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const { minLength: minLength$2, required: required$2, maxLength: maxLength$2 } = Validators;
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
    this.mapErrorsFn
    /**
     * @param {?} errors
     * @param {?} groupErrors
     * @param {?} control
     * @return {?}
     */ = (errors, groupErrors, control) => {
      if (PASSWORD_FIELDS.indexOf(control.name) < 0) return errors;
      return errors.concat(
        groupErrors.filter(
          /**
           * @param {?} __0
    { type: Component, args: [{
                selector: 'abp-register',
                template: "<abp-auth-wrapper [mainContentRef]=\"mainContentRef\">\r\n  <ng-template #mainContentRef>\r\n    <h4>{{ 'AbpAccount::Register' | abpLocalization }}</h4>\r\n    <strong>\r\n      {{ 'AbpAccount::AlreadyRegistered' | abpLocalization }}\r\n      <a class=\"text-decoration-none\" routerLink=\"/account/login\">{{ 'AbpAccount::Login' | abpLocalization }}</a>\r\n    </strong>\r\n    <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" validateOnSubmit class=\"mt-4\">\r\n      <div class=\"form-group\">\r\n        <label for=\"input-user-name\">{{ 'AbpAccount::UserName' | abpLocalization }}</label\r\n        ><span> * </span\r\n        ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"input-email-address\">{{ 'AbpAccount::EmailAddress' | abpLocalization }}</label\r\n        ><span> * </span><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label\r\n        ><span> * </span><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\r\n      </div>\r\n      <abp-button\r\n        [loading]=\"inProgress\"\r\n        buttonType=\"submit\"\r\n        name=\"Action\"\r\n        buttonClass=\"btn-block btn-lg mt-3 btn btn-primary\"\r\n      >\r\n        {{ 'AbpAccount::Register' | abpLocalization }}\r\n      </abp-button>\r\n    </form>\r\n  </ng-template>\r\n</abp-auth-wrapper>\r\n"
            }] }
      {
        validators: [comparePasswords(PASSWORD_FIELDS)],
      },
    );
  }
  /**
   * @return {?}
   */
  onSubmit() {
    if (this.form.invalid) return;
    this.inProgress = true;
    this.store
      .dispatch(
        new ChangePassword({
          currentPassword: this.form.get('password').value,
          newPassword: this.form.get('newPassword').value,
        }),
      )
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          () => (this.inProgress = false),
        ),
      )
      .subscribe({
        /**
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
        /** @type {?} */
        const passwordRules = this.store.selectSnapshot(ConfigState.getSettings('Identity.Password'));
        /** @type {?} */
        const passwordRulesArr = (/** @type {?} */ ([]));
        /** @type {?} */
        let requiredLength = 1;
        if ((passwordRules['Abp.Identity.Password.RequireDigit'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('number');
        }
        if ((passwordRules['Abp.Identity.Password.RequireLowercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('small');
        }
        if ((passwordRules['Abp.Identity.Password.RequireUppercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('capital');
        }
        if (+(passwordRules['Abp.Identity.Password.RequiredUniqueChars'] || 0) > 0) {
            passwordRulesArr.push('special');
        }
        if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
            requiredLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
        }
        this.form = this.fb.group({
            password: ['', required$2],
            newPassword: [
                '',
                {
                    validators: [required$2, validatePassword(passwordRulesArr), minLength$2(requiredLength), maxLength$2(32)],
                },
            ],
            repeatNewPassword: [
                '',
                { validators: [required$2, validatePassword(passwordRulesArr), minLength$2(requiredLength), maxLength$2(32)] },
            ],
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
        this.inProgress = true;
        this.store
            .dispatch(new ChangePassword({
            currentPassword: this.form.get('password').value,
            newPassword: this.form.get('newPassword').value,
        }))
            .pipe(finalize((/**
         * @return {?}
         */
        () => (this.inProgress = false))))
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
                template: "<form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" [mapErrorsFn]=\"mapErrorsFn\" validateOnSubmit>\r\n  <div class=\"form-group\">\r\n    <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\r\n    ><span> * </span\r\n    ><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" autofocus />\r\n  </div>\r\n  <div class=\"form-group\">\r\n    <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\r\n    ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\r\n  </div>\r\n  <div class=\"form-group\">\r\n    <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\r\n    ><span> * </span\r\n    ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\r\n  </div>\r\n  <abp-button\r\n    iconClass=\"fa fa-check\"\r\n    buttonClass=\"btn btn-primary color-white\"\r\n    buttonType=\"submit\"\r\n    [loading]=\"inProgress\"\r\n    [disabled]=\"form?.invalid\"\r\n    >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\r\n  >\r\n</form>\r\n"
            }] }
];
/** @nocollapse */
ChangePasswordComponent.ctorParameters = () => [{ type: FormBuilder }, { type: Store }, { type: ToasterService }];
if (false) {
    /** @type {?} */
    ChangePasswordComponent.prototype.form;
    /** @type {?} */
    ChangePasswordComponent.prototype.inProgress;
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
        /** @type {?} */
        const passwordRules = this.store.selectSnapshot(ConfigState.getSettings('Identity.Password'));
        /** @type {?} */
        const passwordRulesArr = (/** @type {?} */ ([]));
        /** @type {?} */
        let requiredLength = 1;
        if ((passwordRules['Abp.Identity.Password.RequireDigit'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('number');
        }
        if ((passwordRules['Abp.Identity.Password.RequireLowercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('small');
        }
        if ((passwordRules['Abp.Identity.Password.RequireUppercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('capital');
        }
        if (+(passwordRules['Abp.Identity.Password.RequiredUniqueChars'] || 0) > 0) {
            passwordRulesArr.push('special');
        }
        if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
            requiredLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
        }
        this.form = this.fb.group({
            password: ['', required$2],
            newPassword: [
                '',
                {
                    validators: [required$2, validatePassword(passwordRulesArr), minLength$2(requiredLength), maxLength$2(32)],
                },
            ],
            repeatNewPassword: [
                '',
                { validators: [required$2, validatePassword(passwordRulesArr), minLength$2(requiredLength), maxLength$2(32)] },
            ],
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
        this.inProgress = true;
        this.store
            .dispatch(new ChangePassword({
            currentPassword: this.form.get('password').value,
            newPassword: this.form.get('newPassword').value,
        }))
            .pipe(finalize((/**
         * @return {?}
         */
        () => (this.inProgress = false))))
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
                template: "<form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" [mapErrorsFn]=\"mapErrorsFn\" validateOnSubmit>\r\n  <div class=\"form-group\">\r\n    <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\r\n    ><span> * </span\r\n    ><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" autofocus />\r\n  </div>\r\n  <div class=\"form-group\">\r\n    <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\r\n    ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\r\n  </div>\r\n  <div class=\"form-group\">\r\n    <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\r\n    ><span> * </span\r\n    ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\r\n  </div>\r\n  <abp-button\r\n    iconClass=\"fa fa-check\"\r\n    buttonClass=\"btn btn-primary color-white\"\r\n    buttonType=\"submit\"\r\n    [loading]=\"inProgress\"\r\n    [disabled]=\"form?.invalid\"\r\n    >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\r\n  >\r\n</form>\r\n"
            }] }
     */
    save() {
        if (this.tenant.name && !this.inProgress) {
            this.inProgress = true;
    /** @type {?} */
    ChangePasswordComponent.prototype.form;
    /** @type {?} */
    ChangePasswordComponent.prototype.inProgress;
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
TenantBoxComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenant-box',
                template: "<div class=\"card shadow-sm rounded mb-3\">\r\n  <div class=\"card-body px-5\">\r\n    <div class=\"row\">\r\n      <div class=\"col\">\r\n        <span style=\"font-size: 0.8em;\" class=\"text-uppercase text-muted\">{{\r\n          'AbpUiMultiTenancy::Tenant' | abpLocalization\r\n        }}</span\r\n        ><br />\r\n        <h6 class=\"m-0 d-inline-block\">\r\n          <span>\r\n            {{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}\r\n          </span>\r\n        </h6>\r\n      </div>\r\n      <div class=\"col-auto\">\r\n        <a\r\n          id=\"AbpTenantSwitchLink\"\r\n          href=\"javascript:void(0);\"\r\n          class=\"btn btn-sm mt-3 btn-outline-primary\"\r\n          (click)=\"onSwitch()\"\r\n          >{{ 'AbpUiMultiTenancy::Switch' | abpLocalization }}</a\r\n        >\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"inProgress\">\r\n  <ng-template #abpHeader>\r\n    <h5>Switch Tenant</h5>\r\n  </ng-template>\r\n  <ng-template #abpBody>\r\n    <form (ngSubmit)=\"save()\">\r\n      <div class=\"mt-2\">\r\n        <div class=\"form-group\">\r\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\r\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\r\n        </div>\r\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button buttonType=\"button\" buttonClass=\"btn btn-primary\" (click)=\"save()\">\r\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\r\n    </abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
            }] }
];
/** @nocollapse */
TenantBoxComponent.ctorParameters = () => [{ type: Store }, { type: ToasterService }, { type: AccountService }];
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
    mainContentRef: [{ type: Input }],
    cancelContentRef: [{ type: Input }]
};
if (false) {
            this.form = this.fb.group({
                userName: [profile.userName, [required$3, maxLength$3(256)]],
                email: [profile.email, [required$3, email$1, maxLength$3(256)]],
                name: [profile.name || '', [maxLength$3(64)]],
                surname: [profile.surname || '', [maxLength$3(64)]],
                phoneNumber: [profile.phoneNumber || '', [maxLength$3(16)]],
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
                template: "<form validateOnSubmit *ngIf=\"form\" [formGroup]=\"form\" (ngSubmit)=\"submit()\">\r\n  <div class=\"form-group\">\r\n    <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\r\n    ><span> * </span\r\n    ><input\r\n      type=\"text\"\r\n      id=\"username\"\r\n      class=\"form-control\"\r\n      formControlName=\"userName\"\r\n      autofocus\r\n      (keydown.space)=\"$event.preventDefault()\"\r\n    />\r\n  </div>\r\n  <div class=\"row\">\r\n    <div class=\"col col-md-6\">\r\n      <div class=\"form-group\">\r\n        <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\r\n        ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\r\n      </div>\r\n    </div>\r\n    <div class=\"col col-md-6\">\r\n      <div class=\"form-group\">\r\n        <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\r\n        ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\r\n      </div>\r\n    </div>\r\n  </div>\r\n  <div class=\"form-group\">\r\n    <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:Email' | abpLocalization }}</label\r\n    ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\r\n  </div>\r\n  <div class=\"form-group\">\r\n    <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\r\n    ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\r\n  </div>\r\n  <abp-button\r\n    buttonType=\"submit\"\r\n    iconClass=\"fa fa-check\"\r\n    buttonClass=\"btn btn-primary color-white\"\r\n    [loading]=\"inProgress\"\r\n    [disabled]=\"form?.invalid || form?.pristine\"\r\n  >\r\n    {{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\r\n  >\r\n</form>\r\n"
            }] }
 * @deprecated since version 0.9
 * @type {?}
 */
const ACCOUNT_ROUTES = {
  routes: /** @type {?} */ ([
    {
      name: 'Account',
      path: 'account',
      invisible: true,
      layout: 'application' /* application */,
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
 * @record
 */
function RegisterResponse() {}
if (false) {
  /** @type {?} */
  RegisterResponse.prototype.tenantId;
  /** @type {?} */
  RegisterResponse.prototype.userName;
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
        this.tenant = this.store.selectSnapshot(SessionState.getTenant) || ((/** @type {?} */ ({})));
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
        if (this.tenant.name && !this.inProgress) {
            this.inProgress = true;
            this.accountService
                .findTenant(this.tenant.name)
                .pipe(finalize((/**
             * @return {?}
             */
            () => (this.inProgress = false))), take(1), catchError((/**
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
                        name: this.tenant.name,
                    };
                    this.tenantName = this.tenant.name;
                    this.isModalVisible = false;
                }
                else {
                    this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
                        messageLocalizationParams: [this.tenant.name],
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
    { type: Component, args: [{
                selector: 'abp-tenant-box',
                template: "<div class=\"card shadow-sm rounded mb-3\">\r\n  <div class=\"card-body px-5\">\r\n    <div class=\"row\">\r\n      <div class=\"col\">\r\n        <span style=\"font-size: 0.8em;\" class=\"text-uppercase text-muted\">{{\r\n          'AbpUiMultiTenancy::Tenant' | abpLocalization\r\n        }}</span\r\n        ><br />\r\n        <h6 class=\"m-0 d-inline-block\">\r\n          <span>\r\n            {{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}\r\n          </span>\r\n        </h6>\r\n      </div>\r\n      <div class=\"col-auto\">\r\n        <a\r\n          id=\"AbpTenantSwitchLink\"\r\n          href=\"javascript:void(0);\"\r\n          class=\"btn btn-sm mt-3 btn-outline-primary\"\r\n          (click)=\"onSwitch()\"\r\n          >{{ 'AbpUiMultiTenancy::Switch' | abpLocalization }}</a\r\n        >\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"inProgress\">\r\n  <ng-template #abpHeader>\r\n    <h5>Switch Tenant</h5>\r\n  </ng-template>\r\n  <ng-template #abpBody>\r\n    <form (ngSubmit)=\"save()\">\r\n      <div class=\"mt-2\">\r\n        <div class=\"form-group\">\r\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\r\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\r\n        </div>\r\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button buttonType=\"button\" buttonClass=\"btn btn-primary\" (click)=\"save()\">\r\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\r\n    </abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
            }] }
    /** @type {?} */
    TenantBoxComponent.prototype.tenant;
    /** @type {?} */
    TenantBoxComponent.prototype.tenantName;
    /** @type {?} */
    TenantBoxComponent.prototype.isModalVisible;
    /** @type {?} */
    TenantBoxComponent.prototype.inProgress;
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
    { type: NgModule, args: [{
                declarations: [
                    AuthWrapperComponent,
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
export { ACCOUNT_OPTIONS, ACCOUNT_ROUTES, AccountModule, AccountProviders, ChangePasswordComponent, LoginComponent, ManageProfileComponent, PersonalSettingsComponent, RegisterComponent, optionsFactory, AuthWrapperComponent as ɵa, LoginComponent as ɵb, RegisterComponent as ɵd, AccountService as ɵe, TenantBoxComponent as ɵf, ChangePasswordComponent as ɵg, ManageProfileComponent as ɵh, PersonalSettingsComponent as ɵi, AccountRoutingModule as ɵj, optionsFactory as ɵk, ACCOUNT_OPTIONS as ɵl };
