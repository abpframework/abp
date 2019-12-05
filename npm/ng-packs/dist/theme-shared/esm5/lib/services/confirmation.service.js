/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/confirmation.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { MessageService } from 'primeng/components/common/messageservice';
import { fromEvent, Subject } from 'rxjs';
import { takeUntil, debounceTime, filter } from 'rxjs/operators';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
var ConfirmationService = /** @class */ (function (_super) {
    tslib_1.__extends(ConfirmationService, _super);
    function ConfirmationService(messageService) {
        var _this = _super.call(this, messageService) || this;
        _this.messageService = messageService;
        _this.key = 'abpConfirmation';
        _this.sticky = true;
        _this.destroy$ = new Subject();
        return _this;
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    ConfirmationService.prototype.show = /**
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    function (message, title, severity, options) {
        this.listenToEscape();
        return _super.prototype.show.call(this, message, title, severity, options);
    };
    /**
     * @param {?=} status
     * @return {?}
     */
    ConfirmationService.prototype.clear = /**
     * @param {?=} status
     * @return {?}
     */
    function (status) {
        _super.prototype.clear.call(this, status);
        this.destroy$.next();
    };
    /**
     * @return {?}
     */
    ConfirmationService.prototype.listenToEscape = /**
     * @return {?}
     */
    function () {
        var _this = this;
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), debounceTime(150), filter((/**
         * @param {?} key
         * @return {?}
         */
        function (key) { return key && key.key === 'Escape'; })))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        function (_) {
            _this.clear();
        }));
    };
    ConfirmationService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    ConfirmationService.ctorParameters = function () { return [
        { type: MessageService }
    ]; };
    /** @nocollapse */ ConfirmationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ConfirmationService_Factory() { return new ConfirmationService(i0.ɵɵinject(i1.MessageService)); }, token: ConfirmationService, providedIn: "root" });
    return ConfirmationService;
}(AbstractToaster));
export { ConfirmationService };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUV2RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sMENBQTBDLENBQUM7QUFDMUUsT0FBTyxFQUFFLFNBQVMsRUFBYyxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7OztBQUdqRTtJQUN5QywrQ0FBcUM7SUFPNUUsNkJBQXNCLGNBQThCO1FBQXBELFlBQ0Usa0JBQU0sY0FBYyxDQUFDLFNBQ3RCO1FBRnFCLG9CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQU5wRCxTQUFHLEdBQUcsaUJBQWlCLENBQUM7UUFFeEIsWUFBTSxHQUFHLElBQUksQ0FBQztRQUVkLGNBQVEsR0FBRyxJQUFJLE9BQU8sRUFBRSxDQUFDOztJQUl6QixDQUFDOzs7Ozs7OztJQUVELGtDQUFJOzs7Ozs7O0lBQUosVUFDRSxPQUFlLEVBQ2YsS0FBYSxFQUNiLFFBQTBCLEVBQzFCLE9BQThCO1FBRTlCLElBQUksQ0FBQyxjQUFjLEVBQUUsQ0FBQztRQUN0QixPQUFPLGlCQUFNLElBQUksWUFBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxPQUFPLENBQUMsQ0FBQztJQUN2RCxDQUFDOzs7OztJQUVELG1DQUFLOzs7O0lBQUwsVUFBTSxNQUF1QjtRQUMzQixpQkFBTSxLQUFLLFlBQUMsTUFBTSxDQUFDLENBQUM7UUFFcEIsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUUsQ0FBQztJQUN2QixDQUFDOzs7O0lBRUQsNENBQWM7OztJQUFkO1FBQUEsaUJBVUM7UUFUQyxTQUFTLENBQUMsUUFBUSxFQUFFLE9BQU8sQ0FBQzthQUN6QixJQUFJLENBQ0gsU0FBUyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsRUFDeEIsWUFBWSxDQUFDLEdBQUcsQ0FBQyxFQUNqQixNQUFNOzs7O1FBQUMsVUFBQyxHQUFrQixJQUFLLE9BQUEsR0FBRyxJQUFJLEdBQUcsQ0FBQyxHQUFHLEtBQUssUUFBUSxFQUEzQixDQUEyQixFQUFDLENBQzVEO2FBQ0EsU0FBUzs7OztRQUFDLFVBQUEsQ0FBQztZQUNWLEtBQUksQ0FBQyxLQUFLLEVBQUUsQ0FBQztRQUNmLENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7Z0JBdENGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7Z0JBTHpCLGNBQWM7Ozs4QkFIdkI7Q0ErQ0MsQUF2Q0QsQ0FDeUMsZUFBZSxHQXNDdkQ7U0F0Q1ksbUJBQW1COzs7SUFDOUIsa0NBQXdCOztJQUV4QixxQ0FBYzs7SUFFZCx1Q0FBeUI7Ozs7O0lBRWIsNkNBQXdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWJzdHJhY3RUb2FzdGVyIH0gZnJvbSAnLi4vYWJzdHJhY3RzL3RvYXN0ZXInO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uIH0gZnJvbSAnLi4vbW9kZWxzL2NvbmZpcm1hdGlvbic7XG5pbXBvcnQgeyBNZXNzYWdlU2VydmljZSB9IGZyb20gJ3ByaW1lbmcvY29tcG9uZW50cy9jb21tb24vbWVzc2FnZXNlcnZpY2UnO1xuaW1wb3J0IHsgZnJvbUV2ZW50LCBPYnNlcnZhYmxlLCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB0YWtlVW50aWwsIGRlYm91bmNlVGltZSwgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uL21vZGVscy90b2FzdGVyJztcblxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcbmV4cG9ydCBjbGFzcyBDb25maXJtYXRpb25TZXJ2aWNlIGV4dGVuZHMgQWJzdHJhY3RUb2FzdGVyPENvbmZpcm1hdGlvbi5PcHRpb25zPiB7XG4gIGtleSA9ICdhYnBDb25maXJtYXRpb24nO1xuXG4gIHN0aWNreSA9IHRydWU7XG5cbiAgZGVzdHJveSQgPSBuZXcgU3ViamVjdCgpO1xuXG4gIGNvbnN0cnVjdG9yKHByb3RlY3RlZCBtZXNzYWdlU2VydmljZTogTWVzc2FnZVNlcnZpY2UpIHtcbiAgICBzdXBlcihtZXNzYWdlU2VydmljZSk7XG4gIH1cblxuICBzaG93KFxuICAgIG1lc3NhZ2U6IHN0cmluZyxcbiAgICB0aXRsZTogc3RyaW5nLFxuICAgIHNldmVyaXR5OiBUb2FzdGVyLlNldmVyaXR5LFxuICAgIG9wdGlvbnM/OiBDb25maXJtYXRpb24uT3B0aW9ucyxcbiAgKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHRoaXMubGlzdGVuVG9Fc2NhcGUoKTtcbiAgICByZXR1cm4gc3VwZXIuc2hvdyhtZXNzYWdlLCB0aXRsZSwgc2V2ZXJpdHksIG9wdGlvbnMpO1xuICB9XG5cbiAgY2xlYXIoc3RhdHVzPzogVG9hc3Rlci5TdGF0dXMpIHtcbiAgICBzdXBlci5jbGVhcihzdGF0dXMpO1xuXG4gICAgdGhpcy5kZXN0cm95JC5uZXh0KCk7XG4gIH1cblxuICBsaXN0ZW5Ub0VzY2FwZSgpIHtcbiAgICBmcm9tRXZlbnQoZG9jdW1lbnQsICdrZXl1cCcpXG4gICAgICAucGlwZShcbiAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgICBkZWJvdW5jZVRpbWUoMTUwKSxcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkua2V5ID09PSAnRXNjYXBlJyksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKF8gPT4ge1xuICAgICAgICB0aGlzLmNsZWFyKCk7XG4gICAgICB9KTtcbiAgfVxufVxuIl19