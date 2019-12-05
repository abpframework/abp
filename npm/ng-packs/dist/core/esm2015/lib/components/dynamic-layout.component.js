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
export class DynamicLayoutComponent {
    /**
     * @param {?} router
     * @param {?} route
     * @param {?} store
     */
    constructor(router, route, store) {
        this.router = router;
        this.route = route;
        this.store = store;
        const { requirements: { layouts }, routes } = this.store.selectSnapshot(ConfigState.getAll);
        if ((this.route.snapshot.data || {}).layout) {
            this.layout = layouts
                .filter((/**
             * @param {?} l
             * @return {?}
             */
            l => !!l))
                .find((/**
             * @param {?} l
             * @return {?}
             */
            (l) => snq((/**
             * @return {?}
             */
            () => l.type.toLowerCase().indexOf(this.route.snapshot.data.layout)), -1) > -1));
        }
        this.router.events.pipe(takeUntilDestroy(this)).subscribe((/**
         * @param {?} event
         * @return {?}
         */
        event => {
            if (event instanceof NavigationEnd) {
                const { segments } = this.router.parseUrl(event.url).root.children.primary;
                /** @type {?} */
                const layout = (this.route.snapshot.data || {}).layout || findLayout(segments, routes);
                this.layout = layouts
                    .filter((/**
                 * @param {?} l
                 * @return {?}
                 */
                l => !!l))
                    .find((/**
                 * @param {?} l
                 * @return {?}
                 */
                (l) => snq((/**
                 * @return {?}
                 */
                () => l.type.toLowerCase().indexOf(layout)), -1) > -1));
            }
        }));
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
}
DynamicLayoutComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-dynamic-layout',
                template: `
    <ng-container *ngTemplateOutlet="layout ? componentOutlet : routerOutlet"></ng-container>
    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>
    <ng-template #componentOutlet><ng-container *ngComponentOutlet="layout"></ng-container></ng-template>
  `
            }] }
];
/** @nocollapse */
DynamicLayoutComponent.ctorParameters = () => [
    { type: Router },
    { type: ActivatedRoute },
    { type: Store }
];
tslib_1.__decorate([
    Select(ConfigState.getOne('requirements')),
    tslib_1.__metadata("design:type", Observable)
], DynamicLayoutComponent.prototype, "requirements$", void 0);
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
    let layout = "empty" /* empty */;
    /** @type {?} */
    const route = routes
        .reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    (acc, val) => (val.wrapper ? [...acc, ...val.children] : [...acc, val])), [])
        .find((/**
     * @param {?} r
     * @return {?}
     */
    r => r.path === segments[0].path));
    if (route) {
        if (route.layout) {
            layout = route.layout;
        }
        if (route.children && route.children.length && segments.length > 1) {
            /** @type {?} */
            const child = route.children.find((/**
             * @param {?} c
             * @return {?}
             */
            c => c.path === segments[1].path));
            if (child && child.layout) {
                layout = child.layout;
            }
        }
    }
    return layout;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQTBCLE1BQU0sZUFBZSxDQUFDO0FBQ2xFLE9BQU8sRUFBRSxjQUFjLEVBQUUsYUFBYSxFQUFFLE1BQU0sRUFBYyxNQUFNLGlCQUFpQixDQUFDO0FBQ3BGLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBSXRCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUNyRCxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQVV2RCxNQUFNLE9BQU8sc0JBQXNCOzs7Ozs7SUFLakMsWUFBb0IsTUFBYyxFQUFVLEtBQXFCLEVBQVUsS0FBWTtRQUFuRSxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBZ0I7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO2NBQy9FLEVBQ0osWUFBWSxFQUFFLEVBQUUsT0FBTyxFQUFFLEVBQ3pCLE1BQU0sRUFDUCxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUM7UUFFakQsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksSUFBSSxFQUFFLENBQUMsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsSUFBSSxDQUFDLE1BQU0sR0FBRyxPQUFPO2lCQUNsQixNQUFNOzs7O1lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsQ0FB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QUFFLENBQUMsQ0FBQyxDQUFDLElBQUksS0FBSyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDO1lBRW5FLElBQUksS0FBSyxJQUFJLEtBQUssQ0FBQyxNQUFNLEVBQUU7Z0JBQ3pCLE1BQU0sR0FBRyxLQUFLLENBQUMsTUFBTSxDQUFDO2FBQ3ZCO1NBQ0Y7S0FDRjtJQUVELE9BQU8sTUFBTSxDQUFDO0FBQ2hCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIElucHV0LCBPbkRlc3Ryb3ksIFR5cGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjdGl2YXRlZFJvdXRlLCBOYXZpZ2F0aW9uRW5kLCBSb3V0ZXIsIFVybFNlZ21lbnQgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IGVMYXlvdXRUeXBlIH0gZnJvbSAnLi4vZW51bXMvY29tbW9uJztcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscy9jb25maWcnO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi4vbW9kZWxzL2NvbW1vbic7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9jb25maWcuc3RhdGUnO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzL3J4anMtdXRpbHMnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtZHluYW1pYy1sYXlvdXQnLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxuZy1jb250YWluZXIgKm5nVGVtcGxhdGVPdXRsZXQ9XCJsYXlvdXQgPyBjb21wb25lbnRPdXRsZXQgOiByb3V0ZXJPdXRsZXRcIj48L25nLWNvbnRhaW5lcj5cbiAgICA8bmctdGVtcGxhdGUgI3JvdXRlck91dGxldD48cm91dGVyLW91dGxldD48L3JvdXRlci1vdXRsZXQ+PC9uZy10ZW1wbGF0ZT5cbiAgICA8bmctdGVtcGxhdGUgI2NvbXBvbmVudE91dGxldD48bmctY29udGFpbmVyICpuZ0NvbXBvbmVudE91dGxldD1cImxheW91dFwiPjwvbmctY29udGFpbmVyPjwvbmctdGVtcGxhdGU+XG4gIGBcbn0pXG5leHBvcnQgY2xhc3MgRHluYW1pY0xheW91dENvbXBvbmVudCBpbXBsZW1lbnRzIE9uRGVzdHJveSB7XG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0T25lKCdyZXF1aXJlbWVudHMnKSkgcmVxdWlyZW1lbnRzJDogT2JzZXJ2YWJsZTxDb25maWcuUmVxdWlyZW1lbnRzPjtcblxuICBsYXlvdXQ6IFR5cGU8YW55PjtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHJvdXRlOiBBY3RpdmF0ZWRSb3V0ZSwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHtcbiAgICBjb25zdCB7XG4gICAgICByZXF1aXJlbWVudHM6IHsgbGF5b3V0cyB9LFxuICAgICAgcm91dGVzXG4gICAgfSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QWxsKTtcblxuICAgIGlmICgodGhpcy5yb3V0ZS5zbmFwc2hvdC5kYXRhIHx8IHt9KS5sYXlvdXQpIHtcbiAgICAgIHRoaXMubGF5b3V0ID0gbGF5b3V0c1xuICAgICAgICAuZmlsdGVyKGwgPT4gISFsKVxuICAgICAgICAuZmluZCgobDogYW55KSA9PiBzbnEoKCkgPT4gbC50eXBlLnRvTG93ZXJDYXNlKCkuaW5kZXhPZih0aGlzLnJvdXRlLnNuYXBzaG90LmRhdGEubGF5b3V0KSwgLTEpID4gLTEpO1xuICAgIH1cblxuICAgIHRoaXMucm91dGVyLmV2ZW50cy5waXBlKHRha2VVbnRpbERlc3Ryb3kodGhpcykpLnN1YnNjcmliZShldmVudCA9PiB7XG4gICAgICBpZiAoZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRW5kKSB7XG4gICAgICAgIGNvbnN0IHsgc2VnbWVudHMgfSA9IHRoaXMucm91dGVyLnBhcnNlVXJsKGV2ZW50LnVybCkucm9vdC5jaGlsZHJlbi5wcmltYXJ5O1xuXG4gICAgICAgIGNvbnN0IGxheW91dCA9ICh0aGlzLnJvdXRlLnNuYXBzaG90LmRhdGEgfHwge30pLmxheW91dCB8fCBmaW5kTGF5b3V0KHNlZ21lbnRzLCByb3V0ZXMpO1xuXG4gICAgICAgIHRoaXMubGF5b3V0ID0gbGF5b3V0c1xuICAgICAgICAgIC5maWx0ZXIobCA9PiAhIWwpXG4gICAgICAgICAgLmZpbmQoKGw6IGFueSkgPT4gc25xKCgpID0+IGwudHlwZS50b0xvd2VyQ2FzZSgpLmluZGV4T2YobGF5b3V0KSwgLTEpID4gLTEpO1xuICAgICAgfVxuICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7fVxufVxuXG5mdW5jdGlvbiBmaW5kTGF5b3V0KHNlZ21lbnRzOiBVcmxTZWdtZW50W10sIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKTogZUxheW91dFR5cGUge1xuICBsZXQgbGF5b3V0ID0gZUxheW91dFR5cGUuZW1wdHk7XG5cbiAgY29uc3Qgcm91dGUgPSByb3V0ZXNcbiAgICAucmVkdWNlKChhY2MsIHZhbCkgPT4gKHZhbC53cmFwcGVyID8gWy4uLmFjYywgLi4udmFsLmNoaWxkcmVuXSA6IFsuLi5hY2MsIHZhbF0pLCBbXSlcbiAgICAuZmluZChyID0+IHIucGF0aCA9PT0gc2VnbWVudHNbMF0ucGF0aCk7XG5cbiAgaWYgKHJvdXRlKSB7XG4gICAgaWYgKHJvdXRlLmxheW91dCkge1xuICAgICAgbGF5b3V0ID0gcm91dGUubGF5b3V0O1xuICAgIH1cblxuICAgIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYgc2VnbWVudHMubGVuZ3RoID4gMSkge1xuICAgICAgY29uc3QgY2hpbGQgPSByb3V0ZS5jaGlsZHJlbi5maW5kKGMgPT4gYy5wYXRoID09PSBzZWdtZW50c1sxXS5wYXRoKTtcblxuICAgICAgaWYgKGNoaWxkICYmIGNoaWxkLmxheW91dCkge1xuICAgICAgICBsYXlvdXQgPSBjaGlsZC5sYXlvdXQ7XG4gICAgICB9XG4gICAgfVxuICB9XG5cbiAgcmV0dXJuIGxheW91dDtcbn1cbiJdfQ==