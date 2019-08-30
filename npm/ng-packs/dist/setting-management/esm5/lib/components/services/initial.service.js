/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import snq from 'snq';
var InitialService = /** @class */ (function () {
    function InitialService(router) {
        this.router = router;
        this.settings = this.router.config
            .map((/**
         * @param {?} route
         * @return {?}
         */
        function (route) { return snq((/**
         * @return {?}
         */
        function () { return route.data.routes.settings; })); }))
            .filter((/**
         * @param {?} settings
         * @return {?}
         */
        function (settings) { return settings && settings.length; }))
            .reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        function (acc, val) { return tslib_1.__spread(acc, val); }), [])
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        function (a, b) { return a.order - b.order; }));
    }
    InitialService.decorators = [
        { type: Injectable }
    ];
    /** @nocollapse */
    InitialService.ctorParameters = function () { return [
        { type: Router }
    ]; };
    return InitialService;
}());
export { InitialService };
if (false) {
    /** @type {?} */
    InitialService.prototype.settings;
    /**
     * @type {?}
     * @private
     */
    InitialService.prototype.router;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9zZXJ2aWNlcy9pbml0aWFsLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFDQSxPQUFPLEVBQUUsVUFBVSxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ25ELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFFdEI7SUFJRSx3QkFBb0IsTUFBYztRQUFkLFdBQU0sR0FBTixNQUFNLENBQVE7UUFDaEMsSUFBSSxDQUFDLFFBQVEsR0FBRyxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07YUFDL0IsR0FBRzs7OztRQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsR0FBRzs7O1FBQUMsY0FBTSxPQUFBLEtBQUssQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLFFBQVEsRUFBMUIsQ0FBMEIsRUFBQyxFQUFyQyxDQUFxQyxFQUFDO2FBQ25ELE1BQU07Ozs7UUFBQyxVQUFBLFFBQVEsSUFBSSxPQUFBLFFBQVEsSUFBSSxRQUFRLENBQUMsTUFBTSxFQUEzQixDQUEyQixFQUFDO2FBQy9DLE1BQU07Ozs7O1FBQUMsVUFBQyxHQUFHLEVBQUUsR0FBRyxJQUFLLHdCQUFJLEdBQUcsRUFBSyxHQUFHLEdBQWYsQ0FBZ0IsR0FBRSxFQUFFLENBQUM7YUFDMUMsSUFBSTs7Ozs7UUFBQyxVQUFDLENBQUMsRUFBRSxDQUFDLElBQUssT0FBQSxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxLQUFLLEVBQWpCLENBQWlCLEVBQUMsQ0FBQztJQUN2QyxDQUFDOztnQkFWRixVQUFVOzs7O2dCQUhGLE1BQU07O0lBY2YscUJBQUM7Q0FBQSxBQVhELElBV0M7U0FWWSxjQUFjOzs7SUFDekIsa0NBQThCOzs7OztJQUVsQixnQ0FBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTZXR0aW5nVGFiIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgSW5qZWN0YWJsZSwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuXG5ASW5qZWN0YWJsZSgpXG5leHBvcnQgY2xhc3MgSW5pdGlhbFNlcnZpY2Uge1xuICBwdWJsaWMgc2V0dGluZ3M6IFNldHRpbmdUYWJbXTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyKSB7XG4gICAgdGhpcy5zZXR0aW5ncyA9IHRoaXMucm91dGVyLmNvbmZpZ1xuICAgICAgLm1hcChyb3V0ZSA9PiBzbnEoKCkgPT4gcm91dGUuZGF0YS5yb3V0ZXMuc2V0dGluZ3MpKVxuICAgICAgLmZpbHRlcihzZXR0aW5ncyA9PiBzZXR0aW5ncyAmJiBzZXR0aW5ncy5sZW5ndGgpXG4gICAgICAucmVkdWNlKChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4udmFsXSwgW10pXG4gICAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpO1xuICB9XG59XG4iXX0=