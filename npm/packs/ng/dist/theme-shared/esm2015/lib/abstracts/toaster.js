/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Subject } from 'rxjs';
/**
 * @template T
 */
export class AbstractToasterClass {
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
        this.messageService.add(Object.assign({ severity, detail: message, summary: title }, options, { key: this.key }, (typeof (options || ((/** @type {?} */ ({})))).sticky === 'undefined' && { sticky: this.sticky })));
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
    /**
     * @type {?}
     * @protected
     */
    AbstractToasterClass.prototype.status$;
    /**
     * @type {?}
     * @protected
     */
    AbstractToasterClass.prototype.key;
    /**
     * @type {?}
     * @protected
     */
    AbstractToasterClass.prototype.sticky;
    /**
     * @type {?}
     * @protected
     */
    AbstractToasterClass.prototype.messageService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy90b2FzdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFDQSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDOzs7O0FBRzNDLE1BQU0sT0FBTyxvQkFBb0I7Ozs7SUFPL0IsWUFBc0IsY0FBOEI7UUFBOUIsbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBSjFDLFFBQUcsR0FBVyxVQUFVLENBQUM7UUFFekIsV0FBTSxHQUFZLEtBQUssQ0FBQztJQUVxQixDQUFDOzs7Ozs7O0lBQ3hELElBQUksQ0FBQyxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDOUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCxPQUFPLENBQUMsT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQ2pELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLFNBQVMsRUFBRSxPQUFPLENBQUMsQ0FBQztJQUN2RCxDQUFDOzs7Ozs7O0lBRUQsSUFBSSxDQUFDLE9BQWUsRUFBRSxLQUFhLEVBQUUsT0FBVztRQUM5QyxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDcEQsQ0FBQzs7Ozs7OztJQUVELEtBQUssQ0FBQyxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDL0MsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsT0FBTyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3JELENBQUM7Ozs7Ozs7OztJQUVTLElBQUksQ0FBQyxPQUFlLEVBQUUsS0FBYSxFQUFFLFFBQTBCLEVBQUUsT0FBVztRQUNwRixJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUM7UUFFcEMsSUFBSSxDQUFDLGNBQWMsQ0FBQyxHQUFHLGlCQUNyQixRQUFRLEVBQ1IsTUFBTSxFQUFFLE9BQU8sRUFDZixPQUFPLEVBQUUsS0FBSyxJQUNYLE9BQU8sSUFDVixHQUFHLEVBQUUsSUFBSSxDQUFDLEdBQUcsSUFDVixDQUFDLE9BQU8sQ0FBQyxPQUFPLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQU8sQ0FBQyxDQUFDLENBQUMsTUFBTSxLQUFLLFdBQVcsSUFBSSxFQUFFLE1BQU0sRUFBRSxJQUFJLENBQUMsTUFBTSxFQUFFLENBQUMsRUFDdEYsQ0FBQztRQUNILElBQUksQ0FBQyxPQUFPLEdBQUcsSUFBSSxPQUFPLEVBQWtCLENBQUM7UUFDN0MsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDO0lBQ3RCLENBQUM7Ozs7O0lBRUQsS0FBSyxDQUFDLE1BQXVCO1FBQzNCLElBQUksQ0FBQyxjQUFjLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUNwQyxJQUFJLENBQUMsT0FBTyxDQUFDLElBQUksQ0FBQyxNQUFNLDJCQUEwQixDQUFDLENBQUM7UUFDcEQsSUFBSSxDQUFDLE9BQU8sQ0FBQyxRQUFRLEVBQUUsQ0FBQztJQUMxQixDQUFDO0NBQ0Y7Ozs7OztJQTNDQyx1Q0FBMkM7Ozs7O0lBRTNDLG1DQUFtQzs7Ozs7SUFFbkMsc0NBQWtDOzs7OztJQUV0Qiw4Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBNZXNzYWdlU2VydmljZSB9IGZyb20gJ3ByaW1lbmcvY29tcG9uZW50cy9jb21tb24vbWVzc2FnZXNlcnZpY2UnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uL21vZGVscy90b2FzdGVyJztcblxuZXhwb3J0IGNsYXNzIEFic3RyYWN0VG9hc3RlckNsYXNzPFQgPSBUb2FzdGVyLk9wdGlvbnM+IHtcbiAgcHJvdGVjdGVkIHN0YXR1cyQ6IFN1YmplY3Q8VG9hc3Rlci5TdGF0dXM+O1xuXG4gIHByb3RlY3RlZCBrZXk6IHN0cmluZyA9ICdhYnBUb2FzdCc7XG5cbiAgcHJvdGVjdGVkIHN0aWNreTogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGNvbnN0cnVjdG9yKHByb3RlY3RlZCBtZXNzYWdlU2VydmljZTogTWVzc2FnZVNlcnZpY2UpIHt9XG4gIGluZm8obWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnaW5mbycsIG9wdGlvbnMpO1xuICB9XG5cbiAgc3VjY2VzcyhtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdzdWNjZXNzJywgb3B0aW9ucyk7XG4gIH1cblxuICB3YXJuKG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ3dhcm4nLCBvcHRpb25zKTtcbiAgfVxuXG4gIGVycm9yKG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ2Vycm9yJywgb3B0aW9ucyk7XG4gIH1cblxuICBwcm90ZWN0ZWQgc2hvdyhtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIHNldmVyaXR5OiBUb2FzdGVyLlNldmVyaXR5LCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmNsZWFyKHRoaXMua2V5KTtcblxuICAgIHRoaXMubWVzc2FnZVNlcnZpY2UuYWRkKHtcbiAgICAgIHNldmVyaXR5LFxuICAgICAgZGV0YWlsOiBtZXNzYWdlLFxuICAgICAgc3VtbWFyeTogdGl0bGUsXG4gICAgICAuLi5vcHRpb25zLFxuICAgICAga2V5OiB0aGlzLmtleSxcbiAgICAgIC4uLih0eXBlb2YgKG9wdGlvbnMgfHwgKHt9IGFzIGFueSkpLnN0aWNreSA9PT0gJ3VuZGVmaW5lZCcgJiYgeyBzdGlja3k6IHRoaXMuc3RpY2t5IH0pLFxuICAgIH0pO1xuICAgIHRoaXMuc3RhdHVzJCA9IG5ldyBTdWJqZWN0PFRvYXN0ZXIuU3RhdHVzPigpO1xuICAgIHJldHVybiB0aGlzLnN0YXR1cyQ7XG4gIH1cblxuICBjbGVhcihzdGF0dXM/OiBUb2FzdGVyLlN0YXR1cykge1xuICAgIHRoaXMubWVzc2FnZVNlcnZpY2UuY2xlYXIodGhpcy5rZXkpO1xuICAgIHRoaXMuc3RhdHVzJC5uZXh0KHN0YXR1cyB8fCBUb2FzdGVyLlN0YXR1cy5kaXNtaXNzKTtcbiAgICB0aGlzLnN0YXR1cyQuY29tcGxldGUoKTtcbiAgfVxufVxuIl19