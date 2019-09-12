/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { ToastModule } from 'primeng/toast';
import { AccountLayoutComponent } from './components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from './components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from './components/empty-layout/empty-layout.component';
import { LayoutComponent } from './components/layout/layout.component';
import { LayoutState } from './states/layout.state';
/** @type {?} */
export var LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];
var ThemeBasicModule = /** @class */ (function () {
    function ThemeBasicModule() {
    }
    ThemeBasicModule.decorators = [
        { type: NgModule, args: [{
                    declarations: tslib_1.__spread(LAYOUTS, [LayoutComponent]),
                    imports: [
                        CoreModule,
                        ThemeSharedModule,
                        NgbCollapseModule,
                        NgbDropdownModule,
                        ToastModule,
                        NgxValidateCoreModule,
                        NgxsModule.forFeature([LayoutState]),
                    ],
                    exports: tslib_1.__spread(LAYOUTS),
                    entryComponents: tslib_1.__spread(LAYOUTS),
                },] }
    ];
    return ThemeBasicModule;
}());
export { ThemeBasicModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtYmFzaWMubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi90aGVtZS1iYXNpYy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLGlCQUFpQixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDbEYsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDM0QsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHNEQUFzRCxDQUFDO0FBQzlGLE9BQU8sRUFBRSwwQkFBMEIsRUFBRSxNQUFNLDhEQUE4RCxDQUFDO0FBQzFHLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGtEQUFrRCxDQUFDO0FBQ3hGLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQztBQUN2RSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sdUJBQXVCLENBQUM7O0FBRXBELE1BQU0sS0FBTyxPQUFPLEdBQUcsQ0FBQywwQkFBMEIsRUFBRSxzQkFBc0IsRUFBRSxvQkFBb0IsQ0FBQztBQUVqRztJQUFBO0lBYytCLENBQUM7O2dCQWQvQixRQUFRLFNBQUM7b0JBQ1IsWUFBWSxtQkFBTSxPQUFPLEdBQUUsZUFBZSxFQUFDO29CQUMzQyxPQUFPLEVBQUU7d0JBQ1AsVUFBVTt3QkFDVixpQkFBaUI7d0JBQ2pCLGlCQUFpQjt3QkFDakIsaUJBQWlCO3dCQUNqQixXQUFXO3dCQUNYLHFCQUFxQjt3QkFDckIsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLFdBQVcsQ0FBQyxDQUFDO3FCQUNyQztvQkFDRCxPQUFPLG1CQUFNLE9BQU8sQ0FBQztvQkFDckIsZUFBZSxtQkFBTSxPQUFPLENBQUM7aUJBQzlCOztJQUM4Qix1QkFBQztDQUFBLEFBZGhDLElBY2dDO1NBQW5CLGdCQUFnQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiQ29sbGFwc2VNb2R1bGUsIE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IE5neHNNb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBUb2FzdE1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdG9hc3QnO1xuaW1wb3J0IHsgQWNjb3VudExheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9hY2NvdW50LWxheW91dC9hY2NvdW50LWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgQXBwbGljYXRpb25MYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYXBwbGljYXRpb24tbGF5b3V0L2FwcGxpY2F0aW9uLWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgRW1wdHlMYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvZW1wdHktbGF5b3V0L2VtcHR5LWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgTGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2xheW91dC9sYXlvdXQuY29tcG9uZW50JztcbmltcG9ydCB7IExheW91dFN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvbGF5b3V0LnN0YXRlJztcblxuZXhwb3J0IGNvbnN0IExBWU9VVFMgPSBbQXBwbGljYXRpb25MYXlvdXRDb21wb25lbnQsIEFjY291bnRMYXlvdXRDb21wb25lbnQsIEVtcHR5TGF5b3V0Q29tcG9uZW50XTtcblxuQE5nTW9kdWxlKHtcbiAgZGVjbGFyYXRpb25zOiBbLi4uTEFZT1VUUywgTGF5b3V0Q29tcG9uZW50XSxcbiAgaW1wb3J0czogW1xuICAgIENvcmVNb2R1bGUsXG4gICAgVGhlbWVTaGFyZWRNb2R1bGUsXG4gICAgTmdiQ29sbGFwc2VNb2R1bGUsXG4gICAgTmdiRHJvcGRvd25Nb2R1bGUsXG4gICAgVG9hc3RNb2R1bGUsXG4gICAgTmd4VmFsaWRhdGVDb3JlTW9kdWxlLFxuICAgIE5neHNNb2R1bGUuZm9yRmVhdHVyZShbTGF5b3V0U3RhdGVdKSxcbiAgXSxcbiAgZXhwb3J0czogWy4uLkxBWU9VVFNdLFxuICBlbnRyeUNvbXBvbmVudHM6IFsuLi5MQVlPVVRTXSxcbn0pXG5leHBvcnQgY2xhc3MgVGhlbWVCYXNpY01vZHVsZSB7fVxuIl19