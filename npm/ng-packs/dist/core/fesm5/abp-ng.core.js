import { __rest, __assign, __decorate, __metadata, __spread } from 'tslib';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, Component, Directive, ElementRef, Optional, Renderer2, Input, InjectionToken, Inject, Pipe, NgModule, APP_INITIALIZER, Injector } from '@angular/core';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { Store, Action, Selector, State, createSelector, Select, actionMatcher, InitState, UpdateState, setValue, NgxsModule, NGXS_PLUGINS } from '@ngxs/store';
import { NEVER, throwError, of, Subject, Observable, ReplaySubject } from 'rxjs';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { take, catchError, tap, switchMap, takeUntil, finalize, distinctUntilChanged } from 'rxjs/operators';
import snq from 'snq';
import { OAuthService } from 'angular-oauth2-oidc';
import { Navigate, NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var PatchRouteByName = /** @class */ (function () {
    function PatchRouteByName(name, newValue) {
        this.name = name;
        this.newValue = newValue;
    }
    PatchRouteByName.type = '[Config] Patch Route By Name';
    return PatchRouteByName;
}());
var ConfigGetAppConfiguration = /** @class */ (function () {
    function ConfigGetAppConfiguration() {
    }
    ConfigGetAppConfiguration.type = '[Config] Get App Configuration';
    return ConfigGetAppConfiguration;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var LoaderStart = /** @class */ (function () {
    function LoaderStart(payload) {
        this.payload = payload;
    }
    LoaderStart.type = '[Loader] Start';
    return LoaderStart;
}());
var LoaderStop = /** @class */ (function () {
    function LoaderStop(payload) {
        this.payload = payload;
    }
    LoaderStop.type = '[Loader] Stop';
    return LoaderStop;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ProfileGet = /** @class */ (function () {
    function ProfileGet() {
    }
    ProfileGet.type = '[Profile] Get';
    return ProfileGet;
}());
var ProfileUpdate = /** @class */ (function () {
    function ProfileUpdate(payload) {
        this.payload = payload;
    }
    ProfileUpdate.type = '[Profile] Update';
    return ProfileUpdate;
}());
var ProfileChangePassword = /** @class */ (function () {
    function ProfileChangePassword(payload) {
        this.payload = payload;
    }
    ProfileChangePassword.type = '[Profile] Change Password';
    return ProfileChangePassword;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var RestOccurError = /** @class */ (function () {
    function RestOccurError(payload) {
        this.payload = payload;
    }
    RestOccurError.type = '[Rest] Error';
    return RestOccurError;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var SessionSetLanguage = /** @class */ (function () {
    function SessionSetLanguage(payload) {
        this.payload = payload;
    }
    SessionSetLanguage.type = '[Session] Set Language';
    return SessionSetLanguage;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var RestService = /** @class */ (function () {
    function RestService(http, store) {
        this.http = http;
        this.store = store;
    }
    /**
     * @param {?} err
     * @return {?}
     */
    RestService.prototype.handleError = /**
     * @param {?} err
     * @return {?}
     */
    function (err) {
        this.store.dispatch(new RestOccurError(err));
        console.error(err);
        return NEVER;
    };
    /**
     * @template T, R
     * @param {?} request
     * @param {?=} config
     * @param {?=} api
     * @return {?}
     */
    RestService.prototype.request = /**
     * @template T, R
     * @param {?} request
     * @param {?=} config
     * @param {?=} api
     * @return {?}
     */
    function (request, config, api) {
        var _this = this;
        if (config === void 0) { config = {}; }
        var _a = config.observe, observe = _a === void 0 ? "body" /* Body */ : _a, throwErr = config.throwErr;
        /** @type {?} */
        var url = api || this.store.selectSnapshot(ConfigState.getApiUrl()) + request.url;
        var method = request.method, options = __rest(request, ["method"]);
        return this.http.request(method, url, (/** @type {?} */ (__assign({ observe: observe }, options)))).pipe(observe === "body" /* Body */ ? take(1) : null, catchError((/**
         * @param {?} err
         * @return {?}
         */
        function (err) {
            if (throwErr) {
                return throwError(err);
            }
            return _this.handleError(err);
        })));
    };
    RestService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    RestService.ctorParameters = function () { return [
        { type: HttpClient },
        { type: Store }
    ]; };
    /** @nocollapse */ RestService.ngInjectableDef = ɵɵdefineInjectable({ factory: function RestService_Factory() { return new RestService(ɵɵinject(HttpClient), ɵɵinject(Store)); }, token: RestService, providedIn: "root" });
    return RestService;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ProfileService = /** @class */ (function () {
    function ProfileService(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    ProfileService.prototype.get = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/identity/my-profile',
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    ProfileService.prototype.update = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var request = {
            method: 'PUT',
            url: '/api/identity/my-profile',
            body: body,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    ProfileService.prototype.changePassword = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var request = {
            method: 'POST',
            url: '/api/identity/my-profile/changePassword',
            body: body,
        };
        return this.rest.request(request);
    };
    ProfileService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    ProfileService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ ProfileService.ngInjectableDef = ɵɵdefineInjectable({ factory: function ProfileService_Factory() { return new ProfileService(ɵɵinject(RestService)); }, token: ProfileService, providedIn: "root" });
    return ProfileService;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ProfileState = /** @class */ (function () {
    function ProfileState(profileService) {
        this.profileService = profileService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    ProfileState.getProfile = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var profile = _a.profile;
        return profile;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    ProfileState.prototype.profileGet = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var patchState = _a.patchState;
        return this.profileService.get().pipe(tap((/**
         * @param {?} profile
         * @return {?}
         */
        function (profile) {
            return patchState({
                profile: profile,
            });
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    ProfileState.prototype.profileUpdate = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.profileService.update(payload).pipe(tap((/**
         * @param {?} profile
         * @return {?}
         */
        function (profile) {
            return patchState({
                profile: profile,
            });
        })));
    };
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    ProfileState.prototype.changePassword = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.profileService.changePassword(payload);
    };
    __decorate([
        Action(ProfileGet),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", void 0)
    ], ProfileState.prototype, "profileGet", null);
    __decorate([
        Action(ProfileUpdate),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, ProfileUpdate]),
        __metadata("design:returntype", void 0)
    ], ProfileState.prototype, "profileUpdate", null);
    __decorate([
        Action(ProfileChangePassword),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, ProfileChangePassword]),
        __metadata("design:returntype", void 0)
    ], ProfileState.prototype, "changePassword", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", Object)
    ], ProfileState, "getProfile", null);
    ProfileState = __decorate([
        State({
            name: 'ProfileState',
            defaults: (/** @type {?} */ ({})),
        }),
        __metadata("design:paramtypes", [ProfileService])
    ], ProfileState);
    return ProfileState;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ApplicationConfigurationService = /** @class */ (function () {
    function ApplicationConfigurationService(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    ApplicationConfigurationService.prototype.getConfiguration = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/abp/application-configuration',
        };
        return this.rest.request(request);
    };
    ApplicationConfigurationService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    ApplicationConfigurationService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ ApplicationConfigurationService.ngInjectableDef = ɵɵdefineInjectable({ factory: function ApplicationConfigurationService_Factory() { return new ApplicationConfigurationService(ɵɵinject(RestService)); }, token: ApplicationConfigurationService, providedIn: "root" });
    return ApplicationConfigurationService;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var SessionState = /** @class */ (function () {
    function SessionState() {
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    SessionState.getLanguage = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var language = _a.language;
        return language;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    SessionState.prototype.sessionSetLanguage = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        patchState({
            language: payload,
        });
    };
    __decorate([
        Action(SessionSetLanguage),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, SessionSetLanguage]),
        __metadata("design:returntype", void 0)
    ], SessionState.prototype, "sessionSetLanguage", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", String)
    ], SessionState, "getLanguage", null);
    SessionState = __decorate([
        State({
            name: 'SessionState',
            defaults: (/** @type {?} */ ({})),
        }),
        __metadata("design:paramtypes", [])
    ], SessionState);
    return SessionState;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @param {?} routes
 * @param {?=} wrappers
 * @param {?=} parentNameArr
 * @param {?=} parentName
 * @return {?}
 */
function organizeRoutes(routes, wrappers, parentNameArr, parentName) {
    if (wrappers === void 0) { wrappers = []; }
    if (parentNameArr === void 0) { parentNameArr = (/** @type {?} */ ([])); }
    if (parentName === void 0) { parentName = null; }
    /** @type {?} */
    var filter = (/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.children) {
            route.children = organizeRoutes(route.children, wrappers, parentNameArr, route.name);
        }
        if (route.parentName && route.parentName !== parentName) {
            parentNameArr.push(route);
            return false;
        }
        return true;
    });
    if (parentName) {
        // recursive block
        return routes.filter(filter);
    }
    /** @type {?} */
    var filteredRoutes = routes.filter(filter);
    if (parentNameArr.length) {
        return sortRoutes(setChildRoute(__spread(filteredRoutes, wrappers), parentNameArr));
    }
    return filteredRoutes;
}
/**
 * @param {?} routes
 * @param {?} parentNameArr
 * @return {?}
 */
function setChildRoute(routes, parentNameArr) {
    return routes
        .map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.children && route.children.length) {
            route.children = setChildRoute(route.children, parentNameArr);
        }
        /** @type {?} */
        var foundedChildren = parentNameArr.filter((/**
         * @param {?} parent
         * @return {?}
         */
        function (parent) { return parent.parentName === route.name; }));
        if (foundedChildren && foundedChildren.length) {
            route.children = __spread((route.children || []), foundedChildren);
        }
        return route;
    }))
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    function (route) { return route.path || (route.children && route.children.length); }));
}
/**
 * @param {?=} routes
 * @return {?}
 */
function sortRoutes(routes) {
    if (routes === void 0) { routes = []; }
    if (!routes.length)
        return [];
    return routes
        .sort((/**
     * @param {?} a
     * @param {?} b
     * @return {?}
     */
    function (a, b) { return a.order - b.order; }))
        .map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.children && route.children.length) {
            route.children = sortRoutes(route.children);
        }
        return route;
    }));
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ConfigState = /** @class */ (function () {
    function ConfigState(appConfigurationService, store) {
        this.appConfigurationService = appConfigurationService;
        this.store = store;
    }
    ConfigState_1 = ConfigState;
    /**
     * @param {?} state
     * @return {?}
     */
    ConfigState.getAll = /**
     * @param {?} state
     * @return {?}
     */
    function (state) {
        return state;
    };
    /**
     * @param {?} key
     * @return {?}
     */
    ConfigState.getOne = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return state[key];
        }));
        return selector;
    };
    /**
     * @param {?} keys
     * @return {?}
     */
    ConfigState.getDeep = /**
     * @param {?} keys
     * @return {?}
     */
    function (keys) {
        if (typeof keys === 'string') {
            keys = keys.split('.');
        }
        if (!Array.isArray(keys)) {
            throw new Error('The argument must be a dot string or an string array.');
        }
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return ((/** @type {?} */ (keys))).reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) {
                if (acc) {
                    return acc[val];
                }
                return undefined;
            }), state);
        }));
        return selector;
    };
    /**
     * @param {?=} key
     * @return {?}
     */
    ConfigState.getApiUrl = /**
     * @param {?=} key
     * @return {?}
     */
    function (key) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return state.environment.apis[key || 'default'].url;
        }));
        return selector;
    };
    /**
     * @param {?} key
     * @return {?}
     */
    ConfigState.getSetting = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return snq((/**
             * @return {?}
             */
            function () { return state.setting.values[key]; }));
        }));
        return selector;
    };
    /**
     * @param {?=} condition
     * @return {?}
     */
    ConfigState.getGrantedPolicy = /**
     * @param {?=} condition
     * @return {?}
     */
    function (condition) {
        if (condition === void 0) { condition = ''; }
        /** @type {?} */
        var keys = condition
            .replace(/\(|\)|\!|\s/g, '')
            .split(/\|\||&&/)
            .filter((/**
         * @param {?} key
         * @return {?}
         */
        function (key) { return key; }));
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            if (!keys.length)
                return true;
            /** @type {?} */
            var getPolicy = (/**
             * @param {?} key
             * @return {?}
             */
            function (key) { return snq((/**
             * @return {?}
             */
            function () { return state.auth.grantedPolicies[key]; }), false); });
            if (keys.length > 1) {
                keys.forEach((/**
                 * @param {?} key
                 * @return {?}
                 */
                function (key) {
                    /** @type {?} */
                    var value = getPolicy(key);
                    condition = condition.replace(key, value);
                }));
                // tslint:disable-next-line: no-eval
                return eval("!!" + condition);
            }
            return getPolicy(condition);
        }));
        return selector;
    };
    /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    ConfigState.getCopy = /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (key) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        /** @type {?} */
        var keys = (/** @type {?} */ (key.split('::')));
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            var defaultResourceName = state.environment.localization.defaultResourceName;
            if (keys[0] === '') {
                if (!defaultResourceName) {
                    throw new Error("Please check your environment. May you forget set defaultResourceName? \n              Here is the example:\n               { production: false,\n                 localization: {\n                   defaultResourceName: 'MyProjectName'\n                  }\n               }");
                }
                keys[0] = snq((/**
                 * @return {?}
                 */
                function () { return defaultResourceName; }));
            }
            /** @type {?} */
            var copy = keys.reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) {
                if (acc) {
                    return acc[val];
                }
                return undefined;
            }), state.localization.values);
            if (copy && interpolateParams && interpolateParams.length) {
                interpolateParams.forEach((/**
                 * @param {?} param
                 * @param {?} index
                 * @return {?}
                 */
                function (param, index) {
                    copy = copy.replace("'{" + index + "}'", param);
                }));
            }
            return copy || key;
        }));
        return selector;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    ConfigState.prototype.addData = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var _this = this;
        var patchState = _a.patchState, dispatch = _a.dispatch;
        return this.appConfigurationService.getConfiguration().pipe(tap((/**
         * @param {?} configuration
         * @return {?}
         */
        function (configuration) {
            return patchState(__assign({}, configuration));
        })), switchMap((/**
         * @param {?} configuration
         * @return {?}
         */
        function (configuration) {
            return _this.store.selectSnapshot(SessionState.getLanguage)
                ? of(null)
                : dispatch(new SessionSetLanguage(snq((/**
                 * @return {?}
                 */
                function () { return configuration.setting.values['Abp.Localization.DefaultLanguage']; }))));
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    ConfigState.prototype.patchRoute = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState, getState = _a.getState;
        var name = _b.name, newValue = _b.newValue;
        /** @type {?} */
        var routes = getState().routes;
        /** @type {?} */
        var index = routes.findIndex((/**
         * @param {?} route
         * @return {?}
         */
        function (route) { return route.name === name; }));
        routes = patchRouteDeep(routes, name, newValue);
        return patchState({
            routes: routes,
        });
    };
    var ConfigState_1;
    __decorate([
        Action(ConfigGetAppConfiguration),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", void 0)
    ], ConfigState.prototype, "addData", null);
    __decorate([
        Action(PatchRouteByName),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, PatchRouteByName]),
        __metadata("design:returntype", void 0)
    ], ConfigState.prototype, "patchRoute", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", void 0)
    ], ConfigState, "getAll", null);
    ConfigState = ConfigState_1 = __decorate([
        State({
            name: 'ConfigState',
            defaults: (/** @type {?} */ ({})),
        }),
        __metadata("design:paramtypes", [ApplicationConfigurationService, Store])
    ], ConfigState);
    return ConfigState;
}());
/**
 * @param {?} routes
 * @param {?} name
 * @param {?} newValue
 * @param {?=} parentUrl
 * @return {?}
 */
