import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CardBodyComponent } from './card-body.component';
import { CardTitleComponent } from './card-title.component';
import { CardComponent } from './card.component';

const declarationsWithExports = [CardComponent, CardBodyComponent, CardTitleComponent];

@NgModule({
  declarations: [...declarationsWithExports],
  imports: [CommonModule],
  exports: [...declarationsWithExports],
})
export class CardModule {}
