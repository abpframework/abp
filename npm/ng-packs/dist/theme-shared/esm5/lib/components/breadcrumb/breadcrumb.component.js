/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                    template: "\n    <ol *ngIf=\"show\" class=\"breadcrumb\">\n      <li class=\"breadcrumb-item\">\n        <a routerLink=\"/\"><i class=\"fa fa-home\"></i> </a>\n      </li>\n      <li\n        *ngFor=\"let segment of segments; let last = last\"\n        class=\"breadcrumb-item\"\n        [class.active]=\"last\"\n        aria-current=\"page\"\n      >\n        {{ segment | abpLocalization }}\n      </li>\n    </ol>\n  "
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnJlYWRjcnVtYi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQU8sTUFBTSxjQUFjLENBQUM7QUFFaEQ7SUF1QkUsNkJBQW9CLE1BQWMsRUFBVSxLQUFZO1FBQXBDLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBRnhELGFBQVEsR0FBYSxFQUFFLENBQUM7UUFHdEIsSUFBSSxDQUFDLElBQUksR0FBRyxDQUFDLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjOzs7O1FBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLENBQUMsaUJBQWlCLEVBQXZCLENBQXVCLEVBQUMsQ0FBQztJQUM1RSxDQUFDOzs7O0lBRUQsc0NBQVE7OztJQUFSOztZQUNRLFdBQVcsR0FBRyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLENBQUMsTUFBTTs7OztRQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsS0FBSyxFQUFMLENBQUssRUFBQzs7WUFFL0QsVUFBVSxHQUFrQixJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO1FBQ2pHLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQUVwQyxJQUFJLFdBQVcsQ0FBQyxNQUFNLEdBQUcsQ0FBQyxFQUFFO1lBQ3BCLElBQUEsZ0NBQXdCLEVBQXJCLGlCQUFxQjs7Z0JBRTFCLFVBQVUsR0FBa0IsVUFBVTtvQ0FDakMsQ0FBQzs7b0JBQ0YsT0FBTyxHQUFHLEdBQUcsQ0FBQyxDQUFDLENBQUM7Z0JBQ3RCLFVBQVUsR0FBRyxVQUFVLENBQUMsUUFBUSxDQUFDLElBQUk7Ozs7Z0JBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLENBQUMsSUFBSSxLQUFLLE9BQU8sRUFBdEIsQ0FBc0IsRUFBQyxDQUFDO2dCQUV2RSxPQUFLLFFBQVEsQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLElBQUksQ0FBQyxDQUFDOzs7WUFKdEMsS0FBSyxJQUFJLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFHLEdBQUcsQ0FBQyxNQUFNLEVBQUUsQ0FBQyxFQUFFO3dCQUExQixDQUFDO2FBS1Q7U0FDRjtJQUNILENBQUM7O2dCQTVDRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtvQkFDMUIsUUFBUSxFQUFFLDJaQWNUO2lCQUNGOzs7O2dCQXJCUSxNQUFNO2dCQUNOLEtBQUs7O0lBZ0RkLDBCQUFDO0NBQUEsQUE3Q0QsSUE2Q0M7U0EzQlksbUJBQW1COzs7SUFDOUIsbUNBQWM7O0lBRWQsdUNBQXdCOzs7OztJQUVaLHFDQUFzQjs7Ozs7SUFBRSxvQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUsIEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1icmVhZGNydW1iJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8b2wgKm5nSWY9XCJzaG93XCIgY2xhc3M9XCJicmVhZGNydW1iXCI+XG4gICAgICA8bGkgY2xhc3M9XCJicmVhZGNydW1iLWl0ZW1cIj5cbiAgICAgICAgPGEgcm91dGVyTGluaz1cIi9cIj48aSBjbGFzcz1cImZhIGZhLWhvbWVcIj48L2k+IDwvYT5cbiAgICAgIDwvbGk+XG4gICAgICA8bGlcbiAgICAgICAgKm5nRm9yPVwibGV0IHNlZ21lbnQgb2Ygc2VnbWVudHM7IGxldCBsYXN0ID0gbGFzdFwiXG4gICAgICAgIGNsYXNzPVwiYnJlYWRjcnVtYi1pdGVtXCJcbiAgICAgICAgW2NsYXNzLmFjdGl2ZV09XCJsYXN0XCJcbiAgICAgICAgYXJpYS1jdXJyZW50PVwicGFnZVwiXG4gICAgICA+XG4gICAgICAgIHt7IHNlZ21lbnQgfCBhYnBMb2NhbGl6YXRpb24gfX1cbiAgICAgIDwvbGk+XG4gICAgPC9vbD5cbiAgYCxcbn0pXG5leHBvcnQgY2xhc3MgQnJlYWRjcnVtYkNvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIHNob3c6IGJvb2xlYW47XG5cbiAgc2VnbWVudHM6IHN0cmluZ1tdID0gW107XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHtcbiAgICB0aGlzLnNob3cgPSAhIXRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3Qoc3RhdGUgPT4gc3RhdGUuTGVwdG9uTGF5b3V0U3RhdGUpO1xuICB9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7XG4gICAgY29uc3Qgc3BsaXR0ZWRVcmwgPSB0aGlzLnJvdXRlci51cmwuc3BsaXQoJy8nKS5maWx0ZXIoY2h1bmsgPT4gY2h1bmspO1xuXG4gICAgY29uc3QgY3VycmVudFVybDogQUJQLkZ1bGxSb3V0ZSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0Um91dGUoc3BsaXR0ZWRVcmxbMF0pKTtcbiAgICB0aGlzLnNlZ21lbnRzLnB1c2goY3VycmVudFVybC5uYW1lKTtcblxuICAgIGlmIChzcGxpdHRlZFVybC5sZW5ndGggPiAxKSB7XG4gICAgICBjb25zdCBbLCAuLi5hcnJdID0gc3BsaXR0ZWRVcmw7XG5cbiAgICAgIGxldCBjaGlsZFJvdXRlOiBBQlAuRnVsbFJvdXRlID0gY3VycmVudFVybDtcbiAgICAgIGZvciAobGV0IGkgPSAwOyBpIDwgYXJyLmxlbmd0aDsgaSsrKSB7XG4gICAgICAgIGNvbnN0IGVsZW1lbnQgPSBhcnJbaV07XG4gICAgICAgIGNoaWxkUm91dGUgPSBjaGlsZFJvdXRlLmNoaWxkcmVuLmZpbmQoY2hpbGQgPT4gY2hpbGQucGF0aCA9PT0gZWxlbWVudCk7XG5cbiAgICAgICAgdGhpcy5zZWdtZW50cy5wdXNoKGNoaWxkUm91dGUubmFtZSk7XG4gICAgICB9XG4gICAgfVxuICB9XG59XG4iXX0=