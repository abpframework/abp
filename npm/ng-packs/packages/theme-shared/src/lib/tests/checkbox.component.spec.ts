import { createHostFactory, SpectatorHost } from '@ngneat/spectator/vitest';
import { FormCheckboxComponent } from '../components/checkbox/checkbox.component';
import { setInputSignal } from './utils';

describe('FormCheckboxComponent', () => {
  let spectator: SpectatorHost<FormCheckboxComponent>;

  const createHost = createHostFactory(FormCheckboxComponent);

  beforeEach(
    () => {
      spectator = createHost(
        '<abp-checkbox />',
        {
          detectChanges: false,
          hostProps: { attributes: { autofocus: '', name: 'abp-checkbox' } },
        },
      );
      setInputSignal(spectator.component.checkboxId, 'checkbox-id');
      setInputSignal(spectator.component.checkboxReadonly, true);
      spectator.detectChanges();
    },
  );

  it('should display the input', () => {
    expect(spectator.query('input')).toBeTruthy();
  });

  it('should equal the default classes to form-check-input', () => {
    expect(spectator.query('input')).toHaveClass('form-check-input');
  });

  it('should equal the default type to checkbox', () => {
    expect(spectator.query('input')).toHaveAttribute('type', 'checkbox');
  });

  it('should be readonly when checkboxReadonly is true', () => {
    setInputSignal(spectator.component.checkboxReadonly, true);
    spectator.detectComponentChanges();
    expect(spectator.query('[readonly]')).toBeTruthy();
  });

  it('should not contain readonly when checboxReadonly is false', () => {
    setInputSignal(spectator.component.checkboxReadonly, false);
    spectator.detectComponentChanges();
    expect(spectator.query('[disabled]')).toBeFalsy();
  });

});

