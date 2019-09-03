/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import snq from 'snq';
export class DynamicLayoutComponent {
    /**
     * @param {?} router
     * @param {?} store
     */
    constructor(router, store) {
        this.router = router;
        this.store = store;
        this.router.events.pipe(takeUntilDestroy(this)).subscribe((/**
         * @param {?} event
         * @return {?}
         */
        event => {
            if (event instanceof NavigationEnd) {
                const { segments } = this.router.parseUrl(event.url).root.children.primary;
                const { requirements: { layouts }, routes, } = this.store.selectSnapshot(ConfigState.getAll);
                /** @type {?} */
                const layout = findLayout(segments, routes);
                this.layout = layouts.filter((/**
                 * @param {?} l
                 * @return {?}
                 */
                l => !!l)).find((/**
                 * @param {?} l
                 * @return {?}
                 */
                l => snq((/**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBbUIsTUFBTSxlQUFlLENBQUM7QUFDM0QsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLEVBQWMsTUFBTSxpQkFBaUIsQ0FBQztBQUNwRSxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBR2xDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFDeEMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBQzVDLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQVd0QixNQUFNLE9BQU8sc0JBQXNCOzs7OztJQU1qQyxZQUFvQixNQUFjLEVBQVUsS0FBWTtRQUFwQyxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUN0RCxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxTQUFTOzs7O1FBQUMsS0FBSyxDQUFDLEVBQUU7WUFDaEUsSUFBSSxLQUFLLFlBQVksYUFBYSxFQUFFO3NCQUM1QixFQUFFLFFBQVEsRUFBRSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsUUFBUSxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLE9BQU87c0JBQ3BFLEVBQ0osWUFBWSxFQUFFLEVBQUUsT0FBTyxFQUFFLEVBQ3pCLE1BQU0sR0FDUCxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUM7O3NCQUUzQyxNQUFNLEdBQUcsVUFBVSxDQUFDLFFBQVEsRUFBRSxNQUFNLENBQUM7Z0JBRTNDLElBQUksQ0FBQyxNQUFNLEdBQUcsT0FBTyxDQUFDLE1BQU07Ozs7Z0JBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsSUFBSTs7OztnQkFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEdBQUc7OztnQkFBQyxHQUFHLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLFdBQVcsRUFBRSxDQUFDLE9BQU8sQ0FBQyxNQUFNLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxFQUFDLENBQUM7YUFDNUc7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7Ozs7SUFFRCxXQUFXLEtBQUksQ0FBQzs7O1lBL0JqQixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLG9CQUFvQjtnQkFDOUIsUUFBUSxFQUFFOzs7OztHQUtUO2FBQ0Y7Ozs7WUFqQnVCLE1BQU07WUFDYixLQUFLOztBQW1CcEI7SUFEQyxNQUFNLENBQUMsV0FBVyxDQUFDLE1BQU0sQ0FBQyxjQUFjLENBQUMsQ0FBQztzQ0FDNUIsVUFBVTs2REFBc0I7OztJQUQvQywrQ0FDK0M7O0lBRS9DLHdDQUFrQjs7Ozs7SUFFTix3Q0FBc0I7Ozs7O0lBQUUsdUNBQW9COzs7Ozs7O0FBbUIxRCxTQUFTLFVBQVUsQ0FBQyxRQUFzQixFQUFFLE1BQXVCOztRQUM3RCxNQUFNLHNCQUFvQjs7VUFFeEIsS0FBSyxHQUFHLE1BQU07U0FDakIsTUFBTTs7Ozs7SUFBQyxDQUFDLEdBQUcsRUFBRSxHQUFHLEVBQUUsRUFBRSxDQUFDLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLENBQUMsQ0FBQyxHQUFFLEVBQUUsQ0FBQztTQUNuRixJQUFJOzs7O0lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxLQUFLLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUM7SUFFekMsSUFBSSxLQUFLLEVBQUU7UUFDVCxJQUFJLEtBQUssQ0FBQyxNQUFNLEVBQUU7WUFDaEIsTUFBTSxHQUFHLEtBQUssQ0FBQyxNQUFNLENBQUM7U0FDdkI7UUFFRCxJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLElBQUksUUFBUSxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7O2tCQUM1RCxLQUFLLEdBQUcsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJOzs7O1lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxLQUFLLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUM7WUFFbkUsSUFBSSxLQUFLLElBQUksS0FBSyxDQUFDLE1BQU0sRUFBRTtnQkFDekIsTUFBTSxHQUFHLEtBQUssQ0FBQyxNQUFNLENBQUM7YUFDdkI7U0FDRjtLQUNGO0lBRUQsT0FBTyxNQUFNLENBQUM7QUFDaEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25EZXN0cm95LCBUeXBlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOYXZpZ2F0aW9uRW5kLCBSb3V0ZXIsIFVybFNlZ21lbnQgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGVMYXlvdXRUeXBlIH0gZnJvbSAnLi4vZW51bXMnO1xuaW1wb3J0IHsgQUJQLCBDb25maWcgfSBmcm9tICcuLi9tb2RlbHMnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWR5bmFtaWMtbGF5b3V0JyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8bmctY29udGFpbmVyICpuZ1RlbXBsYXRlT3V0bGV0PVwibGF5b3V0ID8gY29tcG9uZW50T3V0bGV0IDogcm91dGVyT3V0bGV0XCI+PC9uZy1jb250YWluZXI+XG5cbiAgICA8bmctdGVtcGxhdGUgI3JvdXRlck91dGxldD48cm91dGVyLW91dGxldD48L3JvdXRlci1vdXRsZXQ+PC9uZy10ZW1wbGF0ZT5cbiAgICA8bmctdGVtcGxhdGUgI2NvbXBvbmVudE91dGxldD48bmctY29udGFpbmVyICpuZ0NvbXBvbmVudE91dGxldD1cImxheW91dFwiPjwvbmctY29udGFpbmVyPjwvbmctdGVtcGxhdGU+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIER5bmFtaWNMYXlvdXRDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncmVxdWlyZW1lbnRzJykpXG4gIHJlcXVpcmVtZW50cyQ6IE9ic2VydmFibGU8Q29uZmlnLlJlcXVpcmVtZW50cz47XG5cbiAgbGF5b3V0OiBUeXBlPGFueT47XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHtcbiAgICB0aGlzLnJvdXRlci5ldmVudHMucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKS5zdWJzY3JpYmUoZXZlbnQgPT4ge1xuICAgICAgaWYgKGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvbkVuZCkge1xuICAgICAgICBjb25zdCB7IHNlZ21lbnRzIH0gPSB0aGlzLnJvdXRlci5wYXJzZVVybChldmVudC51cmwpLnJvb3QuY2hpbGRyZW4ucHJpbWFyeTtcbiAgICAgICAgY29uc3Qge1xuICAgICAgICAgIHJlcXVpcmVtZW50czogeyBsYXlvdXRzIH0sXG4gICAgICAgICAgcm91dGVzLFxuICAgICAgICB9ID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBbGwpO1xuXG4gICAgICAgIGNvbnN0IGxheW91dCA9IGZpbmRMYXlvdXQoc2VnbWVudHMsIHJvdXRlcyk7XG5cbiAgICAgICAgdGhpcy5sYXlvdXQgPSBsYXlvdXRzLmZpbHRlcihsID0+ICEhbCkuZmluZChsID0+IHNucSgoKSA9PiBsLnR5cGUudG9Mb3dlckNhc2UoKS5pbmRleE9mKGxheW91dCksIC0xKSA+IC0xKTtcbiAgICAgIH1cbiAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCkge31cbn1cblxuZnVuY3Rpb24gZmluZExheW91dChzZWdtZW50czogVXJsU2VnbWVudFtdLCByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSk6IGVMYXlvdXRUeXBlIHtcbiAgbGV0IGxheW91dCA9IGVMYXlvdXRUeXBlLmVtcHR5O1xuXG4gIGNvbnN0IHJvdXRlID0gcm91dGVzXG4gICAgLnJlZHVjZSgoYWNjLCB2YWwpID0+ICh2YWwud3JhcHBlciA/IFsuLi5hY2MsIC4uLnZhbC5jaGlsZHJlbl0gOiBbLi4uYWNjLCB2YWxdKSwgW10pXG4gICAgLmZpbmQociA9PiByLnBhdGggPT09IHNlZ21lbnRzWzBdLnBhdGgpO1xuXG4gIGlmIChyb3V0ZSkge1xuICAgIGlmIChyb3V0ZS5sYXlvdXQpIHtcbiAgICAgIGxheW91dCA9IHJvdXRlLmxheW91dDtcbiAgICB9XG5cbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoICYmIHNlZ21lbnRzLmxlbmd0aCA+IDEpIHtcbiAgICAgIGNvbnN0IGNoaWxkID0gcm91dGUuY2hpbGRyZW4uZmluZChjID0+IGMucGF0aCA9PT0gc2VnbWVudHNbMV0ucGF0aCk7XG5cbiAgICAgIGlmIChjaGlsZCAmJiBjaGlsZC5sYXlvdXQpIHtcbiAgICAgICAgbGF5b3V0ID0gY2hpbGQubGF5b3V0O1xuICAgICAgfVxuICAgIH1cbiAgfVxuXG4gIHJldHVybiBsYXlvdXQ7XG59XG4iXX0=