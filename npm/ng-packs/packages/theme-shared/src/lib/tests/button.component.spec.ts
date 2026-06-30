import { ɵSIGNAL as SIGNAL } from '@angular/core';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/vitest';
import { ButtonComponent } from '../components';

const setInputSignal = <T>(inputSignal: () => T, value: T) => {
  const node = inputSignal[SIGNAL];
  node.applyValueToInputSignal(node, value);
};

describe('ButtonComponent', () => {
  let spectator: SpectatorHost<ButtonComponent>;

  const createHost = createHostFactory(ButtonComponent);

  beforeEach(
    () => {
      spectator = createHost('<abp-button>Button</abp-button>', {
        detectChanges: false,
      });
      setInputSignal(spectator.component.iconClass, 'fa fa-check');
      spectator.detectChanges();
    },
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
    spectator.component.setLoading(true);
    spectator.detectComponentChanges();
    expect(spectator.query('i')).toHaveClass('fa-spinner');
  });

  it('should display the spinner icon when loading input is true', () => {
    setInputSignal(spectator.component.loading, true);
    spectator.detectComponentChanges();
    expect(spectator.query('i')).toHaveClass('fa-spinner');
  });

  it('should clear the spinner icon when loading input becomes false', () => {
    setInputSignal(spectator.component.loading, true);
    spectator.detectComponentChanges();
    expect(spectator.query('i')).toHaveClass('fa-spinner');

    setInputSignal(spectator.component.loading, false);
    spectator.detectComponentChanges();
    expect(spectator.query('i')).toHaveClass('fa-check');
    expect(spectator.query('i')).not.toHaveClass('fa-spinner');
  });

  it('should disabled when the loading input is true', () => {
    spectator.component.setLoading(true);
    spectator.detectComponentChanges();
    expect(spectator.query('[disabled]')).toBeTruthy();
  });
});
