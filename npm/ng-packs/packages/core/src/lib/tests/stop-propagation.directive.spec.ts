import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/jest';
import { ClickEventStopPropagationDirective } from '../directives/stop-propagation.directive';

describe('ClickEventStopPropagationDirective', () => {
  let spectator: SpectatorDirective<ClickEventStopPropagationDirective>;
  let directive: ClickEventStopPropagationDirective;
  let link: HTMLAnchorElement;
  let childClickEventFn = jest.fn(() => null);
  let parentClickEventFn = jest.fn(() => null);
  const createDirective = createDirectiveFactory({
    directive: ClickEventStopPropagationDirective,
  });

  beforeEach(() => {
    spectator = createDirective(
      '<div (click)="parentClickEventFn()"><a (click.stop)="childClickEventFn()" >Link</a></div>',
      {
        hostProps: { parentClickEventFn, childClickEventFn },
      },
    );
    directive = spectator.directive;
    link = spectator.query('a');
    childClickEventFn.mockClear();
    parentClickEventFn.mockClear();
  });

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should not call click event of parent when child element is clicked', done => {
    spectator.setHostInput({ parentClickEventFn, childClickEventFn });
    spectator.click('a');
    spectator.detectChanges();
    expect(childClickEventFn).toHaveBeenCalled();
    expect(parentClickEventFn).not.toHaveBeenCalled();
    done();
  });
});
