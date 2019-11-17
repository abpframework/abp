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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9pZGVudGl0eS5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxRQUFRLEVBQVksTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFDbEUsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLHlCQUF5QixDQUFDO0FBQ3hELE9BQU8sRUFBRSxlQUFlLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUNoRixPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sK0JBQStCLENBQUM7QUFDM0UsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1QyxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQWdCM0QsTUFBTSxPQUFPLGNBQWM7OztZQWQxQixRQUFRLFNBQUM7Z0JBQ1IsWUFBWSxFQUFFLENBQUMsY0FBYyxFQUFFLGNBQWMsQ0FBQztnQkFDOUMsT0FBTyxFQUFFO29CQUNQLFVBQVUsQ0FBQyxVQUFVLENBQUMsQ0FBQyxhQUFhLENBQUMsQ0FBQztvQkFDdEMsVUFBVTtvQkFDVixxQkFBcUI7b0JBQ3JCLGVBQWU7b0JBQ2YsaUJBQWlCO29CQUNqQixXQUFXO29CQUNYLGlCQUFpQjtvQkFDakIsMEJBQTBCO29CQUMxQixxQkFBcUI7aUJBQ3RCO2FBQ0Y7Ozs7Ozs7QUFPRCxNQUFNLFVBQVUsaUJBQWlCO0lBQy9CLE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBOZ01vZHVsZSwgUHJvdmlkZXIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgTmd4c01vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgUm9sZXNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcm9sZXMvcm9sZXMuY29tcG9uZW50JztcclxuaW1wb3J0IHsgSWRlbnRpdHlSb3V0aW5nTW9kdWxlIH0gZnJvbSAnLi9pZGVudGl0eS1yb3V0aW5nLm1vZHVsZSc7XHJcbmltcG9ydCB7IElkZW50aXR5U3RhdGUgfSBmcm9tICcuL3N0YXRlcy9pZGVudGl0eS5zdGF0ZSc7XHJcbmltcG9ydCB7IE5nYlRhYnNldE1vZHVsZSwgTmdiRHJvcGRvd25Nb2R1bGUgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XHJcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xyXG5pbXBvcnQgeyBVc2Vyc0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy91c2Vycy91c2Vycy5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50JztcclxuaW1wb3J0IHsgVGFibGVNb2R1bGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcclxuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgZGVjbGFyYXRpb25zOiBbUm9sZXNDb21wb25lbnQsIFVzZXJzQ29tcG9uZW50XSxcclxuICBpbXBvcnRzOiBbXHJcbiAgICBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW0lkZW50aXR5U3RhdGVdKSxcclxuICAgIENvcmVNb2R1bGUsXHJcbiAgICBJZGVudGl0eVJvdXRpbmdNb2R1bGUsXHJcbiAgICBOZ2JUYWJzZXRNb2R1bGUsXHJcbiAgICBUaGVtZVNoYXJlZE1vZHVsZSxcclxuICAgIFRhYmxlTW9kdWxlLFxyXG4gICAgTmdiRHJvcGRvd25Nb2R1bGUsXHJcbiAgICBQZXJtaXNzaW9uTWFuYWdlbWVudE1vZHVsZSxcclxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZSxcclxuICBdLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlNb2R1bGUge31cclxuXHJcbi8qKlxyXG4gKlxyXG4gKiBAZGVwcmVjYXRlZFxyXG4gKi9cclxuZXhwb3J0IGZ1bmN0aW9uIElkZW50aXR5UHJvdmlkZXJzKCk6IFByb3ZpZGVyW10ge1xyXG4gIHJldHVybiBbXTtcclxufVxyXG4iXX0=