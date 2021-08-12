import { animate, query, style, transition, trigger } from '@angular/animations';

export const toastInOut = trigger('toastInOut', [
  transition('* <=> *', [
    query(
      ':enter',
      [
        style({ opacity: 0, transform: 'translateY(20px)' }),
        animate('350ms ease', style({ opacity: 1, transform: 'translateY(0)' })),
      ],
      { optional: true },
    ),
    query(':leave', animate('450ms ease', style({ opacity: 0 })), {
      optional: true,
    }),
  ]),
]);
