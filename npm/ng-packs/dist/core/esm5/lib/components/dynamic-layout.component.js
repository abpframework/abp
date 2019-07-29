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
var DynamicLayoutComponent = /** @class */ (function () {
    function DynamicLayoutComponent(router, store) {
        var _this = this;
        this.router = router;
        this.store = store;
        this.router.events.pipe(takeUntilDestroy(this)).subscribe((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            if (event instanceof NavigationEnd) {
                var segments = _this.router.parseUrl(event.url).root.children.primary.segments;
                var _a = _this.store.selectSnapshot(ConfigState.getAll), layouts = _a.requirements.layouts, routes = _a.routes;
                /** @type {?} */
                var layout_1 = findLayout(segments, routes);
                _this.layout = layouts.filter((/**
                 * @param {?} l
                 * @return {?}
                 */
                function (l) { return !!l; })).find((/**
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
        if (route.children && route.children.length) {
            /** @type {?} */
            var child = route.children.find((/**
             * @param {?} c
             * @return {?}
             */
            function (c) { return c.path === segments[1].path; }));
            if (child.layout) {
                layout = child.layout;
            }
        }
    }
    return layout;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZHluYW1pYy1sYXlvdXQuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBbUIsTUFBTSxlQUFlLENBQUM7QUFDM0QsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLEVBQWMsTUFBTSxpQkFBaUIsQ0FBQztBQUNwRSxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBR2xDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFDeEMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBQzVDLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUV0QjtJQWVFLGdDQUFvQixNQUFjLEVBQVUsS0FBWTtRQUF4RCxpQkFjQztRQWRtQixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUN0RCxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxTQUFTOzs7O1FBQUMsVUFBQSxLQUFLO1lBQzdELElBQUksS0FBSyxZQUFZLGFBQWEsRUFBRTtnQkFDMUIsSUFBQSwwRUFBUTtnQkFDVixJQUFBLG1EQUcyQyxFQUYvQixpQ0FBTyxFQUN2QixrQkFDK0M7O29CQUUzQyxRQUFNLEdBQUcsVUFBVSxDQUFDLFFBQVEsRUFBRSxNQUFNLENBQUM7Z0JBRTNDLEtBQUksQ0FBQyxNQUFNLEdBQUcsT0FBTyxDQUFDLE1BQU07Ozs7Z0JBQUMsVUFBQSxDQUFDLElBQUksT0FBQSxDQUFDLENBQUMsQ0FBQyxFQUFILENBQUcsRUFBQyxDQUFDLElBQUk7Ozs7Z0JBQUMsVUFBQSxDQUFDLElBQUksT0FBQSxHQUFHOzs7Z0JBQUMsY0FBTSxPQUFBLENBQUMsQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFLENBQUMsT0FBTyxDQUFDLFFBQU0sQ0FBQyxFQUFwQyxDQUFvQyxHQUFFLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEVBQXhELENBQXdELEVBQUMsQ0FBQzthQUM1RztRQUNILENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7OztJQUVELDRDQUFXOzs7SUFBWCxjQUFlLENBQUM7O2dCQS9CakIsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxvQkFBb0I7b0JBQzlCLFFBQVEsRUFBRSxvU0FLVDtpQkFDRjs7OztnQkFqQnVCLE1BQU07Z0JBQ2IsS0FBSzs7SUFtQnBCO1FBREMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUMsY0FBYyxDQUFDLENBQUM7MENBQzVCLFVBQVU7aUVBQXNCO0lBcUJqRCw2QkFBQztDQUFBLEFBaENELElBZ0NDO1NBdkJZLHNCQUFzQjs7O0lBQ2pDLCtDQUMrQzs7SUFFL0Msd0NBQWtCOzs7OztJQUVOLHdDQUFzQjs7Ozs7SUFBRSx1Q0FBb0I7Ozs7Ozs7QUFtQjFELFNBQVMsVUFBVSxDQUFDLFFBQXNCLEVBQUUsTUFBdUI7O1FBQzdELE1BQU0sc0JBQW9COztRQUV4QixLQUFLLEdBQUcsTUFBTTtTQUNqQixNQUFNOzs7OztJQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUcsSUFBSyxPQUFBLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxDQUFDLGtCQUFLLEdBQUcsRUFBSyxHQUFHLENBQUMsUUFBUSxFQUFFLENBQUMsa0JBQUssR0FBRyxHQUFFLEdBQUcsRUFBQyxDQUFDLEVBQXpELENBQXlELEdBQUUsRUFBRSxDQUFDO1NBQ25GLElBQUk7Ozs7SUFBQyxVQUFBLENBQUMsSUFBSSxPQUFBLENBQUMsQ0FBQyxJQUFJLEtBQUssUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksRUFBM0IsQ0FBMkIsRUFBQztJQUV6QyxJQUFJLEtBQUssRUFBRTtRQUNULElBQUksS0FBSyxDQUFDLE1BQU0sRUFBRTtZQUNoQixNQUFNLEdBQUcsS0FBSyxDQUFDLE1BQU0sQ0FBQztTQUN2QjtRQUVELElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTs7Z0JBQ3JDLEtBQUssR0FBRyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUk7Ozs7WUFBQyxVQUFBLENBQUMsSUFBSSxPQUFBLENBQUMsQ0FBQyxJQUFJLEtBQUssUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksRUFBM0IsQ0FBMkIsRUFBQztZQUVuRSxJQUFJLEtBQUssQ0FBQyxNQUFNLEVBQUU7Z0JBQ2hCLE1BQU0sR0FBRyxLQUFLLENBQUMsTUFBTSxDQUFDO2FBQ3ZCO1NBQ0Y7S0FDRjtJQUVELE9BQU8sTUFBTSxDQUFDO0FBQ2hCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIE9uRGVzdHJveSwgVHlwZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmF2aWdhdGlvbkVuZCwgUm91dGVyLCBVcmxTZWdtZW50IH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBlTGF5b3V0VHlwZSB9IGZyb20gJy4uL2VudW1zJztcbmltcG9ydCB7IEFCUCwgQ29uZmlnIH0gZnJvbSAnLi4vbW9kZWxzJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICcuLi91dGlscyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1keW5hbWljLWxheW91dCcsXG4gIHRlbXBsYXRlOiBgXG4gICAgPG5nLWNvbnRhaW5lciAqbmdUZW1wbGF0ZU91dGxldD1cImxheW91dCA/IGNvbXBvbmVudE91dGxldCA6IHJvdXRlck91dGxldFwiPjwvbmctY29udGFpbmVyPlxuXG4gICAgPG5nLXRlbXBsYXRlICNyb3V0ZXJPdXRsZXQ+PHJvdXRlci1vdXRsZXQ+PC9yb3V0ZXItb3V0bGV0PjwvbmctdGVtcGxhdGU+XG4gICAgPG5nLXRlbXBsYXRlICNjb21wb25lbnRPdXRsZXQ+PG5nLWNvbnRhaW5lciAqbmdDb21wb25lbnRPdXRsZXQ9XCJsYXlvdXRcIj48L25nLWNvbnRhaW5lcj48L25nLXRlbXBsYXRlPlxuICBgLFxufSlcbmV4cG9ydCBjbGFzcyBEeW5hbWljTGF5b3V0Q29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ3JlcXVpcmVtZW50cycpKVxuICByZXF1aXJlbWVudHMkOiBPYnNlcnZhYmxlPENvbmZpZy5SZXF1aXJlbWVudHM+O1xuXG4gIGxheW91dDogVHlwZTxhbnk+O1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7XG4gICAgdGhpcy5yb3V0ZXIuZXZlbnRzLnBpcGUodGFrZVVudGlsRGVzdHJveSh0aGlzKSkuc3Vic2NyaWJlKGV2ZW50ID0+IHtcbiAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQpIHtcbiAgICAgICAgY29uc3QgeyBzZWdtZW50cyB9ID0gdGhpcy5yb3V0ZXIucGFyc2VVcmwoZXZlbnQudXJsKS5yb290LmNoaWxkcmVuLnByaW1hcnk7XG4gICAgICAgIGNvbnN0IHtcbiAgICAgICAgICByZXF1aXJlbWVudHM6IHsgbGF5b3V0cyB9LFxuICAgICAgICAgIHJvdXRlcyxcbiAgICAgICAgfSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QWxsKTtcblxuICAgICAgICBjb25zdCBsYXlvdXQgPSBmaW5kTGF5b3V0KHNlZ21lbnRzLCByb3V0ZXMpO1xuXG4gICAgICAgIHRoaXMubGF5b3V0ID0gbGF5b3V0cy5maWx0ZXIobCA9PiAhIWwpLmZpbmQobCA9PiBzbnEoKCkgPT4gbC50eXBlLnRvTG93ZXJDYXNlKCkuaW5kZXhPZihsYXlvdXQpLCAtMSkgPiAtMSk7XG4gICAgICB9XG4gICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG59XG5cbmZ1bmN0aW9uIGZpbmRMYXlvdXQoc2VnbWVudHM6IFVybFNlZ21lbnRbXSwgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10pOiBlTGF5b3V0VHlwZSB7XG4gIGxldCBsYXlvdXQgPSBlTGF5b3V0VHlwZS5lbXB0eTtcblxuICBjb25zdCByb3V0ZSA9IHJvdXRlc1xuICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiAodmFsLndyYXBwZXIgPyBbLi4uYWNjLCAuLi52YWwuY2hpbGRyZW5dIDogWy4uLmFjYywgdmFsXSksIFtdKVxuICAgIC5maW5kKHIgPT4gci5wYXRoID09PSBzZWdtZW50c1swXS5wYXRoKTtcblxuICBpZiAocm91dGUpIHtcbiAgICBpZiAocm91dGUubGF5b3V0KSB7XG4gICAgICBsYXlvdXQgPSByb3V0ZS5sYXlvdXQ7XG4gICAgfVxuXG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgY29uc3QgY2hpbGQgPSByb3V0ZS5jaGlsZHJlbi5maW5kKGMgPT4gYy5wYXRoID09PSBzZWdtZW50c1sxXS5wYXRoKTtcblxuICAgICAgaWYgKGNoaWxkLmxheW91dCkge1xuICAgICAgICBsYXlvdXQgPSBjaGlsZC5sYXlvdXQ7XG4gICAgICB9XG4gICAgfVxuICB9XG5cbiAgcmV0dXJuIGxheW91dDtcbn1cbiJdfQ==