/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/feature-management/feature-management.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { GetFeatures, UpdateFeatures } from '../../actions';
import { FeatureManagementState } from '../../states';
import { FormGroup, FormControl } from '@angular/forms';
import { pluck, finalize } from 'rxjs/operators';
export class FeatureManagementComponent {
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
         * @param {?} features
         * @return {?}
         */
        features => {
            this.buildForm(features);
        }));
    }
    /**
     * @param {?} features
     * @return {?}
     */
    buildForm(features) {
        /** @type {?} */
        const formGroupObj = {};
        for (let i = 0; i < features.length; i++) {
            formGroupObj[i] = new FormControl(features[i].value === 'false' ? null : features[i].value);
        }
        this.form = new FormGroup(formGroupObj);
    }
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
];
/** @nocollapse */
FeatureManagementComponent.ctorParameters = () => [
    { type: Store }
];
FeatureManagementComponent.propDecorators = {
    providerKey: [{ type: Input }],
    providerName: [{ type: Input }],
    visible: [{ type: Input }],
    visibleChange: [{ type: Output }]
};
tslib_1.__decorate([
    Select(FeatureManagementState.getFeatures),
    tslib_1.__metadata("design:type", Observable)
], FeatureManagementComponent.prototype, "features$", void 0);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZmVhdHVyZS1tYW5hZ2VtZW50L2ZlYXR1cmUtbWFuYWdlbWVudC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFhLE1BQU0sRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFDakcsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEVBQUUsV0FBVyxFQUFFLGNBQWMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUU1RCxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBRSxXQUFXLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN4RCxPQUFPLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBTWpELE1BQU0sT0FBTywwQkFBMEI7Ozs7SUE4QnJDLFlBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBVGIsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBSy9ELGNBQVMsR0FBRyxLQUFLLENBQUM7SUFJaUIsQ0FBQzs7OztJQXJCcEMsSUFDSSxPQUFPO1FBQ1QsT0FBTyxJQUFJLENBQUMsUUFBUSxDQUFDO0lBQ3ZCLENBQUM7Ozs7O0lBRUQsSUFBSSxPQUFPLENBQUMsS0FBYztRQUN4QixJQUFJLENBQUMsUUFBUSxHQUFHLEtBQUssQ0FBQztRQUN0QixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUUvQixJ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QUFNLENBQUMsc0JBQXNCLENBQUMsV0FBVyxDQUFDO3NDQUNoQyxVQUFVOzZEQUE4Qjs7O0lBdkJuRCxpREFDb0I7O0lBRXBCLGtEQUNxQjs7Ozs7SUFFckIsOENBQW1COztJQWNuQixtREFBK0Q7O0lBRS9ELCtDQUNtRDs7SUFFbkQsK0NBQWtCOztJQUVsQiwwQ0FBZ0I7Ozs7O0lBRUosMkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPbkNoYW5nZXMsIE91dHB1dCwgU2ltcGxlQ2hhbmdlcyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IEdldEZlYXR1cmVzLCBVcGRhdGVGZWF0dXJlcyB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xyXG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudCB9IGZyb20gJy4uLy4uL21vZGVscy9mZWF0dXJlLW1hbmFnZW1lbnQnO1xyXG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzJztcclxuaW1wb3J0IHsgRm9ybUdyb3VwLCBGb3JtQ29udHJvbCB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcclxuaW1wb3J0IHsgcGx1Y2ssIGZpbmFsaXplIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtZmVhdHVyZS1tYW5hZ2VtZW50JyxcclxuICB0ZW1wbGF0ZVVybDogJy4vZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIEZlYXR1cmVNYW5hZ2VtZW50Q29tcG9uZW50IHtcclxuICBASW5wdXQoKVxyXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgcHJvdmlkZXJOYW1lOiBzdHJpbmc7XHJcblxyXG4gIHByb3RlY3RlZCBfdmlzaWJsZTtcclxuXHJcbiAgQElucHV0KClcclxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcclxuICAgIHJldHVybiB0aGlzLl92aXNpYmxlO1xyXG4gIH1cclxuXHJcbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcclxuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcclxuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuXHJcbiAgICBpZiAodmFsdWUpIHRoaXMub3Blbk1vZGFsKCk7XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcclxuXHJcbiAgQFNlbGVjdChGZWF0dXJlTWFuYWdlbWVudFN0YXRlLmdldEZlYXR1cmVzKVxyXG4gIGZlYXR1cmVzJDogT2JzZXJ2YWJsZTxGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlW10+O1xyXG5cclxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcclxuXHJcbiAgZm9ybTogRm9ybUdyb3VwO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgb3Blbk1vZGFsKCkge1xyXG4gICAgaWYgKCF0aGlzLnByb3ZpZGVyS2V5IHx8ICF0aGlzLnByb3ZpZGVyTmFtZSkge1xyXG4gICAgICB0aHJvdyBuZXcgRXJyb3IoJ1Byb3ZpZGVyIEtleSBhbmQgUHJvdmlkZXIgTmFtZSBhcmUgcmVxdWlyZWQuJyk7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5nZXRGZWF0dXJlcygpO1xyXG4gIH1cclxuXHJcbiAgZ2V0RmVhdHVyZXMoKSB7XHJcbiAgICB0aGlzLnN0b3JlXHJcbiAgICAgIC5kaXNwYXRjaChcclxuICAgICAgICBuZXcgR2V0RmVhdHVyZXMoe1xyXG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXHJcbiAgICAgICAgICBwcm92aWRlck5hbWU6IHRoaXMucHJvdmlkZXJOYW1lLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApXHJcbiAgICAgIC5waXBlKHBsdWNrKCdGZWF0dXJlTWFuYWdlbWVudFN0YXRlJywgJ2ZlYXR1cmVzJykpXHJcbiAgICAgIC5zdWJzY3JpYmUoZmVhdHVyZXMgPT4ge1xyXG4gICAgICAgIHRoaXMuYnVpbGRGb3JtKGZlYXR1cmVzKTtcclxuICAgICAgfSk7XHJcbiAgfVxyXG5cclxuICBidWlsZEZvcm0oZmVhdHVyZXMpIHtcclxuICAgIGNvbnN0IGZvcm1Hcm91cE9iaiA9IHt9O1xyXG5cclxuICAgIGZvciAobGV0IGkgPSAwOyBpIDwgZmVhdHVyZXMubGVuZ3RoOyBpKyspIHtcclxuICAgICAgZm9ybUdyb3VwT2JqW2ldID0gbmV3IEZvcm1Db250cm9sKGZlYXR1cmVzW2ldLnZhbHVlID09PSAnZmFsc2UnID8gbnVsbCA6IGZlYXR1cmVzW2ldLnZhbHVlKTtcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLmZvcm0gPSBuZXcgRm9ybUdyb3VwKGZvcm1Hcm91cE9iaik7XHJcbiAgfVxyXG5cclxuICBzYXZlKCkge1xyXG4gICAgaWYgKHRoaXMubW9kYWxCdXN5KSByZXR1cm47XHJcblxyXG4gICAgdGhpcy5tb2RhbEJ1c3kgPSB0cnVlO1xyXG5cclxuICAgIGxldCBmZWF0dXJlcyA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZS5nZXRGZWF0dXJlcyk7XHJcblxyXG4gICAgZmVhdHVyZXMgPSBmZWF0dXJlcy5tYXAoKGZlYXR1cmUsIGkpID0+ICh7XHJcbiAgICAgIG5hbWU6IGZlYXR1cmUubmFtZSxcclxuICAgICAgdmFsdWU6ICF0aGlzLmZvcm0udmFsdWVbaV0gfHwgdGhpcy5mb3JtLnZhbHVlW2ldID09PSAnZmFsc2UnID8gbnVsbCA6IHRoaXMuZm9ybS52YWx1ZVtpXSxcclxuICAgIH0pKTtcclxuXHJcbiAgICB0aGlzLnN0b3JlXHJcbiAgICAgIC5kaXNwYXRjaChcclxuICAgICAgICBuZXcgVXBkYXRlRmVhdHVyZXMoe1xyXG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXHJcbiAgICAgICAgICBwcm92aWRlck5hbWU6IHRoaXMucHJvdmlkZXJOYW1lLFxyXG4gICAgICAgICAgZmVhdHVyZXMsXHJcbiAgICAgICAgfSksXHJcbiAgICAgIClcclxuICAgICAgLnBpcGUoZmluYWxpemUoKCkgPT4gKHRoaXMubW9kYWxCdXN5ID0gZmFsc2UpKSlcclxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=