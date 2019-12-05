/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/config-state.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
export class ConfigStateService {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    getAll() {
        return this.store.selectSnapshot(ConfigState.getAll);
    }
    /**
     * @return {?}
     */
    getApplicationInfo() {
        return this.store.selectSnapshot(ConfigState.getApplicationInfo);
    }
    /**
     * @param {...?} args
     * @return {?}
     */
    getOne(...args) {
        return this.store.selectSnapshot(ConfigState.getOne(...args));
    }
    /**
     * @param {...?} args
     * @return {?}
     */
    getDeep(...args) {
        return this.store.selectSnapshot(ConfigState.getDeep(...args));
    }
    /**
     * @param {...?} args
     * @return {?}
     */
    getRoute(...args) {
        return this.store.selectSnapshot(ConfigState.getRoute(...args));
    }
    /**
     * @param {...?} args
     * @return {?}
     */
    getApiUrl(...args) {
        return this.store.selectSnapshot(ConfigState.getApiUrl(...args));
    }
    /**
     * @param {...?} args
     * @return {?}
     */
    getSetting(...args) {
        return this.store.selectSnapshot(ConfigState.getSetting(...args));
    }
    /**
     * @param {...?} args
     * @return {?}
     */
    getSettings(...args) {
        return this.store.selectSnapshot(ConfigState.getSettings(...args));
    }
    /**
     * @param {...?} args
     * @return {?}
     */
    getGrantedPolicy(...args) {
        return this.store.selectSnapshot(ConfigState.getGrantedPolicy(...args));
    }
    /**
     * @param {...?} args
     * @return {?}
     */
    getLocalization(...args) {
        return this.store.selectSnapshot(ConfigState.getLocalization(...args));
    }
}
ConfigStateService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
ConfigStateService.ctorParameters = () => [
    { type: Store }
];
/** @nocollapse */ ConfigStateService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ConfigStateService_Factory() { return new ConfigStateService(i0.ɵɵinject(i1.Store)); }, token: ConfigStateService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    ConfigStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvY29uZmlnLXN0YXRlLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQzs7O0FBS3hDLE1BQU0sT0FBTyxrQkFBa0I7Ozs7SUFDN0IsWUFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLE1BQU07UUFDSixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUMsQ0FBQztJQUN2RCxDQUFDOzs7O0lBRUQsa0JBQWtCO1FBQ2hCLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGtCQUFrQixDQUFDLENBQUM7SUFDbkUsQ0FBQzs7Ozs7SUFFRCxNQUFNLENBQUMsR0FBRyxJQUEyQztRQUNuRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUMsR0FBRyxJQUFJLENBQUMsQ0FBQyxDQUFDO0lBQ2hFLENBQUM7Ozs7O0lBRUQsT0FBTyxDQUFDLEdBQUcsSUFBNEM7UUFDckQsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsT0FBTyxDQUFDLEdBQUcsSUFBSSxDQUFDLENBQUMsQ0FBQztJQUNqRSxDQUFDOzs7OztJQUVELFFBQVEsQ0FBQyxHQUFHLElBQTZDO1FBQ3ZELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFFBQVEsQ0FBQyxHQUFHLElBQUksQ0FBQyxDQUFDLENBQUM7SUFDbEUsQ0FBQzs7Ozs7SUFFRCxTQUFTLENBQUMsR0FBRyxJQUE4QztRQUN6RCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxTQUFTLENBQUMsR0FBRyxJQUFJLENBQUMsQ0FBQyxDQUFDO0lBQ25FLENBQUM7Ozs7O0lBRUQsVUFBVSxDQUFDLEdBQUcsSUFBK0M7UUFDM0QsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsVUFBVSxDQUFDLEdBQUcsSUFBSSxDQUFDLENBQUMsQ0FBQztJQUNwRSxDQUFDOzs7OztJQUVELFdBQVcsQ0FBQyxHQUFHLElBQWdEO1FBQzdELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFdBQVcsQ0FBQyxHQUFHLElBQUksQ0FBQyxDQUFDLENBQUM7SUFDckUsQ0FBQzs7Ozs7SUFFRCxnQkFBZ0IsQ0FBQyxHQUFHLElBQXFEO1FBQ3ZFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLEdBQUcsSUFBSSxDQUFDLENBQUMsQ0FBQztJQUMxRSxDQUFDOzs7OztJQUVELGVBQWUsQ0FBQyxHQUFHLElBQW9EO1FBQ3JFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGVBQWUsQ0FBQyxHQUFHLElBQUksQ0FBQyxDQUFDLENBQUM7SUFDekUsQ0FBQzs7O1lBNUNGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQUxRLEtBQUs7Ozs7Ozs7O0lBT0EsbUNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBDb25maWdTdGF0ZVNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBnZXRBbGwoKSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QWxsKTtcbiAgfVxuXG4gIGdldEFwcGxpY2F0aW9uSW5mbygpIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcHBsaWNhdGlvbkluZm8pO1xuICB9XG5cbiAgZ2V0T25lKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldE9uZT4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRPbmUoLi4uYXJncykpO1xuICB9XG5cbiAgZ2V0RGVlcCguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXREZWVwPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldERlZXAoLi4uYXJncykpO1xuICB9XG5cbiAgZ2V0Um91dGUoLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0Um91dGU+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0Um91dGUoLi4uYXJncykpO1xuICB9XG5cbiAgZ2V0QXBpVXJsKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldEFwaVVybD4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcGlVcmwoLi4uYXJncykpO1xuICB9XG5cbiAgZ2V0U2V0dGluZyguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRTZXR0aW5nPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFNldHRpbmcoLi4uYXJncykpO1xuICB9XG5cbiAgZ2V0U2V0dGluZ3MoLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0U2V0dGluZ3M+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0U2V0dGluZ3MoLi4uYXJncykpO1xuICB9XG5cbiAgZ2V0R3JhbnRlZFBvbGljeSguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5Pikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3koLi4uYXJncykpO1xuICB9XG5cbiAgZ2V0TG9jYWxpemF0aW9uKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbj4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRMb2NhbGl6YXRpb24oLi4uYXJncykpO1xuICB9XG59XG4iXX0=