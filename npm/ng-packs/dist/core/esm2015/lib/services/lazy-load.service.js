/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { uuid } from '../utils';
import * as i0 from "@angular/core";
export class LazyLoadService {
    constructor() {
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
    load(url, type, content = '', targetQuery = 'body', position = 'afterend') {
        if (!url && !content)
            return;
        /** @type {?} */
        const key = url ? url.slice(url.lastIndexOf('/') + 1) : uuid();
        if (this.loadedLibraries[key]) {
            return this.loadedLibraries[key].asObservable();
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
        });
        document.querySelector(targetQuery).insertAdjacentElement(position, library);
        return this.loadedLibraries[key].asObservable();
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF6eS1sb2FkLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbGF6eS1sb2FkLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBVSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUFjLGFBQWEsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqRCxPQUFPLEVBQUUsSUFBSSxFQUFFLE1BQU0sVUFBVSxDQUFDOztBQUtoQyxNQUFNLE9BQU8sZUFBZTtJQUg1QjtRQUlFLG9CQUFlLEdBQTJDLEVBQUUsQ0FBQztLQWlEOUQ7Ozs7Ozs7OztJQS9DQyxJQUFJLENBQ0YsR0FBVyxFQUNYLElBQXdCLEVBQ3hCLFVBQWtCLEVBQUUsRUFDcEIsY0FBc0IsTUFBTSxFQUM1QixXQUEyQixVQUFVO1FBRXJDLElBQUksQ0FBQyxHQUFHLElBQUksQ0FBQyxPQUFPO1lBQUUsT0FBTzs7Y0FDdkIsR0FBRyxHQUFHLEdBQUcsQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsV0FBVyxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUU7UUFFOUQsSUFBSSxJQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxFQUFFO1lBQzdCLE9BQU8sSUFBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxZQUFZLEVBQUUsQ0FBQztTQUNqRDtRQUVELElBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxhQUFhLEVBQUUsQ0FBQzs7WUFFNUMsT0FBTztRQUNYLElBQUksSUFBSSxLQUFLLFFBQVEsRUFBRTtZQUNyQixPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxRQUFRLENBQUMsQ0FBQztZQUMzQyxPQUFPLENBQUMsSUFBSSxHQUFHLGlCQUFpQixDQUFDO1lBQ2pDLElBQUksR0FBRyxFQUFFO2dCQUNQLENBQUMsbUJBQUEsT0FBTyxFQUFxQixDQUFDLENBQUMsR0FBRyxHQUFHLEdBQUcsQ0FBQzthQUMxQztZQUVELENBQUMsbUJBQUEsT0FBTyxFQUFxQixDQUFDLENBQUMsSUFBSSxHQUFHLE9BQU8sQ0FBQztTQUMvQzthQUFNLElBQUksR0FBRyxFQUFFO1lBQ2QsT0FBTyxHQUFHLFFBQVEsQ0FBQyxhQUFhLENBQUMsTUFBTSxDQUFDLENBQUM7WUFDekMsT0FBTyxDQUFDLElBQUksR0FBRyxVQUFVLENBQUM7WUFDMUIsQ0FBQyxtQkFBQSxPQUFPLEVBQW1CLENBQUMsQ0FBQyxHQUFHLEdBQUcsWUFBWSxDQUFDO1lBRWhELElBQUksR0FBRyxFQUFFO2dCQUNQLENBQUMsbUJBQUEsT0FBTyxFQUFtQixDQUFDLENBQUMsSUFBSSxHQUFHLEdBQUcsQ0FBQzthQUN6QztTQUNGO2FBQU07WUFDTCxPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxPQUFPLENBQUMsQ0FBQztZQUMxQyxDQUFDLG1CQUFBLE9BQU8sRUFBb0IsQ0FBQyxDQUFDLFdBQVcsR0FBRyxPQUFPLENBQUM7U0FDckQ7UUFFRCxPQUFPLENBQUMsTUFBTTs7O1FBQUcsR0FBRyxFQUFFO1lBQ3BCLElBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLENBQUMsSUFBSSxFQUFFLENBQUM7WUFDakMsSUFBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxRQUFRLEVBQUUsQ0FBQztRQUN2QyxDQUFDLENBQUEsQ0FBQztRQUVGLFFBQVEsQ0FBQyxhQUFhLENBQUMsV0FBVyxDQUFDLENBQUMscUJBQXFCLENBQUMsUUFBUSxFQUFFLE9BQU8sQ0FBQyxDQUFDO1FBRTdFLE9BQU8sSUFBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxZQUFZLEVBQUUsQ0FBQztJQUNsRCxDQUFDOzs7WUFwREYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7OztJQUVDLDBDQUE2RCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdCwgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgUmVwbGF5U3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdXVpZCB9IGZyb20gJy4uL3V0aWxzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIExhenlMb2FkU2VydmljZSB7XG4gIGxvYWRlZExpYnJhcmllczogeyBbdXJsOiBzdHJpbmddOiBSZXBsYXlTdWJqZWN0PHZvaWQ+IH0gPSB7fTtcblxuICBsb2FkKFxuICAgIHVybDogc3RyaW5nLFxuICAgIHR5cGU6ICdzY3JpcHQnIHwgJ3N0eWxlJyxcbiAgICBjb250ZW50OiBzdHJpbmcgPSAnJyxcbiAgICB0YXJnZXRRdWVyeTogc3RyaW5nID0gJ2JvZHknLFxuICAgIHBvc2l0aW9uOiBJbnNlcnRQb3NpdGlvbiA9ICdhZnRlcmVuZCcsXG4gICk6IE9ic2VydmFibGU8dm9pZD4ge1xuICAgIGlmICghdXJsICYmICFjb250ZW50KSByZXR1cm47XG4gICAgY29uc3Qga2V5ID0gdXJsID8gdXJsLnNsaWNlKHVybC5sYXN0SW5kZXhPZignLycpICsgMSkgOiB1dWlkKCk7XG5cbiAgICBpZiAodGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XSkge1xuICAgICAgcmV0dXJuIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0uYXNPYnNlcnZhYmxlKCk7XG4gICAgfVxuXG4gICAgdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XSA9IG5ldyBSZXBsYXlTdWJqZWN0KCk7XG5cbiAgICBsZXQgbGlicmFyeTtcbiAgICBpZiAodHlwZSA9PT0gJ3NjcmlwdCcpIHtcbiAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdzY3JpcHQnKTtcbiAgICAgIGxpYnJhcnkudHlwZSA9ICd0ZXh0L2phdmFzY3JpcHQnO1xuICAgICAgaWYgKHVybCkge1xuICAgICAgICAobGlicmFyeSBhcyBIVE1MU2NyaXB0RWxlbWVudCkuc3JjID0gdXJsO1xuICAgICAgfVxuXG4gICAgICAobGlicmFyeSBhcyBIVE1MU2NyaXB0RWxlbWVudCkudGV4dCA9IGNvbnRlbnQ7XG4gICAgfSBlbHNlIGlmICh1cmwpIHtcbiAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdsaW5rJyk7XG4gICAgICBsaWJyYXJ5LnR5cGUgPSAndGV4dC9jc3MnO1xuICAgICAgKGxpYnJhcnkgYXMgSFRNTExpbmtFbGVtZW50KS5yZWwgPSAnc3R5bGVzaGVldCc7XG5cbiAgICAgIGlmICh1cmwpIHtcbiAgICAgICAgKGxpYnJhcnkgYXMgSFRNTExpbmtFbGVtZW50KS5ocmVmID0gdXJsO1xuICAgICAgfVxuICAgIH0gZWxzZSB7XG4gICAgICBsaWJyYXJ5ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnc3R5bGUnKTtcbiAgICAgIChsaWJyYXJ5IGFzIEhUTUxTdHlsZUVsZW1lbnQpLnRleHRDb250ZW50ID0gY29udGVudDtcbiAgICB9XG5cbiAgICBsaWJyYXJ5Lm9ubG9hZCA9ICgpID0+IHtcbiAgICAgIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0ubmV4dCgpO1xuICAgICAgdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XS5jb21wbGV0ZSgpO1xuICAgIH07XG5cbiAgICBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKHRhcmdldFF1ZXJ5KS5pbnNlcnRBZGphY2VudEVsZW1lbnQocG9zaXRpb24sIGxpYnJhcnkpO1xuXG4gICAgcmV0dXJuIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0uYXNPYnNlcnZhYmxlKCk7XG4gIH1cbn1cbiJdfQ==