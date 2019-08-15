(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@ng-bootstrap/ng-bootstrap'), require('@abp/ng.theme.shared'), require('@angular/forms'), require('@ngx-validate/core'), require('@ngxs/store'), require('snq'), require('@ngxs/router-plugin'), require('angular-oauth2-oidc'), require('just-compare'), require('rxjs'), require('rxjs/operators'), require('primeng/toast')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.theme.basic', ['exports', '@abp/ng.core', '@angular/core', '@ng-bootstrap/ng-bootstrap', '@abp/ng.theme.shared', '@angular/forms', '@ngx-validate/core', '@ngxs/store', 'snq', '@ngxs/router-plugin', 'angular-oauth2-oidc', 'just-compare', 'rxjs', 'rxjs/operators', 'primeng/toast'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.theme = global.abp.ng.theme || {}, global.abp.ng.theme.basic = {}), global.ng_core, global.ng.core, global.ngBootstrap, global.ng_theme_shared, global.ng.forms, global.core$1, global.store, global.snq, global.routerPlugin, global.angularOauth2Oidc, global.compare, global.rxjs, global.rxjs.operators, global.toast));
}(this, function (exports, ng_core, core, ngBootstrap, ng_theme_shared, forms, core$1, store, snq, routerPlugin, angularOauth2Oidc, compare, rxjs, operators, toast) { 'use strict';

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
        function ChangePasswordComponent(fb, store, toasterService) {
            this.fb = fb;
            this.store = store;
            this.toasterService = toasterService;
            this.visibleChange = new core.EventEmitter();
        }
        Object.defineProperty(ChangePasswordComponent.prototype, "visible", {
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
        ChangePasswordComponent.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            this.form = this.fb.group({
                password: ['', required],
                newPassword: ['', required],
                repeatNewPassword: ['', required],
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
                .dispatch(new ng_core.ChangePassword({
                currentPassword: this.form.get('password').value,
                newPassword: this.form.get('newPassword').value,
            }))
                .subscribe({
                next: (/**
                 * @return {?}
                 */
                function () {
                    _this.visible = false;
                    _this.form.reset();
                }),
                error: (/**
                 * @param {?} err
                 * @return {?}
                 */
                function (err) {
                    _this.toasterService.error(snq((/**
                     * @return {?}
                     */
                    function () { return err.error.error.message; }), 'AbpAccount::DefaultErrorMessage'), 'Error', {
                        life: 7000,
                    });
                }),
            });
        };
        /**
         * @return {?}
         */
        ChangePasswordComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            this.visible = true;
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
            else if (visible.currentValue === false && this.visible) {
                this.visible = false;
            }
        };
        ChangePasswordComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-change-password',
                        template: "<abp-modal [(visible)]=\"visible\">\n  <ng-template #abpHeader>\n    <h4>{{ 'AbpIdentity::ChangePassword' | abpLocalization }}</h4>\n  </ng-template>\n  <ng-template #abpBody>\n    <form [formGroup]=\"form\" novalidate>\n      <div class=\"form-group\">\n        <label for=\"current-password\">{{ 'AbpIdentity::DisplayName:CurrentPassword' | abpLocalization }}</label\n        ><span> * </span\n        ><input type=\"password\" id=\"current-password\" class=\"form-control\" formControlName=\"password\" autofocus />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"new-password\">{{ 'AbpIdentity::DisplayName:NewPassword' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"new-password\" class=\"form-control\" formControlName=\"newPassword\" />\n      </div>\n      <div class=\"form-group\" [class.is-invalid]=\"form.errors?.passwordMismatch\">\n        <label for=\"confirm-new-password\">{{ 'AbpIdentity::DisplayName:NewPasswordConfirm' | abpLocalization }}</label\n        ><span> * </span\n        ><input type=\"password\" id=\"confirm-new-password\" class=\"form-control\" formControlName=\"repeatNewPassword\" />\n        <div *ngIf=\"form.errors?.passwordMismatch\" class=\"invalid-feedback\">\n          {{ 'AbpIdentity::Identity.PasswordConfirmationFailed' | abpLocalization }}\n        </div>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <abp-button\n      [requestType]=\"['POST']\"\n      requestURLContainSearchValue=\"change-password\"\n      iconClass=\"fa fa-check\"\n      (click)=\"onSubmit()\"\n      >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n    >\n  </ng-template>\n</abp-modal>\n"
                    }] }
        ];
        /** @nocollapse */
        ChangePasswordComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: store.Store },
            { type: ng_theme_shared.ToasterService }
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
    var AccountLayoutComponent = /** @class */ (function () {
        function AccountLayoutComponent() {
            this.isCollapsed = false;
        }
        // required for dynamic component
        AccountLayoutComponent.type = "account" /* account */;
        AccountLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-account',
                        template: "<abp-layout>\n  <ul class=\"navbar-nav mr-auto\">\n    <li class=\"nav-item\">\n      <a class=\"nav-link\" href=\"/\">\n        {{ '::Menu:Home' | abpLocalization }}\n      </a>\n    </li>\n  </ul>\n\n  <span id=\"main-navbar-tools\">\n    <span>\n      <div class=\"dropdown d-inline\" ngbDropdown>\n        <a class=\"btn btn-link dropdown-toggle\" role=\"button\" data-toggle=\"dropdown\" ngbDropdownToggle>\n          English\n        </a>\n\n        <div class=\"dropdown-menu\" ngbDropdownMenu>\n          <a class=\"dropdown-item\">\u010Ce\u0161tina</a>\n          <a class=\"dropdown-item\">Portugu\u00EAs</a>\n          <a class=\"dropdown-item\">T\u00FCrk\u00E7e</a>\n          <a class=\"dropdown-item\">\u7B80\u4F53\u4E2D\u6587</a>\n        </div>\n      </div>\n    </span>\n  </span>\n</abp-layout>\n"
                    }] }
        ];
        return AccountLayoutComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var AddNavigationElement = /** @class */ (function () {
        function AddNavigationElement(payload) {
            this.payload = payload;
        }
        AddNavigationElement.type = '[Layout] Add Navigation Element';
        return AddNavigationElement;
    }());
    var RemoveNavigationElementByName = /** @class */ (function () {
        function RemoveNavigationElementByName(name) {
            this.name = name;
        }
        RemoveNavigationElementByName.type = '[Layout] Remove Navigation ElementByName';
        return RemoveNavigationElementByName;
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
        LayoutState.prototype.layoutAddAction = /**
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
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        LayoutState.prototype.layoutRemoveAction = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var getState = _a.getState, patchState = _a.patchState;
            var name = _b.name;
            var navigationElements = getState().navigationElements;
            /** @type {?} */
            var index = navigationElements.findIndex((/**
             * @param {?} element
             * @return {?}
             */
            function (element) { return element.name === name; }));
            if (index > -1) {
                navigationElements = navigationElements.splice(index, 1);
            }
            return patchState({
                navigationElements: navigationElements,
            });
        };
        __decorate([
            store.Action(AddNavigationElement),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, AddNavigationElement]),
            __metadata("design:returntype", void 0)
        ], LayoutState.prototype, "layoutAddAction", null);
        __decorate([
            store.Action(RemoveNavigationElementByName),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, RemoveNavigationElementByName]),
            __metadata("design:returntype", void 0)
        ], LayoutState.prototype, "layoutRemoveAction", null);
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
    var ApplicationLayoutComponent = /** @class */ (function () {
        function ApplicationLayoutComponent(store, oauthService) {
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
        Object.defineProperty(ApplicationLayoutComponent.prototype, "visibleRoutes$", {
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
        Object.defineProperty(ApplicationLayoutComponent.prototype, "defaultLanguage$", {
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
        Object.defineProperty(ApplicationLayoutComponent.prototype, "dropdownLanguages$", {
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
        Object.defineProperty(ApplicationLayoutComponent.prototype, "selectedLangCulture", {
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
         * @private
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.checkWindowWidth = /**
         * @private
         * @return {?}
         */
        function () {
            var _this = this;
            setTimeout((/**
             * @return {?}
             */
            function () {
                _this.navbarRootDropdowns.forEach((/**
                 * @param {?} item
                 * @return {?}
                 */
                function (item) {
                    item.close();
                }));
                if (window.innerWidth < 768) {
                    _this.isDropdownChildDynamic = false;
                }
                else {
                    _this.isDropdownChildDynamic = true;
                }
            }), 0);
        };
        /**
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.ngAfterViewInit = /**
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
                this.store.dispatch(new AddNavigationElement([
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
            this.checkWindowWidth();
            rxjs.fromEvent(window, 'resize')
                .pipe(ng_core.takeUntilDestroy(this), operators.debounceTime(250))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.checkWindowWidth();
            }));
        };
        /**
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        /**
         * @param {?} cultureName
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.onChangeLang = /**
         * @param {?} cultureName
         * @return {?}
         */
        function (cultureName) {
            this.store.dispatch(new ng_core.SetLanguage(cultureName));
            this.store.dispatch(new ng_core.GetAppConfiguration());
        };
        /**
         * @return {?}
         */
        ApplicationLayoutComponent.prototype.logout = /**
         * @return {?}
         */
        function () {
            this.oauthService.logOut();
            this.store.dispatch(new routerPlugin.Navigate(['/'], null, {
                state: { redirectUrl: this.store.selectSnapshot(routerPlugin.RouterState).state.url },
            }));
            this.store.dispatch(new ng_core.GetAppConfiguration());
        };
        // required for dynamic component
        ApplicationLayoutComponent.type = "application" /* application */;
        ApplicationLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-application',
                        template: "<abp-layout>\n  <ul class=\"navbar-nav mr-auto\">\n    <ng-container\n      *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\n      [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\n      [ngTemplateOutletContext]=\"{ $implicit: route }\"\n    >\n    </ng-container>\n\n    <ng-template #defaultLink let-route>\n      <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\n        <a class=\"nav-link\" [routerLink]=\"[route.url]\">{{ route.name | abpLocalization }}</a>\n      </li>\n    </ng-template>\n\n    <ng-template #dropdownLink let-route>\n      <li\n        #navbarRootDropdown\n        ngbDropdown\n        [abpPermission]=\"route.requiredPolicy\"\n        [abpVisibility]=\"routeContainer\"\n        class=\"nav-item dropdown pointer\"\n        display=\"static\"\n      >\n        <a ngbDropdownToggle class=\"nav-link dropdown-toggle pointer\" data-toggle=\"dropdown\">\n          {{ route.name | abpLocalization }}\n        </a>\n        <div #routeContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            #forTemplate\n            ngFor\n            [ngForOf]=\"route.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </li>\n    </ng-template>\n\n    <ng-template #childWrapper let-child>\n      <ng-template\n        [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\n        [ngTemplateOutletContext]=\"{ $implicit: child }\"\n      ></ng-template>\n    </ng-template>\n\n    <ng-template #defaultChild let-child>\n      <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\n        <a class=\"dropdown-item py-2 px-2\" [routerLink]=\"[child.url]\">\n          <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\n          {{ child.name | abpLocalization }}</a\n        >\n      </div>\n    </ng-template>\n\n    <ng-template #dropdownChild let-child>\n      <div\n        [abpVisibility]=\"childrenContainer\"\n        class=\"dropdown-submenu pointer\"\n        ngbDropdown\n        [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\n        placement=\"right-top\"\n        [abpPermission]=\"child.requiredPolicy\"\n      >\n        <div ngbDropdownToggle [class.dropdown-toggle]=\"false\" class=\"pointer\">\n          <a\n            abpEllipsis=\"200px\"\n            [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\n            role=\"button\"\n            class=\"btn d-block text-left py-2 px-2 dropdown-toggle\"\n          >\n            <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\n            {{ child.name | abpLocalization }}\n          </a>\n        </div>\n        <div #childrenContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            ngFor\n            [ngForOf]=\"child.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </div>\n    </ng-template>\n  </ul>\n\n  <ul class=\"navbar-nav ml-auto\">\n    <ng-container\n      *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\n      [ngTemplateOutlet]=\"element\"\n    ></ng-container>\n  </ul>\n</abp-layout>\n\n<ng-template #language>\n  <li class=\"nav-item dropdown pointer\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle text-white pointer\" data-toggle=\"dropdown\">\n      {{ defaultLanguage$ | async }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a\n        *ngFor=\"let lang of dropdownLanguages$ | async\"\n        class=\"dropdown-item\"\n        (click)=\"onChangeLang(lang.cultureName)\"\n        >{{ lang?.displayName }}</a\n      >\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item dropdown pointer\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle text-white pointer\" data-toggle=\"dropdown\">\n      {{ (currentUser$ | async)?.userName }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenChangePassword = true\">{{\n        'AbpUi::ChangePassword' | abpLocalization\n      }}</a>\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenProfile = true\">{{ 'AbpUi::PersonalInfo' | abpLocalization }}</a>\n      <a class=\"dropdown-item pointer\" (click)=\"logout()\">{{ 'AbpUi::Logout' | abpLocalization }}</a>\n    </div>\n  </li>\n\n  <abp-change-password [(visible)]=\"isOpenChangePassword\"></abp-change-password>\n\n  <abp-profile [(visible)]=\"isOpenProfile\"></abp-profile>\n</ng-template>\n"
                    }] }
        ];
        /** @nocollapse */
        ApplicationLayoutComponent.ctorParameters = function () { return [
            { type: store.Store },
            { type: angularOauth2Oidc.OAuthService }
        ]; };
        ApplicationLayoutComponent.propDecorators = {
            currentUserRef: [{ type: core.ViewChild, args: ['currentUser', { static: false, read: core.TemplateRef },] }],
            languageRef: [{ type: core.ViewChild, args: ['language', { static: false, read: core.TemplateRef },] }],
            navbarRootDropdowns: [{ type: core.ViewChildren, args: ['navbarRootDropdown', { read: ngBootstrap.NgbDropdown },] }]
        };
        __decorate([
            store.Select(ng_core.ConfigState.getOne('routes')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "routes$", void 0);
        __decorate([
            store.Select(ng_core.ConfigState.getOne('currentUser')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "currentUser$", void 0);
        __decorate([
            store.Select(ng_core.ConfigState.getDeep('localization.languages')),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "languages$", void 0);
        __decorate([
            store.Select(LayoutState.getNavigationElements),
            __metadata("design:type", rxjs.Observable)
        ], ApplicationLayoutComponent.prototype, "navElements$", void 0);
        return ApplicationLayoutComponent;
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
    var EmptyLayoutComponent = /** @class */ (function () {
        function EmptyLayoutComponent() {
        }
        // required for dynamic component
        EmptyLayoutComponent.type = "empty" /* empty */;
        EmptyLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-layout-empty',
                        template: "\n    Layout-empty\n    <router-outlet></router-outlet>\n  "
                    }] }
        ];
        return EmptyLayoutComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LayoutComponent = /** @class */ (function () {
        function LayoutComponent(store) {
            this.store = store;
            this.isCollapsed = false;
        }
        Object.defineProperty(LayoutComponent.prototype, "appInfo", {
            get: /**
             * @return {?}
             */
            function () {
                return this.store.selectSnapshot(ng_core.ConfigState.getApplicationInfo);
            },
            enumerable: true,
            configurable: true
        });
        LayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: ' abp-layout',
                        template: "<nav class=\"navbar navbar-expand-md navbar-dark bg-dark fixed-top\" id=\"main-navbar\">\n  <a class=\"navbar-brand\" routerLink=\"/\">\n    <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\n  </a>\n  <button class=\"navbar-toggler\" type=\"button\" [attr.aria-expanded]=\"!isCollapsed\" (click)=\"isCollapsed = !isCollapsed\">\n    <span class=\"navbar-toggler-icon\"></span>\n  </button>\n  <div class=\"collapse navbar-collapse\" id=\"main-navbar-collapse\" [ngbCollapse]=\"isCollapsed\">\n    <ng-content></ng-content>\n  </div>\n</nav>\n\n<div\n  [@routeAnimations]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\"\n  style=\"padding-top: 5rem;\"\n  class=\"container\"\n>\n  <router-outlet #outlet=\"outlet\"></router-outlet>\n</div>\n\n<abp-confirmation></abp-confirmation>\n<abp-toast></abp-toast>\n\n<ng-template #appName>\n  {{ appInfo.name }}\n</ng-template>\n",
                        animations: [ng_theme_shared.slideFromBottom]
                    }] }
        ];
        /** @nocollapse */
        LayoutComponent.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        return LayoutComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var maxLength = forms.Validators.maxLength, required$1 = forms.Validators.required, email = forms.Validators.email;
    var ProfileComponent = /** @class */ (function () {
        function ProfileComponent(fb, store) {
            this.fb = fb;
            this.store = store;
            this.visibleChange = new core.EventEmitter();
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
                .dispatch(new ng_core.GetProfile())
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
            this.store.dispatch(new ng_core.UpdateProfile(this.form.value)).subscribe((/**
             * @return {?}
             */
            function () {
                _this.visible = false;
                _this.form.reset();
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
            { type: core.Component, args: [{
                        selector: 'abp-profile',
                        template: "<abp-modal [(visible)]=\"visible\">\n  <ng-template #abpHeader>\n    <h4>{{ 'AbpIdentity::PersonalInfo' | abpLocalization }}</h4>\n  </ng-template>\n  <ng-template #abpBody>\n    <form *ngIf=\"form\" [formGroup]=\"form\" novalidate>\n      <div class=\"form-group\">\n        <label for=\"username\">{{ 'AbpIdentity::DisplayName:UserName' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"username\" class=\"form-control\" formControlName=\"userName\" autofocus />\n      </div>\n      <div class=\"row\">\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label\n            ><input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n          </div>\n        </div>\n        <div class=\"col col-md-6\">\n          <div class=\"form-group\">\n            <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label\n            ><input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n          </div>\n        </div>\n      </div>\n      <div class=\"form-group\">\n        <label for=\"email-address\">{{ 'AbpIdentity::DisplayName:Email' | abpLocalization }}</label\n        ><span> * </span><input type=\"text\" id=\"email-address\" class=\"form-control\" formControlName=\"email\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"phone-number\">{{ 'AbpIdentity::DisplayName:PhoneNumber' | abpLocalization }}</label\n        ><input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <abp-button\n      [requestType]=\"['PUT']\"\n      requestURLContainSearchValue=\"my-profile\"\n      iconClass=\"fa fa-check\"\n      (click)=\"onSubmit()\"\n      >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n    >\n  </ng-template>\n</abp-modal>\n"
                    }] }
        ];
        /** @nocollapse */
        ProfileComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: store.Store }
        ]; };
        ProfileComponent.propDecorators = {
            visible: [{ type: core.Input }],
            visibleChange: [{ type: core.Output }]
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
    var LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];
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

    exports.AccountLayoutComponent = AccountLayoutComponent;
    exports.AddNavigationElement = AddNavigationElement;
    exports.ApplicationLayoutComponent = ApplicationLayoutComponent;
    exports.EmptyLayoutComponent = EmptyLayoutComponent;
    exports.LAYOUTS = LAYOUTS;
    exports.LayoutState = LayoutState;
    exports.RemoveNavigationElementByName = RemoveNavigationElementByName;
    exports.ThemeBasicModule = ThemeBasicModule;
    exports.ɵa = ApplicationLayoutComponent;
    exports.ɵb = LayoutState;
    exports.ɵc = AccountLayoutComponent;
    exports.ɵd = EmptyLayoutComponent;
    exports.ɵe = LayoutComponent;
    exports.ɵf = ChangePasswordComponent;
    exports.ɵg = ProfileComponent;
    exports.ɵh = LayoutState;
    exports.ɵi = AddNavigationElement;
    exports.ɵj = RemoveNavigationElementByName;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.theme.basic.umd.js.map
