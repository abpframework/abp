import { CoreModule } from '@abp/ng.core';
import { Component } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { NgxsModule } from '@ngxs/store';
import { ConfirmationService } from '../services/confirmation.service';
import { ThemeSharedModule } from '../theme-shared.module';

@Component({
  selector: 'abp-dummy',
  template: `
    <abp-confirmation></abp-confirmation>
  `,
})
class DummyComponent {
  constructor(public confirmation: ConfirmationService) {}
}

describe('ConfirmationService', () => {
  let spectator: Spectator<DummyComponent>;
  let service: ConfirmationService;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    imports: [CoreModule, ThemeSharedModule.forRoot(), NgxsModule.forRoot(), RouterTestingModule],
  });

  beforeEach(() => {
    spectator = createComponent();
    service = spectator.get(ConfirmationService);
  });
});
