import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { AbpFormFieldErrorComponent } from './abp-form-field-error.component';

describe('AbpFormFieldErrorComponent', () => {
  let spectator: SpectatorHost<AbpFormFieldErrorComponent>;
  const createHost = createHostFactory({
    component: AbpFormFieldErrorComponent,
  });

  it('should create', () => {
    spectator = createHost('<abp-form-field-error>Test error</abp-form-field-error>');
    expect(spectator.component).toBeTruthy();
  });

  it('should render content', () => {
    spectator = createHost('<abp-form-field-error>Test error</abp-form-field-error>');
    expect(spectator.element).toHaveText('Test error');
  });

  it('should have correct CSS classes', () => {
    spectator = createHost('<abp-form-field-error>Test error</abp-form-field-error>');
    const div = spectator.query('div');
    expect(div).toHaveClass('invalid-feedback');
    expect(div).toHaveClass('d-block');
  });
});
