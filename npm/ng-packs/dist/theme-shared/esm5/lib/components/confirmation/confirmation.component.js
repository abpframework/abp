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
                    template: "\n    <p-toast\n      position=\"center\"\n      key=\"abpConfirmation\"\n      (onClose)=\"close(dismiss)\"\n      [modal]=\"true\"\n      [baseZIndex]=\"1000\"\n      styleClass=\"\"\n    >\n      <ng-template let-message pTemplate=\"message\">\n        <div *ngIf=\"message.summary\" class=\"modal-header\">\n          <h4 class=\"modal-title\">\n            {{ message.summary | abpLocalization: message.titleLocalizationParams }}\n          </h4>\n        </div>\n        <div class=\"modal-body\">\n          {{ message.detail | abpLocalization: message.messageLocalizationParams }}\n        </div>\n\n        <div class=\"modal-footer justify-content-center\">\n          <button *ngIf=\"!message.hideCancelBtn\" type=\"button\" class=\"btn btn-secondary\" (click)=\"close(reject)\">\n            {{ message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}\n          </button>\n          <button *ngIf=\"!message.hideYesBtn\" type=\"button\" class=\"btn btn-secondary\" (click)=\"close(confirm)\">\n            <span>{{ message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>\n          </button>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDMUMsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFHMUU7SUFzQ0UsK0JBQW9CLG1CQUF3QztRQUF4Qyx3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO1FBSjVELFlBQU8sMkJBQTBCO1FBQ2pDLFdBQU0seUJBQXlCO1FBQy9CLFlBQU8sMkJBQTBCO0lBRThCLENBQUM7Ozs7O0lBRWhFLHFDQUFLOzs7O0lBQUwsVUFBTSxNQUFzQjtRQUMxQixJQUFJLENBQUMsbUJBQW1CLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0lBQ3pDLENBQUM7O2dCQTFDRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjtvQkFDNUIsUUFBUSxFQUFFLDhwQ0E2QlQ7aUJBQ0Y7Ozs7Z0JBbkNRLG1CQUFtQjs7SUE4QzVCLDRCQUFDO0NBQUEsQUEzQ0QsSUEyQ0M7U0FWWSxxQkFBcUI7OztJQUNoQyx3Q0FBaUM7O0lBQ2pDLHVDQUErQjs7SUFDL0Isd0NBQWlDOzs7OztJQUVyQixvREFBZ0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZSc7XG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RvYXN0ZXInO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtY29uZmlybWF0aW9uJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8cC10b2FzdFxuICAgICAgcG9zaXRpb249XCJjZW50ZXJcIlxuICAgICAga2V5PVwiYWJwQ29uZmlybWF0aW9uXCJcbiAgICAgIChvbkNsb3NlKT1cImNsb3NlKGRpc21pc3MpXCJcbiAgICAgIFttb2RhbF09XCJ0cnVlXCJcbiAgICAgIFtiYXNlWkluZGV4XT1cIjEwMDBcIlxuICAgICAgc3R5bGVDbGFzcz1cIlwiXG4gICAgPlxuICAgICAgPG5nLXRlbXBsYXRlIGxldC1tZXNzYWdlIHBUZW1wbGF0ZT1cIm1lc3NhZ2VcIj5cbiAgICAgICAgPGRpdiAqbmdJZj1cIm1lc3NhZ2Uuc3VtbWFyeVwiIGNsYXNzPVwibW9kYWwtaGVhZGVyXCI+XG4gICAgICAgICAgPGg0IGNsYXNzPVwibW9kYWwtdGl0bGVcIj5cbiAgICAgICAgICAgIHt7IG1lc3NhZ2Uuc3VtbWFyeSB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS50aXRsZUxvY2FsaXphdGlvblBhcmFtcyB9fVxuICAgICAgICAgIDwvaDQ+XG4gICAgICAgIDwvZGl2PlxuICAgICAgICA8ZGl2IGNsYXNzPVwibW9kYWwtYm9keVwiPlxuICAgICAgICAgIHt7IG1lc3NhZ2UuZGV0YWlsIHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLm1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXMgfX1cbiAgICAgICAgPC9kaXY+XG5cbiAgICAgICAgPGRpdiBjbGFzcz1cIm1vZGFsLWZvb3RlciBqdXN0aWZ5LWNvbnRlbnQtY2VudGVyXCI+XG4gICAgICAgICAgPGJ1dHRvbiAqbmdJZj1cIiFtZXNzYWdlLmhpZGVDYW5jZWxCdG5cIiB0eXBlPVwiYnV0dG9uXCIgY2xhc3M9XCJidG4gYnRuLXNlY29uZGFyeVwiIChjbGljayk9XCJjbG9zZShyZWplY3QpXCI+XG4gICAgICAgICAgICB7eyBtZXNzYWdlLmNhbmNlbENvcHkgfHwgJ0FicElkZW50aXR5OjpDYW5jZWwnIHwgYWJwTG9jYWxpemF0aW9uIH19XG4gICAgICAgICAgPC9idXR0b24+XG4gICAgICAgICAgPGJ1dHRvbiAqbmdJZj1cIiFtZXNzYWdlLmhpZGVZZXNCdG5cIiB0eXBlPVwiYnV0dG9uXCIgY2xhc3M9XCJidG4gYnRuLXNlY29uZGFyeVwiIChjbGljayk9XCJjbG9zZShjb25maXJtKVwiPlxuICAgICAgICAgICAgPHNwYW4+e3sgbWVzc2FnZS55ZXNDb3B5IHx8ICdBYnBJZGVudGl0eTo6WWVzJyB8IGFicExvY2FsaXphdGlvbiB9fTwvc3Bhbj5cbiAgICAgICAgICA8L2J1dHRvbj5cbiAgICAgICAgPC9kaXY+XG4gICAgICA8L25nLXRlbXBsYXRlPlxuICAgIDwvcC10b2FzdD5cbiAgYCxcbn0pXG5leHBvcnQgY2xhc3MgQ29uZmlybWF0aW9uQ29tcG9uZW50IHtcbiAgY29uZmlybSA9IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm07XG4gIHJlamVjdCA9IFRvYXN0ZXIuU3RhdHVzLnJlamVjdDtcbiAgZGlzbWlzcyA9IFRvYXN0ZXIuU3RhdHVzLmRpc21pc3M7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxuXG4gIGNsb3NlKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpIHtcbiAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2UuY2xlYXIoc3RhdHVzKTtcbiAgfVxufVxuIl19