/**
 * @fileoverview added by tsickle
 * Generated from: lib/animations/bounce.animations.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { animate, animation, keyframes, style } from '@angular/animations';
/** @type {?} */
export var bounceIn = animation([
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYm91bmNlLmFuaW1hdGlvbnMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9hbmltYXRpb25zL2JvdW5jZS5hbmltYXRpb25zLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLE9BQU8sRUFBRSxTQUFTLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBRSxNQUFNLHFCQUFxQixDQUFDOztBQUUzRSxNQUFNLEtBQU8sUUFBUSxHQUFHLFNBQVMsQ0FDL0I7SUFDRSxLQUFLLENBQUMsRUFBRSxPQUFPLEVBQUUsR0FBRyxFQUFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsQ0FBQztJQUNqRCxPQUFPLENBQ0wsd0JBQXdCLEVBQ3hCLFNBQVMsQ0FBQztRQUNSLEtBQUssQ0FBQyxFQUFFLE9BQU8sRUFBRSxHQUFHLEVBQUUsU0FBUyxFQUFFLDRCQUE0QixFQUFFLE1BQU0sRUFBRSxDQUFDLEVBQUUsQ0FBQztRQUMzRSxLQUFLLENBQUMsRUFBRSxPQUFPLEVBQUUsR0FBRyxFQUFFLFNBQVMsRUFBRSw0QkFBNEIsRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLENBQUM7UUFDN0UsS0FBSyxDQUFDLEVBQUUsT0FBTyxFQUFFLEdBQUcsRUFBRSxTQUFTLEVBQUUsNEJBQTRCLEVBQUUsTUFBTSxFQUFFLENBQUMsRUFBRSxDQUFDO0tBQzVFLENBQUMsQ0FDSDtDQUNGLEVBQ0Q7SUFDRSxNQUFNLEVBQUU7UUFDTixJQUFJLEVBQUUsT0FBTztRQUNiLE1BQU0sRUFBRSwrQkFBK0I7UUFDdkMsT0FBTyxFQUFFLE9BQU87UUFDaEIsU0FBUyxFQUFFLHVCQUF1QjtLQUNuQztDQUNGLENBQ0YiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBhbmltYXRlLCBhbmltYXRpb24sIGtleWZyYW1lcywgc3R5bGUgfSBmcm9tICdAYW5ndWxhci9hbmltYXRpb25zJztcclxuXHJcbmV4cG9ydCBjb25zdCBib3VuY2VJbiA9IGFuaW1hdGlvbihcclxuICBbXHJcbiAgICBzdHlsZSh7IG9wYWNpdHk6ICcwJywgZGlzcGxheTogJ3t7IGRpc3BsYXkgfX0nIH0pLFxyXG4gICAgYW5pbWF0ZShcclxuICAgICAgJ3t7IHRpbWV9fSB7eyBlYXNpbmcgfX0nLFxyXG4gICAgICBrZXlmcmFtZXMoW1xyXG4gICAgICAgIHN0eWxlKHsgb3BhY2l0eTogJzAnLCB0cmFuc2Zvcm06ICd7eyB0cmFuc2Zvcm0gfX0gc2NhbGUoMC4wKScsIG9mZnNldDogMCB9KSxcclxuICAgICAgICBzdHlsZSh7IG9wYWNpdHk6ICcwJywgdHJhbnNmb3JtOiAne3sgdHJhbnNmb3JtIH19IHNjYWxlKDAuOCknLCBvZmZzZXQ6IDAuNSB9KSxcclxuICAgICAgICBzdHlsZSh7IG9wYWNpdHk6ICcxJywgdHJhbnNmb3JtOiAne3sgdHJhbnNmb3JtIH19IHNjYWxlKDEuMCknLCBvZmZzZXQ6IDEgfSlcclxuICAgICAgXSlcclxuICAgIClcclxuICBdLFxyXG4gIHtcclxuICAgIHBhcmFtczoge1xyXG4gICAgICB0aW1lOiAnMzUwbXMnLFxyXG4gICAgICBlYXNpbmc6ICdjdWJpYy1iZXppZXIoLjcsLjMxLC43MiwxLjQ3KScsXHJcbiAgICAgIGRpc3BsYXk6ICdibG9jaycsXHJcbiAgICAgIHRyYW5zZm9ybTogJ3RyYW5zbGF0ZSgtNTAlLCAtNTAlKSdcclxuICAgIH1cclxuICB9XHJcbik7XHJcbiJdfQ==