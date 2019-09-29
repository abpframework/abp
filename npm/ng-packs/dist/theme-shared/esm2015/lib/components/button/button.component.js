/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input } from '@angular/core';
export class ButtonComponent {
    constructor() {
        this.buttonClass = 'btn btn-primary';
        this.type = 'button';
        this.loading = false;
        this.disabled = false;
    }
    /**
     * @return {?}
     */
    get icon() {
        return `${this.loading ? 'fa fa-pulse fa-spinner' : this.iconClass || 'd-none'}`;
    }
}
ButtonComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-button',
                template: `
    <button [attr.type]="type" [ngClass]="buttonClass" [disabled]="loading || disabled">
      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>
    </button>
  `
            }] }
];
ButtonComponent.propDecorators = {
    buttonClass: [{ type: Input }],
    type: [{ type: Input }],
    iconClass: [{ type: Input }],
    loading: [{ type: Input }],
    disabled: [{ type: Input }]
};
if (false) {
    /** @type {?} */
    ButtonComponent.prototype.buttonClass;
    /** @type {?} */
    ButtonComponent.prototype.type;
    /** @type {?} */
    ButtonComponent.prototype.iconClass;
    /** @type {?} */
    ButtonComponent.prototype.loading;
    /** @type {?} */
    ButtonComponent.prototype.disabled;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBVWpELE1BQU0sT0FBTyxlQUFlO0lBUjVCO1FBVUUsZ0JBQVcsR0FBVyxpQkFBaUIsQ0FBQztRQUd4QyxTQUFJLEdBQVcsUUFBUSxDQUFDO1FBTXhCLFlBQU8sR0FBWSxLQUFLLENBQUM7UUFHekIsYUFBUSxHQUFZLEtBQUssQ0FBQztJQUs1QixDQUFDOzs7O0lBSEMsSUFBSSxJQUFJO1FBQ04sT0FBTyxHQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLHdCQUF3QixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxJQUFJLFFBQVEsRUFBRSxDQUFDO0lBQ25GLENBQUM7OztZQTFCRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLFlBQVk7Z0JBQ3RCLFFBQVEsRUFBRTs7OztHQUlUO2FBQ0Y7OzswQkFFRSxLQUFLO21CQUdMLEtBQUs7d0JBR0wsS0FBSztzQkFHTCxLQUFLO3VCQUdMLEtBQUs7Ozs7SUFaTixzQ0FDd0M7O0lBRXhDLCtCQUN3Qjs7SUFFeEIsb0NBQ2tCOztJQUVsQixrQ0FDeUI7O0lBRXpCLG1DQUMwQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWJ1dHRvbicsXG4gIHRlbXBsYXRlOiBgXG4gICAgPGJ1dHRvbiBbYXR0ci50eXBlXT1cInR5cGVcIiBbbmdDbGFzc109XCJidXR0b25DbGFzc1wiIFtkaXNhYmxlZF09XCJsb2FkaW5nIHx8IGRpc2FibGVkXCI+XG4gICAgICA8aSBbbmdDbGFzc109XCJpY29uXCIgY2xhc3M9XCJtci0xXCI+PC9pPjxuZy1jb250ZW50PjwvbmctY29udGVudD5cbiAgICA8L2J1dHRvbj5cbiAgYCxcbn0pXG5leHBvcnQgY2xhc3MgQnV0dG9uQ29tcG9uZW50IHtcbiAgQElucHV0KClcbiAgYnV0dG9uQ2xhc3M6IHN0cmluZyA9ICdidG4gYnRuLXByaW1hcnknO1xuXG4gIEBJbnB1dCgpXG4gIHR5cGU6IHN0cmluZyA9ICdidXR0b24nO1xuXG4gIEBJbnB1dCgpXG4gIGljb25DbGFzczogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIGxvYWRpbmc6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBASW5wdXQoKVxuICBkaXNhYmxlZDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGAke3RoaXMubG9hZGluZyA/ICdmYSBmYS1wdWxzZSBmYS1zcGlubmVyJyA6IHRoaXMuaWNvbkNsYXNzIHx8ICdkLW5vbmUnfWA7XG4gIH1cbn1cbiJdfQ==