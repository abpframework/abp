/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { animate, animation, keyframes, style } from '@angular/animations';
/** @type {?} */
export const bounceIn = animation(
  [
    style({ opacity: '0', display: '{{ display }}' }),
    animate(
      '{{ time}} {{ easing }}',
      keyframes([
        style({ opacity: '0', transform: '{{ transform }} scale(0.0)', offset: 0 }),
        style({ opacity: '0', transform: '{{ transform }} scale(0.8)', offset: 0.5 }),
        style({ opacity: '1', transform: '{{ transform }} scale(1.0)', offset: 1 }),
      ]),
    ),
  ],
  {
    params: {
      time: '350ms',
      easing: 'cubic-bezier(.7,.31,.72,1.47)',
      display: 'block',
      transform: 'translate(-50%, -50%)',
    },
  },
);
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYm91bmNlLmFuaW1hdGlvbnMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9hbmltYXRpb25zL2JvdW5jZS5hbmltYXRpb25zLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsT0FBTyxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFFLE1BQU0scUJBQXFCLENBQUM7O0FBRTNFLE1BQU0sT0FBTyxRQUFRLEdBQUcsU0FBUyxDQUMvQjtJQUNFLEtBQUssQ0FBQyxFQUFFLE9BQU8sRUFBRSxHQUFHLEVBQUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxDQUFDO0lBQ2pELE9BQU8sQ0FDTCx3QkFBd0IsRUFDeEIsU0FBUyxDQUFDO1FBQ1IsS0FBSyxDQUFDLEVBQUUsT0FBTyxFQUFFLEdBQUcsRUFBRSxTQUFTLEVBQUUsNEJBQTRCLEVBQUUsTUFBTSxFQUFFLENBQUMsRUFBRSxDQUFDO1FBQzNFLEtBQUssQ0FBQyxFQUFFLE9BQU8sRUFBRSxHQUFHLEVBQUUsU0FBUyxFQUFFLDRCQUE0QixFQUFFLE1BQU0sRUFBRSxHQUFHLEVBQUUsQ0FBQztRQUM3RSxLQUFLLENBQUMsRUFBRSxPQUFPLEVBQUUsR0FBRyxFQUFFLFNBQVMsRUFBRSw0QkFBNEIsRUFBRSxNQUFNLEVBQUUsQ0FBQyxFQUFFLENBQUM7S0FDNUUsQ0FBQyxDQUNIO0NBQ0YsRUFDRDtJQUNFLE1BQU0sRUFBRTtRQUNOLElBQUksRUFBRSxPQUFPO1FBQ2IsTUFBTSxFQUFFLCtCQUErQjtRQUN2QyxPQUFPLEVBQUUsT0FBTztRQUNoQixTQUFTLEVBQUUsdUJBQXVCO0tBQ25DO0NBQ0YsQ0FDRiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGFuaW1hdGUsIGFuaW1hdGlvbiwga2V5ZnJhbWVzLCBzdHlsZSB9IGZyb20gJ0Bhbmd1bGFyL2FuaW1hdGlvbnMnO1xuXG5leHBvcnQgY29uc3QgYm91bmNlSW4gPSBhbmltYXRpb24oXG4gIFtcbiAgICBzdHlsZSh7IG9wYWNpdHk6ICcwJywgZGlzcGxheTogJ3t7IGRpc3BsYXkgfX0nIH0pLFxuICAgIGFuaW1hdGUoXG4gICAgICAne3sgdGltZX19IHt7IGVhc2luZyB9fScsXG4gICAgICBrZXlmcmFtZXMoW1xuICAgICAgICBzdHlsZSh7IG9wYWNpdHk6ICcwJywgdHJhbnNmb3JtOiAne3sgdHJhbnNmb3JtIH19IHNjYWxlKDAuMCknLCBvZmZzZXQ6IDAgfSksXG4gICAgICAgIHN0eWxlKHsgb3BhY2l0eTogJzAnLCB0cmFuc2Zvcm06ICd7eyB0cmFuc2Zvcm0gfX0gc2NhbGUoMC44KScsIG9mZnNldDogMC41IH0pLFxuICAgICAgICBzdHlsZSh7IG9wYWNpdHk6ICcxJywgdHJhbnNmb3JtOiAne3sgdHJhbnNmb3JtIH19IHNjYWxlKDEuMCknLCBvZmZzZXQ6IDEgfSlcbiAgICAgIF0pXG4gICAgKVxuICBdLFxuICB7XG4gICAgcGFyYW1zOiB7XG4gICAgICB0aW1lOiAnMzUwbXMnLFxuICAgICAgZWFzaW5nOiAnY3ViaWMtYmV6aWVyKC43LC4zMSwuNzIsMS40NyknLFxuICAgICAgZGlzcGxheTogJ2Jsb2NrJyxcbiAgICAgIHRyYW5zZm9ybTogJ3RyYW5zbGF0ZSgtNTAlLCAtNTAlKSdcbiAgICB9XG4gIH1cbik7XG4iXX0=
