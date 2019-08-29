/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                template: `
    <ol *ngIf="show" class="breadcrumb">
      <li class="breadcrumb-item">
        <a routerLink="/"><i class="fa fa-home"></i> </a>
      </li>
      <li
        *ngFor="let segment of segments; let last = last"
        class="breadcrumb-item"
        [class.active]="last"
        aria-current="page"
      >
        {{ segment | abpLocalization }}
      </li>
    </ol>
  `
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnJlYWRjcnVtYi5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2JyZWFkY3J1bWIvYnJlYWRjcnVtYi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDbEQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ3pDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBTyxNQUFNLGNBQWMsQ0FBQztBQW9CaEQsTUFBTSxPQUFPLG1CQUFtQjs7Ozs7SUFLOUIsWUFBb0IsTUFBYyxFQUFVLEtBQVk7UUFBcEMsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFGeEQsYUFBUSxHQUFhLEVBQUUsQ0FBQztRQUd0QixJQUFJLENBQUMsSUFBSSxHQUFHLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWM7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsRUFBQyxDQUFDO0lBQzVFLENBQUM7Ozs7SUFFRCxRQUFROztjQUNBLFdBQVcsR0FBRyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLENBQUMsTUFBTTs7OztRQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsS0FBSyxFQUFDOztjQUUvRCxVQUFVLEdBQWtCLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7UUFDakcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLElBQUksQ0FBQyxDQUFDO1FBRXBDLElBQUksV0FBVyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7a0JBQ3BCLENBQUMsRUFBRSxHQUFHLEdBQUcsQ0FBQyxHQUFHLFdBQVc7O2dCQUUxQixVQUFVLEdBQWtCLFVBQVU7WUFDMUMsS0FBSyxJQUFJLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFHLEdBQUcsQ0FBQyxNQUFNLEVBQUUsQ0FBQyxFQUFFLEVBQUU7O3NCQUM3QixPQUFPLEdBQUcsR0FBRyxDQUFDLENBQUMsQ0FBQztnQkFDdEIsVUFBVSxHQUFHLFVBQVUsQ0FBQyxRQUFRLENBQUMsSUFBSTs7OztnQkFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxJQUFJLEtBQUssT0FBTyxFQUFDLENBQUM7Z0JBRXZFLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLENBQUMsQ0FBQzthQUNyQztTQUNGO0lBQ0gsQ0FBQzs7O1lBNUNGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO2dCQUMxQixRQUFRLEVBQUU7Ozs7Ozs7Ozs7Ozs7O0dBY1Q7YUFDRjs7OztZQXJCUSxNQUFNO1lBQ04sS0FBSzs7OztJQXNCWixtQ0FBYzs7SUFFZCx1Q0FBd0I7Ozs7O0lBRVoscUNBQXNCOzs7OztJQUFFLG9DQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWJyZWFkY3J1bWInLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxvbCAqbmdJZj1cInNob3dcIiBjbGFzcz1cImJyZWFkY3J1bWJcIj5cbiAgICAgIDxsaSBjbGFzcz1cImJyZWFkY3J1bWItaXRlbVwiPlxuICAgICAgICA8YSByb3V0ZXJMaW5rPVwiL1wiPjxpIGNsYXNzPVwiZmEgZmEtaG9tZVwiPjwvaT4gPC9hPlxuICAgICAgPC9saT5cbiAgICAgIDxsaVxuICAgICAgICAqbmdGb3I9XCJsZXQgc2VnbWVudCBvZiBzZWdtZW50czsgbGV0IGxhc3QgPSBsYXN0XCJcbiAgICAgICAgY2xhc3M9XCJicmVhZGNydW1iLWl0ZW1cIlxuICAgICAgICBbY2xhc3MuYWN0aXZlXT1cImxhc3RcIlxuICAgICAgICBhcmlhLWN1cnJlbnQ9XCJwYWdlXCJcbiAgICAgID5cbiAgICAgICAge3sgc2VnbWVudCB8IGFicExvY2FsaXphdGlvbiB9fVxuICAgICAgPC9saT5cbiAgICA8L29sPlxuICBgLFxufSlcbmV4cG9ydCBjbGFzcyBCcmVhZGNydW1iQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcbiAgc2hvdzogYm9vbGVhbjtcblxuICBzZWdtZW50czogc3RyaW5nW10gPSBbXTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge1xuICAgIHRoaXMuc2hvdyA9ICEhdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChzdGF0ZSA9PiBzdGF0ZS5MZXB0b25MYXlvdXRTdGF0ZSk7XG4gIH1cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICBjb25zdCBzcGxpdHRlZFVybCA9IHRoaXMucm91dGVyLnVybC5zcGxpdCgnLycpLmZpbHRlcihjaHVuayA9PiBjaHVuayk7XG5cbiAgICBjb25zdCBjdXJyZW50VXJsOiBBQlAuRnVsbFJvdXRlID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRSb3V0ZShzcGxpdHRlZFVybFswXSkpO1xuICAgIHRoaXMuc2VnbWVudHMucHVzaChjdXJyZW50VXJsLm5hbWUpO1xuXG4gICAgaWYgKHNwbGl0dGVkVXJsLmxlbmd0aCA+IDEpIHtcbiAgICAgIGNvbnN0IFssIC4uLmFycl0gPSBzcGxpdHRlZFVybDtcblxuICAgICAgbGV0IGNoaWxkUm91dGU6IEFCUC5GdWxsUm91dGUgPSBjdXJyZW50VXJsO1xuICAgICAgZm9yIChsZXQgaSA9IDA7IGkgPCBhcnIubGVuZ3RoOyBpKyspIHtcbiAgICAgICAgY29uc3QgZWxlbWVudCA9IGFycltpXTtcbiAgICAgICAgY2hpbGRSb3V0ZSA9IGNoaWxkUm91dGUuY2hpbGRyZW4uZmluZChjaGlsZCA9PiBjaGlsZC5wYXRoID09PSBlbGVtZW50KTtcblxuICAgICAgICB0aGlzLnNlZ21lbnRzLnB1c2goY2hpbGRSb3V0ZS5uYW1lKTtcbiAgICAgIH1cbiAgICB9XG4gIH1cbn1cbiJdfQ==