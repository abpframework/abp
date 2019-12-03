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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnJlYWRjcnVtYi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDbEQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ3pDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBTyxNQUFNLGNBQWMsQ0FBQztBQU1oRCxNQUFNLE9BQU8sbUJBQW1COzs7OztJQUs5QixZQUFvQixNQUFjLEVBQVUsS0FBWTtRQUFwQyxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUZ4RCxhQUFRLEdBQWEsRUFBRSxDQUFDO0lBRW1DLENBQUM7Ozs7SUFFNUQsUUFBUTtRQUNOLElBQUksQ0FBQyxJQUFJLEdBQUcsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDLGlCQUFpQixFQUFDLENBQUM7UUFDMUUsSUFBSSxJQUFJLENBQUMsSUFBSSxFQUFFOztnQkFDVCxXQUFXLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxDQUFDLE1BQU07Ozs7WUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssRUFBQzs7Z0JBRS9ELFVBQVUsR0FBa0IsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFFBQVEsQ0FBQyxXQUFXLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztZQUUvRixJQUFJLENBQUMsVUFBVSxFQUFFO2dCQUNmLFVBQVUsR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsUUFBUSxDQUFDLElBQUksRUFBRSxJQUFJLEVBQUUsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO2dCQUMxRixXQUFXLEdBQUcsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxDQUFDO2dCQUNoQyxJQUFJLENBQUMsVUFBVSxFQUFFO29CQUNmLElBQUksQ0FBQyxJQUFJLEdBQUcsS0FBSyxDQUFDO29CQUNsQixPQUFPO2lCQUNSO2FBQ0Y7WUFFRCxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxDQUFDLENBQUM7WUFFcEMsSUFBSSxXQUFXLENBQUMsTUFBTSxHQUFHLENBQUMsRUFBRTtzQkFDcEIsQ0FBQyxFQUFFLEdBQUcsR0FBRyxDQUFDLEdBQUcsV0FBVzs7b0JBRTFCLFVBQVUsR0FBa0IsVUFBVTtnQkFDMUMsS0FBSyxJQUFJLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFHLEdBQUcsQ0FBQyxNQUFNLEVBQUUsQ0FBQyxFQUFFLEVBQUU7OzBCQUM3QixPQUFPLEdBQUcsR0FBRyxDQUFDLENBQUMsQ0FBQztvQkFDdEIsVUFBVSxHQUFHLFVBQVUsQ0FBQyxRQUFRLENBQUMsSUFBSTs7OztvQkFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxJQUFJLEtBQUssT0FBTyxFQUFDLENBQUM7b0JBRXZFLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsQ0FBQztpQkFDckM7YUFDRjtTQUNGO0lBQ0gsQ0FBQzs7O1lBekNGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO2dCQUMxQiw2V0FBMEM7YUFDM0M7Ozs7WUFQUSxNQUFNO1lBQ04sS0FBSzs7OztJQVFaLG1DQUFjOztJQUVkLHVDQUF3Qjs7Ozs7SUFFWixxQ0FBc0I7Ozs7O0lBQUUsb0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlLCBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtYnJlYWRjcnVtYicsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2JyZWFkY3J1bWIuY29tcG9uZW50Lmh0bWwnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQnJlYWRjcnVtYkNvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XHJcbiAgc2hvdzogYm9vbGVhbjtcclxuXHJcbiAgc2VnbWVudHM6IHN0cmluZ1tdID0gW107XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpOiB2b2lkIHtcclxuICAgIHRoaXMuc2hvdyA9ICEhdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChzdGF0ZSA9PiBzdGF0ZS5MZXB0b25MYXlvdXRTdGF0ZSk7XHJcbiAgICBpZiAodGhpcy5zaG93KSB7XHJcbiAgICAgIGxldCBzcGxpdHRlZFVybCA9IHRoaXMucm91dGVyLnVybC5zcGxpdCgnLycpLmZpbHRlcihjaHVuayA9PiBjaHVuayk7XHJcblxyXG4gICAgICBsZXQgY3VycmVudFVybDogQUJQLkZ1bGxSb3V0ZSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0Um91dGUoc3BsaXR0ZWRVcmxbMF0pKTtcclxuXHJcbiAgICAgIGlmICghY3VycmVudFVybCkge1xyXG4gICAgICAgIGN1cnJlbnRVcmwgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFJvdXRlKG51bGwsIG51bGwsIHRoaXMucm91dGVyLnVybCkpO1xyXG4gICAgICAgIHNwbGl0dGVkVXJsID0gW3RoaXMucm91dGVyLnVybF07XHJcbiAgICAgICAgaWYgKCFjdXJyZW50VXJsKSB7XHJcbiAgICAgICAgICB0aGlzLnNob3cgPSBmYWxzZTtcclxuICAgICAgICAgIHJldHVybjtcclxuICAgICAgICB9XHJcbiAgICAgIH1cclxuXHJcbiAgICAgIHRoaXMuc2VnbWVudHMucHVzaChjdXJyZW50VXJsLm5hbWUpO1xyXG5cclxuICAgICAgaWYgKHNwbGl0dGVkVXJsLmxlbmd0aCA+IDEpIHtcclxuICAgICAgICBjb25zdCBbLCAuLi5hcnJdID0gc3BsaXR0ZWRVcmw7XHJcblxyXG4gICAgICAgIGxldCBjaGlsZFJvdXRlOiBBQlAuRnVsbFJvdXRlID0gY3VycmVudFVybDtcclxuICAgICAgICBmb3IgKGxldCBpID0gMDsgaSA8IGFyci5sZW5ndGg7IGkrKykge1xyXG4gICAgICAgICAgY29uc3QgZWxlbWVudCA9IGFycltpXTtcclxuICAgICAgICAgIGNoaWxkUm91dGUgPSBjaGlsZFJvdXRlLmNoaWxkcmVuLmZpbmQoY2hpbGQgPT4gY2hpbGQucGF0aCA9PT0gZWxlbWVudCk7XHJcblxyXG4gICAgICAgICAgdGhpcy5zZWdtZW50cy5wdXNoKGNoaWxkUm91dGUubmFtZSk7XHJcbiAgICAgICAgfVxyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==