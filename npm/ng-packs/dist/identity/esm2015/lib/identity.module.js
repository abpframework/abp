/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { RolesComponent } from './components/roles/roles.component';
import { IdentityRoutingModule } from './identity-routing.module';
import { IdentityState } from './states/identity.state';
import { NgbTabsetModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { UsersComponent } from './components/users/users.component';
import { PermissionManagementModule } from '@abp/ng.permission-management';
import { TableModule } from 'primeng/table';
import { NgxValidateCoreModule } from '@ngx-validate/core';
export class IdentityModule {
}
IdentityModule.decorators = [
    { type: NgModule, args: [{
                declarations: [RolesComponent, UsersComponent],
                imports: [
                    NgxsModule.forFeature([IdentityState]),
                    CoreModule,
                    IdentityRoutingModule,
                    NgbTabsetModule,
                    ThemeSharedModule,
                    TableModule,
                    NgbDropdownModule,
                    PermissionManagementModule,
                    NgxValidateCoreModule,
                ],
            },] }
];
/**
 *
 * @deprecated
 * @return {?}
 */
export function IdentityProviders() {
    return [];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9pZGVudGl0eS5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNsRSxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFDeEQsT0FBTyxFQUFFLGVBQWUsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQ2hGLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsMEJBQTBCLEVBQUUsTUFBTSwrQkFBK0IsQ0FBQztBQUMzRSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBZ0IzRCxNQUFNLE9BQU8sY0FBYzs7O1lBZDFCLFFBQVEsU0FBQztnQkFDUixZQUFZLEVBQUUsQ0FBQyxjQUFjLEVBQUUsY0FBYyxDQUFDO2dCQUM5QyxPQUFPLEVBQUU7b0JBQ1AsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLGFBQWEsQ0FBQyxDQUFDO29CQUN0QyxVQUFVO29CQUNWLHFCQUFxQjtvQkFDckIsZUFBZTtvQkFDZixpQkFBaUI7b0JBQ2pCLFdBQVc7b0JBQ1gsaUJBQWlCO29CQUNqQiwwQkFBMEI7b0JBQzFCLHFCQUFxQjtpQkFDdEI7YUFDRjs7Ozs7OztBQU9ELE1BQU0sVUFBVSxpQkFBaUI7SUFDL0IsT0FBTyxFQUFFLENBQUM7QUFDWixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IE5nTW9kdWxlLCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBOZ3hzTW9kdWxlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBSb2xlc0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9yb2xlcy9yb2xlcy5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBJZGVudGl0eVJvdXRpbmdNb2R1bGUgfSBmcm9tICcuL2lkZW50aXR5LXJvdXRpbmcubW9kdWxlJztcclxuaW1wb3J0IHsgSWRlbnRpdHlTdGF0ZSB9IGZyb20gJy4vc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcclxuaW1wb3J0IHsgTmdiVGFic2V0TW9kdWxlLCBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcclxuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IFVzZXJzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3VzZXJzL3VzZXJzLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50TW9kdWxlIH0gZnJvbSAnQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQnO1xyXG5pbXBvcnQgeyBUYWJsZU1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdGFibGUnO1xyXG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xyXG5cclxuQE5nTW9kdWxlKHtcclxuICBkZWNsYXJhdGlvbnM6IFtSb2xlc0NvbXBvbmVudCwgVXNlcnNDb21wb25lbnRdLFxyXG4gIGltcG9ydHM6IFtcclxuICAgIE5neHNNb2R1bGUuZm9yRmVhdHVyZShbSWRlbnRpdHlTdGF0ZV0pLFxyXG4gICAgQ29yZU1vZHVsZSxcclxuICAgIElkZW50aXR5Um91dGluZ01vZHVsZSxcclxuICAgIE5nYlRhYnNldE1vZHVsZSxcclxuICAgIFRoZW1lU2hhcmVkTW9kdWxlLFxyXG4gICAgVGFibGVNb2R1bGUsXHJcbiAgICBOZ2JEcm9wZG93bk1vZHVsZSxcclxuICAgIFBlcm1pc3Npb25NYW5hZ2VtZW50TW9kdWxlLFxyXG4gICAgTmd4VmFsaWRhdGVDb3JlTW9kdWxlLFxyXG4gIF0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBJZGVudGl0eU1vZHVsZSB7fVxyXG5cclxuLyoqXHJcbiAqXHJcbiAqIEBkZXByZWNhdGVkXHJcbiAqL1xyXG5leHBvcnQgZnVuY3Rpb24gSWRlbnRpdHlQcm92aWRlcnMoKTogUHJvdmlkZXJbXSB7XHJcbiAgcmV0dXJuIFtdO1xyXG59XHJcbiJdfQ==