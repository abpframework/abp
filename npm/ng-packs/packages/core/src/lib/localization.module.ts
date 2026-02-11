import { NgModule } from '@angular/core';
import { LocalizationPipe } from './pipes/localization.pipe';
import { AsyncLocalizationPipe, LazyLocalizationPipe } from './pipes';

/**
 * @deprecated Use `LocalizationPipe`, `AsyncLocalizationPipe` and `LazyLocalizationPipe` directly as a standalone pipe.
 * This module is no longer necessary for using the `LocalizationPipe`, `AsyncLocalizationPipe` and `LazyLocalizationPipe` pipes.
 */

@NgModule({
  exports: [LocalizationPipe, AsyncLocalizationPipe, LazyLocalizationPipe],
  imports: [LocalizationPipe, AsyncLocalizationPipe, LazyLocalizationPipe],
})
export class LocalizationModule {}
