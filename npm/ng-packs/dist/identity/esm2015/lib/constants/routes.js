/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
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
    settings: [],
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb25zdGFudHMvcm91dGVzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBRUEsTUFBTSxPQUFPLGVBQWUsR0FBRztJQUM3QixNQUFNLEVBQUUsbUJBQUE7UUFDTjtZQUNFLElBQUksRUFBRSxzQ0FBc0M7WUFDNUMsSUFBSSxFQUFFLEVBQUU7WUFDUixLQUFLLEVBQUUsQ0FBQztZQUNSLE9BQU8sRUFBRSxJQUFJO1NBQ2Q7UUFDRDtZQUNFLElBQUksRUFBRSxzQ0FBc0M7WUFDNUMsSUFBSSxFQUFFLFVBQVU7WUFDaEIsS0FBSyxFQUFFLENBQUM7WUFDUixVQUFVLEVBQUUsc0NBQXNDO1lBQ2xELE1BQU0saUNBQXlCO1lBQy9CLFNBQVMsRUFBRSxpQkFBaUI7WUFDNUIsUUFBUSxFQUFFO2dCQUNSLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsb0JBQW9CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7Z0JBQzVGLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsb0JBQW9CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7YUFDN0Y7U0FDRjtLQUNGLEVBQW1CO0lBQ3BCLFFBQVEsRUFBRSxFQUFFO0NBQ2IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBlTGF5b3V0VHlwZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuZXhwb3J0IGNvbnN0IElERU5USVRZX1JPVVRFUyA9IHtcbiAgcm91dGVzOiBbXG4gICAge1xuICAgICAgbmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgICBwYXRoOiAnJyxcbiAgICAgIG9yZGVyOiAxLFxuICAgICAgd3JhcHBlcjogdHJ1ZSxcbiAgICB9LFxuICAgIHtcbiAgICAgIG5hbWU6ICdBYnBJZGVudGl0eTo6TWVudTpJZGVudGl0eU1hbmFnZW1lbnQnLFxuICAgICAgcGF0aDogJ2lkZW50aXR5JyxcbiAgICAgIG9yZGVyOiAxLFxuICAgICAgcGFyZW50TmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxuICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtaWQtY2FyZC1vJyxcbiAgICAgIGNoaWxkcmVuOiBbXG4gICAgICAgIHsgcGF0aDogJ3JvbGVzJywgbmFtZTogJ0FicElkZW50aXR5OjpSb2xlcycsIG9yZGVyOiAyLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlJvbGVzJyB9LFxuICAgICAgICB7IHBhdGg6ICd1c2VycycsIG5hbWU6ICdBYnBJZGVudGl0eTo6VXNlcnMnLCBvcmRlcjogMSwgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Vc2VycycgfSxcbiAgICAgIF0sXG4gICAgfSxcbiAgXSBhcyBBQlAuRnVsbFJvdXRlW10sXG4gIHNldHRpbmdzOiBbXSxcbn07XG4iXX0=