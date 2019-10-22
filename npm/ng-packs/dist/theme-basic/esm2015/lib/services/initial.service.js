/**
 * @fileoverview added by tsickle
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
        return this.lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin');
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9pbml0aWFsLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUMvQyxPQUFPLE1BQU0sTUFBTSxxQkFBcUIsQ0FBQzs7O0FBR3pDLE1BQU0sT0FBTyxjQUFjOzs7O0lBQ3pCLFlBQW9CLGVBQWdDO1FBQWhDLG9CQUFlLEdBQWYsZUFBZSxDQUFpQjtRQUNsRCxJQUFJLENBQUMsV0FBVyxFQUFFLENBQUMsU0FBUyxFQUFFLENBQUM7SUFDakMsQ0FBQzs7OztJQUVELFdBQVc7UUFDVCxPQUFPLElBQUksQ0FBQyxlQUFlLENBQUMsSUFBSSxDQUFDLElBQUksRUFBRSxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sRUFBRSxZQUFZLENBQUMsQ0FBQztJQUNoRixDQUFDOzs7WUFSRixVQUFVLFNBQUMsRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFOzs7O1lBSHpCLGVBQWU7Ozs7Ozs7O0lBS1YseUNBQXdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xyXG5pbXBvcnQgeyBMYXp5TG9hZFNlcnZpY2UgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgc3R5bGVzIGZyb20gJy4uL2NvbnN0YW50cy9zdHlsZXMnO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIEluaXRpYWxTZXJ2aWNlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGxhenlMb2FkU2VydmljZTogTGF6eUxvYWRTZXJ2aWNlKSB7XHJcbiAgICB0aGlzLmFwcGVuZFN0eWxlKCkuc3Vic2NyaWJlKCk7XHJcbiAgfVxyXG5cclxuICBhcHBlbmRTdHlsZSgpIHtcclxuICAgIHJldHVybiB0aGlzLmxhenlMb2FkU2VydmljZS5sb2FkKG51bGwsICdzdHlsZScsIHN0eWxlcywgJ2hlYWQnLCAnYWZ0ZXJiZWdpbicpO1xyXG4gIH1cclxufVxyXG4iXX0=