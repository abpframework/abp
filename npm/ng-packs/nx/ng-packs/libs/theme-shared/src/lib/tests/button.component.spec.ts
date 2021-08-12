import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { ButtonComponent } from '../components';

describe('ButtonComponent', () => {
  let spectator: SpectatorHost<ButtonComponent>;

  const createHost = createHostFactory(ButtonComponent);

  beforeEach(
    () =>
      (spectator = createHost('<abp-button iconClass="fa fa-check" [attributes]="attributes">Button</abp-button>', {
        hostProps: { attributes: { autofocus: '', name: 'abp-button' } },
      })),
  );

  it('should display the button', () => {
    expect(spectator.query('button')).toBeTruthy();
  });

  it('should equal the default classes to btn btn-primary', () => {
    expect(spectator.query('button')).toHaveClass('btn btn-primary');
  });

  it('should equal the default type to button', () => {
    expect(spectator.query('button')).toHaveAttribute('type', 'button');
  });

  it('should enabled', () => {
    expect(spectator.query('[disabled]')).toBeFalsy();
  });

  it('should have the text content', () => {
    expect(spectator.query('button')).toHaveText('Button');
  });

  it('should display the icon', () => {
    expect(spectator.query('i.d-none')).toBeFalsy();
    expect(spectator.query('i')).toHaveClass('fa');
  });

  it('should display the spinner icon', () => {
    spectator.component.loading = true;
    spectator.detectComponentChanges();
    expect(spectator.query('i')).toHaveClass('fa-spinner');
  });

  it('should disabled when the loading input is true', () => {
    spectator.component.loading = true;
    spectator.detectComponentChanges();
    expect(spectator.query('[disabled]')).toBeTruthy();
  });

  it('should disabled when the loading input is true', () => {
    expect(spectator.query('[autofocus][name="abp-button"]')).toBeTruthy();
  });
});
