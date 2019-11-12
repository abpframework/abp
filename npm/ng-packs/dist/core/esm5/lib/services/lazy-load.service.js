/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, throwError } from 'rxjs';
import { uuid } from '../utils';
import * as i0 from '@angular/core';
var LazyLoadService = /** @class */ (function() {
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
  LazyLoadService.prototype.load
  /**
   * @param {?} urlOrUrls
   * @param {?} type
   * @param {?=} content
   * @param {?=} targetQuery
   * @param {?=} position
   * @return {?}
   */ = function(urlOrUrls, type, content, targetQuery, position) {
    var _this = this;
    if (content === void 0) {
      content = '';
    }
    if (targetQuery === void 0) {
      targetQuery = 'body';
    }
    if (position === void 0) {
      position = 'afterend';
    }
    if (!urlOrUrls && !content) {
      return throwError('Should pass url or content');
    } else if (!urlOrUrls && content) {
      urlOrUrls = [null];
    }
    if (!Array.isArray(urlOrUrls)) {
      urlOrUrls = [urlOrUrls];
    }
    return new Observable
    /**
     * @param {?} subscriber
     * @return {?}
     */(function(subscriber) {
      /** @type {?} */ (urlOrUrls).forEach(
        /**
         * @param {?} url
         * @param {?} index
         * @return {?}
         */
        function(url, index) {
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
              /** @type {?} */ (library).src = url;
            }
            /** @type {?} */ (library).text = content;
          } else if (url) {
            library = document.createElement('link');
            library.type = 'text/css';
            /** @type {?} */ (library).rel = 'stylesheet';
            if (url) {
              /** @type {?} */ (library).href = url;
            }
          } else {
            library = document.createElement('style');
            /** @type {?} */ (library).textContent = content;
          }
          library.onload
          /**
           * @return {?}
           */ = function() {
            _this.loadedLibraries[key].next();
            _this.loadedLibraries[key].complete();
            if (index === urlOrUrls.length - 1) {
              subscriber.next();
              subscriber.complete();
            }
          };
          document.querySelector(targetQuery).insertAdjacentElement(position, library);
        },
      );
    });
  };
  LazyLoadService.decorators = [
    {
      type: Injectable,
      args: [
        {
          providedIn: 'root',
        },
      ],
    },
  ];
  /** @nocollapse */ LazyLoadService.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function LazyLoadService_Factory() {
      return new LazyLoadService();
    },
    token: LazyLoadService,
    providedIn: 'root',
  });
  return LazyLoadService;
})();
export { LazyLoadService };
if (false) {
  /** @type {?} */
  LazyLoadService.prototype.loadedLibraries;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF6eS1sb2FkLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbGF6eS1sb2FkLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFVBQVUsRUFBRSxhQUFhLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzdELE9BQU8sRUFBRSxJQUFJLEVBQUUsTUFBTSxVQUFVLENBQUM7O0FBRWhDO0lBQUE7UUFJRSxvQkFBZSxHQUEyQyxFQUFFLENBQUM7S0FtRTlEOzs7Ozs7Ozs7SUFqRUMsOEJBQUk7Ozs7Ozs7O0lBQUosVUFDRSxTQUE0QixFQUM1QixJQUF3QixFQUN4QixPQUFvQixFQUNwQixXQUE0QixFQUM1QixRQUFxQztRQUx2QyxpQkFnRUM7UUE3REMsd0JBQUEsRUFBQSxZQUFvQjtRQUNwQiw0QkFBQSxFQUFBLG9CQUE0QjtRQUM1Qix5QkFBQSxFQUFBLHFCQUFxQztRQUVyQyxJQUFJLENBQUMsU0FBUyxJQUFJLENBQUMsT0FBTyxFQUFFO1lBQzFCLE9BQU8sVUFBVSxDQUFDLDRCQUE0QixDQUFDLENBQUM7U0FDakQ7YUFBTSxJQUFJLENBQUMsU0FBUyxJQUFJLE9BQU8sRUFBRTtZQUNoQyxTQUFTLEdBQUcsQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUNwQjtRQUVELElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLFNBQVMsQ0FBQyxFQUFFO1lBQzdCLFNBQVMsR0FBRyxDQUFDLFNBQVMsQ0FBQyxDQUFDO1NBQ3pCO1FBRUQsT0FBTyxJQUFJLFVBQVU7Ozs7UUFBQyxVQUFBLFVBQVU7WUFDOUIsQ0FBQyxtQkFBQSxTQUFTLEVBQVksQ0FBQyxDQUFDLE9BQU87Ozs7O1lBQUMsVUFBQyxHQUFHLEVBQUUsS0FBSzs7b0JBQ25DLEdBQUcsR0FBRyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLFdBQVcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUFFO2dCQUU5RCxJQUFJLEtBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLEVBQUU7b0JBQzdCLFVBQVUsQ0FBQyxJQUFJLEVBQUUsQ0FBQztvQkFDbEIsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDO29CQUN0QixPQUFPO2lCQUNSO2dCQUVELEtBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxhQUFhLEVBQUUsQ0FBQzs7b0JBRTVDLE9BQU87Z0JBQ1gsSUFBSSxJQUFJLEtBQUssUUFBUSxFQUFFO29CQUNyQixPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxRQUFRLENBQUMsQ0FBQztvQkFDM0MsT0FBTyxDQUFDLElBQUksR0FBRyxpQkFBaUIsQ0FBQztvQkFDakMsSUFBSSxHQUFHLEVBQUU7d0JBQ1AsQ0FBQyxtQkFBQSxPQUFPLEVBQXFCLENBQUMsQ0FBQyxHQUFHLEdBQUcsR0FBRyxDQUFDO3FCQUMxQztvQkFFRCxDQUFDLG1CQUFBLE9BQU8sRUFBcUIsQ0FBQyxDQUFDLElBQUksR0FBRyxPQUFPLENBQUM7aUJBQy9DO3FCQUFNLElBQUksR0FBRyxFQUFFO29CQUNkLE9BQU8sR0FBRyxRQUFRLENBQUMsYUFBYSxDQUFDLE1BQU0sQ0FBQyxDQUFDO29CQUN6QyxPQUFPLENBQUMsSUFBSSxHQUFHLFVBQVUsQ0FBQztvQkFDMUIsQ0FBQyxtQkFBQSxPQUFPLEVBQW1CLENBQUMsQ0FBQyxHQUFHLEdBQUcsWUFBWSxDQUFDO29CQUVoRCxJQUFJLEdBQUcsRUFBRTt3QkFDUCxDQUFDLG1CQUFBLE9BQU8sRUFBbUIsQ0FBQyxDQUFDLElBQUksR0FBRyxHQUFHLENBQUM7cUJBQ3pDO2lCQUNGO3FCQUFNO29CQUNMLE9BQU8sR0FBRyxRQUFRLENBQUMsYUFBYSxDQUFDLE9BQU8sQ0FBQyxDQUFDO29CQUMxQyxDQUFDLG1CQUFBLE9BQU8sRUFBb0IsQ0FBQyxDQUFDLFdBQVcsR0FBRyxPQUFPLENBQUM7aUJBQ3JEO2dCQUVELE9BQU8sQ0FBQyxNQUFNOzs7Z0JBQUc7b0JBQ2YsS0FBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQztvQkFDakMsS0FBSSxDQUFDLGVBQWUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxRQUFRLEVBQUUsQ0FBQztvQkFFckMsSUFBSSxLQUFLLEtBQUssU0FBUyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7d0JBQ2xDLFVBQVUsQ0FBQyxJQUFJLEVBQUUsQ0FBQzt3QkFDbEIsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDO3FCQUN2QjtnQkFDSCxDQUFDLENBQUEsQ0FBQztnQkFFRixRQUFRLENBQUMsYUFBYSxDQUFDLFdBQVcsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLFFBQVEsRUFBRSxPQUFPLENBQUMsQ0FBQztZQUMvRSxDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7Z0JBdEVGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7OzswQkFORDtDQTJFQyxBQXZFRCxJQXVFQztTQXBFWSxlQUFlOzs7SUFDMUIsMENBQTZEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgUmVwbGF5U3ViamVjdCwgdGhyb3dFcnJvciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdXVpZCB9IGZyb20gJy4uL3V0aWxzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIExhenlMb2FkU2VydmljZSB7XG4gIGxvYWRlZExpYnJhcmllczogeyBbdXJsOiBzdHJpbmddOiBSZXBsYXlTdWJqZWN0PHZvaWQ+IH0gPSB7fTtcblxuICBsb2FkKFxuICAgIHVybE9yVXJsczogc3RyaW5nIHwgc3RyaW5nW10sXG4gICAgdHlwZTogJ3NjcmlwdCcgfCAnc3R5bGUnLFxuICAgIGNvbnRlbnQ6IHN0cmluZyA9ICcnLFxuICAgIHRhcmdldFF1ZXJ5OiBzdHJpbmcgPSAnYm9keScsXG4gICAgcG9zaXRpb246IEluc2VydFBvc2l0aW9uID0gJ2FmdGVyZW5kJyxcbiAgKTogT2JzZXJ2YWJsZTx2b2lkPiB7XG4gICAgaWYgKCF1cmxPclVybHMgJiYgIWNvbnRlbnQpIHtcbiAgICAgIHJldHVybiB0aHJvd0Vycm9yKCdTaG91bGQgcGFzcyB1cmwgb3IgY29udGVudCcpO1xuICAgIH0gZWxzZSBpZiAoIXVybE9yVXJscyAmJiBjb250ZW50KSB7XG4gICAgICB1cmxPclVybHMgPSBbbnVsbF07XG4gICAgfVxuXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KHVybE9yVXJscykpIHtcbiAgICAgIHVybE9yVXJscyA9IFt1cmxPclVybHNdO1xuICAgIH1cblxuICAgIHJldHVybiBuZXcgT2JzZXJ2YWJsZShzdWJzY3JpYmVyID0+IHtcbiAgICAgICh1cmxPclVybHMgYXMgc3RyaW5nW10pLmZvckVhY2goKHVybCwgaW5kZXgpID0+IHtcbiAgICAgICAgY29uc3Qga2V5ID0gdXJsID8gdXJsLnNsaWNlKHVybC5sYXN0SW5kZXhPZignLycpICsgMSkgOiB1dWlkKCk7XG5cbiAgICAgICAgaWYgKHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0pIHtcbiAgICAgICAgICBzdWJzY3JpYmVyLm5leHQoKTtcbiAgICAgICAgICBzdWJzY3JpYmVyLmNvbXBsZXRlKCk7XG4gICAgICAgICAgcmV0dXJuO1xuICAgICAgICB9XG5cbiAgICAgICAgdGhpcy5sb2FkZWRMaWJyYXJpZXNba2V5XSA9IG5ldyBSZXBsYXlTdWJqZWN0KCk7XG5cbiAgICAgICAgbGV0IGxpYnJhcnk7XG4gICAgICAgIGlmICh0eXBlID09PSAnc2NyaXB0Jykge1xuICAgICAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdzY3JpcHQnKTtcbiAgICAgICAgICBsaWJyYXJ5LnR5cGUgPSAndGV4dC9qYXZhc2NyaXB0JztcbiAgICAgICAgICBpZiAodXJsKSB7XG4gICAgICAgICAgICAobGlicmFyeSBhcyBIVE1MU2NyaXB0RWxlbWVudCkuc3JjID0gdXJsO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIChsaWJyYXJ5IGFzIEhUTUxTY3JpcHRFbGVtZW50KS50ZXh0ID0gY29udGVudDtcbiAgICAgICAgfSBlbHNlIGlmICh1cmwpIHtcbiAgICAgICAgICBsaWJyYXJ5ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnbGluaycpO1xuICAgICAgICAgIGxpYnJhcnkudHlwZSA9ICd0ZXh0L2Nzcyc7XG4gICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTExpbmtFbGVtZW50KS5yZWwgPSAnc3R5bGVzaGVldCc7XG5cbiAgICAgICAgICBpZiAodXJsKSB7XG4gICAgICAgICAgICAobGlicmFyeSBhcyBIVE1MTGlua0VsZW1lbnQpLmhyZWYgPSB1cmw7XG4gICAgICAgICAgfVxuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIGxpYnJhcnkgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdzdHlsZScpO1xuICAgICAgICAgIChsaWJyYXJ5IGFzIEhUTUxTdHlsZUVsZW1lbnQpLnRleHRDb250ZW50ID0gY29udGVudDtcbiAgICAgICAgfVxuXG4gICAgICAgIGxpYnJhcnkub25sb2FkID0gKCkgPT4ge1xuICAgICAgICAgIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0ubmV4dCgpO1xuICAgICAgICAgIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0uY29tcGxldGUoKTtcblxuICAgICAgICAgIGlmIChpbmRleCA9PT0gdXJsT3JVcmxzLmxlbmd0aCAtIDEpIHtcbiAgICAgICAgICAgIHN1YnNjcmliZXIubmV4dCgpO1xuICAgICAgICAgICAgc3Vic2NyaWJlci5jb21wbGV0ZSgpO1xuICAgICAgICAgIH1cbiAgICAgICAgfTtcblxuICAgICAgICBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKHRhcmdldFF1ZXJ5KS5pbnNlcnRBZGphY2VudEVsZW1lbnQocG9zaXRpb24sIGxpYnJhcnkpO1xuICAgICAgfSk7XG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==
