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
export var IDENTITY_ROUTES = {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb25zdGFudHMvcm91dGVzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7QUFNQSxNQUFNLEtBQU8sZUFBZSxHQUFHO0lBQzdCLE1BQU0sRUFBRSxtQkFBQTtRQUNOO1lBQ0UsSUFBSSxFQUFFLHNDQUFzQztZQUM1QyxJQUFJLEVBQUUsRUFBRTtZQUNSLEtBQUssRUFBRSxDQUFDO1lBQ1IsT0FBTyxFQUFFLElBQUk7U0FDZDtRQUNEO1lBQ0UsSUFBSSxFQUFFLHNDQUFzQztZQUM1QyxJQUFJLEVBQUUsVUFBVTtZQUNoQixLQUFLLEVBQUUsQ0FBQztZQUNSLFVBQVUsRUFBRSxzQ0FBc0M7WUFDbEQsTUFBTSxpQ0FBeUI7WUFDL0IsU0FBUyxFQUFFLGlCQUFpQjtZQUM1QixRQUFRLEVBQUU7Z0JBQ1IsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtnQkFDNUYsRUFBRSxJQUFJLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxvQkFBb0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTthQUM3RjtTQUNGO0tBQ0YsRUFBbUI7Q0FDckIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBlTGF5b3V0VHlwZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuLyoqXG4gKlxuICogQGRlcHJlY2F0ZWRcbiAqL1xuZXhwb3J0IGNvbnN0IElERU5USVRZX1JPVVRFUyA9IHtcbiAgcm91dGVzOiBbXG4gICAge1xuICAgICAgbmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgICBwYXRoOiAnJyxcbiAgICAgIG9yZGVyOiAxLFxuICAgICAgd3JhcHBlcjogdHJ1ZSxcbiAgICB9LFxuICAgIHtcbiAgICAgIG5hbWU6ICdBYnBJZGVudGl0eTo6TWVudTpJZGVudGl0eU1hbmFnZW1lbnQnLFxuICAgICAgcGF0aDogJ2lkZW50aXR5JyxcbiAgICAgIG9yZGVyOiAxLFxuICAgICAgcGFyZW50TmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxuICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtaWQtY2FyZC1vJyxcbiAgICAgIGNoaWxkcmVuOiBbXG4gICAgICAgIHsgcGF0aDogJ3JvbGVzJywgbmFtZTogJ0FicElkZW50aXR5OjpSb2xlcycsIG9yZGVyOiAyLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlJvbGVzJyB9LFxuICAgICAgICB7IHBhdGg6ICd1c2VycycsIG5hbWU6ICdBYnBJZGVudGl0eTo6VXNlcnMnLCBvcmRlcjogMSwgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Vc2VycycgfSxcbiAgICAgIF0sXG4gICAgfSxcbiAgXSBhcyBBQlAuRnVsbFJvdXRlW10sXG59O1xuIl19