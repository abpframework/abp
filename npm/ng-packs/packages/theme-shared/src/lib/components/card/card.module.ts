import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CardBodyComponent } from './card-body.component';
import {
  CardComponent,
  CardImgTop,
  CardLink,
  CardSubtitle,
  CardText,
  CardTitle,
  CardHeader,
} from './card.component';
import { CardHeaderComponent } from './card-header.component';
import { CardFooterComponent } from './card-footer.component';

const declarationsWithExports = [
  CardComponent,
  CardBodyComponent,
  CardHeaderComponent,
  CardFooterComponent,
  CardTitle,
  CardSubtitle,
  CardText,
  CardImgTop,
  CardLink,
  CardHeader,
];

@NgModule({
  declarations: [...declarationsWithExports],
  imports: [CommonModule],
  exports: [...declarationsWithExports],
})
export class CardModule {}
