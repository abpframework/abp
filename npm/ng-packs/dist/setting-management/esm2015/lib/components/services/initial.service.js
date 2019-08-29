/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import snq from 'snq';
export class InitialService {
    /**
     * @param {?} router
     */
    constructor(router) {
        this.router = router;
        this.settings = this.router.config
            .map((/**
         * @param {?} route
         * @return {?}
         */
        route => snq((/**
         * @return {?}
         */
        () => route.data.routes.settings))))
            .filter((/**
         * @param {?} settings
         * @return {?}
         */
        settings => settings && settings.length))
            .reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        (acc, val) => [...acc, ...val]), [])
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        (a, b) => a.order - b.order));
    }
}
InitialService.decorators = [
    { type: Injectable }
];
/** @nocollapse */
InitialService.ctorParameters = () => [
    { type: Router }
];
if (false) {
    /** @type {?} */
    InitialService.prototype.settings;
    /**
     * @type {?}
     * @private
     */
    InitialService.prototype.router;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9zZXJ2aWNlcy9pbml0aWFsLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUNBLE9BQU8sRUFBRSxVQUFVLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ3pDLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUd0QixNQUFNLE9BQU8sY0FBYzs7OztJQUd6QixZQUFvQixNQUFjO1FBQWQsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUNoQyxJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTTthQUMvQixHQUFHOzs7O1FBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxHQUFHOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxRQUFRLEVBQUMsRUFBQzthQUNuRCxNQUFNOzs7O1FBQUMsUUFBUSxDQUFDLEVBQUUsQ0FBQyxRQUFRLElBQUksUUFBUSxDQUFDLE1BQU0sRUFBQzthQUMvQyxNQUFNOzs7OztRQUFDLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxFQUFFLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEdBQUcsQ0FBQyxHQUFFLEVBQUUsQ0FBQzthQUMxQyxJQUFJOzs7OztRQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFDLENBQUM7SUFDdkMsQ0FBQzs7O1lBVkYsVUFBVTs7OztZQUhGLE1BQU07Ozs7SUFLYixrQ0FBOEI7Ozs7O0lBRWxCLGdDQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFNldHRpbmdUYWIgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBJbmplY3RhYmxlLCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBJbml0aWFsU2VydmljZSB7XG4gIHB1YmxpYyBzZXR0aW5nczogU2V0dGluZ1RhYltdO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHtcbiAgICB0aGlzLnNldHRpbmdzID0gdGhpcy5yb3V0ZXIuY29uZmlnXG4gICAgICAubWFwKHJvdXRlID0+IHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5zZXR0aW5ncykpXG4gICAgICAuZmlsdGVyKHNldHRpbmdzID0+IHNldHRpbmdzICYmIHNldHRpbmdzLmxlbmd0aClcbiAgICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi52YWxdLCBbXSlcbiAgICAgIC5zb3J0KChhLCBiKSA9PiBhLm9yZGVyIC0gYi5vcmRlcik7XG4gIH1cbn1cbiJdfQ==