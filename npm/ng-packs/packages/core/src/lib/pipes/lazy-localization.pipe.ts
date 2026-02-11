import { Injectable, Pipe } from '@angular/core';
import { AsyncLocalizationPipe } from './async-localization.pipe';

/**
 * @deprecated Use `AsyncLocalizationPipe` instead. This pipe will be removed in a future version.
 * LazyLocalizationPipe has been renamed to AsyncLocalizationPipe to better express its async nature.
 */
@Injectable()
@Pipe({
  name: 'abpLazyLocalization',
})
export class LazyLocalizationPipe extends AsyncLocalizationPipe {}
