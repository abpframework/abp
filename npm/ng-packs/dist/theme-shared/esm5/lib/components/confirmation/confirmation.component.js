/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/confirmation/confirmation.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                    // tslint:disable-next-line: component-max-inline-declarations
                    template: "\n    <p-toast\n      position=\"center\"\n      key=\"abpConfirmation\"\n      (onClose)=\"close(dismiss)\"\n      [modal]=\"true\"\n      [baseZIndex]=\"1000\"\n      styleClass=\"abp-confirm\"\n    >\n      <ng-template let-message pTemplate=\"message\">\n        <i class=\"fa fa-exclamation-circle abp-confirm-icon\"></i>\n        <div *ngIf=\"message.summary\" class=\"abp-confirm-summary\">\n          {{ message.summary | abpLocalization: message.titleLocalizationParams }}\n        </div>\n        <div class=\"abp-confirm-body\">\n          {{ message.detail | abpLocalization: message.messageLocalizationParams }}\n        </div>\n\n        <div class=\"abp-confirm-footer justify-content-center\">\n          <button\n            *ngIf=\"!message.hideCancelBtn\"\n            id=\"cancel\"\n            type=\"button\"\n            class=\"btn btn-sm btn-primary\"\n            (click)=\"close(reject)\"\n          >\n            {{ message.cancelText || message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}\n          </button>\n          <button\n            *ngIf=\"!message.hideYesBtn\"\n            id=\"confirm\"\n            type=\"button\"\n            class=\"btn btn-sm btn-primary\"\n            (click)=\"close(confirm)\"\n            autofocus\n          >\n            <span>{{ message.yesText || message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>\n          </button>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzFDLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBRzFFO0lBbURFLCtCQUFvQixtQkFBd0M7UUFBeEMsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUo1RCxZQUFPLDJCQUEwQjtRQUNqQyxXQUFNLHlCQUF5QjtRQUMvQixZQUFPLDJCQUEwQjtJQUU4QixDQUFDOzs7OztJQUVoRSxxQ0FBSzs7OztJQUFMLFVBQU0sTUFBc0I7UUFDMUIsSUFBSSxDQUFDLG1CQUFtQixDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsQ0FBQztJQUN6QyxDQUFDOztnQkF2REYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxrQkFBa0I7O29CQUU1QixRQUFRLEVBQUUseThDQXlDVDtpQkFDRjs7OztnQkFoRFEsbUJBQW1COztJQTJENUIsNEJBQUM7Q0FBQSxBQXhERCxJQXdEQztTQVZZLHFCQUFxQjs7O0lBQ2hDLHdDQUFpQzs7SUFDakMsdUNBQStCOztJQUMvQix3Q0FBaUM7Ozs7O0lBRXJCLG9EQUFnRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvY29uZmlybWF0aW9uLnNlcnZpY2UnO1xyXG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RvYXN0ZXInO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtY29uZmlybWF0aW9uJyxcclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGNvbXBvbmVudC1tYXgtaW5saW5lLWRlY2xhcmF0aW9uc1xyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8cC10b2FzdFxyXG4gICAgICBwb3NpdGlvbj1cImNlbnRlclwiXHJcbiAgICAgIGtleT1cImFicENvbmZpcm1hdGlvblwiXHJcbiAgICAgIChvbkNsb3NlKT1cImNsb3NlKGRpc21pc3MpXCJcclxuICAgICAgW21vZGFsXT1cInRydWVcIlxyXG4gICAgICBbYmFzZVpJbmRleF09XCIxMDAwXCJcclxuICAgICAgc3R5bGVDbGFzcz1cImFicC1jb25maXJtXCJcclxuICAgID5cclxuICAgICAgPG5nLXRlbXBsYXRlIGxldC1tZXNzYWdlIHBUZW1wbGF0ZT1cIm1lc3NhZ2VcIj5cclxuICAgICAgICA8aSBjbGFzcz1cImZhIGZhLWV4Y2xhbWF0aW9uLWNpcmNsZSBhYnAtY29uZmlybS1pY29uXCI+PC9pPlxyXG4gICAgICAgIDxkaXYgKm5nSWY9XCJtZXNzYWdlLnN1bW1hcnlcIiBjbGFzcz1cImFicC1jb25maXJtLXN1bW1hcnlcIj5cclxuICAgICAgICAgIHt7IG1lc3NhZ2Uuc3VtbWFyeSB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS50aXRsZUxvY2FsaXphdGlvblBhcmFtcyB9fVxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgICAgIDxkaXYgY2xhc3M9XCJhYnAtY29uZmlybS1ib2R5XCI+XHJcbiAgICAgICAgICB7eyBtZXNzYWdlLmRldGFpbCB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS5tZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zIH19XHJcbiAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgIDxkaXYgY2xhc3M9XCJhYnAtY29uZmlybS1mb290ZXIganVzdGlmeS1jb250ZW50LWNlbnRlclwiPlxyXG4gICAgICAgICAgPGJ1dHRvblxyXG4gICAgICAgICAgICAqbmdJZj1cIiFtZXNzYWdlLmhpZGVDYW5jZWxCdG5cIlxyXG4gICAgICAgICAgICBpZD1cImNhbmNlbFwiXHJcbiAgICAgICAgICAgIHR5cGU9XCJidXR0b25cIlxyXG4gICAgICAgICAgICBjbGFzcz1cImJ0biBidG4tc20gYnRuLXByaW1hcnlcIlxyXG4gICAgICAgICAgICAoY2xpY2spPVwiY2xvc2UocmVqZWN0KVwiXHJcbiAgICAgICAgICA+XHJcbiAgICAgICAgICAgIHt7IG1lc3NhZ2UuY2FuY2VsVGV4dCB8fCBtZXNzYWdlLmNhbmNlbENvcHkgfHwgJ0FicElkZW50aXR5OjpDYW5jZWwnIHwgYWJwTG9jYWxpemF0aW9uIH19XHJcbiAgICAgICAgICA8L2J1dHRvbj5cclxuICAgICAgICAgIDxidXR0b25cclxuICAgICAgICAgICAgKm5nSWY9XCIhbWVzc2FnZS5oaWRlWWVzQnRuXCJcclxuICAgICAgICAgICAgaWQ9XCJjb25maXJtXCJcclxuICAgICAgICAgICAgdHlwZT1cImJ1dHRvblwiXHJcbiAgICAgICAgICAgIGNsYXNzPVwiYnRuIGJ0bi1zbSBidG4tcHJpbWFyeVwiXHJcbiAgICAgICAgICAgIChjbGljayk9XCJjbG9zZShjb25maXJtKVwiXHJcbiAgICAgICAgICAgIGF1dG9mb2N1c1xyXG4gICAgICAgICAgPlxyXG4gICAgICAgICAgICA8c3Bhbj57eyBtZXNzYWdlLnllc1RleHQgfHwgbWVzc2FnZS55ZXNDb3B5IHx8ICdBYnBJZGVudGl0eTo6WWVzJyB8IGFicExvY2FsaXphdGlvbiB9fTwvc3Bhbj5cclxuICAgICAgICAgIDwvYnV0dG9uPlxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgICA8L25nLXRlbXBsYXRlPlxyXG4gICAgPC9wLXRvYXN0PlxyXG4gIGAsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBDb25maXJtYXRpb25Db21wb25lbnQge1xyXG4gIGNvbmZpcm0gPSBUb2FzdGVyLlN0YXR1cy5jb25maXJtO1xyXG4gIHJlamVjdCA9IFRvYXN0ZXIuU3RhdHVzLnJlamVjdDtcclxuICBkaXNtaXNzID0gVG9hc3Rlci5TdGF0dXMuZGlzbWlzcztcclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxyXG5cclxuICBjbG9zZShzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSB7XHJcbiAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2UuY2xlYXIoc3RhdHVzKTtcclxuICB9XHJcbn1cclxuIl19