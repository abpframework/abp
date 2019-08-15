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
              {{ title | abpLocalization }}
            </h1>
            <div class="error-details">
              {{ details | abpLocalization }}
            </div>
            <div class="error-actions">
              <a (click)="destroy()" routerLink="/" class="btn btn-primary btn-md mt-2"
                ><span class="glyphicon glyphicon-home"></span> {{ '::Menu:Home' | abpLocalization }}
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9lcnJvcnMvZXJyb3IuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUF5QixNQUFNLGVBQWUsQ0FBQztBQThCakUsTUFBTSxPQUFPLGNBQWM7SUE1QjNCO1FBNkJFLFVBQUssR0FBRyxPQUFPLENBQUM7UUFFaEIsWUFBTyxHQUFHLDhCQUE4QixDQUFDO0lBVzNDLENBQUM7Ozs7SUFIQyxPQUFPO1FBQ0wsSUFBSSxDQUFDLFFBQVEsQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLElBQUksRUFBRSxJQUFJLENBQUMsVUFBVSxDQUFDLGFBQWEsQ0FBQyxDQUFDO0lBQ3RFLENBQUM7OztZQXpDRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLFdBQVc7Z0JBQ3JCLFFBQVEsRUFBRTs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7R0F1QlQ7O2FBRUY7Ozs7SUFFQywrQkFBZ0I7O0lBRWhCLGlDQUF5Qzs7SUFFekMsa0NBQW9COztJQUVwQixvQ0FBdUI7O0lBRXZCLDhCQUFVIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBSZW5kZXJlcjIsIEVsZW1lbnRSZWYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWVycm9yJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8ZGl2IGNsYXNzPVwiZXJyb3JcIj5cbiAgICAgIDxidXR0b24gaWQ9XCJhYnAtY2xvc2UtYnV0dG9uIG1yLTJcIiB0eXBlPVwiYnV0dG9uXCIgY2xhc3M9XCJjbG9zZVwiIChjbGljayk9XCJkZXN0cm95KClcIj5cbiAgICAgICAgPHNwYW4gYXJpYS1oaWRkZW49XCJ0cnVlXCI+JnRpbWVzOzwvc3Bhbj5cbiAgICAgIDwvYnV0dG9uPlxuICAgICAgPGRpdiBjbGFzcz1cInJvdyBjZW50ZXJlZFwiPlxuICAgICAgICA8ZGl2IGNsYXNzPVwiY29sLW1kLTEyXCI+XG4gICAgICAgICAgPGRpdiBjbGFzcz1cImVycm9yLXRlbXBsYXRlXCI+XG4gICAgICAgICAgICA8aDE+XG4gICAgICAgICAgICAgIHt7IHRpdGxlIHwgYWJwTG9jYWxpemF0aW9uIH19XG4gICAgICAgICAgICA8L2gxPlxuICAgICAgICAgICAgPGRpdiBjbGFzcz1cImVycm9yLWRldGFpbHNcIj5cbiAgICAgICAgICAgICAge3sgZGV0YWlscyB8IGFicExvY2FsaXphdGlvbiB9fVxuICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICA8ZGl2IGNsYXNzPVwiZXJyb3ItYWN0aW9uc1wiPlxuICAgICAgICAgICAgICA8YSAoY2xpY2spPVwiZGVzdHJveSgpXCIgcm91dGVyTGluaz1cIi9cIiBjbGFzcz1cImJ0biBidG4tcHJpbWFyeSBidG4tbWQgbXQtMlwiXG4gICAgICAgICAgICAgICAgPjxzcGFuIGNsYXNzPVwiZ2x5cGhpY29uIGdseXBoaWNvbi1ob21lXCI+PC9zcGFuPiB7eyAnOjpNZW51OkhvbWUnIHwgYWJwTG9jYWxpemF0aW9uIH19XG4gICAgICAgICAgICAgIDwvYT5cbiAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgIDwvZGl2PlxuICAgICAgICA8L2Rpdj5cbiAgICAgIDwvZGl2PlxuICAgIDwvZGl2PlxuICBgLFxuICBzdHlsZVVybHM6IFsnZXJyb3IuY29tcG9uZW50LnNjc3MnXSxcbn0pXG5leHBvcnQgY2xhc3MgRXJyb3JDb21wb25lbnQge1xuICB0aXRsZSA9ICdPb3BzISc7XG5cbiAgZGV0YWlscyA9ICdTb3JyeSwgYW4gZXJyb3IgaGFzIG9jY3VyZWQuJztcblxuICByZW5kZXJlcjogUmVuZGVyZXIyO1xuXG4gIGVsZW1lbnRSZWY6IEVsZW1lbnRSZWY7XG5cbiAgaG9zdDogYW55O1xuXG4gIGRlc3Ryb3koKSB7XG4gICAgdGhpcy5yZW5kZXJlci5yZW1vdmVDaGlsZCh0aGlzLmhvc3QsIHRoaXMuZWxlbWVudFJlZi5uYXRpdmVFbGVtZW50KTtcbiAgfVxufVxuIl19