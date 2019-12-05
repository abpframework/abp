/**
 * @fileoverview added by tsickle
 * Generated from: lib/animations/bounce.animations.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { animate, animation, keyframes, style } from '@angular/animations';
/** @type {?} */
export const bounceIn = animation([
    style({ opacity: '0', display: '{{ display }}' }),
    animate('{{ time}} {{ easing }}', keyframes([
        style({ opacity: '0', transform: '{{ transform }} scale(0.0)', offset: 0 }),
        style({ opacity: '0', transform: '{{ transform }} scale(0.8)', offset: 0.5 }),
        style({ opacity: '1', transform: '{{ transform }} scale(1.0)', offset: 1 })
    ]))
], {
    params: {
        time: '350ms',
        easing: 'cubic-bezier(.7,.31,.72,1.47)',
        display: 'block',
        transform: 'translate(-50%, -50%)'
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYm91bmNlLmFuaW1hdGlvbnMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9hbmltYXRpb25zL2JvdW5jZS5hbmltYXRpb25zLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLE9BQU8sRUFBRSxTQUFTLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBRSxNQUFNLHFCQUFxQixDQUFDOztBQUUzRSxNQUFNLE9BQU8sUUFBUSxHQUFHLFNBQVMsQ0FDL0I7SUFDRSxLQUFLLENBQUMsRUFBRSxPQUFPLEVBQUUsR0FBRyxFQUFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsQ0FBQztJQUNqRCxPQUFPLENBQ0wsd0JBQXdCLEVBQ3hCLFNBQVMsQ0FBQztRQUNSLEtBQUssQ0FBQyxFQUFFLE9BQU8sRUFBRSxHQUFHLEVBQUUsU0FBUyxFQUFFLDRCQUE0QixFQUFFLE1BQU0sRUFBRSxDQUFDLEVBQUUsQ0FBQztRQUMzRSxLQUFLLENBQUMsRUFBRSxPQUFPLEVBQUUsR0FBRyxFQUFFLFNBQVMsRUFBRSw0QkFBNEIsRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLENBQUM7UUFDN0UsS0FBSyxDQUFDLEVBQUUsT0FBTyxFQUFFLEdBQUcsRUFBRSxTQUFTLEVBQUUsNEJBQTRCLEVBQUUsTUFBTSxFQUFFLENBQUMsRUFBRSxDQUFDO0tBQzVFLENBQUMsQ0FDSDtDQUNGLEVBQ0Q7SUFDRSxNQUFNLEVBQUU7UUFDTixJQUFJLEVBQUUsT0FBTztRQUNiLE1BQU0sRUFBRSwrQkFBK0I7UUFDdkMsT0FBTyxFQUFFLE9BQU87UUFDaEIsU0FBUyxFQUFFLHVCQUF1QjtLQUNuQztDQUNGLENBQ0YiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBhbmltYXRlLCBhbmltYXRpb24sIGtleWZyYW1lcywgc3R5bGUgfSBmcm9tICdAYW5ndWxhci9hbmltYXRpb25zJztcblxuZXhwb3J0IGNvbnN0IGJvdW5jZUluID0gYW5pbWF0aW9uKFxuICBbXG4gICAgc3R5bGUoeyBvcGFjaXR5OiAnMCcsIGRpc3BsYXk6ICd7eyBkaXNwbGF5IH19JyB9KSxcbiAgICBhbmltYXRlKFxuICAgICAgJ3t7IHRpbWV9fSB7eyBlYXNpbmcgfX0nLFxuICAgICAga2V5ZnJhbWVzKFtcbiAgICAgICAgc3R5bGUoeyBvcGFjaXR5OiAnMCcsIHRyYW5zZm9ybTogJ3t7IHRyYW5zZm9ybSB9fSBzY2FsZSgwLjApJywgb2Zmc2V0OiAwIH0pLFxuICAgICAgICBzdHlsZSh7IG9wYWNpdHk6ICcwJywgdHJhbnNmb3JtOiAne3sgdHJhbnNmb3JtIH19IHNjYWxlKDAuOCknLCBvZmZzZXQ6IDAuNSB9KSxcbiAgICAgICAgc3R5bGUoeyBvcGFjaXR5OiAnMScsIHRyYW5zZm9ybTogJ3t7IHRyYW5zZm9ybSB9fSBzY2FsZSgxLjApJywgb2Zmc2V0OiAxIH0pXG4gICAgICBdKVxuICAgIClcbiAgXSxcbiAge1xuICAgIHBhcmFtczoge1xuICAgICAgdGltZTogJzM1MG1zJyxcbiAgICAgIGVhc2luZzogJ2N1YmljLWJlemllciguNywuMzEsLjcyLDEuNDcpJyxcbiAgICAgIGRpc3BsYXk6ICdibG9jaycsXG4gICAgICB0cmFuc2Zvcm06ICd0cmFuc2xhdGUoLTUwJSwgLTUwJSknXG4gICAgfVxuICB9XG4pO1xuIl19