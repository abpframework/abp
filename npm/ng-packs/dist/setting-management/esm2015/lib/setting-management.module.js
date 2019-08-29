/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { NgModule, Self } from '@angular/core';
import { SettingComponent } from './components/setting/setting.component';
import { SettingManagementRoutingModule } from './setting-management-routing.module';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { InitialService } from './components/services/initial.service';
export class SettingManagementModule {
    /**
     * @param {?} initialService
     */
    constructor(initialService) { }
}
SettingManagementModule.decorators = [
    { type: NgModule, args: [{
                declarations: [SettingComponent],
                imports: [SettingManagementRoutingModule, CoreModule, ThemeSharedModule],
                providers: [InitialService],
            },] }
];
/** @nocollapse */
SettingManagementModule.ctorParameters = () => [
    { type: InitialService, decorators: [{ type: Self }] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3NldHRpbmctbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQVEsTUFBTSxjQUFjLENBQUM7QUFDaEQsT0FBTyxFQUFFLFFBQVEsRUFBd0MsSUFBSSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3JGLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLHdDQUF3QyxDQUFDO0FBQzFFLE9BQU8sRUFBRSw4QkFBOEIsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBQ3JGLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQztBQU92RSxNQUFNLE9BQU8sdUJBQXVCOzs7O0lBQ2xDLFlBQW9CLGNBQThCLElBQUcsQ0FBQzs7O1lBTnZELFFBQVEsU0FBQztnQkFDUixZQUFZLEVBQUUsQ0FBQyxnQkFBZ0IsQ0FBQztnQkFDaEMsT0FBTyxFQUFFLENBQUMsOEJBQThCLEVBQUUsVUFBVSxFQUFFLGlCQUFpQixDQUFDO2dCQUN4RSxTQUFTLEVBQUUsQ0FBQyxjQUFjLENBQUM7YUFDNUI7Ozs7WUFOUSxjQUFjLHVCQVFSLElBQUkiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlLCBub29wIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IE5nTW9kdWxlLCBNb2R1bGVXaXRoUHJvdmlkZXJzLCBBUFBfSU5JVElBTElaRVIsIFNlbGYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFNldHRpbmdDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvc2V0dGluZy9zZXR0aW5nLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudFJvdXRpbmdNb2R1bGUgfSBmcm9tICcuL3NldHRpbmctbWFuYWdlbWVudC1yb3V0aW5nLm1vZHVsZSc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IEluaXRpYWxTZXJ2aWNlIH0gZnJvbSAnLi9jb21wb25lbnRzL3NlcnZpY2VzL2luaXRpYWwuc2VydmljZSc7XG5cbkBOZ01vZHVsZSh7XG4gIGRlY2xhcmF0aW9uczogW1NldHRpbmdDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbU2V0dGluZ01hbmFnZW1lbnRSb3V0aW5nTW9kdWxlLCBDb3JlTW9kdWxlLCBUaGVtZVNoYXJlZE1vZHVsZV0sXG4gIHByb3ZpZGVyczogW0luaXRpYWxTZXJ2aWNlXSxcbn0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRNb2R1bGUge1xuICBjb25zdHJ1Y3RvcihAU2VsZigpIGluaXRpYWxTZXJ2aWNlOiBJbml0aWFsU2VydmljZSkge31cbn1cbiJdfQ==