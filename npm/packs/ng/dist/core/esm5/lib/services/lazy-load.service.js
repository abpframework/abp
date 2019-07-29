/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { uuid } from '../utils';
import * as i0 from "@angular/core";
var LazyLoadService = /** @class */ (function () {
    function LazyLoadService() {
        this.loadedLibraries = {};
    }
    /**
     * @param {?} url
     * @param {?} type
     * @param {?=} content
     * @param {?=} targetQuery
     * @param {?=} position
     * @return {?}
     */
    LazyLoadService.prototype.load = /**
     * @param {?} url
     * @param {?} type
     * @param {?=} content
     * @param {?=} targetQuery
     * @param {?=} position
     * @return {?}
     */
    function (url, type, content, targetQuery, position) {
        var _this = this;
        if (content === void 0) { content = ''; }
        if (targetQuery === void 0) { targetQuery = 'body'; }
        if (position === void 0) { position = 'afterend'; }
        if (!url && !content)
            return;
        /** @type {?} */
        var key = url ? url.slice(url.lastIndexOf('/') + 1) : uuid();
        if (this.loadedLibraries[key]) {
            return this.loadedLibraries[key].asObservable();
        }
        this.loadedLibraries[key] = new ReplaySubject();
        /** @type {?} */
        var library;
        if (type === 'script') {
            library = document.createElement('script');
            library.type = 'text/javascript';
            if (url) {
                ((/** @type {?} */ (library))).src = url;
            }
            ((/** @type {?} */ (library))).text = content;
        }
        else if (url) {
            library = document.createElement('link');
            library.type = 'text/css';
            ((/** @type {?} */ (library))).rel = 'stylesheet';
            if (url) {
                ((/** @type {?} */ (library))).href = url;
            }
        }
        else {
            library = document.createElement('style');
            ((/** @type {?} */ (library))).textContent = content;
        }
        library.onload = (/**
         * @return {?}
         */
        function () {
            _this.loadedLibraries[key].next();
            _this.loadedLibraries[key].complete();
        });
        document.querySelector(targetQuery).insertAdjacentElement(position, library);
        return this.loadedLibraries[key].asObservable();
    };
    LazyLoadService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */ LazyLoadService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function LazyLoadService_Factory() { return new LazyLoadService(); }, token: LazyLoadService, providedIn: "root" });
    return LazyLoadService;
}());
export { LazyLoadService };
if (false) {
    /** @type {?} */
    LazyLoadService.prototype.loadedLibraries;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF6eS1sb2FkLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbGF6eS1sb2FkLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBVSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUFjLGFBQWEsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqRCxPQUFPLEVBQUUsSUFBSSxFQUFFLE1BQU0sVUFBVSxDQUFDOztBQUVoQztJQUFBO1FBSUUsb0JBQWUsR0FBMkMsRUFBRSxDQUFDO0tBaUQ5RDs7Ozs7Ozs7O0lBL0NDLDhCQUFJOzs7Ozs7OztJQUFKLFVBQ0UsR0FBVyxFQUNYLElBQXdCLEVBQ3hCLE9BQW9CLEVBQ3BCLFdBQTRCLEVBQzVCLFFBQXFDO1FBTHZDLGlCQThDQztRQTNDQyx3QkFBQSxFQUFBLFlBQW9CO1FBQ3BCLDRCQUFBLEVBQUEsb0JBQTRCO1FBQzVCLHlCQUFBLEVBQUEscUJBQXFDO1FBRXJDLElBQUksQ0FBQyxHQUFHLElBQUksQ0FBQyxPQUFPO1lBQUUsT0FBTzs7WUFDdkIsR0FBRyxHQUFHLEdBQUcsQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsV0FBVyxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUU7UUFFOUQsSUFBSSxJQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxFQUFFO1lBQzdCLE9BQU8sSUFBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxZQUFZLEVBQUUsQ0FBQztTQUNqRDtRQUVELElBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxhQUFhLEVBQUUsQ0FBQzs7WUFFNUMsT0FBTztRQUNYLElBQUksSUFBSSxLQUFLLFFBQVEsRUFBRTtZQUNyQixPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxRQUFRLENBQUMsQ0FBQztZQUMzQyxPQUFPLENBQUMsSUFBSSxHQUFHLGlCQUFpQixDQUFDO1lBQ2pDLElBQUksR0FBRyxFQUFFO2dCQUNQLENBQUMsbUJBQUEsT0FBTyxFQUFxQixDQUFDLENBQUMsR0FBRyxHQUFHLEdBQUcsQ0FBQzthQUMxQztZQUVELENBQUMsbUJBQUEsT0FBTyxFQUFxQixDQUFDLENBQUMsSUFBSSxHQUFHLE9BQU8sQ0FBQztTQUMvQzthQUFNLElBQUksR0FBRyxFQUFFO1lBQ2QsT0FBTyxHQUFHLFFBQVEsQ0FBQyxhQUFhLENBQUMsTUFBTSxDQUFDLENBQUM7WUFDekMsT0FBTyxDQUFDLElBQUksR0FBRyxVQUFVLENBQUM7WUFDMUIsQ0FBQyxtQkFBQSxPQUFPLEVBQW1CLENBQUMsQ0FBQyxHQUFHLEdBQUcsWUFBWSxDQUFDO1lBRWhELElBQUksR0FBRyxFQUFFO2dCQUNQLENBQUMsbUJBQUEsT0FBTyxFQUFtQixDQUFDLENBQUMsSUFBSSxHQUFHLEdBQUcsQ0FBQzthQUN6QztTQUNGO2FBQU07WUFDTCxPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxPQUFPLENBQUMsQ0FBQztZQUMxQyxDQUFDLG1CQUFBLE9BQU8sRUFBb0IsQ0FBQyxDQUFDLFdBQVcsR0FBRyxPQUFPLENBQUM7U0FDckQ7UUFFRCxPQUFPLENBQUMsTUFBTTs7O1FBQUc7WUFDZixLQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxDQUFDLElBQUksRUFBRSxDQUFDO1lBQ2pDLEtBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLENBQUMsUUFBUSxFQUFFLENBQUM7UUFDdkMsQ0FBQyxDQUFBLENBQUM7UUFFRixRQUFRLENBQUMsYUFBYSxDQUFDLFdBQVcsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLFFBQVEsRUFBRSxPQUFPLENBQUMsQ0FBQztRQUU3RSxPQUFPLElBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLENBQUMsWUFBWSxFQUFFLENBQUM7SUFDbEQsQ0FBQzs7Z0JBcERGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7OzswQkFORDtDQXlEQyxBQXJERCxJQXFEQztTQWxEWSxlQUFlOzs7SUFDMUIsMENBQTZEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0LCBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlLCBSZXBsYXlTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB1dWlkIH0gZnJvbSAnLi4vdXRpbHMnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgTGF6eUxvYWRTZXJ2aWNlIHtcbiAgbG9hZGVkTGlicmFyaWVzOiB7IFt1cmw6IHN0cmluZ106IFJlcGxheVN1YmplY3Q8dm9pZD4gfSA9IHt9O1xuXG4gIGxvYWQoXG4gICAgdXJsOiBzdHJpbmcsXG4gICAgdHlwZTogJ3NjcmlwdCcgfCAnc3R5bGUnLFxuICAgIGNvbnRlbnQ6IHN0cmluZyA9ICcnLFxuICAgIHRhcmdldFF1ZXJ5OiBzdHJpbmcgPSAnYm9keScsXG4gICAgcG9zaXRpb246IEluc2VydFBvc2l0aW9uID0gJ2FmdGVyZW5kJyxcbiAgKTogT2JzZXJ2YWJsZTx2b2lkPiB7XG4gICAgaWYgKCF1cmwgJiYgIWNvbnRlbnQpIHJldHVybjtcbiAgICBjb25zdCBrZXkgPSB1cmwgPyB1cmwuc2xpY2UodXJsLmxhc3RJbmRleE9mKCcvJykgKyAxKSA6IHV1aWQoKTtcblxuICAgIGlmICh0aGlzLmxvYWRlZExpYnJhcmllc1trZXldKSB7XG4gICAgICByZXR1cm4gdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XS5hc09ic2VydmFibGUoKTtcbiAgICB9XG5cbiAgICB0aGlzLmxvYWRlZExpYnJhcmllc1trZXldID0gbmV3IFJlcGxheVN1YmplY3QoKTtcblxuICAgIGxldCBsaWJyYXJ5O1xuICAgIGlmICh0eXBlID09PSAnc2NyaXB0Jykge1xuICAgICAgbGlicmFyeSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ3NjcmlwdCcpO1xuICAgICAgbGlicmFyeS50eXBlID0gJ3RleHQvamF2YXNjcmlwdCc7XG4gICAgICBpZiAodXJsKSB7XG4gICAgICAgIChsaWJyYXJ5IGFzIEhUTUxTY3JpcHRFbGVtZW50KS5zcmMgPSB1cmw7XG4gICAgICB9XG5cbiAgICAgIChsaWJyYXJ5IGFzIEhUTUxTY3JpcHRFbGVtZW50KS50ZXh0ID0gY29udGVudDtcbiAgICB9IGVsc2UgaWYgKHVybCkge1xuICAgICAgbGlicmFyeSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2xpbmsnKTtcbiAgICAgIGxpYnJhcnkudHlwZSA9ICd0ZXh0L2Nzcyc7XG4gICAgICAobGlicmFyeSBhcyBIVE1MTGlua0VsZW1lbnQpLnJlbCA9ICdzdHlsZXNoZWV0JztcblxuICAgICAgaWYgKHVybCkge1xuICAgICAgICAobGlicmFyeSBhcyBIVE1MTGlua0VsZW1lbnQpLmhyZWYgPSB1cmw7XG4gICAgICB9XG4gICAgfSBlbHNlIHtcbiAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdzdHlsZScpO1xuICAgICAgKGxpYnJhcnkgYXMgSFRNTFN0eWxlRWxlbWVudCkudGV4dENvbnRlbnQgPSBjb250ZW50O1xuICAgIH1cblxuICAgIGxpYnJhcnkub25sb2FkID0gKCkgPT4ge1xuICAgICAgdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XS5uZXh0KCk7XG4gICAgICB0aGlzLmxvYWRlZExpYnJhcmllc1trZXldLmNvbXBsZXRlKCk7XG4gICAgfTtcblxuICAgIGRvY3VtZW50LnF1ZXJ5U2VsZWN0b3IodGFyZ2V0UXVlcnkpLmluc2VydEFkamFjZW50RWxlbWVudChwb3NpdGlvbiwgbGlicmFyeSk7XG5cbiAgICByZXR1cm4gdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XS5hc09ic2VydmFibGUoKTtcbiAgfVxufVxuIl19