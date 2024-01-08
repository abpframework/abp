import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { FormInputComponent } from '../components/form-input/form-input.component';


describe('FormInputComponent', () => {
  let spectator: SpectatorHost<FormInputComponent>;

  const createHost = createHostFactory(FormInputComponent);

  beforeEach(
    () =>
      (spectator = createHost(
        '<abp-form-input></abp-form-input>',
        {
          hostProps: { attributes: { autofocus: '', name: 'abp-form-input' } },
        },
      )),
  );

  it('should display the input', () => {
    expect(spectator.query('input')).toBeTruthy();
  });

  it('should equal the default classes to form-control', () => {
    expect(spectator.query('input')).toHaveClass('form-control');
  });

  it('should equal the default type to text', () => {
    expect(spectator.query('input')).toHaveAttribute('type', 'text');
  });

  it('should be readonly when inputReadonly is true', () => {
    spectator.component.inputReadonly = true;
    spectator.detectComponentChanges();
    expect(spectator.query('[readonly]')).toBeTruthy();
  });

  it('should not contain readonly when inputReadonly is false', () => {
    spectator.component.inputReadonly = false;
    spectator.detectComponentChanges();
    expect(spectator.query('[disabled]')).toBeFalsy();
  });

});

