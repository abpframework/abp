/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RouteConfigLoadEnd, Router } from '@angular/router';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { Subject, timer } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import * as i0 from "@angular/core";
import * as i1 from "@angular/router";
import * as i2 from "@ngxs/store";
var SettingManagementService = /** @class */ (function () {
    function SettingManagementService(router, store) {
        var _this = this;
        this.router = router;
        this.store = store;
        this.settings = [];
        this.selected = (/** @type {?} */ ({}));
        this.destroy$ = new Subject();
        /** @type {?} */
        var timeout;
        this.router.events
            .pipe(filter((/**
         * @param {?} event
         * @return {?}
         */
        function (event) { return event instanceof RouteConfigLoadEnd; })), takeUntil(this.destroy$))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            if (timeout) {
                timeout.unsubscribe();
                _this.destroy$.next();
            }
            timeout = timer(150).subscribe((/**
             * @return {?}
             */
            function () {
                _this.setSettings();
            }));
        }));
    }
    /**
     * @return {?}
     */
    SettingManagementService.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () {
        this.destroy$.next();
    };
    /**
     * @return {?}
     */
    SettingManagementService.prototype.setSettings = /**
     * @return {?}
     */
    function () {
        var _this = this;
        setTimeout((/**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var route = _this.router.config.find((/**
             * @param {?} r
             * @return {?}
             */
            function (r) { return r.path === 'setting-management'; }));
            _this.settings = route.data.settings.sort((/**
             * @param {?} a
             * @param {?} b
             * @return {?}
             */
            function (a, b) { return a.order - b.order; }));
            _this.checkSelected();
        }), 0);
    };
    /**
     * @return {?}
     */
    SettingManagementService.prototype.checkSelected = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.selected = this.settings.find((/**
         * @param {?} setting
         * @return {?}
         */
        function (setting) { return setting.url === _this.router.url; })) || ((/** @type {?} */ ({})));
        if (!this.selected.name && this.settings.length) {
            this.setSelected(this.settings[0]);
        }
    };
    /**
     * @param {?} selected
     * @return {?}
     */
    SettingManagementService.prototype.setSelected = /**
     * @param {?} selected
     * @return {?}
     */
    function (selected) {
        this.selected = selected;
        this.store.dispatch(new Navigate([selected.url]));
    };
    SettingManagementService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    SettingManagementService.ctorParameters = function () { return [
        { type: Router },
        { type: Store }
    ]; };
    /** @nocollapse */ SettingManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function SettingManagementService_Factory() { return new SettingManagementService(i0.ɵɵinject(i1.Router), i0.ɵɵinject(i2.Store)); }, token: SettingManagementService, providedIn: "root" });
    return SettingManagementService;
}());
export { SettingManagementService };
if (false) {
    /** @type {?} */
    SettingManagementService.prototype.settings;
    /** @type {?} */
    SettingManagementService.prototype.selected;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.destroy$;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.router;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnNldHRpbmctbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9zZXR0aW5nLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQ0EsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDN0QsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLE9BQU8sRUFBZ0IsS0FBSyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3BELE9BQU8sRUFBRSxNQUFNLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7Ozs7QUFFbkQ7SUFRRSxrQ0FBb0IsTUFBYyxFQUFVLEtBQVk7UUFBeEQsaUJBZ0JDO1FBaEJtQixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQU54RCxhQUFRLEdBQWlCLEVBQUUsQ0FBQztRQUU1QixhQUFRLEdBQUcsbUJBQUEsRUFBRSxFQUFjLENBQUM7UUFFcEIsYUFBUSxHQUFHLElBQUksT0FBTyxFQUFFLENBQUM7O1lBRzNCLE9BQXFCO1FBQ3pCLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTTthQUNmLElBQUksQ0FDSCxNQUFNOzs7O1FBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLFlBQVksa0JBQWtCLEVBQW5DLENBQW1DLEVBQUMsRUFDcEQsU0FBUyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FDekI7YUFDQSxTQUFTOzs7O1FBQUMsVUFBQSxLQUFLO1lBQ2QsSUFBSSxPQUFPLEVBQUU7Z0JBQ1gsT0FBTyxDQUFDLFdBQVcsRUFBRSxDQUFDO2dCQUN0QixLQUFJLENBQUMsUUFBUSxDQUFDLElBQUksRUFBRSxDQUFDO2FBQ3RCO1lBQ0QsT0FBTyxHQUFHLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxTQUFTOzs7WUFBQztnQkFDN0IsS0FBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO1lBQ3JCLENBQUMsRUFBQyxDQUFDO1FBQ0wsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsOENBQVc7OztJQUFYO1FBQ0UsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUUsQ0FBQztJQUN2QixDQUFDOzs7O0lBRUQsOENBQVc7OztJQUFYO1FBQUEsaUJBTUM7UUFMQyxVQUFVOzs7UUFBQzs7Z0JBQ0gsS0FBSyxHQUFHLEtBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDLElBQUk7Ozs7WUFBQyxVQUFBLENBQUMsSUFBSSxPQUFBLENBQUMsQ0FBQyxJQUFJLEtBQUssb0JBQW9CLEVBQS9CLENBQStCLEVBQUM7WUFDM0UsS0FBSSxDQUFDLFFBQVEsR0FBRyxLQUFLLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJOzs7OztZQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQyxDQUFDO1lBQ3RFLEtBQUksQ0FBQyxhQUFhLEVBQUUsQ0FBQztRQUN2QixDQUFDLEdBQUUsQ0FBQyxDQUFDLENBQUM7SUFDUixDQUFDOzs7O0lBRUQsZ0RBQWE7OztJQUFiO1FBQUEsaUJBTUM7UUFMQyxJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSTs7OztRQUFDLFVBQUEsT0FBTyxJQUFJLE9BQUEsT0FBTyxDQUFDLEdBQUcsS0FBSyxLQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsRUFBL0IsQ0FBK0IsRUFBQyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFjLENBQUMsQ0FBQztRQUVyRyxJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLElBQUksSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDL0MsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDcEM7SUFDSCxDQUFDOzs7OztJQUVELDhDQUFXOzs7O0lBQVgsVUFBWSxRQUFvQjtRQUM5QixJQUFJLENBQUMsUUFBUSxHQUFHLFFBQVEsQ0FBQztRQUN6QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFFBQVEsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDcEQsQ0FBQzs7Z0JBakRGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7Z0JBTkwsTUFBTTtnQkFFMUIsS0FBSzs7O21DQUpkO0NBMERDLEFBbERELElBa0RDO1NBakRZLHdCQUF3Qjs7O0lBQ25DLDRDQUE0Qjs7SUFFNUIsNENBQTRCOzs7OztJQUU1Qiw0Q0FBaUM7Ozs7O0lBRXJCLDBDQUFzQjs7Ozs7SUFBRSx5Q0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTZXR0aW5nVGFiIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVDb25maWdMb2FkRW5kLCBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgTmF2aWdhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgU3ViamVjdCwgU3Vic2NyaXB0aW9uLCB0aW1lciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZmlsdGVyLCB0YWtlVW50aWwgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRTZXJ2aWNlIHtcbiAgc2V0dGluZ3M6IFNldHRpbmdUYWJbXSA9IFtdO1xuXG4gIHNlbGVjdGVkID0ge30gYXMgU2V0dGluZ1RhYjtcblxuICBwcml2YXRlIGRlc3Ryb3kkID0gbmV3IFN1YmplY3QoKTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge1xuICAgIGxldCB0aW1lb3V0OiBTdWJzY3JpcHRpb247XG4gICAgdGhpcy5yb3V0ZXIuZXZlbnRzXG4gICAgICAucGlwZShcbiAgICAgICAgZmlsdGVyKGV2ZW50ID0+IGV2ZW50IGluc3RhbmNlb2YgUm91dGVDb25maWdMb2FkRW5kKSxcbiAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShldmVudCA9PiB7XG4gICAgICAgIGlmICh0aW1lb3V0KSB7XG4gICAgICAgICAgdGltZW91dC51bnN1YnNjcmliZSgpO1xuICAgICAgICAgIHRoaXMuZGVzdHJveSQubmV4dCgpO1xuICAgICAgICB9XG4gICAgICAgIHRpbWVvdXQgPSB0aW1lcigxNTApLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgdGhpcy5zZXRTZXR0aW5ncygpO1xuICAgICAgICB9KTtcbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7XG4gICAgdGhpcy5kZXN0cm95JC5uZXh0KCk7XG4gIH1cblxuICBzZXRTZXR0aW5ncygpIHtcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGNvbnN0IHJvdXRlID0gdGhpcy5yb3V0ZXIuY29uZmlnLmZpbmQociA9PiByLnBhdGggPT09ICdzZXR0aW5nLW1hbmFnZW1lbnQnKTtcbiAgICAgIHRoaXMuc2V0dGluZ3MgPSByb3V0ZS5kYXRhLnNldHRpbmdzLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcbiAgICAgIHRoaXMuY2hlY2tTZWxlY3RlZCgpO1xuICAgIH0sIDApO1xuICB9XG5cbiAgY2hlY2tTZWxlY3RlZCgpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0gdGhpcy5zZXR0aW5ncy5maW5kKHNldHRpbmcgPT4gc2V0dGluZy51cmwgPT09IHRoaXMucm91dGVyLnVybCkgfHwgKHt9IGFzIFNldHRpbmdUYWIpO1xuXG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkLm5hbWUgJiYgdGhpcy5zZXR0aW5ncy5sZW5ndGgpIHtcbiAgICAgIHRoaXMuc2V0U2VsZWN0ZWQodGhpcy5zZXR0aW5nc1swXSk7XG4gICAgfVxuICB9XG5cbiAgc2V0U2VsZWN0ZWQoc2VsZWN0ZWQ6IFNldHRpbmdUYWIpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0gc2VsZWN0ZWQ7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgTmF2aWdhdGUoW3NlbGVjdGVkLnVybF0pKTtcbiAgfVxufVxuIl19