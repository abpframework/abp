/**
 * @fileoverview added by tsickle
 * Generated from: lib/feature-management.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { FeatureManagementComponent } from './components/feature-management/feature-management.component';
import { NgxsModule } from '@ngxs/store';
import { FeatureManagementState } from './states/feature-management.state';
var FeatureManagementModule = /** @class */ (function () {
    function FeatureManagementModule() {
    }
    FeatureManagementModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [FeatureManagementComponent],
                    imports: [CoreModule, ThemeSharedModule, NgxsModule.forFeature([FeatureManagementState])],
                    exports: [FeatureManagementComponent],
                },] }
    ];
    return FeatureManagementModule;
}());
export { FeatureManagementModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2ZlYXR1cmUtbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDekMsT0FBTyxFQUFFLDBCQUEwQixFQUFFLE1BQU0sOERBQThELENBQUM7QUFDMUcsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQztBQUUzRTtJQUFBO0lBS3NDLENBQUM7O2dCQUx0QyxRQUFRLFNBQUM7b0JBQ1IsWUFBWSxFQUFFLENBQUMsMEJBQTBCLENBQUM7b0JBQzFDLE9BQU8sRUFBRSxDQUFDLFVBQVUsRUFBRSxpQkFBaUIsRUFBRSxVQUFVLENBQUMsVUFBVSxDQUFDLENBQUMsc0JBQXNCLENBQUMsQ0FBQyxDQUFDO29CQUN6RixPQUFPLEVBQUUsQ0FBQywwQkFBMEIsQ0FBQztpQkFDdEM7O0lBQ3FDLDhCQUFDO0NBQUEsQUFMdkMsSUFLdUM7U0FBMUIsdUJBQXVCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudENvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9mZWF0dXJlLW1hbmFnZW1lbnQvZmVhdHVyZS1tYW5hZ2VtZW50LmNvbXBvbmVudCc7XG5pbXBvcnQgeyBOZ3hzTW9kdWxlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4vc3RhdGVzL2ZlYXR1cmUtbWFuYWdlbWVudC5zdGF0ZSc7XG5cbkBOZ01vZHVsZSh7XG4gIGRlY2xhcmF0aW9uczogW0ZlYXR1cmVNYW5hZ2VtZW50Q29tcG9uZW50XSxcbiAgaW1wb3J0czogW0NvcmVNb2R1bGUsIFRoZW1lU2hhcmVkTW9kdWxlLCBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW0ZlYXR1cmVNYW5hZ2VtZW50U3RhdGVdKV0sXG4gIGV4cG9ydHM6IFtGZWF0dXJlTWFuYWdlbWVudENvbXBvbmVudF0sXG59KVxuZXhwb3J0IGNsYXNzIEZlYXR1cmVNYW5hZ2VtZW50TW9kdWxlIHt9XG4iXX0=