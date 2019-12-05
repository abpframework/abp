/**
 * @fileoverview added by tsickle
 * Generated from: lib/setting-management.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { SettingManagementRoutingModule } from './setting-management-routing.module';
import { SettingManagementComponent } from './components/setting-management.component';
import { NgxsModule } from '@ngxs/store';
import { SettingManagementState } from './states/setting-management.state';
export class SettingManagementModule {
}
SettingManagementModule.decorators = [
    { type: NgModule, args: [{
                declarations: [SettingManagementComponent],
                imports: [
                    SettingManagementRoutingModule,
                    CoreModule,
                    ThemeSharedModule,
                    NgxsModule.forFeature([SettingManagementState]),
                ],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3NldHRpbmctbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLDhCQUE4QixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFDckYsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sMkNBQTJDLENBQUM7QUFDdkYsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQztBQVczRSxNQUFNLE9BQU8sdUJBQXVCOzs7WUFUbkMsUUFBUSxTQUFDO2dCQUNSLFlBQVksRUFBRSxDQUFDLDBCQUEwQixDQUFDO2dCQUMxQyxPQUFPLEVBQUU7b0JBQ1AsOEJBQThCO29CQUM5QixVQUFVO29CQUNWLGlCQUFpQjtvQkFDakIsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLHNCQUFzQixDQUFDLENBQUM7aUJBQ2hEO2FBQ0YiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50Um91dGluZ01vZHVsZSB9IGZyb20gJy4vc2V0dGluZy1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlJztcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3NldHRpbmctbWFuYWdlbWVudC5jb21wb25lbnQnO1xuaW1wb3J0IHsgTmd4c01vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuL3N0YXRlcy9zZXR0aW5nLW1hbmFnZW1lbnQuc3RhdGUnO1xuXG5ATmdNb2R1bGUoe1xuICBkZWNsYXJhdGlvbnM6IFtTZXR0aW5nTWFuYWdlbWVudENvbXBvbmVudF0sXG4gIGltcG9ydHM6IFtcbiAgICBTZXR0aW5nTWFuYWdlbWVudFJvdXRpbmdNb2R1bGUsXG4gICAgQ29yZU1vZHVsZSxcbiAgICBUaGVtZVNoYXJlZE1vZHVsZSxcbiAgICBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW1NldHRpbmdNYW5hZ2VtZW50U3RhdGVdKSxcbiAgXSxcbn0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRNb2R1bGUge31cbiJdfQ==