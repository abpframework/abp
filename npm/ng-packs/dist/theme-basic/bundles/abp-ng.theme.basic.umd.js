(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@ng-bootstrap/ng-bootstrap'), require('@angular/forms'), require('@ngx-validate/core'), require('@ngxs/store'), require('rxjs'), require('rxjs/operators'), require('@ngxs/router-plugin'), require('angular-oauth2-oidc'), require('snq'), require('just-compare'), require('@abp/ng.theme.shared'), require('primeng/toast')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.theme.basic', ['exports', '@abp/ng.core', '@angular/core', '@ng-bootstrap/ng-bootstrap', '@angular/forms', '@ngx-validate/core', '@ngxs/store', 'rxjs', 'rxjs/operators', '@ngxs/router-plugin', 'angular-oauth2-oidc', 'snq', 'just-compare', '@abp/ng.theme.shared', 'primeng/toast'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.theme = global.abp.ng.theme || {}, global.abp.ng.theme.basic = {}), global.ng_core, global.ng.core, global.ngBootstrap, global.ng.forms, global.core$1, global.store, global.rxjs, global.rxjs.operators, global.routerPlugin, global.angularOauth2Oidc, global.snq, global.compare, global.ng_theme_shared, global.toast));
}(this, function (exports, ng_core, core, ngBootstrap, forms, core$1, store, rxjs, operators, routerPlugin, angularOauth2Oidc, snq, compare, ng_theme_shared, toast) { 'use strict';

    snq = snq && snq.hasOwnProperty('default') ? snq['default'] : snq;
    compare = compare && compare.hasOwnProperty('default') ? compare['default'] : compare;

    /*! *****************************************************************************
    Copyright (c) Microsoft Corporation. All rights reserved.
    Licensed under the Apache License, Version 2.0 (the "License"); you may not use
    this file except in compliance with the License. You may obtain a copy of the
    License at http://www.apache.org/licenses/LICENSE-2.0

    THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
    KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
    WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
    MERCHANTABLITY OR NON-INFRINGEMENT.

    See the Apache Version 2.0 License for specific language governing permissions
    and limitations under the License.
    ***************************************************************************** */

    var __assign = function() {
        __assign = Object.assign || function __assign(t) {
            for (var s, i = 1, n = arguments.length; i < n; i++) {
                s = arguments[i];
                for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p)) t[p] = s[p];
            }
            return t;
        };
        return __assign.apply(this, arguments);
    };

    function __decorate(decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    }

    function __metadata(metadataKey, metadataValue) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
    }

    function __read(o, n) {
        var m = typeof Symbol === "function" && o[Symbol.iterator];
        if (!m) return o;
        var i = m.call(o), r, ar = [], e;
        try {
            while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
        }
        catch (error) { e = { error: error }; }
        finally {
            try {
                if (r && !r.done && (m = i["return"])) m.call(i);
            }
            finally { if (e) throw e.error; }
        }
        return ar;
    }

    function __spread() {
        for (var ar = [], i = 0; i < arguments.length; i++)
            ar = ar.concat(__read(arguments[i]));
        return ar;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var minLength = forms.Validators.minLength, required = forms.Validators.required;
    var ChangePasswordComponent = /** @class */ (function () {
        function ChangePasswordComponent(fb, modalService, store) {
            this.fb = fb;
            this.modalService = modalService;
            this.store = store;
            this.visibleChange = new core.EventEmitter();
        }
        /**
         * @return {?}
         */
        ChangePasswordComponent.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            this.form = this.fb.group({
                password: ['', [required, minLength(6), core$1.validatePassword(['small', 'capital', 'number', 'special'])]],
                newPassword: ['', [required, minLength(6), core$1.validatePassword(['small', 'capital', 'number', 'special'])]],
                repeatNewPassword: ['', [required, minLength(6), core$1.validatePassword(['small', 'capital', 'number', 'special'])]],
            }, {
                validators: [core$1.comparePasswords(['newPassword', 'repeatNewPassword'])],
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
                .dispatch(new ng_core.ProfileChangePassword({
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
            rxjs.from(this.modalRef.result)
                .pipe(operators.take(1))
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
            { type: core.Component, args: [{
                        selector: 'abp-change-password',
                        template: "<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h4 class=\"modal-title\" id=\"modal-basic-title\">{{ 'AbpIdentity::ChangePassword' | abpLocalization }}</h4>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n    <div class=\"modal-body\">\n      <div class=\"form-group\">\n        <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\n      </div>\n      <div class=\"form-group\" [class.is-invalid]=\"form.errors?.passwordMismatch\">\n        <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\n        ><span> * </span\n        ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\n        <div *ngIf=\"form.errors?.passwordMismatch\" class=\"invalid-feedback\">\n          {{ 'AbpIdentity::Identity.PasswordConfirmationFailed' | abpLocalization }}\n        </div>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n"
                    }] }
        ];
        /** @nocollapse */
        ChangePasswordComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: ngBootstrap.NgbModal },
            { type: store.Store }
        ]; };
        ChangePasswordComponent.propDecorators = {
            visible: [{ type: core.Input }],
            visibleChange: [{ type: core.Output }],
            modalContent: [{ type: core.ViewChild, args: ['modalContent', { static: false },] }]
        };
        return ChangePasswordComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LayoutAccountComponent = /** @class */ (function () {
        function LayoutAccountComponent() {
            this.isCollapsed = false;
        }
        // required for dynamic component
        LayoutAccountComponent.type = "account" /* account */;
        LayoutAccountComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-account',
                        template: "<abp-layout>\n  <ul class=\"navbar-nav mr-auto\">\n    <li class=\"nav-item\">\n      <a class=\"nav-link\" href=\"/\">\n        Home\n      </a>\n    </li>\n  </ul>\n\n  <span id=\"main-navbar-tools\">\n    <span>\n      <div class=\"dropdown d-inline\" ngbDropdown>\n        <a class=\"btn btn-link dropdown-toggle\" role=\"button\" data-toggle=\"dropdown\" ngbDropdownToggle>\n          English\n        </a>\n\n        <div class=\"dropdown-menu\" ngbDropdownMenu>\n          <a class=\"dropdown-item\">\u010Ce\u0161tina</a>\n          <a class=\"dropdown-item\">Portugu\u00EAs</a>\n          <a class=\"dropdown-item\">T\u00FCrk\u00E7e</a>\n          <a class=\"dropdown-item\">\u7B80\u4F53\u4E2D\u6587</a>\n        </div>\n      </div>\n    </span>\n  </span>\n</abp-layout>\n"
                    }] }
        ];
        return LayoutAccountComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LayoutAddNavigationElement = /** @class */ (function () {
        function LayoutAddNavigationElement(payload) {
            this.payload = payload;
        }
        LayoutAddNavigationElement.type = '[Layout] Add Navigation Element';
        return LayoutAddNavigationElement;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LayoutState = /** @class */ (function () {
        function LayoutState() {
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        LayoutState.getNavigationElements = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var navigationElements = _a.navigationElements;
            return navigationElements;
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        LayoutState.prototype.layoutAction = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var getState = _a.getState, patchState = _a.patchState;
            var _c = _b.payload, payload = _c === void 0 ? [] : _c;
            var navigationElements = getState().navigationElements;
            if (!Array.isArray(payload)) {
                payload = [payload];
            }
            if (navigationElements.length) {
                payload = snq((/**
                 * @return {?}
                 */
                function () {
                    return ((/** @type {?} */ (payload))).filter((/**
                     * @param {?} __0
                     * @return {?}
                     */
                    function (_a) {
                        var name = _a.name;
                        return navigationElements.findIndex((/**
                         * @param {?} nav
                         * @return {?}
                         */
                        function (nav) { return nav.name === name; })) < 0;
                    }));
                }), []);
            }
            if (!payload.length)
                return;
            navigationElements = __spread(navigationElements, payload).map((/**
             * @param {?} element
             * @return {?}
             */
            function (element) { return (__assign({}, element, { order: element.order || 99 })); }))
                .sort((/**
             * @param {?} a
             * @param {?} b
             * @return {?}
             */
            function (a, b) { return a.order - b.order; }));
            return patchState({
                navigationElements: navigationElements,
            });
        };
        __decorate([
            store.Action(LayoutAddNavigationElement),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, LayoutAddNavigationElement]),
            __metadata("design:returntype", void 0)
        ], LayoutState.prototype, "layoutAction", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Array)
        ], LayoutState, "getNavigationElements", null);
        LayoutState = __decorate([
            store.State({
                name: 'LayoutState',
                defaults: (/** @type {?} */ ({ navigationElements: [] })),
            })
        ], LayoutState);
        return LayoutState;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LayoutApplicationComponent = /** @class */ (function () {
        function LayoutApplicationComponent(store, oauthService) {
            this.store = store;
            this.oauthService = oauthService;
            this.isOpenChangePassword = false;
            this.isOpenProfile = false;
            this.rightPartElements = [];
            this.trackByFn = (/**
             * @param {?} _
             * @param {?} item
             * @return {?}
             */
            function (_, item) { return item.name; });
            this.trackElementByFn = (/**
             * @param {?} _
             * @param {?} element
             * @return {?}
             */
            function (_, element) { return element; });
        }
        Object.defineProperty(LayoutApplicationComponent.prototype, "visibleRoutes$", {
            get: /**
             * @return {?}
             */
            function () {
                return this.routes$.pipe(operators.map((/**
                 * @param {?} routes
                 * @return {?}
                 */
                function (routes) { return getVisibleRoutes(routes); })));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LayoutApplicationComponent.prototype, "defaultLanguage$", {
            get: /**
             * @return {?}
             */
            function () {
                var _this = this;
                return this.languages$.pipe(operators.map((/**
                 * @param {?} languages
                 * @return {?}
                 */
                function (languages) { return snq((/**
                 * @return {?}
                 */
                function () { return languages.find((/**
                 * @param {?} lang
                 * @return {?}
                 */
                function (lang) { return lang.cultureName === _this.selectedLangCulture; })).displayName; })); }), ''));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LayoutApplicationComponent.prototype, "dropdownLanguages$", {
            get: /**
             * @return {?}
             */
            function () {
                var _this = this;
                return this.languages$.pipe(operators.map((/**
                 * @param {?} languages
                 * @return {?}
                 */
                function (languages) { return snq((/**
                 * @return {?}
                 */
                function () { return languages.filter((/**
                 * @param {?} lang
                 * @return {?}
                 */
                function (lang) { return lang.cultureName !== _this.selectedLangCulture; })); })); }), []));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LayoutApplicationComponent.prototype, "selectedLangCulture", {
            get: /**
             * @return {?}
             */
            function () {
                return this.store.selectSnapshot(ng_core.SessionState.getLanguage);
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        LayoutApplicationComponent.prototype.ngAfterViewInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            /** @type {?} */
            var navigations = this.store.selectSnapshot(LayoutState.getNavigationElements).map((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var name = _a.name;
                return name;
            }));
            if (navigations.indexOf('LanguageRef') < 0) {
                this.store.dispatch(new LayoutAddNavigationElement([
                    { element: this.languageRef, order: 4, name: 'LanguageRef' },
                    { element: this.currentUserRef, order: 5, name: 'CurrentUserRef' },
                ]));
            }
            this.navElements$
                .pipe(operators.map((/**
             * @param {?} elements
             * @return {?}
             */
            function (elements) { return elements.map((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var element = _a.element;
                return element;
            })); })), operators.filter((/**
             * @param {?} elements
             * @return {?}
             */
            function (elements) { return !compare(elements, _this.rightPartElements); })), ng_core.takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} elements
             * @return {?}
             */
            function (elements) {
                setTimeout((/**
                 * @return {?}
                 */
                function () { return (_this.rightPartElements = elements); }), 0);
            }));
        };
        /**
         * @return {?}
         */
        LayoutApplicationComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        /**
         * @param {?} cultureName
         * @return {?}
         */
        LayoutApplicationComponent.prototype.onChangeLang = /**
         * @param {?} cultureName
         * @return {?}
         */
        function (cultureName) {
            this.store.dispatch(new ng_core.SessionSetLanguage(cultureName));
            this.store.dispatch(new ng_core.ConfigGetAppConfiguration());
        };
        /**
         * @return {?}
         */
        LayoutApplicationComponent.prototype.logout = /**
         * @return {?}
         */
        function () {
            this.oauthService.logOut();
            this.store.dispatch(new routerPlugin.Navigate(['/account/login'], null, {
                state: { redirectUrl: this.store.selectSnapshot(routerPlugin.RouterState).state.url },
            }));
            this.store.dispatch(new ng_core.ConfigGetAppConfiguration());
        };
        // required for dynamic component
        LayoutApplicationComponent.type = "application" /* application */;
        LayoutApplicationComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-application',
                        template: "<abp-layout>\n  <ul class=\"navbar-nav mr-auto\">\n    <ng-container\n      *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\n      [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\n      [ngTemplateOutletContext]=\"{ $implicit: route }\"\n    >\n    </ng-container>\n\n    <ng-template #defaultLink let-route>\n      <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\n        <a class=\"nav-link\" [routerLink]=\"[route.url]\">{{ route.name }}</a>\n      </li>\n    </ng-template>\n\n    <ng-template #dropdownLink let-route>\n      <li class=\"nav-item dropdown\" ngbDropdown [abpPermission]=\"route.requiredPolicy\" [abpVisibility]=\"routeContainer\">\n        <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n          {{ route.name }}\n        </a>\n        <div #routeContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            #forTemplate\n            ngFor\n            [ngForOf]=\"route.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </li>\n    </ng-template>\n\n    <ng-template #childWrapper let-child>\n      <ng-template\n        [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\n        [ngTemplateOutletContext]=\"{ $implicit: child }\"\n      ></ng-template>\n    </ng-template>\n\n    <ng-template #defaultChild let-child>\n      <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\n        <a class=\"dropdown-item py-2 px-2\" [routerLink]=\"[child.url]\">{{ child.name }}</a>\n      </div>\n    </ng-template>\n\n    <ng-template #dropdownChild let-child>\n      <div\n        [abpVisibility]=\"childrenContainer\"\n        class=\"dropdown-submenu\"\n        ngbDropdown\n        placement=\"right-top\"\n        [abpPermission]=\"child.requiredPolicy\"\n      >\n        <a role=\"button\" class=\"btn d-block text-left py-2 px-2\" ngbDropdownToggle> {{ child.name }} </a>\n        <div #childrenContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            ngFor\n            [ngForOf]=\"child.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </div>\n    </ng-template>\n  </ul>\n\n  <ul class=\"navbar-nav ml-auto\">\n    <ng-container\n      *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\n      [ngTemplateOutlet]=\"element\"\n    ></ng-container>\n  </ul>\n</abp-layout>\n\n<abp-change-password [(visible)]=\"isOpenChangePassword\"></abp-change-password>\n\n<abp-profile [(visible)]=\"isOpenProfile\"></abp-profile>\n\n<ng-template #language>\n  <li class=\"nav-item dropdown\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n      {{ defaultLanguage$ | async }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a\n        *ngFor=\"let lang of dropdownLanguages$ | async\"\n        class=\"dropdown-item\"\n        (click)=\"onChangeLang(lang.cultureName)\"\n        >{{ lang?.displayName }}</a\n      >\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item dropdown\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n      {{ (currentUser$ | async)?.userName }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenChangePassword = true\">Change Password</a>\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenProfile = true\">My Profile</a>\n      <a class=\"dropdown-item pointer\" (click)=\"logout()\">Logout</a>\n    </div>\n  </li>\n</ng-template>\n"
                    }] }
        ];
        /** @nocollapse */
        LayoutApplicationComponent.ctorParameters = function () { return [
            { type: store.Store },
            { type: angularOauth2Oidc.OAuthService }
        ]; };
        LayoutApplicationComponent.propDecorators = {
            currentUserRef: [{ type: core.ViewChild, args: ['currentUser', { static: false, read: core.TemplateRef },] }],
            languageRef: [{ type: core.ViewChild, args: ['language', { static: false, read: core.TemplateRef },] }]
        };
        __decorate([
            store.Select(ng_core.ConfigState.getOne('routes')),
            __metadata("design:type", rxjs.Observable)
        ], LayoutApplicationComponent.prototype, "routes$", void 0);
        __decorate([
            store.Select(ng_core.ConfigState.getOne('currentUser')),
            __metadata("design:type", rxjs.Observable)
        ], LayoutApplicationComponent.prototype, "currentUser$", void 0);
        __decorate([
            store.Select(ng_core.ConfigState.getDeep('localization.languages')),
            __metadata("design:type", rxjs.Observable)
        ], LayoutApplicationComponent.prototype, "languages$", void 0);
        __decorate([
            store.Select(LayoutState.getNavigationElements),
            __metadata("design:type", rxjs.Observable)
        ], LayoutApplicationComponent.prototype, "navElements$", void 0);
        return LayoutApplicationComponent;
    }());
    /**
     * @param {?} routes
     * @return {?}
     */
    function getVisibleRoutes(routes) {
        return routes.reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        function (acc, val) {
            if (val.invisible)
                return acc;
            if (val.children && val.children.length) {
                val.children = getVisibleRoutes(val.children);
            }
            return __spread(acc, [val]);
        }), []);
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LayoutEmptyComponent = /** @class */ (function () {
        function LayoutEmptyComponent() {
        }
        // required for dynamic component
        LayoutEmptyComponent.type = "empty" /* empty */;
        LayoutEmptyComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-empty',
                        template: "\n    Layout-empty\n    <router-outlet></router-outlet>\n  "
                    }] }
        ];
        return LayoutEmptyComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LayoutComponent = /** @class */ (function () {
        function LayoutComponent() {
            this.isCollapsed = false;
        }
        LayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: ' abp-layout',
                        template: "<nav class=\"navbar navbar-expand-md navbar-dark bg-dark fixed-top\" id=\"main-navbar\">\n  <a class=\"navbar-brand\" routerLink=\"/\">MyProjectName</a>\n  <button class=\"navbar-toggler\" type=\"button\" [attr.aria-expanded]=\"!isCollapsed\" (click)=\"isCollapsed = !isCollapsed\">\n    <span class=\"navbar-toggler-icon\"></span>\n  </button>\n  <div class=\"collapse navbar-collapse\" id=\"main-navbar-collapse\" [ngbCollapse]=\"isCollapsed\">\n    <ng-content></ng-content>\n  </div>\n</nav>\n\n<div style=\"padding-top: 5rem;\" class=\"container\">\n  <router-outlet></router-outlet>\n</div>\n\n<abp-confirmation></abp-confirmation>\n<abp-toast></abp-toast>\n"
                    }] }
        ];
        return LayoutComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var maxLength = forms.Validators.maxLength, required$1 = forms.Validators.required, email = forms.Validators.email;
    var ProfileComponent = /** @class */ (function () {
        function ProfileComponent(fb, modalService, store) {
            this.fb = fb;
            this.modalService = modalService;
            this.store = store;
            this.visibleChange = new core.EventEmitter();
        }
        /**
         * @return {?}
         */
        ProfileComponent.prototype.buildForm = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.store
                .dispatch(new ng_core.ProfileGet())
                .pipe(operators.withLatestFrom(this.profile$), operators.take(1))
                .subscribe((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var _b = __read(_a, 2), profile = _b[1];
                _this.form = _this.fb.group({
                    userName: [profile.userName, [required$1, maxLength(256)]],
                    email: [profile.email, [required$1, email, maxLength(256)]],
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
            this.store.dispatch(new ng_core.ProfileUpdate(this.form.value)).subscribe((/**
             * @return {?}
             */
            function () { return _this.modalRef.close(); }));
        };
        /**
         * @return {?}
         */
        ProfileComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.buildForm();
            this.modalRef = this.modalService.open(this.modalContent);
            this.visibleChange.emit(true);
            rxjs.from(this.modalRef.result)
                .pipe(operators.take(1))
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
        ProfileComponent.prototype.setVisible = /**
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
            else if (visible.currentValue === false && this.modalService.hasOpenModals()) {
                this.modalRef.close();
            }
        };
        ProfileComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-profile',
                        template: "<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h4 class=\"modal-title\" id=\"modal-basic-title\">{{ 'AbpIdentity::PersonalInfo' | abpLocalization }}</h4>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form *ngIf=\"form\" [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n    <div class=\"modal-body\">\n      <div class=\"form-group\">\n        <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"username\" class=\"form-control\" formControlName=\"userName\" />\n      </div>\n      <div class=\"row\">\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\n            ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n          </div>\n        </div>\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\n            ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n          </div>\n        </div>\n      </div>\n      <div class=\"form-group\">\n        <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:EmailAddress' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\n        ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n"
                    }] }
        ];
        /** @nocollapse */
        ProfileComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: ngBootstrap.NgbModal },
            { type: store.Store }
        ]; };
        ProfileComponent.propDecorators = {
            visible: [{ type: core.Input }],
            visibleChange: [{ type: core.Output }],
            modalContent: [{ type: core.ViewChild, args: ['modalContent', { static: false },] }]
        };
        __decorate([
            store.Select(ng_core.ProfileState.getProfile),
            __metadata("design:type", rxjs.Observable)
        ], ProfileComponent.prototype, "profile$", void 0);
        return ProfileComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var LAYOUTS = [LayoutApplicationComponent, LayoutAccountComponent, LayoutEmptyComponent];
    var ThemeBasicModule = /** @class */ (function () {
        function ThemeBasicModule() {
        }
        ThemeBasicModule.decorators = [
            { type: core.NgModule, args: [{
                        declarations: __spread(LAYOUTS, [LayoutComponent, ChangePasswordComponent, ProfileComponent]),
                        imports: [
                            ng_core.CoreModule,
                            ng_theme_shared.ThemeSharedModule,
                            ngBootstrap.NgbCollapseModule,
                            ngBootstrap.NgbDropdownModule,
                            toast.ToastModule,
                            core$1.NgxValidateCoreModule,
                            store.NgxsModule.forFeature([LayoutState]),
                        ],
                        exports: __spread(LAYOUTS),
                        entryComponents: __spread(LAYOUTS),
                    },] }
        ];
        return ThemeBasicModule;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Layout;
    (function (Layout) {
        /**
         * @record
         */
        function State() { }
        Layout.State = State;
        /**
         * @record
         */
        function NavigationElement() { }
        Layout.NavigationElement = NavigationElement;
    })(Layout || (Layout = {}));

    exports.LAYOUTS = LAYOUTS;
    exports.LayoutAccountComponent = LayoutAccountComponent;
    exports.LayoutAddNavigationElement = LayoutAddNavigationElement;
    exports.LayoutApplicationComponent = LayoutApplicationComponent;
    exports.LayoutEmptyComponent = LayoutEmptyComponent;
    exports.LayoutState = LayoutState;
    exports.ThemeBasicModule = ThemeBasicModule;
    exports.ɵa = LayoutApplicationComponent;
    exports.ɵb = LayoutState;
    exports.ɵc = LayoutAccountComponent;
    exports.ɵd = LayoutEmptyComponent;
    exports.ɵe = LayoutComponent;
    exports.ɵf = ChangePasswordComponent;
    exports.ɵg = ProfileComponent;
    exports.ɵh = LayoutState;
    exports.ɵi = LayoutAddNavigationElement;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.theme.basic.umd.js.map
