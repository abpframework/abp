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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3QuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRTFDO0lBQUE7SUF1QjZCLENBQUM7O2dCQXZCN0IsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxXQUFXOztvQkFFckIsUUFBUSxFQUFFLHMyQkFrQlQ7aUJBQ0Y7O0lBQzRCLHFCQUFDO0NBQUEsQUF2QjlCLElBdUI4QjtTQUFqQixjQUFjIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC10b2FzdCcsXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogY29tcG9uZW50LW1heC1pbmxpbmUtZGVjbGFyYXRpb25zXG4gIHRlbXBsYXRlOiBgXG4gICAgPHAtdG9hc3QgcG9zaXRpb249XCJib3R0b20tcmlnaHRcIiBrZXk9XCJhYnBUb2FzdFwiIHN0eWxlQ2xhc3M9XCJhYnAtdG9hc3RcIiBbYmFzZVpJbmRleF09XCIxMDAwXCI+XG4gICAgICA8bmctdGVtcGxhdGUgbGV0LW1lc3NhZ2UgcFRlbXBsYXRlPVwibWVzc2FnZVwiPlxuICAgICAgICA8c3BhblxuICAgICAgICAgIGNsYXNzPVwidWktdG9hc3QtaWNvbiBwaVwiXG4gICAgICAgICAgW25nQ2xhc3NdPVwie1xuICAgICAgICAgICAgJ3BpLWluZm8tY2lyY2xlJzogbWVzc2FnZS5zZXZlcml0eSA9PT0gJ2luZm8nLFxuICAgICAgICAgICAgJ3BpLWV4Y2xhbWF0aW9uLXRyaWFuZ2xlJzogbWVzc2FnZS5zZXZlcml0eSA9PT0gJ3dhcm4nLFxuICAgICAgICAgICAgJ3BpLXRpbWVzJzogbWVzc2FnZS5zZXZlcml0eSA9PT0gJ2Vycm9yJyxcbiAgICAgICAgICAgICdwaS1jaGVjayc6IG1lc3NhZ2Uuc2V2ZXJpdHkgPT09ICdzdWNjZXNzJ1xuICAgICAgICAgIH1cIlxuICAgICAgICA+PC9zcGFuPlxuICAgICAgICA8ZGl2IGNsYXNzPVwidWktdG9hc3QtbWVzc2FnZS10ZXh0LWNvbnRlbnRcIj5cbiAgICAgICAgICA8ZGl2IGNsYXNzPVwidWktdG9hc3Qtc3VtbWFyeVwiPnt7IG1lc3NhZ2Uuc3VtbWFyeSB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS50aXRsZUxvY2FsaXphdGlvblBhcmFtcyB9fTwvZGl2PlxuICAgICAgICAgIDxkaXYgY2xhc3M9XCJ1aS10b2FzdC1kZXRhaWxcIj57eyBtZXNzYWdlLmRldGFpbCB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS5tZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zIH19PC9kaXY+XG4gICAgICAgIDwvZGl2PlxuICAgICAgPC9uZy10ZW1wbGF0ZT5cbiAgICA8L3AtdG9hc3Q+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIFRvYXN0Q29tcG9uZW50IHt9XG4iXX0=