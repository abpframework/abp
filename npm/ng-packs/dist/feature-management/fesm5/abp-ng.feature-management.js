import { RestService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, EventEmitter, Component, Input, Output, NgModule } from '@angular/core';
import { __decorate, __metadata } from 'tslib';
import { Store, Action, Selector, State, Select, NgxsModule } from '@ngxs/store';
import { Observable } from 'rxjs';
import { tap, pluck, finalize } from 'rxjs/operators';
import { FormControl, FormGroup } from '@angular/forms';

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
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    FeatureManagementService.ctorParameters = function () { return [
        { type: RestService },
        { type: Store }
    ]; };
    /** @nocollapse */ FeatureManagementService.ngInjectableDef = ɵɵdefineInjectable({ factory: function FeatureManagementService_Factory() { return new FeatureManagementService(ɵɵinject(RestService), ɵɵinject(Store)); }, token: FeatureManagementService, providedIn: "root" });
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
        return this.featureManagementService.getFeatures(payload).pipe(tap((/**
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
        Action(GetFeatures),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, GetFeatures]),
        __metadata("design:returntype", void 0)
    ], FeatureManagementState.prototype, "getFeatures", null);
    __decorate([
        Action(UpdateFeatures),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, UpdateFeatures]),
        __metadata("design:returntype", void 0)
    ], FeatureManagementState.prototype, "updateFeatures", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", void 0)
    ], FeatureManagementState, "getFeatures", null);
    FeatureManagementState = __decorate([
        State({
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
        this.visibleChange = new EventEmitter();
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
            .pipe(pluck('FeatureManagementState', 'features'))
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
            formGroupObj[i] = new FormControl(features[i].value === 'false' ? null : features[i].value);
        }
        this.form = new FormGroup(formGroupObj);
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
            .pipe(finalize((/**
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
        { type: Component, args: [{
                    selector: 'abp-feature-management',
                    template: "<abp-modal size=\"md\" [(visible)]=\"visible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h3>{{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}</h3>\r\n  </ng-template>\r\n\r\n  <ng-template #abpBody>\r\n    <form *ngIf=\"form\" (ngSubmit)=\"save()\" [formGroup]=\"form\" validateOnSubmit>\r\n      <div\r\n        class=\"row my-3\"\r\n        *ngFor=\"let feature of features$ | async; let i = index\"\r\n        [ngSwitch]=\"feature.valueType.name\"\r\n      >\r\n        <div class=\"col-4\">{{ feature.name }}</div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'ToggleStringValueType'\">\r\n          <input type=\"checkbox\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'FreeTextStringValueType'\">\r\n          <input type=\"text\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpFeatureManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" [disabled]=\"form?.invalid || modalBusy\" (click)=\"save()\">\r\n      {{ 'AbpFeatureManagement::Save' | abpLocalization }}\r\n    </abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
                }] }
    ];
    /** @nocollapse */
    FeatureManagementComponent.ctorParameters = function () { return [
        { type: Store }
    ]; };
    FeatureManagementComponent.propDecorators = {
        providerKey: [{ type: Input }],
        providerName: [{ type: Input }],
        visible: [{ type: Input }],
        visibleChange: [{ type: Output }]
    };
    __decorate([
        Select(FeatureManagementState.getFeatures),
        __metadata("design:type", Observable)
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
        { type: NgModule, args: [{
                    declarations: [FeatureManagementComponent],
                    imports: [CoreModule, ThemeSharedModule, NgxsModule.forFeature([FeatureManagementState])],
                    exports: [FeatureManagementComponent],
                },] }
    ];
    return FeatureManagementModule;
}());

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: public-api.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: abp-ng.feature-management.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { FeatureManagementComponent, FeatureManagementModule, FeatureManagementComponent as ɵa, FeatureManagementState as ɵb, FeatureManagementState as ɵc, FeatureManagementService as ɵd, GetFeatures as ɵe, UpdateFeatures as ɵf };
//# sourceMappingURL=abp-ng.feature-management.js.map
