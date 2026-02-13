import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { AbpFormFieldHintComponent } from './abp-form-field-hint.component';

describe('AbpFormFieldHintComponent', () => {
  let spectator: SpectatorHost<AbpFormFieldHintComponent>;
  const createHost = createHostFactory({
    component: AbpFormFieldHintComponent,
  });

  it('should create', () => {
    spectator = createHost('<abp-form-field-hint>Test hint</abp-form-field-hint>');
    expect(spectator.component).toBeTruthy();
  });

  it('should render content', () => {
    spectator = createHost('<abp-form-field-hint>Test hint</abp-form-field-hint>');
    expect(spectator.element).toHaveText('Test hint');
  });

  it('should have correct CSS classes', () => {
    spectator = createHost('<abp-form-field-hint>Test hint</abp-form-field-hint>');
    const small = spectator.query('small');
    expect(small).toHaveClass('form-text');
    expect(small).toHaveClass('text-muted');
  });
});
