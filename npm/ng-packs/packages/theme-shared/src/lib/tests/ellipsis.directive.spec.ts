import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { EllipsisDirective } from '../directives/ellipsis.directive';
import { setInputSignal } from './utils';

describe('EllipsisDirective', () => {
  let spectator: SpectatorDirective<EllipsisDirective>;
  let directive: EllipsisDirective;
  let el: HTMLDivElement;
  const createDirective = createDirectiveFactory({
    directive: EllipsisDirective,
  });

  beforeEach(() => {
    spectator = createDirective(
      '<div [abpEllipsis]="width" [abpEllipsisEnabled]="true" [title]="title">test content</div>',
      {
        detectChanges: false,
      },
    );
    setInputSignal(spectator.directive.width, '100px');
    setInputSignal(spectator.directive.enabled, true);
    setInputSignal(spectator.directive.title, 'title');
    spectator.detectChanges();
    directive = spectator.directive;
    el = spectator.query('div');
  });

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should have 100px ellipsis width', () => {
    expect(directive.width()).toBe('100px');
  });

  test('should be enabled if abpEllipsisEnabled input is true', () => {
    expect(directive.enabled()).toBe(true);
  });

  test('should have given title', () => {
    expect(directive.title()).toBe('title');
  });

  test('should add abp-ellipsis-inline class to element if width is given', () => {
    expect(el).toHaveClass('abp-ellipsis-inline');
  });
});

describe('EllipsisDirective when title is not specified', () => {
  let spectator: SpectatorDirective<EllipsisDirective>;
  let directive: EllipsisDirective;
  let el: HTMLDivElement;

  const createDirective = createDirectiveFactory({
    directive: EllipsisDirective,
  });

  beforeEach(() => {
    spectator = createDirective(
      '<div [abpEllipsis]="width" [abpEllipsisEnabled]="true" [title]="title">test content</div>',
      {
        hostProps: {
          title: undefined,
          width: '100px',
        },
      },
    );
    setInputSignal(spectator.directive.width, '100px');
    setInputSignal(spectator.directive.enabled, true);
    setInputSignal(spectator.directive.title, undefined);
    spectator.detectChanges();
    directive = spectator.directive;
    el = spectator.query('div') as HTMLDivElement;
  });

  test('should have element innerText as title', () => {
    expect(directive.title()).toBe(el.innerText);
  });
});

describe('EllipsisDirective when width is not given', () => {
  let spectator: SpectatorDirective<EllipsisDirective>;
  let directive: EllipsisDirective;
  let el: HTMLDivElement;
  const createDirective = createDirectiveFactory({
    directive: EllipsisDirective,
  });

  beforeEach(() => {
    spectator = createDirective(
      '<div [abpEllipsis]="width" [abpEllipsisEnabled]="true" [title]="title">test content</div>',
      {
        hostProps: {
          title: 'test title',
          width: undefined,
        },
      },
    );
    setInputSignal(spectator.directive.width, undefined);
    setInputSignal(spectator.directive.enabled, true);
    setInputSignal(spectator.directive.title, 'test title');
    spectator.detectChanges();
    directive = spectator.directive;
    el = spectator.query('div') as HTMLDivElement;
  });

  test('should add abp-ellipsis class to element', () => {
    expect(el).toHaveClass('abp-ellipsis');
  });
});
