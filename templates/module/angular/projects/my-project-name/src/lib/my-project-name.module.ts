import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { MyProjectNameComponent } from './components/my-project-name.component';
import { MyProjectNameRoutingModule } from './my-project-name-routing.module';

@NgModule({
  declarations: [MyProjectNameComponent],
  imports: [CoreModule, ThemeSharedModule, MyProjectNameRoutingModule],
  exports: [MyProjectNameComponent],
})
export class MyProjectNameModule {
  static forChild(): ModuleWithProviders<MyProjectNameModule> {
    return {
      ngModule: MyProjectNameModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<MyProjectNameModule> {
    return new LazyModuleFactory(MyProjectNameModule.forChild());
  }
}
