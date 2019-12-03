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
                template: "<abp-modal size=\"md\" [(visible)]=\"visible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h3>{{ 'AbpFeatureManagement::Features' | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form *ngIf=\"form\" (ngSubmit)=\"save()\" [formGroup]=\"form\" validateOnSubmit>\n      <div\n        class=\"row my-3\"\n        *ngFor=\"let feature of features$ | async; let i = index\"\n        [ngSwitch]=\"feature.valueType.name\"\n      >\n        <div class=\"col-4\">{{ feature.name }}</div>\n        <div class=\"col-8\" *ngSwitchCase=\"'ToggleStringValueType'\">\n          <input type=\"checkbox\" name=\"feature.name\" [formControlName]=\"i\" />\n        </div>\n        <div class=\"col-8\" *ngSwitchCase=\"'FreeTextStringValueType'\">\n          <input type=\"text\" name=\"feature.name\" [formControlName]=\"i\" />\n        </div>\n      </div>\n      <div *ngIf=\"!(features$ | async)?.length\">\n        {{ 'AbpFeatureManagement::NoFeatureFoundMessage' | abpLocalization }}\n      </div>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <ng-container *ngIf=\"(features$ | async)?.length\">\n      <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n        {{ 'AbpFeatureManagement::Cancel' | abpLocalization }}\n      </button>\n      <abp-button iconClass=\"fa fa-check\" [disabled]=\"form?.invalid || modalBusy\" (click)=\"save()\">\n        {{ 'AbpFeatureManagement::Save' | abpLocalization }}\n      </abp-button>\n    </ng-container>\n  </ng-template>\n</abp-modal>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZmVhdHVyZS1tYW5hZ2VtZW50L2ZlYXR1cmUtbWFuYWdlbWVudC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFhLE1BQU0sRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFDakcsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEVBQUUsV0FBVyxFQUFFLGNBQWMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUU1RCxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBRSxXQUFXLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN4RCxPQUFPLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBTWpELE1BQU0sT0FBTywwQkFBMEI7Ozs7SUE4QnJDLFlBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBVGIsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBSy9ELGNBQVMsR0FBRyxLQUFLLENBQUM7SUFJaUIsQ0FBQzs7OztJQXJCcEMsSUFDSSxPQUFPO1FBQ1QsT0FBTyxJQUFJLENBQUMsUUFBUSxDQUFDO0lBQ3ZCLENBQUM7Ozs7O0lBRUQsSUFBSSxPQUFPLENBQUMsS0FBYztRQUN4QixJQUFJLENBQUMsUUFBUSxHQUFHLEtBQUssQ0FBQztRQUN0QixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUUvQixJ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QUFNLENBQUMsc0JBQXNCLENBQUMsV0FBVyxDQUFDO3NDQUNoQyxVQUFVOzZEQUE4Qjs7O0lBdkJuRCxpREFDb0I7O0lBRXBCLGtEQUNxQjs7Ozs7SUFFckIsOENBQW1COztJQWNuQixtREFBK0Q7O0lBRS9ELCtDQUNtRDs7SUFFbkQsK0NBQWtCOztJQUVsQiwwQ0FBZ0I7Ozs7O0lBRUosMkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPbkNoYW5nZXMsIE91dHB1dCwgU2ltcGxlQ2hhbmdlcyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IEdldEZlYXR1cmVzLCBVcGRhdGVGZWF0dXJlcyB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnQgfSBmcm9tICcuLi8uLi9tb2RlbHMvZmVhdHVyZS1tYW5hZ2VtZW50JztcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMnO1xuaW1wb3J0IHsgRm9ybUdyb3VwLCBGb3JtQ29udHJvbCB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IHBsdWNrLCBmaW5hbGl6ZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWZlYXR1cmUtbWFuYWdlbWVudCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9mZWF0dXJlLW1hbmFnZW1lbnQuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBGZWF0dXJlTWFuYWdlbWVudENvbXBvbmVudCB7XG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgQElucHV0KClcbiAgcHJvdmlkZXJOYW1lOiBzdHJpbmc7XG5cbiAgcHJvdGVjdGVkIF92aXNpYmxlO1xuXG4gIEBJbnB1dCgpXG4gIGdldCB2aXNpYmxlKCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLl92aXNpYmxlO1xuICB9XG5cbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcbiAgICB0aGlzLl92aXNpYmxlID0gdmFsdWU7XG4gICAgdGhpcy52aXNpYmxlQ2hhbmdlLmVtaXQodmFsdWUpO1xuXG4gICAgaWYgKHZhbHVlKSB0aGlzLm9wZW5Nb2RhbCgpO1xuICB9XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IHZpc2libGVDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPGJvb2xlYW4+KCk7XG5cbiAgQFNlbGVjdChGZWF0dXJlTWFuYWdlbWVudFN0YXRlLmdldEZlYXR1cmVzKVxuICBmZWF0dXJlcyQ6IE9ic2VydmFibGU8RmVhdHVyZU1hbmFnZW1lbnQuRmVhdHVyZVtdPjtcblxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcblxuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignUHJvdmlkZXIgS2V5IGFuZCBQcm92aWRlciBOYW1lIGFyZSByZXF1aXJlZC4nKTtcbiAgICB9XG5cbiAgICB0aGlzLmdldEZlYXR1cmVzKCk7XG4gIH1cblxuICBnZXRGZWF0dXJlcygpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBHZXRGZWF0dXJlcyh7XG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXG4gICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAucGlwZShwbHVjaygnRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZScsICdmZWF0dXJlcycpKVxuICAgICAgLnN1YnNjcmliZShmZWF0dXJlcyA9PiB7XG4gICAgICAgIHRoaXMuYnVpbGRGb3JtKGZlYXR1cmVzKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgYnVpbGRGb3JtKGZlYXR1cmVzKSB7XG4gICAgY29uc3QgZm9ybUdyb3VwT2JqID0ge307XG5cbiAgICBmb3IgKGxldCBpID0gMDsgaSA8IGZlYXR1cmVzLmxlbmd0aDsgaSsrKSB7XG4gICAgICBmb3JtR3JvdXBPYmpbaV0gPSBuZXcgRm9ybUNvbnRyb2woZmVhdHVyZXNbaV0udmFsdWUgPT09ICdmYWxzZScgPyBudWxsIDogZmVhdHVyZXNbaV0udmFsdWUpO1xuICAgIH1cblxuICAgIHRoaXMuZm9ybSA9IG5ldyBGb3JtR3JvdXAoZm9ybUdyb3VwT2JqKTtcbiAgfVxuXG4gIHNhdmUoKSB7XG4gICAgaWYgKHRoaXMubW9kYWxCdXN5KSByZXR1cm47XG5cbiAgICB0aGlzLm1vZGFsQnVzeSA9IHRydWU7XG5cbiAgICBsZXQgZmVhdHVyZXMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KEZlYXR1cmVNYW5hZ2VtZW50U3RhdGUuZ2V0RmVhdHVyZXMpO1xuXG4gICAgZmVhdHVyZXMgPSBmZWF0dXJlcy5tYXAoKGZlYXR1cmUsIGkpID0+ICh7XG4gICAgICBuYW1lOiBmZWF0dXJlLm5hbWUsXG4gICAgICB2YWx1ZTogIXRoaXMuZm9ybS52YWx1ZVtpXSB8fCB0aGlzLmZvcm0udmFsdWVbaV0gPT09ICdmYWxzZScgPyBudWxsIDogdGhpcy5mb3JtLnZhbHVlW2ldLFxuICAgIH0pKTtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgbmV3IFVwZGF0ZUZlYXR1cmVzKHtcbiAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcbiAgICAgICAgICBwcm92aWRlck5hbWU6IHRoaXMucHJvdmlkZXJOYW1lLFxuICAgICAgICAgIGZlYXR1cmVzLFxuICAgICAgICB9KSxcbiAgICAgIClcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLm1vZGFsQnVzeSA9IGZhbHNlKSkpXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgICB9KTtcbiAgfVxufVxuIl19