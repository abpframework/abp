/**
 * @fileoverview added by tsickle
 * Generated from: lib/utils/widget-utils.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoid2lkZ2V0LXV0aWxzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvd2lkZ2V0LXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLE1BQU0sQ0FBQzs7Ozs7QUFFckMsTUFBTSxVQUFVLHdCQUF3QixDQUFDLEtBQUs7O1FBQ3RDLE1BQU0sR0FBRyxFQUFFO0lBRWpCLEtBQUssSUFBSSxDQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxLQUFLLEVBQUUsQ0FBQyxFQUFFLEVBQUU7O1lBQ3hCLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLEdBQUcsQ0FBQyxHQUFHLEdBQUc7O1lBQ25DLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLElBQUksQ0FBQyxHQUFHLEdBQUc7O1lBQ3BDLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUFHLEtBQUssQ0FBQyxHQUFHLEdBQUc7UUFDM0MsTUFBTSxDQUFDLElBQUksQ0FBQyxPQUFPLEdBQUcsQ0FBQyxHQUFHLElBQUksR0FBRyxDQUFDLEdBQUcsSUFBSSxHQUFHLENBQUMsR0FBRyxRQUFRLENBQUMsQ0FBQztLQUMzRDtJQUVELE9BQU8sTUFBTSxDQUFDO0FBQ2hCLENBQUM7O0FBRUQsTUFBTSxLQUFPLGNBQWMsR0FBRyxJQUFJLGFBQWEsQ0FBQyxDQUFDLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBSZXBsYXlTdWJqZWN0IH0gZnJvbSAncnhqcyc7XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gZ2V0UmFuZG9tQmFja2dyb3VuZENvbG9yKGNvdW50KSB7XHJcbiAgY29uc3QgY29sb3JzID0gW107XHJcblxyXG4gIGZvciAobGV0IGkgPSAwOyBpIDwgY291bnQ7IGkrKykge1xyXG4gICAgY29uc3QgciA9ICgoaSArIDUpICogKGkgKyA1KSAqIDQ3NCkgJSAyNTU7XHJcbiAgICBjb25zdCBnID0gKChpICsgNSkgKiAoaSArIDUpICogMTYwMCkgJSAyNTU7XHJcbiAgICBjb25zdCBiID0gKChpICsgNSkgKiAoaSArIDUpICogODQwNjUpICUgMjU1O1xyXG4gICAgY29sb3JzLnB1c2goJ3JnYmEoJyArIHIgKyAnLCAnICsgZyArICcsICcgKyBiICsgJywgMC43KScpO1xyXG4gIH1cclxuXHJcbiAgcmV0dXJuIGNvbG9ycztcclxufVxyXG5cclxuZXhwb3J0IGNvbnN0IGNoYXJ0SnNMb2FkZWQkID0gbmV3IFJlcGxheVN1YmplY3QoMSk7XHJcbiJdfQ==