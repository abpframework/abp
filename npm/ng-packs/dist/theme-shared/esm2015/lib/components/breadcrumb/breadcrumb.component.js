/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/breadcrumb/breadcrumb.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';
export class BreadcrumbComponent {
    /**
     * @param {?} router
     * @param {?} store
     */
    constructor(router, store) {
        this.router = router;
        this.store = store;
        this.segments = [];
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.show = !!this.store.selectSnapshot((/**
         * @param {?} state
         * @return {?}
         */
        state => state.LeptonLayoutState));
        /** @type {?} */
        const splittedUrl = this.router.url.split('/').filter((/**
         * @param {?} chunk
         * @return {?}
         */
        chunk => chunk));
        /** @type {?} */
        const currentUrl = this.store.selectSnapshot(ConfigState.getRoute(splittedUrl[0]));
        this.segments.push(currentUrl.name);
        if (splittedUrl.length > 1) {
            const [, ...arr] = splittedUrl;
            /** @type {?} */
            let childRoute = currentUrl;
            for (let i = 0; i < arr.length; i++) {
                /** @type {?} */
                const element = arr[i];
                childRoute = childRoute.children.find((/**
                 * @param {?} child
                 * @return {?}
                 */
                child => child.path === element));
                this.segments.push(childRoute.name);
            }
        }
    }
}
BreadcrumbComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-breadcrumb',
                template: "<ol *ngIf=\"show\" class=\"breadcrumb\">\r\n  <li class=\"breadcrumb-item\">\r\n    <a routerLink=\"/\"><i class=\"fa fa-home\"></i> </a>\r\n  </li>\r\n  <li\r\n    *ngFor=\"let segment of segments; let last = last\"\r\n    class=\"breadcrumb-item\"\r\n    [class.active]=\"last\"\r\n    aria-current=\"page\"\r\n  >\r\n    {{ segment | abpLocalization }}\r\n  </li>\r\n</ol>\r\n"
            }] }
];
/** @nocollapse */
BreadcrumbComponent.ctorParameters = () => [
    { type: Router },
    { type: Store }
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnJlYWRjcnVtYi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQU8sTUFBTSxjQUFjLENBQUM7QUFNaEQsTUFBTSxPQUFPLG1CQUFtQjs7Ozs7SUFLOUIsWUFBb0IsTUFBYyxFQUFVLEtBQVk7UUFBcEMsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFGeEQsYUFBUSxHQUFhLEVBQUUsQ0FBQztJQUVtQyxDQUFDOzs7O0lBRTVELFFBQVE7UUFDTixJQUFJLENBQUMsSUFBSSxHQUFHLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWM7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsRUFBQyxDQUFDOztjQUVwRSxXQUFXLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE1BQU07Ozs7UUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssRUFBQzs7Y0FFL0QsVUFBVSxHQUFrQixJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO1FBQ2pHLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQUVwQyxJQUFJLFdBQVcsQ0FBQyxNQUFNLEdBQUcsQ0FBQyxFQUFFO2tCQUNwQixDQUFDLEVBQUUsR0FBRyxHQUFHLENBQUMsR0FBRyxXQUFXOztnQkFFMUIsVUFBVSxHQUFrQixVQUFVO1lBQzFDLEtBQUssSUFBSSxDQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxHQUFHLENBQUMsTUFBTSxFQUFFLENBQUMsRUFBRSxFQUFFOztzQkFDN0IsT0FBTyxHQUFHLEdBQUcsQ0FBQyxDQUFDLENBQUM7Z0JBQ3RCLFVBQVUsR0FBRyxVQUFVLENBQUMsUUFBUSxDQUFDLElBQUk7Ozs7Z0JBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUMsSUFBSSxLQUFLLE9BQU8sRUFBQyxDQUFDO2dCQUV2RSxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxDQUFDLENBQUM7YUFDckM7U0FDRjtJQUNILENBQUM7OztZQTlCRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtnQkFDMUIsdVlBQTBDO2FBQzNDOzs7O1lBUFEsTUFBTTtZQUNOLEtBQUs7Ozs7SUFRWixtQ0FBYzs7SUFFZCx1Q0FBd0I7Ozs7O0lBRVoscUNBQXNCOzs7OztJQUFFLG9DQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBDb25maWdTdGF0ZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLWJyZWFkY3J1bWInLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9icmVhZGNydW1iLmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIEJyZWFkY3J1bWJDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xyXG4gIHNob3c6IGJvb2xlYW47XHJcblxyXG4gIHNlZ21lbnRzOiBzdHJpbmdbXSA9IFtdO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgbmdPbkluaXQoKTogdm9pZCB7XHJcbiAgICB0aGlzLnNob3cgPSAhIXRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3Qoc3RhdGUgPT4gc3RhdGUuTGVwdG9uTGF5b3V0U3RhdGUpO1xyXG5cclxuICAgIGNvbnN0IHNwbGl0dGVkVXJsID0gdGhpcy5yb3V0ZXIudXJsLnNwbGl0KCcvJykuZmlsdGVyKGNodW5rID0+IGNodW5rKTtcclxuXHJcbiAgICBjb25zdCBjdXJyZW50VXJsOiBBQlAuRnVsbFJvdXRlID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRSb3V0ZShzcGxpdHRlZFVybFswXSkpO1xyXG4gICAgdGhpcy5zZWdtZW50cy5wdXNoKGN1cnJlbnRVcmwubmFtZSk7XHJcblxyXG4gICAgaWYgKHNwbGl0dGVkVXJsLmxlbmd0aCA+IDEpIHtcclxuICAgICAgY29uc3QgWywgLi4uYXJyXSA9IHNwbGl0dGVkVXJsO1xyXG5cclxuICAgICAgbGV0IGNoaWxkUm91dGU6IEFCUC5GdWxsUm91dGUgPSBjdXJyZW50VXJsO1xyXG4gICAgICBmb3IgKGxldCBpID0gMDsgaSA8IGFyci5sZW5ndGg7IGkrKykge1xyXG4gICAgICAgIGNvbnN0IGVsZW1lbnQgPSBhcnJbaV07XHJcbiAgICAgICAgY2hpbGRSb3V0ZSA9IGNoaWxkUm91dGUuY2hpbGRyZW4uZmluZChjaGlsZCA9PiBjaGlsZC5wYXRoID09PSBlbGVtZW50KTtcclxuXHJcbiAgICAgICAgdGhpcy5zZWdtZW50cy5wdXNoKGNoaWxkUm91dGUubmFtZSk7XHJcbiAgICAgIH1cclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19