/**
 * @fileoverview added by tsickle
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
        this.show = !!this.store.selectSnapshot((/**
         * @param {?} state
         * @return {?}
         */
        function (state) { return state.LeptonLayoutState; }));
    }
    /**
     * @return {?}
     */
    BreadcrumbComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var splittedUrl = this.router.url.split('/').filter((/**
         * @param {?} chunk
         * @return {?}
         */
        function (chunk) { return chunk; }));
        /** @type {?} */
        var currentUrl = this.store.selectSnapshot(ConfigState.getRoute(splittedUrl[0]));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnJlYWRjcnVtYi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQU8sTUFBTSxjQUFjLENBQUM7QUFFaEQ7SUFTRSw2QkFBb0IsTUFBYyxFQUFVLEtBQVk7UUFBcEMsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFGeEQsYUFBUSxHQUFhLEVBQUUsQ0FBQztRQUd0QixJQUFJLENBQUMsSUFBSSxHQUFHLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWM7Ozs7UUFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLEtBQUssQ0FBQyxpQkFBaUIsRUFBdkIsQ0FBdUIsRUFBQyxDQUFDO0lBQzVFLENBQUM7Ozs7SUFFRCxzQ0FBUTs7O0lBQVI7O1lBQ1EsV0FBVyxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxNQUFNOzs7O1FBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLEVBQUwsQ0FBSyxFQUFDOztZQUUvRCxVQUFVLEdBQWtCLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7UUFDakcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLElBQUksQ0FBQyxDQUFDO1FBRXBDLElBQUksV0FBVyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7WUFDcEIsSUFBQSxnQ0FBd0IsRUFBckIsaUJBQXFCOztnQkFFMUIsVUFBVSxHQUFrQixVQUFVO29DQUNqQyxDQUFDOztvQkFDRixPQUFPLEdBQUcsR0FBRyxDQUFDLENBQUMsQ0FBQztnQkFDdEIsVUFBVSxHQUFHLFVBQVUsQ0FBQyxRQUFRLENBQUMsSUFBSTs7OztnQkFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLEtBQUssQ0FBQyxJQUFJLEtBQUssT0FBTyxFQUF0QixDQUFzQixFQUFDLENBQUM7Z0JBRXZFLE9BQUssUUFBUSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxDQUFDLENBQUM7OztZQUp0QyxLQUFLLElBQUksQ0FBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsR0FBRyxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQUU7d0JBQTFCLENBQUM7YUFLVDtTQUNGO0lBQ0gsQ0FBQzs7Z0JBOUJGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO29CQUMxQiw2V0FBMEM7aUJBQzNDOzs7O2dCQVBRLE1BQU07Z0JBQ04sS0FBSzs7SUFrQ2QsMEJBQUM7Q0FBQSxBQS9CRCxJQStCQztTQTNCWSxtQkFBbUI7OztJQUM5QixtQ0FBYzs7SUFFZCx1Q0FBd0I7Ozs7O0lBRVoscUNBQXNCOzs7OztJQUFFLG9DQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWJyZWFkY3J1bWInLFxuICB0ZW1wbGF0ZVVybDogJy4vYnJlYWRjcnVtYi5jb21wb25lbnQuaHRtbCdcbn0pXG5leHBvcnQgY2xhc3MgQnJlYWRjcnVtYkNvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIHNob3c6IGJvb2xlYW47XG5cbiAgc2VnbWVudHM6IHN0cmluZ1tdID0gW107XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHtcbiAgICB0aGlzLnNob3cgPSAhIXRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3Qoc3RhdGUgPT4gc3RhdGUuTGVwdG9uTGF5b3V0U3RhdGUpO1xuICB9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7XG4gICAgY29uc3Qgc3BsaXR0ZWRVcmwgPSB0aGlzLnJvdXRlci51cmwuc3BsaXQoJy8nKS5maWx0ZXIoY2h1bmsgPT4gY2h1bmspO1xuXG4gICAgY29uc3QgY3VycmVudFVybDogQUJQLkZ1bGxSb3V0ZSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0Um91dGUoc3BsaXR0ZWRVcmxbMF0pKTtcbiAgICB0aGlzLnNlZ21lbnRzLnB1c2goY3VycmVudFVybC5uYW1lKTtcblxuICAgIGlmIChzcGxpdHRlZFVybC5sZW5ndGggPiAxKSB7XG4gICAgICBjb25zdCBbLCAuLi5hcnJdID0gc3BsaXR0ZWRVcmw7XG5cbiAgICAgIGxldCBjaGlsZFJvdXRlOiBBQlAuRnVsbFJvdXRlID0gY3VycmVudFVybDtcbiAgICAgIGZvciAobGV0IGkgPSAwOyBpIDwgYXJyLmxlbmd0aDsgaSsrKSB7XG4gICAgICAgIGNvbnN0IGVsZW1lbnQgPSBhcnJbaV07XG4gICAgICAgIGNoaWxkUm91dGUgPSBjaGlsZFJvdXRlLmNoaWxkcmVuLmZpbmQoY2hpbGQgPT4gY2hpbGQucGF0aCA9PT0gZWxlbWVudCk7XG5cbiAgICAgICAgdGhpcy5zZWdtZW50cy5wdXNoKGNoaWxkUm91dGUubmFtZSk7XG4gICAgICB9XG4gICAgfVxuICB9XG59XG4iXX0=