/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/dynamic-layout.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import snq from 'snq';
import { ConfigState } from '../states/config.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';
var DynamicLayoutComponent = /** @class */ (function () {
    function DynamicLayoutComponent(router, route, store) {
        var _this = this;
        this.router = router;
        this.route = route;
        this.store = store;
        var _a = this.store.selectSnapshot(ConfigState.getAll), layouts = _a.requirements.layouts, routes = _a.routes;
        if ((this.route.snapshot.data || {}).layout) {
            this.layout = layouts
                .filter((/**
             * @param {?} l
             * @return {?}
             */
            function (l) { return !!l; }))
                .find((/**
             * @param {?} l
             * @return {?}
             */
            function (l) { return snq((/**
             * @return {?}
             */
            function () { return l.type.toLowerCase().indexOf(_this.route.snapshot.data.layout); }), -1) > -1; }));
        }
        this.router.events.pipe(takeUntilDestroy(this)).subscribe((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            if (event instanceof NavigationEnd) {
                var segments = _this.router.parseUrl(event.url).root.children.primary.segments;
                /** @type {?} */
                var layout_1 = (_this.route.snapshot.data || {}).layout || findLayout(segments, routes);
                _this.layout = layouts
                    .filter((/**
                 * @param {?} l
                 * @return {?}
                 */
                function (l) { return !!l; }))
                    .find((/**
                 * @param {?} l
                 * @return {?}
                 */
                function (l) { return snq((/**
                 * @return {?}
                 */
                function () { return l.type.toLowerCase().indexOf(layout_1); }), -1) > -1; }));
            }
        }));
    }
    /**
     * @return {?}
     */
    DynamicLayoutComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    DynamicLayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-dynamic-layout',
                    template: "\n    <ng-container *ngTemplateOutlet=\"layout ? componentOutlet : routerOutlet\"></ng-container>\n    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>\n    <ng-template #componentOutlet><ng-container *ngComponentOutlet=\"layout\"></ng-container></ng-template>\n  "
                }] }
    ];
    /** @nocollapse */
    DynamicLayoutComponent.ctorParameters = function () { return [
        { type: Router },
        { type: ActivatedRoute },
        { type: Store }
    ]; };
    tslib_1.__decorate([
        Select(ConfigState.getOne('requirements')),
        tslib_1.__metadata("design:type", Observable)
    ], DynamicLayoutComponent.prototype, "requirements$", void 0);
    return DynamicLayoutComponent;
}());
export { DynamicLayoutComponent };
if (false) {
    /** @type {?} */
    DynamicLayoutComponent.prototype.requirements$;
    /** @type {?} */
    DynamicLayoutComponent.prototype.layout;
    /**
     * @type {?}
     * @private
     */
    DynamicLayoutComponent.prototype.router;
    /**
     * @type {?}
     * @private
     */
    DynamicLayoutComponent.prototype.route;
    /**
     * @type {?}
     * @private
     */
    DynamicLayoutComponent.prototype.store;
}
/**
 * @param {?} segments
 * @param {?} routes
 * @return {?}
 */
