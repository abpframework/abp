/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Subject } from 'rxjs';
/**
 * @abstract
 * @template T
 */
var /**
   * @abstract
   * @template T
   */
  AbstractToaster = /** @class */ (function() {
    function AbstractToaster(messageService) {
      this.messageService = messageService;
      this.key = 'abpToast';
      this.sticky = false;
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.info
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */ = function(message, title, options) {
      return this.show(message, title, 'info', options);
    };
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.success
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */ = function(message, title, options) {
      return this.show(message, title, 'success', options);
    };
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.warn
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */ = function(message, title, options) {
      return this.show(message, title, 'warn', options);
    };
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.error
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */ = function(message, title, options) {
      return this.show(message, title, 'error', options);
    };
    /**
     * @protected
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.show
    /**
     * @protected
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */ = function(message, title, severity, options) {
      this.messageService.clear(this.key);
      this.messageService.add(
        tslib_1.__assign(
          { severity: severity, detail: message || '', summary: title || '' },
          options,
          { key: this.key },
          typeof (options || /** @type {?} */ ({})).sticky === 'undefined' && { sticky: this.sticky },
        ),
      );
      this.status$ = new Subject();
      return this.status$;
    };
    /**
     * @param {?=} status
     * @return {?}
     */
    AbstractToaster.prototype.clear
    /**
     * @param {?=} status
     * @return {?}
     */ = function(status) {
      this.messageService.clear(this.key);
      this.status$.next(status || 'dismiss' /* dismiss */);
      this.status$.complete();
    };
    return AbstractToaster;
  })();
/**
 * @abstract
 * @template T
 */
export { AbstractToaster };
if (false) {
  /** @type {?} */
  AbstractToaster.prototype.status$;
  /** @type {?} */
  AbstractToaster.prototype.key;
  /** @type {?} */
  AbstractToaster.prototype.sticky;
  /**
   * @type {?}
   * @protected
   */
  AbstractToaster.prototype.messageService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy90b2FzdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQ0EsT0FBTyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQzs7Ozs7QUFHM0M7Ozs7O0lBT0UseUJBQXNCLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUpwRCxRQUFHLEdBQUcsVUFBVSxDQUFDO1FBRWpCLFdBQU0sR0FBRyxLQUFLLENBQUM7SUFFd0MsQ0FBQzs7Ozs7OztJQUV4RCw4QkFBSTs7Ozs7O0lBQUosVUFBSyxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDOUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCxpQ0FBTzs7Ozs7O0lBQVAsVUFBUSxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDakQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsU0FBUyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3ZELENBQUM7Ozs7Ozs7SUFFRCw4QkFBSTs7Ozs7O0lBQUosVUFBSyxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDOUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCwrQkFBSzs7Ozs7O0lBQUwsVUFBTSxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDL0MsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsT0FBTyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3JELENBQUM7Ozs7Ozs7OztJQUVTLDhCQUFJOzs7Ozs7OztJQUFkLFVBQWUsT0FBZSxFQUFFLEtBQWEsRUFBRSxRQUEwQixFQUFFLE9BQVc7UUFDcEYsSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBRXBDLElBQUksQ0FBQyxjQUFjLENBQUMsR0FBRyxvQkFDckIsUUFBUSxVQUFBLEVBQ1IsTUFBTSxFQUFFLE9BQU8sSUFBSSxFQUFFLEVBQ3JCLE9BQU8sRUFBRSxLQUFLLElBQUksRUFBRSxJQUNqQixPQUFPLElBQ1YsR0FBRyxFQUFFLElBQUksQ0FBQyxHQUFHLElBQ1YsQ0FBQyxPQUFPLENBQUMsT0FBTyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFPLENBQUMsQ0FBQyxDQUFDLE1BQU0sS0FBSyxXQUFXLElBQUksRUFBRSxNQUFNLEVBQUUsSUFBSSxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQ3RGLENBQUM7UUFDSCxJQUFJLENBQUMsT0FBTyxHQUFHLElBQUksT0FBTyxFQUFrQixDQUFDO1FBQzdDLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQztJQUN0QixDQUFDOzs7OztJQUVELCtCQUFLOzs7O0lBQUwsVUFBTSxNQUF1QjtRQUMzQixJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDcEMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsTUFBTSwyQkFBMEIsQ0FBQyxDQUFDO1FBQ3BELElBQUksQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDMUIsQ0FBQztJQUNILHNCQUFDO0FBQUQsQ0FBQyxBQTdDRCxJQTZDQzs7Ozs7Ozs7SUE1Q0Msa0NBQWlDOztJQUVqQyw4QkFBaUI7O0lBRWpCLGlDQUFlOzs7OztJQUVILHlDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlLCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vbW9kZWxzL3RvYXN0ZXInO1xuXG5leHBvcnQgYWJzdHJhY3QgY2xhc3MgQWJzdHJhY3RUb2FzdGVyPFQgPSBUb2FzdGVyLk9wdGlvbnM+IHtcbiAgc3RhdHVzJDogU3ViamVjdDxUb2FzdGVyLlN0YXR1cz47XG5cbiAga2V5ID0gJ2FicFRvYXN0JztcblxuICBzdGlja3kgPSBmYWxzZTtcblxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgbWVzc2FnZVNlcnZpY2U6IE1lc3NhZ2VTZXJ2aWNlKSB7fVxuXG4gIGluZm8obWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnaW5mbycsIG9wdGlvbnMpO1xuICB9XG5cbiAgc3VjY2VzcyhtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdzdWNjZXNzJywgb3B0aW9ucyk7XG4gIH1cblxuICB3YXJuKG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ3dhcm4nLCBvcHRpb25zKTtcbiAgfVxuXG4gIGVycm9yKG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ2Vycm9yJywgb3B0aW9ucyk7XG4gIH1cblxuICBwcm90ZWN0ZWQgc2hvdyhtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIHNldmVyaXR5OiBUb2FzdGVyLlNldmVyaXR5LCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmNsZWFyKHRoaXMua2V5KTtcblxuICAgIHRoaXMubWVzc2FnZVNlcnZpY2UuYWRkKHtcbiAgICAgIHNldmVyaXR5LFxuICAgICAgZGV0YWlsOiBtZXNzYWdlIHx8ICcnLFxuICAgICAgc3VtbWFyeTogdGl0bGUgfHwgJycsXG4gICAgICAuLi5vcHRpb25zLFxuICAgICAga2V5OiB0aGlzLmtleSxcbiAgICAgIC4uLih0eXBlb2YgKG9wdGlvbnMgfHwgKHt9IGFzIGFueSkpLnN0aWNreSA9PT0gJ3VuZGVmaW5lZCcgJiYgeyBzdGlja3k6IHRoaXMuc3RpY2t5IH0pXG4gICAgfSk7XG4gICAgdGhpcy5zdGF0dXMkID0gbmV3IFN1YmplY3Q8VG9hc3Rlci5TdGF0dXM+KCk7XG4gICAgcmV0dXJuIHRoaXMuc3RhdHVzJDtcbiAgfVxuXG4gIGNsZWFyKHN0YXR1cz86IFRvYXN0ZXIuU3RhdHVzKSB7XG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5jbGVhcih0aGlzLmtleSk7XG4gICAgdGhpcy5zdGF0dXMkLm5leHQoc3RhdHVzIHx8IFRvYXN0ZXIuU3RhdHVzLmRpc21pc3MpO1xuICAgIHRoaXMuc3RhdHVzJC5jb21wbGV0ZSgpO1xuICB9XG59XG4iXX0=
