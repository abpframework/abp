/**
 * @fileoverview added by tsickle
 * Generated from: lib/abstracts/toaster.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Subject } from 'rxjs';
/**
 * @abstract
 * @template T
 */
export class AbstractToaster {
    /**
     * @param {?} messageService
     */
    constructor(messageService) {
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
    info(message, title, options) {
        return this.show(message, title, 'info', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    success(message, title, options) {
        return this.show(message, title, 'success', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    warn(message, title, options) {
        return this.show(message, title, 'warn', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    error(message, title, options) {
        return this.show(message, title, 'error', options);
    }
    /**
     * @protected
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    show(message, title, severity, options) {
        this.messageService.clear(this.key);
        this.messageService.add(Object.assign({ severity, detail: message || '', summary: title || '' }, options, { key: this.key }, (typeof (options || ((/** @type {?} */ ({})))).sticky === 'undefined' && { sticky: this.sticky })));
        this.status$ = new Subject();
        return this.status$;
    }
    /**
     * @param {?=} status
     * @return {?}
     */
    clear(status) {
        this.messageService.clear(this.key);
        this.status$.next(status || "dismiss" /* dismiss */);
        this.status$.complete();
    }
}
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy90b2FzdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQ0EsT0FBTyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQzs7Ozs7QUFJM0MsTUFBTSxPQUFnQixlQUFlOzs7O0lBT25DLFlBQXNCLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUpwRCxRQUFHLEdBQUcsVUFBVSxDQUFDO1FBRWpCLFdBQU0sR0FBRyxLQUFLLENBQUM7SUFFd0MsQ0FBQzs7Ozs7OztJQUV4RCxJQUFJLENBQUMsT0FBaUMsRUFBRSxLQUErQixFQUFFLE9BQVc7UUFDbEYsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCxPQUFPLENBQUMsT0FBaUMsRUFBRSxLQUErQixFQUFFLE9BQVc7UUFDckYsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsU0FBUyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3ZELENBQUM7Ozs7Ozs7SUFFRCxJQUFJLENBQUMsT0FBaUMsRUFBRSxLQUErQixFQUFFLE9BQVc7UUFDbEYsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCxLQUFLLENBQUMsT0FBaUMsRUFBRSxLQUErQixFQUFFLE9BQVc7UUFDbkYsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsT0FBTyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3JELENBQUM7Ozs7Ozs7OztJQUVTLElBQUksQ0FDWixPQUFpQyxFQUNqQyxLQUErQixFQUMvQixRQUEwQixFQUMxQixPQUFXO1FBRVgsSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBRXBDLElBQUksQ0FBQyxjQUFjLENBQUMsR0FBRyxpQkFDckIsUUFBUSxFQUNSLE1BQU0sRUFBRSxPQUFPLElBQUksRUFBRSxFQUNyQixPQUFPLEVBQUUsS0FBSyxJQUFJLEVBQUUsSUFDakIsT0FBTyxJQUNWLEdBQUcsRUFBRSxJQUFJLENBQUMsR0FBRyxJQUNWLENBQUMsT0FBTyxDQUFDLE9BQU8sSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBTyxDQUFDLENBQUMsQ0FBQyxNQUFNLEtBQUssV0FBVyxJQUFJLEVBQUUsTUFBTSxFQUFFLElBQUksQ0FBQyxNQUFNLEVBQUUsQ0FBQyxFQUN0RixDQUFDO1FBQ0gsSUFBSSxDQUFDLE9BQU8sR0FBRyxJQUFJLE9BQU8sRUFBa0IsQ0FBQztRQUM3QyxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUM7SUFDdEIsQ0FBQzs7Ozs7SUFFRCxLQUFLLENBQUMsTUFBdUI7UUFDM0IsSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBQ3BDLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLE1BQU0sMkJBQTBCLENBQUMsQ0FBQztRQUNwRCxJQUFJLENBQUMsT0FBTyxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQzFCLENBQUM7Q0FDRjs7O0lBakRDLGtDQUFpQzs7SUFFakMsOEJBQWlCOztJQUVqQixpQ0FBZTs7Ozs7SUFFSCx5Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBNZXNzYWdlU2VydmljZSB9IGZyb20gJ3ByaW1lbmcvY29tcG9uZW50cy9jb21tb24vbWVzc2FnZXNlcnZpY2UnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uL21vZGVscy90b2FzdGVyJztcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbmV4cG9ydCBhYnN0cmFjdCBjbGFzcyBBYnN0cmFjdFRvYXN0ZXI8VCA9IFRvYXN0ZXIuT3B0aW9ucz4ge1xuICBzdGF0dXMkOiBTdWJqZWN0PFRvYXN0ZXIuU3RhdHVzPjtcblxuICBrZXkgPSAnYWJwVG9hc3QnO1xuXG4gIHN0aWNreSA9IGZhbHNlO1xuXG4gIGNvbnN0cnVjdG9yKHByb3RlY3RlZCBtZXNzYWdlU2VydmljZTogTWVzc2FnZVNlcnZpY2UpIHt9XG5cbiAgaW5mbyhtZXNzYWdlOiBDb25maWcuTG9jYWxpemF0aW9uUGFyYW0sIHRpdGxlOiBDb25maWcuTG9jYWxpemF0aW9uUGFyYW0sIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdpbmZvJywgb3B0aW9ucyk7XG4gIH1cblxuICBzdWNjZXNzKG1lc3NhZ2U6IENvbmZpZy5Mb2NhbGl6YXRpb25QYXJhbSwgdGl0bGU6IENvbmZpZy5Mb2NhbGl6YXRpb25QYXJhbSwgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ3N1Y2Nlc3MnLCBvcHRpb25zKTtcbiAgfVxuXG4gIHdhcm4obWVzc2FnZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLCB0aXRsZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnd2FybicsIG9wdGlvbnMpO1xuICB9XG5cbiAgZXJyb3IobWVzc2FnZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLCB0aXRsZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnZXJyb3InLCBvcHRpb25zKTtcbiAgfVxuXG4gIHByb3RlY3RlZCBzaG93KFxuICAgIG1lc3NhZ2U6IENvbmZpZy5Mb2NhbGl6YXRpb25QYXJhbSxcbiAgICB0aXRsZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLFxuICAgIHNldmVyaXR5OiBUb2FzdGVyLlNldmVyaXR5LFxuICAgIG9wdGlvbnM/OiBULFxuICApOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5jbGVhcih0aGlzLmtleSk7XG5cbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmFkZCh7XG4gICAgICBzZXZlcml0eSxcbiAgICAgIGRldGFpbDogbWVzc2FnZSB8fCAnJyxcbiAgICAgIHN1bW1hcnk6IHRpdGxlIHx8ICcnLFxuICAgICAgLi4ub3B0aW9ucyxcbiAgICAgIGtleTogdGhpcy5rZXksXG4gICAgICAuLi4odHlwZW9mIChvcHRpb25zIHx8ICh7fSBhcyBhbnkpKS5zdGlja3kgPT09ICd1bmRlZmluZWQnICYmIHsgc3RpY2t5OiB0aGlzLnN0aWNreSB9KSxcbiAgICB9KTtcbiAgICB0aGlzLnN0YXR1cyQgPSBuZXcgU3ViamVjdDxUb2FzdGVyLlN0YXR1cz4oKTtcbiAgICByZXR1cm4gdGhpcy5zdGF0dXMkO1xuICB9XG5cbiAgY2xlYXIoc3RhdHVzPzogVG9hc3Rlci5TdGF0dXMpIHtcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmNsZWFyKHRoaXMua2V5KTtcbiAgICB0aGlzLnN0YXR1cyQubmV4dChzdGF0dXMgfHwgVG9hc3Rlci5TdGF0dXMuZGlzbWlzcyk7XG4gICAgdGhpcy5zdGF0dXMkLmNvbXBsZXRlKCk7XG4gIH1cbn1cbiJdfQ==