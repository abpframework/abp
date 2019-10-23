/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { LazyLoadService } from '@abp/ng.core';
import styles from '../constants/styles';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
var InitialService = /** @class */ (function () {
    function InitialService(lazyLoadService) {
        this.lazyLoadService = lazyLoadService;
        this.appendStyle().subscribe();
    }
    /**
     * @return {?}
     */
    InitialService.prototype.appendStyle = /**
     * @return {?}
     */
    function () {
        return this.lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin');
    };
    InitialService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    InitialService.ctorParameters = function () { return [
        { type: LazyLoadService }
    ]; };
    /** @nocollapse */ InitialService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function InitialService_Factory() { return new InitialService(i0.ɵɵinject(i1.LazyLoadService)); }, token: InitialService, providedIn: "root" });
    return InitialService;
}());
export { InitialService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    InitialService.prototype.lazyLoadService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pbml0aWFsLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUMvQyxPQUFPLE1BQU0sTUFBTSxxQkFBcUIsQ0FBQzs7O0FBRXpDO0lBRUUsd0JBQW9CLGVBQWdDO1FBQWhDLG9CQUFlLEdBQWYsZUFBZSxDQUFpQjtRQUNsRCxJQUFJLENBQUMsV0FBVyxFQUFFLENBQUMsU0FBUyxFQUFFLENBQUM7SUFDakMsQ0FBQzs7OztJQUVELG9DQUFXOzs7SUFBWDtRQUNFLE9BQU8sSUFBSSxDQUFDLGVBQWUsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxFQUFFLFlBQVksQ0FBQyxDQUFDO0lBQ2hGLENBQUM7O2dCQVJGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7Z0JBSHpCLGVBQWU7Ozt5QkFGeEI7Q0FjQyxBQVRELElBU0M7U0FSWSxjQUFjOzs7Ozs7SUFDYix5Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IExhenlMb2FkU2VydmljZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCBzdHlsZXMgZnJvbSAnLi4vY29uc3RhbnRzL3N0eWxlcyc7XHJcblxyXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxyXG5leHBvcnQgY2xhc3MgSW5pdGlhbFNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgbGF6eUxvYWRTZXJ2aWNlOiBMYXp5TG9hZFNlcnZpY2UpIHtcclxuICAgIHRoaXMuYXBwZW5kU3R5bGUoKS5zdWJzY3JpYmUoKTtcclxuICB9XHJcblxyXG4gIGFwcGVuZFN0eWxlKCkge1xyXG4gICAgcmV0dXJuIHRoaXMubGF6eUxvYWRTZXJ2aWNlLmxvYWQobnVsbCwgJ3N0eWxlJywgc3R5bGVzLCAnaGVhZCcsICdhZnRlcmJlZ2luJyk7XHJcbiAgfVxyXG59XHJcbiJdfQ==