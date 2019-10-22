/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
export class ToastComponent {
}
ToastComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-toast',
                // tslint:disable-next-line: component-max-inline-declarations
                template: `
    <p-toast position="bottom-right" key="abpToast" styleClass="abp-toast" [baseZIndex]="1000">
      <ng-template let-message pTemplate="message">
        <span
          class="ui-toast-icon pi"
          [ngClass]="{
            'pi-info-circle': message.severity == 'info',
            'pi-exclamation-triangle': message.severity == 'warn',
            'pi-times': message.severity == 'error',
            'pi-check': message.severity == 'success'
          }"
        ></span>
        <div class="ui-toast-message-text-content">
          <div class="ui-toast-summary">{{ message.summary | abpLocalization: message.titleLocalizationParams }}</div>
          <div class="ui-toast-detail">{{ message.detail | abpLocalization: message.messageLocalizationParams }}</div>
        </div>
      </ng-template>
    </p-toast>
  `
            }] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3QuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUF5QjFDLE1BQU0sT0FBTyxjQUFjOzs7WUF2QjFCLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsV0FBVzs7Z0JBRXJCLFFBQVEsRUFBRTs7Ozs7Ozs7Ozs7Ozs7Ozs7O0dBa0JUO2FBQ0YiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXRvYXN0JyxcclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGNvbXBvbmVudC1tYXgtaW5saW5lLWRlY2xhcmF0aW9uc1xyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8cC10b2FzdCBwb3NpdGlvbj1cImJvdHRvbS1yaWdodFwiIGtleT1cImFicFRvYXN0XCIgc3R5bGVDbGFzcz1cImFicC10b2FzdFwiIFtiYXNlWkluZGV4XT1cIjEwMDBcIj5cclxuICAgICAgPG5nLXRlbXBsYXRlIGxldC1tZXNzYWdlIHBUZW1wbGF0ZT1cIm1lc3NhZ2VcIj5cclxuICAgICAgICA8c3BhblxyXG4gICAgICAgICAgY2xhc3M9XCJ1aS10b2FzdC1pY29uIHBpXCJcclxuICAgICAgICAgIFtuZ0NsYXNzXT1cIntcclxuICAgICAgICAgICAgJ3BpLWluZm8tY2lyY2xlJzogbWVzc2FnZS5zZXZlcml0eSA9PSAnaW5mbycsXHJcbiAgICAgICAgICAgICdwaS1leGNsYW1hdGlvbi10cmlhbmdsZSc6IG1lc3NhZ2Uuc2V2ZXJpdHkgPT0gJ3dhcm4nLFxyXG4gICAgICAgICAgICAncGktdGltZXMnOiBtZXNzYWdlLnNldmVyaXR5ID09ICdlcnJvcicsXHJcbiAgICAgICAgICAgICdwaS1jaGVjayc6IG1lc3NhZ2Uuc2V2ZXJpdHkgPT0gJ3N1Y2Nlc3MnXHJcbiAgICAgICAgICB9XCJcclxuICAgICAgICA+PC9zcGFuPlxyXG4gICAgICAgIDxkaXYgY2xhc3M9XCJ1aS10b2FzdC1tZXNzYWdlLXRleHQtY29udGVudFwiPlxyXG4gICAgICAgICAgPGRpdiBjbGFzcz1cInVpLXRvYXN0LXN1bW1hcnlcIj57eyBtZXNzYWdlLnN1bW1hcnkgfCBhYnBMb2NhbGl6YXRpb246IG1lc3NhZ2UudGl0bGVMb2NhbGl6YXRpb25QYXJhbXMgfX08L2Rpdj5cclxuICAgICAgICAgIDxkaXYgY2xhc3M9XCJ1aS10b2FzdC1kZXRhaWxcIj57eyBtZXNzYWdlLmRldGFpbCB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS5tZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zIH19PC9kaXY+XHJcbiAgICAgICAgPC9kaXY+XHJcbiAgICAgIDwvbmctdGVtcGxhdGU+XHJcbiAgICA8L3AtdG9hc3Q+XHJcbiAgYFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVG9hc3RDb21wb25lbnQge31cclxuIl19