/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/initial.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { LazyLoadService } from '@abp/ng.core';
import styles from '../constants/styles';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
export class InitialService {
    /**
     * @param {?} lazyLoadService
     */
    constructor(lazyLoadService) {
        this.lazyLoadService = lazyLoadService;
        this.appendStyle().subscribe();
    }
    /**
     * @return {?}
     */
    appendStyle() {
        return this.lazyLoadService.load(null, 'style', styles, 'head', 'beforeend');
    }
}
InitialService.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */
InitialService.ctorParameters = () => [
    { type: LazyLoadService }
];
/** @nocollapse */ InitialService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function InitialService_Factory() { return new InitialService(i0.ɵɵinject(i1.LazyLoadService)); }, token: InitialService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    InitialService.prototype.lazyLoadService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pbml0aWFsLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRTNDLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDL0MsT0FBTyxNQUFNLE1BQU0scUJBQXFCLENBQUM7OztBQUd6QyxNQUFNLE9BQU8sY0FBYzs7OztJQUN6QixZQUFvQixlQUFnQztRQUFoQyxvQkFBZSxHQUFmLGVBQWUsQ0FBaUI7UUFDbEQsSUFBSSxDQUFDLFdBQVcsRUFBRSxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQ2pDLENBQUM7Ozs7SUFFRCxXQUFXO1FBQ1QsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUUsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLEVBQUUsV0FBVyxDQUFDLENBQUM7SUFDL0UsQ0FBQzs7O1lBUkYsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRTs7OztZQUh6QixlQUFlOzs7Ozs7OztJQUtWLHlDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBMYXp5TG9hZFNlcnZpY2UgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHN0eWxlcyBmcm9tICcuLi9jb25zdGFudHMvc3R5bGVzJztcblxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcbmV4cG9ydCBjbGFzcyBJbml0aWFsU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgbGF6eUxvYWRTZXJ2aWNlOiBMYXp5TG9hZFNlcnZpY2UpIHtcbiAgICB0aGlzLmFwcGVuZFN0eWxlKCkuc3Vic2NyaWJlKCk7XG4gIH1cblxuICBhcHBlbmRTdHlsZSgpIHtcbiAgICByZXR1cm4gdGhpcy5sYXp5TG9hZFNlcnZpY2UubG9hZChudWxsLCAnc3R5bGUnLCBzdHlsZXMsICdoZWFkJywgJ2JlZm9yZWVuZCcpO1xuICB9XG59XG4iXX0=