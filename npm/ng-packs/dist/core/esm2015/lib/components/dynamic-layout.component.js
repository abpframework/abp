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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQTBCLE1BQU0sZUFBZSxDQUFDO0FBQ2xFLE9BQU8sRUFBRSxjQUFjLEVBQUUsYUFBYSxFQUFFLE1BQU0sRUFBYyxNQUFNLGlCQUFpQixDQUFDO0FBQ3BGLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBSXRCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUNyRCxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQVV2RCxNQUFNLE9BQU8sc0JBQXNCOzs7Ozs7SUFLakMsWUFBb0IsTUFBYyxFQUFVLEtBQXFCLEVBQVUsS0FBWTtRQUFuRSxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBZ0I7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO2NBQy9FLEVBQ0osWUFBWSxFQUFFLEVBQUUsT0FBTyxFQUFFLEVBQ3pCLE1BQU0sRUFDUCxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUM7UUFFakQsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksSUFBSSxFQUFFLENBQUMsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsSUFBSSxDQUFDLE1BQU0sR0FBRyxPQUFPO2lCQUNsQixNQUFNOzs7O1lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsQ0FB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QUFFLENBQUMsQ0FBQyxDQUFDLElBQUksS0FBSyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDO1lBRW5FLElBQUksS0FBSyxJQUFJLEtBQUssQ0FBQyxNQUFNLEVBQUU7Z0JBQ3pCLE1BQU0sR0FBRyxLQUFLLENBQUMsTUFBTSxDQUFDO2FBQ3ZCO1NBQ0Y7S0FDRjtJQUVELE9BQU8sTUFBTSxDQUFDO0FBQ2hCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIElucHV0LCBPbkRlc3Ryb3ksIFR5cGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQWN0aXZhdGVkUm91dGUsIE5hdmlnYXRpb25FbmQsIFJvdXRlciwgVXJsU2VnbWVudCB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5pbXBvcnQgeyBlTGF5b3V0VHlwZSB9IGZyb20gJy4uL2VudW1zL2NvbW1vbic7XHJcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscy9jb25maWcnO1xyXG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMvY29tbW9uJztcclxuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvY29uZmlnLnN0YXRlJztcclxuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzL3J4anMtdXRpbHMnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtZHluYW1pYy1sYXlvdXQnLFxyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8bmctY29udGFpbmVyICpuZ1RlbXBsYXRlT3V0bGV0PVwibGF5b3V0ID8gY29tcG9uZW50T3V0bGV0IDogcm91dGVyT3V0bGV0XCI+PC9uZy1jb250YWluZXI+XHJcbiAgICA8bmctdGVtcGxhdGUgI3JvdXRlck91dGxldD48cm91dGVyLW91dGxldD48L3JvdXRlci1vdXRsZXQ+PC9uZy10ZW1wbGF0ZT5cclxuICAgIDxuZy10ZW1wbGF0ZSAjY29tcG9uZW50T3V0bGV0PjxuZy1jb250YWluZXIgKm5nQ29tcG9uZW50T3V0bGV0PVwibGF5b3V0XCI+PC9uZy1jb250YWluZXI+PC9uZy10ZW1wbGF0ZT5cclxuICBgXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBEeW5hbWljTGF5b3V0Q29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcclxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncmVxdWlyZW1lbnRzJykpIHJlcXVpcmVtZW50cyQ6IE9ic2VydmFibGU8Q29uZmlnLlJlcXVpcmVtZW50cz47XHJcblxyXG4gIGxheW91dDogVHlwZTxhbnk+O1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHJvdXRlOiBBY3RpdmF0ZWRSb3V0ZSwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHtcclxuICAgIGNvbnN0IHtcclxuICAgICAgcmVxdWlyZW1lbnRzOiB7IGxheW91dHMgfSxcclxuICAgICAgcm91dGVzXHJcbiAgICB9ID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBbGwpO1xyXG5cclxuICAgIGlmICgodGhpcy5yb3V0ZS5zbmFwc2hvdC5kYXRhIHx8IHt9KS5sYXlvdXQpIHtcclxuICAgICAgdGhpcy5sYXlvdXQgPSBsYXlvdXRzXHJcbiAgICAgICAgLmZpbHRlcihsID0+ICEhbClcclxuICAgICAgICAuZmluZCgobDogYW55KSA9PiBzbnEoKCkgPT4gbC50eXBlLnRvTG93ZXJDYXNlKCkuaW5kZXhPZih0aGlzLnJvdXRlLnNuYXBzaG90LmRhdGEubGF5b3V0KSwgLTEpID4gLTEpO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMucm91dGVyLmV2ZW50cy5waXBlKHRha2VVbnRpbERlc3Ryb3kodGhpcykpLnN1YnNjcmliZShldmVudCA9PiB7XHJcbiAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQpIHtcclxuICAgICAgICBjb25zdCB7IHNlZ21lbnRzIH0gPSB0aGlzLnJvdXRlci5wYXJzZVVybChldmVudC51cmwpLnJvb3QuY2hpbGRyZW4ucHJpbWFyeTtcclxuXHJcbiAgICAgICAgY29uc3QgbGF5b3V0ID0gKHRoaXMucm91dGUuc25hcHNob3QuZGF0YSB8fCB7fSkubGF5b3V0IHx8IGZpbmRMYXlvdXQoc2VnbWVudHMsIHJvdXRlcyk7XHJcblxyXG4gICAgICAgIHRoaXMubGF5b3V0ID0gbGF5b3V0c1xyXG4gICAgICAgICAgLmZpbHRlcihsID0+ICEhbClcclxuICAgICAgICAgIC5maW5kKChsOiBhbnkpID0+IHNucSgoKSA9PiBsLnR5cGUudG9Mb3dlckNhc2UoKS5pbmRleE9mKGxheW91dCksIC0xKSA+IC0xKTtcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBuZ09uRGVzdHJveSgpIHt9XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGZpbmRMYXlvdXQoc2VnbWVudHM6IFVybFNlZ21lbnRbXSwgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10pOiBlTGF5b3V0VHlwZSB7XHJcbiAgbGV0IGxheW91dCA9IGVMYXlvdXRUeXBlLmVtcHR5O1xyXG5cclxuICBjb25zdCByb3V0ZSA9IHJvdXRlc1xyXG4gICAgLnJlZHVjZSgoYWNjLCB2YWwpID0+ICh2YWwud3JhcHBlciA/IFsuLi5hY2MsIC4uLnZhbC5jaGlsZHJlbl0gOiBbLi4uYWNjLCB2YWxdKSwgW10pXHJcbiAgICAuZmluZChyID0+IHIucGF0aCA9PT0gc2VnbWVudHNbMF0ucGF0aCk7XHJcblxyXG4gIGlmIChyb3V0ZSkge1xyXG4gICAgaWYgKHJvdXRlLmxheW91dCkge1xyXG4gICAgICBsYXlvdXQgPSByb3V0ZS5sYXlvdXQ7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiBzZWdtZW50cy5sZW5ndGggPiAxKSB7XHJcbiAgICAgIGNvbnN0IGNoaWxkID0gcm91dGUuY2hpbGRyZW4uZmluZChjID0+IGMucGF0aCA9PT0gc2VnbWVudHNbMV0ucGF0aCk7XHJcblxyXG4gICAgICBpZiAoY2hpbGQgJiYgY2hpbGQubGF5b3V0KSB7XHJcbiAgICAgICAgbGF5b3V0ID0gY2hpbGQubGF5b3V0O1xyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICByZXR1cm4gbGF5b3V0O1xyXG59XHJcbiJdfQ==