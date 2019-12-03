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
                    template: "<abp-modal size=\"md\" [(visible)]=\"visible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h3>{{ 'AbpFeatureManagement::Features' | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form *ngIf=\"form\" (ngSubmit)=\"save()\" [formGroup]=\"form\" validateOnSubmit>\n      <div\n        class=\"row my-3\"\n        *ngFor=\"let feature of features$ | async; let i = index\"\n        [ngSwitch]=\"feature.valueType.name\"\n      >\n        <div class=\"col-4\">{{ feature.name }}</div>\n        <div class=\"col-8\" *ngSwitchCase=\"'ToggleStringValueType'\">\n          <input type=\"checkbox\" name=\"feature.name\" [formControlName]=\"i\" />\n        </div>\n        <div class=\"col-8\" *ngSwitchCase=\"'FreeTextStringValueType'\">\n          <input type=\"text\" name=\"feature.name\" [formControlName]=\"i\" />\n        </div>\n      </div>\n      <div *ngIf=\"!(features$ | async)?.length\">\n        {{ 'AbpFeatureManagement::NoFeatureFoundMessage' | abpLocalization }}\n      </div>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <ng-container *ngIf=\"(features$ | async)?.length\">\n      <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n        {{ 'AbpFeatureManagement::Cancel' | abpLocalization }}\n      </button>\n      <abp-button iconClass=\"fa fa-check\" [disabled]=\"form?.invalid || modalBusy\" (click)=\"save()\">\n        {{ 'AbpFeatureManagement::Save' | abpLocalization }}\n      </abp-button>\n    </ng-container>\n  </ng-template>\n</abp-modal>\n"
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
    tslib_1.__decorate([
        Select(FeatureManagementState.getFeatures),
        tslib_1.__metadata("design:type", Observable)
    ], FeatureManagementComponent.prototype, "features$", void 0);
    return FeatureManagementComponent;
}());
export { FeatureManagementComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZmVhdHVyZS1tYW5hZ2VtZW50L2ZlYXR1cmUtbWFuYWdlbWVudC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFhLE1BQU0sRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFDakcsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEVBQUUsV0FBVyxFQUFFLGNBQWMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUU1RCxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBRSxXQUFXLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN4RCxPQUFPLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRWpEO0lBa0NFLG9DQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztRQVRiLGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQUsvRCxjQUFTLEdBQUcsS0FBSyxDQUFDO0lBSWlCLENBQUM7SUFyQnBDLHNCQUNJLCtDQUFPOzs7O1FBRFg7WUFFRSxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUM7UUFDdkIsQ0FBQzs7Ozs7UUFFRCxVQUFZLEtBQWM7WUFDeEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxLQUFLLENBQUM7WUFDdEIsSUFBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7WUFFL0IsSUFBSSxLQUFLO2dCQUFFLElBQUksQ0FB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QUFLOytCQUdMLEtBQUs7MEJBS0wsS0FBSztnQ0FZTCxNQUFNOztJQUdQO1FBREMsTUFBTSxDQUFDLHNCQUFzQixDQUFDLFdBQVcsQ0FBQzswQ0FDaEMsVUFBVTtpRUFBOEI7SUFpRXJELGlDQUFDO0NBQUEsQUE3RkQsSUE2RkM7U0F6RlksMEJBQTBCOzs7SUFDckMsaURBQ29COztJQUVwQixrREFDcUI7Ozs7O0lBRXJCLDhDQUFtQjs7SUFjbkIsbURBQStEOztJQUUvRCwrQ0FDbUQ7O0lBRW5ELCtDQUFrQjs7SUFFbEIsMENBQWdCOzs7OztJQUVKLDJDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT25DaGFuZ2VzLCBPdXRwdXQsIFNpbXBsZUNoYW5nZXMgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBHZXRGZWF0dXJlcywgVXBkYXRlRmVhdHVyZXMgfSBmcm9tICcuLi8uLi9hY3Rpb25zJztcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50IH0gZnJvbSAnLi4vLi4vbW9kZWxzL2ZlYXR1cmUtbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzJztcbmltcG9ydCB7IEZvcm1Hcm91cCwgRm9ybUNvbnRyb2wgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBwbHVjaywgZmluYWxpemUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1mZWF0dXJlLW1hbmFnZW1lbnQnLFxuICB0ZW1wbGF0ZVVybDogJy4vZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgRmVhdHVyZU1hbmFnZW1lbnRDb21wb25lbnQge1xuICBASW5wdXQoKVxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyTmFtZTogc3RyaW5nO1xuXG4gIHByb3RlY3RlZCBfdmlzaWJsZTtcblxuICBASW5wdXQoKVxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcbiAgfVxuXG4gIHNldCB2aXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgdGhpcy5fdmlzaWJsZSA9IHZhbHVlO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcblxuICAgIGlmICh2YWx1ZSkgdGhpcy5vcGVuTW9kYWwoKTtcbiAgfVxuXG4gIEBPdXRwdXQoKSByZWFkb25seSB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBTZWxlY3QoRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZS5nZXRGZWF0dXJlcylcbiAgZmVhdHVyZXMkOiBPYnNlcnZhYmxlPEZlYXR1cmVNYW5hZ2VtZW50LkZlYXR1cmVbXT47XG5cbiAgbW9kYWxCdXN5ID0gZmFsc2U7XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIG9wZW5Nb2RhbCgpIHtcbiAgICBpZiAoIXRoaXMucHJvdmlkZXJLZXkgfHwgIXRoaXMucHJvdmlkZXJOYW1lKSB7XG4gICAgICB0aHJvdyBuZXcgRXJyb3IoJ1Byb3ZpZGVyIEtleSBhbmQgUHJvdmlkZXIgTmFtZSBhcmUgcmVxdWlyZWQuJyk7XG4gICAgfVxuXG4gICAgdGhpcy5nZXRGZWF0dXJlcygpO1xuICB9XG5cbiAgZ2V0RmVhdHVyZXMoKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKFxuICAgICAgICBuZXcgR2V0RmVhdHVyZXMoe1xuICAgICAgICAgIHByb3ZpZGVyS2V5OiB0aGlzLnByb3ZpZGVyS2V5LFxuICAgICAgICAgIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWUsXG4gICAgICAgIH0pLFxuICAgICAgKVxuICAgICAgLnBpcGUocGx1Y2soJ0ZlYXR1cmVNYW5hZ2VtZW50U3RhdGUnLCAnZmVhdHVyZXMnKSlcbiAgICAgIC5zdWJzY3JpYmUoZmVhdHVyZXMgPT4ge1xuICAgICAgICB0aGlzLmJ1aWxkRm9ybShmZWF0dXJlcyk7XG4gICAgICB9KTtcbiAgfVxuXG4gIGJ1aWxkRm9ybShmZWF0dXJlcykge1xuICAgIGNvbnN0IGZvcm1Hcm91cE9iaiA9IHt9O1xuXG4gICAgZm9yIChsZXQgaSA9IDA7IGkgPCBmZWF0dXJlcy5sZW5ndGg7IGkrKykge1xuICAgICAgZm9ybUdyb3VwT2JqW2ldID0gbmV3IEZvcm1Db250cm9sKGZlYXR1cmVzW2ldLnZhbHVlID09PSAnZmFsc2UnID8gbnVsbCA6IGZlYXR1cmVzW2ldLnZhbHVlKTtcbiAgICB9XG5cbiAgICB0aGlzLmZvcm0gPSBuZXcgRm9ybUdyb3VwKGZvcm1Hcm91cE9iaik7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGlmICh0aGlzLm1vZGFsQnVzeSkgcmV0dXJuO1xuXG4gICAgdGhpcy5tb2RhbEJ1c3kgPSB0cnVlO1xuXG4gICAgbGV0IGZlYXR1cmVzID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChGZWF0dXJlTWFuYWdlbWVudFN0YXRlLmdldEZlYXR1cmVzKTtcblxuICAgIGZlYXR1cmVzID0gZmVhdHVyZXMubWFwKChmZWF0dXJlLCBpKSA9PiAoe1xuICAgICAgbmFtZTogZmVhdHVyZS5uYW1lLFxuICAgICAgdmFsdWU6ICF0aGlzLmZvcm0udmFsdWVbaV0gfHwgdGhpcy5mb3JtLnZhbHVlW2ldID09PSAnZmFsc2UnID8gbnVsbCA6IHRoaXMuZm9ybS52YWx1ZVtpXSxcbiAgICB9KSk7XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBVcGRhdGVGZWF0dXJlcyh7XG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXG4gICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcbiAgICAgICAgICBmZWF0dXJlcyxcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiAodGhpcy5tb2RhbEJ1c3kgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgICAgfSk7XG4gIH1cbn1cbiJdfQ==