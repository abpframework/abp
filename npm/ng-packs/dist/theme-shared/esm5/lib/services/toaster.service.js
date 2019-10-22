/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdG9hc3Rlci5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7OztBQUd2RDtJQUNvQywwQ0FBZTtJQURuRDs7S0FLQzs7Ozs7SUFIQywrQkFBTTs7OztJQUFOLFVBQU8sUUFBbUI7UUFBMUIsaUJBRUM7UUFEQyxJQUFJLENBQUMsY0FBYyxDQUFDLE1BQU0sQ0FBQyxRQUFRLENBQUMsR0FBRzs7OztRQUFDLFVBQUEsT0FBTyxJQUFJLE9BQUEsb0JBQUcsR0FBRyxFQUFFLEtBQUksQ0FBQyxHQUFHLElBQUssT0FBTyxFQUFHLEVBQS9CLENBQStCLEVBQUMsQ0FBQyxDQUFDO0lBQ3ZGLENBQUM7O2dCQUpGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozt5QkFKbEM7Q0FTQyxBQUxELENBQ29DLGVBQWUsR0FJbEQ7U0FKWSxjQUFjIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBBYnN0cmFjdFRvYXN0ZXIgfSBmcm9tICcuLi9hYnN0cmFjdHMvdG9hc3Rlcic7XHJcbmltcG9ydCB7IE1lc3NhZ2UgfSBmcm9tICdwcmltZW5nL2NvbXBvbmVudHMvY29tbW9uL21lc3NhZ2UnO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIFRvYXN0ZXJTZXJ2aWNlIGV4dGVuZHMgQWJzdHJhY3RUb2FzdGVyIHtcclxuICBhZGRBbGwobWVzc2FnZXM6IE1lc3NhZ2VbXSk6IHZvaWQge1xyXG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5hZGRBbGwobWVzc2FnZXMubWFwKG1lc3NhZ2UgPT4gKHsga2V5OiB0aGlzLmtleSwgLi4ubWVzc2FnZSB9KSkpO1xyXG4gIH1cclxufVxyXG4iXX0=