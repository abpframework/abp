/**
 * @fileoverview added by tsickle
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
                    template: "\n    <p-toast position=\"bottom-right\" key=\"abpToast\" styleClass=\"abp-toast\" [baseZIndex]=\"1000\">\n      <ng-template let-message pTemplate=\"message\">\n        <span\n          class=\"ui-toast-icon pi\"\n          [ngClass]=\"{\n            'pi-info-circle': message.severity == 'info',\n            'pi-exclamation-triangle': message.severity == 'warn',\n            'pi-times': message.severity == 'error',\n            'pi-check': message.severity == 'success'\n          }\"\n        ></span>\n        <div class=\"ui-toast-message-text-content\">\n          <div class=\"ui-toast-summary\">{{ message.summary | abpLocalization: message.titleLocalizationParams }}</div>\n          <div class=\"ui-toast-detail\">{{ message.detail | abpLocalization: message.messageLocalizationParams }}</div>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
                }] }
    ];
    return ToastComponent;
}());
export { ToastComponent };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3QuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFMUM7SUFBQTtJQXVCNkIsQ0FBQzs7Z0JBdkI3QixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLFdBQVc7O29CQUVyQixRQUFRLEVBQUUsazJCQWtCVDtpQkFDRjs7SUFDNEIscUJBQUM7Q0FBQSxBQXZCOUIsSUF1QjhCO1NBQWpCLGNBQWMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRvYXN0JyxcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcbiAgdGVtcGxhdGU6IGBcbiAgICA8cC10b2FzdCBwb3NpdGlvbj1cImJvdHRvbS1yaWdodFwiIGtleT1cImFicFRvYXN0XCIgc3R5bGVDbGFzcz1cImFicC10b2FzdFwiIFtiYXNlWkluZGV4XT1cIjEwMDBcIj5cbiAgICAgIDxuZy10ZW1wbGF0ZSBsZXQtbWVzc2FnZSBwVGVtcGxhdGU9XCJtZXNzYWdlXCI+XG4gICAgICAgIDxzcGFuXG4gICAgICAgICAgY2xhc3M9XCJ1aS10b2FzdC1pY29uIHBpXCJcbiAgICAgICAgICBbbmdDbGFzc109XCJ7XG4gICAgICAgICAgICAncGktaW5mby1jaXJjbGUnOiBtZXNzYWdlLnNldmVyaXR5ID09ICdpbmZvJyxcbiAgICAgICAgICAgICdwaS1leGNsYW1hdGlvbi10cmlhbmdsZSc6IG1lc3NhZ2Uuc2V2ZXJpdHkgPT0gJ3dhcm4nLFxuICAgICAgICAgICAgJ3BpLXRpbWVzJzogbWVzc2FnZS5zZXZlcml0eSA9PSAnZXJyb3InLFxuICAgICAgICAgICAgJ3BpLWNoZWNrJzogbWVzc2FnZS5zZXZlcml0eSA9PSAnc3VjY2VzcydcbiAgICAgICAgICB9XCJcbiAgICAgICAgPjwvc3Bhbj5cbiAgICAgICAgPGRpdiBjbGFzcz1cInVpLXRvYXN0LW1lc3NhZ2UtdGV4dC1jb250ZW50XCI+XG4gICAgICAgICAgPGRpdiBjbGFzcz1cInVpLXRvYXN0LXN1bW1hcnlcIj57eyBtZXNzYWdlLnN1bW1hcnkgfCBhYnBMb2NhbGl6YXRpb246IG1lc3NhZ2UudGl0bGVMb2NhbGl6YXRpb25QYXJhbXMgfX08L2Rpdj5cbiAgICAgICAgICA8ZGl2IGNsYXNzPVwidWktdG9hc3QtZGV0YWlsXCI+e3sgbWVzc2FnZS5kZXRhaWwgfCBhYnBMb2NhbGl6YXRpb246IG1lc3NhZ2UubWVzc2FnZUxvY2FsaXphdGlvblBhcmFtcyB9fTwvZGl2PlxuICAgICAgICA8L2Rpdj5cbiAgICAgIDwvbmctdGVtcGxhdGU+XG4gICAgPC9wLXRvYXN0PlxuICBgXG59KVxuZXhwb3J0IGNsYXNzIFRvYXN0Q29tcG9uZW50IHt9XG4iXX0=