function findLayout(segments, routes) {
    /** @type {?} */
    var layout = "empty" /* empty */;
    /** @type {?} */
    var route = routes
        .reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    function (acc, val) { return (val.wrapper ? tslib_1.__spread(acc, val.children) : tslib_1.__spread(acc, [val])); }), [])
        .find((/**
     * @param {?} r
     * @return {?}
     */
    function (r) { return r.path === segments[0].path; }));
    if (route) {
        if (route.layout) {
            layout = route.layout;
        }
        if (route.children && route.children.length && segments.length > 1) {
            /** @type {?} */
            var child = route.children.find((/**
             * @param {?} c
             * @return {?}
             */
            function (c) { return c.path === segments[1].path; }));
            if (child && child.layout) {
                layout = child.layout;
            }
        }
    }
    return layout;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQTBCLE1BQU0sZUFBZSxDQUFDO0FBQ2xFLE9BQU8sRUFBRSxjQUFjLEVBQUUsYUFBYSxFQUFFLE1BQU0sRUFBYyxNQUFNLGlCQUFpQixDQUFDO0FBQ3BGLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBSXRCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUNyRCxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUV2RDtJQWFFLGdDQUFvQixNQUFjLEVBQVUsS0FBcUIsRUFBVSxLQUFZO1FBQXZGLGlCQXVCQztRQXZCbUIsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQWdCO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUMvRSxJQUFBLGtEQUcyQyxFQUYvQixpQ0FBTyxFQUN2QixrQkFDK0M7UUFFakQsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksSUFBSSxFQUFFLENBQUMsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsSUFBSSxDQUFDLE1BQU0sR0FBRyxPQUFPO2lCQUNsQixNQUFNOzs7O1lBQUMsVUFBQSxDQUFDLElBQUksT0FBQSxDQUFDLENBQUMsQ0FBQyxFQUFILENBQUcsRUFBQztpQkFDaEIsSUFBSTs7OztZQUFDLFVBQUMsQ0FBTSxJQUFLLE9BQUEsR0FBRzs7O1lBQUMsY0FB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O1lBRW5FLElBQUksS0FBSyxJQUFJLEtBQUssQ0FBQyxNQUFNLEVBQUU7Z0JBQ3pCLE1BQU0sR0FBRyxLQUFLLENBQUMsTUFBTSxDQUFDO2FBQ3ZCO1NBQ0Y7S0FDRjtJQUVELE9BQU8sTUFBTSxDQUFDO0FBQ2hCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIElucHV0LCBPbkRlc3Ryb3ksIFR5cGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQWN0aXZhdGVkUm91dGUsIE5hdmlnYXRpb25FbmQsIFJvdXRlciwgVXJsU2VnbWVudCB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5pbXBvcnQgeyBlTGF5b3V0VHlwZSB9IGZyb20gJy4uL2VudW1zL2NvbW1vbic7XHJcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscy9jb25maWcnO1xyXG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMvY29tbW9uJztcclxuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvY29uZmlnLnN0YXRlJztcclxuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzL3J4anMtdXRpbHMnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtZHluYW1pYy1sYXlvdXQnLFxyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8bmctY29udGFpbmVyICpuZ1RlbXBsYXRlT3V0bGV0PVwibGF5b3V0ID8gY29tcG9uZW50T3V0bGV0IDogcm91dGVyT3V0bGV0XCI+PC9uZy1jb250YWluZXI+XHJcbiAgICA8bmctdGVtcGxhdGUgI3JvdXRlck91dGxldD48cm91dGVyLW91dGxldD48L3JvdXRlci1vdXRsZXQ+PC9uZy10ZW1wbGF0ZT5cclxuICAgIDxuZy10ZW1wbGF0ZSAjY29tcG9uZW50T3V0bGV0PjxuZy1jb250YWluZXIgKm5nQ29tcG9uZW50T3V0bGV0PVwibGF5b3V0XCI+PC9uZy1jb250YWluZXI+PC9uZy10ZW1wbGF0ZT5cclxuICBgXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBEeW5hbWljTGF5b3V0Q29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcclxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncmVxdWlyZW1lbnRzJykpIHJlcXVpcmVtZW50cyQ6IE9ic2VydmFibGU8Q29uZmlnLlJlcXVpcmVtZW50cz47XHJcblxyXG4gIGxheW91dDogVHlwZTxhbnk+O1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHJvdXRlOiBBY3RpdmF0ZWRSb3V0ZSwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHtcclxuICAgIGNvbnN0IHtcclxuICAgICAgcmVxdWlyZW1lbnRzOiB7IGxheW91dHMgfSxcclxuICAgICAgcm91dGVzXHJcbiAgICB9ID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBbGwpO1xyXG5cclxuICAgIGlmICgodGhpcy5yb3V0ZS5zbmFwc2hvdC5kYXRhIHx8IHt9KS5sYXlvdXQpIHtcclxuICAgICAgdGhpcy5sYXlvdXQgPSBsYXlvdXRzXHJcbiAgICAgICAgLmZpbHRlcihsID0+ICEhbClcclxuICAgICAgICAuZmluZCgobDogYW55KSA9PiBzbnEoKCkgPT4gbC50eXBlLnRvTG93ZXJDYXNlKCkuaW5kZXhPZih0aGlzLnJvdXRlLnNuYXBzaG90LmRhdGEubGF5b3V0KSwgLTEpID4gLTEpO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMucm91dGVyLmV2ZW50cy5waXBlKHRha2VVbnRpbERlc3Ryb3kodGhpcykpLnN1YnNjcmliZShldmVudCA9PiB7XHJcbiAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQpIHtcclxuICAgICAgICBjb25zdCB7IHNlZ21lbnRzIH0gPSB0aGlzLnJvdXRlci5wYXJzZVVybChldmVudC51cmwpLnJvb3QuY2hpbGRyZW4ucHJpbWFyeTtcclxuXHJcbiAgICAgICAgY29uc3QgbGF5b3V0ID0gKHRoaXMucm91dGUuc25hcHNob3QuZGF0YSB8fCB7fSkubGF5b3V0IHx8IGZpbmRMYXlvdXQoc2VnbWVudHMsIHJvdXRlcyk7XHJcblxyXG4gICAgICAgIHRoaXMubGF5b3V0ID0gbGF5b3V0c1xyXG4gICAgICAgICAgLmZpbHRlcihsID0+ICEhbClcclxuICAgICAgICAgIC5maW5kKChsOiBhbnkpID0+IHNucSgoKSA9PiBsLnR5cGUudG9Mb3dlckNhc2UoKS5pbmRleE9mKGxheW91dCksIC0xKSA+IC0xKTtcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBuZ09uRGVzdHJveSgpIHt9XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGZpbmRMYXlvdXQoc2VnbWVudHM6IFVybFNlZ21lbnRbXSwgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10pOiBlTGF5b3V0VHlwZSB7XHJcbiAgbGV0IGxheW91dCA9IGVMYXlvdXRUeXBlLmVtcHR5O1xyXG5cclxuICBjb25zdCByb3V0ZSA9IHJvdXRlc1xyXG4gICAgLnJlZHVjZSgoYWNjLCB2YWwpID0+ICh2YWwud3JhcHBlciA/IFsuLi5hY2MsIC4uLnZhbC5jaGlsZHJlbl0gOiBbLi4uYWNjLCB2YWxdKSwgW10pXHJcbiAgICAuZmluZChyID0+IHIucGF0aCA9PT0gc2VnbWVudHNbMF0ucGF0aCk7XHJcblxyXG4gIGlmIChyb3V0ZSkge1xyXG4gICAgaWYgKHJvdXRlLmxheW91dCkge1xyXG4gICAgICBsYXlvdXQgPSByb3V0ZS5sYXlvdXQ7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiBzZWdtZW50cy5sZW5ndGggPiAxKSB7XHJcbiAgICAgIGNvbnN0IGNoaWxkID0gcm91dGUuY2hpbGRyZW4uZmluZChjID0+IGMucGF0aCA9PT0gc2VnbWVudHNbMV0ucGF0aCk7XHJcblxyXG4gICAgICBpZiAoY2hpbGQgJiYgY2hpbGQubGF5b3V0KSB7XHJcbiAgICAgICAgbGF5b3V0ID0gY2hpbGQubGF5b3V0O1xyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICByZXR1cm4gbGF5b3V0O1xyXG59XHJcbiJdfQ==