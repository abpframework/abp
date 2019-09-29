/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { PermissionManagementComponent } from './components/permission-management.component';
import { PermissionManagementState } from './states/permission-management.state';
var PermissionManagementModule = /** @class */ (function () {
    function PermissionManagementModule() {
    }
    PermissionManagementModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [PermissionManagementComponent],
                    imports: [CoreModule, ThemeSharedModule, NgxsModule.forFeature([PermissionManagementState])],
                    exports: [PermissionManagementComponent],
                },] }
    ];
    return PermissionManagementModule;
}());
export { PermissionManagementModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3Blcm1pc3Npb24tbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSw2QkFBNkIsRUFBRSxNQUFNLDhDQUE4QyxDQUFDO0FBQzdGLE9BQU8sRUFBRSx5QkFBeUIsRUFBRSxNQUFNLHNDQUFzQyxDQUFDO0FBRWpGO0lBQUE7SUFLeUMsQ0FBQzs7Z0JBTHpDLFFBQVEsU0FBQztvQkFDUixZQUFZLEVBQUUsQ0FBQyw2QkFBNkIsQ0FBQztvQkFDN0MsT0FBTyxFQUFFLENBQUMsVUFBVSxFQUFFLGlCQUFpQixFQUFFLFVBQVUsQ0FBQyxVQUFVLENBQUMsQ0FBQyx5QkFBeUIsQ0FBQyxDQUFDLENBQUM7b0JBQzVGLE9BQU8sRUFBRSxDQUFDLDZCQUE2QixDQUFDO2lCQUN6Qzs7SUFDd0MsaUNBQUM7Q0FBQSxBQUwxQyxJQUswQztTQUE3QiwwQkFBMEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5neHNNb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuY29tcG9uZW50JztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuL3N0YXRlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc3RhdGUnO1xuXG5ATmdNb2R1bGUoe1xuICBkZWNsYXJhdGlvbnM6IFtQZXJtaXNzaW9uTWFuYWdlbWVudENvbXBvbmVudF0sXG4gIGltcG9ydHM6IFtDb3JlTW9kdWxlLCBUaGVtZVNoYXJlZE1vZHVsZSwgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlXSldLFxuICBleHBvcnRzOiBbUGVybWlzc2lvbk1hbmFnZW1lbnRDb21wb25lbnRdLFxufSlcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uTWFuYWdlbWVudE1vZHVsZSB7fVxuIl19