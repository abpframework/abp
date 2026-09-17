import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { StopPropagationDirective } from '../directives/stop-propagation.directive';
import { beforeEach, describe, expect, test, vi } from 'vitest';

describe('StopPropagationDirective', () => {
  let spectator: SpectatorDirective<StopPropagationDirective>;
  let directive: StopPropagationDirective;
  let link: HTMLAnchorElement;
  const childClickEventFn = vi.fn(() => null);
  const parentClickEventFn = vi.fn(() => null);
  const createDirective = createDirectiveFactory({
    directive: StopPropagationDirective,
  });

  beforeEach(() => {
    spectator = createDirective(
      '<div (click)="parentClickEventFn()"><a click.stop>Link</a></div>',
      {
        hostProps: { parentClickEventFn },
      },
    );
    directive = spectator.directive;
    directive.stopPropEvent.subscribe(childClickEventFn);
    link = spectator.query('a');
    childClickEventFn.mockClear();
    parentClickEventFn.mockClear();
  });

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should not call click event of parent when child element is clicked', () => {
    spectator.click(link);
    spectator.detectChanges();
    expect(childClickEventFn).toHaveBeenCalled();
    expect(parentClickEventFn).not.toHaveBeenCalled();
  });
});
