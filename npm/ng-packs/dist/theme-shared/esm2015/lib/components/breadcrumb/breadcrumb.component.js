/**
 * @fileoverview added by tsickle
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
        this.show = !!this.store.selectSnapshot((/**
         * @param {?} state
         * @return {?}
         */
        state => state.LeptonLayoutState));
    }
    /**
     * @return {?}
     */
    ngOnInit() {
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
                template: "<ol *ngIf=\"show\" class=\"breadcrumb\">\n  <li class=\"breadcrumb-item\">\n    <a routerLink=\"/\"><i class=\"fa fa-home\"></i> </a>\n  </li>\n  <li\n    *ngFor=\"let segment of segments; let last = last\"\n    class=\"breadcrumb-item\"\n    [class.active]=\"last\"\n    aria-current=\"page\"\n  >\n    {{ segment | abpLocalization }}\n  </li>\n</ol>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnJlYWRjcnVtYi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDbEQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ3pDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBTyxNQUFNLGNBQWMsQ0FBQztBQU1oRCxNQUFNLE9BQU8sbUJBQW1COzs7OztJQUs5QixZQUFvQixNQUFjLEVBQVUsS0FBWTtRQUFwQyxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUZ4RCxhQUFRLEdBQWEsRUFBRSxDQUFDO1FBR3RCLElBQUksQ0FBQyxJQUFJLEdBQUcsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDLGlCQUFpQixFQUFDLENBQUM7SUFDNUUsQ0FBQzs7OztJQUVELFFBQVE7O2NBQ0EsV0FBVyxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxNQUFNOzs7O1FBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxLQUFLLEVBQUM7O2NBRS9ELFVBQVUsR0FBa0IsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFFBQVEsQ0FBQyxXQUFXLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztRQUNqRyxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxDQUFDLENBQUM7UUFFcEMsSUFBSSxXQUFXLENBQUMsTUFBTSxHQUFHLENBQUMsRUFBRTtrQkFDcEIsQ0FBQyxFQUFFLEdBQUcsR0FBRyxDQUFDLEdBQUcsV0FBVzs7Z0JBRTFCLFVBQVUsR0FBa0IsVUFBVTtZQUMxQyxLQUFLLElBQUksQ0FBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsR0FBRyxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQUUsRUFBRTs7c0JBQzdCLE9BQU8sR0FBRyxHQUFHLENBQUMsQ0FBQyxDQUFDO2dCQUN0QixVQUFVLEdBQUcsVUFBVSxDQUFDLFFBQVEsQ0FBQyxJQUFJOzs7O2dCQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksS0FBSyxPQUFPLEVBQUMsQ0FBQztnQkFFdkUsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLElBQUksQ0FBQyxDQUFDO2FBQ3JDO1NBQ0Y7SUFDSCxDQUFDOzs7WUE5QkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxnQkFBZ0I7Z0JBQzFCLDZXQUEwQzthQUMzQzs7OztZQVBRLE1BQU07WUFDTixLQUFLOzs7O0lBUVosbUNBQWM7O0lBRWQsdUNBQXdCOzs7OztJQUVaLHFDQUFzQjs7Ozs7SUFBRSxvQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUsIEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1icmVhZGNydW1iJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2JyZWFkY3J1bWIuY29tcG9uZW50Lmh0bWwnXG59KVxuZXhwb3J0IGNsYXNzIEJyZWFkY3J1bWJDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBzaG93OiBib29sZWFuO1xuXG4gIHNlZ21lbnRzOiBzdHJpbmdbXSA9IFtdO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7XG4gICAgdGhpcy5zaG93ID0gISF0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KHN0YXRlID0+IHN0YXRlLkxlcHRvbkxheW91dFN0YXRlKTtcbiAgfVxuXG4gIG5nT25Jbml0KCk6IHZvaWQge1xuICAgIGNvbnN0IHNwbGl0dGVkVXJsID0gdGhpcy5yb3V0ZXIudXJsLnNwbGl0KCcvJykuZmlsdGVyKGNodW5rID0+IGNodW5rKTtcblxuICAgIGNvbnN0IGN1cnJlbnRVcmw6IEFCUC5GdWxsUm91dGUgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFJvdXRlKHNwbGl0dGVkVXJsWzBdKSk7XG4gICAgdGhpcy5zZWdtZW50cy5wdXNoKGN1cnJlbnRVcmwubmFtZSk7XG5cbiAgICBpZiAoc3BsaXR0ZWRVcmwubGVuZ3RoID4gMSkge1xuICAgICAgY29uc3QgWywgLi4uYXJyXSA9IHNwbGl0dGVkVXJsO1xuXG4gICAgICBsZXQgY2hpbGRSb3V0ZTogQUJQLkZ1bGxSb3V0ZSA9IGN1cnJlbnRVcmw7XG4gICAgICBmb3IgKGxldCBpID0gMDsgaSA8IGFyci5sZW5ndGg7IGkrKykge1xuICAgICAgICBjb25zdCBlbGVtZW50ID0gYXJyW2ldO1xuICAgICAgICBjaGlsZFJvdXRlID0gY2hpbGRSb3V0ZS5jaGlsZHJlbi5maW5kKGNoaWxkID0+IGNoaWxkLnBhdGggPT09IGVsZW1lbnQpO1xuXG4gICAgICAgIHRoaXMuc2VnbWVudHMucHVzaChjaGlsZFJvdXRlLm5hbWUpO1xuICAgICAgfVxuICAgIH1cbiAgfVxufVxuIl19