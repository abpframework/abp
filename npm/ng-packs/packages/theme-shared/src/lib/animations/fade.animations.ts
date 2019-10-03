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

export const fadeInDown = animation(
  [
    style({ opacity: '0', display: '{{ display }}', transform: '{{ transform }} translateY(-20px)' }),
    animate('{{ time }} {{ easing }}', style({ opacity: '1', transform: '{{ transform }} translateY(0)' }))
  ],
  { params: { time: '350ms', easing: 'ease', display: 'block', transform: 'translate(-50%, -50%)' } }
);

export const fadeInUp = animation(
  [
    style({ opacity: '0', display: '{{ display }}', transform: '{{ transform }} translateY(20px)' }),
    animate('{{ time }} {{ easing }}', style({ opacity: '1', transform: '{{ transform }} translateY(0)' }))
  ],
  { params: { time: '350ms', easing: 'ease', display: 'block', transform: 'translate(-50%, -50%)' } }
);

export const fadeInLeft = animation(
  [
    style({ opacity: '0', display: '{{ display }}', transform: '{{ transform }} translateX(20px)' }),
    animate('{{ time }} {{ easing }}', style({ opacity: '1', transform: '{{ transform }} translateX(0)' }))
  ],
  { params: { time: '350ms', easing: 'ease', display: 'block', transform: 'translate(-50%, -50%)' } }
);

export const fadeInRight = animation(
  [
    style({ opacity: '0', display: '{{ display }}', transform: '{{ transform }} translateX(-20px)' }),
    animate('{{ time }} {{ easing }}', style({ opacity: '1', transform: '{{ transform }} translateX(0)' }))
  ],
  { params: { time: '350ms', easing: 'ease', display: 'block', transform: 'translate(-50%, -50%)' } }
);

export const fadeOutDown = animation(
  [
    style({ opacity: '1', display: '{{ display }}', transform: '{{ transform }} translateY(0)' }),
    animate('{{ time }} {{ easing }}', style({ opacity: '0', transform: '{{ transform }} translateY(20px)' }))
  ],
  { params: { time: '350ms', easing: 'ease', display: 'block', transform: 'translate(-50%, -50%)' } }
);

export const fadeOutUp = animation(
  [
    style({ opacity: '1', display: '{{ display }}', transform: '{{ transform }} translateY(0)' }),
    animate('{{ time }} {{ easing }}', style({ opacity: '0', transform: '{{ transform }} translateY(-20px)' }))
  ],
  { params: { time: '350ms', easing: 'ease', display: 'block', transform: 'translate(-50%, -50%)' } }
);

export const fadeOutLeft = animation(
  [
    style({ opacity: '1', display: '{{ display }}', transform: '{{ transform }} translateX(0)' }),
    animate('{{ time }} {{ easing }}', style({ opacity: '0', transform: '{{ transform }} translateX(20px)' }))
  ],
  { params: { time: '350ms', easing: 'ease', display: 'block', transform: 'translate(-50%, -50%)' } }
);

export const fadeOutRight = animation(
  [
    style({ opacity: '1', display: '{{ display }}', transform: '{{ transform }} translateX(0)' }),
    animate('{{ time }} {{ easing }}', style({ opacity: '0', transform: '{{ transform }} translateX(-20px)' }))
  ],
  { params: { time: '350ms', easing: 'ease', display: 'block', transform: 'translate(-50%, -50%)' } }
);
