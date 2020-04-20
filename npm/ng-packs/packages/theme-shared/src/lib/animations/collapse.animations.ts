import {
  animate,
  animation,
  trigger,
  state,
  style,
  transition,
  useAnimation,
} from '@angular/animations';

export const collapseY = animation(
  [
    style({ height: '*', overflow: 'hidden', 'box-sizing': 'border-box' }),
    animate('{{ time }} {{ easing }}', style({ height: '0', padding: '0px' })),
  ],
  { params: { time: '350ms', easing: 'ease' } },
);

export const collapseYWithMargin = animation(
  [
    style({ 'margin-top': '0' }),
    animate('{{ time }} {{ easing }}', style({ 'margin-left': '-100%' })),
  ],
  {
    params: { time: '500ms', easing: 'ease' },
  },
);

export const collapseX = animation(
  [
    style({ width: '*', overflow: 'hidden', 'box-sizing': 'border-box' }),
    animate('{{ time }} {{ easing }}', style({ width: '0', padding: '0px' })),
  ],
  { params: { time: '350ms', easing: 'ease' } },
);

export const expandY = animation(
  [
    style({ height: '0', overflow: 'hidden', 'box-sizing': 'border-box' }),
    animate('{{ time }} {{ easing }}', style({ height: '*', padding: '*' })),
  ],
  { params: { time: '350ms', easing: 'ease' } },
);

export const expandYWithMargin = animation(
  [
    style({ 'margin-top': '-100%' }),
    animate('{{ time }} {{ easing }}', style({ 'margin-top': '0' })),
  ],
  {
    params: { time: '500ms', easing: 'ease' },
  },
);

export const expandX = animation(
  [
    style({ width: '0', overflow: 'hidden', 'box-sizing': 'border-box' }),
    animate('{{ time }} {{ easing }}', style({ width: '*', padding: '*' })),
  ],
  { params: { time: '350ms', easing: 'ease' } },
);

export const collapse = trigger('collapse', [
  state('collapsed', style({ height: '0', overflow: 'hidden' })),
  state('expanded', style({ height: '*', overflow: 'hidden' })),
  transition('expanded => collapsed', useAnimation(collapseY)),
  transition('collapsed => expanded', useAnimation(expandY)),
]);

export const collapseWithMargin = trigger('collapseWithMargin', [
  state('collapsed', style({ 'margin-top': '-100%' })),
  state('expanded', style({ 'margin-top': '0' })),
  transition('expanded => collapsed', useAnimation(collapseYWithMargin), {
    params: { time: '400ms', easing: 'linear' },
  }),
  transition('collapsed => expanded', useAnimation(expandYWithMargin)),
]);

export const collapseLinearWithMargin = trigger('collapseLinearWithMargin', [
  state('collapsed', style({ 'margin-top': '-100vh' })),
  state('expanded', style({ 'margin-top': '0' })),
  transition(
    'expanded => collapsed',
    useAnimation(collapseYWithMargin, { params: { time: '200ms', easing: 'linear' } }),
  ),
  transition(
    'collapsed => expanded',
    useAnimation(expandYWithMargin, { params: { time: '250ms', easing: 'linear' } }),
  ),
]);
