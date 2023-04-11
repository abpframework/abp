import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CardBodyComponent } from './card-body.component';
import { CardComponent } from './card.component';
import { CardHeaderComponent } from './card-header.component';
import { CardFooterComponent } from './card-footer.component';
import { CardTitleDirective } from './card-title.directive';
import { CardSubtitleDirective } from './card-subtitle.directive';
import { CardImgTopDirective } from './card-img-top.directive';
import { CardHeaderDirective } from './card-header.directive';

const declarationsWithExports = [
  CardComponent,
  CardBodyComponent,
  CardHeaderComponent,
  CardFooterComponent,
  CardTitleDirective,
  CardSubtitleDirective,
  CardImgTopDirective,
  CardHeaderDirective,
];

@NgModule({
  declarations: [...declarationsWithExports],
  imports: [CommonModule],
  exports: [...declarationsWithExports],
})
export class CardModule {}
