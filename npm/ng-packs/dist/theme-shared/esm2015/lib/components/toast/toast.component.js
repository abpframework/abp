/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
export class ToastComponent {
}
ToastComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-toast',
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3QuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUF3QjFDLE1BQU0sT0FBTyxjQUFjOzs7WUF0QjFCLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsV0FBVztnQkFDckIsUUFBUSxFQUFFOzs7Ozs7Ozs7Ozs7Ozs7Ozs7R0FrQlQ7YUFDRiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdG9hc3QnLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxwLXRvYXN0IHBvc2l0aW9uPVwiYm90dG9tLXJpZ2h0XCIga2V5PVwiYWJwVG9hc3RcIiBzdHlsZUNsYXNzPVwiYWJwLXRvYXN0XCIgW2Jhc2VaSW5kZXhdPVwiMTAwMFwiPlxuICAgICAgPG5nLXRlbXBsYXRlIGxldC1tZXNzYWdlIHBUZW1wbGF0ZT1cIm1lc3NhZ2VcIj5cbiAgICAgICAgPHNwYW5cbiAgICAgICAgICBjbGFzcz1cInVpLXRvYXN0LWljb24gcGlcIlxuICAgICAgICAgIFtuZ0NsYXNzXT1cIntcbiAgICAgICAgICAgICdwaS1pbmZvLWNpcmNsZSc6IG1lc3NhZ2Uuc2V2ZXJpdHkgPT0gJ2luZm8nLFxuICAgICAgICAgICAgJ3BpLWV4Y2xhbWF0aW9uLXRyaWFuZ2xlJzogbWVzc2FnZS5zZXZlcml0eSA9PSAnd2FybicsXG4gICAgICAgICAgICAncGktdGltZXMnOiBtZXNzYWdlLnNldmVyaXR5ID09ICdlcnJvcicsXG4gICAgICAgICAgICAncGktY2hlY2snOiBtZXNzYWdlLnNldmVyaXR5ID09ICdzdWNjZXNzJ1xuICAgICAgICAgIH1cIlxuICAgICAgICA+PC9zcGFuPlxuICAgICAgICA8ZGl2IGNsYXNzPVwidWktdG9hc3QtbWVzc2FnZS10ZXh0LWNvbnRlbnRcIj5cbiAgICAgICAgICA8ZGl2IGNsYXNzPVwidWktdG9hc3Qtc3VtbWFyeVwiPnt7IG1lc3NhZ2Uuc3VtbWFyeSB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS50aXRsZUxvY2FsaXphdGlvblBhcmFtcyB9fTwvZGl2PlxuICAgICAgICAgIDxkaXYgY2xhc3M9XCJ1aS10b2FzdC1kZXRhaWxcIj57eyBtZXNzYWdlLmRldGFpbCB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS5tZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zIH19PC9kaXY+XG4gICAgICAgIDwvZGl2PlxuICAgICAgPC9uZy10ZW1wbGF0ZT5cbiAgICA8L3AtdG9hc3Q+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIFRvYXN0Q29tcG9uZW50IHt9XG4iXX0=