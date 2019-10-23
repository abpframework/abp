/**
 * @fileoverview added by tsickle
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
        if (position === void 0) { position = 'afterend'; }
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF6eS1sb2FkLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbGF6eS1sb2FkLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFVBQVUsRUFBRSxhQUFhLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzdELE9BQU8sRUFBRSxJQUFJLEVBQUUsTUFBTSxVQUFVLENBQUM7O0FBRWhDO0lBQUE7UUFJRSxvQkFBZSxHQUEyQyxFQUFFLENBQUM7S0FtRTlEOzs7Ozs7Ozs7SUFqRUMsOEJBQUk7Ozs7Ozs7O0lBQUosVUFDRSxTQUE0QixFQUM1QixJQUF3QixFQUN4QixPQUFvQixFQUNwQixXQUE0QixFQUM1QixRQUFxQztRQUx2QyxpQkFnRUM7UUE3REMsd0JBQUEsRUFBQSxZQUFvQjtRQUNwQiw0QkFBQSxFQUFBLG9CQUE0QjtRQUM1Qix5QkFBQSxFQUFBLHFCQUFxQztRQUVyQyxJQUFJLENBQUMsU0FBUyxJQUFJLENBQUMsT0FBTyxFQUFFO1lBQzFCLE9BQU8sVUFBVSxDQUFDLDRCQUE0QixDQUFDLENBQUM7U0FDakQ7YUFBTSxJQUFJLENBQUMsU0FBUyxJQUFJLE9BQU8sRUFBRTtZQUNoQyxTQUFTLEdBQUcsQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUNwQjtRQUVELElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLFNBQVMsQ0FBQyxFQUFFO1lBQzdCLFNBQVMsR0FBRyxDQUFDLFNBQVMsQ0FBQyxDQUFDO1NBQ3pCO1FBRUQsT0FBTyxJQUFJLFVBQVU7Ozs7UUFBQyxVQUFBLFVBQVU7WUFDOUIsQ0FBQyxtQkFBQSxTQUFTLEVBQVksQ0FBQyxDQUFDLE9BQU87Ozs7O1lBQUMsVUFBQyxHQUFHLEVBQUUsS0FBSzs7b0JBQ25DLEdBQUcsR0FBRyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLFdBQVcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUFFO2dCQUU5RCxJQUFJLEtBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLEVBQUU7b0JBQzdCLFVBQVUsQ0FBQyxJQUFJLEVBQUUsQ0FBQztvQkFDbEIsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDO29CQUN0QixPQUFPO2lCQUNSO2dCQUVELEtBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxhQUFhLEVBQUUsQ0FBQzs7b0JBRTVDLE9BQU87Z0JBQ1gsSUFBSSxJQUFJLEtBQUssUUFBUSxFQUFFO29CQUNyQixPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxRQUFRLENBQUMsQ0FBQztvQkFDM0MsT0FBTyxDQUFDLElBQUksR0FBRyxpQkFBaUIsQ0FBQztvQkFDakMsSUFBSSxHQUFHLEVBQUU7d0JBQ1AsQ0FBQyxtQkFBQSxPQUFPLEVBQXFCLENBQUMsQ0FBQyxHQUFHLEdBQUcsR0FBRyxDQUFDO3FCQUMxQztvQkFFRCxDQUFDLG1CQUFBLE9BQU8sRUFBcUIsQ0FBQyxDQUFDLElBQUksR0FBRyxPQUFPLENBQUM7aUJBQy9DO3FCQUFNLElBQUksR0FBRyxFQUFFO29CQUNkLE9BQU8sR0FBRyxRQUFRLENBQUMsYUFBYSxDQUFDLE1BQU0sQ0FBQyxDQUFDO29CQUN6QyxPQUFPLENBQUMsSUFBSSxHQUFHLFVBQVUsQ0FBQztvQkFDMUIsQ0FBQyxtQkFBQSxPQUFPLEVBQW1CLENBQUMsQ0FBQyxHQUFHLEdBQUcsWUFBWSxDQUFDO29CQUVoRCxJQUFJLEdBQUcsRUFBRTt3QkFDUCxDQUFDLG1CQUFBLE9BQU8sRUFBbUIsQ0FBQyxDQUFDLElBQUksR0FBRyxHQUFHLENBQUM7cUJBQ3pDO2lCQUNGO3FCQUFNO29CQUNMLE9BQU8sR0FBRyxRQUFRLENBQUMsYUFBYSxDQUFDLE9BQU8sQ0FBQyxDQUFDO29CQUMxQyxDQUFDLG1CQUFBLE9BQU8sRUFBb0IsQ0FBQyxDQUFDLFdBQVcsR0FBRyxPQUFPLENBQUM7aUJBQ3JEO2dCQUVELE9BQU8sQ0FBQyxNQUFNOzs7Z0JBQUc7b0JBQ2YsS0FBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztvQkFDakMsS0FBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxRQUFRLEVBQUUsQ0FBQztvQkFFckMsSUFBSSxLQUFLLEtBQUssU0FBUyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7d0JBQ2xDLFVBQVUsQ0FBQyxJQUFJLEVBQUUsQ0FBQzt3QkFDbEIsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDO3FCQUN2QjtnQkFDSCxDQUFDLENBQUEsQ0FBQztnQkFFRixRQUFRLENBQUMsYUFBYSxDQUFDLFdBQVcsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLFFBQVEsRUFBRSxPQUFPLENBQUMsQ0FBQztZQUMvRSxDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7Z0JBdEVGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7OzswQkFORDtDQTJFQyxBQXZFRCxJQXVFQztTQXBFWSxlQUFlOzs7SUFDMUIsMENBQTZEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlLCBSZXBsYXlTdWJqZWN0LCB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IHV1aWQgfSBmcm9tICcuLi91dGlscyc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgTGF6eUxvYWRTZXJ2aWNlIHtcclxuICBsb2FkZWRMaWJyYXJpZXM6IHsgW3VybDogc3RyaW5nXTogUmVwbGF5U3ViamVjdDx2b2lkPiB9ID0ge307XHJcblxyXG4gIGxvYWQoXHJcbiAgICB1cmxPclVybHM6IHN0cmluZyB8IHN0cmluZ1tdLFxyXG4gICAgdHlwZTogJ3NjcmlwdCcgfCAnc3R5bGUnLFxyXG4gICAgY29udGVudDogc3RyaW5nID0gJycsXHJcbiAgICB0YXJnZXRRdWVyeTogc3RyaW5nID0gJ2JvZHknLFxyXG4gICAgcG9zaXRpb246IEluc2VydFBvc2l0aW9uID0gJ2FmdGVyZW5kJyxcclxuICApOiBPYnNlcnZhYmxlPHZvaWQ+IHtcclxuICAgIGlmICghdXJsT3JVcmxzICYmICFjb250ZW50KSB7XHJcbiAgICAgIHJldHVybiB0aHJvd0Vycm9yKCdTaG91bGQgcGFzcyB1cmwgb3IgY29udGVudCcpO1xyXG4gICAgfSBlbHNlIGlmICghdXJsT3JVcmxzICYmIGNvbnRlbnQpIHtcclxuICAgICAgdXJsT3JVcmxzID0gW251bGxdO1xyXG4gICAgfVxyXG5cclxuICAgIGlmICghQXJyYXkuaXNBcnJheSh1cmxPclVybHMpKSB7XHJcbiAgICAgIHVybE9yVXJscyA9IFt1cmxPclVybHNdO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBuZXcgT2JzZXJ2YWJsZShzdWJzY3JpYmVyID0+IHtcclxuICAgICAgKHVybE9yVXJscyBhcyBzdHJpbmdbXSkuZm9yRWFjaCgodXJsLCBpbmRleCkgPT4ge1xyXG4gICAgICAgIGNvbnN0IGtleSA9IHVybCA/IHVybC5zbGljZSh1cmwubGFzdEluZGV4T2YoJy8nKSArIDEpIDogdXVpZCgpO1xyXG5cclxuICAgICAgICBpZiAodGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XSkge1xyXG4gICAgICAgICAgc3Vic2NyaWJlci5uZXh0KCk7XHJcbiAgICAgICAgICBzdWJzY3JpYmVyLmNvbXBsZXRlKCk7XHJcbiAgICAgICAgICByZXR1cm47XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLmxvYWRlZExpYnJhcmllc1trZXldID0gbmV3IFJlcGxheVN1YmplY3QoKTtcclxuXHJcbiAgICAgICAgbGV0IGxpYnJhcnk7XHJcbiAgICAgICAgaWYgKHR5cGUgPT09ICdzY3JpcHQnKSB7XHJcbiAgICAgICAgICBsaWJyYXJ5ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnc2NyaXB0Jyk7XHJcbiAgICAgICAgICBsaWJyYXJ5LnR5cGUgPSAndGV4dC9qYXZhc2NyaXB0JztcclxuICAgICAgICAgIGlmICh1cmwpIHtcclxuICAgICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTFNjcmlwdEVsZW1lbnQpLnNyYyA9IHVybDtcclxuICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAobGlicmFyeSBhcyBIVE1MU2NyaXB0RWxlbWVudCkudGV4dCA9IGNvbnRlbnQ7XHJcbiAgICAgICAgfSBlbHNlIGlmICh1cmwpIHtcclxuICAgICAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdsaW5rJyk7XHJcbiAgICAgICAgICBsaWJyYXJ5LnR5cGUgPSAndGV4dC9jc3MnO1xyXG4gICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTExpbmtFbGVtZW50KS5yZWwgPSAnc3R5bGVzaGVldCc7XHJcblxyXG4gICAgICAgICAgaWYgKHVybCkge1xyXG4gICAgICAgICAgICAobGlicmFyeSBhcyBIVE1MTGlua0VsZW1lbnQpLmhyZWYgPSB1cmw7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdzdHlsZScpO1xyXG4gICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTFN0eWxlRWxlbWVudCkudGV4dENvbnRlbnQgPSBjb250ZW50O1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgbGlicmFyeS5vbmxvYWQgPSAoKSA9PiB7XHJcbiAgICAgICAgICB0aGlzLmxvYWRlZExpYnJhcmllc1trZXldLm5leHQoKTtcclxuICAgICAgICAgIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0uY29tcGxldGUoKTtcclxuXHJcbiAgICAgICAgICBpZiAoaW5kZXggPT09IHVybE9yVXJscy5sZW5ndGggLSAxKSB7XHJcbiAgICAgICAgICAgIHN1YnNjcmliZXIubmV4dCgpO1xyXG4gICAgICAgICAgICBzdWJzY3JpYmVyLmNvbXBsZXRlKCk7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfTtcclxuXHJcbiAgICAgICAgZG9jdW1lbnQucXVlcnlTZWxlY3Rvcih0YXJnZXRRdWVyeSkuaW5zZXJ0QWRqYWNlbnRFbGVtZW50KHBvc2l0aW9uLCBsaWJyYXJ5KTtcclxuICAgICAgfSk7XHJcbiAgICB9KTtcclxuICB9XHJcbn1cclxuIl19