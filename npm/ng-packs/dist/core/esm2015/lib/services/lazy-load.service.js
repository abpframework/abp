/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/lazy-load.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, throwError } from 'rxjs';
import { uuid } from '../utils';
import * as i0 from "@angular/core";
export class LazyLoadService {
    constructor() {
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
    load(urlOrUrls, type, content = '', targetQuery = 'body', position = 'beforeend') {
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
        subscriber => {
            ((/** @type {?} */ (urlOrUrls))).forEach((/**
             * @param {?} url
             * @param {?} index
             * @return {?}
             */
            (url, index) => {
                /** @type {?} */
                const key = url ? url.slice(url.lastIndexOf('/') + 1) : uuid();
                if (this.loadedLibraries[key]) {
                    subscriber.next();
                    subscriber.complete();
                    return;
                }
                this.loadedLibraries[key] = new ReplaySubject();
                /** @type {?} */
                let library;
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
                () => {
                    this.loadedLibraries[key].next();
                    this.loadedLibraries[key].complete();
                    if (index === urlOrUrls.length - 1) {
                        subscriber.next();
                        subscriber.complete();
                    }
                });
                document.querySelector(targetQuery).insertAdjacentElement(position, library);
            }));
        }));
    }
}
LazyLoadService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */ LazyLoadService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function LazyLoadService_Factory() { return new LazyLoadService(); }, token: LazyLoadService, providedIn: "root" });
if (false) {
    /** @type {?} */
    LazyLoadService.prototype.loadedLibraries;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF6eS1sb2FkLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbGF6eS1sb2FkLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxVQUFVLEVBQUUsYUFBYSxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM3RCxPQUFPLEVBQUUsSUFBSSxFQUFFLE1BQU0sVUFBVSxDQUFDOztBQUtoQyxNQUFNLE9BQU8sZUFBZTtJQUg1QjtRQUlFLG9CQUFlLEdBQTJDLEVBQUUsQ0FBQztLQW1FOUQ7Ozs7Ozs7OztJQWpFQyxJQUFJLENBQ0YsU0FBNEIsRUFDNUIsSUFBd0IsRUFDeEIsVUFBa0IsRUFBRSxFQUNwQixjQUFzQixNQUFNLEVBQzVCLFdBQTJCLFdBQVc7UUFFdEMsSUFBSSxDQUFDLFNBQVMsSUFBSSxDQUFDLE9BQU8sRUFBRTtZQUMxQixPQUFPLFVBQVUsQ0FBQyw0QkFBNEIsQ0FBQyxDQUFDO1NBQ2pEO2FBQU0sSUFBSSxDQUFDLFNBQVMsSUFBSSxPQUFPLEVBQUU7WUFDaEMsU0FBUyxHQUFHLENBQUMsSUFBSSxDQUFDLENBQUM7U0FDcEI7UUFFRCxJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sQ0FBQyxTQUFTLENBQUMsRUFBRTtZQUM3QixTQUFTLEdBQUcsQ0FBQyxTQUFTLENBQUMsQ0FBQztTQUN6QjtRQUVELE9BQU8sSUFBSSxVQUFVOzs7O1FBQUMsVUFBVSxDQUFDLEVBQUU7WUFDakMsQ0FBQyxtQkFBQSxTQUFTLEVBQVksQ0FBQyxDQUFDLE9BQU87Ozs7O1lBQUMsQ0FBQyxHQUFHLEVBQUUsS0FBSyxFQUFFLEVBQUU7O3NCQUN2QyxHQUFHLEdBQUcsR0FBRyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxXQUFXLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksRUFBRTtnQkFFOUQsSUFBSSxJQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxFQUFFO29CQUM3QixVQUFVLENBQUMsSUFBSSxFQUFFLENBQUM7b0JBQ2xCLFVBQVUsQ0FBQyxRQUFRLEVBQUUsQ0FBQztvQkFDdEIsT0FBTztpQkFDUjtnQkFFRCxJQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxHQUFHLElBQUksYUFBYSxFQUFFLENBQUM7O29CQUU1QyxPQUFPO2dCQUNYLElBQUksSUFBSSxLQUFLLFFBQVEsRUFBRTtvQkFDckIsT0FBTyxHQUFHLFFBQVEsQ0FBQyxhQUFhLENBQUMsUUFBUSxDQUFDLENBQUM7b0JBQzNDLE9BQU8sQ0FBQyxJQUFJLEdBQUcsaUJBQWlCLENBQUM7b0JBQ2pDLElBQUksR0FBRyxFQUFFO3dCQUNQLENBQUMsbUJBQUEsT0FBTyxFQUFxQixDQUFDLENBQUMsR0FBRyxHQUFHLEdBQUcsQ0FBQztxQkFDMUM7b0JBRUQsQ0FBQyxtQkFBQSxPQUFPLEVBQXFCLENBQUMsQ0FBQyxJQUFJLEdBQUcsT0FBTyxDQUFDO2lCQUMvQztxQkFBTSxJQUFJLEdBQUcsRUFBRTtvQkFDZCxPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxNQUFNLENBQUMsQ0FBQztvQkFDekMsT0FBTyxDQUFDLElBQUksR0FBRyxVQUFVLENBQUM7b0JBQzFCLENBQUMsbUJBQUEsT0FBTyxFQUFtQixDQUFDLENBQUMsR0FBRyxHQUFHLFlBQVksQ0FBQztvQkFFaEQsSUFBSSxHQUFHLEVBQUU7d0JBQ1AsQ0FBQyxtQkFBQSxPQUFPLEVBQW1CLENBQUMsQ0FBQyxJQUFJLEdBQUcsR0FBRyxDQUFDO3FCQUN6QztpQkFDRjtxQkFBTTtvQkFDTCxPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxPQUFPLENBQUMsQ0FBQztvQkFDMUMsQ0FBQyxtQkFBQSxPQUFPLEVBQW9CLENBQUMsQ0FBQyxXQUFXLEdBQUcsT0FBTyxDQUFDO2lCQUNyRDtnQkFFRCxPQUFPLENBQUMsTUFBTTs7O2dCQUFHLEdBQUcsRUFBRTtvQkFDcEIsSUFBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztvQkFDakMsSUFBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxRQUFRLEVBQUUsQ0FBQztvQkFFckMsSUFBSSxLQUFLLEtBQUssU0FBUyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7d0JBQ2xDLFVBQVUsQ0FBQyxJQUFJLEVBQUUsQ0FBQzt3QkFDbEIsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDO3FCQUN2QjtnQkFDSCxDQUFDLENBQUEsQ0FBQztnQkFFRixRQUFRLENBQUMsYUFBYSxDQUFDLFdBQVcsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLFFBQVEsRUFBRSxPQUFPLENBQUMsQ0FBQztZQUMvRSxDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7O1lBdEVGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7Ozs7SUFFQywwQ0FBNkQiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlLCBSZXBsYXlTdWJqZWN0LCB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB1dWlkIH0gZnJvbSAnLi4vdXRpbHMnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgTGF6eUxvYWRTZXJ2aWNlIHtcbiAgbG9hZGVkTGlicmFyaWVzOiB7IFt1cmw6IHN0cmluZ106IFJlcGxheVN1YmplY3Q8dm9pZD4gfSA9IHt9O1xuXG4gIGxvYWQoXG4gICAgdXJsT3JVcmxzOiBzdHJpbmcgfCBzdHJpbmdbXSxcbiAgICB0eXBlOiAnc2NyaXB0JyB8ICdzdHlsZScsXG4gICAgY29udGVudDogc3RyaW5nID0gJycsXG4gICAgdGFyZ2V0UXVlcnk6IHN0cmluZyA9ICdib2R5JyxcbiAgICBwb3NpdGlvbjogSW5zZXJ0UG9zaXRpb24gPSAnYmVmb3JlZW5kJyxcbiAgKTogT2JzZXJ2YWJsZTx2b2lkPiB7XG4gICAgaWYgKCF1cmxPclVybHMgJiYgIWNvbnRlbnQpIHtcbiAgICAgIHJldHVybiB0aHJvd0Vycm9yKCdTaG91bGQgcGFzcyB1cmwgb3IgY29udGVudCcpO1xuICAgIH0gZWxzZSBpZiAoIXVybE9yVXJscyAmJiBjb250ZW50KSB7XG4gICAgICB1cmxPclVybHMgPSBbbnVsbF07XG4gICAgfVxuXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KHVybE9yVXJscykpIHtcbiAgICAgIHVybE9yVXJscyA9IFt1cmxPclVybHNdO1xuICAgIH1cblxuICAgIHJldHVybiBuZXcgT2JzZXJ2YWJsZShzdWJzY3JpYmVyID0+IHtcbiAgICAgICh1cmxPclVybHMgYXMgc3RyaW5nW10pLmZvckVhY2goKHVybCwgaW5kZXgpID0+IHtcbiAgICAgICAgY29uc3Qga2V5ID0gdXJsID8gdXJsLnNsaWNlKHVybC5sYXN0SW5kZXhPZignLycpICsgMSkgOiB1dWlkKCk7XG5cbiAgICAgICAgaWYgKHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0pIHtcbiAgICAgICAgICBzdWJzY3JpYmVyLm5leHQoKTtcbiAgICAgICAgICBzdWJzY3JpYmVyLmNvbXBsZXRlKCk7XG4gICAgICAgICAgcmV0dXJuO1xuICAgICAgICB9XG5cbiAgICAgICAgdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XSA9IG5ldyBSZXBsYXlTdWJqZWN0KCk7XG5cbiAgICAgICAgbGV0IGxpYnJhcnk7XG4gICAgICAgIGlmICh0eXBlID09PSAnc2NyaXB0Jykge1xuICAgICAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdzY3JpcHQnKTtcbiAgICAgICAgICBsaWJyYXJ5LnR5cGUgPSAndGV4dC9qYXZhc2NyaXB0JztcbiAgICAgICAgICBpZiAodXJsKSB7XG4gICAgICAgICAgICAobGlicmFyeSBhcyBIVE1MU2NyaXB0RWxlbWVudCkuc3JjID0gdXJsO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIChsaWJyYXJ5IGFzIEhUTUxTY3JpcHRFbGVtZW50KS50ZXh0ID0gY29udGVudDtcbiAgICAgICAgfSBlbHNlIGlmICh1cmwpIHtcbiAgICAgICAgICBsaWJyYXJ5ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnbGluaycpO1xuICAgICAgICAgIGxpYnJhcnkudHlwZSA9ICd0ZXh0L2Nzcyc7XG4gICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTExpbmtFbGVtZW50KS5yZWwgPSAnc3R5bGVzaGVldCc7XG5cbiAgICAgICAgICBpZiAodXJsKSB7XG4gICAgICAgICAgICAobGlicmFyeSBhcyBIVE1MTGlua0VsZW1lbnQpLmhyZWYgPSB1cmw7XG4gICAgICAgICAgfVxuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdzdHlsZScpO1xuICAgICAgICAgIChsaWJyYXJ5IGFzIEhUTUxTdHlsZUVsZW1lbnQpLnRleHRDb250ZW50ID0gY29udGVudDtcbiAgICAgICAgfVxuXG4gICAgICAgIGxpYnJhcnkub25sb2FkID0gKCkgPT4ge1xuICAgICAgICAgIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0ubmV4dCgpO1xuICAgICAgICAgIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0uY29tcGxldGUoKTtcblxuICAgICAgICAgIGlmIChpbmRleCA9PT0gdXJsT3JVcmxzLmxlbmd0aCAtIDEpIHtcbiAgICAgICAgICAgIHN1YnNjcmliZXIubmV4dCgpO1xuICAgICAgICAgICAgc3Vic2NyaWJlci5jb21wbGV0ZSgpO1xuICAgICAgICAgIH1cbiAgICAgICAgfTtcblxuICAgICAgICBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKHRhcmdldFF1ZXJ5KS5pbnNlcnRBZGphY2VudEVsZW1lbnQocG9zaXRpb24sIGxpYnJhcnkpO1xuICAgICAgfSk7XG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==