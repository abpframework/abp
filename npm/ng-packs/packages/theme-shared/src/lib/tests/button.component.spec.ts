import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { ButtonComponent } from '../components';

describe('ButtonComponent', () => {
  let host: SpectatorHost<ButtonComponent>;

  const createHost = createHostFactory(ButtonComponent);

  beforeEach(() => (host = createHost('<abp-button iconClass="fa fa-check">Button</abp-button>')));

  it('should display the button', () => {
    expect(host.query('button')).toBeTruthy();
  });

  it('should equal the default classes to btn btn-primary', () => {
    expect(host.query('button')).toHaveClass('btn btn-primary');
  });

  it('should equal the default type to button', () => {
    expect(host.query('button')).toHaveAttribute('type', 'button');
  });

  it('should enabled', () => {
    expect(host.query('[disabled]')).toBeFalsy();
  });

  it('should have the text content', () => {
    expect(host.query('button')).toHaveText('Button');
  });

  it('should display the icon', () => {
    expect(host.query('i.d-none')).toBeFalsy();
    expect(host.query('i')).toHaveClass('fa');
  });

  it('should display the spinner icon', () => {
    host.component.loading = true;
    host.detectComponentChanges();
    expect(host.query('i')).toHaveClass('fa-spinner');
  });

  it('should disabled when the loading input is true', () => {
    host.component.loading = true;
    host.detectComponentChanges();
    expect(host.query('[disabled]')).toBeDefined();
  });
});
