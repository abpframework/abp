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
    var colors = [];
    for (var i = 0; i < count; i++) {
        /** @type {?} */
        var r = ((i + 5) * (i + 5) * 474) % 255;
        /** @type {?} */
        var g = ((i + 5) * (i + 5) * 1600) % 255;
        /** @type {?} */
        var b = ((i + 5) * (i + 5) * 84065) % 255;
        colors.push('rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    }
    return colors;
}
/** @type {?} */
export var chartJsLoaded$ = new ReplaySubject(1);
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoid2lkZ2V0LXV0aWxzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvd2lkZ2V0LXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0sTUFBTSxDQUFDOzs7OztBQUVyQyxNQUFNLFVBQVUsd0JBQXdCLENBQUMsS0FBSzs7UUFDdEMsTUFBTSxHQUFHLEVBQUU7SUFFakIsS0FBSyxJQUFJLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFHLEtBQUssRUFBRSxDQUFDLEVBQUUsRUFBRTs7WUFDeEIsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsR0FBRyxDQUFDLEdBQUcsR0FBRzs7WUFDbkMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsSUFBSSxDQUFDLEdBQUcsR0FBRzs7WUFDcEMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQUcsS0FBSyxDQUFDLEdBQUcsR0FBRztRQUMzQyxNQUFNLENBQUMsSUFBSSxDQUFDLE9BQU8sR0FBRyxDQUFDLEdBQUcsSUFBSSxHQUFHLENBQUMsR0FBRyxJQUFJLEdBQUcsQ0FBQyxHQUFHLFFBQVEsQ0FBQyxDQUFDO0tBQzNEO0lBRUQsT0FBTyxNQUFNLENBQUM7QUFDaEIsQ0FBQzs7QUFFRCxNQUFNLEtBQU8sY0FBYyxHQUFHLElBQUksYUFBYSxDQUFDLENBQUMsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFJlcGxheVN1YmplY3QgfSBmcm9tICdyeGpzJztcblxuZXhwb3J0IGZ1bmN0aW9uIGdldFJhbmRvbUJhY2tncm91bmRDb2xvcihjb3VudCkge1xuICBjb25zdCBjb2xvcnMgPSBbXTtcblxuICBmb3IgKGxldCBpID0gMDsgaSA8IGNvdW50OyBpKyspIHtcbiAgICBjb25zdCByID0gKChpICsgNSkgKiAoaSArIDUpICogNDc0KSAlIDI1NTtcbiAgICBjb25zdCBnID0gKChpICsgNSkgKiAoaSArIDUpICogMTYwMCkgJSAyNTU7XG4gICAgY29uc3QgYiA9ICgoaSArIDUpICogKGkgKyA1KSAqIDg0MDY1KSAlIDI1NTtcbiAgICBjb2xvcnMucHVzaCgncmdiYSgnICsgciArICcsICcgKyBnICsgJywgJyArIGIgKyAnLCAwLjcpJyk7XG4gIH1cblxuICByZXR1cm4gY29sb3JzO1xufVxuXG5leHBvcnQgY29uc3QgY2hhcnRKc0xvYWRlZCQgPSBuZXcgUmVwbGF5U3ViamVjdCgxKTtcbiJdfQ==