/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
 * @return {?}
 */
export function IdentityProviders() {
    return [];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9pZGVudGl0eS5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNsRSxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFDeEQsT0FBTyxFQUFFLGVBQWUsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQ2hGLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsMEJBQTBCLEVBQUUsTUFBTSwrQkFBK0IsQ0FBQztBQUMzRSxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBRTNEO0lBQUE7SUFjNkIsQ0FBQzs7Z0JBZDdCLFFBQVEsU0FBQztvQkFDUixZQUFZLEVBQUUsQ0FBQyxjQUFjLEVBQUUsY0FBYyxDQUFDO29CQUM5QyxPQUFPLEVBQUU7d0JBQ1AsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLGFBQWEsQ0FBQyxDQUFDO3dCQUN0QyxVQUFVO3dCQUNWLHFCQUFxQjt3QkFDckIsZUFBZTt3QkFDZixpQkFBaUI7d0JBQ2pCLFdBQVc7d0JBQ1gsaUJBQWlCO3dCQUNqQiwwQkFBMEI7d0JBQzFCLHFCQUFxQjtxQkFDdEI7aUJBQ0Y7O0lBQzRCLHFCQUFDO0NBQUEsQUFkOUIsSUFjOEI7U0FBakIsY0FBYzs7OztBQUUzQixNQUFNLFVBQVUsaUJBQWlCO0lBQy9CLE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgTmdNb2R1bGUsIFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOZ3hzTW9kdWxlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgUm9sZXNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcm9sZXMvcm9sZXMuY29tcG9uZW50JztcbmltcG9ydCB7IElkZW50aXR5Um91dGluZ01vZHVsZSB9IGZyb20gJy4vaWRlbnRpdHktcm91dGluZy5tb2R1bGUnO1xuaW1wb3J0IHsgSWRlbnRpdHlTdGF0ZSB9IGZyb20gJy4vc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcbmltcG9ydCB7IE5nYlRhYnNldE1vZHVsZSwgTmdiRHJvcGRvd25Nb2R1bGUgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IFVzZXJzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3VzZXJzL3VzZXJzLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFRhYmxlTW9kdWxlIH0gZnJvbSAncHJpbWVuZy90YWJsZSc7XG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuXG5ATmdNb2R1bGUoe1xuICBkZWNsYXJhdGlvbnM6IFtSb2xlc0NvbXBvbmVudCwgVXNlcnNDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtJZGVudGl0eVN0YXRlXSksXG4gICAgQ29yZU1vZHVsZSxcbiAgICBJZGVudGl0eVJvdXRpbmdNb2R1bGUsXG4gICAgTmdiVGFic2V0TW9kdWxlLFxuICAgIFRoZW1lU2hhcmVkTW9kdWxlLFxuICAgIFRhYmxlTW9kdWxlLFxuICAgIE5nYkRyb3Bkb3duTW9kdWxlLFxuICAgIFBlcm1pc3Npb25NYW5hZ2VtZW50TW9kdWxlLFxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZSxcbiAgXSxcbn0pXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlNb2R1bGUge31cblxuZXhwb3J0IGZ1bmN0aW9uIElkZW50aXR5UHJvdmlkZXJzKCk6IFByb3ZpZGVyW10ge1xuICByZXR1cm4gW107XG59XG4iXX0=