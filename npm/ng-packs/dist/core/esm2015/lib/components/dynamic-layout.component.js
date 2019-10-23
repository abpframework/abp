/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBMEIsTUFBTSxlQUFlLENBQUM7QUFDbEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxhQUFhLEVBQUUsTUFBTSxFQUFjLE1BQU0saUJBQWlCLENBQUM7QUFDcEYsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFJdEIsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBQ3JELE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBVXZELE1BQU0sT0FBTyxzQkFBc0I7Ozs7OztJQUtqQyxZQUFvQixNQUFjLEVBQVUsS0FBcUIsRUFBVSxLQUFZO1FBQW5FLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFnQjtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87Y0FDL0UsRUFDSixZQUFZLEVBQUUsRUFBRSxPQUFPLEVBQUUsRUFDekIsTUFBTSxFQUNQLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE1BQU0sQ0FBQztRQUVqRCxJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxJQUFJLEVBQUUsQ0FBQyxDQUFDLE1BQU0sRUFBRTtZQUMzQyxJQUFJLENBQUMsTUFBTSxHQUFHLE9BQU87aUJBQ2xCLE1BQU07Ozs7WUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUM7aUJBQ2hCLElBQUk7Ozs7WUFBQyxDQUFDLENBQU0sRUFBRSxFQUFFLENBQUMsR0FBRzs7O1lBQUMsR0FBRyxFQUFFLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUUsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFFLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEVBQUMsQ0FBQztTQUN4RztRQUVELElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLFNBQVM7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRTtZQUNoRSxJQUFJLEtBQUssWUFBWSxhQUFhLEVBQUU7c0JBQzVCLEVBQUUsUUFBUSxFQUFFLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxRQUFRLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsT0FBTzs7c0JBRXBFLE1BQU0sR0FBRyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksSUFBSSxFQUFFLENBQUMsQ0FBQyxNQUFNLElBQUksVUFBVSxDQUFDLFFBQVEsRUFBRSxNQUFNLENBQUM7Z0JBRXRGLElBQUksQ0FBQyxNQUFNLEdBQUcsT0FBTztxQkFDbEIsTUFBTTs7OztnQkFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUM7cUJBQ2hCLElBQUk7Ozs7Z0JBQUMsQ0FBQyxDQUFNLEVBQUUsRUFBRSxDQUFDLEdBQUc7OztnQkFBQyxHQUFHLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLFdBQVcsRUFBRSxDQUFDLE9BQU8sQ0FBQyxNQUFNLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxFQUFDLENBQUM7YUFDL0U7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7Ozs7SUFFRCxXQUFXLEtBQUksQ0FBQzs7O1lBdENqQixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLG9CQUFvQjtnQkFDOUIsUUFBUSxFQUFFOzs7O0dBSVQ7YUFDRjs7OztZQWpCdUMsTUFBTTtZQUFyQyxjQUFjO1lBQ04sS0FBSzs7QUFrQndCO0lBQTNDLE1BQU0sQ0FBQyxXQUFXLENBQUMsTUFBTSxDQUFDLGNBQWMsQ0FBQyxDQUFDO3NDQUFnQixVQUFVOzZEQUFzQjs7O0lBQTNGLCtDQUEyRjs7SUFFM0Ysd0NBQWtCOzs7OztJQUVOLHdDQUFzQjs7Ozs7SUFBRSx1Q0FBNkI7Ozs7O0lBQUUsdUNBQW9COzs7Ozs7O0FBNEJ6RixTQUFTLFVBQVUsQ0FBQyxRQUFzQixFQUFFLE1BQXVCOztRQUM3RCxNQUFNLHNCQUFvQjs7VUFFeEIsS0FBSyxHQUFHLE1BQU07U0FDakIsTUFBTTs7Ozs7SUFBQyxDQUFDLEdBQUcsRUFBRSxHQUFHLEVBQUUsRUFBRSxDQUFDLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLENBQUMsQ0FBQyxHQUFFLEVBQUUsQ0FBQztTQUNuRixJQUFJOzs7O0lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxLQUFLLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUM7SUFFekMsSUFBSSxLQUFLLEVBQUU7UUFDVCxJQUFJLEtBQUssQ0FBQyxNQUFNLEVBQUU7WUFDaEIsTUFBTSxHQUFHLEtBQUssQ0FBQyxNQUFNLENBQUM7U0FDdkI7UUFFRCxJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLElBQUksUUFBUSxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7O2tCQUM1RCxLQUFLLEdBQUcsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJOzs7O1lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxLQUFLLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUM7WUFFbkUsSUFBSSxLQUFLLElBQUksS0FBSyxDQUFDLE1BQU0sRUFBRTtnQkFDekIsTUFBTSxHQUFHLEtBQUssQ0FBQyxNQUFNLENBQUM7YUFDdkI7U0FDRjtLQUNGO0lBRUQsT0FBTyxNQUFNLENBQUM7QUFDaEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgSW5wdXQsIE9uRGVzdHJveSwgVHlwZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBBY3RpdmF0ZWRSb3V0ZSwgTmF2aWdhdGlvbkVuZCwgUm91dGVyLCBVcmxTZWdtZW50IH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XHJcbmltcG9ydCB7IGVMYXlvdXRUeXBlIH0gZnJvbSAnLi4vZW51bXMvY29tbW9uJztcclxuaW1wb3J0IHsgQ29uZmlnIH0gZnJvbSAnLi4vbW9kZWxzL2NvbmZpZyc7XHJcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscy9jb21tb24nO1xyXG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9jb25maWcuc3RhdGUnO1xyXG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnLi4vdXRpbHMvcnhqcy11dGlscyc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1keW5hbWljLWxheW91dCcsXHJcbiAgdGVtcGxhdGU6IGBcclxuICAgIDxuZy1jb250YWluZXIgKm5nVGVtcGxhdGVPdXRsZXQ9XCJsYXlvdXQgPyBjb21wb25lbnRPdXRsZXQgOiByb3V0ZXJPdXRsZXRcIj48L25nLWNvbnRhaW5lcj5cclxuICAgIDxuZy10ZW1wbGF0ZSAjcm91dGVyT3V0bGV0Pjxyb3V0ZXItb3V0bGV0Pjwvcm91dGVyLW91dGxldD48L25nLXRlbXBsYXRlPlxyXG4gICAgPG5nLXRlbXBsYXRlICNjb21wb25lbnRPdXRsZXQ+PG5nLWNvbnRhaW5lciAqbmdDb21wb25lbnRPdXRsZXQ9XCJsYXlvdXRcIj48L25nLWNvbnRhaW5lcj48L25nLXRlbXBsYXRlPlxyXG4gIGBcclxufSlcclxuZXhwb3J0IGNsYXNzIER5bmFtaWNMYXlvdXRDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xyXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0T25lKCdyZXF1aXJlbWVudHMnKSkgcmVxdWlyZW1lbnRzJDogT2JzZXJ2YWJsZTxDb25maWcuUmVxdWlyZW1lbnRzPjtcclxuXHJcbiAgbGF5b3V0OiBUeXBlPGFueT47XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgcm91dGU6IEFjdGl2YXRlZFJvdXRlLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge1xyXG4gICAgY29uc3Qge1xyXG4gICAgICByZXF1aXJlbWVudHM6IHsgbGF5b3V0cyB9LFxyXG4gICAgICByb3V0ZXNcclxuICAgIH0gPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFsbCk7XHJcblxyXG4gICAgaWYgKCh0aGlzLnJvdXRlLnNuYXBzaG90LmRhdGEgfHwge30pLmxheW91dCkge1xyXG4gICAgICB0aGlzLmxheW91dCA9IGxheW91dHNcclxuICAgICAgICAuZmlsdGVyKGwgPT4gISFsKVxyXG4gICAgICAgIC5maW5kKChsOiBhbnkpID0+IHNucSgoKSA9PiBsLnR5cGUudG9Mb3dlckNhc2UoKS5pbmRleE9mKHRoaXMucm91dGUuc25hcHNob3QuZGF0YS5sYXlvdXQpLCAtMSkgPiAtMSk7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5yb3V0ZXIuZXZlbnRzLnBpcGUodGFrZVVudGlsRGVzdHJveSh0aGlzKSkuc3Vic2NyaWJlKGV2ZW50ID0+IHtcclxuICAgICAgaWYgKGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvbkVuZCkge1xyXG4gICAgICAgIGNvbnN0IHsgc2VnbWVudHMgfSA9IHRoaXMucm91dGVyLnBhcnNlVXJsKGV2ZW50LnVybCkucm9vdC5jaGlsZHJlbi5wcmltYXJ5O1xyXG5cclxuICAgICAgICBjb25zdCBsYXlvdXQgPSAodGhpcy5yb3V0ZS5zbmFwc2hvdC5kYXRhIHx8IHt9KS5sYXlvdXQgfHwgZmluZExheW91dChzZWdtZW50cywgcm91dGVzKTtcclxuXHJcbiAgICAgICAgdGhpcy5sYXlvdXQgPSBsYXlvdXRzXHJcbiAgICAgICAgICAuZmlsdGVyKGwgPT4gISFsKVxyXG4gICAgICAgICAgLmZpbmQoKGw6IGFueSkgPT4gc25xKCgpID0+IGwudHlwZS50b0xvd2VyQ2FzZSgpLmluZGV4T2YobGF5b3V0KSwgLTEpID4gLTEpO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIG5nT25EZXN0cm95KCkge31cclxufVxyXG5cclxuZnVuY3Rpb24gZmluZExheW91dChzZWdtZW50czogVXJsU2VnbWVudFtdLCByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSk6IGVMYXlvdXRUeXBlIHtcclxuICBsZXQgbGF5b3V0ID0gZUxheW91dFR5cGUuZW1wdHk7XHJcblxyXG4gIGNvbnN0IHJvdXRlID0gcm91dGVzXHJcbiAgICAucmVkdWNlKChhY2MsIHZhbCkgPT4gKHZhbC53cmFwcGVyID8gWy4uLmFjYywgLi4udmFsLmNoaWxkcmVuXSA6IFsuLi5hY2MsIHZhbF0pLCBbXSlcclxuICAgIC5maW5kKHIgPT4gci5wYXRoID09PSBzZWdtZW50c1swXS5wYXRoKTtcclxuXHJcbiAgaWYgKHJvdXRlKSB7XHJcbiAgICBpZiAocm91dGUubGF5b3V0KSB7XHJcbiAgICAgIGxheW91dCA9IHJvdXRlLmxheW91dDtcclxuICAgIH1cclxuXHJcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoICYmIHNlZ21lbnRzLmxlbmd0aCA+IDEpIHtcclxuICAgICAgY29uc3QgY2hpbGQgPSByb3V0ZS5jaGlsZHJlbi5maW5kKGMgPT4gYy5wYXRoID09PSBzZWdtZW50c1sxXS5wYXRoKTtcclxuXHJcbiAgICAgIGlmIChjaGlsZCAmJiBjaGlsZC5sYXlvdXQpIHtcclxuICAgICAgICBsYXlvdXQgPSBjaGlsZC5sYXlvdXQ7XHJcbiAgICAgIH1cclxuICAgIH1cclxuICB9XHJcblxyXG4gIHJldHVybiBsYXlvdXQ7XHJcbn1cclxuIl19