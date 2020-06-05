import { NgModule } from '@angular/core';
import { MyProjectNameModule } from '../../../projects/my-project-name/src/public-api';

@NgModule({
  imports: [MyProjectNameModule],
})
export class MyProjectNameWrapperModule {}
