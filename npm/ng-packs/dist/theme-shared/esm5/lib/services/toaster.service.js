/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/toaster.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { MessageService } from 'primeng/components/common/messageservice';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
var ToasterService = /** @class */ (function (_super) {
    tslib_1.__extends(ToasterService, _super);
    function ToasterService(messageService) {
        var _this = _super.call(this, messageService) || this;
        _this.messageService = messageService;
        return _this;
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
    /** @nocollapse */
    ToasterService.ctorParameters = function () { return [
        { type: MessageService }
    ]; };
    /** @nocollapse */ ToasterService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ToasterService_Factory() { return new ToasterService(i0.ɵɵinject(i1.MessageService)); }, token: ToasterService, providedIn: "root" });
    return ToasterService;
}(AbstractToaster));
export { ToasterService };
if (false) {
    /**
     * @type {?}
     * @protected
     */
    ToasterService.prototype.messageService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdG9hc3Rlci5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBRXZELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQzs7O0FBRTFFO0lBQ29DLDBDQUFlO0lBQ2pELHdCQUFzQixjQUE4QjtRQUFwRCxZQUNFLGtCQUFNLGNBQWMsQ0FBQyxTQUN0QjtRQUZxQixvQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7O0lBRXBELENBQUM7Ozs7O0lBRUQsK0JBQU07Ozs7SUFBTixVQUFPLFFBQW1CO1FBQTFCLGlCQUVDO1FBREMsSUFBSSxDQUFDLGNBQWMsQ0FBQyxNQUFNLENBQUMsUUFBUSxDQUFDLEdBQUc7Ozs7UUFBQyxVQUFBLE9BQU8sSUFBSSxPQUFBLG9CQUFHLEdBQUcsRUFBRSxLQUFJLENBQUMsR0FBRyxJQUFLLE9BQU8sRUFBRyxFQUEvQixDQUErQixFQUFDLENBQUMsQ0FBQztJQUN2RixDQUFDOztnQkFSRixVQUFVLFNBQUMsRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFOzs7O2dCQUZ6QixjQUFjOzs7eUJBSHZCO0NBY0MsQUFURCxDQUNvQyxlQUFlLEdBUWxEO1NBUlksY0FBYzs7Ozs7O0lBQ2Isd0NBQXdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBBYnN0cmFjdFRvYXN0ZXIgfSBmcm9tICcuLi9hYnN0cmFjdHMvdG9hc3Rlcic7XHJcbmltcG9ydCB7IE1lc3NhZ2UgfSBmcm9tICdwcmltZW5nL2NvbXBvbmVudHMvY29tbW9uL21lc3NhZ2UnO1xyXG5pbXBvcnQgeyBNZXNzYWdlU2VydmljZSB9IGZyb20gJ3ByaW1lbmcvY29tcG9uZW50cy9jb21tb24vbWVzc2FnZXNlcnZpY2UnO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIFRvYXN0ZXJTZXJ2aWNlIGV4dGVuZHMgQWJzdHJhY3RUb2FzdGVyIHtcclxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgbWVzc2FnZVNlcnZpY2U6IE1lc3NhZ2VTZXJ2aWNlKSB7XHJcbiAgICBzdXBlcihtZXNzYWdlU2VydmljZSk7XHJcbiAgfVxyXG5cclxuICBhZGRBbGwobWVzc2FnZXM6IE1lc3NhZ2VbXSk6IHZvaWQge1xyXG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5hZGRBbGwobWVzc2FnZXMubWFwKG1lc3NhZ2UgPT4gKHsga2V5OiB0aGlzLmtleSwgLi4ubWVzc2FnZSB9KSkpO1xyXG4gIH1cclxufVxyXG4iXX0=