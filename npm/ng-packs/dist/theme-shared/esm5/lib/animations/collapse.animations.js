/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { trigger, state, style, transition, animate } from '@angular/animations';
/** @type {?} */
export var collapse = trigger('collapse', [
    state('open', style({
        height: '*',
        overflow: 'hidden',
    })),
    state('close', style({
        height: '0px',
        overflow: 'hidden',
    })),
    transition("open <=> close", animate('{{duration}}ms'), { params: { duration: '350' } }),
]);
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29sbGFwc2UuYW5pbWF0aW9ucy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2FuaW1hdGlvbnMvY29sbGFwc2UuYW5pbWF0aW9ucy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLE9BQU8sRUFBRSxLQUFLLEVBQUUsS0FBSyxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQzs7QUFFakYsTUFBTSxLQUFPLFFBQVEsR0FBRyxPQUFPLENBQUMsVUFBVSxFQUFFO0lBQzFDLEtBQUssQ0FDSCxNQUFNLEVBQ04sS0FBSyxDQUFDO1FBQ0osTUFBTSxFQUFFLEdBQUc7UUFDWCxRQUFRLEVBQUUsUUFBUTtLQUNuQixDQUFDLENBQ0g7SUFDRCxLQUFLLENBQ0gsT0FBTyxFQUNQLEtBQUssQ0FBQztRQUNKLE1BQU0sRUFBRSxLQUFLO1FBQ2IsUUFBUSxFQUFFLFFBQVE7S0FDbkIsQ0FBQyxDQUNIO0lBQ0QsVUFBVSxDQUFDLGdCQUFnQixFQUFFLE9BQU8sQ0FBQyxnQkFBZ0IsQ0FBQyxFQUFFLEVBQUUsTUFBTSxFQUFFLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxFQUFFLENBQUM7Q0FDekYsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IHRyaWdnZXIsIHN0YXRlLCBzdHlsZSwgdHJhbnNpdGlvbiwgYW5pbWF0ZSB9IGZyb20gJ0Bhbmd1bGFyL2FuaW1hdGlvbnMnO1xuXG5leHBvcnQgY29uc3QgY29sbGFwc2UgPSB0cmlnZ2VyKCdjb2xsYXBzZScsIFtcbiAgc3RhdGUoXG4gICAgJ29wZW4nLFxuICAgIHN0eWxlKHtcbiAgICAgIGhlaWdodDogJyonLFxuICAgICAgb3ZlcmZsb3c6ICdoaWRkZW4nLFxuICAgIH0pLFxuICApLFxuICBzdGF0ZShcbiAgICAnY2xvc2UnLFxuICAgIHN0eWxlKHtcbiAgICAgIGhlaWdodDogJzBweCcsXG4gICAgICBvdmVyZmxvdzogJ2hpZGRlbicsXG4gICAgfSksXG4gICksXG4gIHRyYW5zaXRpb24oYG9wZW4gPD0+IGNsb3NlYCwgYW5pbWF0ZSgne3tkdXJhdGlvbn19bXMnKSwgeyBwYXJhbXM6IHsgZHVyYXRpb246ICczNTAnIH0gfSksXG5dKTtcbiJdfQ==