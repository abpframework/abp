/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ReplaySubject } from 'rxjs';
/**
 * @param {?} count
 * @return {?}
 */
export function getRandomBackgroundColor(count) {
    /** @type {?} */
    const colors = [];
    for (let i = 0; i < count; i++) {
        /** @type {?} */
        const r = ((i + 5) * (i + 5) * 474) % 255;
        /** @type {?} */
        const g = ((i + 5) * (i + 5) * 1600) % 255;
        /** @type {?} */
        const b = ((i + 5) * (i + 5) * 84065) % 255;
        colors.push('rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    }
    return colors;
}
/** @type {?} */
export const chartJsLoaded$ = new ReplaySubject(1);
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoid2lkZ2V0LXV0aWxzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvd2lkZ2V0LXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0sTUFBTSxDQUFDOzs7OztBQUVyQyxNQUFNLFVBQVUsd0JBQXdCLENBQUMsS0FBSzs7VUFDdEMsTUFBTSxHQUFHLEVBQUU7SUFFakIsS0FBSyxJQUFJLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFHLEtBQUssRUFBRSxDQUFDLEVBQUUsRUFBRTs7Y0FDeEIsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsR0FBRyxDQUFDLEdBQUcsR0FBRzs7Y0FDbkMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsSUFBSSxDQUFDLEdBQUcsR0FBRzs7Y0FDcEMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsS0FBSyxDQUFDLEdBQUcsR0FBRztRQUMzQyxNQUFNLENBQUMsSUFBSSxDQUFDLE9BQU8sR0FBRyxDQUFDLEdBQUcsSUFBSSxHQUFHLENBQUMsR0FBRyxJQUFJLEdBQUcsQ0FBQyxHQUFHLFFBQVEsQ0FBQyxDQUFDO0tBQzNEO0lBRUQsT0FBTyxNQUFNLENBQUM7QUFDaEIsQ0FBQzs7QUFFRCxNQUFNLE9BQU8sY0FBYyxHQUFHLElBQUksYUFBYSxDQUFDLENBQUMsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFJlcGxheVN1YmplY3QgfSBmcm9tICdyeGpzJztcblxuZXhwb3J0IGZ1bmN0aW9uIGdldFJhbmRvbUJhY2tncm91bmRDb2xvcihjb3VudCkge1xuICBjb25zdCBjb2xvcnMgPSBbXTtcblxuICBmb3IgKGxldCBpID0gMDsgaSA8IGNvdW50OyBpKyspIHtcbiAgICBjb25zdCByID0gKChpICsgNSkgKiAoaSArIDUpICogNDc0KSAlIDI1NTtcbiAgICBjb25zdCBnID0gKChpICsgNSkgKiAoaSArIDUpICogMTYwMCkgJSAyNTU7XG4gICAgY29uc3QgYiA9ICgoaSArIDUpICogKGkgKyA1KSAqIDg0MDY1KSAlIDI1NTtcbiAgICBjb2xvcnMucHVzaCgncmdiYSgnICsgciArICcsICcgKyBnICsgJywgJyArIGIgKyAnLCAwLjcpJyk7XG4gIH1cblxuICByZXR1cm4gY29sb3JzO1xufVxuXG5leHBvcnQgY29uc3QgY2hhcnRKc0xvYWRlZCQgPSBuZXcgUmVwbGF5U3ViamVjdCgxKTtcbiJdfQ==