function patchRouteDeep(routes, name, newValue, parentUrl) {
    if (parentUrl === void 0) { parentUrl = null; }
    routes = routes.map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.name === name) {
            if (newValue.path) {
                newValue.url = parentUrl + "/" + newValue.path;
            }
            if (newValue.children && newValue.children.length) {
                newValue.children = newValue.children.map((/**
                 * @param {?} child
                 * @return {?}
                 */
                function (child) { return (__assign({}, child, { url: parentUrl + "/" + route.path + "/" + child.path })); }));
            }
            return __assign({}, route, newValue);
        }
        else if (route.children && route.children.length) {
            route.children = patchRouteDeep(route.children, name, newValue, (parentUrl || '/') + route.path);
        }
        return route;
    }));
    if (parentUrl) {
        // recursive block
        return routes;
    }
    return organizeRoutes(routes);
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @param {?=} a
 * @return {?}
 */
function uuid(a) {
    return a
        ? (a ^ ((Math.random() * 16) >> (a / 4))).toString(16)
        : ('' + 1e7 + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, uuid);
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @param {?} injector
 * @return {?}
 */
function getInitialData(injector) {
    /** @type {?} */
    var fn = (/**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var store = injector.get(Store);
        return store.dispatch(new ConfigGetAppConfiguration()).toPromise();
    });
    return fn;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @param {?} value
 * @return {?}
 */
function isFunction(value) {
    return typeof value === 'function';
}
/** @type {?} */
var takeUntilDestroy = (/**
 * @param {?} componentInstance
 * @param {?=} destroyMethodName
 * @return {?}
 */
function (componentInstance, destroyMethodName) {
    if (destroyMethodName === void 0) { destroyMethodName = 'ngOnDestroy'; }
    return (/**
     * @template T
     * @param {?} source
     * @return {?}
     */
    function (source) {
        /** @type {?} */
        var originalDestroy = componentInstance[destroyMethodName];
        if (isFunction(originalDestroy) === false) {
            throw new Error(componentInstance.constructor.name + " is using untilDestroyed but doesn't implement " + destroyMethodName);
        }
        if (!componentInstance['__takeUntilDestroy']) {
            componentInstance['__takeUntilDestroy'] = new Subject();
            componentInstance[destroyMethodName] = (/**
             * @return {?}
             */
            function () {
                isFunction(originalDestroy) && originalDestroy.apply(this, arguments);
                componentInstance['__takeUntilDestroy'].next(true);
                componentInstance['__takeUntilDestroy'].complete();
            });
        }
        return source.pipe(takeUntil(componentInstance['__takeUntilDestroy']));
    });
});

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var DynamicLayoutComponent = /** @class */ (function () {
    function DynamicLayoutComponent(router, store) {
        var _this = this;
        this.router = router;
        this.store = store;
        this.router.events.pipe(takeUntilDestroy(this)).subscribe((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            if (event instanceof NavigationEnd) {
                var segments = _this.router.parseUrl(event.url).root.children.primary.segments;
                var _a = _this.store.selectSnapshot(ConfigState.getAll), layouts = _a.requirements.layouts, routes = _a.routes;
                /** @type {?} */
                var layout_1 = findLayout(segments, routes);
                _this.layout = layouts.filter((/**
                 * @param {?} l
                 * @return {?}
                 */
                function (l) { return !!l; })).find((/**
                 * @param {?} l
                 * @return {?}
                 */
                function (l) { return snq((/**
                 * @return {?}
                 */
                function () { return l.type.toLowerCase().indexOf(layout_1); }), -1) > -1; }));
            }
        }));
    }
    /**
     * @return {?}
     */
    DynamicLayoutComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    DynamicLayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-dynamic-layout',
                    template: "\n    <ng-container *ngTemplateOutlet=\"layout ? componentOutlet : routerOutlet\"></ng-container>\n\n    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>\n    <ng-template #componentOutlet><ng-container *ngComponentOutlet=\"layout\"></ng-container></ng-template>\n  "
                }] }
    ];
    /** @nocollapse */
    DynamicLayoutComponent.ctorParameters = function () { return [
        { type: Router },
        { type: Store }
    ]; };
    __decorate([
        Select(ConfigState.getOne('requirements')),
        __metadata("design:type", Observable)
    ], DynamicLayoutComponent.prototype, "requirements$", void 0);
    return DynamicLayoutComponent;
}());
/**
 * @param {?} segments
 * @param {?} routes
 * @return {?}
 */
