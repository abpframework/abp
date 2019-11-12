/**
 * @fileoverview added by tsickle
 * Generated from: lib/constants/routes.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 *
 * @deprecated
 * @type {?}
 */
export const IDENTITY_ROUTES = {
    routes: (/** @type {?} */ ([
        {
            name: 'AbpUiNavigation::Menu:Administration',
            path: '',
            order: 1,
            wrapper: true,
        },
        {
            name: 'AbpIdentity::Menu:IdentityManagement',
            path: 'identity',
            order: 1,
            parentName: 'AbpUiNavigation::Menu:Administration',
            layout: "application" /* application */,
            iconClass: 'fa fa-id-card-o',
            children: [
                { path: 'roles', name: 'AbpIdentity::Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
                { path: 'users', name: 'AbpIdentity::Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
            ],
        },
    ])),
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb25zdGFudHMvcm91dGVzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7QUFNQSxNQUFNLE9BQU8sZUFBZSxHQUFHO0lBQzdCLE1BQU0sRUFBRSxtQkFBQTtRQUNOO1lBQ0UsSUFBSSxFQUFFLHNDQUFzQztZQUM1QyxJQUFJLEVBQUUsRUFBRTtZQUNSLEtBQUssRUFBRSxDQUFDO1lBQ1IsT0FBTyxFQUFFLElBQUk7U0FDZDtRQUNEO1lBQ0UsSUFBSSxFQUFFLHNDQUFzQztZQUM1QyxJQUFJLEVBQUUsVUFBVTtZQUNoQixLQUFLLEVBQUUsQ0FBQztZQUNSLFVBQVUsRUFBRSxzQ0FBc0M7WUFDbEQsTUFBTSxpQ0FBeUI7WUFDL0IsU0FBUyxFQUFFLGlCQUFpQjtZQUM1QixRQUFRLEVBQUU7Z0JBQ1IsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtnQkFDNUYsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTthQUM3RjtTQUNGO0tBQ0YsRUFBbUI7Q0FDckIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBlTGF5b3V0VHlwZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbi8qKlxyXG4gKlxyXG4gKiBAZGVwcmVjYXRlZFxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IElERU5USVRZX1JPVVRFUyA9IHtcclxuICByb3V0ZXM6IFtcclxuICAgIHtcclxuICAgICAgbmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXHJcbiAgICAgIHBhdGg6ICcnLFxyXG4gICAgICBvcmRlcjogMSxcclxuICAgICAgd3JhcHBlcjogdHJ1ZSxcclxuICAgIH0sXHJcbiAgICB7XHJcbiAgICAgIG5hbWU6ICdBYnBJZGVudGl0eTo6TWVudTpJZGVudGl0eU1hbmFnZW1lbnQnLFxyXG4gICAgICBwYXRoOiAnaWRlbnRpdHknLFxyXG4gICAgICBvcmRlcjogMSxcclxuICAgICAgcGFyZW50TmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXHJcbiAgICAgIGxheW91dDogZUxheW91dFR5cGUuYXBwbGljYXRpb24sXHJcbiAgICAgIGljb25DbGFzczogJ2ZhIGZhLWlkLWNhcmQtbycsXHJcbiAgICAgIGNoaWxkcmVuOiBbXHJcbiAgICAgICAgeyBwYXRoOiAncm9sZXMnLCBuYW1lOiAnQWJwSWRlbnRpdHk6OlJvbGVzJywgb3JkZXI6IDIsIHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuUm9sZXMnIH0sXHJcbiAgICAgICAgeyBwYXRoOiAndXNlcnMnLCBuYW1lOiAnQWJwSWRlbnRpdHk6OlVzZXJzJywgb3JkZXI6IDEsIHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuVXNlcnMnIH0sXHJcbiAgICAgIF0sXHJcbiAgICB9LFxyXG4gIF0gYXMgQUJQLkZ1bGxSb3V0ZVtdLFxyXG59O1xyXG4iXX0=