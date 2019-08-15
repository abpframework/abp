/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
var ConfirmationService = /** @class */ (function (_super) {
    tslib_1.__extends(ConfirmationService, _super);
    function ConfirmationService() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.key = 'abpConfirmation';
        _this.sticky = true;
        return _this;
    }
    ConfirmationService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */ ConfirmationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ConfirmationService_Factory() { return new ConfirmationService(i0.ɵɵinject(i1.MessageService)); }, token: ConfirmationService, providedIn: "root" });
    return ConfirmationService;
}(AbstractToaster));
export { ConfirmationService };
if (false) {
    /** @type {?} */
    ConfirmationService.prototype.key;
    /** @type {?} */
    ConfirmationService.prototype.sticky;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLHNCQUFzQixDQUFDOzs7QUFHdkQ7SUFDeUMsK0NBQXFDO0lBRDlFO1FBQUEscUVBS0M7UUFIQyxTQUFHLEdBQVcsaUJBQWlCLENBQUM7UUFFaEMsWUFBTSxHQUFZLElBQUksQ0FBQzs7S0FDeEI7O2dCQUxBLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs4QkFKbEM7Q0FTQyxBQUxELENBQ3lDLGVBQWUsR0FJdkQ7U0FKWSxtQkFBbUI7OztJQUM5QixrQ0FBZ0M7O0lBRWhDLHFDQUF1QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFic3RyYWN0VG9hc3RlciB9IGZyb20gJy4uL2Fic3RyYWN0cy90b2FzdGVyJztcbmltcG9ydCB7IENvbmZpcm1hdGlvbiB9IGZyb20gJy4uL21vZGVscy9jb25maXJtYXRpb24nO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIENvbmZpcm1hdGlvblNlcnZpY2UgZXh0ZW5kcyBBYnN0cmFjdFRvYXN0ZXI8Q29uZmlybWF0aW9uLk9wdGlvbnM+IHtcbiAga2V5OiBzdHJpbmcgPSAnYWJwQ29uZmlybWF0aW9uJztcblxuICBzdGlja3k6IGJvb2xlYW4gPSB0cnVlO1xufVxuIl19