function findLayout(segments, routes) {
    /** @type {?} */
    var layout = "empty" /* empty */;
    /** @type {?} */
    var route = routes
        .reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    function (acc, val) { return (val.wrapper ? __spread(acc, val.children) : __spread(acc, [val])); }), [])
        .find((/**
     * @param {?} r
     * @return {?}
     */
    function (r) { return r.path === segments[0].path; }));
    if (route) {
        if (route.layout) {
            layout = route.layout;
        }
        if (route.children && route.children.length) {
            /** @type {?} */
            var child = route.children.find((/**
             * @param {?} c
             * @return {?}
             */
            function (c) { return c.path === segments[1].path; }));
            if (child.layout) {
                layout = child.layout;
            }
        }
    }
    return layout;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var RouterOutletComponent = /** @class */ (function () {
    function RouterOutletComponent() {
    }
    RouterOutletComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-router-outlet',
                    template: "\n    <router-outlet></router-outlet>\n  "
                }] }
    ];
    return RouterOutletComponent;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var PermissionDirective = /** @class */ (function () {
    function PermissionDirective(elRef, renderer, store) {
        this.elRef = elRef;
        this.renderer = renderer;
        this.store = store;
    }
    /**
     * @return {?}
     */
    PermissionDirective.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.condition) {
            this.store
                .select(ConfigState.getGrantedPolicy(this.condition))
                .pipe(takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} isGranted
             * @return {?}
             */
            function (isGranted) {
                if (!isGranted) {
                    _this.renderer.removeChild(((/** @type {?} */ (_this.elRef.nativeElement))).parentElement, _this.elRef.nativeElement);
                }
            }));
        }
    };
    /**
     * @return {?}
     */
    PermissionDirective.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    PermissionDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[abpPermission]',
                },] }
    ];
    /** @nocollapse */
    PermissionDirective.ctorParameters = function () { return [
        { type: ElementRef, decorators: [{ type: Optional }] },
        { type: Renderer2 },
        { type: Store }
    ]; };
    PermissionDirective.propDecorators = {
        condition: [{ type: Input, args: ['abpPermission',] }]
    };
    return PermissionDirective;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var VisibilityDirective = /** @class */ (function () {
    function VisibilityDirective(elRef, renderer) {
        this.elRef = elRef;
        this.renderer = renderer;
        this.completed$ = new Subject();
    }
    /**
     * @return {?}
     */
    VisibilityDirective.prototype.ngAfterViewInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        /** @type {?} */
        var observer = new MutationObserver((/**
         * @param {?} mutations
         * @return {?}
         */
        function (mutations) {
            mutations.forEach((/**
             * @param {?} mutation
             * @return {?}
             */
            function (mutation) {
                if (!mutation.target)
                    return;
                /** @type {?} */
                var htmlNodes = snq((/**
                 * @return {?}
                 */
                function () { return Array.from(mutation.target.childNodes).filter((/**
                 * @param {?} node
                 * @return {?}
                 */
                function (node) { return node instanceof HTMLElement; })); }), []);
                if (!htmlNodes.length) {
                    _this.renderer.removeChild(_this.elRef.nativeElement.parentElement, _this.elRef.nativeElement);
                    _this.disconnect();
                }
                else {
                    setTimeout((/**
                     * @return {?}
                     */
                    function () {
                        _this.disconnect();
                    }), 0);
                }
            }));
        }));
        observer.observe(this.focusedElement, {
            childList: true,
        });
        this.completed$.subscribe((/**
         * @return {?}
         */
        function () { return observer.disconnect(); }));
    };
    /**
     * @return {?}
     */
    VisibilityDirective.prototype.disconnect = /**
     * @return {?}
     */
    function () {
        this.completed$.next();
        this.completed$.complete();
    };
    VisibilityDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[abpVisibility]',
                },] }
    ];
    /** @nocollapse */
    VisibilityDirective.ctorParameters = function () { return [
        { type: ElementRef, decorators: [{ type: Optional }] },
        { type: Renderer2 }
    ]; };
    VisibilityDirective.propDecorators = {
        focusedElement: [{ type: Input, args: ['abpVisibility',] }]
    };
    return VisibilityDirective;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var AuthGuard = /** @class */ (function () {
    function AuthGuard(oauthService, store) {
        this.oauthService = oauthService;
        this.store = store;
    }
    /**
     * @param {?} _
     * @param {?} state
     * @return {?}
     */
    AuthGuard.prototype.canActivate = /**
     * @param {?} _
     * @param {?} state
     * @return {?}
     */
    function (_, state) {
        /** @type {?} */
        var hasValidAccessToken = this.oauthService.hasValidAccessToken();
        if (hasValidAccessToken) {
            return hasValidAccessToken;
        }
        this.store.dispatch(new Navigate(['/account/login'], null, { state: { redirectUrl: state.url } }));
        return false;
    };
    AuthGuard.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    AuthGuard.ctorParameters = function () { return [
        { type: OAuthService },
        { type: Store }
    ]; };
    /** @nocollapse */ AuthGuard.ngInjectableDef = ɵɵdefineInjectable({ factory: function AuthGuard_Factory() { return new AuthGuard(ɵɵinject(OAuthService), ɵɵinject(Store)); }, token: AuthGuard, providedIn: "root" });
    return AuthGuard;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var PermissionGuard = /** @class */ (function () {
    function PermissionGuard(store) {
        this.store = store;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionGuard.prototype.canActivate = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var data = _a.data;
        /** @type {?} */
        var resource = (/** @type {?} */ (data.requiredPolicy));
        return this.store.select(ConfigState.getGrantedPolicy(resource));
    };
    PermissionGuard.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    PermissionGuard.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ PermissionGuard.ngInjectableDef = ɵɵdefineInjectable({ factory: function PermissionGuard_Factory() { return new PermissionGuard(ɵɵinject(Store)); }, token: PermissionGuard, providedIn: "root" });
    return PermissionGuard;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ApiInterceptor = /** @class */ (function () {
    function ApiInterceptor(oAuthService, store) {
        this.oAuthService = oAuthService;
        this.store = store;
    }
    /**
     * @param {?} request
     * @param {?} next
     * @return {?}
     */
    ApiInterceptor.prototype.intercept = /**
     * @param {?} request
     * @param {?} next
     * @return {?}
     */
    function (request, next) {
        var _this = this;
        this.store.dispatch(new LoaderStart(request));
        /** @type {?} */
        var headers = (/** @type {?} */ ({}));
        /** @type {?} */
        var token = this.oAuthService.getAccessToken();
        if (!request.headers.has('Authorization') && token) {
            headers['Authorization'] = "Bearer " + token;
        }
        /** @type {?} */
        var lang = this.store.selectSnapshot(SessionState.getLanguage);
        if (!request.headers.has('Accept-Language') && lang) {
            headers['Accept-Language'] = lang;
        }
        return next
            .handle(request.clone({
            setHeaders: headers,
        }))
            .pipe(finalize((/**
         * @return {?}
         */
        function () { return _this.store.dispatch(new LoaderStop(request)); })));
    };
    ApiInterceptor.decorators = [
        { type: Injectable }
    ];
    /** @nocollapse */
    ApiInterceptor.ctorParameters = function () { return [
        { type: OAuthService },
        { type: Store }
    ]; };
    return ApiInterceptor;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var Rest;
(function (Rest) {
    /**
     * @record
     */
    function Config() { }
    Rest.Config = Config;
    /**
     * @record
     * @template T
     */
    function Request() { }
    Rest.Request = Request;
})(Rest || (Rest = {}));

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
var NGXS_CONFIG_PLUGIN_OPTIONS = new InjectionToken('NGXS_CONFIG_PLUGIN_OPTIONS');
var ConfigPlugin = /** @class */ (function () {
    function ConfigPlugin(options, router) {
        this.options = options;
        this.router = router;
        this.initialized = false;
    }
    /**
     * @param {?} state
     * @param {?} event
     * @param {?} next
     * @return {?}
     */
    ConfigPlugin.prototype.handle = /**
     * @param {?} state
     * @param {?} event
     * @param {?} next
     * @return {?}
     */
    function (state, event, next) {
        /** @type {?} */
        var matches = actionMatcher(event);
        /** @type {?} */
        var isInitAction = matches(InitState) || matches(UpdateState);
        // const layouts = snq(() => this.options.requirements.layouts.filter(layout => layout instanceof Type), []);
        if (isInitAction && !this.initialized) {
            var _a = transformRoutes(this.router.config), routes = _a.routes, wrappers = _a.wrappers;
            routes = organizeRoutes(routes, wrappers);
            state = setValue(state, 'ConfigState', __assign({}, (state.ConfigState && __assign({}, state.ConfigState)), this.options, { routes: routes }));
            this.initialized = true;
        }
        return next(state, event);
    };
    ConfigPlugin.decorators = [
        { type: Injectable }
    ];
    /** @nocollapse */
    ConfigPlugin.ctorParameters = function () { return [
        { type: undefined, decorators: [{ type: Inject, args: [NGXS_CONFIG_PLUGIN_OPTIONS,] }] },
        { type: Router }
    ]; };
    return ConfigPlugin;
}());
/**
 * @param {?=} routes
 * @param {?=} wrappers
 * @return {?}
 */
function transformRoutes(routes, wrappers) {
    if (routes === void 0) { routes = []; }
    if (wrappers === void 0) { wrappers = []; }
    /** @type {?} */
    var abpRoutes = routes
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        return snq((/**
         * @return {?}
         */
        function () { return route.data.routes.find((/**
         * @param {?} r
         * @return {?}
         */
        function (r) { return r.path === route.path; })); }), false);
    }))
        .reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    function (acc, val) { return __spread(acc, val.data.routes); }), []);
    wrappers = abpRoutes.filter((/**
     * @param {?} ar
     * @return {?}
     */
    function (ar) { return ar.wrapper; }));
    /** @type {?} */
    var transformed = (/** @type {?} */ ([]));
    routes
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    function (route) { return route.component || route.loadChildren; }))
        .forEach((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        /** @type {?} */
        var abpPackage = abpRoutes.find((/**
         * @param {?} abp
         * @return {?}
         */
        function (abp) { return abp.path.toLowerCase() === route.path.toLowerCase(); }));
        var length = transformed.length;
        if (abpPackage) {
            transformed.push(abpPackage);
        }
        if (transformed.length === length) {
            transformed.push((/** @type {?} */ ({
                path: route.path,
                name: snq((/**
                 * @return {?}
                 */
                function () { return route.data.routes.name; }), route.path),
                children: route.data.routes.children || [],
            })));
        }
    }));
    return { routes: setUrls(transformed), wrappers: wrappers };
}
/**
 * @param {?} routes
 * @param {?=} parentUrl
 * @return {?}
 */
