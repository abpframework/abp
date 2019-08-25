/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { AccountLayoutComponent } from './components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from './components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from './components/empty-layout/empty-layout.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ToastModule } from 'primeng/toast';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { LayoutState } from './states/layout.state';
/** @type {?} */
export const LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];
export class ThemeBasicModule {
}
ThemeBasicModule.decorators = [
    { type: NgModule, args: [{
                declarations: [...LAYOUTS, LayoutComponent, ChangePasswordComponent, ProfileComponent],
                imports: [
                    CoreModule,
                    ThemeSharedModule,
                    NgbCollapseModule,
                    NgbDropdownModule,
                    ToastModule,
                    NgxValidateCoreModule,
                    NgxsModule.forFeature([LayoutState]),
                ],
                exports: [...LAYOUTS],
                entryComponents: [...LAYOUTS],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGhlbWUtYmFzaWMubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi90aGVtZS1iYXNpYy5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUNsRixPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSx3REFBd0QsQ0FBQztBQUNqRyxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxzREFBc0QsQ0FBQztBQUM5RixPQUFPLEVBQUUsMEJBQTBCLEVBQUUsTUFBTSw4REFBOEQsQ0FBQztBQUMxRyxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsTUFBTSxrREFBa0QsQ0FBQztBQUN4RixPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFDdkUsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7QUFDMUUsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUMzRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx1QkFBdUIsQ0FBQzs7QUFFcEQsTUFBTSxPQUFPLE9BQU8sR0FBRyxDQUFDLDBCQUEwQixFQUFFLHNCQUFzQixFQUFFLG9CQUFvQixDQUFDO0FBZ0JqRyxNQUFNLE9BQU8sZ0JBQWdCOzs7WUFkNUIsUUFBUSxTQUFDO2dCQUNSLFlBQVksRUFBRSxDQUFDLEdBQUcsT0FBTyxFQUFFLGVBQWUsRUFBRSx1QkFBdUIsRUFBRSxnQkFBZ0IsQ0FBQztnQkFDdEYsT0FBTyxFQUFFO29CQUNQLFVBQVU7b0JBQ1YsaUJBQWlCO29CQUNqQixpQkFBaUI7b0JBQ2pCLGlCQUFpQjtvQkFDakIsV0FBVztvQkFDWCxxQkFBcUI7b0JBQ3JCLFVBQVUsQ0FBQyxVQUFVLENBQUMsQ0FBQyxXQUFXLENBQUMsQ0FBQztpQkFDckM7Z0JBQ0QsT0FBTyxFQUFFLENBQUMsR0FBRyxPQUFPLENBQUM7Z0JBQ3JCLGVBQWUsRUFBRSxDQUFDLEdBQUcsT0FBTyxDQUFDO2FBQzlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiQ29sbGFwc2VNb2R1bGUsIE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgQ2hhbmdlUGFzc3dvcmRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvY2hhbmdlLXBhc3N3b3JkL2NoYW5nZS1wYXNzd29yZC5jb21wb25lbnQnO1xuaW1wb3J0IHsgQWNjb3VudExheW91dENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9hY2NvdW50LWxheW91dC9hY2NvdW50LWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgQXBwbGljYXRpb25MYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvYXBwbGljYXRpb24tbGF5b3V0L2FwcGxpY2F0aW9uLWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgRW1wdHlMYXlvdXRDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvZW1wdHktbGF5b3V0L2VtcHR5LWxheW91dC5jb21wb25lbnQnO1xuaW1wb3J0IHsgTGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2xheW91dC9sYXlvdXQuY29tcG9uZW50JztcbmltcG9ydCB7IFByb2ZpbGVDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcHJvZmlsZS9wcm9maWxlLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IFRvYXN0TW9kdWxlIH0gZnJvbSAncHJpbWVuZy90b2FzdCc7XG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuaW1wb3J0IHsgTmd4c01vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IExheW91dFN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvbGF5b3V0LnN0YXRlJztcblxuZXhwb3J0IGNvbnN0IExBWU9VVFMgPSBbQXBwbGljYXRpb25MYXlvdXRDb21wb25lbnQsIEFjY291bnRMYXlvdXRDb21wb25lbnQsIEVtcHR5TGF5b3V0Q29tcG9uZW50XTtcblxuQE5nTW9kdWxlKHtcbiAgZGVjbGFyYXRpb25zOiBbLi4uTEFZT1VUUywgTGF5b3V0Q29tcG9uZW50LCBDaGFuZ2VQYXNzd29yZENvbXBvbmVudCwgUHJvZmlsZUNvbXBvbmVudF0sXG4gIGltcG9ydHM6IFtcbiAgICBDb3JlTW9kdWxlLFxuICAgIFRoZW1lU2hhcmVkTW9kdWxlLFxuICAgIE5nYkNvbGxhcHNlTW9kdWxlLFxuICAgIE5nYkRyb3Bkb3duTW9kdWxlLFxuICAgIFRvYXN0TW9kdWxlLFxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZSxcbiAgICBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW0xheW91dFN0YXRlXSksXG4gIF0sXG4gIGV4cG9ydHM6IFsuLi5MQVlPVVRTXSxcbiAgZW50cnlDb21wb25lbnRzOiBbLi4uTEFZT1VUU10sXG59KVxuZXhwb3J0IGNsYXNzIFRoZW1lQmFzaWNNb2R1bGUge31cbiJdfQ==