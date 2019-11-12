/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, throwError } from 'rxjs';
import { uuid } from '../utils';
import * as i0 from '@angular/core';
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
  load(urlOrUrls, type, content = '', targetQuery = 'body', position = 'afterend') {
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
     */(subscriber => {
      /** @type {?} */ (urlOrUrls).forEach(
        /**
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
           */ = () => {
            this.loadedLibraries[key].next();
            this.loadedLibraries[key].complete();
            if (index === urlOrUrls.length - 1) {
              subscriber.next();
              subscriber.complete();
            }
          };
          document.querySelector(targetQuery).insertAdjacentElement(position, library);
        },
      );
    });
  }
}
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
if (false) {
  /** @type {?} */
  LazyLoadService.prototype.loadedLibraries;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF6eS1sb2FkLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbGF6eS1sb2FkLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFVBQVUsRUFBRSxhQUFhLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzdELE9BQU8sRUFBRSxJQUFJLEVBQUUsTUFBTSxVQUFVLENBQUM7O0FBS2hDLE1BQU0sT0FBTyxlQUFlO0lBSDVCO1FBSUUsb0JBQWUsR0FBMkMsRUFBRSxDQUFDO0tBbUU5RDs7Ozs7Ozs7O0lBakVDLElBQUksQ0FDRixTQUE0QixFQUM1QixJQUF3QixFQUN4QixVQUFrQixFQUFFLEVBQ3BCLGNBQXNCLE1BQU0sRUFDNUIsV0FBMkIsVUFBVTtRQUVyQyxJQUFJLENBQUMsU0FBUyxJQUFJLENBQUMsT0FBTyxFQUFFO1lBQzFCLE9BQU8sVUFBVSxDQUFDLDRCQUE0QixDQUFDLENBQUM7U0FDakQ7YUFBTSxJQUFJLENBQUMsU0FBUyxJQUFJLE9BQU8sRUFBRTtZQUNoQyxTQUFTLEdBQUcsQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUNwQjtRQUVELElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLFNBQVMsQ0FBQyxFQUFFO1lBQzdCLFNBQVMsR0FBRyxDQUFDLFNBQVMsQ0FBQyxDQUFDO1NBQ3pCO1FBRUQsT0FBTyxJQUFJLFVBQVU7Ozs7UUFBQyxVQUFVLENBQUMsRUFBRTtZQUNqQyxDQUFDLG1CQUFBLFNBQVMsRUFBWSxDQUFDLENBQUMsT0FBTzs7Ozs7WUFBQyxDQUFDLEdBQUcsRUFBRSxLQUFLLEVBQUUsRUFBRTs7c0JBQ3ZDLEdBQUcsR0FBRyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLFdBQVcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUFFO2dCQUU5RCxJQUFJLElBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLEVBQUU7b0JBQzdCLFVBQVUsQ0FBQyxJQUFJLEVBQUUsQ0FBQztvQkFDbEIsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDO29CQUN0QixPQUFPO2lCQUNSO2dCQUVELElBQUksQ0FBQyxlQUFlLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxhQUFhLEVBQUUsQ0FBQzs7b0JBRTVDLE9BQU87Z0JBQ1gsSUFBSSxJQUFJLEtBQUssUUFBUSxFQUFFO29CQUNyQixPQUFPLEdBQUcsUUFBUSxDQUFDLGFBQWEsQ0FBQyxRQUFRLENBQUMsQ0FBQztvQkFDM0MsT0FBTyxDQUFDLElBQUksR0FBRyxpQkFBaUIsQ0FBQztvQkFDakMsSUFBSSxHQUFHLEVBQUU7d0JBQ1AsQ0FBQyxtQkFBQSxPQUFPLEVBQXFCLENBQUMsQ0FBQyxHQUFHLEdBQUcsR0FBRyxDQUFDO3FCQUMxQztvQkFFRCxDQUFDLG1CQUFBLE9BQU8sRUFBcUIsQ0FBQyxDQUFDLElBQUksR0FBRyxPQUFPLENBQUM7aUJBQy9DO3FCQUFNLElBQUksR0FBRyxFQUFFO29CQUNkLE9BQU8sR0FBRyxRQUFRLENBQUMsYUFBYSxDQUFDLE1BQU0sQ0FBQyxDQUFDO29CQUN6QyxPQUFPLENBQUMsSUFBSSxHQUFHLFVBQVUsQ0FBQztvQkFDMUIsQ0FBQyxtQkFBQSxPQUFPLEVBQW1CLENBQUMsQ0FBQyxHQUFHLEdBQUcsWUFBWSxDQUFDO29CQUVoRCxJQUFJLEdBQUcsRUFBRTt3QkFDUCxDQUFDLG1CQUFBLE9BQU8sRUFBbUIsQ0FBQyxDQUFDLElBQUksR0FBRyxHQUFHLENBQUM7cUJBQ3pDO2lCQUNGO3FCQUFNO29CQUNMLE9BQU8sR0FBRyxRQUFRLENBQUMsYUFBYSxDQUFDLE9BQU8sQ0FBQyxDQUFDO29CQUMxQyxDQUFDLG1CQUFBLE9BQU8sRUFBb0IsQ0FBQyxDQUFDLFdBQVcsR0FBRyxPQUFPLENBQUM7aUJBQ3JEO2dCQUVELE9BQU8sQ0FBQyxNQUFNOzs7Z0JBQUcsR0FBRyxFQUFFO29CQUNwQixJQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxDQUFDLElBQUksRUFBRSxDQUFDO29CQUNqQyxJQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxDQUFDLFFBQVEsRUFBRSxDQUFDO29CQUVyQyxJQUFJLEtBQUssS0FBSyxTQUFTLENBQUMsTUFBTSxHQUFHLENBQUMsRUFBRTt3QkFDbEMsVUFBVSxDQUFDLElBQUksRUFBRSxDQUFDO3dCQUNsQixVQUFVLENBQUMsUUFBUSxFQUFFLENBQUM7cUJBQ3ZCO2dCQUNILENBQUMsQ0FBQSxDQUFDO2dCQUVGLFFBQVEsQ0FBQyxhQUFhLENBQUMsV0FBVyxDQUFDLENBQUMscUJBQXFCLENBQUMsUUFBUSxFQUFFLE9BQU8sQ0FBQyxDQUFDO1lBQy9FLENBQUMsRUFBQyxDQUFDO1FBQ0wsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOzs7WUF0RUYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7OztJQUVDLDBDQUE2RCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUsIFJlcGxheVN1YmplY3QsIHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHV1aWQgfSBmcm9tICcuLi91dGlscyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBMYXp5TG9hZFNlcnZpY2Uge1xuICBsb2FkZWRMaWJyYXJpZXM6IHsgW3VybDogc3RyaW5nXTogUmVwbGF5U3ViamVjdDx2b2lkPiB9ID0ge307XG5cbiAgbG9hZChcbiAgICB1cmxPclVybHM6IHN0cmluZyB8IHN0cmluZ1tdLFxuICAgIHR5cGU6ICdzY3JpcHQnIHwgJ3N0eWxlJyxcbiAgICBjb250ZW50OiBzdHJpbmcgPSAnJyxcbiAgICB0YXJnZXRRdWVyeTogc3RyaW5nID0gJ2JvZHknLFxuICAgIHBvc2l0aW9uOiBJbnNlcnRQb3NpdGlvbiA9ICdhZnRlcmVuZCcsXG4gICk6IE9ic2VydmFibGU8dm9pZD4ge1xuICAgIGlmICghdXJsT3JVcmxzICYmICFjb250ZW50KSB7XG4gICAgICByZXR1cm4gdGhyb3dFcnJvcignU2hvdWxkIHBhc3MgdXJsIG9yIGNvbnRlbnQnKTtcbiAgICB9IGVsc2UgaWYgKCF1cmxPclVybHMgJiYgY29udGVudCkge1xuICAgICAgdXJsT3JVcmxzID0gW251bGxdO1xuICAgIH1cblxuICAgIGlmICghQXJyYXkuaXNBcnJheSh1cmxPclVybHMpKSB7XG4gICAgICB1cmxPclVybHMgPSBbdXJsT3JVcmxzXTtcbiAgICB9XG5cbiAgICByZXR1cm4gbmV3IE9ic2VydmFibGUoc3Vic2NyaWJlciA9PiB7XG4gICAgICAodXJsT3JVcmxzIGFzIHN0cmluZ1tdKS5mb3JFYWNoKCh1cmwsIGluZGV4KSA9PiB7XG4gICAgICAgIGNvbnN0IGtleSA9IHVybCA/IHVybC5zbGljZSh1cmwubGFzdEluZGV4T2YoJy8nKSArIDEpIDogdXVpZCgpO1xuXG4gICAgICAgIGlmICh0aGlzLmxvYWRlZExpYnJhcmllc1trZXldKSB7XG4gICAgICAgICAgc3Vic2NyaWJlci5uZXh0KCk7XG4gICAgICAgICAgc3Vic2NyaWJlci5jb21wbGV0ZSgpO1xuICAgICAgICAgIHJldHVybjtcbiAgICAgICAgfVxuXG4gICAgICAgIHRoaXMubG9hZGVkTGlicmFyaWVzW2tleV0gPSBuZXcgUmVwbGF5U3ViamVjdCgpO1xuXG4gICAgICAgIGxldCBsaWJyYXJ5O1xuICAgICAgICBpZiAodHlwZSA9PT0gJ3NjcmlwdCcpIHtcbiAgICAgICAgICBsaWJyYXJ5ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnc2NyaXB0Jyk7XG4gICAgICAgICAgbGlicmFyeS50eXBlID0gJ3RleHQvamF2YXNjcmlwdCc7XG4gICAgICAgICAgaWYgKHVybCkge1xuICAgICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTFNjcmlwdEVsZW1lbnQpLnNyYyA9IHVybDtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICAobGlicmFyeSBhcyBIVE1MU2NyaXB0RWxlbWVudCkudGV4dCA9IGNvbnRlbnQ7XG4gICAgICAgIH0gZWxzZSBpZiAodXJsKSB7XG4gICAgICAgICAgbGlicmFyeSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2xpbmsnKTtcbiAgICAgICAgICBsaWJyYXJ5LnR5cGUgPSAndGV4dC9jc3MnO1xuICAgICAgICAgIChsaWJyYXJ5IGFzIEhUTUxMaW5rRWxlbWVudCkucmVsID0gJ3N0eWxlc2hlZXQnO1xuXG4gICAgICAgICAgaWYgKHVybCkge1xuICAgICAgICAgICAgKGxpYnJhcnkgYXMgSFRNTExpbmtFbGVtZW50KS5ocmVmID0gdXJsO1xuICAgICAgICAgIH1cbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICBsaWJyYXJ5ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnc3R5bGUnKTtcbiAgICAgICAgICAobGlicmFyeSBhcyBIVE1MU3R5bGVFbGVtZW50KS50ZXh0Q29udGVudCA9IGNvbnRlbnQ7XG4gICAgICAgIH1cblxuICAgICAgICBsaWJyYXJ5Lm9ubG9hZCA9ICgpID0+IHtcbiAgICAgICAgICB0aGlzLmxvYWRlZExpYnJhcmllc1trZXldLm5leHQoKTtcbiAgICAgICAgICB0aGlzLmxvYWRlZExpYnJhcmllc1trZXldLmNvbXBsZXRlKCk7XG5cbiAgICAgICAgICBpZiAoaW5kZXggPT09IHVybE9yVXJscy5sZW5ndGggLSAxKSB7XG4gICAgICAgICAgICBzdWJzY3JpYmVyLm5leHQoKTtcbiAgICAgICAgICAgIHN1YnNjcmliZXIuY29tcGxldGUoKTtcbiAgICAgICAgICB9XG4gICAgICAgIH07XG5cbiAgICAgICAgZG9jdW1lbnQucXVlcnlTZWxlY3Rvcih0YXJnZXRRdWVyeSkuaW5zZXJ0QWRqYWNlbnRFbGVtZW50KHBvc2l0aW9uLCBsaWJyYXJ5KTtcbiAgICAgIH0pO1xuICAgIH0pO1xuICB9XG59XG4iXX0=
