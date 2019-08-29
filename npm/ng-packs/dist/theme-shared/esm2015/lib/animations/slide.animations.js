/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { animate, state, style, transition, trigger } from '@angular/animations';
/** @type {?} */
export const slideFromBottom = trigger('routeAnimations', [
    state('void', style({ 'margin-top': '20px', opacity: '0' })),
    state('*', style({ 'margin-top': '0px', opacity: '1' })),
    transition(':enter', [animate('0.2s ease-out', style({ opacity: '1', 'margin-top': '0px' }))]),
]);
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2xpZGUuYW5pbWF0aW9ucy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2FuaW1hdGlvbnMvc2xpZGUuYW5pbWF0aW9ucy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLE9BQU8sRUFBRSxLQUFLLEVBQUUsS0FBSyxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQVMsTUFBTSxxQkFBcUIsQ0FBQzs7QUFDeEYsTUFBTSxPQUFPLGVBQWUsR0FBRyxPQUFPLENBQUMsaUJBQWlCLEVBQUU7SUFDeEQsS0FBSyxDQUFDLE1BQU0sRUFBRSxLQUFLLENBQUMsRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLE9BQU8sRUFBRSxHQUFHLEVBQUUsQ0FBQyxDQUFDO0lBQzVELEtBQUssQ0FBQyxHQUFHLEVBQUUsS0FBSyxDQUFDLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxPQUFPLEVBQUUsR0FBRyxFQUFFLENBQUMsQ0FBQztJQUN4RCxVQUFVLENBQUMsUUFBUSxFQUFFLENBQUMsT0FBTyxDQUFDLGVBQWUsRUFBRSxLQUFLLENBQUMsRUFBRSxPQUFPLEVBQUUsR0FBRyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQztDQUMvRixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgYW5pbWF0ZSwgc3RhdGUsIHN0eWxlLCB0cmFuc2l0aW9uLCB0cmlnZ2VyLCBxdWVyeSB9IGZyb20gJ0Bhbmd1bGFyL2FuaW1hdGlvbnMnO1xuZXhwb3J0IGNvbnN0IHNsaWRlRnJvbUJvdHRvbSA9IHRyaWdnZXIoJ3JvdXRlQW5pbWF0aW9ucycsIFtcbiAgc3RhdGUoJ3ZvaWQnLCBzdHlsZSh7ICdtYXJnaW4tdG9wJzogJzIwcHgnLCBvcGFjaXR5OiAnMCcgfSkpLFxuICBzdGF0ZSgnKicsIHN0eWxlKHsgJ21hcmdpbi10b3AnOiAnMHB4Jywgb3BhY2l0eTogJzEnIH0pKSxcbiAgdHJhbnNpdGlvbignOmVudGVyJywgW2FuaW1hdGUoJzAuMnMgZWFzZS1vdXQnLCBzdHlsZSh7IG9wYWNpdHk6ICcxJywgJ21hcmdpbi10b3AnOiAnMHB4JyB9KSldKSxcbl0pO1xuIl19