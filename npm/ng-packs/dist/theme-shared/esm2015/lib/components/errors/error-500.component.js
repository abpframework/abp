/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
export class Error500Component {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    ngOnInit() { }
}
Error500Component.decorators = [
    { type: Component, args: [{
                selector: 'abp-error-500',
                template: `
    <div class="error">
      <div class="row centered">
        <div class="col-md-12">
          <div class="error-template">
            <h1>
              Oops!
            </h1>
            <div class="error-details">
              Sorry, an error has occured.
            </div>
            <div class="error-actions">
              <a routerLink="/" class="btn btn-primary btn-md mt-2"
                ><span class="glyphicon glyphicon-home"></span> Take Me Home
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
/** @nocollapse */
Error500Component.ctorParameters = () => [
    { type: Store }
];
if (false) {
    /**
     * @type {?}
     * @private
     */
    Error500Component.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3ItNTAwLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvZXJyb3JzL2Vycm9yLTUwMC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDbEQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQTJCcEMsTUFBTSxPQUFPLGlCQUFpQjs7OztJQUM1QixZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFcEMsUUFBUSxLQUFVLENBQUM7OztZQTVCcEIsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxlQUFlO2dCQUN6QixRQUFRLEVBQUU7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7O0dBb0JUOzthQUVGOzs7O1lBMUJRLEtBQUs7Ozs7Ozs7SUE0QkEsa0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtZXJyb3ItNTAwJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8ZGl2IGNsYXNzPVwiZXJyb3JcIj5cbiAgICAgIDxkaXYgY2xhc3M9XCJyb3cgY2VudGVyZWRcIj5cbiAgICAgICAgPGRpdiBjbGFzcz1cImNvbC1tZC0xMlwiPlxuICAgICAgICAgIDxkaXYgY2xhc3M9XCJlcnJvci10ZW1wbGF0ZVwiPlxuICAgICAgICAgICAgPGgxPlxuICAgICAgICAgICAgICBPb3BzIVxuICAgICAgICAgICAgPC9oMT5cbiAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJlcnJvci1kZXRhaWxzXCI+XG4gICAgICAgICAgICAgIFNvcnJ5LCBhbiBlcnJvciBoYXMgb2NjdXJlZC5cbiAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgPGRpdiBjbGFzcz1cImVycm9yLWFjdGlvbnNcIj5cbiAgICAgICAgICAgICAgPGEgcm91dGVyTGluaz1cIi9cIiBjbGFzcz1cImJ0biBidG4tcHJpbWFyeSBidG4tbWQgbXQtMlwiXG4gICAgICAgICAgICAgICAgPjxzcGFuIGNsYXNzPVwiZ2x5cGhpY29uIGdseXBoaWNvbi1ob21lXCI+PC9zcGFuPiBUYWtlIE1lIEhvbWVcbiAgICAgICAgICAgICAgPC9hPlxuICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgPC9kaXY+XG4gICAgICAgIDwvZGl2PlxuICAgICAgPC9kaXY+XG4gICAgPC9kaXY+XG4gIGAsXG4gIHN0eWxlVXJsczogWydlcnJvci01MDAuY29tcG9uZW50LnNjc3MnXSxcbn0pXG5leHBvcnQgY2xhc3MgRXJyb3I1MDBDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHt9XG59XG4iXX0=