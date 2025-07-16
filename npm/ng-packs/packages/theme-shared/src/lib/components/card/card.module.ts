import { NgModule } from '@angular/core';
import { CardBodyComponent } from './card-body.component';
import { CardComponent } from './card.component';
import { CardHeaderComponent } from './card-header.component';
import { CardFooterComponent } from './card-footer.component';
import { CardTitleDirective } from './card-title.directive';
import { CardSubtitleDirective } from './card-subtitle.directive';
import { CardImgTopDirective } from './card-img-top.directive';
import { CardHeaderDirective } from './card-header.directive';

export const CARD_DIRECTIVES = [
  CardTitleDirective,
  CardSubtitleDirective,
  CardImgTopDirective,
  CardHeaderDirective,
];

export const CARD_COMPONENTS = [
  CardComponent,
  CardBodyComponent,
  CardHeaderComponent,
  CardFooterComponent,
];

@NgModule({
  declarations: [],
  imports: [...CARD_COMPONENTS, ...CARD_DIRECTIVES],
  exports: [...CARD_COMPONENTS, ...CARD_DIRECTIVES],
})
export class CardModule {}
