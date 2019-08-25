/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { animate, state, style, transition, trigger } from '@angular/animations';
/** @type {?} */
export var slideFromBottom = trigger('routeAnimations', [
    state('void', style({ 'margin-top': '20px', opacity: '0' })),
    state('*', style({ 'margin-top': '0px', opacity: '1' })),
    transition(':enter', [animate('0.2s ease-out', style({ opacity: '1', 'margin-top': '0px' }))]),
]);
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2xpZGUuYW5pbWF0aW9uLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvYW5pbWF0aW9ucy9zbGlkZS5hbmltYXRpb24udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxPQUFPLEVBQUUsS0FBSyxFQUFFLEtBQUssRUFBRSxVQUFVLEVBQUUsT0FBTyxFQUFTLE1BQU0scUJBQXFCLENBQUM7O0FBQ3hGLE1BQU0sS0FBTyxlQUFlLEdBQUcsT0FBTyxDQUFDLGlCQUFpQixFQUFFO0lBQ3hELEtBQUssQ0FBQyxNQUFNLEVBQUUsS0FBSyxDQUFDLEVBQUUsWUFBWSxFQUFFLE1BQU0sRUFBRSxPQUFPLEVBQUUsR0FBRyxFQUFFLENBQUMsQ0FBQztJQUM1RCxLQUFLLENBQUMsR0FBRyxFQUFFLEtBQUssQ0FBQyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsT0FBTyxFQUFFLEdBQUcsRUFBRSxDQUFDLENBQUM7SUFDeEQsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDLE9BQU8sQ0FBQyxlQUFlLEVBQUUsS0FBSyxDQUFDLEVBQUUsT0FBTyxFQUFFLEdBQUcsRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUM7Q0FDL0YsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGFuaW1hdGUsIHN0YXRlLCBzdHlsZSwgdHJhbnNpdGlvbiwgdHJpZ2dlciwgcXVlcnkgfSBmcm9tICdAYW5ndWxhci9hbmltYXRpb25zJztcbmV4cG9ydCBjb25zdCBzbGlkZUZyb21Cb3R0b20gPSB0cmlnZ2VyKCdyb3V0ZUFuaW1hdGlvbnMnLCBbXG4gIHN0YXRlKCd2b2lkJywgc3R5bGUoeyAnbWFyZ2luLXRvcCc6ICcyMHB4Jywgb3BhY2l0eTogJzAnIH0pKSxcbiAgc3RhdGUoJyonLCBzdHlsZSh7ICdtYXJnaW4tdG9wJzogJzBweCcsIG9wYWNpdHk6ICcxJyB9KSksXG4gIHRyYW5zaXRpb24oJzplbnRlcicsIFthbmltYXRlKCcwLjJzIGVhc2Utb3V0Jywgc3R5bGUoeyBvcGFjaXR5OiAnMScsICdtYXJnaW4tdG9wJzogJzBweCcgfSkpXSksXG5dKTtcbiJdfQ==