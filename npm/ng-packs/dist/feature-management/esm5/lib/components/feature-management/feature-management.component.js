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
            providerName: this.providerName
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
            value: !_this.form.value[i] || _this.form.value[i] === 'false' ? null : _this.form.value[i]
        }); }));
        this.store
            .dispatch(new UpdateFeatures({
            providerKey: this.providerKey,
            providerName: this.providerName,
            features: features
        }))
            .subscribe((/**
         * @return {?}
         */
        function () {
            _this.modalBusy = false;
            _this.visible = false;
        }));
    };
    FeatureManagementComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-feature-management',
                    template: "<abp-modal size=\"md\" [(visible)]=\"visible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h3>{{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form *ngIf=\"form\" (ngSubmit)=\"save()\" [formGroup]=\"form\">\n      <div\n        class=\"row my-3\"\n        *ngFor=\"let feature of features$ | async; let i = index\"\n        [ngSwitch]=\"feature.valueType.name\"\n      >\n        <div class=\"col-4\">{{ feature.name }}</div>\n        <div class=\"col-8\" *ngSwitchCase=\"'ToggleStringValueType'\">\n          <input type=\"checkbox\" name=\"feature.name\" [formControlName]=\"i\" />\n        </div>\n        <div class=\"col-8\" *ngSwitchCase=\"'FreeTextStringValueType'\">\n          <input type=\"text\" name=\"feature.name\" [formControlName]=\"i\" />\n        </div>\n      </div>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpFeatureManagement::Cancel' | abpLocalization }}\n    </button>\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\">\n      {{ 'AbpFeatureManagement::Save' | abpLocalization }}\n    </abp-button>\n  </ng-template>\n</abp-modal>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZmVhdHVyZS1tYW5hZ2VtZW50L2ZlYXR1cmUtbWFuYWdlbWVudC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQWEsTUFBTSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUNqRyxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxXQUFXLEVBQUUsY0FBYyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRTVELE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3hELE9BQU8sRUFBRSxLQUFLLEVBQU8sTUFBTSxnQkFBZ0IsQ0FBQztBQUU1QztJQWtDRSxvQ0FBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87UUFUYixrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFXLENBQUM7UUFLL0QsY0FBUyxHQUFHLEtBQUssQ0FBQztJQUlpQixDQUFDO0lBckJwQyxzQkFDSSwrQ0FBTzs7OztRQURYO1lBRUUsT0FBTyxJQUFJLENBQUMsUUFBUSxDQUFDO1FBQ3ZCLENBQUM7Ozs7O1FBRUQsVUFBWSxLQUFjO1lBQ3hCLElBQUksQ0FBQyxRQUFRLEdBQUcsS0FBSyxDQUFDO1lBQ3RCLElBQUksQ0FBQyxhQUFhLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1lBRS9CLElBQUksS0FBSztnQkFBRSxJQUFJLENBQUMsU0FBUyxFQUFF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aUNBQUM7Q0FBQSxBQTNGRCxJQTJGQztTQXZGWSwwQkFBMEI7OztJQUNyQyxpREFDb0I7O0lBRXBCLGtEQUNxQjs7Ozs7SUFFckIsOENBQW1COztJQWNuQixtREFBK0Q7O0lBRS9ELCtDQUNtRDs7SUFFbkQsK0NBQWtCOztJQUVsQiwwQ0FBZ0I7Ozs7O0lBRUosMkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPbkNoYW5nZXMsIE91dHB1dCwgU2ltcGxlQ2hhbmdlcyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IEdldEZlYXR1cmVzLCBVcGRhdGVGZWF0dXJlcyB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnQgfSBmcm9tICcuLi8uLi9tb2RlbHMvZmVhdHVyZS1tYW5hZ2VtZW50JztcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMnO1xuaW1wb3J0IHsgRm9ybUdyb3VwLCBGb3JtQ29udHJvbCB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IHBsdWNrLCB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1mZWF0dXJlLW1hbmFnZW1lbnQnLFxuICB0ZW1wbGF0ZVVybDogJy4vZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJ1xufSlcbmV4cG9ydCBjbGFzcyBGZWF0dXJlTWFuYWdlbWVudENvbXBvbmVudCB7XG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgQElucHV0KClcbiAgcHJvdmlkZXJOYW1lOiBzdHJpbmc7XG5cbiAgcHJvdGVjdGVkIF92aXNpYmxlO1xuXG4gIEBJbnB1dCgpXG4gIGdldCB2aXNpYmxlKCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLl92aXNpYmxlO1xuICB9XG5cbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcbiAgICB0aGlzLl92aXNpYmxlID0gdmFsdWU7XG4gICAgdGhpcy52aXNpYmxlQ2hhbmdlLmVtaXQodmFsdWUpO1xuXG4gICAgaWYgKHZhbHVlKSB0aGlzLm9wZW5Nb2RhbCgpO1xuICB9XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IHZpc2libGVDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPGJvb2xlYW4+KCk7XG5cbiAgQFNlbGVjdChGZWF0dXJlTWFuYWdlbWVudFN0YXRlLmdldEZlYXR1cmVzKVxuICBmZWF0dXJlcyQ6IE9ic2VydmFibGU8RmVhdHVyZU1hbmFnZW1lbnQuRmVhdHVyZVtdPjtcblxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcblxuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignUHJvdmlkZXIgS2V5IGFuZCBQcm92aWRlciBOYW1lIGFyZSByZXF1aXJlZC4nKTtcbiAgICB9XG5cbiAgICB0aGlzLmdldEZlYXR1cmVzKCk7XG4gIH1cblxuICBnZXRGZWF0dXJlcygpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBHZXRGZWF0dXJlcyh7XG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXG4gICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZVxuICAgICAgICB9KVxuICAgICAgKVxuICAgICAgLnBpcGUocGx1Y2soJ0ZlYXR1cmVNYW5hZ2VtZW50U3RhdGUnLCAnZmVhdHVyZXMnKSlcbiAgICAgIC5zdWJzY3JpYmUoZmVhdHVyZXMgPT4ge1xuICAgICAgICB0aGlzLmJ1aWxkRm9ybShmZWF0dXJlcyk7XG4gICAgICB9KTtcbiAgfVxuXG4gIGJ1aWxkRm9ybShmZWF0dXJlcykge1xuICAgIGNvbnN0IGZvcm1Hcm91cE9iaiA9IHt9O1xuXG4gICAgZm9yIChsZXQgaSA9IDA7IGkgPCBmZWF0dXJlcy5sZW5ndGg7IGkrKykge1xuICAgICAgZm9ybUdyb3VwT2JqW2ldID0gbmV3IEZvcm1Db250cm9sKGZlYXR1cmVzW2ldLnZhbHVlID09PSAnZmFsc2UnID8gbnVsbCA6IGZlYXR1cmVzW2ldLnZhbHVlKTtcbiAgICB9XG5cbiAgICB0aGlzLmZvcm0gPSBuZXcgRm9ybUdyb3VwKGZvcm1Hcm91cE9iaik7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcblxuICAgIGxldCBmZWF0dXJlcyA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZS5nZXRGZWF0dXJlcyk7XG5cbiAgICBmZWF0dXJlcyA9IGZlYXR1cmVzLm1hcCgoZmVhdHVyZSwgaSkgPT4gKHtcbiAgICAgIG5hbWU6IGZlYXR1cmUubmFtZSxcbiAgICAgIHZhbHVlOiAhdGhpcy5mb3JtLnZhbHVlW2ldIHx8IHRoaXMuZm9ybS52YWx1ZVtpXSA9PT0gJ2ZhbHNlJyA/IG51bGwgOiB0aGlzLmZvcm0udmFsdWVbaV1cbiAgICB9KSk7XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBVcGRhdGVGZWF0dXJlcyh7XG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXG4gICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcbiAgICAgICAgICBmZWF0dXJlc1xuICAgICAgICB9KVxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMubW9kYWxCdXN5ID0gZmFsc2U7XG4gICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgICAgfSk7XG4gIH1cbn1cbiJdfQ==