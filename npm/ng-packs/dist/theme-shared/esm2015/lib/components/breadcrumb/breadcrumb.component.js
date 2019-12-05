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
        if (this.show) {
            /** @type {?} */
            let splittedUrl = this.router.url.split('/').filter((/**
             * @param {?} chunk
             * @return {?}
             */
            chunk => chunk));
            /** @type {?} */
            let currentUrl = this.store.selectSnapshot(ConfigState.getRoute(splittedUrl[0]));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnJlYWRjcnVtYi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQU8sTUFBTSxjQUFjLENBQUM7QUFNaEQsTUFBTSxPQUFPLG1CQUFtQjs7Ozs7SUFLOUIsWUFBb0IsTUFBYyxFQUFVLEtBQVk7UUFBcEMsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFGeEQsYUFBUSxHQUFhLEVBQUUsQ0FBQztJQUVtQyxDQUFDOzs7O0lBRTVELFFBQVE7UUFDTixJQUFJLENBQUMsSUFBSSxHQUFHLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWM7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsRUFBQyxDQUFDO1FBQzFFLElBQUksSUFBSSxDQUFDLElBQUksRUFBRTs7Z0JBQ1QsV0FBVyxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxNQUFNOzs7O1lBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxLQUFLLEVBQUM7O2dCQUUvRCxVQUFVLEdBQWtCLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7WUFFL0YsSUFBSSxDQUFDLFVBQVUsRUFBRTtnQkFDZixVQUFVLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUUsSUFBSSxFQUFFLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztnQkFDMUYsV0FBVyxHQUFHLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQztnQkFDaEMsSUFBSSxDQUFDLFVBQVUsRUFBRTtvQkFDZixJQUFJLENBQUMsSUFBSSxHQUFHLEtBQUssQ0FBQztvQkFDbEIsT0FBTztpQkFDUjthQUNGO1lBRUQsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLElBQUksQ0FBQyxDQUFDO1lBRXBDLElBQUksV0FBVyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7c0JBQ3BCLENBQUMsRUFBRSxHQUFHLEdBQUcsQ0FBQyxHQUFHLFdBQVc7O29CQUUxQixVQUFVLEdBQWtCLFVBQVU7Z0JBQzFDLEtBQUssSUFBSSxDQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxHQUFHLENBQUMsTUFBTSxFQUFFLENBQUMsRUFBRSxFQUFFOzswQkFDN0IsT0FBTyxHQUFHLEdBQUcsQ0FBQyxDQUFDLENBQUM7b0JBQ3RCLFVBQVUsR0FBRyxVQUFVLENBQUMsUUFBUSxDQUFDLElBQUk7Ozs7b0JBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUMsSUFBSSxLQUFLLE9BQU8sRUFBQyxDQUFDO29CQUV2RSxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxDQUFDLENBQUM7aUJBQ3JDO2FBQ0Y7U0FDRjtJQUNILENBQUM7OztZQXpDRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtnQkFDMUIsNldBQTBDO2FBQzNDOzs7O1lBUFEsTUFBTTtZQUNOLEtBQUs7Ozs7SUFRWixtQ0FBYzs7SUFFZCx1Q0FBd0I7Ozs7O0lBRVoscUNBQXNCOzs7OztJQUFFLG9DQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWJyZWFkY3J1bWInLFxuICB0ZW1wbGF0ZVVybDogJy4vYnJlYWRjcnVtYi5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIEJyZWFkY3J1bWJDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBzaG93OiBib29sZWFuO1xuXG4gIHNlZ21lbnRzOiBzdHJpbmdbXSA9IFtdO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIG5nT25Jbml0KCk6IHZvaWQge1xuICAgIHRoaXMuc2hvdyA9ICEhdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChzdGF0ZSA9PiBzdGF0ZS5MZXB0b25MYXlvdXRTdGF0ZSk7XG4gICAgaWYgKHRoaXMuc2hvdykge1xuICAgICAgbGV0IHNwbGl0dGVkVXJsID0gdGhpcy5yb3V0ZXIudXJsLnNwbGl0KCcvJykuZmlsdGVyKGNodW5rID0+IGNodW5rKTtcblxuICAgICAgbGV0IGN1cnJlbnRVcmw6IEFCUC5GdWxsUm91dGUgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFJvdXRlKHNwbGl0dGVkVXJsWzBdKSk7XG5cbiAgICAgIGlmICghY3VycmVudFVybCkge1xuICAgICAgICBjdXJyZW50VXJsID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRSb3V0ZShudWxsLCBudWxsLCB0aGlzLnJvdXRlci51cmwpKTtcbiAgICAgICAgc3BsaXR0ZWRVcmwgPSBbdGhpcy5yb3V0ZXIudXJsXTtcbiAgICAgICAgaWYgKCFjdXJyZW50VXJsKSB7XG4gICAgICAgICAgdGhpcy5zaG93ID0gZmFsc2U7XG4gICAgICAgICAgcmV0dXJuO1xuICAgICAgICB9XG4gICAgICB9XG5cbiAgICAgIHRoaXMuc2VnbWVudHMucHVzaChjdXJyZW50VXJsLm5hbWUpO1xuXG4gICAgICBpZiAoc3BsaXR0ZWRVcmwubGVuZ3RoID4gMSkge1xuICAgICAgICBjb25zdCBbLCAuLi5hcnJdID0gc3BsaXR0ZWRVcmw7XG5cbiAgICAgICAgbGV0IGNoaWxkUm91dGU6IEFCUC5GdWxsUm91dGUgPSBjdXJyZW50VXJsO1xuICAgICAgICBmb3IgKGxldCBpID0gMDsgaSA8IGFyci5sZW5ndGg7IGkrKykge1xuICAgICAgICAgIGNvbnN0IGVsZW1lbnQgPSBhcnJbaV07XG4gICAgICAgICAgY2hpbGRSb3V0ZSA9IGNoaWxkUm91dGUuY2hpbGRyZW4uZmluZChjaGlsZCA9PiBjaGlsZC5wYXRoID09PSBlbGVtZW50KTtcblxuICAgICAgICAgIHRoaXMuc2VnbWVudHMucHVzaChjaGlsZFJvdXRlLm5hbWUpO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgfVxuICB9XG59XG4iXX0=