/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/breadcrumb/breadcrumb.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';
var BreadcrumbComponent = /** @class */ (function () {
    function BreadcrumbComponent(router, store) {
        this.router = router;
        this.store = store;
        this.segments = [];
    }
    /**
     * @return {?}
     */
    BreadcrumbComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        this.show = !!this.store.selectSnapshot((/**
         * @param {?} state
         * @return {?}
         */
        function (state) { return state.LeptonLayoutState; }));
        if (this.show) {
            /** @type {?} */
            var splittedUrl = this.router.url.split('/').filter((/**
             * @param {?} chunk
             * @return {?}
             */
            function (chunk) { return chunk; }));
            /** @type {?} */
            var currentUrl = this.store.selectSnapshot(ConfigState.getRoute(splittedUrl[0]));
            if (!currentUrl) {
                currentUrl = this.store.selectSnapshot(ConfigState.getRoute(null, null, this.router.url));
                splittedUrl = [this.router.url];
                if (!currentUrl) {
                    this.show = false;
                    return;
                }
            }
            this.segments.push(currentUrl.name);
            if (splittedUrl.length > 1) {
                var _a = tslib_1.__read(splittedUrl), arr = _a.slice(1);
                /** @type {?} */
                var childRoute = currentUrl;
                var _loop_1 = function (i) {
                    /** @type {?} */
                    var element = arr[i];
                    childRoute = childRoute.children.find((/**
                     * @param {?} child
                     * @return {?}
                     */
                    function (child) { return child.path === element; }));
                    this_1.segments.push(childRoute.name);
                };
                var this_1 = this;
                for (var i = 0; i < arr.length; i++) {
                    _loop_1(i);
                }
            }
        }
    };
    BreadcrumbComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-breadcrumb',
                    template: "<ol *ngIf=\"show\" class=\"breadcrumb\">\n  <li class=\"breadcrumb-item\">\n    <a routerLink=\"/\"><i class=\"fa fa-home\"></i> </a>\n  </li>\n  <li\n    *ngFor=\"let segment of segments; let last = last\"\n    class=\"breadcrumb-item\"\n    [class.active]=\"last\"\n    aria-current=\"page\"\n  >\n    {{ segment | abpLocalization }}\n  </li>\n</ol>\n"
                }] }
    ];
    /** @nocollapse */
    BreadcrumbComponent.ctorParameters = function () { return [
        { type: Router },
        { type: Store }
    ]; };
    return BreadcrumbComponent;
}());
export { BreadcrumbComponent };
if (false) {
    /** @type {?} */
    BreadcrumbComponent.prototype.show;
    /** @type {?} */
    BreadcrumbComponent.prototype.segments;
    /**
     * @type {?}
     * @private
     */
    BreadcrumbComponent.prototype.router;
    /**
     * @type {?}
     * @private
     */
    BreadcrumbComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnJlYWRjcnVtYi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUNsRCxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDekMsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFPLE1BQU0sY0FBYyxDQUFDO0FBRWhEO0lBU0UsNkJBQW9CLE1BQWMsRUFBVSxLQUFZO1FBQXBDLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBRnhELGFBQVEsR0FBYSxFQUFFLENBQUM7SUFFbUMsQ0FBQzs7OztJQUU1RCxzQ0FBUTs7O0lBQVI7UUFDRSxJQUFJLENBQUMsSUFBSSxHQUFHLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWM7Ozs7UUFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLEtBQUssQ0FBQyxpQkFBaUIsRUFBdkIsQ0FBdUIsRUFBQyxDQUFDO1FBQzFFLElBQUksSUFBSSxDQUFDLElBQUksRUFBRTs7Z0JBQ1QsV0FBVyxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxNQUFNOzs7O1lBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLEVBQUwsQ0FBSyxFQUFDOztnQkFFL0QsVUFBVSxHQUFrQixJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO1lBRS9GLElBQUksQ0FBQyxVQUFVLEVBQUU7Z0JBQ2YsVUFBVSxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxRQUFRLENBQUMsSUFBSSxFQUFFLElBQUksRUFBRSxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7Z0JBQzFGLFdBQVcsR0FBRyxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLENBQUM7Z0JBQ2hDLElBQUksQ0FBQyxVQUFVLEVBQUU7b0JBQ2YsSUFBSSxDQUFDLElBQUksR0FBRyxLQUFLLENBQUM7b0JBQ2xCLE9BQU87aUJBQ1I7YUFDRjtZQUVELElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsQ0FBQztZQUVwQyxJQUFJLFdBQVcsQ0FBQyxNQUFNLEdBQUcsQ0FBQyxFQUFFO2dCQUNwQixJQUFBLGdDQUF3QixFQUFyQixpQkFBcUI7O29CQUUxQixVQUFVLEdBQWtCLFVBQVU7d0NBQ2pDLENBQUM7O3dCQUNGLE9BQU8sR0FBRyxHQUFHLENBQUMsQ0FBQyxDQUFDO29CQUN0QixVQUFVLEdBQUcsVUFBVSxDQUFDLFFBQVEsQ0FBQyxJQUFJOzs7O29CQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsS0FBSyxDQUFDLElBQUksS0FBSyxPQUFPLEVBQXRCLENBQXNCLEVBQUMsQ0FBQztvQkFFdkUsT0FBSyxRQUFRLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsQ0FBQzs7O2dCQUp0QyxLQUFLLElBQUksQ0FBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsR0FBRyxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQUU7NEJBQTFCLENBQUM7aUJBS1Q7YUFDRjtTQUNGO0lBQ0gsQ0FBQzs7Z0JBekNGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO29CQUMxQiw2V0FBMEM7aUJBQzNDOzs7O2dCQVBRLE1BQU07Z0JBQ04sS0FBSzs7SUE2Q2QsMEJBQUM7Q0FBQSxBQTFDRCxJQTBDQztTQXRDWSxtQkFBbUI7OztJQUM5QixtQ0FBYzs7SUFFZCx1Q0FBd0I7Ozs7O0lBRVoscUNBQXNCOzs7OztJQUFFLG9DQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWJyZWFkY3J1bWInLFxuICB0ZW1wbGF0ZVVybDogJy4vYnJlYWRjcnVtYi5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIEJyZWFkY3J1bWJDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBzaG93OiBib29sZWFuO1xuXG4gIHNlZ21lbnRzOiBzdHJpbmdbXSA9IFtdO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIG5nT25Jbml0KCk6IHZvaWQge1xuICAgIHRoaXMuc2hvdyA9ICEhdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChzdGF0ZSA9PiBzdGF0ZS5MZXB0b25MYXlvdXRTdGF0ZSk7XG4gICAgaWYgKHRoaXMuc2hvdykge1xuICAgICAgbGV0IHNwbGl0dGVkVXJsID0gdGhpcy5yb3V0ZXIudXJsLnNwbGl0KCcvJykuZmlsdGVyKGNodW5rID0+IGNodW5rKTtcblxuICAgICAgbGV0IGN1cnJlbnRVcmw6IEFCUC5GdWxsUm91dGUgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFJvdXRlKHNwbGl0dGVkVXJsWzBdKSk7XG5cbiAgICAgIGlmICghY3VycmVudFVybCkge1xuICAgICAgICBjdXJyZW50VXJsID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRSb3V0ZShudWxsLCBudWxsLCB0aGlzLnJvdXRlci51cmwpKTtcbiAgICAgICAgc3BsaXR0ZWRVcmwgPSBbdGhpcy5yb3V0ZXIudXJsXTtcbiAgICAgICAgaWYgKCFjdXJyZW50VXJsKSB7XG4gICAgICAgICAgdGhpcy5zaG93ID0gZmFsc2U7XG4gICAgICAgICAgcmV0dXJuO1xuICAgICAgICB9XG4gICAgICB9XG5cbiAgICAgIHRoaXMuc2VnbWVudHMucHVzaChjdXJyZW50VXJsLm5hbWUpO1xuXG4gICAgICBpZiAoc3BsaXR0ZWRVcmwubGVuZ3RoID4gMSkge1xuICAgICAgICBjb25zdCBbLCAuLi5hcnJdID0gc3BsaXR0ZWRVcmw7XG5cbiAgICAgICAgbGV0IGNoaWxkUm91dGU6IEFCUC5GdWxsUm91dGUgPSBjdXJyZW50VXJsO1xuICAgICAgICBmb3IgKGxldCBpID0gMDsgaSA8IGFyci5sZW5ndGg7IGkrKykge1xuICAgICAgICAgIGNvbnN0IGVsZW1lbnQgPSBhcnJbaV07XG4gICAgICAgICAgY2hpbGRSb3V0ZSA9IGNoaWxkUm91dGUuY2hpbGRyZW4uZmluZChjaGlsZCA9PiBjaGlsZC5wYXRoID09PSBlbGVtZW50KTtcblxuICAgICAgICAgIHRoaXMuc2VnbWVudHMucHVzaChjaGlsZFJvdXRlLm5hbWUpO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgfVxuICB9XG59XG4iXX0=