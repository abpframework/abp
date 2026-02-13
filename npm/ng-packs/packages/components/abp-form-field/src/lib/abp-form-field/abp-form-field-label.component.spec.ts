import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { AbpFormFieldLabelComponent } from './abp-form-field-label.component';

describe('AbpFormFieldLabelComponent', () => {
  let spectator: SpectatorHost<AbpFormFieldLabelComponent>;
  const createHost = createHostFactory({
    component: AbpFormFieldLabelComponent,
  });

  it('should create', () => {
    spectator = createHost('<abp-form-field-label>Test Label</abp-form-field-label>');
    expect(spectator.component).toBeTruthy();
  });

  it('should render content', () => {
    spectator = createHost('<abp-form-field-label>Test Label</abp-form-field-label>');
    expect(spectator.element).toHaveText('Test Label');
  });

  it('should have for input property', () => {
    spectator = createHost(
      '<abp-form-field-label for="test-input">Test Label</abp-form-field-label>'
    );
    expect(spectator.component.for()).toBe('test-input');
  });

  it('should have empty for by default', () => {
    spectator = createHost('<abp-form-field-label>Test Label</abp-form-field-label>');
    expect(spectator.component.for()).toBe('');
  });
});
