/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { CoreModule } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { LayoutAccountComponent } from './components/layout-account/layout-account.component';
import { LayoutApplicationComponent } from './components/layout-application/layout-application.component';
import { LayoutEmptyComponent } from './components/layout-empty/layout-empty.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ToastModule } from 'primeng/toast';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { LayoutState } from './states/layout.state';
/** @type {?} */
export var LAYOUTS = [LayoutApplicationComponent, LayoutAccountComponent, LayoutEmptyComponent];
var ThemeBasicModule = /** @class */ (function () {
    function ThemeBasicModule() {
    }
    ThemeBasicModule.decorators = [
        { type: NgModule, args: [{
                    declarations: tslib_1.__spread(LAYOUTS, [LayoutComponent, ChangePasswordComponent, ProfileComponent]),
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtYmFzaWMubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi90aGVtZS1iYXNpYy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLGlCQUFpQixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDbEYsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sd0RBQXdELENBQUM7QUFDakcsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sc0RBQXNELENBQUM7QUFDOUYsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sOERBQThELENBQUM7QUFDMUcsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sa0RBQWtELENBQUM7QUFDeEYsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLHNDQUFzQyxDQUFDO0FBQ3ZFLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLHdDQUF3QyxDQUFDO0FBQzFFLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDNUMsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDM0QsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sdUJBQXVCLENBQUM7O0FBRXBELE1BQU0sS0FBTyxPQUFPLEdBQUcsQ0FBQywwQkFBMEIsRUFBRSxzQkFBc0IsRUFBRSxvQkFBb0IsQ0FBQztBQUVqRztJQUFBO0lBYytCLENBQUM7O2dCQWQvQixRQUFRLFNBQUM7b0JBQ1IsWUFBWSxtQkFBTSxPQUFPLEdBQUUsZUFBZSxFQUFFLHVCQUF1QixFQUFFLGdCQUFnQixFQUFDO29CQUN0RixPQUFPLEVBQUU7d0JBQ1AsVUFBVTt3QkFDVixpQkFBaUI7d0JBQ2pCLGlCQUFpQjt3QkFDakIsaUJBQWlCO3dCQUNqQixXQUFXO3dCQUNYLHFCQUFxQjt3QkFDckIsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLFdBQVcsQ0FBQyxDQUFDO3FCQUNyQztvQkFDRCxPQUFPLG1CQUFNLE9BQU8sQ0FBQztvQkFDckIsZUFBZSxtQkFBTSxPQUFPLENBQUM7aUJBQzlCOztJQUM4Qix1QkFBQztDQUFBLEFBZGhDLElBY2dDO1NBQW5CLGdCQUFnQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYkNvbGxhcHNlTW9kdWxlLCBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcbmltcG9ydCB7IENoYW5nZVBhc3N3b3JkQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2NoYW5nZS1wYXNzd29yZC9jaGFuZ2UtcGFzc3dvcmQuY29tcG9uZW50JztcbmltcG9ydCB7IExheW91dEFjY291bnRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbGF5b3V0LWFjY291bnQvbGF5b3V0LWFjY291bnQuY29tcG9uZW50JztcbmltcG9ydCB7IExheW91dEFwcGxpY2F0aW9uQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2xheW91dC1hcHBsaWNhdGlvbi9sYXlvdXQtYXBwbGljYXRpb24uY29tcG9uZW50JztcbmltcG9ydCB7IExheW91dEVtcHR5Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2xheW91dC1lbXB0eS9sYXlvdXQtZW1wdHkuY29tcG9uZW50JztcbmltcG9ydCB7IExheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9sYXlvdXQvbGF5b3V0LmNvbXBvbmVudCc7XG5pbXBvcnQgeyBQcm9maWxlQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3Byb2ZpbGUvcHJvZmlsZS5jb21wb25lbnQnO1xuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBUb2FzdE1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdG9hc3QnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IE5neHNNb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBMYXlvdXRTdGF0ZSB9IGZyb20gJy4vc3RhdGVzL2xheW91dC5zdGF0ZSc7XG5cbmV4cG9ydCBjb25zdCBMQVlPVVRTID0gW0xheW91dEFwcGxpY2F0aW9uQ29tcG9uZW50LCBMYXlvdXRBY2NvdW50Q29tcG9uZW50LCBMYXlvdXRFbXB0eUNvbXBvbmVudF07XG5cbkBOZ01vZHVsZSh7XG4gIGRlY2xhcmF0aW9uczogWy4uLkxBWU9VVFMsIExheW91dENvbXBvbmVudCwgQ2hhbmdlUGFzc3dvcmRDb21wb25lbnQsIFByb2ZpbGVDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbXG4gICAgQ29yZU1vZHVsZSxcbiAgICBUaGVtZVNoYXJlZE1vZHVsZSxcbiAgICBOZ2JDb2xsYXBzZU1vZHVsZSxcbiAgICBOZ2JEcm9wZG93bk1vZHVsZSxcbiAgICBUb2FzdE1vZHVsZSxcbiAgICBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUsXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtMYXlvdXRTdGF0ZV0pLFxuICBdLFxuICBleHBvcnRzOiBbLi4uTEFZT1VUU10sXG4gIGVudHJ5Q29tcG9uZW50czogWy4uLkxBWU9VVFNdLFxufSlcbmV4cG9ydCBjbGFzcyBUaGVtZUJhc2ljTW9kdWxlIHt9XG4iXX0=