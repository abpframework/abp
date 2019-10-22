/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { GetFeatures, UpdateFeatures } from '../../actions';
import { FeatureManagementState } from '../../states';
import { FormGroup, FormControl } from '@angular/forms';
import { pluck } from 'rxjs/operators';
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
            providerName: this.providerName
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
            value: !this.form.value[i] || this.form.value[i] === 'false' ? null : this.form.value[i]
        })));
        this.store
            .dispatch(new UpdateFeatures({
            providerKey: this.providerKey,
            providerName: this.providerName,
            features
        }))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.modalBusy = false;
            this.visible = false;
        }));
    }
}
FeatureManagementComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-feature-management',
                template: "<abp-modal size=\"md\" [(visible)]=\"visible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h3>{{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}</h3>\r\n  </ng-template>\r\n\r\n  <ng-template #abpBody>\r\n    <form *ngIf=\"form\" (ngSubmit)=\"save()\" [formGroup]=\"form\">\r\n      <div\r\n        class=\"row my-3\"\r\n        *ngFor=\"let feature of features$ | async; let i = index\"\r\n        [ngSwitch]=\"feature.valueType.name\"\r\n      >\r\n        <div class=\"col-4\">{{ feature.name }}</div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'ToggleStringValueType'\">\r\n          <input type=\"checkbox\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n        <div class=\"col-8\" *ngSwitchCase=\"'FreeTextStringValueType'\">\r\n          <input type=\"text\" name=\"feature.name\" [formControlName]=\"i\" />\r\n        </div>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpFeatureManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\">\r\n      {{ 'AbpFeatureManagement::Save' | abpLocalization }}\r\n    </abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZmVhdHVyZS1tYW5hZ2VtZW50L2ZlYXR1cmUtbWFuYWdlbWVudC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQWEsTUFBTSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUNqRyxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxXQUFXLEVBQUUsY0FBYyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRTVELE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3hELE9BQU8sRUFBRSxLQUFLLEVBQU8sTUFBTSxnQkFBZ0IsQ0FBQztBQU01QyxNQUFNLE9BQU8sMEJBQTBCOzs7O0lBOEJyQyxZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztRQVRiLGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQUsvRCxjQUFTLEdBQUcsS0FBSyxDQUFDO0lBSWlCLENBQUM7Ozs7SUFyQnBDLElBQ0ksT0FBTztRQUNULE9BQU8sSUFBSSxDQUFDLFFBQVEsQ0FBQztJQUN2QixDQUFDOzs7OztJQUVELElBQUksT0FBTyxDQUFDLEtBQWM7UUFDeEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxLQUFLLENBQUM7UUFDdEIsSUFBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7UUFFL0IsSUFBSSxLQUFLO1lB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QWNuQixtREFBK0Q7O0lBRS9ELCtDQUNtRDs7SUFFbkQsK0NBQWtCOztJQUVsQiwwQ0FBZ0I7Ozs7O0lBRUosMkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPbkNoYW5nZXMsIE91dHB1dCwgU2ltcGxlQ2hhbmdlcyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IEdldEZlYXR1cmVzLCBVcGRhdGVGZWF0dXJlcyB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xyXG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudCB9IGZyb20gJy4uLy4uL21vZGVscy9mZWF0dXJlLW1hbmFnZW1lbnQnO1xyXG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzJztcclxuaW1wb3J0IHsgRm9ybUdyb3VwLCBGb3JtQ29udHJvbCB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcclxuaW1wb3J0IHsgcGx1Y2ssIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLWZlYXR1cmUtbWFuYWdlbWVudCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2ZlYXR1cmUtbWFuYWdlbWVudC5jb21wb25lbnQuaHRtbCdcclxufSlcclxuZXhwb3J0IGNsYXNzIEZlYXR1cmVNYW5hZ2VtZW50Q29tcG9uZW50IHtcclxuICBASW5wdXQoKVxyXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgcHJvdmlkZXJOYW1lOiBzdHJpbmc7XHJcblxyXG4gIHByb3RlY3RlZCBfdmlzaWJsZTtcclxuXHJcbiAgQElucHV0KClcclxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcclxuICAgIHJldHVybiB0aGlzLl92aXNpYmxlO1xyXG4gIH1cclxuXHJcbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcclxuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcclxuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuXHJcbiAgICBpZiAodmFsdWUpIHRoaXMub3Blbk1vZGFsKCk7XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcclxuXHJcbiAgQFNlbGVjdChGZWF0dXJlTWFuYWdlbWVudFN0YXRlLmdldEZlYXR1cmVzKVxyXG4gIGZlYXR1cmVzJDogT2JzZXJ2YWJsZTxGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlW10+O1xyXG5cclxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcclxuXHJcbiAgZm9ybTogRm9ybUdyb3VwO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgb3Blbk1vZGFsKCkge1xyXG4gICAgaWYgKCF0aGlzLnByb3ZpZGVyS2V5IHx8ICF0aGlzLnByb3ZpZGVyTmFtZSkge1xyXG4gICAgICB0aHJvdyBuZXcgRXJyb3IoJ1Byb3ZpZGVyIEtleSBhbmQgUHJvdmlkZXIgTmFtZSBhcmUgcmVxdWlyZWQuJyk7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5nZXRGZWF0dXJlcygpO1xyXG4gIH1cclxuXHJcbiAgZ2V0RmVhdHVyZXMoKSB7XHJcbiAgICB0aGlzLnN0b3JlXHJcbiAgICAgIC5kaXNwYXRjaChcclxuICAgICAgICBuZXcgR2V0RmVhdHVyZXMoe1xyXG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXHJcbiAgICAgICAgICBwcm92aWRlck5hbWU6IHRoaXMucHJvdmlkZXJOYW1lXHJcbiAgICAgICAgfSlcclxuICAgICAgKVxyXG4gICAgICAucGlwZShwbHVjaygnRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZScsICdmZWF0dXJlcycpKVxyXG4gICAgICAuc3Vic2NyaWJlKGZlYXR1cmVzID0+IHtcclxuICAgICAgICB0aGlzLmJ1aWxkRm9ybShmZWF0dXJlcyk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgYnVpbGRGb3JtKGZlYXR1cmVzKSB7XHJcbiAgICBjb25zdCBmb3JtR3JvdXBPYmogPSB7fTtcclxuXHJcbiAgICBmb3IgKGxldCBpID0gMDsgaSA8IGZlYXR1cmVzLmxlbmd0aDsgaSsrKSB7XHJcbiAgICAgIGZvcm1Hcm91cE9ialtpXSA9IG5ldyBGb3JtQ29udHJvbChmZWF0dXJlc1tpXS52YWx1ZSA9PT0gJ2ZhbHNlJyA/IG51bGwgOiBmZWF0dXJlc1tpXS52YWx1ZSk7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5mb3JtID0gbmV3IEZvcm1Hcm91cChmb3JtR3JvdXBPYmopO1xyXG4gIH1cclxuXHJcbiAgc2F2ZSgpIHtcclxuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcclxuXHJcbiAgICBsZXQgZmVhdHVyZXMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KEZlYXR1cmVNYW5hZ2VtZW50U3RhdGUuZ2V0RmVhdHVyZXMpO1xyXG5cclxuICAgIGZlYXR1cmVzID0gZmVhdHVyZXMubWFwKChmZWF0dXJlLCBpKSA9PiAoe1xyXG4gICAgICBuYW1lOiBmZWF0dXJlLm5hbWUsXHJcbiAgICAgIHZhbHVlOiAhdGhpcy5mb3JtLnZhbHVlW2ldIHx8IHRoaXMuZm9ybS52YWx1ZVtpXSA9PT0gJ2ZhbHNlJyA/IG51bGwgOiB0aGlzLmZvcm0udmFsdWVbaV1cclxuICAgIH0pKTtcclxuXHJcbiAgICB0aGlzLnN0b3JlXHJcbiAgICAgIC5kaXNwYXRjaChcclxuICAgICAgICBuZXcgVXBkYXRlRmVhdHVyZXMoe1xyXG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXHJcbiAgICAgICAgICBwcm92aWRlck5hbWU6IHRoaXMucHJvdmlkZXJOYW1lLFxyXG4gICAgICAgICAgZmVhdHVyZXNcclxuICAgICAgICB9KVxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICAgIHRoaXMubW9kYWxCdXN5ID0gZmFsc2U7XHJcbiAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=