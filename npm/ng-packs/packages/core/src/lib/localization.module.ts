import { NgModule } from '@angular/core';
import { LocalizationPipe } from './pipes';

@NgModule({
  providers: [LocalizationPipe],
  exports: [LocalizationPipe],
  declarations: [LocalizationPipe],
})
export class LocalizationModule {}
