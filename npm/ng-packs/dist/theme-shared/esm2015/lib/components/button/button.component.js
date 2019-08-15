/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { StartLoader, StopLoader } from '@abp/ng.core';
import { Component, Input } from '@angular/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { filter } from 'rxjs/operators';
export class ButtonComponent {
    /**
     * @param {?} actions
     */
    constructor(actions) {
        this.actions = actions;
        this.buttonClass = 'btn btn-primary';
        this.buttonType = 'button';
        this.loading = false;
        this.disabled = false;
    }
    /**
     * @return {?}
     */
    get icon() {
        return `${this.loading ? 'fa fa-spin fa-spinner' : this.iconClass || 'd-none'}`;
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        if (this.requestType || this.requestURLContainSearchValue) {
            this.actions
                .pipe(ofActionSuccessful(StartLoader, StopLoader), filter((/**
             * @param {?} event
             * @return {?}
             */
            (event) => {
                /** @type {?} */
                let condition = true;
                if (this.requestType) {
                    if (!Array.isArray(this.requestType))
                        this.requestType = [this.requestType];
                    condition =
                        condition &&
                            this.requestType.findIndex((/**
                             * @param {?} type
                             * @return {?}
                             */
                            type => type.toLowerCase() === event.payload.method.toLowerCase())) > -1;
                }
                if (condition && this.requestURLContainSearchValue) {
                    condition =
                        condition &&
                            event.payload.url.toLowerCase().indexOf(this.requestURLContainSearchValue.toLowerCase()) > -1;
                }
                return condition;
            })))
                .subscribe((/**
             * @return {?}
             */
            () => {
                setTimeout((/**
                 * @return {?}
                 */
                () => {
                    this.loading = !this.loading;
                }), 0);
            }));
        }
    }
}
ButtonComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-button',
                template: `
    <button [attr.type]="buttonType" [ngClass]="buttonClass" [disabled]="loading || disabled">
      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>
    </button>
  `
            }] }
];
/** @nocollapse */
ButtonComponent.ctorParameters = () => [
    { type: Actions }
];
ButtonComponent.propDecorators = {
    buttonClass: [{ type: Input }],
    buttonType: [{ type: Input }],
    iconClass: [{ type: Input }],
    loading: [{ type: Input }],
    disabled: [{ type: Input }],
    requestType: [{ type: Input }],
    requestURLContainSearchValue: [{ type: Input }]
};
if (false) {
    /** @type {?} */
    ButtonComponent.prototype.buttonClass;
    /** @type {?} */
    ButtonComponent.prototype.buttonType;
    /** @type {?} */
    ButtonComponent.prototype.iconClass;
    /** @type {?} */
    ButtonComponent.prototype.loading;
    /** @type {?} */
    ButtonComponent.prototype.disabled;
    /** @type {?} */
    ButtonComponent.prototype.requestType;
    /** @type {?} */
    ButtonComponent.prototype.requestURLContainSearchValue;
    /**
     * @type {?}
     * @private
     */
    ButtonComponent.prototype.actions;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ3pELE9BQU8sRUFBRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDMUQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBVXhDLE1BQU0sT0FBTyxlQUFlOzs7O0lBMEIxQixZQUFvQixPQUFnQjtRQUFoQixZQUFPLEdBQVAsT0FBTyxDQUFTO1FBeEJwQyxnQkFBVyxHQUFXLGlCQUFpQixDQUFDO1FBR3hDLGVBQVUsR0FBVyxRQUFRLENBQUM7UUFNOUIsWUFBTyxHQUFZLEtBQUssQ0FBQztRQUd6QixhQUFRLEdBQVksS0FBSyxDQUFDO0lBWWEsQ0FBQzs7OztJQUp4QyxJQUFJLElBQUk7UUFDTixPQUFPLEdBQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsdUJBQXVCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksUUFBUSxFQUFFLENBQUM7SUFDbEYsQ0FBQzs7OztJQUlELFFBQVE7UUFDTixJQUFJLElBQUksQ0FBQyxXQUFXLElBQUksSUFBSSxDQUFDLDRCQUE0QixFQUFFO1lBQ3pELElBQUksQ0FBQyxPQUFPO2lCQUNULElBQUksQ0FDSCxrQkFBa0IsQ0FBQyxXQUFXLEVBQUUsVUFBVSxDQUFDLEVBQzNDLE1BQU07Ozs7WUFBQyxDQUFDLEtBQStCLEVBQUUsRUFBRTs7b0JBQ3JDLFNBQVMsR0FBRyxJQUFJO2dCQUNwQixJQUFJLElBQUksQ0FBQyxXQUFXLEVBQUU7b0JBQ3BCLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLElBQUksQ0FBQyxXQUFXLENBQUM7d0JBQUUsSUFBSSxDQUFDLFdBQVcsR0FBRyxDQUFDLElBQUksQ0FBQyxXQUFXLENBQUMsQ0FBQztvQkFFNUUsU0FBUzt3QkFDUCxTQUFTOzRCQUNULElBQUksQ0FBQyxXQUFXLENBQUMsU0FBUzs7Ozs0QkFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUUsS0FBSyxLQUFLLENBQUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxXQUFXLEVBQUUsRUFBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO2lCQUN0RztnQkFFRCxJQUFJLFNBQVMsSUFBSSxJQUFJLENBQUMsNEJBQTRCLEVBQUU7b0JBQ2xELFNBQVM7d0JBQ1AsU0FBUzs0QkFDVCxLQUFLLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxXQUFXLEVBQUUsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLDRCQUE0QixDQUFDLFdBQVcsRUFBRSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7aUJBQ2pHO2dCQUVELE9BQU8sU0FBUyxDQUFDO1lBQ25CLENBQUMsRUFBQyxDQUNIO2lCQUNBLFNBQVM7OztZQUFDLEdBQUcsRUFBRTtnQkFDZCxVQUFVOzs7Z0JBQUMsR0FBRyxFQUFFO29CQUNkLElBQUksQ0FBQyxPQUFPLEdBQUcsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDO2dCQUMvQixDQUFDLEdBQUUsQ0FBQyxDQUFDLENBQUM7WUFDUixDQUFDLEVBQUMsQ0FBQztTQUNOO0lBQ0gsQ0FBQzs7O1lBbEVGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsWUFBWTtnQkFDdEIsUUFBUSxFQUFFOzs7O0dBSVQ7YUFDRjs7OztZQVZRLE9BQU87OzswQkFZYixLQUFLO3lCQUdMLEtBQUs7d0JBR0wsS0FBSztzQkFHTCxLQUFLO3VCQUdMLEtBQUs7MEJBR0wsS0FBSzsyQ0FHTCxLQUFLOzs7O0lBbEJOLHNDQUN3Qzs7SUFFeEMscUNBQzhCOztJQUU5QixvQ0FDa0I7O0lBRWxCLGtDQUN5Qjs7SUFFekIsbUNBQzBCOztJQUUxQixzQ0FDK0I7O0lBRS9CLHVEQUNxQzs7Ozs7SUFNekIsa0NBQXdCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBJbnB1dCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBY3Rpb25zLCBvZkFjdGlvblN1Y2Nlc3NmdWwgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBmaWx0ZXIgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1idXR0b24nLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxidXR0b24gW2F0dHIudHlwZV09XCJidXR0b25UeXBlXCIgW25nQ2xhc3NdPVwiYnV0dG9uQ2xhc3NcIiBbZGlzYWJsZWRdPVwibG9hZGluZyB8fCBkaXNhYmxlZFwiPlxuICAgICAgPGkgW25nQ2xhc3NdPVwiaWNvblwiIGNsYXNzPVwibXItMVwiPjwvaT48bmctY29udGVudD48L25nLWNvbnRlbnQ+XG4gICAgPC9idXR0b24+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIEJ1dHRvbkNvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIEBJbnB1dCgpXG4gIGJ1dHRvbkNsYXNzOiBzdHJpbmcgPSAnYnRuIGJ0bi1wcmltYXJ5JztcblxuICBASW5wdXQoKVxuICBidXR0b25UeXBlOiBzdHJpbmcgPSAnYnV0dG9uJztcblxuICBASW5wdXQoKVxuICBpY29uQ2xhc3M6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBsb2FkaW5nOiBib29sZWFuID0gZmFsc2U7XG5cbiAgQElucHV0KClcbiAgZGlzYWJsZWQ6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBASW5wdXQoKVxuICByZXF1ZXN0VHlwZTogc3RyaW5nIHwgc3RyaW5nW107XG5cbiAgQElucHV0KClcbiAgcmVxdWVzdFVSTENvbnRhaW5TZWFyY2hWYWx1ZTogc3RyaW5nO1xuXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGAke3RoaXMubG9hZGluZyA/ICdmYSBmYS1zcGluIGZhLXNwaW5uZXInIDogdGhpcy5pY29uQ2xhc3MgfHwgJ2Qtbm9uZSd9YDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYWN0aW9uczogQWN0aW9ucykge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICBpZiAodGhpcy5yZXF1ZXN0VHlwZSB8fCB0aGlzLnJlcXVlc3RVUkxDb250YWluU2VhcmNoVmFsdWUpIHtcbiAgICAgIHRoaXMuYWN0aW9uc1xuICAgICAgICAucGlwZShcbiAgICAgICAgICBvZkFjdGlvblN1Y2Nlc3NmdWwoU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIpLFxuICAgICAgICAgIGZpbHRlcigoZXZlbnQ6IFN0YXJ0TG9hZGVyIHwgU3RvcExvYWRlcikgPT4ge1xuICAgICAgICAgICAgbGV0IGNvbmRpdGlvbiA9IHRydWU7XG4gICAgICAgICAgICBpZiAodGhpcy5yZXF1ZXN0VHlwZSkge1xuICAgICAgICAgICAgICBpZiAoIUFycmF5LmlzQXJyYXkodGhpcy5yZXF1ZXN0VHlwZSkpIHRoaXMucmVxdWVzdFR5cGUgPSBbdGhpcy5yZXF1ZXN0VHlwZV07XG5cbiAgICAgICAgICAgICAgY29uZGl0aW9uID1cbiAgICAgICAgICAgICAgICBjb25kaXRpb24gJiZcbiAgICAgICAgICAgICAgICB0aGlzLnJlcXVlc3RUeXBlLmZpbmRJbmRleCh0eXBlID0+IHR5cGUudG9Mb3dlckNhc2UoKSA9PT0gZXZlbnQucGF5bG9hZC5tZXRob2QudG9Mb3dlckNhc2UoKSkgPiAtMTtcbiAgICAgICAgICAgIH1cblxuICAgICAgICAgICAgaWYgKGNvbmRpdGlvbiAmJiB0aGlzLnJlcXVlc3RVUkxDb250YWluU2VhcmNoVmFsdWUpIHtcbiAgICAgICAgICAgICAgY29uZGl0aW9uID1cbiAgICAgICAgICAgICAgICBjb25kaXRpb24gJiZcbiAgICAgICAgICAgICAgICBldmVudC5wYXlsb2FkLnVybC50b0xvd2VyQ2FzZSgpLmluZGV4T2YodGhpcy5yZXF1ZXN0VVJMQ29udGFpblNlYXJjaFZhbHVlLnRvTG93ZXJDYXNlKCkpID4gLTE7XG4gICAgICAgICAgICB9XG5cbiAgICAgICAgICAgIHJldHVybiBjb25kaXRpb247XG4gICAgICAgICAgfSksXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgICAgICB0aGlzLmxvYWRpbmcgPSAhdGhpcy5sb2FkaW5nO1xuICAgICAgICAgIH0sIDApO1xuICAgICAgICB9KTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==