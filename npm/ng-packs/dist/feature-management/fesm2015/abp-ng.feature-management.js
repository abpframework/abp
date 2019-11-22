import { RestService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  Injectable,
  ɵɵdefineInjectable,
  ɵɵinject,
  EventEmitter,
  Component,
  Input,
  Output,
  NgModule,
} from '@angular/core';
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
class GetFeatures {
  /**
   * @param {?} payload
   */
  constructor(payload) {
    this.payload = payload;
  }
}
GetFeatures.type = '[FeatureManagement] Get Features';
if (false) {
  /** @type {?} */
  GetFeatures.type;
  /** @type {?} */
  GetFeatures.prototype.payload;
}
class UpdateFeatures {
  /**
   * @param {?} payload
   */
  constructor(payload) {
    this.payload = payload;
  }
}
UpdateFeatures.type = '[FeatureManagement] Update Features';
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
class FeatureManagementService {
  /**
   * @param {?} rest
   * @param {?} store
   */
  constructor(rest, store) {
    this.rest = rest;
    this.store = store;
  }
  /**
   * @param {?} params
   * @return {?}
   */
  getFeatures(params) {
    /** @type {?} */
    const request = {
      method: 'GET',
      url: '/api/abp/features',
      params,
    };
    return this.rest.request(request);
  }
  /**
   * @param {?} __0
   * @return {?}
   */
  updateFeatures({ features, providerKey, providerName }) {
    /** @type {?} */
    const request = {
      method: 'PUT',
      url: '/api/abp/features',
      body: { features },
      params: { providerKey, providerName },
    };
    return this.rest.request(request);
  }
}
FeatureManagementService.decorators = [
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
FeatureManagementService.ctorParameters = () => [{ type: RestService }, { type: Store }];
/** @nocollapse */ FeatureManagementService.ngInjectableDef = ɵɵdefineInjectable({
  factory: function FeatureManagementService_Factory() {
    return new FeatureManagementService(ɵɵinject(RestService), ɵɵinject(Store));
  },
  token: FeatureManagementService,
  providedIn: 'root',
});
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
let FeatureManagementState = class FeatureManagementState {
    /**
     * @param {?} featureManagementService
     */
    constructor(featureManagementService) {
        this.featureManagementService = featureManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getFeatures({ features }) {
        return features || [];
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getFeatures({ patchState }, { payload }) {
        return this.featureManagementService.getFeatures(payload).pipe(tap((/**
    __metadata('design:returntype', void 0),
  ],
  FeatureManagementState.prototype,
  'getFeatures',
  null,
);
__decorate(
  [
    Action(UpdateFeatures),
    __metadata('design:type', Function),
    __metadata('design:paramtypes', [Object, UpdateFeatures]),
    __metadata('design:returntype', void 0),
  ],
  FeatureManagementState.prototype,
  'updateFeatures',
  null,
);
__decorate(
  [
    Selector(),
    __metadata('design:type', Function),
    __metadata('design:paramtypes', [Object]),
    __metadata('design:returntype', void 0),
  ],
  FeatureManagementState,
  'getFeatures',
  null,
);
FeatureManagementState = __decorate(
  [
    State({
      name: 'FeatureManagementState',
      defaults: /** @type {?} */ ({ features: {} }),
    }),
    __metadata('design:paramtypes', [FeatureManagementService]),
  ],
  FeatureManagementState,
);
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
class FeatureManagementComponent {
  /**
   * @param {?} store
   */
  constructor(store) {
    this.store = store;
    this.visibleChange = new EventEmitter();
    this.modalBusy = false;
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
    if (value) this.openModal();
  }
  /**
   * @return {?}
   */
  openModal() {
    if (!this.providerKey || !this.providerName) {
      throw new Error('Provider Key and Provider Name are required.');
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set visible(value) {
        this._visible = value;
        this.visibleChange.emit(value);
        if (value)
            this.openModal();
    }
    /**
     * @return {?}
     */
    openModal() {
        if (!this.providerKey || !this.providerName) {
            throw new Error('Provider Key and Provider Name are required.');
        }
        this.getFeatures();
    }
    /**
     * @return {?}
     */
    getFeatures() {
        this.store
            .dispatch(new GetFeatures({
    /**
     * @param {?} value
     * @return {?}
     */
    set visible(value) {
        this._visible = value;
        this.visibleChange.emit(value);
        if (value)
            this.openModal();
    }
    /**
     * @return {?}
     */
    openModal() {
        if (!this.providerKey || !this.providerName) {
            throw new Error('Provider Key and Provider Name are required.');
        }
        this.getFeatures();
    }
    /**
     * @return {?}
     */
    getFeatures() {
        this.store
            .dispatch(new GetFeatures({
            providerKey: this.providerKey,
            providerName: this.providerName,
        }))
            .pipe(pluck('FeatureManagementState', 'features'))
            .subscribe((/**
         */
        () => (this.modalBusy = false))))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.visible = false;
        }));
    }
}
FeatureManagementComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-feature-management',
                template: "<abp-modal size=\"md\" [(visible)]=\"visible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h3>{{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}</h3>\r\n  </ng-template>\r\n\r\n  <ng-template #abpBody>\r\n    <form *ngIf=\"form\" (ngSubmit)=\"save()\" [formGroup]=\"form\" validateOnSubmit>\r\n      <div\r\n        class=\"row my-3\"\r\n        *ngFor=\"let feature of features$ | async; let i = index\"\r\n        [ngSwitch]=\"feature.valueType.name\"\r\n      >\r\n        <div class=\"col-4\">{{ feature.name }}</div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'ToggleStringValueType'\">\r\n          <input type=\"checkbox\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'FreeTextStringValueType'\">\r\n          <input type=\"text\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpFeatureManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" [disabled]=\"form?.invalid || modalBusy\" (click)=\"save()\">\r\n      {{ 'AbpFeatureManagement::Save' | abpLocalization }}\r\n    </abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
            }] }
];
/** @nocollapse */
FeatureManagementComponent.ctorParameters = () => [{ type: Store }];
    /**
     * @return {?}
     */
    save() {
        if (this.modalBusy)
            return;
        this.modalBusy = true;
        /** @type {?} */
        let features = this.store.selectSnapshot(FeatureManagementState.getFeatures);
        features = features.map((/**
         * @param {?} feature
         * @param {?} i
         * @return {?}
         */
        (feature, i) => ({
            name: feature.name,
            value: !this.form.value[i] || this.form.value[i] === 'false' ? null : this.form.value[i],
        })));
        this.store
            .dispatch(new UpdateFeatures({
            providerKey: this.providerKey,
            providerName: this.providerName,
            features,
        }))
            .pipe(finalize((/**
         * @return {?}
         */
        () => (this.modalBusy = false))))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.visible = false;
        }));
    }
}
FeatureManagementComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-feature-management',
                template: "<abp-modal size=\"md\" [(visible)]=\"visible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h3>{{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}</h3>\r\n  </ng-template>\r\n\r\n  <ng-template #abpBody>\r\n    <form *ngIf=\"form\" (ngSubmit)=\"save()\" [formGroup]=\"form\" validateOnSubmit>\r\n      <div\r\n        class=\"row my-3\"\r\n        *ngFor=\"let feature of features$ | async; let i = index\"\r\n        [ngSwitch]=\"feature.valueType.name\"\r\n      >\r\n        <div class=\"col-4\">{{ feature.name }}</div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'ToggleStringValueType'\">\r\n          <input type=\"checkbox\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'FreeTextStringValueType'\">\r\n          <input type=\"text\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpFeatureManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" [disabled]=\"form?.invalid || modalBusy\" (click)=\"save()\">\r\n      {{ 'AbpFeatureManagement::Save' | abpLocalization }}\r\n    </abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
            }] }
