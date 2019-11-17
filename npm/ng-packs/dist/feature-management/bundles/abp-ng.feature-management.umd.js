(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@abp/ng.theme.shared'), require('@angular/core'), require('@ngxs/store'), require('rxjs'), require('rxjs/operators'), require('@angular/forms')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.feature-management', ['exports', '@abp/ng.core', '@abp/ng.theme.shared', '@angular/core', '@ngxs/store', 'rxjs', 'rxjs/operators', '@angular/forms'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng['feature-management'] = {}), global.ng_core, global.ng_theme_shared, global.ng.core, global.store, global.rxjs, global.rxjs.operators, global.ng.forms));
}(this, (function (exports, ng_core, ng_theme_shared, core, store, rxjs, operators, forms) { 'use strict';

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
    /* global Reflect, Promise */

    var extendStatics = function(d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };

    function __extends(d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    }

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

    function __rest(s, e) {
        var t = {};
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
            t[p] = s[p];
        if (s != null && typeof Object.getOwnPropertySymbols === "function")
            for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
                if (e.indexOf(p[i]) < 0 && Object.prototype.propertyIsEnumerable.call(s, p[i]))
                    t[p[i]] = s[p[i]];
            }
        return t;
    }

    function __decorate(decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    }

    function __param(paramIndex, decorator) {
        return function (target, key) { decorator(target, key, paramIndex); }
    }

    function __metadata(metadataKey, metadataValue) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
    }

    function __awaiter(thisArg, _arguments, P, generator) {
        return new (P || (P = Promise))(function (resolve, reject) {
            function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
            function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
            function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
            step((generator = generator.apply(thisArg, _arguments || [])).next());
        });
    }

    function __generator(thisArg, body) {
        var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
        return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
        function verb(n) { return function (v) { return step([n, v]); }; }
        function step(op) {
            if (f) throw new TypeError("Generator is already executing.");
            while (_) try {
                if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
                if (y = 0, t) op = [op[0] & 2, t.value];
                switch (op[0]) {
                    case 0: case 1: t = op; break;
                    case 4: _.label++; return { value: op[1], done: false };
                    case 5: _.label++; y = op[1]; op = [0]; continue;
                    case 7: op = _.ops.pop(); _.trys.pop(); continue;
                    default:
                        if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                        if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                        if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                        if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                        if (t[2]) _.ops.pop();
                        _.trys.pop(); continue;
                }
                op = body.call(thisArg, _);
            } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
            if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
        }
    }

    function __exportStar(m, exports) {
        for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
    }

    function __values(o) {
        var m = typeof Symbol === "function" && o[Symbol.iterator], i = 0;
        if (m) return m.call(o);
        return {
            next: function () {
                if (o && i >= o.length) o = void 0;
                return { value: o && o[i++], done: !o };
            }
        };
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

    function __spreadArrays() {
        for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
        for (var r = Array(s), k = 0, i = 0; i < il; i++)
            for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
                r[k] = a[j];
        return r;
    };

    function __await(v) {
        return this instanceof __await ? (this.v = v, this) : new __await(v);
    }

    function __asyncGenerator(thisArg, _arguments, generator) {
        if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
        var g = generator.apply(thisArg, _arguments || []), i, q = [];
        return i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i;
        function verb(n) { if (g[n]) i[n] = function (v) { return new Promise(function (a, b) { q.push([n, v, a, b]) > 1 || resume(n, v); }); }; }
        function resume(n, v) { try { step(g[n](v)); } catch (e) { settle(q[0][3], e); } }
        function step(r) { r.value instanceof __await ? Promise.resolve(r.value.v).then(fulfill, reject) : settle(q[0][2], r); }
        function fulfill(value) { resume("next", value); }
        function reject(value) { resume("throw", value); }
        function settle(f, v) { if (f(v), q.shift(), q.length) resume(q[0][0], q[0][1]); }
    }

    function __asyncDelegator(o) {
        var i, p;
        return i = {}, verb("next"), verb("throw", function (e) { throw e; }), verb("return"), i[Symbol.iterator] = function () { return this; }, i;
        function verb(n, f) { i[n] = o[n] ? function (v) { return (p = !p) ? { value: __await(o[n](v)), done: n === "return" } : f ? f(v) : v; } : f; }
    }

    function __asyncValues(o) {
        if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
        var m = o[Symbol.asyncIterator], i;
        return m ? m.call(o) : (o = typeof __values === "function" ? __values(o) : o[Symbol.iterator](), i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i);
        function verb(n) { i[n] = o[n] && function (v) { return new Promise(function (resolve, reject) { v = o[n](v), settle(resolve, reject, v.done, v.value); }); }; }
        function settle(resolve, reject, d, v) { Promise.resolve(v).then(function(v) { resolve({ value: v, done: d }); }, reject); }
    }

    function __makeTemplateObject(cooked, raw) {
        if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
        return cooked;
    };

    function __importStar(mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
        result.default = mod;
        return result;
    }

    function __importDefault(mod) {
        return (mod && mod.__esModule) ? mod : { default: mod };
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/feature-management.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var GetFeatures = /** @class */ (function () {
        function GetFeatures(payload) {
            this.payload = payload;
        }
        GetFeatures.type = '[FeatureManagement] Get Features';
        return GetFeatures;
    }());
    if (false) {
        /** @type {?} */
        GetFeatures.type;
        /** @type {?} */
        GetFeatures.prototype.payload;
    }
    var UpdateFeatures = /** @class */ (function () {
        function UpdateFeatures(payload) {
            this.payload = payload;
        }
        UpdateFeatures.type = '[FeatureManagement] Update Features';
        return UpdateFeatures;
    }());
    if (false) {
        /** @type {?} */
        UpdateFeatures.type;
        /** @type {?} */
        UpdateFeatures.prototype.payload;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/feature-management.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var FeatureManagementService = /** @class */ (function () {
        function FeatureManagementService(rest, store) {
            this.rest = rest;
            this.store = store;
        }
        /**
         * @param {?} params
         * @return {?}
         */
        FeatureManagementService.prototype.getFeatures = /**
         * @param {?} params
         * @return {?}
         */
        function (params) {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/abp/features',
                params: params,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        FeatureManagementService.prototype.updateFeatures = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var features = _a.features, providerKey = _a.providerKey, providerName = _a.providerName;
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: '/api/abp/features',
                body: { features: features },
                params: { providerKey: providerKey, providerName: providerName },
            };
            return this.rest.request(request);
        };
        FeatureManagementService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        FeatureManagementService.ctorParameters = function () { return [
            { type: ng_core.RestService },
            { type: store.Store }
        ]; };
        /** @nocollapse */ FeatureManagementService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function FeatureManagementService_Factory() { return new FeatureManagementService(core.ɵɵinject(ng_core.RestService), core.ɵɵinject(store.Store)); }, token: FeatureManagementService, providedIn: "root" });
        return FeatureManagementService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        FeatureManagementService.prototype.rest;
        /**
         * @type {?}
         * @private
         */
        FeatureManagementService.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/feature-management.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var FeatureManagementState = /** @class */ (function () {
        function FeatureManagementState(featureManagementService) {
            this.featureManagementService = featureManagementService;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        FeatureManagementState.getFeatures = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var features = _a.features;
            return features || [];
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        FeatureManagementState.prototype.getFeatures = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.featureManagementService.getFeatures(payload).pipe(operators.tap((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var features = _a.features;
                return patchState({
                    features: features,
                });
            })));
        };
        /**
         * @param {?} _
         * @param {?} __1
         * @return {?}
         */
        FeatureManagementState.prototype.updateFeatures = /**
         * @param {?} _
         * @param {?} __1
         * @return {?}
         */
        function (_, _a) {
            var payload = _a.payload;
            return this.featureManagementService.updateFeatures(payload);
        };
        FeatureManagementState.ctorParameters = function () { return [
            { type: FeatureManagementService }
        ]; };
        __decorate([
            store.Action(GetFeatures),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetFeatures]),
            __metadata("design:returntype", void 0)
        ], FeatureManagementState.prototype, "getFeatures", null);
        __decorate([
            store.Action(UpdateFeatures),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, UpdateFeatures]),
            __metadata("design:returntype", void 0)
        ], FeatureManagementState.prototype, "updateFeatures", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", void 0)
        ], FeatureManagementState, "getFeatures", null);
        FeatureManagementState = __decorate([
            store.State({
                name: 'FeatureManagementState',
                defaults: (/** @type {?} */ ({ features: {} })),
            }),
            __metadata("design:paramtypes", [FeatureManagementService])
        ], FeatureManagementState);
        return FeatureManagementState;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        FeatureManagementState.prototype.featureManagementService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/feature-management/feature-management.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var FeatureManagementComponent = /** @class */ (function () {
        function FeatureManagementComponent(store) {
            this.store = store;
            this.visibleChange = new core.EventEmitter();
            this.modalBusy = false;
        }
        Object.defineProperty(FeatureManagementComponent.prototype, "visible", {
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
                if (value)
                    this.openModal();
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        FeatureManagementComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            if (!this.providerKey || !this.providerName) {
                throw new Error('Provider Key and Provider Name are required.');
            }
            this.getFeatures();
        };
        /**
         * @return {?}
         */
        FeatureManagementComponent.prototype.getFeatures = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.store
                .dispatch(new GetFeatures({
                providerKey: this.providerKey,
                providerName: this.providerName,
            }))
                .pipe(operators.pluck('FeatureManagementState', 'features'))
                .subscribe((/**
             * @param {?} features
             * @return {?}
             */
            function (features) {
                _this.buildForm(features);
            }));
        };
        /**
         * @param {?} features
         * @return {?}
         */
        FeatureManagementComponent.prototype.buildForm = /**
         * @param {?} features
         * @return {?}
         */
        function (features) {
            /** @type {?} */
            var formGroupObj = {};
            for (var i = 0; i < features.length; i++) {
                formGroupObj[i] = new forms.FormControl(features[i].value === 'false' ? null : features[i].value);
            }
            this.form = new forms.FormGroup(formGroupObj);
        };
        /**
         * @return {?}
         */
        FeatureManagementComponent.prototype.save = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.modalBusy)
                return;
            this.modalBusy = true;
            /** @type {?} */
            var features = this.store.selectSnapshot(FeatureManagementState.getFeatures);
            features = features.map((/**
             * @param {?} feature
             * @param {?} i
             * @return {?}
             */
            function (feature, i) { return ({
                name: feature.name,
                value: !_this.form.value[i] || _this.form.value[i] === 'false' ? null : _this.form.value[i],
            }); }));
            this.store
                .dispatch(new UpdateFeatures({
                providerKey: this.providerKey,
                providerName: this.providerName,
                features: features,
            }))
                .pipe(operators.finalize((/**
             * @return {?}
             */
            function () { return (_this.modalBusy = false); })))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.visible = false;
            }));
        };
        FeatureManagementComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-feature-management',
                        template: "<abp-modal size=\"md\" [(visible)]=\"visible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h3>{{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}</h3>\r\n  </ng-template>\r\n\r\n  <ng-template #abpBody>\r\n    <form *ngIf=\"form\" (ngSubmit)=\"save()\" [formGroup]=\"form\" validateOnSubmit>\r\n      <div\r\n        class=\"row my-3\"\r\n        *ngFor=\"let feature of features$ | async; let i = index\"\r\n        [ngSwitch]=\"feature.valueType.name\"\r\n      >\r\n        <div class=\"col-4\">{{ feature.name }}</div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'ToggleStringValueType'\">\r\n          <input type=\"checkbox\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'FreeTextStringValueType'\">\r\n          <input type=\"text\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpFeatureManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" [disabled]=\"form?.invalid || modalBusy\" (click)=\"save()\">\r\n      {{ 'AbpFeatureManagement::Save' | abpLocalization }}\r\n    </abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
                    }] }
        ];
        /** @nocollapse */
        FeatureManagementComponent.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        FeatureManagementComponent.propDecorators = {
            providerKey: [{ type: core.Input }],
            providerName: [{ type: core.Input }],
            visible: [{ type: core.Input }],
            visibleChange: [{ type: core.Output }]
        };
        __decorate([
            store.Select(FeatureManagementState.getFeatures),
            __metadata("design:type", rxjs.Observable)
        ], FeatureManagementComponent.prototype, "features$", void 0);
        return FeatureManagementComponent;
    }());
    if (false) {
        /** @type {?} */
        FeatureManagementComponent.prototype.providerKey;
        /** @type {?} */
        FeatureManagementComponent.prototype.providerName;
        /**
         * @type {?}
         * @protected
         */
        FeatureManagementComponent.prototype._visible;
        /** @type {?} */
        FeatureManagementComponent.prototype.visibleChange;
        /** @type {?} */
        FeatureManagementComponent.prototype.features$;
        /** @type {?} */
        FeatureManagementComponent.prototype.modalBusy;
        /** @type {?} */
        FeatureManagementComponent.prototype.form;
        /**
         * @type {?}
         * @private
         */
        FeatureManagementComponent.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/feature-management.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var FeatureManagementModule = /** @class */ (function () {
        function FeatureManagementModule() {
        }
        FeatureManagementModule.decorators = [
            { type: core.NgModule, args: [{
                        declarations: [FeatureManagementComponent],
                        imports: [ng_core.CoreModule, ng_theme_shared.ThemeSharedModule, store.NgxsModule.forFeature([FeatureManagementState])],
                        exports: [FeatureManagementComponent],
                    },] }
        ];
        return FeatureManagementModule;
    }());

    exports.FeatureManagementComponent = FeatureManagementComponent;
    exports.FeatureManagementModule = FeatureManagementModule;
    exports.ɵa = FeatureManagementComponent;
    exports.ɵb = FeatureManagementState;
    exports.ɵc = FeatureManagementState;
    exports.ɵd = FeatureManagementService;
    exports.ɵe = GetFeatures;
    exports.ɵf = UpdateFeatures;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=abp-ng.feature-management.umd.js.map
