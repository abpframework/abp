/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
export class ErrorComponent {
    constructor() {
        this.title = 'Oops!';
        this.details = 'Sorry, an error has occured.';
    }
    /**
     * @return {?}
     */
    destroy() {
        this.renderer.removeChild(this.host, this.elementRef.nativeElement);
    }
}
ErrorComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-error',
                template: `
    <div class="error">
      <button id="abp-close-button mr-2" type="button" class="close" (click)="destroy()">
        <span aria-hidden="true">&times;</span>
      </button>
      <div class="row centered">
        <div class="col-md-12">
          <div class="error-template">
            <h1>
              {{ title }}
            </h1>
            <div class="error-details">
              {{ details }}
            </div>
            <div class="error-actions">
              <a routerLink="/" class="btn btn-primary btn-md mt-2"
                ><span class="glyphicon glyphicon-home"></span> Take me home
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
                styles: [".error{position:fixed;top:0;background-color:#fff;width:100vw;height:100vh;z-index:999999}.centered{position:fixed;top:50%;left:50%;transform:translate(-50%,-50%)}"]
            }] }
];
if (false) {
    /** @type {?} */
    ErrorComponent.prototype.title;
    /** @type {?} */
    ErrorComponent.prototype.details;
    /** @type {?} */
    ErrorComponent.prototype.renderer;
    /** @type {?} */
    ErrorComponent.prototype.elementRef;
    /** @type {?} */
    ErrorComponent.prototype.host;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9lcnJvcnMvZXJyb3IuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUF5QixNQUFNLGVBQWUsQ0FBQztBQThCakUsTUFBTSxPQUFPLGNBQWM7SUE1QjNCO1FBNkJFLFVBQUssR0FBRyxPQUFPLENBQUM7UUFFaEIsWUFBTyxHQUFHLDhCQUE4QixDQUFDO0lBVzNDLENBQUM7Ozs7SUFIQyxPQUFPO1FBQ0wsSUFBSSxDQUFDLFFBQVEsQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLElBQUksRUFBRSxJQUFJLENBQUMsVUFBVSxDQUFDLGFBQWEsQ0FBQyxDQUFDO0lBQ3RFLENBQUM7OztZQXpDRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLFdBQVc7Z0JBQ3JCLFFBQVEsRUFBRTs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7R0F1QlQ7O2FBRUY7Ozs7SUFFQywrQkFBZ0I7O0lBRWhCLGlDQUF5Qzs7SUFFekMsa0NBQW9COztJQUVwQixvQ0FBdUI7O0lBRXZCLDhCQUFVIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBSZW5kZXJlcjIsIEVsZW1lbnRSZWYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWVycm9yJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8ZGl2IGNsYXNzPVwiZXJyb3JcIj5cbiAgICAgIDxidXR0b24gaWQ9XCJhYnAtY2xvc2UtYnV0dG9uIG1yLTJcIiB0eXBlPVwiYnV0dG9uXCIgY2xhc3M9XCJjbG9zZVwiIChjbGljayk9XCJkZXN0cm95KClcIj5cbiAgICAgICAgPHNwYW4gYXJpYS1oaWRkZW49XCJ0cnVlXCI+JnRpbWVzOzwvc3Bhbj5cbiAgICAgIDwvYnV0dG9uPlxuICAgICAgPGRpdiBjbGFzcz1cInJvdyBjZW50ZXJlZFwiPlxuICAgICAgICA8ZGl2IGNsYXNzPVwiY29sLW1kLTEyXCI+XG4gICAgICAgICAgPGRpdiBjbGFzcz1cImVycm9yLXRlbXBsYXRlXCI+XG4gICAgICAgICAgICA8aDE+XG4gICAgICAgICAgICAgIHt7IHRpdGxlIH19XG4gICAgICAgICAgICA8L2gxPlxuICAgICAgICAgICAgPGRpdiBjbGFzcz1cImVycm9yLWRldGFpbHNcIj5cbiAgICAgICAgICAgICAge3sgZGV0YWlscyB9fVxuICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICA8ZGl2IGNsYXNzPVwiZXJyb3ItYWN0aW9uc1wiPlxuICAgICAgICAgICAgICA8YSByb3V0ZXJMaW5rPVwiL1wiIGNsYXNzPVwiYnRuIGJ0bi1wcmltYXJ5IGJ0bi1tZCBtdC0yXCJcbiAgICAgICAgICAgICAgICA+PHNwYW4gY2xhc3M9XCJnbHlwaGljb24gZ2x5cGhpY29uLWhvbWVcIj48L3NwYW4+IFRha2UgbWUgaG9tZVxuICAgICAgICAgICAgICA8L2E+XG4gICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgPC9kaXY+XG4gICAgICA8L2Rpdj5cbiAgICA8L2Rpdj5cbiAgYCxcbiAgc3R5bGVVcmxzOiBbJ2Vycm9yLmNvbXBvbmVudC5zY3NzJ10sXG59KVxuZXhwb3J0IGNsYXNzIEVycm9yQ29tcG9uZW50IHtcbiAgdGl0bGUgPSAnT29wcyEnO1xuXG4gIGRldGFpbHMgPSAnU29ycnksIGFuIGVycm9yIGhhcyBvY2N1cmVkLic7XG5cbiAgcmVuZGVyZXI6IFJlbmRlcmVyMjtcblxuICBlbGVtZW50UmVmOiBFbGVtZW50UmVmO1xuXG4gIGhvc3Q6IGFueTtcblxuICBkZXN0cm95KCkge1xuICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlQ2hpbGQodGhpcy5ob3N0LCB0aGlzLmVsZW1lbnRSZWYubmF0aXZlRWxlbWVudCk7XG4gIH1cbn1cbiJdfQ==