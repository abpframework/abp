/**
 * @fileoverview added by tsickle
 * Generated from: lib/animations/slide.animations.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { animate, style, transition, trigger } from '@angular/animations';
/** @type {?} */
export const slideFromBottom = trigger('slideFromBottom', [
    transition('* <=> *', [
        style({ 'margin-top': '20px', opacity: '0' }),
        animate('0.2s ease-out', style({ opacity: '1', 'margin-top': '0px' })),
    ]),
]);
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2xpZGUuYW5pbWF0aW9ucy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2FuaW1hdGlvbnMvc2xpZGUuYW5pbWF0aW9ucy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxPQUFPLEVBQVMsS0FBSyxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQVMsTUFBTSxxQkFBcUIsQ0FBQzs7QUFDeEYsTUFBTSxPQUFPLGVBQWUsR0FBRyxPQUFPLENBQUMsaUJBQWlCLEVBQUU7SUFDeEQsVUFBVSxDQUFDLFNBQVMsRUFBRTtRQUNwQixLQUFLLENBQUMsRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLE9BQU8sRUFBRSxHQUFHLEVBQUUsQ0FBQztRQUM3QyxPQUFPLENBQUMsZUFBZSxFQUFFLEtBQUssQ0FBQyxFQUFFLE9BQU8sRUFBRSxHQUFHLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxDQUFDLENBQUM7S0FDdkUsQ0FBQztDQUNILENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBhbmltYXRlLCBzdGF0ZSwgc3R5bGUsIHRyYW5zaXRpb24sIHRyaWdnZXIsIHF1ZXJ5IH0gZnJvbSAnQGFuZ3VsYXIvYW5pbWF0aW9ucyc7XG5leHBvcnQgY29uc3Qgc2xpZGVGcm9tQm90dG9tID0gdHJpZ2dlcignc2xpZGVGcm9tQm90dG9tJywgW1xuICB0cmFuc2l0aW9uKCcqIDw9PiAqJywgW1xuICAgIHN0eWxlKHsgJ21hcmdpbi10b3AnOiAnMjBweCcsIG9wYWNpdHk6ICcwJyB9KSxcbiAgICBhbmltYXRlKCcwLjJzIGVhc2Utb3V0Jywgc3R5bGUoeyBvcGFjaXR5OiAnMScsICdtYXJnaW4tdG9wJzogJzBweCcgfSkpLFxuICBdKSxcbl0pO1xuIl19