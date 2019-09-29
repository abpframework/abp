/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
var ToasterService = /** @class */ (function (_super) {
    tslib_1.__extends(ToasterService, _super);
    function ToasterService() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @param {?} messages
     * @return {?}
     */
    ToasterService.prototype.addAll = /**
     * @param {?} messages
     * @return {?}
     */
    function (messages) {
        var _this = this;
        this.messageService.addAll(messages.map((/**
         * @param {?} message
         * @return {?}
         */
        function (message) { return (tslib_1.__assign({ key: _this.key }, message)); })));
    };
    ToasterService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */ ToasterService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ToasterService_Factory() { return new ToasterService(i0.ɵɵinject(i1.MessageService)); }, token: ToasterService, providedIn: "root" });
    return ToasterService;
}(AbstractToaster));
export { ToasterService };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdG9hc3Rlci5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7OztBQUd2RDtJQUNvQywwQ0FBZTtJQURuRDs7S0FLQzs7Ozs7SUFIQywrQkFBTTs7OztJQUFOLFVBQU8sUUFBbUI7UUFBMUIsaUJBRUM7UUFEQyxJQUFJLENBQUMsY0FBYyxDQUFDLE1BQU0sQ0FBQyxRQUFRLENBQUMsR0FBRzs7OztRQUFDLFVBQUEsT0FBTyxJQUFJLE9BQUEsb0JBQUcsR0FBRyxFQUFFLEtBQUksQ0FBQyxHQUFHLElBQUssT0FBTyxFQUFHLEVBQS9CLENBQStCLEVBQUMsQ0FBQyxDQUFDO0lBQ3ZGLENBQUM7O2dCQUpGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozt5QkFKbEM7Q0FTQyxBQUxELENBQ29DLGVBQWUsR0FJbEQ7U0FKWSxjQUFjIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWJzdHJhY3RUb2FzdGVyIH0gZnJvbSAnLi4vYWJzdHJhY3RzL3RvYXN0ZXInO1xuaW1wb3J0IHsgTWVzc2FnZSB9IGZyb20gJ3ByaW1lbmcvY29tcG9uZW50cy9jb21tb24vbWVzc2FnZSc7XG5cbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXG5leHBvcnQgY2xhc3MgVG9hc3RlclNlcnZpY2UgZXh0ZW5kcyBBYnN0cmFjdFRvYXN0ZXIge1xuICBhZGRBbGwobWVzc2FnZXM6IE1lc3NhZ2VbXSk6IHZvaWQge1xuICAgIHRoaXMubWVzc2FnZVNlcnZpY2UuYWRkQWxsKG1lc3NhZ2VzLm1hcChtZXNzYWdlID0+ICh7IGtleTogdGhpcy5rZXksIC4uLm1lc3NhZ2UgfSkpKTtcbiAgfVxufVxuIl19