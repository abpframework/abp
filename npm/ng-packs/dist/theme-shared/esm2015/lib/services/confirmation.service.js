/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { MessageService } from 'primeng/components/common/messageservice';
import { fromEvent, Subject } from 'rxjs';
import { takeUntil, debounceTime, filter } from 'rxjs/operators';
import * as i0 from '@angular/core';
import * as i1 from 'primeng/components/common/messageservice';
export class ConfirmationService extends AbstractToaster {
  /**
   * @param {?} messageService
   */
  constructor(messageService) {
    super(messageService);
    this.messageService = messageService;
    this.key = 'abpConfirmation';
    this.sticky = true;
    this.destroy$ = new Subject();
  }
  /**
   * @param {?} message
   * @param {?} title
   * @param {?} severity
   * @param {?=} options
   * @return {?}
   */
  show(message, title, severity, options) {
    this.listenToEscape();
    return super.show(message, title, severity, options);
  }
  /**
   * @param {?=} status
   * @return {?}
   */
  clear(status) {
    super.clear(status);
    this.destroy$.next();
  }
  /**
   * @return {?}
   */
  listenToEscape() {
    fromEvent(document, 'keyup')
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(150),
        filter(
          /**
           * @param {?} key
           * @return {?}
           */
          key => key && key.code === 'Escape',
        ),
      )
      .subscribe(
        /**
         * @param {?} _
         * @return {?}
         */
        _ => {
          this.clear();
        },
      );
  }
}
ConfirmationService.decorators = [{ type: Injectable, args: [{ providedIn: 'root' }] }];
/** @nocollapse */
ConfirmationService.ctorParameters = () => [{ type: MessageService }];
/** @nocollapse */ ConfirmationService.ngInjectableDef = i0.ɵɵdefineInjectable({
  factory: function ConfirmationService_Factory() {
    return new ConfirmationService(i0.ɵɵinject(i1.MessageService));
  },
  token: ConfirmationService,
  providedIn: 'root',
});
if (false) {
  /** @type {?} */
  ConfirmationService.prototype.key;
  /** @type {?} */
  ConfirmationService.prototype.sticky;
  /** @type {?} */
  ConfirmationService.prototype.destroy$;
  /**
   * @type {?}
   * @protected
   */
  ConfirmationService.prototype.messageService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFFdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQzFFLE9BQU8sRUFBRSxTQUFTLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3RELE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLE1BQU0sRUFBRSxNQUFNLGdCQUFnQixDQUFDOzs7QUFJakUsTUFBTSxPQUFPLG1CQUFvQixTQUFRLGVBQXFDOzs7O0lBTzVFLFlBQXNCLGNBQThCO1FBQ2xELEtBQUssQ0FBQyxjQUFjLENBQUMsQ0FBQztRQURGLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQU5wRCxRQUFHLEdBQUcsaUJBQWlCLENBQUM7UUFFeEIsV0FBTSxHQUFHLElBQUksQ0FBQztRQUVkLGFBQVEsR0FBRyxJQUFJLE9BQU8sRUFBRSxDQUFDO0lBSXpCLENBQUM7Ozs7Ozs7O0lBRUQsSUFBSSxDQUNGLE9BQWUsRUFDZixLQUFhLEVBQ2IsUUFBMEIsRUFDMUIsT0FBOEI7UUFFOUIsSUFBSSxDQUFDLGNBQWMsRUFBRSxDQUFDO1FBRXRCLE9BQU8sS0FBSyxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxPQUFPLENBQUMsQ0FBQztJQUN2RCxDQUFDOzs7OztJQUVELEtBQUssQ0FBQyxNQUF1QjtRQUMzQixLQUFLLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDO1FBRXBCLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxFQUFFLENBQUM7SUFDdkIsQ0FBQzs7OztJQUVELGNBQWM7UUFDWixTQUFTLENBQUMsUUFBUSxFQUFFLE9BQU8sQ0FBQzthQUN6QixJQUFJLENBQ0gsU0FBUyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsRUFDeEIsWUFBWSxDQUFDLEdBQUcsQ0FBQyxFQUNqQixNQUFNOzs7O1FBQUMsQ0FBQyxHQUFrQixFQUFFLEVBQUUsQ0FBQyxHQUFHLElBQUksR0FBRyxDQUFDLElBQUksS0FBSyxRQUFRLEVBQUMsQ0FDN0Q7YUFDQSxTQUFTOzs7O1FBQUMsQ0FBQyxDQUFDLEVBQUU7WUFDYixJQUFJLENBQUMsS0FBSyxFQUFFLENBQUM7UUFDZixDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7OztZQXZDRixVQUFVLFNBQUMsRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFOzs7O1lBTHpCLGNBQWM7Ozs7O0lBT3JCLGtDQUF3Qjs7SUFFeEIscUNBQWM7O0lBRWQsdUNBQXlCOzs7OztJQUViLDZDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFic3RyYWN0VG9hc3RlciB9IGZyb20gJy4uL2Fic3RyYWN0cy90b2FzdGVyJztcbmltcG9ydCB7IENvbmZpcm1hdGlvbiB9IGZyb20gJy4uL21vZGVscy9jb25maXJtYXRpb24nO1xuaW1wb3J0IHsgTWVzc2FnZVNlcnZpY2UgfSBmcm9tICdwcmltZW5nL2NvbXBvbmVudHMvY29tbW9uL21lc3NhZ2VzZXJ2aWNlJztcbmltcG9ydCB7IGZyb21FdmVudCwgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdGFrZVVudGlsLCBkZWJvdW5jZVRpbWUsIGZpbHRlciB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi9tb2RlbHMvdG9hc3Rlcic7XG5cbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXG5leHBvcnQgY2xhc3MgQ29uZmlybWF0aW9uU2VydmljZSBleHRlbmRzIEFic3RyYWN0VG9hc3RlcjxDb25maXJtYXRpb24uT3B0aW9ucz4ge1xuICBrZXkgPSAnYWJwQ29uZmlybWF0aW9uJztcblxuICBzdGlja3kgPSB0cnVlO1xuXG4gIGRlc3Ryb3kkID0gbmV3IFN1YmplY3QoKTtcblxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgbWVzc2FnZVNlcnZpY2U6IE1lc3NhZ2VTZXJ2aWNlKSB7XG4gICAgc3VwZXIobWVzc2FnZVNlcnZpY2UpO1xuICB9XG5cbiAgc2hvdyhcbiAgICBtZXNzYWdlOiBzdHJpbmcsXG4gICAgdGl0bGU6IHN0cmluZyxcbiAgICBzZXZlcml0eTogVG9hc3Rlci5TZXZlcml0eSxcbiAgICBvcHRpb25zPzogQ29uZmlybWF0aW9uLk9wdGlvbnNcbiAgKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHRoaXMubGlzdGVuVG9Fc2NhcGUoKTtcblxuICAgIHJldHVybiBzdXBlci5zaG93KG1lc3NhZ2UsIHRpdGxlLCBzZXZlcml0eSwgb3B0aW9ucyk7XG4gIH1cblxuICBjbGVhcihzdGF0dXM/OiBUb2FzdGVyLlN0YXR1cykge1xuICAgIHN1cGVyLmNsZWFyKHN0YXR1cyk7XG5cbiAgICB0aGlzLmRlc3Ryb3kkLm5leHQoKTtcbiAgfVxuXG4gIGxpc3RlblRvRXNjYXBlKCkge1xuICAgIGZyb21FdmVudChkb2N1bWVudCwgJ2tleXVwJylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWwodGhpcy5kZXN0cm95JCksXG4gICAgICAgIGRlYm91bmNlVGltZSgxNTApLFxuICAgICAgICBmaWx0ZXIoKGtleTogS2V5Ym9hcmRFdmVudCkgPT4ga2V5ICYmIGtleS5jb2RlID09PSAnRXNjYXBlJylcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoXyA9PiB7XG4gICAgICAgIHRoaXMuY2xlYXIoKTtcbiAgICAgIH0pO1xuICB9XG59XG4iXX0=
