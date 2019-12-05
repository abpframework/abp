/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/lazy-load.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, throwError } from 'rxjs';
import { uuid } from '../utils';
import * as i0 from "@angular/core";
var LazyLoadService = /** @class */ (function () {
    function LazyLoadService() {
        this.loadedLibraries = {};
    }
    /**
     * @param {?} urlOrUrls
     * @param {?} type
     * @param {?=} content
     * @param {?=} targetQuery
     * @param {?=} position
     * @return {?}
     */
    LazyLoadService.prototype.load = /**
     * @param {?} urlOrUrls
     * @param {?} type
     * @param {?=} content
     * @param {?=} targetQuery
     * @param {?=} position
     * @return {?}
     */
    function (urlOrUrls, type, content, targetQuery, position) {
        var _this = this;
        if (content === void 0) { content = ''; }
        if (targetQuery === void 0) { targetQuery = 'body'; }
        if (position === void 0) { position = 'beforeend'; }
        if (!urlOrUrls && !content) {
            return throwError('Should pass url or content');
        }
        else if (!urlOrUrls && content) {
            urlOrUrls = [null];
        }
        if (!Array.isArray(urlOrUrls)) {
            urlOrUrls = [urlOrUrls];
        }
        return new Observable((/**
         * @param {?} subscriber
         * @return {?}
         */
        function (subscriber) {
            ((/** @type {?} */ (urlOrUrls))).forEach((/**
             * @param {?} url
             * @param {?} index
             * @return {?}
             */
            function (url, index) {
                /** @type {?} */
                var key = url ? url.slice(url.lastIndexOf('/') + 1) : uuid();
                if (_this.loadedLibraries[key]) {
                    subscriber.next();
                    subscriber.complete();
                    return;
                }
                _this.loadedLibraries[key] = new ReplaySubject();
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
                    if (index === urlOrUrls.length - 1) {
                        subscriber.next();
                        subscriber.complete();
                    }
                });
                document.querySelector(targetQuery).insertAdjacentElement(position, library);
            }));
        }));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF6eS1sb2FkLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbGF6eS1sb2FkLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxVQUFVLEVBQUUsYUFBYSxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM3RCxPQUFPLEVBQUUsSUFBSSxFQUFFLE1BQU0sVUFBVSxDQUFDOztBQUVoQztJQUFBO1FBSUUsb0JBQWUsR0FBMkMsRUFBRSxDQUFDO0tBbUU5RDs7Ozs7Ozs7O0lBakVDLDhCQUFJOzs7Ozs7OztJQUFKLFVBQ0UsU0FBNEIsRUFDNUIsSUFBd0IsRUFDeEIsT0FBb0IsRUFDcEIsV0FBNEIsRUFDNUIsUUFBc0M7UUFMeEMsaUJBZ0VDO1FBN0RDLHdCQUFBLEVBQUEsWUFBb0I7UUFDcEIsNEJBQUEsRUFBQSxvQkFBNEI7UUFDNUIseUJBQUEsRUFBQSxzQkFBc0M7UUFFdEMsSUFBSSxDQUFDLFNBQVMsSUFBSSxDQUFDLE9BQU8sRUFBRTtZQUMxQixPQUFPLFVBQVUsQ0FBQyw0QkFBNEIsQ0FBQyxDQUFDO1NBQ2pEO2FBQU0sSUFBSSxDQUFDLFNBQVMsSUFBSSxPQUFPLEVBQUU7WUFDaEMsU0FBUyxHQUFHLENBQUMsSUFBSSxDQUFDLENBQUM7U0FDcEI7UUFFRCxJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sQ0FBQyxTQUFTLENBQUMsRUFBRTtZQUM3QixTQUFTLEdBQUcsQ0FBQyxTQUFTLENBQUMsQ0FBQztTQUN6QjtRQUVELE9BQU8sSUFBSSxVQUFVOzs7O1FBQUMsVUFBQSxVQUFVO1lBQzlCLENBQUMsbUJBQUEsU0FBUyxFQUFZLENBQUMsQ0FBQyxPQUFPOzs7OztZQUFDLFVBQUMsR0FBRyxFQUFFLEtBQUs7O29CQUNuQyxHQUFHLEdBQUcsR0FBRyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxXQUFXLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksRUFBRTtnQkFFOUQsSUFBSSxLQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxFQUFFO29CQUM3QixVQUFVLENBQUMsSUFBSSxFQUFFLENBQUM7b0JBQ2xCLFVBQVUsQ0FBQyxRQUFRLEVBQUUsQ0FBQztvQkFDdEIsT0FBTztpQkFDUjtnQkFFRCxLQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxHQUFHLElBQUksYUFBYSxFQUFFLENBQUM7O29CQUU1QyxPQUFPO2dCQUNYLElBQUksSUFBSSxLQUFLLFFBQVEsRUFBRTtvQkFDckIsT0FBTyxHQUFHLFFBQVEsQ0FBQyxhQUFhLENBQUMsUUFBUSxDQUFDLENBQUM7b0JBQzNDLE9BQU8sQ0FBQyxJQUFJLEdBQUcsaUJBQWlCLENBQUM7b0JBQ2pDLElBQUksR0FBRyxFQUFFO3dCQUNQLENBQUMsbUJBQUEsT0FBTyxFQUFxQixDQUFDLENBQUMsR0FBRyxHQUFHLEdBQUcsQ0FBQztxQkFDMUM7b0JBRUQsQ0FBQyxtQkFBQSxPQUFPLEVBQXFCLENBQUMsQ0FBQyxJQUFJLEdBQUcsT0FBTyxDQUFDO2lCQUMvQztxQkFBTSxJQUFJLEdBQUcsRUFBRTtvQkFDZCxPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxNQUFNLENBQUMsQ0FBQztvQkFDekMsT0FBTyxDQUFDLElBQUksR0FBRyxVQUFVLENBQUM7b0JBQzFCLENBQUMsbUJBQUEsT0FBTyxFQUFtQixDQUFDLENBQUMsR0FBRyxHQUFHLFlBQVksQ0FBQztvQkFFaEQsSUFBSSxHQUFHLEVBQUU7d0JBQ1AsQ0FBQyxtQkFBQSxPQUFPLEVBQW1CLENBQUMsQ0FBQyxJQUFJLEdBQUcsR0FBRyxDQUFDO3FCQUN6QztpQkFDRjtxQkFBTTtvQkFDTCxPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxPQUFPLENBQUMsQ0FBQztvQkFDMUMsQ0FBQyxtQkFBQSxPQUFPLEVBQW9CLENBQUMsQ0FBQyxXQUFXLEdBQUcsT0FBTyxDQUFDO2lCQUNyRDtnQkFFRCxPQUFPLENBQUMsTUFBTTs7O2dCQUFHO29CQUNmLEtBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLENBQUMsSUFBSSxFQUFFLENBQUM7b0JBQ2pDLEtBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLENBQUMsUUFBUSxFQUFFLENBQUM7b0JBRXJDLElBQUksS0FBSyxLQUFLLFNBQVMsQ0FBQyxNQUFNLEdBQUcsQ0FBQyxFQUFFO3dCQUNsQyxVQUFVLENBQUMsSUFBSSxFQUFFLENBQUM7d0JBQ2xCLFVBQVUsQ0FBQyxRQUFRLEVBQUUsQ0FBQztxQkFDdkI7Z0JBQ0gsQ0FBQyxDQUFBLENBQUM7Z0JBRUYsUUFBUSxDQUFDLGFBQWEsQ0FBQyxXQUFXLENBQUMsQ0FBQyxxQkFBcUIsQ0FBQyxRQUFRLEVBQUUsT0FBTyxDQUFDLENBQUM7WUFDL0UsQ0FBQyxFQUFDLENBQUM7UUFDTCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7O2dCQXRFRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7MEJBTkQ7Q0EyRUMsQUF2RUQsSUF1RUM7U0FwRVksZUFBZTs7O0lBQzFCLDBDQUE2RCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUsIFJlcGxheVN1YmplY3QsIHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHV1aWQgfSBmcm9tICcuLi91dGlscyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBMYXp5TG9hZFNlcnZpY2Uge1xuICBsb2FkZWRMaWJyYXJpZXM6IHsgW3VybDogc3RyaW5nXTogUmVwbGF5U3ViamVjdDx2b2lkPiB9ID0ge307XG5cbiAgbG9hZChcbiAgICB1cmxPclVybHM6IHN0cmluZyB8IHN0cmluZ1tdLFxuICAgIHR5cGU6ICdzY3JpcHQnIHwgJ3N0eWxlJyxcbiAgICBjb250ZW50OiBzdHJpbmcgPSAnJyxcbiAgICB0YXJnZXRRdWVyeTogc3RyaW5nID0gJ2JvZHknLFxuICAgIHBvc2l0aW9uOiBJbnNlcnRQb3NpdGlvbiA9ICdiZWZvcmVlbmQnLFxuICApOiBPYnNlcnZhYmxlPHZvaWQ+IHtcbiAgICBpZiAoIXVybE9yVXJscyAmJiAhY29udGVudCkge1xuICAgICAgcmV0dXJuIHRocm93RXJyb3IoJ1Nob3VsZCBwYXNzIHVybCBvciBjb250ZW50Jyk7XG4gICAgfSBlbHNlIGlmICghdXJsT3JVcmxzICYmIGNvbnRlbnQpIHtcbiAgICAgIHVybE9yVXJscyA9IFtudWxsXTtcbiAgICB9XG5cbiAgICBpZiAoIUFycmF5LmlzQXJyYXkodXJsT3JVcmxzKSkge1xuICAgICAgdXJsT3JVcmxzID0gW3VybE9yVXJsc107XG4gICAgfVxuXG4gICAgcmV0dXJuIG5ldyBPYnNlcnZhYmxlKHN1YnNjcmliZXIgPT4ge1xuICAgICAgKHVybE9yVXJscyBhcyBzdHJpbmdbXSkuZm9yRWFjaCgodXJsLCBpbmRleCkgPT4ge1xuICAgICAgICBjb25zdCBrZXkgPSB1cmwgPyB1cmwuc2xpY2UodXJsLmxhc3RJbmRleE9mKCcvJykgKyAxKSA6IHV1aWQoKTtcblxuICAgICAgICBpZiAodGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XSkge1xuICAgICAgICAgIHN1YnNjcmliZXIubmV4dCgpO1xuICAgICAgICAgIHN1YnNjcmliZXIuY29tcGxldGUoKTtcbiAgICAgICAgICByZXR1cm47XG4gICAgICAgIH1cblxuICAgICAgICB0aGlzLmxvYWRlZExpYnJhcmllc1trZXldID0gbmV3IFJlcGxheVN1YmplY3QoKTtcblxuICAgICAgICBsZXQgbGlicmFyeTtcbiAgICAgICAgaWYgKHR5cGUgPT09ICdzY3JpcHQnKSB7XG4gICAgICAgICAgbGlicmFyeSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ3NjcmlwdCcpO1xuICAgICAgICAgIGxpYnJhcnkudHlwZSA9ICd0ZXh0L2phdmFzY3JpcHQnO1xuICAgICAgICAgIGlmICh1cmwpIHtcbiAgICAgICAgICAgIChsaWJyYXJ5IGFzIEhUTUxTY3JpcHRFbGVtZW50KS5zcmMgPSB1cmw7XG4gICAgICAgICAgfVxuXG4gICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTFNjcmlwdEVsZW1lbnQpLnRleHQgPSBjb250ZW50O1xuICAgICAgICB9IGVsc2UgaWYgKHVybCkge1xuICAgICAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdsaW5rJyk7XG4gICAgICAgICAgbGlicmFyeS50eXBlID0gJ3RleHQvY3NzJztcbiAgICAgICAgICAobGlicmFyeSBhcyBIVE1MTGlua0VsZW1lbnQpLnJlbCA9ICdzdHlsZXNoZWV0JztcblxuICAgICAgICAgIGlmICh1cmwpIHtcbiAgICAgICAgICAgIChsaWJyYXJ5IGFzIEhUTUxMaW5rRWxlbWVudCkuaHJlZiA9IHVybDtcbiAgICAgICAgICB9XG4gICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgbGlicmFyeSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ3N0eWxlJyk7XG4gICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTFN0eWxlRWxlbWVudCkudGV4dENvbnRlbnQgPSBjb250ZW50O1xuICAgICAgICB9XG5cbiAgICAgICAgbGlicmFyeS5vbmxvYWQgPSAoKSA9PiB7XG4gICAgICAgICAgdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XS5uZXh0KCk7XG4gICAgICAgICAgdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XS5jb21wbGV0ZSgpO1xuXG4gICAgICAgICAgaWYgKGluZGV4ID09PSB1cmxPclVybHMubGVuZ3RoIC0gMSkge1xuICAgICAgICAgICAgc3Vic2NyaWJlci5uZXh0KCk7XG4gICAgICAgICAgICBzdWJzY3JpYmVyLmNvbXBsZXRlKCk7XG4gICAgICAgICAgfVxuICAgICAgICB9O1xuXG4gICAgICAgIGRvY3VtZW50LnF1ZXJ5U2VsZWN0b3IodGFyZ2V0UXVlcnkpLmluc2VydEFkamFjZW50RWxlbWVudChwb3NpdGlvbiwgbGlicmFyeSk7XG4gICAgICB9KTtcbiAgICB9KTtcbiAgfVxufVxuIl19