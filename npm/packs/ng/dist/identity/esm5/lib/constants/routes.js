/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
export var IDENTITY_ROUTES = (/** @type {?} */ ([
    {
        name: 'Administration',
        path: '',
        order: 1,
        wrapper: true,
    },
    {
        name: 'Identity',
        path: 'identity',
        order: 1,
        parentName: 'Administration',
        layout: "application" /* application */,
        children: [
            { path: 'roles', name: 'Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
            { path: 'users', name: 'Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
        ],
    },
]));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb25zdGFudHMvcm91dGVzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBRUEsTUFBTSxLQUFPLGVBQWUsR0FBRyxtQkFBQTtJQUM3QjtRQUNFLElBQUksRUFBRSxnQkFBZ0I7UUFDdEIsSUFBSSxFQUFFLEVBQUU7UUFDUixLQUFLLEVBQUUsQ0FBQztRQUNSLE9BQU8sRUFBRSxJQUFJO0tBQ2Q7SUFDRDtRQUNFLElBQUksRUFBRSxVQUFVO1FBQ2hCLElBQUksRUFBRSxVQUFVO1FBQ2hCLEtBQUssRUFBRSxDQUFDO1FBQ1IsVUFBVSxFQUFFLGdCQUFnQjtRQUM1QixNQUFNLGlDQUF5QjtRQUMvQixRQUFRLEVBQUU7WUFDUixFQUFFLElBQUksRUFBRSxPQUFPLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtZQUMvRSxFQUFFLElBQUksRUFBRSxPQUFPLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTtTQUNoRjtLQUNGO0NBQ0YsRUFBbUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBlTGF5b3V0VHlwZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuZXhwb3J0IGNvbnN0IElERU5USVRZX1JPVVRFUyA9IFtcbiAge1xuICAgIG5hbWU6ICdBZG1pbmlzdHJhdGlvbicsXG4gICAgcGF0aDogJycsXG4gICAgb3JkZXI6IDEsXG4gICAgd3JhcHBlcjogdHJ1ZSxcbiAgfSxcbiAge1xuICAgIG5hbWU6ICdJZGVudGl0eScsXG4gICAgcGF0aDogJ2lkZW50aXR5JyxcbiAgICBvcmRlcjogMSxcbiAgICBwYXJlbnROYW1lOiAnQWRtaW5pc3RyYXRpb24nLFxuICAgIGxheW91dDogZUxheW91dFR5cGUuYXBwbGljYXRpb24sXG4gICAgY2hpbGRyZW46IFtcbiAgICAgIHsgcGF0aDogJ3JvbGVzJywgbmFtZTogJ1JvbGVzJywgb3JkZXI6IDIsIHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuUm9sZXMnIH0sXG4gICAgICB7IHBhdGg6ICd1c2VycycsIG5hbWU6ICdVc2VycycsIG9yZGVyOiAxLCByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlVzZXJzJyB9LFxuICAgIF0sXG4gIH0sXG5dIGFzIEFCUC5GdWxsUm91dGVbXTtcbiJdfQ==