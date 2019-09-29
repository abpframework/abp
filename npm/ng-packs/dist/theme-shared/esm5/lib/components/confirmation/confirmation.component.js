/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { ConfirmationService } from '../../services/confirmation.service';
var ConfirmationComponent = /** @class */ (function () {
    function ConfirmationComponent(confirmationService) {
        this.confirmationService = confirmationService;
        this.confirm = "confirm" /* confirm */;
        this.reject = "reject" /* reject */;
        this.dismiss = "dismiss" /* dismiss */;
    }
    /**
     * @param {?} status
     * @return {?}
     */
    ConfirmationComponent.prototype.close = /**
     * @param {?} status
     * @return {?}
     */
    function (status) {
        this.confirmationService.clear(status);
    };
    ConfirmationComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-confirmation',
                    template: "\n    <p-toast\n      position=\"center\"\n      key=\"abpConfirmation\"\n      (onClose)=\"close(dismiss)\"\n      [modal]=\"true\"\n      [baseZIndex]=\"1000\"\n      styleClass=\"abp-confirm\"\n    >\n      <ng-template let-message pTemplate=\"message\">\n        <i class=\"fa fa-exclamation-circle abp-confirm-icon\"></i>\n        <div *ngIf=\"message.summary\" class=\"abp-confirm-summary\">\n          {{ message.summary | abpLocalization: message.titleLocalizationParams }}\n        </div>\n        <div class=\"abp-confirm-body\">\n          {{ message.detail | abpLocalization: message.messageLocalizationParams }}\n        </div>\n\n        <div class=\"abp-confirm-footer justify-content-center\">\n          <button *ngIf=\"!message.hideCancelBtn\" type=\"button\" class=\"btn btn-sm btn-primary\" (click)=\"close(reject)\">\n            {{ message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}\n          </button>\n          <button\n            *ngIf=\"!message.hideYesBtn\"\n            type=\"button\"\n            class=\"btn btn-sm btn-primary\"\n            (click)=\"close(confirm)\"\n            autofocus\n          >\n            <span>{{ message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>\n          </button>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
                }] }
    ];
    /** @nocollapse */
    ConfirmationComponent.ctorParameters = function () { return [
        { type: ConfirmationService }
    ]; };
    return ConfirmationComponent;
}());
export { ConfirmationComponent };
if (false) {
    /** @type {?} */
    ConfirmationComponent.prototype.confirm;
    /** @type {?} */
    ConfirmationComponent.prototype.reject;
    /** @type {?} */
    ConfirmationComponent.prototype.dismiss;
    /**
     * @type {?}
     * @private
     */
    ConfirmationComponent.prototype.confirmationService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDMUMsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFHMUU7SUEyQ0UsK0JBQW9CLG1CQUF3QztRQUF4Qyx3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO1FBSjVELFlBQU8sMkJBQTBCO1FBQ2pDLFdBQU0seUJBQXlCO1FBQy9CLFlBQU8sMkJBQTBCO0lBRThCLENBQUM7Ozs7O0lBRWhFLHFDQUFLOzs7O0lBQUwsVUFBTSxNQUFzQjtRQUMxQixJQUFJLENBQUMsbUJBQW1CLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0lBQ3pDLENBQUM7O2dCQS9DRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjtvQkFDNUIsUUFBUSxFQUFFLHl5Q0FrQ1Q7aUJBQ0Y7Ozs7Z0JBeENRLG1CQUFtQjs7SUFtRDVCLDRCQUFDO0NBQUEsQUFoREQsSUFnREM7U0FWWSxxQkFBcUI7OztJQUNoQyx3Q0FBaUM7O0lBQ2pDLHVDQUErQjs7SUFDL0Isd0NBQWlDOzs7OztJQUVyQixvREFBZ0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZSc7XG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RvYXN0ZXInO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtY29uZmlybWF0aW9uJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8cC10b2FzdFxuICAgICAgcG9zaXRpb249XCJjZW50ZXJcIlxuICAgICAga2V5PVwiYWJwQ29uZmlybWF0aW9uXCJcbiAgICAgIChvbkNsb3NlKT1cImNsb3NlKGRpc21pc3MpXCJcbiAgICAgIFttb2RhbF09XCJ0cnVlXCJcbiAgICAgIFtiYXNlWkluZGV4XT1cIjEwMDBcIlxuICAgICAgc3R5bGVDbGFzcz1cImFicC1jb25maXJtXCJcbiAgICA+XG4gICAgICA8bmctdGVtcGxhdGUgbGV0LW1lc3NhZ2UgcFRlbXBsYXRlPVwibWVzc2FnZVwiPlxuICAgICAgICA8aSBjbGFzcz1cImZhIGZhLWV4Y2xhbWF0aW9uLWNpcmNsZSBhYnAtY29uZmlybS1pY29uXCI+PC9pPlxuICAgICAgICA8ZGl2ICpuZ0lmPVwibWVzc2FnZS5zdW1tYXJ5XCIgY2xhc3M9XCJhYnAtY29uZmlybS1zdW1tYXJ5XCI+XG4gICAgICAgICAge3sgbWVzc2FnZS5zdW1tYXJ5IHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLnRpdGxlTG9jYWxpemF0aW9uUGFyYW1zIH19XG4gICAgICAgIDwvZGl2PlxuICAgICAgICA8ZGl2IGNsYXNzPVwiYWJwLWNvbmZpcm0tYm9keVwiPlxuICAgICAgICAgIHt7IG1lc3NhZ2UuZGV0YWlsIHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLm1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXMgfX1cbiAgICAgICAgPC9kaXY+XG5cbiAgICAgICAgPGRpdiBjbGFzcz1cImFicC1jb25maXJtLWZvb3RlciBqdXN0aWZ5LWNvbnRlbnQtY2VudGVyXCI+XG4gICAgICAgICAgPGJ1dHRvbiAqbmdJZj1cIiFtZXNzYWdlLmhpZGVDYW5jZWxCdG5cIiB0eXBlPVwiYnV0dG9uXCIgY2xhc3M9XCJidG4gYnRuLXNtIGJ0bi1wcmltYXJ5XCIgKGNsaWNrKT1cImNsb3NlKHJlamVjdClcIj5cbiAgICAgICAgICAgIHt7IG1lc3NhZ2UuY2FuY2VsQ29weSB8fCAnQWJwSWRlbnRpdHk6OkNhbmNlbCcgfCBhYnBMb2NhbGl6YXRpb24gfX1cbiAgICAgICAgICA8L2J1dHRvbj5cbiAgICAgICAgICA8YnV0dG9uXG4gICAgICAgICAgICAqbmdJZj1cIiFtZXNzYWdlLmhpZGVZZXNCdG5cIlxuICAgICAgICAgICAgdHlwZT1cImJ1dHRvblwiXG4gICAgICAgICAgICBjbGFzcz1cImJ0biBidG4tc20gYnRuLXByaW1hcnlcIlxuICAgICAgICAgICAgKGNsaWNrKT1cImNsb3NlKGNvbmZpcm0pXCJcbiAgICAgICAgICAgIGF1dG9mb2N1c1xuICAgICAgICAgID5cbiAgICAgICAgICAgIDxzcGFuPnt7IG1lc3NhZ2UueWVzQ29weSB8fCAnQWJwSWRlbnRpdHk6OlllcycgfCBhYnBMb2NhbGl6YXRpb24gfX08L3NwYW4+XG4gICAgICAgICAgPC9idXR0b24+XG4gICAgICAgIDwvZGl2PlxuICAgICAgPC9uZy10ZW1wbGF0ZT5cbiAgICA8L3AtdG9hc3Q+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIENvbmZpcm1hdGlvbkNvbXBvbmVudCB7XG4gIGNvbmZpcm0gPSBUb2FzdGVyLlN0YXR1cy5jb25maXJtO1xuICByZWplY3QgPSBUb2FzdGVyLlN0YXR1cy5yZWplY3Q7XG4gIGRpc21pc3MgPSBUb2FzdGVyLlN0YXR1cy5kaXNtaXNzO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSkge31cblxuICBjbG9zZShzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlLmNsZWFyKHN0YXR1cyk7XG4gIH1cbn1cbiJdfQ==