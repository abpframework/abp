/**
 * @fileoverview added by tsickle
 * Generated from: lib/identity.module.ts
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
var IdentityModule = /** @class */ (function () {
    function IdentityModule() {
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
    return IdentityModule;
}());
export { IdentityModule };
/**
 *
 * @deprecated
 * @return {?}
 */
export function IdentityProviders() {
    return [];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9pZGVudGl0eS5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxRQUFRLEVBQVksTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFDbEUsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLHlCQUF5QixDQUFDO0FBQ3hELE9BQU8sRUFBRSxlQUFlLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUNoRixPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sK0JBQStCLENBQUM7QUFDM0UsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUUzRDtJQUFBO0lBYzZCLENBQUM7O2dCQWQ3QixRQUFRLFNBQUM7b0JBQ1IsWUFBWSxFQUFFLENBQUMsY0FBYyxFQUFFLGNBQWMsQ0FBQztvQkFDOUMsT0FBTyxFQUFFO3dCQUNQLFVBQVUsQ0FBQyxVQUFVLENBQUMsQ0FBQyxhQUFhLENBQUMsQ0FBQzt3QkFDdEMsVUFBVTt3QkFDVixxQkFBcUI7d0JBQ3JCLGVBQWU7d0JBQ2YsaUJBQWlCO3dCQUNqQixXQUFXO3dCQUNYLGlCQUFpQjt3QkFDakIsMEJBQTBCO3dCQUMxQixxQkFBcUI7cUJBQ3RCO2lCQUNGOztJQUM0QixxQkFBQztDQUFBLEFBZDlCLElBYzhCO1NBQWpCLGNBQWM7Ozs7OztBQU0zQixNQUFNLFVBQVUsaUJBQWlCO0lBQy9CLE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgTmdNb2R1bGUsIFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOZ3hzTW9kdWxlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgUm9sZXNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcm9sZXMvcm9sZXMuY29tcG9uZW50JztcbmltcG9ydCB7IElkZW50aXR5Um91dGluZ01vZHVsZSB9IGZyb20gJy4vaWRlbnRpdHktcm91dGluZy5tb2R1bGUnO1xuaW1wb3J0IHsgSWRlbnRpdHlTdGF0ZSB9IGZyb20gJy4vc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcbmltcG9ydCB7IE5nYlRhYnNldE1vZHVsZSwgTmdiRHJvcGRvd25Nb2R1bGUgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IFVzZXJzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3VzZXJzL3VzZXJzLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFRhYmxlTW9kdWxlIH0gZnJvbSAncHJpbWVuZy90YWJsZSc7XG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuXG5ATmdNb2R1bGUoe1xuICBkZWNsYXJhdGlvbnM6IFtSb2xlc0NvbXBvbmVudCwgVXNlcnNDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtJZGVudGl0eVN0YXRlXSksXG4gICAgQ29yZU1vZHVsZSxcbiAgICBJZGVudGl0eVJvdXRpbmdNb2R1bGUsXG4gICAgTmdiVGFic2V0TW9kdWxlLFxuICAgIFRoZW1lU2hhcmVkTW9kdWxlLFxuICAgIFRhYmxlTW9kdWxlLFxuICAgIE5nYkRyb3Bkb3duTW9kdWxlLFxuICAgIFBlcm1pc3Npb25NYW5hZ2VtZW50TW9kdWxlLFxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZSxcbiAgXSxcbn0pXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlNb2R1bGUge31cblxuLyoqXG4gKlxuICogQGRlcHJlY2F0ZWRcbiAqL1xuZXhwb3J0IGZ1bmN0aW9uIElkZW50aXR5UHJvdmlkZXJzKCk6IFByb3ZpZGVyW10ge1xuICByZXR1cm4gW107XG59XG4iXX0=