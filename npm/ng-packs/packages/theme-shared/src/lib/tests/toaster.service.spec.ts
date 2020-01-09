import { CoreModule } from '@abp/ng.core';
import { Component } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { NgxsModule } from '@ngxs/store';
import { ToasterService } from '../services/toaster.service';
import { ThemeSharedModule } from '../theme-shared.module';

@Component({
  selector: 'abp-dummy',
  template: `
    <abp-toast></abp-toast>
  `,
})
class DummyComponent {
  constructor(public toaster: ToasterService) {}
}

describe('ToasterService', () => {
  let spectator: Spectator<DummyComponent>;
  let service: ToasterService;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    imports: [CoreModule, ThemeSharedModule.forRoot(), NgxsModule.forRoot(), RouterTestingModule],
  });

  beforeEach(() => {
    spectator = createComponent();
    service = spectator.get(ToasterService);
  });
});
