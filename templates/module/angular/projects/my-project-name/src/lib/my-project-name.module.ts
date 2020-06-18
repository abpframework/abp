import { NgModule } from '@angular/core';
import { MyProjectNameComponent } from './components/my-project-name.component';
import { MyProjectNameRoutingModule } from './my-project-name-routing.module';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CoreModule } from '@abp/ng.core';

@NgModule({
  declarations: [MyProjectNameComponent],
  imports: [CoreModule, ThemeSharedModule, MyProjectNameRoutingModule],
  exports: [MyProjectNameComponent],
})
export class MyProjectNameModule {}
