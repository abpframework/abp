import { animate, animation, keyframes, style } from '@angular/animations';

export const bounceIn = animation(
  [
    style({ opacity: '0', display: '{{ display }}' }),
    animate(
      '{{ time}} {{ easing }}',
      keyframes([
        style({ opacity: '0', transform: '{{ transform }} scale(0.0)', offset: 0 }),
        style({ opacity: '0', transform: '{{ transform }} scale(0.8)', offset: 0.5 }),
        style({ opacity: '1', transform: '{{ transform }} scale(1.0)', offset: 1 })
      ])
    )
  ],
  {
    params: {
      time: '350ms',
      easing: 'cubic-bezier(.7,.31,.72,1.47)',
      display: 'block',
      transform: 'translate(-50%, -50%)'
    }
  }
);