function setUrls(routes, parentUrl) {
    if (parentUrl) {
        // this if block using for only recursive call
        return routes.map((/**
         * @param {?} route
         * @return {?}
         */
        function (route) { return (__assign({}, route, { url: parentUrl + "/" + route.path }, (route.children &&
            route.children.length && {
            children: setUrls(route.children, parentUrl + "/" + route.path),
        }))); }));
    }
    return routes.map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) { return (__assign({}, route, { url: "/" + route.path }, (route.children &&
        route.children.length && {
        children: setUrls(route.children, "/" + route.path),
    }))); }));
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ConfigService = /** @class */ (function () {
    function ConfigService(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    ConfigService.prototype.getAll = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(ConfigState.getAll);
    };
    /**
     * @param {?} key
     * @return {?}
     */
    ConfigService.prototype.getOne = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        return this.store.selectSnapshot(ConfigState.getOne(key));
    };
    /**
     * @param {?} keys
     * @return {?}
     */
    ConfigService.prototype.getDeep = /**
     * @param {?} keys
     * @return {?}
     */
    function (keys) {
        return this.store.selectSnapshot(ConfigState.getDeep(keys));
    };
    /**
     * @param {?} key
     * @return {?}
     */
    ConfigService.prototype.getSetting = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        return this.store.selectSnapshot(ConfigState.getSetting(key));
    };
    ConfigService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    ConfigService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ ConfigService.ngInjectableDef = ɵɵdefineInjectable({ factory: function ConfigService_Factory() { return new ConfigService(ɵɵinject(Store)); }, token: ConfigService, providedIn: "root" });
    return ConfigService;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var LazyLoadService = /** @class */ (function () {
    function LazyLoadService() {
        this.loadedLibraries = {};
    }
    /**
     * @param {?} url
     * @param {?} type
     * @param {?=} content
     * @param {?=} targetQuery
     * @param {?=} position
     * @return {?}
     */
    LazyLoadService.prototype.load = /**
     * @param {?} url
     * @param {?} type
     * @param {?=} content
     * @param {?=} targetQuery
     * @param {?=} position
     * @return {?}
     */
    function (url, type, content, targetQuery, position) {
        var _this = this;
        if (content === void 0) { content = ''; }
        if (targetQuery === void 0) { targetQuery = 'body'; }
        if (position === void 0) { position = 'afterend'; }
        if (!url && !content)
            return;
        /** @type {?} */
        var key = url ? url.slice(url.lastIndexOf('/') + 1) : uuid();
        if (this.loadedLibraries[key]) {
            return this.loadedLibraries[key].asObservable();
        }
        this.loadedLibraries[key] = new ReplaySubject();
        /** @type {?} */
        var library;
        if (type === 'script') {
            library = document.createElement('script');
            library.type = 'text/javascript';
            if (url) {
                ((/** @type {?} */ (library))).src = url;
            }
            ((/** @type {?} */ (library))).text = content;
        }
        else if (url) {
            library = document.createElement('link');
            library.type = 'text/css';
            ((/** @type {?} */ (library))).rel = 'stylesheet';
            if (url) {
                ((/** @type {?} */ (library))).href = url;
            }
        }
        else {
            library = document.createElement('style');
            ((/** @type {?} */ (library))).textContent = content;
        }
        library.onload = (/**
         * @return {?}
         */
        function () {
            _this.loadedLibraries[key].next();
            _this.loadedLibraries[key].complete();
        });
        document.querySelector(targetQuery).insertAdjacentElement(position, library);
        return this.loadedLibraries[key].asObservable();
    };
    LazyLoadService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */ LazyLoadService.ngInjectableDef = ɵɵdefineInjectable({ factory: function LazyLoadService_Factory() { return new LazyLoadService(); }, token: LazyLoadService, providedIn: "root" });
    return LazyLoadService;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var LocalizationService = /** @class */ (function () {
    function LocalizationService(store) {
        this.store = store;
    }
    /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationService.prototype.get = /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (keys) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        return this.store.select(ConfigState.getCopy.apply(ConfigState, __spread([keys], interpolateParams)));
    };
    /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationService.prototype.instant = /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (keys) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getCopy.apply(ConfigState, __spread([keys], interpolateParams)));
    };
    LocalizationService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    LocalizationService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ LocalizationService.ngInjectableDef = ɵɵdefineInjectable({ factory: function LocalizationService_Factory() { return new LocalizationService(ɵɵinject(Store)); }, token: LocalizationService, providedIn: "root" });
    return LocalizationService;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @param {?} environment
 * @return {?}
 */
function environmentFactory(environment) {
    return __assign({}, environment);
}
/**
 * @param {?} config
 * @return {?}
 */
function configFactory(config) {
    return __assign({}, config);
}
/** @type {?} */
var ENVIRONMENT = new InjectionToken('ENVIRONMENT');
/** @type {?} */
var CONFIG = new InjectionToken('CONFIG');

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var LocalizationPipe = /** @class */ (function () {
    function LocalizationPipe(store) {
        this.store = store;
        this.initialized = false;
    }
    /**
     * @param {?} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationPipe.prototype.transform = /**
     * @param {?} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (value) {
        var _this = this;
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        if (!this.initialized) {
            this.initialized = true;
            this.store
                .select(ConfigState.getCopy.apply(ConfigState, __spread([value], interpolateParams.reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) { return (Array.isArray(val) ? __spread(acc, val) : __spread(acc, [val])); }), []))))
                .pipe(takeUntilDestroy(this), distinctUntilChanged())
                .subscribe((/**
             * @param {?} copy
             * @return {?}
             */
            function (copy) { return (_this.value = copy); }));
        }
        return this.value;
    };
    /**
     * @return {?}
     */
    LocalizationPipe.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    LocalizationPipe.decorators = [
        { type: Pipe, args: [{
                    name: 'abpLocalization',
                    pure: false,
                },] }
    ];
    /** @nocollapse */
    LocalizationPipe.ctorParameters = function () { return [
        { type: Store }
    ]; };
    return LocalizationPipe;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var CoreModule = /** @class */ (function () {
    function CoreModule() {
    }
    /**
     * @param {?=} options
     * @return {?}
     */
    CoreModule.forRoot = /**
     * @param {?=} options
     * @return {?}
     */
    function (options) {
        if (options === void 0) { options = (/** @type {?} */ ({})); }
        return {
            ngModule: CoreModule,
            providers: [
                {
                    provide: NGXS_PLUGINS,
                    useClass: ConfigPlugin,
                    multi: true,
                },
                {
                    provide: NGXS_CONFIG_PLUGIN_OPTIONS,
                    useValue: options,
                },
                {
                    provide: HTTP_INTERCEPTORS,
                    useClass: ApiInterceptor,
                    multi: true,
                },
                {
                    provide: APP_INITIALIZER,
                    multi: true,
                    deps: [Injector],
                    useFactory: getInitialData,
                },
            ],
        };
    };
    CoreModule.decorators = [
        { type: NgModule, args: [{
                    imports: [
                        NgxsModule.forFeature([ProfileState, SessionState, ConfigState]),
                        NgxsStoragePluginModule.forRoot({ key: 'SessionState' }),
                        NgxsRouterPluginModule.forRoot(),
                        CommonModule,
                        HttpClientModule,
                        FormsModule,
                        ReactiveFormsModule,
                        RouterModule,
                    ],
                    declarations: [
                        RouterOutletComponent,
                        DynamicLayoutComponent,
                        PermissionDirective,
                        VisibilityDirective,
                        LocalizationPipe,
                    ],
                    exports: [
                        CommonModule,
                        HttpClientModule,
                        FormsModule,
                        ReactiveFormsModule,
                        RouterModule,
                        RouterOutletComponent,
                        DynamicLayoutComponent,
                        PermissionDirective,
                        VisibilityDirective,
                        LocalizationPipe,
                    ],
                    providers: [LocalizationPipe],
                    entryComponents: [RouterOutletComponent, DynamicLayoutComponent],
                },] }
    ];
    return CoreModule;
}());

