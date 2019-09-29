/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                    template: "\n    <ng-container *ngTemplateOutlet=\"layout ? componentOutlet : routerOutlet\"></ng-container>\n\n    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>\n    <ng-template #componentOutlet><ng-container *ngComponentOutlet=\"layout\"></ng-container></ng-template>\n  "
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBMEIsTUFBTSxlQUFlLENBQUM7QUFDbEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxhQUFhLEVBQUUsTUFBTSxFQUFjLE1BQU0saUJBQWlCLENBQUM7QUFDcEYsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFJdEIsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBQ3JELE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBRXZEO0lBZUUsZ0NBQW9CLE1BQWMsRUFBVSxLQUFxQixFQUFVLEtBQVk7UUFBdkYsaUJBdUJDO1FBdkJtQixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBZ0I7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQy9FLElBQUEsa0RBRzJDLEVBRi9CLGlDQUFPLEVBQ3ZCLGtCQUMrQztRQUVqRCxJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxJQUFJLEVBQUUsQ0FBQyxDQUFDLE1BQU0sRUFBRTtZQUMzQyxJQUFJLENBQUMsTUFBTSxHQUFHLE9BQU87aUJBQ2xCLE1BQU07Ozs7WUFBQyxVQUFBLENBQUMsSUFBSSxPQUFBLENBQUMsQ0FBQyxDQUFDLEVBQUgsQ0FBRyxFQUFDO2lCQUNoQixJQUFJOzs7O1lBQUMsVUFBQyxDQUFNLElBQUssT0FBQSxHQUFHOzs7WUFBQyxjQUFN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QUVuRSxJQUFJLEtBQUssSUFBSSxLQUFLLENBQUMsTUFBTSxFQUFFO2dCQUN6QixNQUFNLEdBQUcsS0FBSyxDQUFDLE1BQU0sQ0FBQzthQUN2QjtTQUNGO0tBQ0Y7SUFFRCxPQUFPLE1BQU0sQ0FBQztBQUNoQixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBJbnB1dCwgT25EZXN0cm95LCBUeXBlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBY3RpdmF0ZWRSb3V0ZSwgTmF2aWdhdGlvbkVuZCwgUm91dGVyLCBVcmxTZWdtZW50IH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBlTGF5b3V0VHlwZSB9IGZyb20gJy4uL2VudW1zL2NvbW1vbic7XG5pbXBvcnQgeyBDb25maWcgfSBmcm9tICcuLi9tb2RlbHMvY29uZmlnJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscy9jb21tb24nO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvY29uZmlnLnN0YXRlJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICcuLi91dGlscy9yeGpzLXV0aWxzJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWR5bmFtaWMtbGF5b3V0JyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8bmctY29udGFpbmVyICpuZ1RlbXBsYXRlT3V0bGV0PVwibGF5b3V0ID8gY29tcG9uZW50T3V0bGV0IDogcm91dGVyT3V0bGV0XCI+PC9uZy1jb250YWluZXI+XG5cbiAgICA8bmctdGVtcGxhdGUgI3JvdXRlck91dGxldD48cm91dGVyLW91dGxldD48L3JvdXRlci1vdXRsZXQ+PC9uZy10ZW1wbGF0ZT5cbiAgICA8bmctdGVtcGxhdGUgI2NvbXBvbmVudE91dGxldD48bmctY29udGFpbmVyICpuZ0NvbXBvbmVudE91dGxldD1cImxheW91dFwiPjwvbmctY29udGFpbmVyPjwvbmctdGVtcGxhdGU+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIER5bmFtaWNMYXlvdXRDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncmVxdWlyZW1lbnRzJykpXG4gIHJlcXVpcmVtZW50cyQ6IE9ic2VydmFibGU8Q29uZmlnLlJlcXVpcmVtZW50cz47XG5cbiAgbGF5b3V0OiBUeXBlPGFueT47XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSByb3V0ZTogQWN0aXZhdGVkUm91dGUsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7XG4gICAgY29uc3Qge1xuICAgICAgcmVxdWlyZW1lbnRzOiB7IGxheW91dHMgfSxcbiAgICAgIHJvdXRlcyxcbiAgICB9ID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBbGwpO1xuXG4gICAgaWYgKCh0aGlzLnJvdXRlLnNuYXBzaG90LmRhdGEgfHwge30pLmxheW91dCkge1xuICAgICAgdGhpcy5sYXlvdXQgPSBsYXlvdXRzXG4gICAgICAgIC5maWx0ZXIobCA9PiAhIWwpXG4gICAgICAgIC5maW5kKChsOiBhbnkpID0+IHNucSgoKSA9PiBsLnR5cGUudG9Mb3dlckNhc2UoKS5pbmRleE9mKHRoaXMucm91dGUuc25hcHNob3QuZGF0YS5sYXlvdXQpLCAtMSkgPiAtMSk7XG4gICAgfVxuXG4gICAgdGhpcy5yb3V0ZXIuZXZlbnRzLnBpcGUodGFrZVVudGlsRGVzdHJveSh0aGlzKSkuc3Vic2NyaWJlKGV2ZW50ID0+IHtcbiAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQpIHtcbiAgICAgICAgY29uc3QgeyBzZWdtZW50cyB9ID0gdGhpcy5yb3V0ZXIucGFyc2VVcmwoZXZlbnQudXJsKS5yb290LmNoaWxkcmVuLnByaW1hcnk7XG5cbiAgICAgICAgY29uc3QgbGF5b3V0ID0gKHRoaXMucm91dGUuc25hcHNob3QuZGF0YSB8fCB7fSkubGF5b3V0IHx8IGZpbmRMYXlvdXQoc2VnbWVudHMsIHJvdXRlcyk7XG5cbiAgICAgICAgdGhpcy5sYXlvdXQgPSBsYXlvdXRzXG4gICAgICAgICAgLmZpbHRlcihsID0+ICEhbClcbiAgICAgICAgICAuZmluZCgobDogYW55KSA9PiBzbnEoKCkgPT4gbC50eXBlLnRvTG93ZXJDYXNlKCkuaW5kZXhPZihsYXlvdXQpLCAtMSkgPiAtMSk7XG4gICAgICB9XG4gICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG59XG5cbmZ1bmN0aW9uIGZpbmRMYXlvdXQoc2VnbWVudHM6IFVybFNlZ21lbnRbXSwgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10pOiBlTGF5b3V0VHlwZSB7XG4gIGxldCBsYXlvdXQgPSBlTGF5b3V0VHlwZS5lbXB0eTtcblxuICBjb25zdCByb3V0ZSA9IHJvdXRlc1xuICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiAodmFsLndyYXBwZXIgPyBbLi4uYWNjLCAuLi52YWwuY2hpbGRyZW5dIDogWy4uLmFjYywgdmFsXSksIFtdKVxuICAgIC5maW5kKHIgPT4gci5wYXRoID09PSBzZWdtZW50c1swXS5wYXRoKTtcblxuICBpZiAocm91dGUpIHtcbiAgICBpZiAocm91dGUubGF5b3V0KSB7XG4gICAgICBsYXlvdXQgPSByb3V0ZS5sYXlvdXQ7XG4gICAgfVxuXG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiBzZWdtZW50cy5sZW5ndGggPiAxKSB7XG4gICAgICBjb25zdCBjaGlsZCA9IHJvdXRlLmNoaWxkcmVuLmZpbmQoYyA9PiBjLnBhdGggPT09IHNlZ21lbnRzWzFdLnBhdGgpO1xuXG4gICAgICBpZiAoY2hpbGQgJiYgY2hpbGQubGF5b3V0KSB7XG4gICAgICAgIGxheW91dCA9IGNoaWxkLmxheW91dDtcbiAgICAgIH1cbiAgICB9XG4gIH1cblxuICByZXR1cm4gbGF5b3V0O1xufVxuIl19