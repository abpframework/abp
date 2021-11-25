import { CONTENT_STRATEGY, DomInsertionService } from '@abp/ng.core';
import { inject, InjectionToken } from '@angular/core';
import styles from '../constants/styles';

export const THEME_SHARED_APPEND_CONTENT = new InjectionToken<void>('THEME_SHARED_APPEND_CONTENT', {
  providedIn: 'root',
  factory: () => {
    const domInsertion: DomInsertionService = inject(DomInsertionService);

    domInsertion.insertContent(CONTENT_STRATEGY.AppendStyleToHead(styles));
  },
});
