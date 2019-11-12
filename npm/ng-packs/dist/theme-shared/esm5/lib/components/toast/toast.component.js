/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/toast/toast.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
var ToastComponent = /** @class */ (function () {
    function ToastComponent() {
    }
    ToastComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-toast',
                    // tslint:disable-next-line: component-max-inline-declarations
                    template: "\n    <p-toast position=\"bottom-right\" key=\"abpToast\" styleClass=\"abp-toast\" [baseZIndex]=\"1000\">\n      <ng-template let-message pTemplate=\"message\">\n        <span\n          class=\"ui-toast-icon pi\"\n          [ngClass]=\"{\n            'pi-info-circle': message.severity === 'info',\n            'pi-exclamation-triangle': message.severity === 'warn',\n            'pi-times': message.severity === 'error',\n            'pi-check': message.severity === 'success'\n          }\"\n        ></span>\n        <div class=\"ui-toast-message-text-content\">\n          <div class=\"ui-toast-summary\">{{ message.summary | abpLocalization: message.titleLocalizationParams }}</div>\n          <div class=\"ui-toast-detail\">{{ message.detail | abpLocalization: message.messageLocalizationParams }}</div>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
                }] }
    ];
    return ToastComponent;
}());
export { ToastComponent };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3QuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRTFDO0lBQUE7SUF1QjZCLENBQUM7O2dCQXZCN0IsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxXQUFXOztvQkFFckIsUUFBUSxFQUFFLHMyQkFrQlQ7aUJBQ0Y7O0lBQzRCLHFCQUFDO0NBQUEsQUF2QjlCLElBdUI4QjtTQUFqQixjQUFjIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC10b2FzdCcsXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcclxuICB0ZW1wbGF0ZTogYFxyXG4gICAgPHAtdG9hc3QgcG9zaXRpb249XCJib3R0b20tcmlnaHRcIiBrZXk9XCJhYnBUb2FzdFwiIHN0eWxlQ2xhc3M9XCJhYnAtdG9hc3RcIiBbYmFzZVpJbmRleF09XCIxMDAwXCI+XHJcbiAgICAgIDxuZy10ZW1wbGF0ZSBsZXQtbWVzc2FnZSBwVGVtcGxhdGU9XCJtZXNzYWdlXCI+XHJcbiAgICAgICAgPHNwYW5cclxuICAgICAgICAgIGNsYXNzPVwidWktdG9hc3QtaWNvbiBwaVwiXHJcbiAgICAgICAgICBbbmdDbGFzc109XCJ7XHJcbiAgICAgICAgICAgICdwaS1pbmZvLWNpcmNsZSc6IG1lc3NhZ2Uuc2V2ZXJpdHkgPT09ICdpbmZvJyxcclxuICAgICAgICAgICAgJ3BpLWV4Y2xhbWF0aW9uLXRyaWFuZ2xlJzogbWVzc2FnZS5zZXZlcml0eSA9PT0gJ3dhcm4nLFxyXG4gICAgICAgICAgICAncGktdGltZXMnOiBtZXNzYWdlLnNldmVyaXR5ID09PSAnZXJyb3InLFxyXG4gICAgICAgICAgICAncGktY2hlY2snOiBtZXNzYWdlLnNldmVyaXR5ID09PSAnc3VjY2VzcydcclxuICAgICAgICAgIH1cIlxyXG4gICAgICAgID48L3NwYW4+XHJcbiAgICAgICAgPGRpdiBjbGFzcz1cInVpLXRvYXN0LW1lc3NhZ2UtdGV4dC1jb250ZW50XCI+XHJcbiAgICAgICAgICA8ZGl2IGNsYXNzPVwidWktdG9hc3Qtc3VtbWFyeVwiPnt7IG1lc3NhZ2Uuc3VtbWFyeSB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS50aXRsZUxvY2FsaXphdGlvblBhcmFtcyB9fTwvZGl2PlxyXG4gICAgICAgICAgPGRpdiBjbGFzcz1cInVpLXRvYXN0LWRldGFpbFwiPnt7IG1lc3NhZ2UuZGV0YWlsIHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLm1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXMgfX08L2Rpdj5cclxuICAgICAgICA8L2Rpdj5cclxuICAgICAgPC9uZy10ZW1wbGF0ZT5cclxuICAgIDwvcC10b2FzdD5cclxuICBgLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVG9hc3RDb21wb25lbnQge31cclxuIl19