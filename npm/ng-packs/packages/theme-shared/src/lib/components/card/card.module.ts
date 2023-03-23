import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CardBodyComponent } from './card-body.component';
import { CardComponent } from './card.component';
import { CardHeaderComponent } from './card-header.component';
import { CardFooterComponent } from './card-footer.component';
import { CardTitle } from './card-title.directive';
import { CardSubtitle } from './card-subtitle.directive';
import { CardImgTop } from './card-img-top.directive';
import { CardHeader } from './card-header.directive';

const declarationsWithExports = [
  CardComponent,
  CardBodyComponent,
  CardHeaderComponent,
  CardFooterComponent,
  CardTitle,
  CardSubtitle,
  CardImgTop,
  CardHeader,
];

@NgModule({
  declarations: [...declarationsWithExports],
  imports: [CommonModule],
  exports: [...declarationsWithExports],
})
export class CardModule { }
