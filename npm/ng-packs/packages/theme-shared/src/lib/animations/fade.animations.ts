import { animate, animation, style } from '@angular/animations';

export const fadeIn = animation(
  [style({ opacity: '0', display: '{{ display }}' }), animate('{{ time}} {{ easing }}', style({ opacity: '1' }))],
  { params: { time: '350ms', easing: 'ease', display: 'block' } }
);

export const fadeOut = animation(
  [
    style({ opacity: '1' }),
    animate('{{ time}} {{ easing }}', style({ opacity: '0' })),
    style({ opacity: '0', display: 'none' })
  ],
  { params: { time: '350ms', easing: 'ease' } }
);
