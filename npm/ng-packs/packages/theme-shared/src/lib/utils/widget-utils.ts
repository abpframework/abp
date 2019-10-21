import { ReplaySubject } from 'rxjs';

export function getRandomBackgroundColor(count) {
  const colors = [];

  for (let i = 0; i < count; i++) {
    const r = ((i + 5) * (i + 5) * 474) % 255;
    const g = ((i + 5) * (i + 5) * 1600) % 255;
    const b = ((i + 5) * (i + 5) * 84065) % 255;
    colors.push('rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
  }

  return colors;
}

export const chartJsLoaded$ = new ReplaySubject(1);
