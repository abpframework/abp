import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/jest';
import { StopPropagationDirective } from '../directives/stop-propagation.directive';

describe('StopPropagationDirective', () => {
  let spectator: SpectatorDirective<StopPropagationDirective>;
  let directive: StopPropagationDirective;
  let link: HTMLAnchorElement;
  const childClickEventFn = jest.fn(() => null);
  const parentClickEventFn = jest.fn(() => null);
  const createDirective = createDirectiveFactory({
    directive: StopPropagationDirective,
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
