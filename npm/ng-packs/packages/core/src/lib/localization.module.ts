import { NgModule } from '@angular/core';
import { LocalizationPipe } from './pipes/localization.pipe';

@NgModule({
  exports: [LocalizationPipe],
  declarations: [LocalizationPipe],
})
export class LocalizationModule {}