export { ApiInterceptor, ApplicationConfigurationService, AuthGuard, CONFIG, ConfigGetAppConfiguration, ConfigPlugin, ConfigService, ConfigState, CoreModule, DynamicLayoutComponent, ENVIRONMENT, LazyLoadService, LoaderStart, LoaderStop, LocalizationService, NGXS_CONFIG_PLUGIN_OPTIONS, PatchRouteByName, PermissionDirective, PermissionGuard, ProfileChangePassword, ProfileGet, ProfileService, ProfileState, ProfileUpdate, Rest, RestOccurError, RestService, RouterOutletComponent, SessionSetLanguage, SessionState, VisibilityDirective, configFactory, environmentFactory, getInitialData, organizeRoutes, setChildRoute, sortRoutes, takeUntilDestroy, uuid, ProfileState as ɵa, ProfileService as ɵb, RestService as ɵc, ProfileGet as ɵd, ProfileUpdate as ɵe, ProfileChangePassword as ɵf, SessionState as ɵh, SessionSetLanguage as ɵi, ConfigState as ɵj, ApplicationConfigurationService as ɵk, PatchRouteByName as ɵl, ConfigGetAppConfiguration as ɵm, RouterOutletComponent as ɵn, DynamicLayoutComponent as ɵo, ConfigState as ɵp, PermissionDirective as ɵq, VisibilityDirective as ɵr, LocalizationPipe as ɵs, NGXS_CONFIG_PLUGIN_OPTIONS as ɵt, ConfigPlugin as ɵu, ApiInterceptor as ɵw, getInitialData as ɵx };
//# sourceMappingURL=abp-ng.core.js.map
