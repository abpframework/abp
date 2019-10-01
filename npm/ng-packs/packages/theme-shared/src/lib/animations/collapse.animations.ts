import { animate, animation, trigger, state, style, transition, useAnimation } from '@angular/animations';

export const collapseY = animation(
  [
    style({ height: '*', overflow: 'hidden', 'box-sizing': 'border-box' }),
    animate('{{ time }} {{ easing }}', style({ height: '0', padding: '0px' }))
  ],
  { params: { time: '350ms', easing: 'ease' } }
);

export const collapseX = animation(
  [
    style({ width: '*', overflow: 'hidden', 'box-sizing': 'border-box' }),
    animate('{{ time }} {{ easing }}', style({ width: '0', padding: '0px' }))
  ],
  { params: { time: '350ms', easing: 'ease' } }
);

export const expandY = animation(
  [
    style({ height: '0', overflow: 'hidden', 'box-sizing': 'border-box' }),
    animate('{{ time }} {{ easing }}', style({ height: '*', padding: '*' }))
  ],
  { params: { time: '350ms', easing: 'ease' } }
);

export const expandX = animation(
  [
    style({ width: '0', overflow: 'hidden', 'box-sizing': 'border-box' }),
    animate('{{ time }} {{ easing }}', style({ width: '*', padding: '*' }))
  ],
  { params: { time: '350ms', easing: 'ease' } }
);

export const collapse = trigger('collapse', [
  state('collapsed', style({ height: '0', overflow: 'hidden' })),
  state('expanded', style({ height: '*', overflow: 'hidden' })),
  transition('expanded => collapsed', useAnimation(collapseY)),
  transition('collapsed => expanded', useAnimation(expandY))
]);
